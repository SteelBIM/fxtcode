using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

namespace FXT.DataCenter.Infrastructure.Common.CommonWeb
{
    /// <summary>
    /// �ű���ز�����
    /// </summary>
    public static class ScriptHelper
    {
        #region �ͻ��˽ű���ʾ

        /// <summary>
        /// �ͻ��˽ű���ʾ
        /// </summary> 
        /// <param name="message">Ҫ����������</param>
        public static void Alert(string message)
        {
            HttpHelper.CurrentPage.ClientScript.RegisterStartupScript(HttpHelper.CurrentPage.GetType(), "", "<script>alert(\"" + EncodeScriptText(message) + "\");</script>");
        }

        #endregion

        #region �Կͻ��˽ű����б���

        /// <summary>
        /// �Կͻ��˽ű����б���
        /// </summary>
        /// <param name="script">Ҫ����Ľű�</param>
        /// <returns></returns>
        private static string EncodeScriptText(string script)
        {
            return script.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("\n", @"\n").Replace("\t", @"\t").Replace("\a", @"\a").Replace("\b", @"\b");
        }

        #endregion

        #region ��ʾ�ͻ�����Ϣ������ҳ����ת

        /// <summary>
        /// ��ʾ�ͻ�����Ϣ������ҳ���ض���ĳ��URL
        /// </summary>
        /// <param name="message">Ҫ��������Ϣ</param>
        /// <param name="url">�ض����URL</param>
        public static void ShowAndTopRedirect(string message, string url)
        {
            ShowAndRedirect("top",message,url);
        }

        /// <summary>
        /// ��ʾ�ͻ�����Ϣ����ǰҳ���ض���ĳ��URL
        /// </summary>
        /// <param name="message">Ҫ��������Ϣ</param>
        /// <param name="url">�ض����URL</param>
        public static void ShowAndRedirect(string message, string url)
        {
            ShowAndRedirect("window", message, url);
        }

        /// <summary>
        /// ��ʾ�ͻ�����Ϣ��ҳ���ض���ĳ��URL
        /// </summary>
        /// <param name="page">��ǰҳ������ҳ��</param>
        /// <param name="message">Ҫ��������Ϣ</param>
        /// <param name="url">�ض����URL</param>
        private static void ShowAndRedirect(string page, string message, string url)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script language='javascript'>");
            builder.AppendFormat("alert('{0}');", EncodeScriptText(message));
            builder.AppendFormat(page + ".location.href='{0}'", url);
            builder.Append("</script>");
            HttpHelper.CurrentPage.ClientScript.RegisterClientScriptBlock(HttpHelper.CurrentPage.GetType(), "", builder.ToString());
        }

        #endregion

        #region ����ĳ���ؼ�����ѡ����ʾ��Ϣ

        /// <summary>
        /// ����ĳ���ؼ�����ѡ����ʾ��Ϣ
        /// </summary>
        /// <param name="Control">�ؼ�</param>
        /// <param name="message">��ʾ����Ϣ</param>
        public static void ShowConfirm(WebControl Control, string message)
        {
            Control.Attributes.Add("onclick", "return confirm('" + EncodeScriptText(message) + "');");
        }

        #endregion

        #region ע��һ�νű���ҳ��ײ�

        /// <summary>
        /// ע��һ�νű���ҳ��ײ�
        /// </summary>
        /// <param name="script">Ҫע��Ľű�</param>
        public static void RegisterStartupScript(string script)
        {
            HttpHelper.CurrentPage.ClientScript.RegisterStartupScript(HttpHelper.CurrentPage.GetType(), "", EncodeScriptText(script), true);
        }

        #endregion

        #region ע��һ�νű���ҳ�涥��

        /// <summary>
        /// ע��һ�νű���ҳ�涥��
        /// </summary>
        /// <param name="script">Ҫע��Ľű�</param>
        public static void RegisterClientScriptBlock(string script)
        {
            HttpHelper.CurrentPage.ClientScript.RegisterClientScriptBlock(HttpHelper.CurrentPage.GetType(), "", EncodeScriptText(script), true);
        }

        #endregion

        /// <summary>
        /// ��ýű��ĵ����ı�
        /// </summary>
        /// <param name="text">�ı�</param>
        /// <returns>�ű��ĵ����ı�</returns>
        public static string AlertText(string text)
        {
            return "<script>alert(\"" + EncodeScriptText(text) + "\");</script>";
        }

        /// <summary>
        /// ���������������ҳ
        /// </summary>
        /// <param name="content">����</param>
        /// <returns>���˺������</returns>
        public static string FilterTagForJs(string content)
        {
            return content.Replace("'", "\\'");
        }
    }
}
