using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AET.Unity.SimplSharp {
  /// <summary>
  /// Base class for objects used by generic IdCollection{T}
  /// </summary>
  public abstract class IdCollectionItem {
    public int Id { get; set; }
  }

  /// <summary>
  /// A collection of items exposed by Id.
  /// If Id is not found, it does not throw an error, just logs a message and returns null.
  /// If an Id exists when setting, it simply replaces the existing item in the collection with that Id.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class IdCollection<T> : IEnumerable<T> where T : IdCollectionItem {
    private readonly Dictionary<int, T> itemsById = new Dictionary<int, T>();
    public int Count {
      get { return itemsById.Count; }
    }

    public void Clear() {
      itemsById.Clear();
    }

    public T this[int id] {
      get {
        T item;
        if (itemsById.TryGetValue(id, out item)) return item;
        ErrorMessage.Error("Tried to read {0} config with Id {1} which has not been loaded.", typeof(T).Name, id);
        return null;
      }
      set {
        if (itemsById.ContainsKey(id)) {
          itemsById[id] = value;
        } else {
          itemsById.Add(id, value);
          value.Id = id;
        }
      }
    }

    public IEnumerator<T> GetEnumerator() {
      return itemsById.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}
