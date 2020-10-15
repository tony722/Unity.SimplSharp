using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace AET.Unity.SimplSharp.HttpClient {
  public interface IHttpClient {

    string Post(string url, string contents);

    string Post(string url, string contents, IEnumerable<KeyValuePair<string, string>> additionalHeaders);

    string Get(string url);

    string Get(string url, IEnumerable<KeyValuePair<string, string>> additionalHeaders);

    ushort Debug { get; set; }
  }
}