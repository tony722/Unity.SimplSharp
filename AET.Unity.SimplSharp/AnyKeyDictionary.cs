using System.Collections.Generic;

namespace AET.Unity.SimplSharp {
  /// <summary>
  /// A dictionary that does not throw KeyNotFoundExceptions or ArgumentExceptions (Item with the same key has already been added).
  /// Instead if you try to retrieve an item that has no key, it logs an error message and 
  /// </summary>
  /// <typeparam name="TKey"></typeparam>
  /// <typeparam name="TValue"></typeparam>
  public class AnyIndexDictionary<TKey, TValue> {
    private readonly Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

    public AnyIndexDictionary() { }

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
      }
    }

    public int Count {
      get { return dict.Keys.Count; }
    }
  }
}