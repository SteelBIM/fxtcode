using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FXT.DataCenter.WebUI.Infrastructure.WebSecurity
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 无权限警示
        /// </summary>
        /// <param name="alert">警示内容</param>
        /// <returns></returns>
        public ContentResult AuthorizeWarning(string alert)
        {
            var script =
                "<script>alert('"+alert+"');if(typeof(top.tb_remove)!='function'){history.go(-1);}else{top.tb_remove();} </script>";
            return this.Content(script);
        }

        /// <summary>
        /// 用JS关闭弹窗
        /// </summary>
        /// <returns></returns>
        public ContentResult CloseThickbox(string alert)
        {
            string script = string.Format("<script>top.tb_remove();alert('{0}');parent.location.reload(1);</script>",alert);
            return this.Content(script);
        }
        /// <summary>
        /// 当弹出DIV弹窗时，需要刷新浏览器整个页面
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public ContentResult CloseThickboxToLoad(string alert=null)
        {
            string script = string.Format("<script>parent.location.reload(1);</script>", alert);
            return this.Content(script);
        }
        /// <summary>
        /// 用JS提示消息
        /// 停留当前页面
        /// </summary>
        /// <returns></returns>
        public ContentResult Thickbox(string alert)
        {
            string script = string.Format("<script>alert('{0}');return false;</script>", alert);
            return this.Content(script);
        }
       
        /// <summary>
        /// 当弹出DIV弹窗时，需要刷新浏览器整个页面
        /// </summary>
        /// <returns></returns>
        public ContentResult RefreshParent(string alert = null)
        {
            var script = string.Format("<script>{0};self.parent.window.location.reload();</script>", string.IsNullOrEmpty(alert) ? string.Empty : "alert('" + alert + "')");
            return this.Content(script);
        }

        /// <summary>
        /// 弹出警告消息
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        public JavaScriptResult AjaxAlert(string alert)
        {
            return JavaScript("alert('"+alert+"')");
        }

        /// <summary>
        /// 弹出窗口并跳转到指定的页面
        /// </summary>
        /// <returns></returns>
        public ContentResult AlertAndRedirect(string alert = null,string action = null)
        {
            var script = string.Format("<script>{0};{1};</script>", string.IsNullOrEmpty(alert) ? string.Empty : "alert('" + alert + "')", string.IsNullOrEmpty(action) ? string.Empty : "location.href='"+Url.Action(action)+"'");
            return this.Content(script);
        }

        /// <summary>
        ///  警告并且历史返回
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ContentResult Back(string notice=null)
        {
            var content = new StringBuilder("<script>");
            if (!string.IsNullOrEmpty(notice))
                content.AppendFormat("alert('{0}');", notice);
            content.Append("history.go(-1)</script>");
            return this.Content(content.ToString());
        }
        public ContentResult PageReturn(string msg, string url = null)
        {
            var content = new StringBuilder("<script type='text/javascript'>");
            if (!string.IsNullOrEmpty(msg))
                content.AppendFormat("alert('{0}');", msg);
            if (string.IsNullOrWhiteSpace(url))
                url = Request.Url.ToString();
            content.Append("window.location.href='" + url + "'</script>");
            return this.Content(content.ToString());
        }

        /// <summary>
        /// 转向到一个提示页面，然后自动返回指定的页面
        /// </summary>
        /// <param name="notice"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        public ContentResult Stop(string notice, string redirect, bool isAlert = false)
        {
            var content = "<meta http-equiv='refresh' content='1;url=" + redirect + "' /><body style='margin-top:0px;color:red;font-size:24px;'>" + notice + "</body>";

            if (isAlert)
                content = string.Format("<script>alert('{0}'); window.location.href='{1}'</script>", notice, redirect);

            return this.Content(content);
        }
    }
}
