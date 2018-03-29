using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace FXT.DataCenter.Infrastructure.Common.Common
{
    /// <summary>
    /// XML文件辅助类
    /// </summary>
    public class XmlHelper
    {
        // 定义XML文档
        private XmlDocument doc = null;
        // 定义路径
        private string fileName;

        /// <summary>
        /// XML文件路径
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }

        public XmlHelper(string fileName)
        {
            if (File.Exists(fileName))
            {
                // 产生XML文档
                doc = new XmlDocument();
                // 加载文件
                doc.Load(fileName);

                // 保存文件路径
                this.fileName = fileName;
            }
            else
            {
                throw new FileNotFoundException(fileName + "，XML文件不存在");
            }
        }

        // 保存文档
        public void Save()
        {
            // 保存
            doc.Save(fileName);
        }

        /// <summary>
        /// 设置XML文件某节点的值
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">节点值</param>
        public void SetContent(string nodeName, string nodeValue)
        {
            // 获得节点
            XmlNode node = GetNode(nodeName);

            // 设置值
            node.InnerText = nodeValue;
        }

        /// <summary>
        /// 获得XML文件某节点的值
        /// </summary>
        /// <param name="fileName">XML文件名</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>节点值</returns>
        public string GetContent(string nodeName)
        {
            // 获得指定的节点并获得值
            return GetNode(nodeName).InnerText;
        }


        private XmlNode GetNode(string nodeName)
        {
            // 查询节点
            XmlNode node = doc.SelectSingleNode("//" + nodeName);

            if (node == null)
            {
                throw new ApplicationException(nodeName + "，XML节点不存在");
            }
            else
            {
                return node;
            }
        }
    }
}
