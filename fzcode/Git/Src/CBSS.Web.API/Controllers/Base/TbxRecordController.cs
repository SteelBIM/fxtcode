using CBSS.Framework.Contract.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.API.Controllers
{
    /// <summary>
    /// 用户总记录（用户权限、默认书籍）
    /// </summary>
    public partial class BaseController
    {
        [HttpPost]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BaseController.GetTbxRecord()”的 XML 注释
        public static APIResponse GetTbxRecord()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BaseController.GetTbxRecord()”的 XML 注释
        {
            return APIResponse.GetResponse("");

        }
    }
}
