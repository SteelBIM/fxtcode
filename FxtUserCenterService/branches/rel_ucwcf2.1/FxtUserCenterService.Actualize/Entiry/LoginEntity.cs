using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtUserCenterService.Actualize
{
    public class SecurityInfo
    {
        public string time { get; set; }
        public string code { get; set; }
    }

    public class ParametersInfo 
    {
        public UserInfo uinfo { get; set; }
        public AppInfo appinfo { get; set; }
        public FunInfo funinfo { get; set; }
    }

    public class UserInfo
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class AppInfo
    {
        public string splatype{ get; set; }
		public string platVer{ get; set; }
		public string stype{ get; set; }
		public string version{ get; set; }
		public string vcode{ get; set; }
		public string systypecode{ get; set; }
		public string channel{ get; set; }
    }

    public class FunInfo
    {
        /// <summary>
        /// 产品使用网址
        /// </summary>
        public string weburl { get; set; }
    }
}
    