using System;

namespace AET.Unity.SimplSharp {
  public static class Extensions {
    /// <summary>
    /// Parses a string to an Int. Returns 0 if anything goes wrong (i.e. null, not integer, etc).
    /// </summary>
    public static int SafeParseInt(this string value) {
      if(String.IsNullOrEmpty(value)) return 0;
      try {
        return int.Parse(value);
      } catch {
        return 0;
      }
    }

    /// <summary>
    /// Parses a string to a ushort. Returns 0 if anything goes wrong (i.e. null, not integer, etc).
    /// </summary>
    public static ushort SafeParseUshort(this string value) {
      if (String.IsNullOrEmpty(value)) return 0;
      try {
        return ushort.Parse(value);
      } catch {
        return 0;
      }
    }

    /// <summary>
    /// Parses a string to DateTime. Returns DateTime.MinValue if anything goes wrong.
    /// 
    /// </summary>

    public static DateTime SafeParseDateTime(this string value) {
      if (String.IsNullOrEmpty(value)) return new DateTime();
      try {
        return DateTime.Parse(value);
      } catch {
        return DateTime.MinValue;
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
    /// (Backported from .NET Framework 4+)
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