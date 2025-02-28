using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp.CrestronXmlLinq;

namespace AET.Unity.SimplSharp {
  public static class ParsingAndConversionExtensions {
    /// <summary>
    /// Parses a string to an Int. Returns 0 if anything goes wrong (i.e. null, not integer, etc).
    /// </summary>
    public static double SafeParseDouble(this string value) {
      if (value == null) return 0;
      double valueDouble;
      var valueBytes = Encoding.ASCII.GetBytes(value.Trim());
      if (Microsoft.Xml.XmlConverter.TryParseDouble(valueBytes, 0, valueBytes.Length, out valueDouble))
        return valueDouble;
      return 0;
    }

    public static double SafeParseDouble(this XElement element) { return element.Value.SafeParseDouble(); }
    public static double SafeParseDouble(this XAttribute attribute) { return attribute.Value.SafeParseDouble(); }

    /// <summary>
    /// Parses a string to an Int. Returns 0 if anything goes wrong (i.e. null, not integer, etc).
    /// </summary>
    public static int SafeParseInt(this string value) { return Convert.ToInt32(Math.Round(SafeParseDouble(value))); }

    public static int SafeParseInt(this XElement element) { return element.Value.SafeParseInt(); }
    public static int SafeParseInt(this XAttribute attribute) { return attribute.Value.SafeParseInt(); }
    public static int SafeParseInt(this object value) {
      if (value == null) return 0;
      if (value is string) return SafeParseInt((string)value);
      return 0;
    }
    /// <summary>
    /// Parses a string to a ushort. Returns 0 if anything goes wrong (i.e. null, not integer, etc).
    /// </summary>
    public static ushort SafeParseUshort(this string value) { return Convert.ToUInt16(Math.Round(SafeParseDouble(value))); }

    public static ushort SafeParseUshort(this XElement element) { return element.Value.SafeParseUshort(); }
    public static ushort SafeParseUshort(this XAttribute attribute) { return attribute.Value.SafeParseUshort(); }

    /// <summary>
    /// Parses a string to DateTime. Returns DateTime.MinValue if anything goes wrong.
    /// </summary>
    public static DateTime SafeParseDateTime(this string value) {
      if (string.IsNullOrEmpty(value)) return new DateTime();
      try {
        return DateTime.Parse(value);
      } catch {
        return DateTime.MinValue;
      }
    }

    public static DateTime SafeParseDateTime(this XElement element) { return element.Value.SafeParseDateTime(); }
    public static DateTime SafeParseDateTime(this XAttribute attribute) { return attribute.Value.SafeParseDateTime(); }

    public static T SafeParseEnum<T>(this string value) where T : struct, IConvertible {
      // constraint "struct, IConvertible" because System.Enum is not allowed until C# 7
      if (string.IsNullOrEmpty(value)) {
        ErrorMessage.Warn("Tried to parse Enum '{0}' from a null/empty value.", typeof (T).Name);
        return default(T);
      }

      try {
        return (T) Enum.Parse(typeof (T), value, true);
      } catch {
        ErrorMessage.Warn("Tried to parse Enum '{0}', but value '{1}' not found.", typeof (T).Name, value);
        return default(T);
      }
    }

    public static bool SafeParseBool(this ushort value) { return value > 0; }

    public static bool SafeParseBool(this object value) {
      if(value == null) return false;
      if(value is bool) return (bool)value;
      if(value is string) return SafeParseBool((string)value);
      if(value is ushort) return SafeParseBool((ushort)value);
      return false;
    }

    public static bool SafeParseBool(this XElement element) { return element.Value.SafeParseBool(); }
    public static bool SafeParseBool(this XAttribute attribute) { return attribute.Value.SafeParseBool(); }

    public static bool SafeParseBool(this string value) {
      if (value.IsNullOrWhiteSpace()) return false;
      var trimmedValue = value.Trim();
      switch (trimmedValue.ToLower()) {
        case "true":
        case "1":
        case "x":
        case "y":
        case "yes":
        case "on":
          return true;
        default:
          return false;
      }
    }

    /// <summary>
    /// Converts value from 0-100 to 0-65535
    /// </summary>
    public static int ConvertHundredBaseTo16Bit(this int value) { return (value*0xffff)/100; }

    public static ushort ConvertHundredBaseTo16Bit(this ushort value) { return (ushort) ConvertHundredBaseTo16Bit((int) value); }

    public static int Convert16BitToHundredBase(this int value) { return (((value + 1)*100)/65535); }

    public static ushort Convert16BitToHundredBase(this ushort value) { return (ushort) Convert16BitToHundredBase((int) value); }


    /// <summary>
    /// Converts an int to a bool. Returns false if 0. Returns true if > zero.
    /// </summary>
    public static bool ToBool(this int value) { return value > 0; }

    public static bool ToBool(this ushort value) { return value > 0; }

    /// <summary>
    /// Converts a bool to a ushort.
    /// </summary>
    public static ushort ToUshort(this bool value) { return (ushort) (value ? 1 : 0); }

    public static ushort ToUshort(this bool? value) { return ToUshort(value ?? false); }

    /// <summary>
    /// Converts a string to a Version.
    /// </summary>
    public static Version SafeParseVersion(this string version) {
      if (version.IsNullOrWhiteSpace()) return new Version(0, 0, 0);
      var parts = version.MakeVersionParts();
      if (parts.Length == 1) return new Version(parts[0], 0, 0);
      if (parts.Length == 2) return new Version(parts[0], parts[1], 0);
      return new Version(parts[0], parts[1], parts[2]);
    }

    private static int[] MakeVersionParts(this string value) {
      var parts = value.Split('.');
      var intParts = parts.Select(p => p.SafeParseInt()).ToArray();
      return intParts;
    }

    public static IEnumerable<string> SplitLines(this string input) {
      if(input == null) yield break;
      var start = 0;
      for (var i = 0; i < input.Length; i++) {
        if (input[i] == '\r') {
          if (i < input.Length - 1 && input[i + 1] == '\n') {
            yield return input.Substring(start, i - start);
            i++; // Skip over '\n'
          } else {
            yield return input.Substring(start, i - start);
          }
          start = i + 1;
        } else if (input[i] == '\n') {
          yield return input.Substring(start, i - start);
          start = i + 1;
        }
      }
      if (start < input.Length) {
        yield return input.Substring(start);
      }
    }

    public static string Base64Decode(this string encodedValue) {
      if (encodedValue.IsNullOrWhiteSpace()) return string.Empty;
      byte[] decodedBytes = Convert.FromBase64String(encodedValue);
      string decodedString = Encoding.UTF8.GetString(decodedBytes, 0, decodedBytes.Length);
      return decodedString;
    }

    public static string HtmlEncode(this string text) {
      if (string.IsNullOrEmpty(text)) return text;

      StringBuilder encodedText = new StringBuilder(text.Length);
      foreach (char c in text) {
        switch (c) {
          case '<':
            encodedText.Append("&lt;");
            break;
          case '>':
            encodedText.Append("&gt;");
            break;
          case '&':
            encodedText.Append("&amp;");
            break;
          case '"':
            encodedText.Append("&quot;");
            break;
          case '\'':
            encodedText.Append("&#39;");
            break;
          default:
            if (c > 159) {
              encodedText.Append("&#");
              encodedText.Append(((int)c).ToString());
              encodedText.Append(";");
            } else {
              encodedText.Append(c);
            }
            break;
        }
      }
      return encodedText.ToString();
    }

  }
}
