public class Node<T> {
    public T Value { get; set; }
    public Node<T> Next { get; set; }
    public Node(T value, Node<T> next = null) {
        Value = value;
        Next = next;
    }
}

public static class FloydsCycleFinding {
    public static Node<T> GetMeetingPoint<T>(Node<T> head) {
        if (head == null || head.Next == null) return null;
        Node<T> tortoise = head;
        Node<T> hare = head;
        while (hare != null && hare.Next != null) {
            tortoise = tortoise.Next;
            hare = hare.Next.Next;
            if (tortoise == hare) {
                return tortoise;
            }
        }
        return null;
    }

    public static bool HasCycle<T>(Node<T> head) {
        return GetMeetingPoint(head) != null;
    }

    public static Node<T> FindCycleStart<T>(Node<T> head) {
        Node<T> meetingPoint = GetMeetingPoint(head);
        if (meetingPoint == null) return null;
        Node<T> ptr1 = head;
        Node<T> ptr2 = meetingPoint;
        while (ptr1 != ptr2) {
            ptr1 = ptr1.Next;
            ptr2 = ptr2.Next;
        }
        return ptr1;
    }

    public static int FindCycleLength<T>(Node<T> head) {
        Node<T> meetingPoint = GetMeetingPoint(head);
        if (meetingPoint == null) return 0;
        int length = 1;
        Node<T> current = meetingPoint;
        while (current.Next != meetingPoint) {
            current = current.Next;
            length++;
        }
        return length;
    }
}