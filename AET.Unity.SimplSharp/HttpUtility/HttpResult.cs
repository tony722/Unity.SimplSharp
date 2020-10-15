namespace AET.Unity.SimplSharp.HttpUtility {
  public class HttpResult {
    private readonly int _status;
    public int Status { get { return _status; } }

    private readonly string _responseUrl;
    public string ResponseUrl { get { return _responseUrl; } }

    private readonly string _content;
    public string Content { get { return _content; } }

    public HttpResult(int status, string responseUrl, string content) {
      _status = status;
      _responseUrl = responseUrl;
      _content = content;
    }
  }
}