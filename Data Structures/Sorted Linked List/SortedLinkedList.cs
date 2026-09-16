public class SortedLinkedList {
    private class Node {
        public int Data;
        public Node Next;
        public Node(int data) {
            Data = data;
            Next = null;
        }
    }

    private Node head;

    public SortedLinkedList() {
        head = null;
    }

    public void Insert(int data) {
        Node newNode = new Node(data);
        if (head == null || head.Data >= newNode.Data) {
            newNode.Next = head;
            head = newNode;
        } else {
            Node current = head;
            while (current.Next != null && current.Next.Data < newNode.Data) {
                current = current.Next;
            }
            newNode.Next = current.Next;
            current.Next = newNode;
        }
    }

    public void Remove(int data) {
        if (head == null) return;
        if (head.Data == data) {
            head = head.Next;
            return;
        }
        Node current = head;
        while (current.Next != null && current.Next.Data != data) {
            current = current.Next;
        }
        if (current.Next != null) {
            current.Next = current.Next.Next;
        }
    }

    public bool Search(int data) {
        Node current = head;
        while (current != null) {
            if (current.Data == data) return true;
            current = current.Next;
        }
        return false;
    }

    public void Print() {
        Node current = head;
        while (current != null) {
            System.Console.Write(current.Data + " ");
            current = current.Next;
        }
        System.Console.WriteLine();
    }
}