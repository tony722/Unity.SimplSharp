namespace AET.Unity.SimplSharp.HttpUtility {
  public class HttpResult {
    public int Status { get; private set; }
    public string Error { get; private set; }
    public string ResponseUrl { get; private set; }

    public string Content { get; private set; }

    public HttpResult(int status, string responseUrl, string content) {
      Status = status;
      ResponseUrl = responseUrl;
      Content = content;
    }

    public HttpResult(string error) {
      Error = error;
    }
  }
}