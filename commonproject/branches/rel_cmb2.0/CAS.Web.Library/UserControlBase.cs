using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;

namespace CAS.Web.Library
{
    /// <summary>
    /// 用户控件基类
    /// </summary>
    public class UserControlBase : System.Web.UI.UserControl
    {
        //定义基本全部要用到的一些变量，在基类里提前处理
        protected int ProvinceId = 0; //省
        protected string ProvinceName = ""; //省
        protected int CityId = 0; //城市
        protected string CityName = ""; //市        
        protected int CompanyId = 0;
        protected int CompanyName = 0;
        protected int FXTCompanyId = 0;
        protected string UserId = "";
        protected string UserName = "";
        
        protected override void OnLoad(EventArgs e)
        {
            ProvinceId = Public.LoginInfo.provinceid;
            CityId = Public.LoginInfo.cityid;
            CompanyId = Public.LoginInfo.companyid;
            FXTCompanyId = Public.LoginInfo.fxtcompanyid;

            UserId = Public.LoginInfo.userid;
            ProvinceName = Public.LoginInfo.provincename;
            CityName = Public.LoginInfo.cityname;
            
            base.OnLoad(e);
        }
    }
}