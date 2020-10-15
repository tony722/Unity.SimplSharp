using System;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net;
using Crestron.SimplSharp.Net.Http;

namespace AET.Unity.SimplSharp.HttpClient {
  public class CrestronHttpClient : IHttpClient {
    public CrestronHttpClient() { }

    public void Post(string url, string contents) {
      Post(url, contents, (responseString) => {
        if (Debug == 1) CrestronConsole.PrintLine("Unity.HttpClient.Post({0}) Results:\r\n{1}", url, responseString);
      });
    }

    public ushort Debug { get; set; }
    public ushort TimeoutMs { get; set; }

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
      var request = new HttpClientRequest {
        RequestType = RequestType.Get,
        Url = {Url = url},
        KeepAlive = false,        
      };
      if (Debug == 1) CrestronConsole.PrintLine("Unity.HttpClient.GetAsync({0}):", url);
      DispatchAsync(request, callbackAction);
    }

    public void Post(string url, string contents, Action<string> responseCallback) {
      var httpClient = new Crestron.SimplSharp.Net.Http.HttpClient();
      try {
        if (Debug == 1) CrestronConsole.PrintLine("Unity.HttpClient HttpRequest: {0} | {1}", url, contents);
        var response = httpClient.Post(url, Encoding.ASCII.GetBytes(contents));
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
      PostAsync(url, contents, null);
    }

    public void PostAsync(string url, string contents, Action<string> callbackAction) {
      var request = new HttpClientRequest {
        RequestType = RequestType.Post,
        CloseStream = true,
        ContentString = contents,
        Url = {Url = url},
        KeepAlive = false,
        Header = {ContentType = "application/json"}
      };
      if (Debug == 1) CrestronConsole.PrintLine("Unity.HttpClient.PostAsync({0}):\r\n{1}", url, contents);
      DispatchAsync(request, callbackAction);
    }
    
    private void DispatchAsync(HttpClientRequest request, Action<string> callbackAction) {
      var httpClient = new Crestron.SimplSharp.Net.Http.HttpClient() { UseConnectionPooling = true, KeepAlive = false }; 
      var callbackObject = new AsyncCallbackObject
        {Url = request.Url.Url, Callback = callbackAction, HttpClient = httpClient, RequestType = request.RequestType, Request = request.ContentString};
      try {
        var error = httpClient.DispatchStringAsyncEx(request, Encoding.ASCII, AsyncCallback, callbackObject);
        if (error != Crestron.SimplSharp.Net.Http.HttpClient.DISPATCHASYNC_ERROR.PENDING) {
          ErrorMessage.Error("Unity.HttpClient.DispatchAsync({0}) {2} Error: {1}\r\n", request.Url.Url, error.ToString(), request.RequestType.ToString());
        }
      }
      catch (Exception ex) {
        ErrorMessage.Error("Unity.HttpClient API Error: {0}", ex.Message);
      }
    }

    private void AsyncCallback(string response, HTTP_CALLBACK_ERROR err, object userObject) {
      var callbackObject = userObject as AsyncCallbackObject;
      if (callbackObject == null) return;
      try {
        if (Debug == 1)
          CrestronConsole.PrintLine("Unity.HttpClient.AsyncCallback({0}) {1}:\r\nRequest: {2}\r\nResponse: {3}", callbackObject.Url, callbackObject.RequestType, callbackObject.Request, response);
        if (err != HTTP_CALLBACK_ERROR.COMPLETED) ErrorMessage.Error("Unity.HttpClient.AsyncCallback({0}) {2} Error1: {1}\r\nRequest: {3}\r\nResponse: {4}", callbackObject.Url, err.ToString(), callbackObject.RequestType, callbackObject.Request, response);
        else if(callbackObject.Callback != null) callbackObject.Callback(response);
      }
      catch (Exception ex) {
        ErrorMessage.Error("Unity.HttpClient.AsyncCallback({0}) {2} Error2: {1}", callbackObject.Url, ex.Message, callbackObject.RequestType);
      }
      finally {
        try {
          callbackObject.HttpClient.Abort();
        } finally {
          callbackObject.HttpClient.Dispose();
        }
      }
    }

    private class AsyncCallbackObject {
      public string Url { get; set; }
      public string Request { get; set; }
      public Action<String> Callback { get; set; }
      public Crestron.SimplSharp.Net.Http.HttpClient HttpClient { get; set; }
      public RequestType RequestType { get; set; }
    }    
  }
}