using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FxtMCAS.Common
{
    #region 使用说明
    /*****
     * 目的：为提高消息模板，可阅读性
     * 使用说明：每一个消息都创建一个实体。消息实体主要有两部分组成：消息模板与要替换的属性
     * 消息实体继承MessageTemplateHelper。
     * 使用案例：
     * MessageTemplate_Bank bank = new MessageTemplate_Bank();
     * bank.age = "27";
     * bank.name = "曾曾";
     * result = bank.ReplaceTemplate();//完成内容替换
     * *******/
    #endregion
   
    /// <summary>
    /// 消息、操作记录模板帮助类
    /// hody 2015-04-14
    /// </summary>
    public class MessageTemplateHelper
    {
        /// <summary>
        /// 替换模板内容
        /// </summary>
        /// <returns></returns>
        public string ReplaceTemplate()
        {
            string template = string.Empty;
            try
            {
                PropertyInfo[] props = null;
                Type type = this.GetType();
                //获取类的属性
                props = type.GetProperties();
                string result = string.Empty;
                //获取模板内容
                FieldInfo conststring = type.GetField("template", BindingFlags.Public | BindingFlags.Static);
                template = conststring.GetValue(null).ToString();
                //替换模板内容
                for (int i = 0; i < props.Length; i++)
                {
                    string pattern = string.Format("<%={0}%>", props[i].Name);
                    Regex rgx = new Regex(pattern);
                    template = rgx.Replace(template, props[i].GetValue(this, null).ToString());
                }
                return template;
            }
            catch (Exception ex)
            {
                return "exception error";
            }
        }
    }
    #region 例子
    //public class MessageTemplateCase_Bank : MessageTemplateHelper
    //{
    //    public string name { get; set; }
    //    public string age { get; set; }
    //    public static string template = "<%=name%>今年<%=age%>岁";
    //}
    #endregion
    #region 银行模板集
    /// <summary>
    /// 发起询价跟进
    /// hody 2015-04-14
    /// </summary>
    public class StartQueryFollowUp_Bank : MessageTemplateHelper
    {
        /// <summary>
        /// 银行
        /// </summary>
        public string companyname { get; set; }
        /// <summary>
        /// 分行
        /// </summary>
        public string subbranchname { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string truename { get; set; }
        public static string template = "<%=companyname%>，<%=subbranchname%>  <%=department%>  <%=truename%>发起询价";
    }
    /// <summary>
    /// 委托预评跟进
    /// hans 2015-05-06
    /// </summary>
    public class StartYPFollowUp_Bank : MessageTemplateHelper
    {
        /// <summary>
        /// 银行
        /// </summary>
        public string companyname { get; set; }
        /// <summary>
        /// 分行
        /// </summary>
        public string subbranchname { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string truename { get; set; }
        public static string template = "<%=companyname%>，<%=subbranchname%>  <%=department%>  <%=truename%>委托预评";
    }
    /// <summary>
    /// 委托报告跟进
    /// hans 2015-05-06
    /// </summary>
    public class StartReportFollowUp_Bank : MessageTemplateHelper
    {
        /// <summary>
        /// 银行
        /// </summary>
        public string companyname { get; set; }
        /// <summary>
        /// 分行
        /// </summary>
        public string subbranchname { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string truename { get; set; }
        public static string template = "<%=companyname%>，<%=subbranchname%>  <%=department%>  <%=truename%>委托报告";
    }
    #endregion
   

    #region 评估机构模板集
    /// <summary>
    /// 发起询价跟进
    /// hody 2015-04-14
    /// </summary>
    public class StartQueryFollowUp_SOA : MessageTemplateHelper
    {
        /// <summary>
        /// 银行
        /// </summary>
        public string companyname { get; set; }
        /// <summary>
        /// 分行
        /// </summary>
        public string subbranchname { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string truename { get; set; }
        public static string template = "<%=companyname%>，<%=subbranchname%>  <%=department%>  <%=truename%>发起询价";
    }
    /// <summary>
    /// 委托预评跟进
    /// hans 2015-05-06
    /// </summary>
    public class StartYPFollowUp_SOA : MessageTemplateHelper
    {
        /// <summary>
        /// 银行
        /// </summary>
        public string companyname { get; set; }
        /// <summary>
        /// 分行
        /// </summary>
        public string subbranchname { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string truename { get; set; }
        public static string template = "<%=companyname%>，<%=subbranchname%>  <%=department%>  <%=truename%>委托预评";
    }
    /// <summary>
    /// 委托报告跟进
    /// hans 2015-05-06
    /// </summary>
    public class StartReportFollowUp_SOA : MessageTemplateHelper
    {
        /// <summary>
        /// 银行
        /// </summary>
        public string companyname { get; set; }
        /// <summary>
        /// 分行
        /// </summary>
        public string subbranchname { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string truename { get; set; }
        public static string template = "<%=companyname%>，<%=subbranchname%>  <%=department%>  <%=truename%>委托报告";
    }
    #endregion
   
}
