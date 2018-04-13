using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;

namespace Kingsun.SynchronousStudy.BLL
{
    public class ScoreImplement : BaseImplement
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
            ScoreManagement manage = new ScoreManagement();
            KingResponse response = null;
            switch (request.Function.Trim())
            {
                case "AddScore"://///修改成绩
                    response = manage.AddScore(request);
                    break;
                case "UpdScore"://///修改成绩
                    response = manage.UpdScore(request);
                    break;
                case "QueryScore"://///查询成绩
                    response = manage.QueryScore(request);
                    break;
                case "QueryScoreById"://///查询成绩
                    response = manage.QueryScoreById(request);
                    break;
                case "QueryScoreByUserId"://///查询成绩
                    response = manage.QueryScoreByUserId(request);
                    break;
                default:
                    response = KingResponse.GetErrorResponse("未找到相应的接口!", request);
                    break;
            }
            return response;
        }
    }
}
