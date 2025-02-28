using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AET.Unity.SimplSharp.HttpUtility;
using Crestron.SimplSharp;

namespace AET.Unity.SimplSharp.HttpClient {
  public class TestHttpClient : IHttpClient {
    public void Clear() {
      RequestContents = null;
      ResponseContents = null;
      Url = null;
      StatusCode = 200;
      ThrowException = null;
    }

    public string RequestContents { get; set; }
    public string ResponseContents { get; set; }
    public Dictionary<string, string> ResponseContentsForGetUrl { get; private set; } = new Dictionary<string, string>();
    public string Url { get; set; }
    public bool CaseSensitiveUrl { get; set; }
    public bool IncludeQueryString { get; set; }

    public int StatusCode = 200; 
    
    public Exception ThrowException { get; set; }

    public virtual HttpResult Post(string url, string contents) {
      if(ThrowException != null) throw ThrowException;
      Url = url;
      RequestContents = contents;
      return new HttpResult(StatusCode,Url,ResponseContents);
    }

    public virtual HttpResult Post(string url, string contents, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      return Post(url, contents);
    }

    public virtual HttpResult Get(string url) {
      try {
        if(ThrowException != null) throw ThrowException;
        Url = url;
        if (ResponseContents != null)  return new HttpResult(StatusCode, Url,ResponseContents);
        if (url == null) return new HttpResult(404, url, "Not Found");
        return GetResponseContents(url);
      } catch (SocketException ex) {
        ErrorMessage.Error("Unity.CrestronHttpClient.Get({0}) SocketException: {1}", url, ex.Message);
        return new HttpResult(ex.Message);
      }
      catch (Exception ex) {
        ErrorMessage.Error("Unity.CrestronHttpClient.Get({0}) Error: {1}", url, ex.Message);
        return new HttpResult(ex.Message);
      }
    }

    protected virtual HttpResult GetResponseContents(string url) {
      string responseContents = null;
      if (IncludeQueryString) {
        if (CaseSensitiveUrl) ResponseContentsForGetUrl.TryGetValue(url, out responseContents);
        else responseContents = ResponseContentsForGetUrl.FirstOrDefault(kvp => kvp.Key.Equals(url, StringComparison.OrdinalIgnoreCase)).Value;

      } 
      else {
        var baseUrl = url.Split('?')[0];
        responseContents = CaseSensitiveUrl
          ? ResponseContentsForGetUrl.FirstOrDefault(kvp => baseUrl.Equals(kvp.Key, StringComparison.Ordinal)).Value
          : ResponseContentsForGetUrl.FirstOrDefault(kvp => baseUrl.Equals(kvp.Key, StringComparison.OrdinalIgnoreCase)).Value;
      }
      if(responseContents == null) return new HttpResult(404, url, "Not Found");
      return new HttpResult(200, Url, responseContents);
    }

    public virtual HttpResult Get(string url, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      return Get(url);
    }

    public ushort Debug { get; set; }
    public Encoding Encoding { get; set; }
  }
}