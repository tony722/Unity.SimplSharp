using Crestron.SimplSharp.CrestronSockets;
using Crestron.SimplSharp.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AET.Unity.SimplSharp.HttpClient {
    public class HttpsSecureTcpClient {      
      
      private string message;
      private Uri uri;
      private bool debug;
      private RequestType requestType;

      // Declare a delegate for the event, so that the event invocation can pass strongly typed arguments
      public delegate void MyEventDelegate(object o, MyEventArgs e);

      // Declare the event itself, of the delegate type. The name of this is what S+ uses as the second parameter for REGISTEREVENT
      public event MyEventDelegate MyEvent;

      public HttpsSecureTcpClient() : this(false) { }

      public HttpsSecureTcpClient(bool debug) { this.debug = debug; }      

      public void HttpsGet(string url) {
        requestType = RequestType.Get;
        try {
          uri = new Uri(url);
          var server = uri.Host;
          var port = uri.Port;

          // Create a new socket
          var client = new SecureTCPClient(server, port, 16000);
          client.SocketStatusChange += ClientSocketStatusChange;
          client.ConnectToServerAsync(ClientConnectCallback);          
        } catch (Exception e) {
          ErrorMessage.Error("Unity.CrestronHttpsTcpClient Error: {0}", e.Message);
        } 
      }

      void ClientConnectCallback(SecureTCPClient client) {
        if (client.ClientStatus == SocketStatus.SOCKET_STATUS_CONNECTED) {
          SendHttpRequest(client);
        } else {
          ErrorMessage.Notice("Unity.CrestronHttpsTcpClient: No connection could be made with the server.");
        }
      }

      private void SendHttpRequest(SecureTCPClient client) {
        client.ReceiveDataAsync(ClientReceiveCallback);
        var httpRequest = string.Format("GET {0} HTTP/1.1\r\n" +
                                        "Host: {1}\r\n" +
                                        "User-Agent: curl/8.0.1\r\n" +
                                        "Accept: text/csv; charset=utf-8\r\n" +
                                        "Connection: close\r\n\r\n", uri.PathAndQuery, uri.Host);

        var httpRequestBytes = Encoding.UTF8.GetBytes(httpRequest);
        client.SendDataAsync(httpRequestBytes, httpRequestBytes.Length, SecureTCPClientSendCallback);
      }
      
      private void ClientReceiveCallback(SecureTCPClient client, int bytesReceived) {
        if (bytesReceived > 0) {
          try {
            string received = Encoding.UTF8.GetString(client.IncomingDataBuffer, 0, client.IncomingDataBuffer.Length);            
            message += received;
          } catch (Exception e) {
            ErrorMessage.Notice("Unity.CrestronHttpsTcpClient: Exception in ClientReceiveCallback: " + e.Message);
          }
          client.ReceiveDataAsync(ClientReceiveCallback);
        }
      }

      private void ClientSocketStatusChange(SecureTCPClient client, SocketStatus clientSocketStatus) {
        if (clientSocketStatus == SocketStatus.SOCKET_STATUS_CONNECTED || message.Length <= 0) return;
        
        // HTTP requests separate the headers from the body by inserting 2x CRLF (Carriage Return Line Feed ie; \r\n\r\n) between them
        int headerEndIndex = message.IndexOf("\r\n\r\n");

        // Now that we have the index of where the double CRLF is we take that position and add 8 (one for each char of \ r \ n \ r \ n)
        // This gives us the exact start location of the actual body content
        string body = message.Substring(headerEndIndex + 8);
        int bodyEndIndex = body.IndexOf("\r\n\r\n");

        // For some reason the content starts with c3, probably something to do with encoding
        if (body.Contains("c3")) {
          int c3Index = body.IndexOf("c3");

          // There are 4 chars between c3 and the start of the actual body content
          string bodyStart = body.Substring(c3Index + 4);

          // Look for the first " which indicates the start of the CSV data
          int lastQuotationIndex = bodyStart.LastIndexOf("\"");

          // Look for the last " in the CSV data, and cut everything else past it
          string actualContent = bodyStart.Substring(0, lastQuotationIndex + 1);

          // Send the data to S+
          var args = new MyEventArgs {EventMessage = actualContent};
          MyEvent.Invoke(this, args);
        }

        // Now that we've gotten a full message we'll clear out the message string, so that the next request can begin populating it anew
        message = string.Empty;

        // Best practice: Dispose once we have sent the message and received the full response
        client.Dispose();        
      }

      private void SecureTCPClientSendCallback(SecureTCPClient client, int numberOfBytesSent) { }
    }

    // Separate public class for the event arguments, which can be passed to the event. The name of this class is what is used
    // in S+ as the second parameter for the EVENTHANDLER.
    public class MyEventArgs : EventArgs {
      public string EventMessage;

      // All classes that are exposed to S+ need to have a public parameterless constructor
    }

    //public class HttpResponse {
    //  private string content;
    //  public HttpResponse(string content) { this.content = content; }

    //}
  }

