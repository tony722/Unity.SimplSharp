namespace AET.Unity.SimplSharp.Http {
  public interface IHttpReader {
    string GetHttpsText(string url);
    string GetHttpText(string url);
  }
}
