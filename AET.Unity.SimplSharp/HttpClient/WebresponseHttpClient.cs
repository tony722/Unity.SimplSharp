using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AET.Unity.SimplSharp.HttpUtility;
using Crestron.SimplSharp;
using AET.Unity.Http;
using Crestron.SimplSharp.CrestronDataStore;
using Crestron.SimplSharp.Reflection;

namespace AET.Unity.SimplSharp.HttpClient {
  public class WebresponseHttpClient : IHttpClient {

    #region IHttpClient Members

    public AET.Unity.SimplSharp.HttpUtility.HttpResult Post(string url, string contents) {
      throw new NotImplementedException();
    }

    public AET.Unity.SimplSharp.HttpUtility.HttpResult Post(string url, string contents, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      throw new NotImplementedException();
    }

    public AET.Unity.SimplSharp.HttpUtility.HttpResult Get(string url)
    {
      //var webrequestType = typeof(System.Net.WebRequest).GetCType();
      //var client = webrequestType.GetMethod("Create", BindingFlags.Public | BindingFlags.Static, CType.DefaultBinder, new CType[] { typeof(string).GetCType() }, null).Invoke(null, new object[] { url });
      //webrequestType.GetProperty("Method").SetValue(client, "GET", null);
      //var response = webrequestType.GetMethod("GetResponse").Invoke(client, null);
      //var responseType = response.GetType().GetCType();
      //var responseCode = (string)responseType.GetProperty("StatusCode").GetValue(response, null);
      //var responseUri = responseType.GetProperty("ResponseUri").GetValue(response, null);
      //var responseStream = response.GetType().GetCType().GetMethod("GetResponseStream").Invoke(response, null);
      //var streamReaderType = typeof(System.IO.StreamReader).GetCType();
      //var srCtor = streamReaderType.GetConstructor(new CType[] { typeof(System.IO.Stream).GetCType() });
      //var sr = srCtor.Invoke(new object[] { responseStream });
      //var content = (string)streamReaderType.GetMethod("ReadToEnd").Invoke(sr, null);
      //return new HttpResult(responseCode=="OK"?200:400, responseUri.ToString(), content);
      var client = new AET.Unity.Http.HttpClient();
      var content = client.Get(url);
      return new HttpResult(200,url,content);
    }

    public AET.Unity.SimplSharp.HttpUtility.HttpResult Get(string url, IEnumerable<KeyValuePair<string, string>> additionalHeaders) {
      throw new NotImplementedException();
    }

    public ushort Debug {
      get {
        throw new NotImplementedException();
      }
      set {
        throw new NotImplementedException();
      }
    }

    #endregion
  }
}