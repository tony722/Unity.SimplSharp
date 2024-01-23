using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
