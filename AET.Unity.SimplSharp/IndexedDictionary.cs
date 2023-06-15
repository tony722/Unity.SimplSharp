using System;
using System.Collections;
using System.Collections.Generic;

namespace AET.Unity.SimplSharp {
  public class IndexedDictionary<TKey, TValue> : IEnumerable<TValue> {
    private readonly List<TValue> list = new List<TValue>();
    private readonly Dictionary<TKey, TValue> dict;

    private readonly object mutex = new object();
    public IndexedDictionary() { 
      dict = new Dictionary<TKey, TValue>();
    }
    public IndexedDictionary(IEqualityComparer<TKey> comparer) {
      dict = new Dictionary<TKey, TValue>(comparer);
    }
    public TValue this[int index] {
      get { return list[index]; }
    }

    public TValue this[TKey key] {
      get { return dict[key]; }
    }

    public Dictionary<TKey, TValue>.KeyCollection Keys {
      get { return dict.Keys; }
    }

    public int IndexOf(TValue item) {
      return list.IndexOf(item);
    }

    public int IndexOfKey(TKey key) {
      return list.IndexOf(dict[key]);
    }

    public bool ContainsKey(TKey key) {
      return dict.ContainsKey(key);
    }

    public bool TryGetValue(TKey key, out TValue value) {
      return dict.TryGetValue(key, out value);
    }

    public int Count {
      get { return list.Count; }
    }

    public virtual void Clear() {
      list.Clear();
      dict.Clear();
    }

    public virtual void Add(TKey key, TValue value) {
      lock (mutex) {
        list.Add(value);
        dict.Add(key, value);
      }
    }

    public virtual TValue Remove(TKey key) {
      var value = dict[key];
      dict.Remove(key);
      list.Remove(value);
      return value;
    }

    protected IEnumerable<KeyValuePair<TKey, TValue>> DictionaryItems {
      get { return dict; }
    }

    public IEnumerable<TValue> Values {
      get { return list; }
    }

    IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() {
      return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    protected IEnumerator<TValue> GetEnumerator() {
      return list.GetEnumerator();
    }
  }
}