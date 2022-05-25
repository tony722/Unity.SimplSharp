﻿using System;
using System.Collections;
using System.Collections.Generic;
using AET.Unity.SimplSharp.Concurrent;

namespace AET.Unity.SimplSharp {
  /// <summary>
  /// A dictionary that does not throw KeyNotFoundExceptions or ArgumentExceptions (Item with the same key has already been added).
  /// Instead if you try to retrieve an item that has no key, it logs an error message and returns default(t)
  /// </summary>
  /// <typeparam name="TKey"></typeparam>
  /// <typeparam name="TValue"></typeparam>
  public class AnyIndexDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> {
    private readonly ConcurrentDictionary<TKey, TValue> dict = new ConcurrentDictionary<TKey, TValue>();

    public AnyIndexDictionary() { }

    public event EventHandler<SetValueEventArgs> OnSetValue;
    
    public AnyIndexDictionary(IList<TKey> keys, IList<TValue> values) {
      for (var i = 0; i < keys.Count; i++) {
        this[keys[i]] = values[i];
      }
    }

    public TValue this[TKey key] {
      get {
        TValue value;
        if (dict.TryGetValue(key, out value)) {
          return value;
        }
        else {
          ErrorMessage.Notice("Tried to request key {0} that does not exist: returned default.", key);
          return default(TValue);
        }
      }
      set {
        if (dict.ContainsKey(key)) {
          dict[key] = value;
        }
        else {
          dict.Add(key, value);
        }

        if (OnSetValue != null) OnSetValue(this, new SetValueEventArgs(key, value));
      }
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