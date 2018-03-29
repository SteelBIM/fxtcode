using System.Reflection;
using System.Xml;

namespace CAS.Common
{
    /// <summary>
    /// wcf api公共入口helper
    /// </summary>
    public static class EntranceHelper
    {
        /// <summary>
        /// 根据配置获取类
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static MatchClass_CASCommon GetMatchClass(XmlDocument apiConfig, string key)
        {
            XmlNode xml = apiConfig.SelectSingleNode("/Match/Class[@Key='" + key + "']");
            if (xml == null)
            {
                return null;
            }
            string library = xml.Attributes["Library"].Value;
            string className = xml.Attributes["ClassName"].Value;
            return new MatchClass_CASCommon() { Key = key, ClassName = className, Library = library };
        }
        /// <summary>
        /// 根据配置获取方法
        /// 配置xml文件规则:
        /// &lt;Match&gt;
        ///      &lt;Class Key="A" Library="FxtDataAcquisition.API.Service" ClassName="FxtDataAcquisition.API.Service.FxtRunFlatsActualize.FxtRunFlats"&gt;
        ///        &lt;Method Key="test" MethodName="Test" /&gt;
        ///        &lt;Method Key="test2" MethodName="Test2" /&gt;
        ///      &lt;/Class&gt;
        ///    &lt;/Match&gt;
        ///  例子:public static XmlDocument ApiConfig = new XmlDocument();
        ///  ApiConfig.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MatchClass.xml"));
        /// </summary>
        /// <param name="apiConfig"></param>
        /// <param name="type"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(XmlDocument apiConfig,string type, string functionName, out object objClass)
        {
            objClass = null;
            MatchClass_CASCommon mc = GetMatchClass(apiConfig, type);
            if (mc == null)
            {
                return null;
            }
            objClass = System.Reflection.Assembly.Load(mc.Library).CreateInstance(mc.ClassName);
            if (objClass == null)
            {
                return null;
            }
            XmlNode xml = apiConfig.SelectSingleNode("/Match/Class[@Key='" + type + "']/Method[@Key='" + functionName + "']");
            if (xml == null)
            {
                return null;
            }
            string methodName = xml.Attributes["MethodName"].Value;
            MethodInfo method = objClass.GetType().GetMethod(methodName);
            return method;
        }
    }

    internal class MatchClass_CASCommon
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 类库名称
        /// </summary>
        public string Library { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }
    }
}
