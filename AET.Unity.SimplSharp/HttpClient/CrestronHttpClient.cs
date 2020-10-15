using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AET.Unity.SimplSharp.HttpUtility;
using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp.HttpClient {
  public class CrestronHttpClient : IHttpClient {
    private readonly HttpClientPool pool;

    public CrestronHttpClient(int poolSize) {
      pool = new HttpClientPool(poolSize);
    }

    public ushort Debug { get; set; }

    public string Get(string url) {
      return Get(url, null);
    }

    public string Get(string url, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      if (Debug == 1) CrestronConsole.PrintLine("Unity.CrestronPooledHttpClient.PostAsync({0})", url);
      try {
        var results = pool.Get(url);
        if (Debug == 1) CrestronConsole.PrintLine("Results status({0}): {1}", results.Status, results.Content);
        return results.Content;
      } catch (Exception ex) {
        ErrorMessage.Error("Unity.CrestronHttpClient.Get({0}) Error: {1}", url, ex.Message);
      }
      return string.Empty;
    }

    public string Post(string url, string contents) {
      return Post(url, contents, null);
    }

    public string Post(string url, string contents, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      if (Debug == 1) CrestronConsole.PrintLine("Unity.CrestronPooledHttpClient.Post({0})\r\nContents: {1}", url, contents);
      try {
        var results = pool.Post(url, contents, additionalHeaders);
        if (Debug == 1) CrestronConsole.PrintLine("Results status({0}): {1}", results.Status, results.Content);
        return results.Content;
      } catch (Exception ex) {
        ErrorMessage.Error("Unity.CrestronHttpClient.Post({0}) Error: {1}\r\nContents: {2}", url, ex.Message, contents);
      }
      return string.Empty;
    }
  }
}