using FXT.DataCenter.Infrastructure.Common.Common;

namespace FXT.DataCenter.Infrastructure.Common.CommonWeb
{
    /// <summary>
    /// 自定义XML配置文件辅助类(带缓存功能)
    /// </summary>
    public class CustomConfigHelper
    {
        // 定义XmlHelper
        private XmlHelper xmlHelper = null;
        private string fileName = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">要读取的配置文件</param>
        public CustomConfigHelper(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// 获得某节点的内容
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <returns>节点的值</returns>
        public string GetContent(string nodeName)
        {
            string key = fileName + nodeName;

            string content = (string)CacheHelper.GetCache(key);
            if (string.IsNullOrEmpty(content))
            {
                CreateXmlHelper();
                content = xmlHelper.GetContent(nodeName);
                CacheHelper.SetCache(key, content);
            }
            return content;
        }

        private void CreateXmlHelper()
        {
            if (xmlHelper == null)
                xmlHelper = new XmlHelper(fileName);
        }

        /// <summary>
        /// 设置节点的内容
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">要赋得值</param>
        public void SetContent(string nodeName, string nodeValue)
        {
            CreateXmlHelper();

            xmlHelper.SetContent(nodeName, nodeValue);

            string key = fileName + nodeName;
            CacheHelper.SetCache(key, nodeValue);
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            xmlHelper.Save();
        }
    }
}
