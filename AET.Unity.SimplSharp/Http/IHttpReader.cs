using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AET.Unity.SimplSharp.Http {
  public interface IHttpReader {
    string GetHttpsText(string url);
    string GetHttpText(string url);
  }
}
