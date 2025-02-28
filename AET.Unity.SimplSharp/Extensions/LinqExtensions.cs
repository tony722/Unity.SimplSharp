using System;
using System.Collections.Generic;
using System.Linq;
using AET.Unity.SimplSharp.Concurrent;

namespace AET.Unity.SimplSharp {
  public static class LinqExtensions {
    public static IEnumerable<T> Insert<T>(
        this IEnumerable<T> values, T value) {
      yield return value;
      foreach (var item in values) {
        yield return item;
      }
    }

    public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source) {
      using (var e = source.GetEnumerator()) {
        if (e.MoveNext()) {
          for (var value = e.Current; e.MoveNext(); value = e.Current) {
            yield return value;
          }
        }
      }
    }

    public static IEnumerable<V> GetValues<K, V>(this IDictionary<K, V> dict, IEnumerable<K> keys) {
      return keys.Select((x) => dict[x]);
    }

    public static IEnumerable<V> GetValues<K, V>(this AnyKeyConcurrentDictionary<K, V> dict, IEnumerable<K> keys) where V : new() {
      return keys.Select((x) => dict[x]);
    }

    /// <summary>
    /// ToDictionary method that allows duplicate keys (uses last item if duplicates exist)
    /// </summary>
    public static Dictionary<TKey, TElement> SafeToDictionary<TSource, TKey, TElement>(
      this IEnumerable<TSource> source, 
      Func<TSource, TKey> keySelector, 
      Func<TSource, TElement> elementSelector, 
      IEqualityComparer<TKey> comparer)
    {
      var dictionary = new Dictionary<TKey, TElement>(comparer);

      if (source == null)
      {
        return dictionary;
      }

      foreach (var element in source)
      {
        dictionary[keySelector(element)] = elementSelector(element);
      }

      return dictionary; 
    }

    public static ConcurrentList<T> ToConcurrentList<T>(this IEnumerable<T> collection) {
      return new ConcurrentList<T>(collection);
    }
  }
}
