using LSSD.StoreFront.DB;
using LSSD.StoreFront.DB.repositories;
using LSSD.StoreFront.EmailRunner.Models;
using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace LSSD.StoreFront.EmailRunner.Helpers
{
    class EmailHelper
    {
        private string hostname;
        private string username;
        private string password;
        private string fromaddress;
        private string replyToAddress;
        private int smtpPort;

        public EmailHelper(string hostname, int smtpPort, string username, string password, string fromaddress, string replyToAddress)
        {
            this.hostname = hostname;
            this.username = username;
            this.password = password;
            this.fromaddress = fromaddress;
            this.smtpPort = smtpPort;
            this.replyToAddress = replyToAddress;
        }        

        Queue<EmailNotification> _mailQueue = new Queue<EmailNotification>();

        public void NewMessage(string EmailAddressTo, CannedEmailMessage Message, Order Order)
        {            
            this._mailQueue.Enqueue(new EmailNotification()
            {
                To = EmailAddressTo,
                Order = Order,
                Message = Message
            });
        }

        public void FlushQueue(DatabaseContext dbContext)
        {
            OrderNotificationLogRepository notificationRepo = new OrderNotificationLogRepository(dbContext);

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
                    while (this._mailQueue.Count > 0)
                    {
                        EmailNotification notification = this._mailQueue.Dequeue();

                        try
                        {
                            // Send the mail message and log the results in the DB
                            MailMessage msg = new MailMessage();
                            msg.To.Add(notification.To);
                            msg.Body = notification.Message.Content;
                            msg.Subject = notification.Message.Subject;
                            msg.From = new MailAddress(this.fromaddress);
                            msg.ReplyToList.Add(new MailAddress(this.replyToAddress));
                            msg.IsBodyHtml = true;
                            smtpClient.Send(msg);

                            // Log a success
                            notificationRepo.LogNotification(notification.Order.OrderThumbprint, notification.To, true, notification.Message.ForCustomer, notification.Message.ForOrderDesk, string.Empty);
                        }
                        catch (Exception ex)
                        {
                            // Log a failure
                            notificationRepo.LogNotification(notification.Order.OrderThumbprint, notification.To, false, notification.Message.ForCustomer, notification.Message.ForOrderDesk, ex.Message);
                        }
                    }
                    
                }
            }
        }

    }
}
