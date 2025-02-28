using System;
using System.Collections;
using System.Collections.Generic;

namespace AET.Unity.SimplSharp.Concurrent {
  public class ConcurrentList<T> : IList<T> {
    private readonly List<T> list;
    private IMutex mutex;

    public ConcurrentList() {
      list = new List<T>();
    }

    public ConcurrentList(IEnumerable<T> collection) {
      list = new List<T>(collection);
    }

    public IMutex Mutex {
      get { return mutex ?? (mutex = new CrestronMutex()); }
      set { mutex = value; }
    }


    public IEnumerator<T> GetEnumerator() {
      return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    private void ThreadsafeExecute(Action action) {
      Mutex.Enter();
      try {
        action();
      }
      finally {
        Mutex.Exit();
      }
    }

    public virtual void Add(T item) {
      ThreadsafeExecute(() => list.Add(item));
    }

    public virtual void Clear() {
      ThreadsafeExecute(() => list.Clear());
    }

    public bool Contains(T item) {
      return list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex) {
      ThreadsafeExecute(() => list.CopyTo(array, arrayIndex));
    }

    public virtual bool Remove(T item) {
      Mutex.Enter();
      try {
        return list.Remove(item);
      }
      finally {
        Mutex.Exit();
      }
    }

    public int Count {
      get { return list.Count; }
    }
    public bool IsReadOnly {
      get { return false; }
    }
    public int IndexOf(T item) {
      return list.IndexOf(item);
    }

    public virtual void Insert(int index, T item) {
      ThreadsafeExecute(() => list.Insert(index, item));
    }

    public virtual void RemoveAt(int index) {
      ThreadsafeExecute(() => list.RemoveAt(index));
    }

    public T this[int index] {
      get { return list[index]; }
      set { ThreadsafeExecute(() => list[index] = value); }
    }
  }
}
