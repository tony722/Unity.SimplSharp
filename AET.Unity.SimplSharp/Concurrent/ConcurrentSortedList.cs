using System;
using System.Collections.Generic;
using System.Linq;


namespace AET.Unity.SimplSharp.Concurrent {
  public class ConcurrentSortedList<TKey, TValue> : IEnumerable<TValue> {
    private readonly SortedList<TKey, TValue> list = new SortedList<TKey, TValue>();
    private IMutex mutex;


    public IMutex Mutex {
      get { return mutex ?? (mutex = new CrestronMutex()); }
      set { mutex = value; }
    }

    private void ThreadsafeExecute(Action action) {
      Mutex.Enter();
      try {
        action();
      } finally {
        Mutex.Exit();
      }
    }

    public void Add(KeyValuePair<TKey, TValue> item) {
      Add(item.Key, item.Value);
    }

    public void Add(TKey key, TValue value) {
      ThreadsafeExecute(() => list.Add(key, value));
    }

    public void Clear() {
      ThreadsafeExecute(() => list.Clear());
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) {
      return list.Contains(item);
    }
    public bool ContainsKey(TKey key) {
      return list.ContainsKey(key);
    }

    public int Count {
      get { return list.Count; }
    }

    public bool Remove(TKey key) {
      Mutex.Enter();
      try {
        return list.Remove(key);
      } finally { Mutex.Exit(); }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) {
      return Remove(item.Key);
    }

    public bool TryGetValue(TKey key, out TValue value) {
      return list.TryGetValue(key, out value);
    }

    public virtual TValue this[TKey key] {
      get { return list[key]; }
      set { ThreadsafeExecute(() => list[key] = value); }
    }

    public IEnumerable<TKey> Keys {
      get { return list.Keys; }
    }

    public IEnumerable<TValue> Values {
      get { return list.Values; }
    }

    IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() {
      return list.Values.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return list.Values.GetEnumerator();
    }
  }
}
