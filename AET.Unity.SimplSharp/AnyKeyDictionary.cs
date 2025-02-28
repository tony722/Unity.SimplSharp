using System;
using System.Collections.Generic;

namespace AET.Unity.SimplSharp {

  /// <summary>
  /// Functions the same as Dictionary except instead of KeyNotFound it creates the key and default value
  /// </summary>
  /// <typeparam name="TKey"></typeparam>
  /// <typeparam name="TValue"></typeparam>
  public class AnyKeyDictionary<TKey, TValue> : Dictionary<TKey, TValue> {
    private Func<TKey, TValue> valueFactory = key => default(TValue) ;

    #region Constructors from Dictionary<>
    /// <summary>
    /// Initializes a new instance of the MyDictionary class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
    /// </summary>
    public AnyKeyDictionary() { }

    /// <summary>
    /// Initializes a new instance of the MyDictionary class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
    /// </summary>
    /// <param name="capacity">The initial number of elements that the MyDictionary can contain.</param>
    public AnyKeyDictionary(int capacity) : base(capacity) { }

    /// <summary>
    /// Initializes a new instance of the MyDictionary class that is empty, has the default initial capacity, and uses the specified IEqualityComparer<T>.
    /// </summary>
    /// <param name="comparer">The IEqualityComparer<T> implementation to use when comparing keys, or null to use the default EqualityComparer<T> for the type of the key.</param>
    public AnyKeyDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }

    /// <summary>
    /// Initializes a new instance of the MyDictionary class that contains elements copied from the specified IDictionary<TKey,TValue> and uses the default equality comparer for the key type.
    /// </summary>
    /// <param name="dictionary">The IDictionary<TKey,TValue> whose elements are copied to the new MyDictionary.</param>
    public AnyKeyDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

    /// <summary>
    /// Initializes a new instance of the MyDictionary class that contains elements copied from the specified IDictionary<TKey,TValue> and uses the specified IEqualityComparer<T>.
    /// </summary>
    /// <param name="dictionary">The IDictionary<TKey,TValue> whose elements are copied to the new MyDictionary.</param>
    /// <param name="comparer">The IEqualityComparer<T> implementation to use when comparing keys, or null to use the default EqualityComparer<T> for the type of the key.</param>
    public AnyKeyDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }
    #endregion

    public new TValue this[TKey key] {
      get {
        TValue value;
        if(TryGetValue(key, out value)) return value;
        return base[key] = ValueFactory(key);
      }
      set { base[key] = value; }
    }

    public Func<TKey, TValue> ValueFactory {
      get { return valueFactory; }
      set { valueFactory = value; }
    }
  }
}
