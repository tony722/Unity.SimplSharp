using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AET.Unity.SimplSharp {
  /// <summary>
  /// Base class solely for use by generic ConfigCollection{T}
  /// </summary>
  public abstract class ConfigCollectionItem {
    public int Id { get; set; }
  }

  /// <summary>
  /// Provides a collection for configuration classes to derive from.
  /// 
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class ConfigCollection<T>  where T : ConfigCollectionItem {
    private readonly Dictionary<int, T> roomsById = new Dictionary<int, T>();
    public int Count {
      get { return roomsById.Count; }
    }

    public T this[int id] {
      get {
        T item;
        if (roomsById.TryGetValue(id, out item)) return item;
        ErrorMessage.Error("Tried to read {0} config with Id {1} which has not been loaded.", typeof(T).Name, id);
        return null;
      }
      set {
        if (roomsById.ContainsKey(id)) {
          roomsById[id] = value;
        } else {
          roomsById.Add(id, value);
          value.Id = id;
        }
      }
    }
  }
}
