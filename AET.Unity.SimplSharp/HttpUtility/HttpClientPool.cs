using System;
using System.Collections.Generic;
using System.Text;
using Crestron.SimplSharp.Net.Http;
using ContentSource = Crestron.SimplSharp.Net.Http.ContentSource;
using RequestType = Crestron.SimplSharp.Net.Http.RequestType;

namespace AET.Unity.SimplSharp.HttpUtility {
  public sealed class HttpClientPool : IDisposable {

    private readonly ObjectPool<Lazy<Crestron.SimplSharp.Net.Http.HttpClient>> httpClientPool;

    public HttpClientPool() : this(10) { }

    public HttpClientPool(int poolSize) {
      httpClientPool = new ObjectPool<Lazy<Crestron.SimplSharp.Net.Http.HttpClient>>(poolSize, poolSize,
            () => new Lazy<Crestron.SimplSharp.Net.Http.HttpClient>(() => new Crestron.SimplSharp.Net.Http.HttpClient {
              TimeoutEnabled = true,
              Timeout = 5,
              KeepAlive = false
            })) { CleanupPoolOnDispose = true };
    }

    private HttpResult SendRequest(string url, RequestType requestType, string content, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      var obj = httpClientPool.GetFromPool();
      var client = obj.Value;

      try {
        if (client.ProcessBusy)
          client.Abort();

        var httpRequest = new HttpClientRequest {
          RequestType = requestType,
          Encoding = Encoding.UTF8,
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

        HttpClientResponse httpResponse = client.Dispatch(httpRequest);
        return new HttpResult(httpResponse.Code, httpResponse.ResponseUrl, httpResponse.ContentString);
      } catch (Exception ex) {
        throw ex;
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
