using System;
using System.Net;

namespace AET.Unity.SimplSharp.Http {
  class TestHttpReader : IHttpReader {
    public string GetHttpsText(string url) {
      using (var httpClient = new System.Net.Http.HttpClient()) {
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        httpClient.DefaultRequestHeaders.Accept.Clear();
        using (var response = httpClient.GetAsync(new Uri(url)).Result) {
          var content = response.Content.ReadAsStringAsync().Result;
          return content;
        }
      }
    }

    public string GetHttpText(string url) {
      throw new NotImplementedException();
    }
  }
}
