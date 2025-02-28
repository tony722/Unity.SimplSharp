using System;
using System.Text;
using Crestron.SimplSharp.Cryptography;


namespace AET.Unity.SimplSharp {
  public static class GuidExtensions {

    /// <summary>
    /// Creates a deterministic GUID based on the provided string.
    /// The same string will always produce the same GUID.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>A GUID derived from the string.</returns>
    public static string ToGuid(this string input) {
      using (var md5 = MD5.Create()) {
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        return new Guid(hash).ToString();
      }
    }

    public static string NewGuid() {
      return Guid.NewGuid().ToString();
    }
  }
}
