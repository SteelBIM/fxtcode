using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
namespace SqlTCP
{
    /// <summary>
    /// XML 操作基类
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// 读取Xml到DataSet中
        /// </summary>
        /// <param name="XmlPath">路径</param>
        /// <returns>结果集</returns>
        public static DataSet GetXml(string XmlPath)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(@XmlPath);
            return ds;
        }

        /// <summary>
        /// 读取xml文档并返回一个节点:适用于一级节点
        /// </summary>
        /// <param name="XmlPath">xml路径</param>
        /// <param name="NodeName">节点</param>
        /// <returns></returns>
        public static string ReadXmlReturnNode(string XmlPath, string Node)
        {
            XmlDocument docXml = new XmlDocument();
            docXml.Load(@XmlPath);
            XmlNodeList xn = docXml.GetElementsByTagName(Node);
            return xn.Item(0).InnerText.ToString();
        }

        /// <summary>
        /// 查找数据,返回当前节点的所有下级节点,填充到一个DataSet中
        /// </summary>
        /// <param name="xmlPath">xml文档路径</param>
        /// <param name="XmlPathNode">节点的路径:根节点/父节点/当前节点</param>
        /// <returns></returns>
        public static DataSet GetXmlData(string xmlPath, string XmlPathNode)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            DataSet ds = new DataSet();
            StringReader read = new StringReader(objXmlDoc.SelectSingleNode(XmlPathNode).OuterXml);
            ds.ReadXml(read);
            return ds;
        }

        /// <summary>
        /// 更新Xml节点内容
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="Node">要更换内容的节点:节点路径 根节点/父节点/当前节点</param>
        /// <param name="Content">新的内容</param>
        public static void XmlNodeReplace(string xmlPath, string Node, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            objXmlDoc.SelectSingleNode(Node).InnerText = Content;
            objXmlDoc.Save(xmlPath);

        }

        /// <summary>
        /// 更改节点的属性值
        /// </summary>
        /// <param name="xmlPath">文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <param name="NodeAttribute1">要更改的节点属性的名称</param>
        /// <param name="NodeAttributeText">更改的属性值</param>
        public static void XmlAttributeEdit(string xmlPath, string NodePath, string NodeAttribute1, string NodeAttributeText)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            XmlElement xe = (XmlElement)nodePath;
            xe.SetAttribute(NodeAttribute1, NodeAttributeText);
            objXmlDoc.Save(xmlPath);
        }

        /// <summary>
        /// 删除XML节点和此节点下的子节点
        /// </summary>
        /// <param name="xmlPath">xml文档路径</param>
        /// <param name="Node">节点路径</param>
        public static void XmlNodeDelete(string xmlPath, string Node)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            string mainNode = Node.Substring(0, Node.LastIndexOf("/"));
            objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
            objXmlDoc.Save(xmlPath);
        }

        /// <summary>
        /// 删除一个节点的属性
        /// </summary>
        /// <param name="xmlPath">文件路径</param>
        /// <param name="NodePath">节点路径（xpath）</param>
        /// <param name="NodeAttribute">属性名称</param>
        public static void xmlnNodeAttributeDel(string xmlPath, string NodePath, string NodeAttribute)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            XmlElement xe = (XmlElement)nodePath;
            xe.RemoveAttribute(NodeAttribute);
            objXmlDoc.Save(xmlPath);
        }

        /// <summary>
        /// 插入一个节点和此节点的子节点
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="MailNode">当前节点路径</param>
        /// <param name="ChildNode">新插入节点</param>
        /// <param name="Element">插入节点的子节点</param>
        /// <param name="Content">子节点的内容</param>
        public static void XmlInsertNode(string xmlPath, string MailNode, string ChildNode, string Element, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
            XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode);
            objRootNode.AppendChild(objChildNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objChildNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }

        /// <summary>
        /// 向一个节点添加属性
        /// </summary>
        /// <param name="xmlPath">xml文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <param name="NodeAttribute1">要添加的节点属性的名称</param>
        /// <param name="NodeAttributeText">要添加属性的值</param>
        public static void AddAttribute(string xmlPath, string NodePath, string NodeAttribute1, string NodeAttributeText)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlAttribute nodeAttribute = objXmlDoc.CreateAttribute(NodeAttribute1);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            nodePath.Attributes.Append(nodeAttribute);
            XmlElement xe = (XmlElement)nodePath;
            xe.SetAttribute(NodeAttribute1, NodeAttributeText);
            objXmlDoc.Save(xmlPath);
        }

        /// <summary>
        /// 插入一节点,带一属性
        /// </summary>
        /// <param name="xmlPath">Xml文档路径</param>
        /// <param name="MainNode">当前节点路径</param>
        /// <param name="Element">新节点</param>
        /// <param name="Attrib">属性名称</param>
        /// <param name="AttribContent">属性值</param>
        /// <param name="Content">新节点值</param>
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Attrib, string AttribContent, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }

        /// <summary>
        /// 插入一节点,不带属性
        /// </summary>
        /// <param name="xmlPath">Xml文档路径</param>
        /// <param name="MainNode">当前节点路径</param>
        /// <param name="Element">新节点</param>
        /// <param name="Content">新节点值</param>
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }

        /// <summary>
        /// 在根节点下添加父节点
        /// </summary>
        public static void AddParentNode(string xmlPath, string parentNode)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlPath);
            // 创建一个新的menber节点并将它添加到根节点下
            XmlElement Node = xdoc.CreateElement(parentNode);
            xdoc.DocumentElement.PrependChild(Node);
            xdoc.Save(xmlPath);
        }

        /// <summary>
        /// 根据节点属性读取子节点值(较省资源模式)
        /// </summary>
        /// <param name="XmlPath">xml路径</param>
        /// <param name="FatherElement">父节点值</param>
        /// <param name="AttributeName">属性名称</param>
        /// <param name="AttributeValue">属性值</param>
        /// <param name="ArrayLength">返回的数组长度</param>
        /// <returns></returns>
        public static System.Collections.ArrayList GetSubElementByAttribute(string XmlPath, string FatherElement, string AttributeName, string AttributeValue, int ArrayLength)
        {
            System.Collections.ArrayList al = new System.Collections.ArrayList();
            XmlDocument docXml = new XmlDocument();
            docXml.Load(@XmlPath);
            XmlNodeList xn;
            xn = docXml.DocumentElement.SelectNodes("//" + FatherElement + "[" + @AttributeName + "='" + AttributeValue + "']");
            XmlNodeList xx = xn.Item(0).ChildNodes;
            for (int i = 0; i < ArrayLength & i < xx.Count; i++)
            {

                al.Add(xx.Item(i).InnerText);
            }

            return al;
        }
    }
}