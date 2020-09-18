using System;
using System.Text;
using Crestron.SimplSharp.Net.Http;
using Crestron.SimplSharp;
using System.Threading;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.Net;

namespace AET.Unity.SimplSharp.HttpClient {
  public class CrestronHttpClient : IHttpClient {
    private CMutex mutex = new CMutex();    

    public CrestronHttpClient() { }

    public void Post(string url, string contents) {
      Post(url, contents, (responseString) => {
        if (Debug == 1) CrestronConsole.PrintLine("Unity.HttpClient.Post({0}) Results:\r\n{1}", url, responseString);
      });
    }

    public ushort Debug { get; set; }
    public ushort TimeoutMs { get; set; }

    public void Post(string url, string contents, Action<string> responseCallback) {
      var request = BuildRequest(url, contents);
      var httpClient = new Crestron.SimplSharp.Net.Http.HttpClient();
      try {
        if (Debug == 1) CrestronConsole.PrintLine("Unity.HttpClient HttpRequest: {0} | {1}", url, contents);
        var response = httpClient.Post(url, System.Text.Encoding.ASCII.GetBytes(contents));
        responseCallback(response);
      }
      catch (Exception ex) {
        ErrorMessage.Error("Unity.HttpClient Error: {0}", ex.Message);
      }
      finally {
        httpClient.Abort();
        httpClient.Dispose();
      }
    }

    public void PostAsync(string url, string contents) {
      PostAsync(url, contents, (responseString) => {
        if (Debug == 1) CrestronConsole.PrintLine("Unity.HttpClient.PostAsync({0}):\r\n{1}", url, responseString);
      });
    }

    public void PostAsync(string url, string contents, Action<string> callbackAction) {
      var httpClient = new Crestron.SimplSharp.Net.Http.HttpClient();
      var callbackObject = new AsyncCallbackObject { Url = url, Callback = callbackAction, HttpClient = httpClient };
      if (Debug == 1) CrestronConsole.PrintLine("Unity.HttpClient.PostAsync({0}):", url);
      var request = BuildRequest(url, contents);
      try {
        if (Debug == 1) CrestronConsole.PrintLine("{0}", contents);
        var error = httpClient.DispatchStringAsyncEx(request, Encoding.ASCII, PostAsyncCallback, callbackObject);
        if (error != Crestron.SimplSharp.Net.Http.HttpClient.DISPATCHASYNC_ERROR.PENDING) {
          ErrorMessage.Error("Unity.HttpClient.PostAsync({0}): {1}", url, error.ToString());
        }
      } catch (Exception ex) {
        ErrorMessage.Error("Unity.HttpClient API Error: {0}", ex.Message);
      }
    }

    private void PostAsyncCallback(string response, HTTP_CALLBACK_ERROR err, object userObject) {
      var callbackObject = userObject as AsyncCallbackObject;
      if (callbackObject == null) return;
      try {
        if (err == HTTP_CALLBACK_ERROR.COMPLETED) callbackObject.Callback(response);
        else ErrorMessage.Error("Unity.HttpClient.PostAsync({0}): {1}", callbackObject.Url, err.ToString());
      } catch (Exception ex) {
        ErrorMessage.Error("Unity.HttpClient.PostAsync({0}): {1}", callbackObject.Url, ex.Message);
      } finally {
        callbackObject.HttpClient.Abort();
        callbackObject.HttpClient.Dispose();
      }
    }

    public string Get(string url) {
      var httpClient = new Crestron.SimplSharp.Net.Http.HttpClient();
      try {
        var results = httpClient.Get(url);
        if (Debug == 1) CrestronConsole.PrintLine("Unity.HttpClient.Get: {0}\r\n{1}", url, results);
        return results;
      }
      catch (Exception ex) {
        ErrorMessage.Error("Unity.HttpClient Get({1}) Error: {0}", ex.Message, url);
        return "";
      }
      finally {
        httpClient.Abort();
        httpClient.Dispose();
      }
    }

    public void GetAsync(string url, Action<string> callbackAction) {
      var httpClient = new Crestron.SimplSharp.Net.Http.HttpClient();
      var callbackObject = new AsyncCallbackObject { Url = url, Callback = callbackAction, HttpClient = httpClient };
      try {
        httpClient.KeepAlive = false;
        if (Debug == 1) CrestronConsole.PrintLine("Unity.HttpClient.GetAsync: {0}", url);
        httpClient.GetAsyncEx(url, Encoding.ASCII, GetAsyncCallback, callbackObject);
      } catch (Exception ex) {
        ErrorMessage.Error("Unity.HttpClient.GetAsync({1}) Error: {0}", ex.Message, url);
      }
    }

    private void GetAsyncCallback(string results, HTTP_CALLBACK_ERROR err, object userObject) {
      var callbackObject = userObject as AsyncCallbackObject;
      if (callbackObject == null) return;
      try {
        if (Debug == 1) CrestronConsole.PrintLine("{0}", results);
        if (err == HTTP_CALLBACK_ERROR.COMPLETED) callbackObject.Callback(results);
        else ErrorMessage.Error("Unity.HttpClient.GetAsync({1}) Error: {0}", err.ToString(), callbackObject.Url);
      } catch (Exception ex) {
        ErrorMessage.Error("Unity.HttpClient.GetAsync({1}) Error: {0}", ex.Message, callbackObject.Url);
      } finally {
        callbackObject.HttpClient.Abort();
        callbackObject.HttpClient.Dispose();
      }
    }

    private HttpClientRequest BuildRequest(string url, string contents) {
      return new HttpClientRequest {
        RequestType = RequestType.Post,
        CloseStream = true,
        ContentString = contents,
        Url = { Url = url },
        KeepAlive = false,
        Header = { ContentType = "application/json" }
      };
    }

    private class AsyncCallbackObject {
      public string Url { get; set; }
      public Action<String> Callback { get; set; }
      public Crestron.SimplSharp.Net.Http.HttpClient HttpClient { get; set; }
    }
  }
}
