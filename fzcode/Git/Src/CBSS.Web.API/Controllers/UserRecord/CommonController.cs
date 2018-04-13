using CBSS.Cfgmanager.BLL;
using CBSS.Cfgmanager.IBLL;
using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.Framework.Redis;
using CBSS.IBS.BLL;
using CBSS.IBS.Contract.IBSResource;
using CBSS.IBS.IBLL;
using CBSS.Pay.BLL;
using CBSS.Pay.IBLL;
using CBSS.Tbx.BLL;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.IBLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.API.Controllers
{
    /// <summary>
    /// 说说看记录
    /// </summary>
    public partial class UserRecordController
    {
        static ICfgmanagerService cfgmanagerService = new CfgmanagerService();
        static ITbxService tbxService = new TbxService();
        static IIBSService ibsService = new IBSService();
        static IPayOrder ipayOrder = new PayOrderBLL();
        static RedisHashHelper redis = new RedisHashHelper("Tbx");
        static RedisListHelper redisList = new RedisListHelper("Tbx");
    }
}
