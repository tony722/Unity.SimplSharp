using System;
using System.Collections.Generic;
using System.Linq;

namespace Unity.SimplSharp {

  /// <summary>
  /// With this dictionary, both the key and the value are unique.
  /// </summary>
  public class UniqueValueIndexedDictionary<TKey, TValue> : IndexedDictionary<TKey, TValue> {
    private static readonly Object _mutex = new Object();

    private readonly HashCollection<TValue> uniqueValues = new HashCollection<TValue>();

    public override void Add(TKey key, TValue value) {
      lock (_mutex) {
        uniqueValues.Add(value);
        base.Add(key, value);
      }
    }

    public override TValue Remove(TKey key) {      
      var value = base.Remove(key);
      uniqueValues.Remove(value);
      return value;
    }

    public virtual void RemoveValue(TValue value) {
      var i = DictionaryItems.First(kvp => EqualityComparer<TValue>.Default.Equals(kvp.Value, value));
      Remove(i.Key);
    }
  }
}
