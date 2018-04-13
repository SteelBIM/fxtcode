using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Core.Utility
{
    /// <summary>
    /// 邮件发送帮助类
    /// </summary>
    public sealed class MailHelper
    {
        private MailHelper() { }


        private static void SendEmail(string clientHost, string emailAddress, string receiveAddress,
          string userName, string password, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailAddress);
            mail.To.Add(new MailAddress(receiveAddress));
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            SmtpClient client = new SmtpClient();
            client.Host = clientHost;
            client.Credentials = new NetworkCredential(userName, password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.SendAsync(mail,null);
            }
            catch (Exception)
            {

            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MailHelper.SendEmail(string, string)”的 XML 注释
        public static void SendEmail(string subject, string content)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MailHelper.SendEmail(string, string)”的 XML 注释
        {
            //TODO
            return;
        }
    }
}
