using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public unsafe class XorLinkedList<T> : IDisposable where T : unmanaged
{
    private struct Node
    {
        public T Value;
        public IntPtr Link;
    }

    private IntPtr _head = IntPtr.Zero;
    private IntPtr _tail = IntPtr.Zero;
    private int _count = 0;

    public int Count => _count;

    public void AddFirst(T value)
    {
        IntPtr newNodePtr = AllocateNode(value, Xor(IntPtr.Zero, _head));

        if (_head != IntPtr.Zero)
        {
            Node* headNode = (Node*)_head;
            IntPtr next = Xor(IntPtr.Zero, headNode->Link);
            headNode->Link = Xor(newNodePtr, next);
        }
        else
        {
            _tail = newNodePtr;
        }

        _head = newNodePtr;
        _count++;
    }

    public void AddLast(T value)
    {
        IntPtr newNodePtr = AllocateNode(value, Xor(_tail, IntPtr.Zero));

        if (_tail != IntPtr.Zero)
        {
            Node* tailNode = (Node*)_tail;
            IntPtr prev = Xor(tailNode->Link, IntPtr.Zero);
            tailNode->Link = Xor(prev, newNodePtr);
        }
        else
        {
            _head = newNodePtr;
        }

        _tail = newNodePtr;
        _count++;
    }

    public bool RemoveFirst()
    {
        if (_head == IntPtr.Zero)
        {
            return false;
        }

        IntPtr nodeToRemove = _head;
        Node* headNode = (Node*)nodeToRemove;
        IntPtr next = Xor(IntPtr.Zero, headNode->Link);

        if (next != IntPtr.Zero)
        {
            Node* nextNode = (Node*)next;
            IntPtr nextNext = Xor(nodeToRemove, nextNode->Link);
            nextNode->Link = Xor(IntPtr.Zero, nextNext);
        }
        else
        {
            _tail = IntPtr.Zero;
        }

        _head = next;
        FreeNode(nodeToRemove);
        _count--;
        return true;
    }

    public bool RemoveLast()
    {
        if (_tail == IntPtr.Zero)
        {
            return false;
        }

        IntPtr nodeToRemove = _tail;
        Node* tailNode = (Node*)nodeToRemove;
        IntPtr prev = Xor(tailNode->Link, IntPtr.Zero);

        if (prev != IntPtr.Zero)
        {
            Node* prevNode = (Node*)prev;
            IntPtr prevPrev = Xor(prevNode->Link, nodeToRemove);
            prevNode->Link = Xor(prevPrev, IntPtr.Zero);
        }
        else
        {
            _head = IntPtr.Zero;
        }

        _tail = prev;
        FreeNode(nodeToRemove);
        _count--;
        return true;
    }

    public IEnumerable<T> ForwardTraversal()
    {
        IntPtr curr = _head;
        IntPtr prev = IntPtr.Zero;

        while (curr != IntPtr.Zero)
        {
            Node* currNode = (Node*)curr;
            yield return currNode->Value;

            IntPtr next = Xor(prev, currNode->Link);
            prev = curr;
            curr = next;
        }
    }

    public IEnumerable<T> BackwardTraversal()
    {
        IntPtr curr = _tail;
        IntPtr next = IntPtr.Zero;

        while (curr != IntPtr.Zero)
        {
            Node* currNode = (Node*)curr;
            yield return currNode->Value;

            IntPtr prev = Xor(currNode->Link, next);
            next = curr;
            curr = prev;
        }
    }

    private IntPtr AllocateNode(T value, IntPtr link)
    {
        IntPtr nodePtr = Marshal.AllocHGlobal(Marshal.SizeOf<Node>());
        Node* node = (Node*)nodePtr;
        node->Value = value;
        node->Link = link;
        return nodePtr;
    }

    private void FreeNode(IntPtr nodePtr)
    {
        Marshal.FreeHGlobal(nodePtr);
    }

    private IntPtr Xor(IntPtr a, IntPtr b)
    {
        return new IntPtr(a.ToInt64() ^ b.ToInt64());
    }

    public void Dispose()
    {
        IntPtr curr = _head;
        IntPtr prev = IntPtr.Zero;

        while (curr != IntPtr.Zero)
        {
            Node* currNode = (Node*)curr;
            IntPtr next = Xor(prev, currNode->Link);
            FreeNode(curr);
            prev = curr;
            curr = next;
        }

        _head = IntPtr.Zero;
        _tail = IntPtr.Zero;
        _count = 0;
        GC.SuppressFinalize(this);
    }

    ~XorLinkedList()
    {
        Dispose();
    }
}