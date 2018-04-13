using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using CBSS.Core.Utility.Models;

namespace CBSS.Core.Utility
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SerializationType”的 XML 注释
    public enum SerializationType
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SerializationType”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SerializationType.Xml”的 XML 注释
        Xml,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SerializationType.Xml”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SerializationType.Json”的 XML 注释
        Json,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SerializationType.Json”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SerializationType.DataContract”的 XML 注释
        DataContract,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SerializationType.DataContract”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“SerializationType.Binary”的 XML 注释
        Binary
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“SerializationType.Binary”的 XML 注释
    }

    [System.Serializable]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper”的 XML 注释
    public static class XMLHelper
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper”的 XML 注释
    {
        /// <summary>
        /// 获取xml指定节点
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static string GetAppSetting(string settingName)
        {
            XmlDocument doc = XMLLoad("Config/SettingConfig.xml");
            var re = doc.SelectSingleNode(@"//" + settingName);
            return re.InnerText;
        }

#pragma warning disable CS1573 // 参数“filePath”在“XMLHelper.GetAppSetting(string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        /// <summary>
        /// 获取xml指定节点
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        public static string GetAppSetting(string filePath, string settingName)
#pragma warning restore CS1573 // 参数“filePath”在“XMLHelper.GetAppSetting(string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            XmlDocument doc = XMLLoad(filePath);
            var re = doc.SelectSingleNode(@"//" + settingName);
            return re.InnerText;
        }

#pragma warning disable CS1573 // 参数“subnodeName”在“XMLHelper.GetAppSetting(string, string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning disable CS1571 // XML 注释中对“settingName”有重复的 param 标记
        /// <summary>
        /// 根据xml文件路径，获取指定节点
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="settingName">节点字段</param>
        ///  /// <param name="settingName">子节点字段</param>
        /// <returns></returns>
        public static string GetAppSetting(string filePath, string settingName, string subnodeName)
