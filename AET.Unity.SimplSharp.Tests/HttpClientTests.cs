using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using AET.Unity.SimplSharp.HttpClient;

namespace AET.Unity.SimplSharp.Tests {
  [TestClass]
  public class HttpClientTests {

    [TestMethod]
    public void HttpParse_Discovery() {
      var c = new HttpsSecureTcpClient();
      c.HttpsGet("https://docs.google.com/spreadsheets/d/1Vol-_8nDKHZtpKwFw3I2Nk4Dx5-Aki8ZFRbfPqixWcc/export?format=csv&gid=2672119");
    }
  }
}
