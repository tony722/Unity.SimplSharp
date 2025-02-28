using System;
using System.Collections.Generic;
using System.Text;
using Crestron.SimplSharp.Net.Http;
using ContentSource = Crestron.SimplSharp.Net.Http.ContentSource;
using RequestType = Crestron.SimplSharp.Net.Http.RequestType;

namespace AET.Unity.SimplSharp.HttpUtility {
  public sealed class HttpClientPool : IDisposable {

    private readonly ObjectPool<Lazy<Crestron.SimplSharp.Net.Http.HttpClient>> httpClientPool;
    private Encoding encoding;

    public HttpClientPool() : this(10) { }

    public Encoding Encoding { get { return encoding ?? (Encoding = Encoding.UTF8); } set { encoding = value; } }

    public HttpClientPool(int poolSize, int timeout) {
      httpClientPool = new ObjectPool<Lazy<Crestron.SimplSharp.Net.Http.HttpClient>>(poolSize, poolSize,
            () => new Lazy<Crestron.SimplSharp.Net.Http.HttpClient>(() => new Crestron.SimplSharp.Net.Http.HttpClient {
              TimeoutEnabled = true,
              Timeout = timeout,
              KeepAlive = false
            })) { CleanupPoolOnDispose = true };
    }

    public HttpClientPool(int poolSize) : this(poolSize, 5) { }

    private HttpResult SendRequest(string url, RequestType requestType, string content, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      var obj = httpClientPool.GetFromPool();
      var client = obj.Value;

      try {
        if (client.ProcessBusy)
          client.Abort();

        var httpRequest = new HttpClientRequest {
          RequestType = requestType,
          Encoding = Encoding,
          KeepAlive = false,
        };

        if (requestType != RequestType.Get && !string.IsNullOrEmpty(content)) {
          httpRequest.ContentSource = ContentSource.ContentString;
          httpRequest.ContentString = content;
        }

        if (additionalHeaders != null) {
          foreach (var item in additionalHeaders)
            httpRequest.Header.AddHeader(new HttpHeader(item.Key, item.Value));
        }

        httpRequest.Url.Parse(url);

        var httpResponse = client.Dispatch(httpRequest);       
        var responseContent = Encoding.GetString(httpResponse.ContentBytes, 0, httpResponse.ContentBytes.Length);
        return new HttpResult(httpResponse.Code, httpResponse.ResponseUrl, responseContent);
      } finally {
        httpClientPool.AddToPool(obj);
      }
    }



    public HttpResult Get(string url) {
      return Get(url, null);
    }

    public HttpResult Get(string url, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      return SendRequest(url, RequestType.Get, null, additionalHeaders);
    }

    public HttpResult Post(string url, string value) {
      return Post(url, value, null);
    }

    public HttpResult Post(string url, string value, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      return SendRequest(url, RequestType.Post, value, additionalHeaders);
    }

    public HttpResult Put(string url, string value) {
      return Put(url, value, null);
    }

    public HttpResult Put(string url, string value, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      return SendRequest(url, RequestType.Put, value, additionalHeaders);
    }

    public HttpResult Delete(string url) {
      return Delete(url, null);
    }

    public HttpResult Delete(string url, string value) {
      return Delete(url, value, null);
    }

    public HttpResult Delete(string url, string value, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      return SendRequest(url, RequestType.Delete, value, additionalHeaders);
    }

    public void Dispose() {
      httpClientPool.Dispose();
    }
  }
}
