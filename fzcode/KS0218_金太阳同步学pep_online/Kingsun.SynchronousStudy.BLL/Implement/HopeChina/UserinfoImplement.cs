using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;

namespace Kingsun.SynchronousStudy.BLL
{
    public class UserinfoImplement : BaseImplement
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
            UserinfoManagement manage = new UserinfoManagement();
            KingResponse response = null;
            switch (request.Function.Trim())
            {
                case "AddUserinfo"://///新增
                 //   response = manage.AddUserinfo(request);
                    break;
                case "UpdUserFinish"://///交券
                    response = manage.UpdUserFinish(request);
                    break;
                case "QueryUserinfo"://///查询
                    response = manage.QueryUserinfo(request);
                    break;
                case "QueryUserinfoById"://///查询
                    response = manage.QueryUserinfoById(request);
                    break;
                case "StarPageByUserId"://///返回当前状态
                    response = manage.StarPageByUserId(request);
                    break;
                default:
                    response = KingResponse.GetErrorResponse("未找到相应的接口!", request);
                    break;
            }
            return response;
        }
    }
}
