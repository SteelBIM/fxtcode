using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;

namespace Kingsun.SynchronousStudy.BLL
{
    public class OrderImplement : BaseImplement
    {
        OrderManagement manage = new OrderManagement();
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

            KingResponse response = null;
            switch (request.Function.Trim())
            {
                case "QueryList"://///获取列表
                    response = manage.QueryList(request);
                    break;
                case "GetOrderInfoByID"://///获取订单信息通过tborderid
                    response = manage.GetOrderInfoByID(request);
                    break;
                case "GetOrderInfoByOrderID"://///获取订单信息通过orderid
                    response = manage.GetOrderInfoByOrderID(request);
                    break;
                case "GetAllEditions"://///获取所有的版本信息
                    response = manage.GetAllEditions(request);
                    break;
                case "QueryCoupon"://///获取所有的版本信息
                    response = manage.QueryCoupon(request);
                    break;

                default:
                    response = KingResponse.GetErrorResponse("未找到相应的接口!", request);
                    break;
            }
            return response;
        }

        public KingResponse SaveOrderID(string orderid, string tborderid)
        {
            return manage.SaveOrderID(orderid, tborderid);
        }

        public void ClearTimeOverOrder()
        {
            manage.ClearTimeOverOrder();
        }
    }
}
