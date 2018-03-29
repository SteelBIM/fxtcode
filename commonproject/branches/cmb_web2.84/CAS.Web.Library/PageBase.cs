using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
namespace CAS.Web.Library
{
    public class PageBase:System.Web.UI.Page
    {    
        /// <summary>
        /// 是否有传入master，如果有传入，则使用指定的模板
        /// 传入的master值
        /// </summary>
        protected string RequestMaster = null;

        protected int CityId, FXTCompanyId;

        protected override void OnPreInit(EventArgs e)
        {
            //当找不到api时，为保证API重启后刷新页面找API，这里加入尝试找API代码 kevin
            if (Public.CompanyFxt == null)
            {
                Public.SetCompanyFxt();
            }
            //如果还是找不到，提示错误。
            if (Public.CompanyFxt == null)
            {
                Public.ResponseWriteError("找不到API，请联系平台管理员。");
                return;
            }
            if (Request["masterpage"] != null)
            {
                RequestMaster = Request["masterpage"];
            }
            base.OnPreInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Title = string.IsNullOrEmpty(this.Title) ? Public.ProductName : this.Title + " - " + Public.ProductName;
            //不管是否登录都需要的全局变量，用于所有地方调用            
            Public.LoginInfo.systypecode = Public.SysTypeCode;
            CityId = Public.LoginInfo.cityid;
            FXTCompanyId = Public.LoginInfo.fxtcompanyid;
            base.OnLoad(e);
        }

        /// <summary>
        /// 设置母版页
        /// </summary>
        /// <param name="master"></param>
        protected void SetMaster(System.Web.UI.Page page,string master)
        {
            master = RequestMaster == null ? master : RequestMaster;
            //这里可以扩展为从数据库读取该文件对应的母版 
            string path = Public.TemplatePath + "master/" + master + ".Master";
            if (!FileHelper.ExistsFile(Server.MapPath(path)))
            {
                Public.RootUrl = "/";
                Public.RootUrlFull = HttpContext.Current.Request.Url.Authority + Public.RootUrl;
                path = Public.TemplatePath + "master/" + master + ".Master";
                if (!FileHelper.ExistsFile(Server.MapPath(path)))
                {
                    Public.ResponseWriteError("找不到模板:" + path);
                    return;
                }
            }
            page.MasterPageFile = path;
        }

        protected string GetRequest(string key)
        {
            return Public.GetRequest(key, "");
        }

        protected string GetRequest(string key,string def)
        {
            return Public.GetRequest(key, def);
        }

        /// <summary>
        /// 页面code下拉框绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string CodeList(int id)
        {
            return CodeList(id, 0);
        }

        /// <summary>
        /// 页面code下拉框绑定
        /// </summary>
        /// <param name="id"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        protected string CodeList(int id,int defVal) {
            return Public.GetDropdownCodes(id,defVal);
        }
    }
}