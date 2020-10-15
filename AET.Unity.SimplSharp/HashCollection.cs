using System.Collections.Generic;

namespace AET.Unity.SimplSharp {
  //
  //This class should be replaced by HashSet<T> if Crestron upgrades to a later Framework that supports it
  //
  public class HashCollection<T> : ICollection<T> {
    private readonly Dictionary<T, bool> dict;
    private readonly List<T> list;

    public HashCollection() {
      dict = new Dictionary<T, bool>();
      list = new List<T>();
    }

    void ICollection<T>.Add(T item) {
      dict.Add(item, false);
      list.Add(item);
    }


    public void Add(T item) {
      dict.Add(item, false);
      list.Add(item);
    }

    public void Clear() {
      dict.Clear();
      list.Clear();
    }

    public int IndexOf(T item) {
      return list.IndexOf(item);
    }

    public bool Contains(T item) {
      return dict.ContainsKey(item);
    }

    public void CopyTo(T[] array, int arrayIndex) {
      dict.Keys.CopyTo(array, arrayIndex);
    }

    public int Count {
      get { return list.Count; }
    }

    public T this[int index] {
      get { return list[index]; }
    }

    public bool IsReadOnly {
      get { return false; }
    }

    public bool Remove(T item) {
      list.Remove(item);
      return dict.Remove(item);
    }

    public IEnumerator<T> GetEnumerator() {
      return list.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}