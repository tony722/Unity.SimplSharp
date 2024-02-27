using System;
using System.Collections;
using System.Collections.Generic;
using AET.Unity.SimplSharp.Concurrent;

namespace AET.Unity.SimplSharp {
  /// <summary>
  /// A dictionary that does not throw KeyNotFoundExceptions or ArgumentExceptions (Item with the same key has already been added).
  /// Instead if you try to retrieve an item that has no key, it logs an error message and returns new(t)
  /// </summary>
  /// <typeparam name="TKey"></typeparam>
  /// <typeparam name="TValue"></typeparam>
  public class AnyKeyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> {
    private readonly ConcurrentDictionary<TKey, TValue> dict = new ConcurrentDictionary<TKey, TValue>();

    public AnyKeyDictionary() {
      ValueFactory = () => default(TValue);
    }

    public event EventHandler<SetValueEventArgs> OnSetValue;
    
    public AnyKeyDictionary(IList<TKey> keys, IList<TValue> values) {
      for (var i = 0; i < keys.Count; i++) {
        this[keys[i]] = values[i];
      }
    }

    public void Clear() {
      dict.Clear();
    }

    public Func<TValue> ValueFactory { private get; set; }
    public bool EnableMissingItemNotice { get; set; }
    public virtual TValue this[TKey key] {
      get {
        TValue value;
        if (dict.TryGetValue(key, out value)) return value;
        if(EnableMissingItemNotice) ErrorMessage.Notice("Tried to request key {0} that does not exist: returned new {1}.", key, typeof(TValue).Name);
        value = ValueFactory != null ? ValueFactory() : default(TValue);
        dict.Add(key, value);
        return value;
      }
      set {
        dict[key] = value;
        if (OnSetValue != null) OnSetValue(this, new SetValueEventArgs(key, value));
      }
    }
    
    public void Remove(TKey id) {
      dict.Remove(id);
    }

    public int Count {
      get { return dict.Keys.Count; }
    }

    public ICollection<TValue> Values {
      get { return dict.Values; }
    }

    public ICollection<TKey> Keys {
      get { return dict.Keys; }
    }


    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
      return dict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public class SetValueEventArgs : EventArgs {
      public SetValueEventArgs(TKey key, TValue value) {
        Key = key;
        Value = value;
      }

      public TKey Key { get; set; }
      public TValue Value { get; set; }
    }
  }
}