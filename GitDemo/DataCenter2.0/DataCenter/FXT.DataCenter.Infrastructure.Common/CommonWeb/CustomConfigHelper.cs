using FXT.DataCenter.Infrastructure.Common.Common;

namespace FXT.DataCenter.Infrastructure.Common.CommonWeb
{
    /// <summary>
    /// �Զ���XML�����ļ�������(�����湦��)
    /// </summary>
    public class CustomConfigHelper
    {
        // ����XmlHelper
        private XmlHelper xmlHelper = null;
        private string fileName = null;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="fileName">Ҫ��ȡ�������ļ�</param>
        public CustomConfigHelper(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// ���ĳ�ڵ������
        /// </summary>
        /// <param name="nodeName">�ڵ�����</param>
        /// <returns>�ڵ��ֵ</returns>
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
        /// ���ýڵ������
        /// </summary>
        /// <param name="nodeName">�ڵ�����</param>
        /// <param name="nodeValue">Ҫ����ֵ</param>
        public void SetContent(string nodeName, string nodeValue)
        {
            CreateXmlHelper();

            xmlHelper.SetContent(nodeName, nodeValue);

            string key = fileName + nodeName;
            CacheHelper.SetCache(key, nodeValue);
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Save()
        {
            xmlHelper.Save();
        }
    }
}
