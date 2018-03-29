using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using System.Xml;
using System.IO;
using System.Threading;
using log4net;

namespace FxtDataAcquisition.Data
{
    public sealed class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory = null;
        private ISession _session;
        private ILog log = LogManager.GetLogger(typeof(NHibernateHelper));

        private static ISessionFactory GetSessionFactory(string factoryName = null)
        {
            string strNHibernate = string.Format("hibernate.{0}.cfg.xml", Transform);
            //NHibernate监控
            //if (Transform.ToLower().Equals("debug"))
            //    HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            var config = new Configuration().Configure(strNHibernate, factoryName);
            string connect = System.Configuration.ConfigurationManager.ConnectionStrings[factoryName].ConnectionString;
            config.Properties.Add("connection.connection_string", connect);
            var buildSessionFactory = config.BuildSessionFactory();
            return buildSessionFactory;
        }

        public ISession GetSession(string factoryName = null)
        {
            factoryName = string.IsNullOrEmpty(factoryName) ? "Default" : factoryName;
            if (_sessionFactory == null)
            {
                lock (this)
                {
                    if (_sessionFactory == null)
                        _sessionFactory = GetSessionFactory(factoryName);
                }
            }
            _session = _sessionFactory.OpenSession();
            return _session;
        }

        public void CloseSession()
        {
            if (_session != null && _session.IsOpen)
            {
                _session.Close();
                _session = null;
            }
        }

        public void CloseSessionFactory()
        {
            if (_sessionFactory != null)
            {
                _sessionFactory.Dispose();
                _sessionFactory = null;
            }
        }

        /// <summary>
        /// 获得运行模式
        /// </summary>
        static string Transform
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Transform"];
            }
        }
        //private static ISessionFactory sessionFactory;

        //private static ISession currentSession;
        //private static object lockHelper = new object();

        //public static ISession GetCurrentSession(string factoryName = null)
        //{
        //    sessionFactory = GetSessionFactory(factoryName);
        //    currentSession = sessionFactory.OpenSession();
        //    return currentSession;
        //}

        //static ISessionFactory GetSessionFactory(string factoryName = null)
        //{
        //    string strTransform = System.Configuration.ConfigurationManager.AppSettings["Transform"];
        //    string strNHibernate = string.Format("hibernate.{0}.cfg.xml", strTransform);

        //    factoryName = string.IsNullOrEmpty(factoryName) ? "Defalut" : factoryName;
        //    var config = new Configuration().Configure(strNHibernate, factoryName);
        //    //NHibernate监控
        //    if (strTransform.ToLower().Equals("debug"))
        //        HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

        //    return config.BuildSessionFactory();
        //}
    }

    public static class ConfigurationExtensions
    {
        /// <summary>
        ///                Configure NHibernate from a specified session-factory.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="fileName">System location of '.cfg.xml' configuration file.</param>
        /// <param name="factoryName">Name value of the desired session-factory.</param>
        /// <returns></returns>
        public static Configuration Configure(this Configuration config, string fileName, string factoryName)
        {


            // Load Configuration XML
            XmlDocument doc = new XmlDocument();
            doc.Load(string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, fileName));


            return config.Configure(PrepareConfiguration(doc, factoryName));
        }
        /// <summary>
        ///                Configure NHibernate from a specified session-factory.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="textReader">The XmlReader containing the hibernate-configuration.</param>
        /// <param name="factoryName">Name value of the desired session-factory.</param>
        /// <returns></returns>
        public static Configuration Configure(this Configuration config, XmlReader textReader, string factoryName)
        {
            // Load Configuration XML
            XmlDocument doc = new XmlDocument();
            doc.Load(textReader);


            return config.Configure(PrepareConfiguration(doc, factoryName));
        }
        /// <summary>
        ///                Parses the configuration xml and ensures the target session-factory is the only one included.
        /// </summary>
        /// <param name="doc">The XmlDocument containing the hibernate-configuration.</param>
        /// <param name="factoryName">Name value of the desired session-factory.</param>
        /// <returns></returns>
        private static XmlTextReader PrepareConfiguration(XmlDocument doc, string factoryName)
        {
            const string CONFIG_XSD_MUTATION = "-x-factories";

            // Add Proper Namespace
            XmlNamespaceManager namespaceMgr = new XmlNamespaceManager(doc.NameTable);
            namespaceMgr.AddNamespace("nhibernate", "urn:nhibernate-configuration-2.2" + CONFIG_XSD_MUTATION);


            // Query Elements
            XmlNode nhibernateNode = doc.SelectSingleNode("descendant::nhibernate:hibernate-configuration", namespaceMgr);


            if (nhibernateNode != null)
            {
                if (nhibernateNode.SelectSingleNode("descendant::nhibernate:session-factory[@name='" + factoryName + "']", namespaceMgr) != default(XmlNode))
                    foreach (XmlNode node in nhibernateNode.SelectNodes("descendant::nhibernate:session-factory[@name!='" + factoryName + "']", namespaceMgr)) nhibernateNode.RemoveChild(node);
                else
                    throw new Exception("<session-factory name=\"" + factoryName + "\"> element was not found in the configuration file.");
            }
            else
                throw new Exception("<hibernate-configuration xmlns=\"urn:nhibernate-configuration-2.2-x-factories\"> element was not found in the configuration file.");


            return new XmlTextReader(new StringReader(nhibernateNode.OuterXml.Replace(CONFIG_XSD_MUTATION, "")));
        }


    }
}
