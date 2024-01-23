using AET.Unity.SimplSharp.HttpUtility;
using System.Collections.Generic;

namespace AET.Unity.SimplSharp.HttpClient {
  public interface IHttpClient {

    HttpResult Post(string url, string contents);

    HttpResult Post(string url, string contents, IEnumerable<KeyValuePair<string, string>> additionalHeaders);

    HttpResult Get(string url);

    HttpResult Get(string url, IEnumerable<KeyValuePair<string, string>> additionalHeaders);

    ushort Debug { get; set; }
  }
}