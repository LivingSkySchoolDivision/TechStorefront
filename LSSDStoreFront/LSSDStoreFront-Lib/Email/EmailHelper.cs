using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace LSSD.StoreFront.Lib.Email
{
    public class EmailHelper
    {
        private static EmailHelper instance = null;
        private static readonly Object threadLock = new object();

        private string hostname;
        private string username;
        private string password;
        private string fromaddress;
        private string replyToAddress;
        private int smtpPort;

        private EmailHelper(string hostname, int smtpPort, string username, string password, string fromaddress, string replyToAddress)
        {
            this.hostname = hostname;
            this.username = username;
            this.password = password;
            this.fromaddress = fromaddress;
            this.smtpPort = smtpPort;
            this.replyToAddress = replyToAddress;
        }

        public static EmailHelper GetInstance(string hostname, int smtpPort, string username, string password, string fromaddress, string replyToAddress)
        {
            lock (threadLock)
            {
                if (instance == null)
                {                    
                    instance = new EmailHelper(hostname, smtpPort, username, password, fromaddress, replyToAddress);
                } else
                {
                    // If we get a different username, password, or hostname, reset the singleton to the new information
                    if (!(instance.hostname.Equals(hostname) && instance.username.Equals(username) && instance.password.Equals(password)))
                    {
                        instance = new EmailHelper(hostname, smtpPort, username, password, fromaddress, replyToAddress);
                    }
                }

                return instance;
            }            
        }
        
        Queue<MailMessage> _mailQueue = new Queue<MailMessage>();

        public void NewMessage(string to, CannedEmailMessage message)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(to);
            msg.Body = message.Content;
            msg.Subject = message.Subject;
            msg.From = new MailAddress(this.fromaddress);
            msg.ReplyToList.Add(new MailAddress(this.replyToAddress));
            msg.IsBodyHtml = true;
            this._mailQueue.Enqueue(msg);
        }


        public void FlushQueue()
        {
            if (this._mailQueue.Count > 0)
            {                
                using (SmtpClient smtpClient = new SmtpClient(this.hostname)
                {
                    Port = this.smtpPort,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(this.username, this.password)
                })
                {
                    List<MailMessage> sentMessages = new List<MailMessage>();
                    Dictionary<MailMessage, Exception> problemMessages = new Dictionary<MailMessage, Exception>();

                    while (this._mailQueue.Count > 0)
                    {
                        MailMessage message = this._mailQueue.Dequeue();

                        try
                        {
                            smtpClient.Send(message);
                            sentMessages.Add(message);
                        }
                        catch (Exception ex)
                        {
                            problemMessages.Add(message, ex);
                        }
                    }

                    // Log mail messages

                    // Add failed messages back into the queue
                    foreach(MailMessage msg in problemMessages.Keys)
                    {
                        this._mailQueue.Enqueue(msg);
                    }
                }
            }
        }



    }
}
