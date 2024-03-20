using System.Collections.Generic;

namespace AET.Unity.SimplSharp.Concurrent {
  public class ConcurrentQueue<T> {
    private readonly Queue<T> queue = new Queue<T>();
    private readonly CrestronMutex mutex = new CrestronMutex();

    public void Enqueue(T value) { mutex.ThreadsafeExecute(() => queue.Enqueue(value)); }
    public T Dequeue() { return mutex.ThreadsafeExecute(() => queue.Dequeue()); }
    public T Peek() { return mutex.ThreadsafeExecute(() => queue.Peek()); }
    
    public int Count { get { return queue.Count; } }
  }
}
