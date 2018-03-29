using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JdSoft.Apple.Apns.Notifications;

namespace IOSPush
{
   /// <summary>
   ///IOS设备消息推送 
   /// </summary>
   public class PushNotificationForIos
    {
        /// <summary>
        /// 推送服务方法
        /// </summary>
        /// <param name="strDeviceToken">手机UDID</param>
        /// <param name="strContent">推送内容</param>
        /// <param name="strCertificate">推送服务用证书名称</param>
       public static string pushNotifications(string strDeviceToken, string strContent, string strCertificate, ref int issucess,string entrustid)
       {
           string result = "";
           bool sandbox = true;
           string testDeviceToken = strDeviceToken;
           string p12File = strCertificate;
           string p12FilePassword = "19050000";// "yang123"
           string p12Filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12File);
           NotificationService service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);
           service.SendRetries = 5; //5 retries before generating notificationfailed event
           service.ReconnectDelay = 5000; //5 seconds
           service.Error += new NotificationService.OnError(service_Error);
           service.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);
           service.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
           service.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
           service.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
           service.Connecting += new NotificationService.OnConnecting(service_Connecting);
           service.Connected += new NotificationService.OnConnected(service_Connected);
           service.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);
           Notification alertNotification = new Notification(testDeviceToken);
           //设备后台接收参数
           alertNotification.Payload.AddCustom("customContent", entrustid);
           //通知内容
           alertNotification.Payload.Alert.Body = strContent;
           alertNotification.Payload.Alert.ActionLocalizedKey = "Open";
           alertNotification.Payload.Badge = 1;
           alertNotification.Payload.Sound = "default";
           alertNotification.Payload.Badge = 1;
           if (service.QueueNotification(alertNotification))
               result = "Notification Queued!";
           else
               result = "Notification Failed to be Queued!";
           service.Close();
           service.Dispose();
           return result;
       }

        static void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
        {
            Console.WriteLine("Bad Device Token: {0}", ex.Message);
        }

        static void service_Disconnected(object sender)
        {
            Console.WriteLine("Disconnected...");
        }

        static void service_Connected(object sender)
        {
            Console.WriteLine("Connected...");
        }

        static void service_Connecting(object sender)
        {
            Console.WriteLine("Connecting...");
        }

        static void service_NotificationTooLong(object sender, NotificationLengthException ex)
        {
            Console.WriteLine(string.Format("Notification Too Long: {0}", ex.Notification.ToString()));
        }

        static void service_NotificationSuccess(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Success: {0}", notification.ToString()));
        }

        static void service_NotificationFailed(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Failed: {0}", notification.ToString()));
        }

        static void service_Error(object sender, Exception ex)
        {
            Console.WriteLine(string.Format("Error: {0}", ex.Message));
        }



    }

}
