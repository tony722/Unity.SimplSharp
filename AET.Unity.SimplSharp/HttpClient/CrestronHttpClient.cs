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

    public CrestronHttpClient(int poolSize, int timeout) {
      pool = new HttpClientPool(poolSize, timeout);
    }

    public Encoding Encoding { get { return pool.Encoding; } set { pool.Encoding = value; } }

    public ushort Debug { get; set; }

    public HttpResult Get(string url) {
      return Get(url, null);
    }

    public HttpResult Get(string url, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      if (Debug == 1) {
        CrestronConsole.PrintLine("Unity.CrestronPooledHttpClient.PostAsync({0})", url);        
        if(additionalHeaders != null) CrestronConsole.PrintLine("    Headers: {0}", PrintDictionary(additionalHeaders));
      }
      try {
        var results = pool.Get(url, additionalHeaders);
        if (Debug == 1) CrestronConsole.PrintLine("Results status({0}): {1}", results.Status, results.Content);
        return results;
      } catch (SocketException ex) {
        ErrorMessage.Error("Unity.CrestronHttpClient.Get({0}) SocketException: {1}", url, ex.Message);
        return new HttpResult(ex.Message);
      }
      catch (Exception ex) {
        ErrorMessage.Error("Unity.CrestronHttpClient.Get({0}) Error: {1}", url, ex.Message);
        return new HttpResult(ex.Message);
      }
    }

    public HttpResult Post(string url, string contents) {
      return Post(url, contents, null);
    }

    public HttpResult Post(string url, string contents, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      if (Debug == 1) {
        CrestronConsole.PrintLine("Unity.CrestronPooledHttpClient.Post({0})\r\nContents: {1}", url, contents);
        if (additionalHeaders != null) CrestronConsole.PrintLine("    Headers: {0}", PrintDictionary(additionalHeaders));
      }
      try {
        var results = pool.Post(url, contents, additionalHeaders);
        if (Debug == 1) CrestronConsole.PrintLine("Results status({0}): {1}", results.Status, results.Content);
        return results;
      } catch (Exception ex) {
        ErrorMessage.Error("Unity.CrestronHttpClient.Post({0}) Error: {1}\r\nContents: {2}", url, ex.Message, contents);
      }
      return new HttpResult(0,string.Empty,string.Empty);
    }

    private string PrintDictionary(IEnumerable<KeyValuePair<string, string>> dict) {
      if (dict == null) return string.Empty;
      return string.Join(",", dict.Select(kv => kv.Key + "=" + kv.Value).ToArray());
    }
  }
}