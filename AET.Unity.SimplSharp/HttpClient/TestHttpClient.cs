using System;

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

    public void PostAsync(string url, string contents) {
      Url = url;
      RequestContents = contents;
    }

    public void PostAsync(string url, string contents, Action<string> callbackAction) {
      Post(url, contents);
      callbackAction(ResponseContents);
    }

    public void Post(string url, string contents) {
      Url = url;
      RequestContents = contents;
    }

    public void Post(string url, string contents, Action<string> response) {
      Post(url, contents);
      response(ResponseContents);
    }

    public string Get(string url) {
      return ResponseContents;
    }

    public void GetAsync(string url, Action<string> callbackAction) {
      callbackAction(ResponseContents);
    }

    public ushort Debug { get; set; }
  }
}
