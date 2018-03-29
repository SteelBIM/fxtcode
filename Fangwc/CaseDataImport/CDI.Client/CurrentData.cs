using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CDI.Models;
using System.Runtime.Serialization;

namespace CDI.Client
{
    internal class CurrentData
    {
        #region Fields
        string epName = "EndPointHttp";
        ILog logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);
        IProxy proxyServer = new WCFProxy();

        #endregion

        private CurrentData()
        {
            
        }
        class UniqueInstance
        {
            internal static CurrentData instance;
            static UniqueInstance()
            {
                instance = new CurrentData();
            }
        }
        public static CurrentData Instance
        {
            get
            {
                return UniqueInstance.instance;
            }
        }

        
        public string EndPointName
        {
            get
            {
                var s = ConfigurationManager.AppSettings["UsedProtocol"];
                if (string.Compare(s,"tcp",true)==0)
                {
                    epName = "EndPointTCP";
                }
                else if(string.Compare(s,"https",true)==0)
                {
                    epName = "EndPointHttps";
                }
                else if (string.Compare(s, "http", true) == 0)
                {
                    epName = "EndPointHttp";
                }
                else
                {
                    epName = "EndPointHttp";
                }
                return epName;
            }
        }

        public ILog Logger
        {
            get
            {
                return logger;
            }
        }

        public string UserName { get; set; }

        public TokenRequestModel Token { get; set; }
        
        public IProxy ProxyServer
        {
            get
            {
                return proxyServer;
            }
        }

        public void LogOff()
        {
            if (Token != null)
            {
                lock (this)
                {
                    if (Token != null)
                    {
                        try
                        {
                            var r = proxyServer.Logout(Token);
                            if (r.Status == 1)
                                Token = null;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }

    

}
