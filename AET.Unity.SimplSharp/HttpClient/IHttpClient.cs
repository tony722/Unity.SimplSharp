using System;

namespace AET.Unity.SimplSharp.HttpClient {
  public interface IHttpClient {
    void PostAsync(string url, string contents);

    void PostAsync(string url, string contents, Action<string> callbackAction);

    void Post(string url, string contents);

    void Post(string url, string contents, Action<string> response);

    string Get(string url);

    void GetAsync(string url, Action<string> callbackAction);

    ushort Debug { get; set; }
  }
}
