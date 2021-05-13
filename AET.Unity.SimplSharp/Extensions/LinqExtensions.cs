﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AET.Unity.SimplSharp {
  public static class LinqExtensions {
    public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source) {
      using (var e = source.GetEnumerator()) {
        if (e.MoveNext()) {
          for (var value = e.Current; e.MoveNext(); value = e.Current) {
            yield return value;
          }
        }
      }
    }
  }
}
