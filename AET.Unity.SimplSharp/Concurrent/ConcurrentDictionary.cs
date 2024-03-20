using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AET.Unity.SimplSharp.Concurrent {
  public class ConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue> {
    private readonly Dictionary<TKey, TValue> dictionary;
    private IMutex mutex;

    public ConcurrentDictionary() {
      dictionary = new Dictionary<TKey, TValue>();
    }
    public ConcurrentDictionary(IEqualityComparer<TKey> comparer) {
      dictionary =  new Dictionary<TKey, TValue>(comparer);
    }


    public IMutex Mutex {
      get { return mutex ?? (mutex = new CrestronMutex()); }
      set { mutex = value; }
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

    public void Add(KeyValuePair<TKey, TValue> item) {
      Add(item.Key, item.Value);
    }

    public void Add(TKey key, TValue value) {
      ThreadsafeExecute(() => dictionary.Add(key, value));
    }

    public void Clear() {
      ThreadsafeExecute(() => dictionary.Clear());
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) {
      return Mutex.ThreadsafeExecute(() => dictionary.Contains(item));
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
      ThreadsafeExecute(() => {
        var dict = (IDictionary) (dictionary);
        dict.CopyTo(array, arrayIndex);
      });
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) {
      return Remove(item.Key);
    }

    public int Count {
      get { return dictionary.Count; }
    }
    public bool IsReadOnly {
      get { return false; }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
      return dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public bool ContainsKey(TKey key) { return Mutex.ThreadsafeExecute(() => dictionary.ContainsKey(key)); }

    public bool Remove(TKey key) {
      Mutex.Enter();
      try {
        return dictionary.Remove(key);
      } finally { Mutex.Exit(); }
    }

    public bool TryGetValue(TKey key, out TValue value) {
      Mutex.Enter();
      var r = dictionary.TryGetValue(key, out value);
      Mutex.Exit();
      return r;
    }

    public virtual TValue this[TKey key] {
      get { return dictionary[key]; }
      set { ThreadsafeExecute(() => dictionary[key] = value); }
    }

    public ICollection<TKey> Keys {
      get { return dictionary.Keys; }
    }
    public ICollection<TValue> Values {
      get {
        return dictionary.Values; 
      }
    }
  }
}
