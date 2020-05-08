using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace AET.SimplSharp {
  public class Emailer {
    private const string GmailServerHost = "smtp.gmail.com";
    private const int GmailServerPort = 465;
    private const bool SecureConnection = true;
    private const string NoCCAddresses = "";
    private const int NoAttachments = 0;
    private const string NoAttachmentFiles = "";

    public string SMTPServer { get; set; }
    public string SMTPFrom { get; set; }
    
    public string Username { get; set; }
    public string Password { get; set; }

    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    public void SendSMTPMessage() {
      CrestronMailFunctions.SendMail(SMTPServer, Username, Password, SMTPFrom, To, NoCCAddresses, Subject, Body);
    }

    public void SendGmailMessage() {
      CrestronMailFunctions.SendMail(GmailServerHost, GmailServerPort, SecureConnection, Username, Password, Username, To, NoCCAddresses, Subject, Body, NoAttachments, NoAttachmentFiles);
    }
  }
}