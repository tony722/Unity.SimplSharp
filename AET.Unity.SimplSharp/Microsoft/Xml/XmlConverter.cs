using System.Text;

namespace Microsoft.Xml {
  /// <summary>
  /// Imported from .NET Core
  /// </summary>
  internal static class XmlConverter {
    // Licensed to the .NET Foundation under one or more agreements.
    // The .NET Foundation licenses this file to you under the MIT license.
    // See the LICENSE file in the project root for more information.

    // Note: this is only part of the XmlConverter class
    // https://source.dot.net/#System.Private.DataContractSerialization/System/Xml/XmlConverter.cs,9ee8f1cde85f7b1c

    public static bool TryParseDouble(byte[] chars, int offset, int count, out double result) {
      result = 0;
      int offsetMax = offset + count;
      bool negative = false;
      if (offset < offsetMax && chars[offset] == '-') {
        negative = true;
        offset++;
        count--;
      }
      if (count < 1 || count > 10)
        return false;
      int value = 0;
      int ch;
      while (offset < offsetMax) {
        ch = (chars[offset] - '0');
        if (ch == ('.' - '0')) {
          offset++;
          int pow10 = 1;
          while (offset < offsetMax) {
            ch = chars[offset] - '0';
            if (((uint)ch) >= 10)
              return false;
            pow10 *= 10;
            value = value * 10 + ch;
            offset++;
          }
          if (negative)
            result = -(double)value / pow10;
          else
            result = (double)value / pow10;
          return true;
        } else if (((uint)ch) >= 10)
          return false;
        value = value * 10 + ch;
        offset++;
      }
      // Ten digits w/out a decimal point might have overflowed the int
      if (count == 10)
        return false;
      if (negative)
        result = -value;
      else
        result = value;
      return true;
    }
  }
}