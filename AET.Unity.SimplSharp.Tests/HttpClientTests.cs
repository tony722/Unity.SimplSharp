using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AET.Unity.SimplSharp.HttpClient;
using FluentAssertions;

namespace AET.Unity.SimplSharp.Tests {
  [TestClass]
  public class HttpClientTests {
    private readonly TestHttpClient client = new TestHttpClient();
    
    [TestMethod]
    public void ResponseContentsForGetUrl_NullUrl_Returns404() {
      client.Get(null).Status.Should().Be(404);
    }

    [TestMethod]
    public void ResponseContentsForGetUrl_CaseInsensitiveIgnoreQuerystring_ReturnsExpectedContents() {
      client.ResponseContentsForGetUrl["/test/"] = "test";
      client.Get("/TEST/").Content.Should().Be("test");
      client.Get("/TEST/?12345").Content.Should().Be("test");
      client.Get("/TeSt/too much/").Status.Should().Be(404);
    }

    [TestMethod]
    public void ResponseContentsForGetUrl_CaseSensitiveIgnoreQuerystring_ReturnsExpectedContents() {
      client.CaseSensitiveUrl = true;
      client.ResponseContentsForGetUrl["/test/"] = "test";
      client.Get("/TEST/").Status.Should().Be(404);
      client.Get("/test/").Content.Should().Be("test");
      client.Get("/test/?12345").Content.Should().Be("test");
    }

    [TestMethod]
    public void ResponseContentsForGetUrl_CaseInsensitiveIncludeQuerystring_ReturnsExpectedContents() {
      client.IncludeQueryString = true;
      client.ResponseContentsForGetUrl["/test"] = "test";
      client.Get("/TEST/?12345").Status.Should().Be(404);
      client.Get("/TEST").Content.Should().Be("test");
    }
    [TestMethod]
    public void ResponseContentsForGetUrl_CaseSensitiveFullMatch_ReturnsExpectedContents() {
      client.IncludeQueryString = true;
      client.CaseSensitiveUrl = true;
      client.ResponseContentsForGetUrl["/test"] = "test";
      client.Get("/TEST/?12345").Status.Should().Be(404);
      client.Get("/test/?12345").Status.Should().Be(404);
      client.Get("/TEST").Status.Should().Be(404);
      client.Get("/test").Content.Should().Be("test");
    }
  }
}
