using System.Collections.Generic;
using AET.Unity.SimplSharp.HttpUtility;

namespace AET.Unity.SimplSharp.HttpClient {
  public class TestHttpClient : IHttpClient {
    public TestHttpClient() {
      StatusCode = 200;
    }
    public static void Clear() {
      RequestContents = null;
      ResponseContents = null;
      Url = null;
      StatusCode = 200;
    }

    public static string RequestContents { get; set; }
    public static string ResponseContents { get; set; }
    public static string Url { get; set; }

    public static int StatusCode; 


    public HttpResult Post(string url, string contents) {
      Url = url;
      RequestContents = contents;
      return new HttpResult(StatusCode,Url,ResponseContents);
      
    }

    public HttpResult Post(string url, string contents, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      return Post(url, contents);
    }

    public HttpResult Get(string url) {
      Url = url;
      return new HttpResult(StatusCode, Url,ResponseContents);
    }

    public HttpResult Get(string url, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      return Get(url);
    }

    public ushort Debug { get; set; }
  }
}