using System;
using System.Collections.Generic;

namespace AET.Unity.SimplSharp.HttpClient {
  public class TestHttpClient : IHttpClient {
    public static void Clear() {
      RequestContents = null;
      ResponseContents = null;
      Url = null;
    }

    public static string RequestContents { get; set; }
    public static string ResponseContents { get; set; }
    public static string Url { get; set; }


    public string Post(string url, string contents) {
      Url = url;
      RequestContents = contents;
      return ResponseContents;
    }

    public string Post(string url, string contents, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      return Post(url, contents);
    }

    public string Get(string url) {
      Url = url;
      return ResponseContents;
    }

    public string Get(string url, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      return Get(url);
    }

    public ushort Debug { get; set; }
  }
}