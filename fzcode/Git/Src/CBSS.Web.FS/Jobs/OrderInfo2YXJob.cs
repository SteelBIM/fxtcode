using CBSS.Core.Log;
using CBSS.Tbx.BLL;
using CBSS.Tbx.IBLL;
using CourseActivate.Web.FS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.FS.Jobs
{
    public class OrderInfo2YXJob
    {
        ITbxService tbxService = new TbxService();
        public void SyncOrderInfo2YX()
        {
            lock (TimedTask._SyncYXOrderLock)
            {
                try
                {
                    tbxService.OrderInfo2YXDB();
                    // ibsService.GetDubbingByCataId(13989);
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.FsExceptionLog, "同步订单到优学异常", ex);
                }

            }
        }
       
    }
}