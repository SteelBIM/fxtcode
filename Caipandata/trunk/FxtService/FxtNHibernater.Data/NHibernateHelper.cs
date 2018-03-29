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

/**
 * 作者: 李晓东
 * 时间: 2013.11.27
 * 摘要: 新建NHibernater的Session会话工厂(Factory)帮助类
 *       2013.12.17 修改人:李晓东
 *                 新增声明NHibernateHelper(string factoryName) 
 *                 当前文件新建类ConfigurationExtensions对Configuration的扩展
 *       2014.01.21 修改人:李晓东
 *                  修改GetCurrentSession
 *       2014.03.11 修改人:李晓东 
 *                  修改:NHibernateHelper中获取连接方式
 * **/
namespace FxtNHibernater.Data
{
    public sealed class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory = null;
        private static ISession _session;
        static NHibernateHelper()
        {

        }

        private static ISessionFactory GetSessionFactory(string factoryName = null)
        {
            string strNHibernate = string.Format("hibernate.{0}.cfg.xml", Transform);
            //NHibernate监控
            //if (Transform.ToLower().Equals("debug"))
            //    HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            var config = new Configuration().Configure(strNHibernate, factoryName);
            var buildSessionFactory = config.BuildSessionFactory();
            return buildSessionFactory;
        }

        public static ISession GetSession(string factoryName = null)
        {
            factoryName = string.IsNullOrEmpty(factoryName) ? "Default" : factoryName;
            _sessionFactory = GetSessionFactory(factoryName);
            _session = _sessionFactory.OpenSession();
            return _session;
        }

        public static void CloseSession()
        {
            if (_session != null)
            {
                if (_session.IsOpen)
                {
                    //_session.Flush();
                    //_session.Clear();
                    _session.Close();
                }
                _session = null;
            }
            if (_sessionFactory != null)
            {
                _sessionFactory.Close();
                _sessionFactory.Dispose();
            }
            _sessionFactory = null;
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
