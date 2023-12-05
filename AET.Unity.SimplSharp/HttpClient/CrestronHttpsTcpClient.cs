using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using AET.Unity.SimplSharp.HttpUtility;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;
using Crestron.SimplSharp.Net.Http;

namespace AET.Unity.SimplSharp.HttpClient {
  /// <summary>
  ///   This class uses the Crestron TCPClient object to workaround a bug in the Crestron HttpsClient
  /// </summary>
  public class CrestronHttpsTcpClient : IHttpClient {
    private string _message;

    #region IHttpClient Members

    public HttpResult Post(string url, string contents) { throw new NotImplementedException(); }

    public HttpResult Post(string url, string contents, IEnumerable<KeyValuePair<string, string>> additionalHeaders) { throw new NotImplementedException(); }

    public HttpResult Get(string url) { throw new NotImplementedException(); }

    public HttpResult Get(string url, IEnumerable<KeyValuePair<string, string>> additionalHeaders) { throw new NotImplementedException(); }

    public ushort Debug { get; set; }

    #endregion
  }
}