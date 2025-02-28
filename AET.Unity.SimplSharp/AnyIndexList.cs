using System;
using System.Collections.Generic;

namespace AET.Unity.SimplSharp {
  public class AnyIndexList<T> : List<T> {
    private Func<int, T> valueFactory = index => default(T);

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1" /> class that is empty and has the default initial capacity.</summary>
    public AnyIndexList() { }

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1" /> class that is empty and has the specified initial capacity.</summary>
    /// <param name="capacity">The number of elements that the new list can initially store.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="capacity" /> is less than 0.</exception>
    public AnyIndexList(int capacity) : base(capacity) { }

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1" /> class that is empty and has the specified initial capacity.</summary>
    /// <param name="capacity">The number of elements that the new list can initially store.</param>
    /// <param name="valueFactory">A function that creates values to populate the newly created list with.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="capacity" /> is less than 0.</exception>
    public AnyIndexList(int capacity, Func<int, T> valueFactory) {
      ValueFactory = valueFactory;
      EnsureListHasCapacityForIndex(capacity);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1" /> class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.</summary>
    /// <param name="collection">The collection whose elements are copied to the new list.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="collection" /> is <see langword="null" />.</exception>
    public AnyIndexList(IEnumerable<T> collection) : base(collection) { }

    /// <summary>
    /// When requesting a list index that does not exist, AnyIndexList uses default(T) to create new items up to the requested index.
    /// This allows you to create a function to create each new item.
    /// <param name="arg">The index number of the item being created.</param>
    /// </summary>
    public Func<int, T> ValueFactory { get { return valueFactory; } set { valueFactory = value; } }

    public new T this[int index] {
      get {
        EnsureListHasCapacityForIndex(index);
        return base[index];
      }
      set {
        EnsureListHasCapacityForIndex(index);
        base[index] = value;
      }
    }

    private void EnsureListHasCapacityForIndex(int index) {
      while(Count <= index) Add(ValueFactory(index));
    }
  }
}
