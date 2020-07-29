using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp.Net.Http;
using Crestron.SimplSharp.Net.Https;

namespace AET.Unity.SimplSharp.Http {
  public class CrestronHttpReader: IHttpReader {
    public string GetHttpsText(string url) {
      var client = new HttpsClient();
      client.PeerVerification = false;
      var response = client.GetResponse(url);
      return response.ContentString;
    }

    public string GetHttpText(string url) {
      var client = new HttpClient();
      var response = client.GetResponse(url);
      return response.ContentString;
    }
  }
}
