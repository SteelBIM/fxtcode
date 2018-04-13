using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Core.Utility
{
    public class EmailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="SendTo">收送人的地址</param>
        /// <param name="EmailAddress">我的Email地址</param>
        /// <param name="SendTitle">发送标题</param>
        /// <param name="SendBody">发送的内容</param>
        /// <param name="UserName">我的Email登录名</param>
        /// <param name="Password">我的Email密码</param>
        /// <param name="SmtpServer">Smtp邮件服务器</param>
        /// <param name="SmtpServerPort">Smtp邮件服务器端口</param>
        public static bool SendEmail(string SendTo, string EmailAddress, string SendTitle, string SendBody, string UserName, string Password, string SmtpServer, int SmtpServerPort)
        {
            try
            {
                MailMessage mailObj = new MailMessage();
                mailObj.From = new MailAddress(EmailAddress);//"发送邮箱地址"
                mailObj.To.Add(SendTo);//"接收邮箱地址"
                mailObj.Subject = SendTitle;// "主题"
                mailObj.Body = SendBody;// "内容"
                mailObj.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = SmtpServer;// "邮件服务器地址"
                smtp.Port = SmtpServerPort;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(UserName, Password);//"登录用户名","登录密码"
                smtp.Send(mailObj);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