#pragma warning restore CS1571 // XML 注释中对“settingName”有重复的 param 标记
#pragma warning restore CS1573 // 参数“subnodeName”在“XMLHelper.GetAppSetting(string, string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            XmlDocument doc = XMLLoad(filePath);
            XmlNode re = doc.SelectSingleNode(@"//" + settingName);
            if (re != null)
            {
                var name = re.SelectSingleNode(@"//" + subnodeName);
                if (name != null)
                {
                    return name.InnerText;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

#pragma warning disable CS1572 // XML 注释中有“settingName”的 param 标记，但是没有该名称的参数
#pragma warning disable CS1573 // 参数“secondNode”在“XMLHelper.GetAppSettingList(string, string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning disable CS1573 // 参数“parentNode”在“XMLHelper.GetAppSettingList(string, string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning disable CS1572 // XML 注释中有“settingName”的 param 标记，但是没有该名称的参数
        /// <summary>
        /// 根据xml文件路径，获取指定节点
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="settingName">节点字段</param>
        ///  /// <param name="settingName">子节点字段</param>
        /// <returns></returns>
        public static List<string> GetAppSettingList(string filePath, string parentNode, string secondNode)
#pragma warning restore CS1572 // XML 注释中有“settingName”的 param 标记，但是没有该名称的参数
#pragma warning restore CS1573 // 参数“parentNode”在“XMLHelper.GetAppSettingList(string, string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning restore CS1573 // 参数“secondNode”在“XMLHelper.GetAppSettingList(string, string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning restore CS1572 // XML 注释中有“settingName”的 param 标记，但是没有该名称的参数
        {
            List<string> list = new List<string>();
            XmlDocument doc = XMLLoad(filePath);
            XmlNode re = doc.SelectSingleNode(@"//" + parentNode);
            if (re != null)
            {
                var name = re.SelectSingleNode(@"//" + secondNode);
                if (name != null&& name.ChildNodes.Count>0)
                {
                    for(int i=0;i<name.ChildNodes.Count;i++)
                    {
                        list.Add(name.ChildNodes[i].InnerText);
                    }
                    return list;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }



        /// <summary>
        /// 读取指定xml路径,获取字节点，并转换成实体
        /// </summary>
        public static Dictionary<string, T> Read<T>(string xmlPath, string parentNode)
        {
            Dictionary<string, T> list = new Dictionary<string, T>();
            XmlDocument doc = XMLLoad(xmlPath);
            var xn = doc.SelectNodes(parentNode);
            if (xn.Count > 0)
            {
                var cxn = xn[0].ChildNodes;
                Type type = typeof(T);
                for (int i = 0; i < cxn.Count; i++)
                {
                    var attrs = cxn[i].Attributes;
                    string fullName = type.Namespace + "." + type.Name;
                    object obj = type.Assembly.CreateInstance(fullName);
                    for (int j = 0; j < attrs.Count; j++)
                    {
                        var proc = type.GetProperty(attrs[j].Name);
                        proc.SetValue(obj, attrs[j].Value);
                    }
                    list.Add(cxn[i].Name, (T)obj);
                }
            }
            return list;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.XMLLoad(string)”的 XML 注释
        public static XmlDocument XMLLoad(string xmlPath)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.XMLLoad(string)”的 XML 注释
        {
            XmlDocument xmldoc = new XmlDocument();
            string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + xmlPath;
            if (File.Exists(filename)) xmldoc.Load(filename);
            return xmldoc;
        }

        #region ========== XmlSerializer ==========
        /// <summary>
        /// 序列化，使用标准的XmlSerializer，优先考虑使用。
        /// 不能序列化IDictionary接口.
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void XmlSerialize(object obj, string filename)
        {
            FileStream fs = null;
            // serialize it...
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }



        /// <summary>
        /// 反序列化，使用标准的XmlSerializer，优先考虑使用。
        /// 不能序列化IDictionary接口.
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns>type类型的对象实例</returns>
        public static object XmlDeserializeFromFile(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream...
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.ToXml<T>(T)”的 XML 注释
        public static string ToXml<T>(this T objectToSerialize)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.ToXml<T>(T)”的 XML 注释
        {
            using (var writer = new StringWriter())
            {
                new XmlSerializer(typeof(T)).Serialize(writer, objectToSerialize);

                return writer.ToString();
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        public static T FromXml<T>(this string xmlDocument)
        {
            using (StringReader reader = new StringReader(xmlDocument))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                var obj = serializer.Deserialize(reader);
                if (obj is T)
                {
                    return (T)obj;
                }

                return default(T);
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.XmlSerialize(object, Encoding)”的 XML 注释
        public static string XmlSerialize(object obj, System.Text.Encoding ecoding)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.XmlSerialize(object, Encoding)”的 XML 注释
        {
            if (obj == null)
            {
                return null;
            }
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();//制定编码和磁盘文件 
            StreamWriter sWriter = new StreamWriter(stream, ecoding);
            XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
            //empty namespaces
            xsn.Add(String.Empty, String.Empty);

            ser.Serialize(sWriter, obj, xsn);//序列化 

            string str = ecoding.GetString(stream.ToArray());

            stream.Close();

            return str;

        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.XmlSerialize(object)”的 XML 注释
        public static string XmlSerialize(object obj)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.XmlSerialize(object)”的 XML 注释
        {
            if (obj == null)
            {
                return null;
            }
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            StringWriter sWriter = new StringWriter();
            ser.Serialize(sWriter, obj);
            return sWriter.ToString();
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.XmlDeserialize(Type, string)”的 XML 注释
        public static object XmlDeserialize(Type type, string xmlStr)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.XmlDeserialize(Type, string)”的 XML 注释
        {
            if (xmlStr == null || xmlStr.Trim() == "")
            {
                return null;
            }
            XmlSerializer ser = new XmlSerializer(type);
            StringReader sWriter = new StringReader(xmlStr);
            return ser.Deserialize(sWriter);
        }


#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.SelectSingleNodeText<TText>(XmlElement, string, XmlNamespaceManager)”的 XML 注释
        public static XmlNodeComponent<TText> SelectSingleNodeText<TText>(this XmlElement xmlElement, string xPath, XmlNamespaceManager ns = null)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.SelectSingleNodeText<TText>(XmlElement, string, XmlNamespaceManager)”的 XML 注释
        {
            var node = default(XmlNode);

            if (ns != null)
            {
                node = xmlElement.SelectSingleNode(xPath, ns);
            }
            else
            {
                node = xmlElement.SelectSingleNode(xPath);
            }

            var innerText = new XmlNodeComponent<TText>
            {
                Node = node
            };

            if (node != default(XmlNode) && !string.IsNullOrWhiteSpace(node.InnerText))
            {
                innerText.Value = node.InnerText.As<TText>();
            }

            return innerText;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.SelectXmlNodeText<TText>(XmlNode, string, XmlNamespaceManager)”的 XML 注释
        public static XmlNodeComponent<TText> SelectXmlNodeText<TText>(this XmlNode xmlElement, string xPath, XmlNamespaceManager ns = null)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.SelectXmlNodeText<TText>(XmlNode, string, XmlNamespaceManager)”的 XML 注释
        {
            var node = default(XmlNode);

            if (ns != null)
            {
                node = xmlElement.SelectSingleNode(xPath, ns);
            }
            else
            {
                node = xmlElement.SelectSingleNode(xPath);
            }

            var innerText = new XmlNodeComponent<TText>
            {
                Node = node
            };

            if (node != default(XmlNode) && !string.IsNullOrWhiteSpace(node.InnerText))
            {
                innerText.Value = node.InnerText.As<TText>();
            }

            return innerText;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.SelectNodesText<TText>(XmlElement, string, XmlNamespaceManager)”的 XML 注释
        public static IEnumerable<XmlNodeComponent<TText>> SelectNodesText<TText>(this XmlElement xmlElement, string xPath, XmlNamespaceManager ns = null)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.SelectNodesText<TText>(XmlElement, string, XmlNamespaceManager)”的 XML 注释
        {
            var innerTexts = new List<XmlNodeComponent<TText>>();
            var nodes = default(XmlNodeList);

            if (ns != null)
            {
                nodes = xmlElement.SelectNodes(xPath, ns);
            }
            else
            {
                nodes = xmlElement.SelectNodes(xPath);
            }

            if (nodes != default(XmlNodeList) && nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    var node = new XmlNodeComponent<TText>
                    {
                        Node = nodes.Item(i)
                    };
                    var text = nodes.Item(i).InnerText;

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        node.Value = text.As<TText>();
                    }

                    innerTexts.Add(node);
                }
            }

            return innerTexts;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.SelectNodeAttributeValue<TText>(XmlElement, string, string, XmlNamespaceManager)”的 XML 注释
        public static TText SelectNodeAttributeValue<TText>(this XmlElement xmlElement, string xPath, string attributeName, XmlNamespaceManager ns = null)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.SelectNodeAttributeValue<TText>(XmlElement, string, string, XmlNamespaceManager)”的 XML 注释
        {
            var attributeValue = default(TText);
            var node = default(XmlElement);

            if (ns != null)
            {
                node = xmlElement.SelectSingleNode(xPath, ns) as XmlElement;
            }
            else
            {
                node = xmlElement.SelectSingleNode(xPath) as XmlElement;
            }

            if (node != default(XmlElement))
            {
                attributeValue = node.GetAttribute(attributeName).As<TText>();
            }

            return attributeValue;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.GetNodeAttributeValue<TText>(XmlElement, string)”的 XML 注释
        public static TText GetNodeAttributeValue<TText>(this XmlElement xmlElement, string attributeName)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.GetNodeAttributeValue<TText>(XmlElement, string)”的 XML 注释
        {
            var attributeValue = default(TText);

            if (xmlElement != null)
            {
                attributeValue = xmlElement.GetAttribute(attributeName).As<TText>();
            }

            return attributeValue;
        }

        #endregion

        #region ========== DataContractSerializer ==========
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.DataContractSerialize(object)”的 XML 注释
        public static string DataContractSerialize(object o)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.DataContractSerialize(object)”的 XML 注释
        {
            if (o == null)
            {
                return null;
            }
            MemoryStream stream = new MemoryStream();
            DataContractSerializer ser = new DataContractSerializer(o.GetType());
            ser.WriteObject(stream, o);

            string ret = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            return ret;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“XMLHelper.DataContractDeserialize(Type, string)”的 XML 注释
        public static object DataContractDeserialize(Type type, string xmlStr)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“XMLHelper.DataContractDeserialize(Type, string)”的 XML 注释
        {
            if (xmlStr == null || xmlStr.Trim() == "")
            {
                return null;
            }
            DataContractSerializer ser = new DataContractSerializer(type);
            MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlStr));//new StringReader(xmlStr);
            return ser.ReadObject(stream);
        }

        #endregion

        #region ========== BinaryBytes ==========
        /// <summary>
        /// 将对象使用二进制格式序列化成byte数组
        /// </summary>
        /// <param name="obj">待保存的对象</param>
        /// <returns>byte数组</returns>
        public static byte[] SaveToBinaryBytes(object obj)
        {
            //将对象序列化到MemoryStream中
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            //从MemoryStream中获取获取byte数组
            return ms.ToArray();
        }

        /// <summary>
        /// 将使用二进制格式保存的byte数组反序列化成对象
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <returns>对象</returns>
        public static object LoadFromBinaryBytes(byte[] bytes)
        {
            object result = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (bytes != null)
            {
                MemoryStream ms = new MemoryStream(bytes);
                result = formatter.Deserialize(ms);
            }
            return result;
        }
        #endregion

        #region ========= other ==========
        /// <summary>
        /// 使用BinaryFormatter将对象系列化到MemoryStream中。
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>保存对象的MemoryStream</returns>
        public static MemoryStream SaveToMemoryStream(object obj)
        {
            MemoryStream ms = new MemoryStream();
            BufferedStream stream = new BufferedStream(ms);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            return ms;
        }

        #endregion

        #region ========== JSONSerializer ==========
        /// <summary>
        /// 将C#数据实体转化为JSON数据
        /// </summary>
        /// <param name="obj">要转化的数据实体</param>
        /// <returns>JSON格式字符串</returns>
        public static string JsonSerialize<T>(T obj)
        {
            return JsonSerialize(obj, Encoding.UTF8);
        }

#pragma warning disable CS1573 // 参数“encoding”在“XMLHelper.JsonSerialize(object, Encoding)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        /// <summary>
        /// 将C#数据实体转化为JSON数据
        /// </summary>
        /// <param name="obj">要转化的数据实体</param>
        /// <returns>JSON格式字符串</returns>
        public static string JsonSerialize(object obj, Encoding encoding)
#pragma warning restore CS1573 // 参数“encoding”在“XMLHelper.JsonSerialize(object, Encoding)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            if (obj == null)
            {
                return null;
            }
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            stream.Position = 0;

            StreamReader sr = new StreamReader(stream, encoding);
            string resultStr = sr.ReadToEnd();
            sr.Close();
            stream.Close();

            return resultStr;
        }


        /// <summary>
        /// 将JSON数据转化为C#数据实体
        /// </summary>
        /// <param name="json">符合JSON格式的字符串</param>
        /// <returns>T类型的对象</returns>
        public static T JsonDeserialize<T>(string json)
        {
            return (T)JsonDeserialize(typeof(T), json);
        }

#pragma warning disable CS1573 // 参数“type”在“XMLHelper.JsonDeserialize(Type, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        /// <summary>
        /// 将JSON数据转化为C#数据实体
        /// </summary>
        /// <param name="json">符合JSON格式的字符串</param>
        /// <returns>T类型的对象</returns>
        public static object JsonDeserialize(Type type, string json)
#pragma warning restore CS1573 // 参数“type”在“XMLHelper.JsonDeserialize(Type, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            if (json == null)
            {
                return null;
            }
            //json 必须为 {name:"value",name:"value"} 的格式(要符合JSON格式要求)
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
            MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json.ToCharArray()));
            object obj = serializer.ReadObject(ms);
            ms.Close();

            return obj;
        }
        #endregion

    }
}