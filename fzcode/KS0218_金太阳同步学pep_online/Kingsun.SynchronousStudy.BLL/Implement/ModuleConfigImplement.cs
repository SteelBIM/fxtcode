using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingsun.SynchronousStudy.BLL.Management;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;


namespace Kingsun.SynchronousStudy.BLL
{
     public class ModuleConfigImplement : BaseImplement
    {
         public override KingResponse ProcessRequest(KingRequest request)
         {
             if (string.IsNullOrEmpty(request.Function))
             {
                 return KingResponse.GetErrorResponse("无法确定接口信息！", request);
             }
             if (string.IsNullOrEmpty(request.Data))
             {
                 return KingResponse.GetErrorResponse("提交的数据不能为空！", request);
             }
             ModuleConfigManagement manage = new ModuleConfigManagement();
             KingResponse response = null;
             switch (request.Function.Trim())
             {
                 case "QueryModuleConfigList"://///查询列表
                     response = manage.QueryModuleConfigList(request);
                     break;
                 case "DeleteModule"://///删除配置
                     response = manage.DeleteModuleConfig(request);
                     break;
                 case "ModifyModule"://///查询列表
                     response = manage.ModifyModule(request);
                     break;
                 case "QueryModule"://///查询列表
                     response = manage.QueryModule(request);
                     break;
                 default:
                     response = KingResponse.GetErrorResponse("未找到相应的接口!", request);
                     break;
             }
             return response;
         }
    }
}
