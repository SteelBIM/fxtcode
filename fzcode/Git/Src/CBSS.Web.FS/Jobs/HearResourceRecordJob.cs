using CBSS.Core.Log;
using CBSS.IBS.BLL;
using CBSS.IBS.Contract.IBSResource;
using CBSS.IBS.IBLL;
using CourseActivate.Web.FS.Controllers;
using FluentScheduler;
using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using CBSS.Tbx.IBLL;
using CBSS.Tbx.BLL;

namespace CourseActivate.Web.FS.Jobs
{
    /// <summary>
    /// 说说看记录
    /// </summary>
    public class HearResourceRecordJob
    {
        IIBSService ibsService = new IBSService();
        ITbxService tbxService = new TbxService();

        public void Execute()
        {
        }
    }

}