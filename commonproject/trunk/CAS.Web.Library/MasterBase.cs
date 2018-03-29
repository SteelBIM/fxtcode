using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using CAS.Common;
namespace CAS.Web.Library
{
    public class MasterBase : System.Web.UI.MasterPage
    {
        //Doctype
        protected string Doctype = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
        //公用META
        protected string Meta = "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=EmulateIE7\" /><meta http-equiv=\"content-type\" content=\"text/html;charset=utf-8\" />";
        //公用Scripts
        protected string Scripts
        {
            get
            {
                //测试环境使用此项，保证静态文件最新
#if DEBUG
                Public.StaticVersion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
#endif
                StringBuilder str = new StringBuilder();
                str.Append("<link rel=\"Stylesheet\" type=\"text/css\" href=\"" + Public.StaticUrl + "css/" + Public.CurrentStyle + "/public.css?" + Public.StaticVersion + "\" />");
                str.Append("<link rel=\"Stylesheet\" type=\"text/css\" href=\"" + Public.StaticUrl + "css/" + Public.CurrentStyle + "/form.css?" + Public.StaticVersion + "\" />");
                str.Append("<link rel=\"Stylesheet\" type=\"text/css\" href=\"" + Public.TemplateStylePath + "style.css?" + Public.StaticVersion + "\" />");
                str.Append("<script type=\"text/javascript\" src=\"" + Public.StaticUrl + "js/jquery-1.7.1.min.js\"></script>");
                str.Append("<script type=\"text/javascript\" src=\"" + Public.StaticUrl + "js/json2.js\"></script>");
                str.Append("<script type=\"text/javascript\" src=\"" + Public.StaticUrl + "js/init.js?" + Public.StaticVersion + "\"></script>");
                //赋值常用变量
                str.Append("<script type=\"text/javascript\">CAS.Domain = \"" + Public.Domain + "\";");
                str.Append("CAS.RootUrlFull = \"http://" + Public.RootUrlFull + "\";");
                str.Append("CAS.RootUrl = \"" + Public.RootUrl + "\";CAS.TemplateStylePath = \"" + Public.TemplateStylePath + "\";");
                str.Append("CAS.TemplatePath = \"" + Public.TemplatePath + "\";CAS.APIUrl = \"" + Public.APIUrl + "\";");
                str.Append("CAS.StaticUrl = \"" + Public.StaticUrl + "\";CAS.StaticVersion = \"" + Public.StaticVersion + "\";");
                str.Append("CAS.Style = \"" + Public.CurrentStyle + "\";");
                //单机构不用CA，这里最后还是用数据库字段存储 kevin
                if(Public.CompanyFxt.issingle==1)
                    str.Append("CAS.CA = false ;");
                //初始登录信息变量
                str.AppendFormat("CAS.Define.provinceid={0};", Public.LoginInfo.provinceid);
                str.AppendFormat("CAS.Define.provincename='{0}';", Public.LoginInfo.provincename);
                str.AppendFormat("CAS.Define.cityid={0};", Public.LoginInfo.cityid);
                str.AppendFormat("CAS.Define.cityname='{0}';", Public.LoginInfo.cityname);
                str.AppendFormat("CAS.Define.companyid={0};", Public.LoginInfo.companyid);
                str.AppendFormat("CAS.Define.companyname='{0}';", Public.LoginInfo.companyname);
                str.AppendFormat("CAS.Define.fxtcompanyid={0};",Public.CompanyFxt.fk_companyid);
                str.AppendFormat("CAS.Define.userid='{0}';", Public.LoginInfo.userid);
                str.AppendFormat("CAS.Define.username='{0}';", Public.LoginInfo.username);
                str.AppendFormat("CAS.Define.departmentid={0};", Public.LoginInfo.departmentid);
                str.AppendFormat("CAS.Define.departmentname='{0}';", Public.LoginInfo.departmentname);
                str.AppendFormat("CAS.Define.deptfullname='{0}';", Public.LoginInfo.deptfullname);
                str.AppendFormat("CAS.Define.mobilephone='{0}';", Public.LoginInfo.mobilephone);
                str.AppendFormat("CAS.Define.systypecode={0};", Public.CompanyFxt.fk_systypecode);
                str.AppendFormat("CAS.Define.surveycenterurl='{0}';", Public.SurveyCenterUrl);
                str.AppendFormat("CAS.Define.fdepartmentid={0};",Public.LoginInfo.fdepartmentid);
                str.AppendFormat("CAS.Define.topdeptid={0};", Public.LoginInfo.topdeptid);
                str.AppendFormat("CAS.Define.msgserver='{0}';", Public.CompanyFxt.msgserverpath); 
                str.Append("if (!top.dialog) {CAS.Include(['jquery.dialog.js']);} ");
                //已退出状态提示重新登录 kevin
                if (Public.NeedReLogin) {
                    str.Append("$(function(){ReLogin();});");
                }
    
#if DEBUG
                str.Append("CAS.Debug=true;");//跟踪调试
#endif
                str.Append("</script>");
                str.Append("<!--[if lt IE 7]><style type=\"text/css\">*html{background-image:url(about:blank)}</style><script type=\"text/javascript\" src=\"" + Public.StaticUrl + "js/jquery.bgiframe.js\"></script><![endif]-->");
                
                return str.ToString();
            }
        }

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
        }

    }
}