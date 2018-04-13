using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Collections;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.AppLibrary.Model;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class OrderManagement : BaseManagement
    {
        private static ArrayList orderList = new ArrayList();
        private static object obj = new object();
        internal KingResponse QueryList(KingRequest request)
        {

            PageParameter param = JsonHelper.DecodeJson<PageParameter>(request.Data);
            #region 验证相关数据有效性
            if (param == null)
            {
                return KingResponse.GetErrorResponse("当前信息为空");
            }
            if (param.PageIndex < 1)
            {
                return KingResponse.GetErrorResponse("当前页面数不正确");
            }
            if (param.PageSize < 1)
            {
                return KingResponse.GetErrorResponse("当前页pageSize不正确");
            }
            #endregion
            param.OrderColumns = "CreateDate";
            param.TbNames = "V_OrderDetails";
            param.IsOrderByASC = 2;
            param.Columns = "*";
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = param.getParameterList();
            System.Data.DataSet ds = ExecuteProcedure("proc_pageView", list);
            if (ds == null || ds.Tables.Count < 1)
            {

                return KingResponse.GetErrorResponse("执行存储过程失败，" + _operatorError);
            }
            object obj = new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = FillData<V_OrderDetails>(ds.Tables[0])
            };
            return KingResponse.GetResponse(request, obj);
        }



        internal KingResponse QueryCoupon(KingRequest request)
        {

            PageParameter param = JsonHelper.DecodeJson<PageParameter>(request.Data);
            #region 验证相关数据有效性
            if (param == null)
            {
                return KingResponse.GetErrorResponse("当前信息为空");
            }
            if (param.PageIndex < 1)
            {
                return KingResponse.GetErrorResponse("当前页面数不正确");
            }
            if (param.PageSize < 1)
            {
                return KingResponse.GetErrorResponse("当前页pageSize不正确");
            }
            #endregion
            param.OrderColumns = "";
            param.TbNames = "V_Coupon";
            param.IsOrderByASC = 2;
            param.Columns = "*";
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = param.getParameterList();
            System.Data.DataSet ds = ExecuteProcedure("proc_pageView", list);
            if (ds == null || ds.Tables.Count < 1)
            {

                return KingResponse.GetErrorResponse("执行存储过程失败，" + _operatorError);
            }
            object obj = new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = FillData<V_Coupon>(ds.Tables[0])
            };
            return KingResponse.GetResponse(request, obj);
        }
        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal KingResponse GetOrderInfoByID(KingRequest request)
        {
            TB_Order submitData = JsonHelper.DecodeJson<TB_Order>(request.Data);
            #region 验证相关数据有效性
            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("当前信息为空");
            }
            if (!submitData.ID.HasValue || submitData.ID.Value == Guid.Empty)
            {
                return KingResponse.GetErrorResponse("编号不能为空");
            }
            #endregion
            TB_Order orderinfo = Select<TB_Order>(submitData.ID);
            if (string.IsNullOrEmpty(_operatorError))
            {
                return KingResponse.GetResponse(request, orderinfo);
            }
            else
            {
                return KingResponse.GetErrorResponse(_operatorError, request);
            }

        }

        /// <summary>
        /// 获取订单详情通过订单编号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal KingResponse GetOrderInfoByOrderID(KingRequest request)
        {
            TB_Order submitData = JsonHelper.DecodeJson<TB_Order>(request.Data);
            #region 验证相关数据有效性
            if (submitData == null)
            {
                return KingResponse.GetErrorResponse("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.OrderID))
            {
                return KingResponse.GetErrorResponse("订单编号不能为空");
            }
            #endregion
            V_OrderDetails order = GetOrderDetailByOrderID(submitData.OrderID);
            if (string.IsNullOrEmpty(_operatorError))
            {
                if (order == null)
                {
                    return KingResponse.GetErrorResponse("获取订单失败", request);
                }
                else
                {
                    return KingResponse.GetResponse(request, order);
                }


            }
            else
            {
                return KingResponse.GetErrorResponse(_operatorError, request);
            }
        }



        internal KingResponse GetAllEditions(KingRequest request)
        {
            string sql = "select distinct EditionID,EditionName from Tb_Course where [Disable]=1";
            System.Data.DataSet result = ExecuteSql(sql);
            List<object> editionList = new List<object>();
            if (result == null || result.Tables.Count == 0)
            {
                return KingResponse.GetErrorResponse("服务器出错", request);
            }
            else
            {
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    var obj = new
                    {
                        EditionID = row["EditionID"].ToString(),
                        EditionName = row["EditionName"].ToString()
                    };
                    editionList.Add(obj);
                }
            }
            return KingResponse.GetResponse(request, editionList);
        }

        /// <summary>
        /// 获取订单详情信息通过订单编号
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public V_OrderDetails GetOrderDetailByOrderID(string orderid)
        {
            IList<V_OrderDetails> list = Search<V_OrderDetails>("OrderID='" + orderid + "'");
            if (string.IsNullOrEmpty(_operatorError))
            {
                if (list != null && list.Count == 1)
                {
                    return list[0];
                }
            }
            return null;
        }

        public KingResponse SaveOrderID(string orderid, string tborderid)
        {
            string sql = "update [TB_Order] set OrderID='" + orderid + "' where ID='" + tborderid + "'";
            ExecuteSql(sql);
            if (string.IsNullOrEmpty(_operatorError))
            {
                return KingResponse.GetErrorResponse(_operatorError);
            }
            return KingResponse.GetResponse(new KingRequest(), "");
        }

        /// <summary>
        /// 获取订单表信息通过订单编号
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public TB_Order GetOrderByOrderID(string orderid)
        {
            IList<TB_Order> list = Search<TB_Order>("OrderID='" + orderid + "'");
            if (string.IsNullOrEmpty(_operatorError))
            {
                if (list != null && list.Count == 1)
                {
                    return list[0];
                }
            }
            return null;
        }

        public DataTable Excel(string where)
        {
            PageParameter param = new PageParameter();
            param.OrderColumns = "CreateDate";
            param.TbNames = "V_OrderDetails";
            param.IsOrderByASC = 2;
            param.Columns = "*";
            param.PageIndex = 1;
            param.PageSize = int.MaxValue;
            param.Where = where;
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = param.getParameterList();
            System.Data.DataSet ds = ExecuteProcedure("proc_pageView", list);
            if (ds != null)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 清理过期订单
        /// </summary>
        public void ClearTimeOverOrder()
        {
            //List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            //System.Data.SqlClient.SqlParameter[] param = new System.Data.SqlClient.SqlParameter[1];
            //param[0] = new System.Data.SqlClient.SqlParameter("@overtime", System.Data.SqlDbType.Int);
            //param[0].Value = ProjectConstant.OrderOverTime;
            //list.AddRange(param);
            //System.Data.DataSet ds = ExecuteProcedure("proc_ClearTimeOverOrder", list);
        }
    }
}
