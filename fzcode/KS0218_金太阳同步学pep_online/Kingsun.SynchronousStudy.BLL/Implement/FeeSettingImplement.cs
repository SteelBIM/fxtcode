using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;

namespace Kingsun.SynchronousStudy.BLL
{
    public class FeeSettingImplement : BaseImplement
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
            FeeSettingManagement manage = new FeeSettingManagement();
            KingResponse response = null;
            switch (request.Function.Trim())
            {
                case "QueryFeeComboList"://///查询列表
                    response = manage.QueryFeeComboList(request);
                    break;
                case "QueryAppID"://///查询列表
                    response = manage.QueryAppID(request);
                    break;
                case "AddCombo"://///查询列表
                    response = manage.AddCombo(request);
                    break;
                case "ModifyFeeCombo"://///查询列表
                    response = manage.ModifyFeeCombo(request);
                    break;
                case "QueryFee"://///查询列表
                    response = manage.QueryFee(request);
                    break;
                case "JFeeCombo"://///查询列表
                    response = manage.JFeeCombo(request);
                    break;
                case "QueryED"://///查询列表
                    response = manage.QueryED(request);
                    break;
                case "KVip"://///查询列表
                    response = manage.KVip(request);
                    break;
                case "QueryUserInfo"://///查询列表
                    response = manage.QueryUserInfo(request);
                    break;

                default:
                    response = KingResponse.GetErrorResponse("未找到相应的接口!", request);
                    break;
            }
            return response;
        }
    }
}
