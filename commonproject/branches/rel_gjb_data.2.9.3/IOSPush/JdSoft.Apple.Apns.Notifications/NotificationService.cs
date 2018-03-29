using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JdSoft.Apple.Apns.Notifications
{
    using System.IO;

    public class NotificationService : IDisposable
    {
        #region Constants
        private const string hostSandbox = "gateway.sandbox.push.apple.com";
        private const string hostProduction = "gateway.push.apple.com";
        private const int apnsPort = 2195;
        #endregion

        #region Delegates and Events
        /// <summary>
        /// 处理一般例外
        /// </summary>
        /// <param name="sender">NotificationConnection实例产生的异常</param>
        /// <param name="ex">异常实例</param>
        public delegate void OnError(object sender, Exception ex);
        /// <summary>
        /// Occurs when a General Error is thrown
        /// </summary>
        public event OnError Error;

        /// <summary>
        /// 处理通知正在发送通知的有效载荷时太长太长异常
        /// </summary>
        /// <param name="sender">NotificationConnection实例产生的异常</param>
        /// <param name="ex">NotificationTooLongException实例</param>
        public delegate void OnNotificationTooLong(object sender, NotificationLengthException ex);
        /// <summary>
        ///正在发送一个通知时，会发生按照苹果的规格有长度超过可允许限度的256字节的有效载荷
        /// </summary>
        public event OnNotificationTooLong NotificationTooLong;

        /// <summary>
        ///把手坏的设备令牌例外，提供的设备令牌时，是不正确的长度
        /// </summary>
        /// <param name="sender">NotificatioConnection实例产生的异常</param>
        /// <param name="ex">BadDeviceTokenException实例</param>
        public delegate void OnBadDeviceToken(object sender, BadDeviceTokenException ex);
        /// <summary>
        /// 当发生指定的设备令牌长度是不正确的
        /// </summary>
        public event OnBadDeviceToken BadDeviceToken;

        /// <summary>
        /// 处理成功通知发送事件
        /// </summary>
        /// <param name="sender">NotificationConnection实例</param>
        /// <param name="notification">通知对象，被送往</param>
        public delegate void OnNotificationSuccess(object sender, Notification notification);
        /// <summary>
        ///通知时，已成功发送到苹果公司的服务器发生
        /// </summary>
        public event OnNotificationSuccess NotificationSuccess;

        /// <summary>
        ///处理失败通知交付
        /// </summary>
        /// <param name="sender">NotificationConnection实例</param>
        /// <param name="failed">通知对象发送失败</param>
        public delegate void OnNotificationFailed(object sender, Notification failed);
        /// <summary>
        /// 当一个通知已经发送到苹果公司的服务器失败时发生。这事件引发的NotificationConnection后曾试图重新发送该通知指定的SendRetries。
        /// </summary>
        public event OnNotificationFailed NotificationFailed;

        /// <summary>
        /// 处理连接事件
        /// </summary>
        /// <param name="sender">NotificationConnection实例</param>
        public delegate void OnConnecting(object sender);
        /// <summary>
        ///当发生连接到苹果服务器
        /// </summary>
        public event OnConnecting Connecting;

        /// <summary>
        ///处理关事件
        /// </summary>
        /// <param name="sender">NotificationConnection Instance</param>
        public delegate void OnConnected(object sender);
        /// <summary>
        ///当成功连接到苹果公司的服务器通过SSL认证发生
        /// </summary>
        public event OnConnected Connected;

        /// <summary>
        /// 手柄断开事件
        /// </summary>
        /// <param name="sender">NotificationConnection实例</param>
        public delegate void OnDisconnected(object sender);
        /// <summary>
        /// 当连接到苹果的服务器已经失去了发生
        /// </summary>
        public event OnDisconnected Disconnected;
        #endregion

        #region Instance Variables
        private List<NotificationConnection> notificationConnections = new List<NotificationConnection>();
        private Random rand = new Random((int)DateTime.Now.Ticks);
        private int sequential = 0;
        private int reconnectDelay = 5000;
        private int sendRetries = 1;

        private bool closing = false;
        private bool disposing = false;
        #endregion

        #region Constructors

        ///公升; summarygt
        ///构造
        /// </摘要>
        ///的<param name="host">推送通知网关主机</参数>
        //<param name="port">推送通知网关端口</参数>
        // /<param name="p12File"> PKCS12。P12或PFX文件包含公钥和私钥</参数>
        ///<param name="connections">的数目APNS连接开始</参数>
        public NotificationService(string host, int port, string p12File, int connections)
            : this(host, port, p12File, null, connections)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        ///的<param name="host">推送通知网关主机</参数>
        ///<param name="port">推送通知网关端口</参数>
        ///<param name="p12File"> PKCS12。P12或PFX文件包含公钥和私钥</参数>
        ///<param name="p12FilePassword">的用密码保护p12File的</参数>
        ///<param name="connections">的数目APNS连接开始</参数>
        public NotificationService(string host, int port, string p12File, string p12FilePassword, int connections)
            : this(host, port, System.IO.File.ReadAllBytes(p12File), p12FilePassword, connections)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">Push Notification Gateway Host</param>
        /// <param name="port">Push Notification Gateway Port</param>
        /// <param name="p12FileBytes">PKCS12 .p12 or .pfx File containing Public and Private Keys</param>
        /// <param name="p12FilePassword">Password protecting the p12File</param>
        /// <param name="connections">Number of Apns Connections to start with</param>
        public NotificationService(string host, int port, byte[] p12FileBytes, string p12FilePassword, int connections)
        {
            this.SendRetries = 1;
            closing = false;
            disposing = false;
            Host = host;
            Port = port;
            P12FileBytes = p12FileBytes;
            P12FilePassword = p12FilePassword;
            DistributionType = NotificationServiceDistributionType.Sequential;
            Connections = connections;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sandbox">布尔标志，表示默认沙箱或生产主机和端口是否应该使用</param>
        /// <param name="p12File">PKCS12。P12或PFX文件包含公共密钥和私人密钥</param>
        /// <param name="connections">Number of Apns Connections to start with</param>
        public NotificationService(bool sandbox, string p12File, int connections)
            : this(sandbox ? hostSandbox : hostProduction, apnsPort, p12File, null, connections)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sandbox">Boolean flag indicating whether the default Sandbox or Production Host and Port should be used</param>
        /// <param name="p12File">PKCS12 .p12 or .pfx File containing Public and Private Keys</param>
        /// <param name="p12FilePassword">Password protecting the p12File</param>
        /// <param name="connections">Number of Apns Connections to start with</param>
        public NotificationService(bool sandbox, string p12File, string p12FilePassword, int connections) :
            this(sandbox ? hostSandbox : hostProduction, apnsPort, p12File, p12FilePassword, connections)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sandbox">Boolean flag indicating whether the default Sandbox or Production Host and Port should be used</param>
        /// <param name="p12File">PKCS12 .p12 or .pfx File containing Public and Private Keys</param>
        /// <param name="p12FilePassword">Password protecting the p12File</param>
        /// <param name="connections">Number of Apns Connections to start with</param>
        public NotificationService(bool sandbox, byte[] p12FileBytes, string p12FilePassword, int connections) :
            this(sandbox ? hostSandbox : hostProduction, apnsPort, p12FileBytes, p12FilePassword, connections)
        {
        }

        /// <summary>
        /// Constructor	
        /// </summary>
        /// <param name="host">Push Notification Gateway Host</param>
        /// <param name="port">Push Notification Gateway Port</param>
        /// <param name="p12FileStream">Stream to PKCS12 .p12 or .pfx file containing Public and Private Keys</param>
        /// <param name="p12FilePassword">Password protecting the p12File</param>
        /// <param name="connections">Number of Apns Connections to start with</param>
        public NotificationService(bool sandbox, Stream p12FileStream, string p12FilePassword, int connections) :
            this(sandbox ? hostSandbox : hostProduction, apnsPort, getAllBytesFromStream(p12FileStream), p12FilePassword, connections)
        {
        }

        #endregion

        #region Properties
        /// <summary>
        ///这种情况下的唯一标识符
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取或设置等待的毫秒数，如果连接丢失或失败，然后重新连接到APNS主机
        /// </summary>
        public int ReconnectDelay
        {
            get { return reconnectDelay; }
            set
            {
                reconnectDelay = value;

                foreach (NotificationConnection con in notificationConnections)
                    con.ReconnectDelay = reconnectDelay;
            }
        }

        /// <summary>
        /// 获取或设置的次数尽量重新发送通知前NotificationFailed事件的提出
        /// </summary>
        public int SendRetries
        {
            get { return sendRetries; }
            set
            {
                sendRetries = value;

                foreach (NotificationConnection con in notificationConnections)
                    con.SendRetries = sendRetries;
            }
        }

        /// <summary>
        /// 获取或设置所使用的方法，在所有开放的通知分发排队APNS连接
        /// </summary>
        public NotificationServiceDistributionType DistributionType
        {
            get;
            set;
        }

        /// <summary>
        /// 获取的PKCS12 P12或PFX文件正在使用
        /// </summary>
        public byte[] P12FileBytes
        {
            get;
            private set;
        }

        private string P12FilePassword;

        /// <summary>
        /// 获取推送通知网关主机
        /// </summary>
        public string Host
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取推送通知网关端口
        /// </summary>
        public int Port
        {
            get;
            private set;
        }

        /// <summary>
        ///对于任何使用请你:)
        /// </summary>
        public object Tag
        {
            get;
            set;
        }

        /// <summary>
        ///获取或设置APNS在使用中的连接数。更改此属性将动态地改变使用中的连接数。如果下降，连接将被关闭（等待队列先清空），或如果提出，将添加新的连接。
        /// </summary>
        public int Connections
        {
            get
            {
                return notificationConnections.Count;
            }
            set
            {
                //不想0连接或更少
                if (value <= 0)
                    return;

                //三角洲
                int difference = value - notificationConnections.Count;

                if (difference > 0)
                {
                    //需要补充的连接
                    for (int i = 0; i < difference; i++)
                    {
                        NotificationConnection newCon = new NotificationConnection(Host, Port, P12FileBytes, P12FilePassword);
                        newCon.SendRetries = SendRetries;
                        newCon.ReconnectDelay = ReconnectDelay;

                        newCon.Error += new NotificationConnection.OnError(newCon_Error);
                        newCon.NotificationFailed += new NotificationConnection.OnNotificationFailed(newCon_NotificationFailed);
                        newCon.NotificationTooLong += new NotificationConnection.OnNotificationTooLong(newCon_NotificationTooLong);
                        newCon.NotificationSuccess += new NotificationConnection.OnNotificationSuccess(newCon_NotificationSuccess);
                        newCon.Connecting += new NotificationConnection.OnConnecting(newCon_Connecting);
                        newCon.Connected += new NotificationConnection.OnConnected(newCon_Connected);
                        newCon.Disconnected += new NotificationConnection.OnDisconnected(newCon_Disconnected);
                        newCon.BadDeviceToken += new NotificationConnection.OnBadDeviceToken(newCon_BadDeviceToken);
                        notificationConnections.Add(newCon);
                    }

                }
                else if (difference < 0)
                {
                    //需要补充的连接
                    for (int i = 0; i < difference * -1; i++)
                    {
                        if (notificationConnections.Count > 0)
                        {
                            NotificationConnection toClose = notificationConnections[0];
                            notificationConnections.RemoveAt(0);

                            toClose.Close();
                            toClose.Dispose();
                            toClose = null;
                        }
                    }
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 一个通知队列APNS连接使用属性所指定的DistributionType之一。
        /// </summary>
        /// <param name="notification">通知对象发送</param>
        /// <returns>如果为true，通知已成功排队</returns>
        public bool QueueNotification(Notification notification)
        {
            bool queued = false;

            if (!disposing && !closing)
            {
                int tries = 0;

                while (tries < SendRetries && !queued)
                {
                    if (DistributionType == NotificationServiceDistributionType.Sequential)
                        queued = queueSequential(notification);
                    else if (DistributionType == NotificationServiceDistributionType.Random)
                        queued = queueRandom(notification);

                    tries++;
                }
            }

            return queued;
        }

        /// <summary>
        ///关闭所有连接APNS，但首先等待所有排队的每个通知APNS连接被发送。这将导致QueueNotification始终返回false，调用此方法后。
        /// </summary>
        public void Close()
        {
            closing = true;

            foreach (NotificationConnection con in notificationConnections)
                con.Close();
        }

        /// <summary>
        ///  关闭所有连接APNS在每个排队的通知，而无需等待APNS连接被发送。这将导致QueueNotification始终返回false，调用此方法后。
        /// </summary>
        public void Dispose()
        {
            disposing = true;

            foreach (NotificationConnection con in notificationConnections)
                con.Dispose();
        }
        #endregion

        #region Private Methods
        void newCon_NotificationSuccess(object sender, Notification notification)
        {
            var onNotificationSuccess = NotificationSuccess;
            if (onNotificationSuccess != null)
                NotificationSuccess(sender, notification);
        }

        void newCon_NotificationTooLong(object sender, NotificationLengthException ex)
        {
            var onNotificationTooLong = NotificationTooLong;
            if (onNotificationTooLong != null)
                NotificationTooLong(sender, ex);
        }

        void newCon_BadDeviceToken(object sender, BadDeviceTokenException ex)
        {
            var onBadDeviceToken = BadDeviceToken;
            if (onBadDeviceToken != null)
                BadDeviceToken(this, ex);
        }

        void newCon_NotificationFailed(object sender, Notification failed)
        {
            var onNotificationFailed = NotificationFailed;
            if (onNotificationFailed != null)
                NotificationFailed(sender, failed);
        }

        void newCon_Error(object sender, Exception ex)
        {
            var onError = Error;
            if (onError != null)
                Error(sender, ex);
        }

        void newCon_Disconnected(object sender)
        {
            var onDisconnected = Disconnected;
            if (onDisconnected != null)
                Disconnected(sender);
        }

        void newCon_Connected(object sender)
        {
            var onConnected = Connected;
            if (onConnected != null)
                Connected(sender);
        }

        void newCon_Connecting(object sender)
        {
            var onConnecting = Connecting;
            if (onConnecting != null)
                onConnecting(sender);
        }

        private bool queueSequential(Notification notification)
        {
            if (sequential > notificationConnections.Count - 1)
                sequential = 0;

            if (notificationConnections[sequential] != null)
                return notificationConnections[sequential++].QueueNotification(notification);

            return false;
        }

        private bool queueRandom(Notification notification)
        {
            int index = rand.Next(0, notificationConnections.Count - 1);

            if (notificationConnections[index] != null)
                return notificationConnections[index].QueueNotification(notification);

            return false;
        }

        private static byte[] getAllBytesFromStream(Stream s)
        {
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;

                while ((read = s.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);

                return ms.ToArray();
            }
        }
        #endregion
    }
}
