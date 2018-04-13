using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;

namespace Kingsun.SynchronousStudy.BLL
{
    public class ArticleImplement : BaseImplement
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
            ArticleManagement manage = new ArticleManagement();
            KingResponse response = null;
            switch (request.Function.Trim())
            {
                case "AddArticle"://///新增课程
                    response = manage.AddArticle(request);
                    break;
                case "QueryArticle"://///查询课程
                    response = manage.QueryArticle(request);
                    break;
                case "QueryArticleById"://///查询课程
                    response = manage.QueryArticleById(request);
                    break;
                case "QueryArticleByWhere"://///查询课程
                    response = manage.QueryArticleByWhere(request);
                    break;
                default:
                    response = KingResponse.GetErrorResponse("未找到相应的接口!", request);
                    break;
            }
            return response;
        }
    }
}
