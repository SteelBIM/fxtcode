using Kingsun.IBS.BLL.IBS2MOD;
using Kingsun.IBS.IBLL.IBS2MOD;
using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kingsun.IBS.Api.Controllers
{
    public class ModDataPushController : ApiController
    {
        [HttpPost]
        [HttpGet]
        public void MODDataPush(string Info)
        {
            MOD_PushData moddata = JsonHelper.DecodeJson<MOD_PushData>(Info);
            var result = false;
       
            switch(moddata.DataType)
            {
                case 2://班级信息变动
                    IIBS_MOD_ClassInfoBLL classbll = new MOD2IBS_ClassInfoBLL();
                    switch (moddata.ChangeType) 
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }
                    break;
                case 3://学校信息变动
                    IIBS_MOD_SchInfoBLL schbll = new MOD2IBS_SchInfoBLL();
                    switch (moddata.ChangeType)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }
                    break;
                case 4://用户信息变动
                     IIBS_MOD_UserInfoBLL userbll = new MOD2IBS_UserInfoBLL();
                    switch (moddata.ChangeType)
                    {
                            
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }
                    break;
                case 5://区域信息变动
                    IIBS_MOD_AreaInfoBLL areabll = new MOD2IBS_AreaInfoBLL();
                    switch (moddata.ChangeType)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }
                    break;
                case 7://用户班级关系变动
                    break;
                case 8://班级学校关系变动
                    break;
                case 9://学校区域关系变动
                    break;
                default: 
                    break;
            }
            if (result) 
            {
                
            }
            
        }
    }
}