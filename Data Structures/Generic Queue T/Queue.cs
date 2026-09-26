public class Queue<T>
{
    private class Node<U>
    {
        public U Value;
        public Node<U> Next;
        public Node(U value)
        {
            Value = value;
            Next = null;
        }
    }

    private Node<T> head;
    private Node<T> tail;
    private int _count;

    public Queue()
    {
        head = null;
        tail = null;
        _count = 0;
    }

    public void Enqueue(T item)
    {
        Node<T> newNode = new Node<T>(item);
        if (tail != null)
        {
            tail.Next = newNode;
        }
        tail = newNode;
        if (head == null)
        {
            head = tail;
        }
        _count++;
    }

    public T Dequeue()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty.");
        }
        T value = head.Value;
        head = head.Next;
        if (head == null)
        {
            tail = null;
        }
        _count--;
        return value;
    }

    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty.");
        }
        return head.Value;
    }

    public int Count
    {
        get { return _count; }
    }

    public bool IsEmpty
    {
        get { return _count == 0; }
    }
}