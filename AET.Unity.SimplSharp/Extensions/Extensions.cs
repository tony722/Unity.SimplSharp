using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AET.Unity.SimplSharp {
  public static class Extensions {
    /// <summary>
    /// True if string contains only whitespace characters, or is null
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string value) {
      if (value == null) return true;
      foreach (var c in value) {
        if (!char.IsWhiteSpace(c))
          return false;
      }

      return true;
    }

    public static IList<string> ParseCsv(this string value) {
      try {
        var parser = new CsvParser();
        return parser.ParseLine(value);
      } catch (Exception ex) {
        ErrorMessage.Warn("Error Unity.SimplSharp ParseCsv({0}): {1}", value, ex.Message);
        return new List<string>();
      }
    }

    /// <summary>
    /// Formats a list of objects as a comma-separated string with "or" before last item
    /// </summary>
    public static string FormatAsList<T>(this T[] value) {
      if (value == null) return null;
      if (value.Length == 1) return string.Format("{0}", value[0]);
      if (value.Length == 2) return string.Format("{0} or {1}", value[0], value[1]);
      var s = "";
      int i;
      for (i = 0; i < value.Length - 1; i++) {
        s += value[i] + ", ";
      }

      s += "or " + value[i];
      return s;
    }

    /// <summary>
    /// Splits a string into an array, trimming each resulting array item
    /// </summary>
    public static string[] SplitAndTrim(this string text, char separator) {
      if (text.IsNullOrWhiteSpace()) {
        return null;
      }

      return text.Split(separator).Select(t => t.Trim()).ToArray();
    }
  }
}