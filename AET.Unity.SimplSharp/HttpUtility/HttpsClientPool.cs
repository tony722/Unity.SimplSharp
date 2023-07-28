using System;
using System.Collections.Generic;
using System.Text;
using Crestron.SimplSharp.Net.Https;

namespace AET.Unity.SimplSharp.HttpUtility {
  public sealed class HttpsClientPool : IDisposable {

    private readonly ObjectPool<Lazy<Crestron.SimplSharp.Net.Https.HttpsClient>> httpClientPool;

    public HttpsClientPool() : this(10) { }

    public HttpsClientPool(int poolSize, bool enableCertificateVerification) {
      httpClientPool = new ObjectPool<Lazy<Crestron.SimplSharp.Net.Https.HttpsClient>>(poolSize, poolSize,
            () => new Lazy<Crestron.SimplSharp.Net.Https.HttpsClient>(() => new Crestron.SimplSharp.Net.Https.HttpsClient {
              TimeoutEnabled = true,
              Timeout = 5,
              KeepAlive = false,
              PeerVerification = enableCertificateVerification,
              HostVerification = enableCertificateVerification
            })) { CleanupPoolOnDispose = true };
    }

    public HttpsClientPool(int poolSize) : this(poolSize, true) { }

    private HttpResult SendRequest(string url, RequestType requestType, string content, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      var obj = httpClientPool.GetFromPool();
      var client = obj.Value;

      try {
        if (client.ProcessBusy)
          client.Abort();

        var httpRequest = new HttpsClientRequest {
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
            httpRequest.Header.AddHeader(new HttpsHeader(item.Key, item.Value));
        }

        httpRequest.Url.Parse(url);

        HttpsClientResponse httpResponse = client.Dispatch(httpRequest);
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
