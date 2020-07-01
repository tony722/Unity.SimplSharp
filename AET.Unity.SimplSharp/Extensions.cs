using System;
using System.Data.SqlTypes;
using System.Text;
using Crestron.SimplSharp;


namespace AET.Unity.SimplSharp {
  public static class Extensions {
    /// <summary>
    /// Parses a string to an Int. Returns 0 if anything goes wrong (i.e. null, not integer, etc).
    /// </summary>
    public static double SafeParseDouble(this string value) {
      if (value == null) return 0;
      double valueDouble;
      var valueBytes = Encoding.ASCII.GetBytes(value.Trim());
      if (Microsoft.Xml.XmlConverter.TryParseDouble(valueBytes, 0, valueBytes.Length, out valueDouble)) return valueDouble;
      return 0;
    }
    /// <summary>
        /// Parses a string to an Int. Returns 0 if anything goes wrong (i.e. null, not integer, etc).
        /// </summary>
    public static int SafeParseInt(this string value) {
      return Convert.ToInt32(Math.Round(SafeParseDouble(value)));
    }

    /// <summary>
    /// Parses a string to a ushort. Returns 0 if anything goes wrong (i.e. null, not integer, etc).
    /// </summary>
    public static ushort SafeParseUshort(this string value) {
      return Convert.ToUInt16(Math.Round(SafeParseDouble(value)));
    }

    /// <summary>
    /// Parses a string to DateTime. Returns DateTime.MinValue if anything goes wrong.
    /// </summary>

    public static DateTime SafeParseDateTime(this string value) {
      if (String.IsNullOrEmpty(value)) return new DateTime();
      try {
        return DateTime.Parse(value);
      } catch {
        return DateTime.MinValue;
      }
    }

    public static T SafeParseEnum<T>(this string value) where T : struct, IConvertible { // constraint "struct, IConvertible" because System.Enum is not allowed until C# 7
      if (String.IsNullOrEmpty(value)) {
        ErrorMessage.Warn("Tried to parse Enum '{0}' from a null/empty value.", typeof(T).Name);
        return default(T);
      }
      try {
        return (T)Enum.Parse(typeof(T), value, true);
      } catch {
        ErrorMessage.Warn("Tried to parse Enum '{0}', but value '{1}' not found.", typeof(T).Name, value);
        return default(T);
      }
    }

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
        default: return false;
      }
    }

    /// <summary>
    /// Converts value from 0-100 to 0-65535
    /// </summary>
    public static int ConvertHundredBaseTo16Bit(this int value) {
      return (value * 0xffff) / 100;
    }

    /// <summary>
    /// Converts an int to a bool. Returns false if 0. Returns true if > zero.
    /// </summary>
    public static bool ToBool(this int value) {
      return value > 0;
    }

    public static bool ToBool(this ushort value) {
      return value > 0;
    }

    /// <summary>
    /// Converts a bool to a ushort.
    /// </summary>
    public static ushort ToUshort(this bool value) {
      return (ushort)(value ? 1 : 0);
    }

    /// <summary>
    /// True if string contains only whitespace characters, or is null
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string value) {
      if (value == null) return true;
      for (int index = 0; index < value.Length; ++index) {
        if (!char.IsWhiteSpace(value[index]))
          return false;
      }
      return true;
    }
  }
}