using Kingsun.SpokenBroadcas.Common;
using Kingsun.SpokenBroadcas.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.BLL
{
    public class OrderManagement : BaseManagement
    {
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
        /// <summary>
        /// 根据Tb_Order表ID获取课程名称
        /// </summary>
        /// <param name="tborderid"></param>
        /// <returns></returns>
        public string GetCourseName(string tborderid)
        {
            string sql = string.Format(@"  select Name from Tb_CoursePeriod a left join Tb_CoursePeriodTime b on a.ID=b.CoursePeriodID left join Tb_Order c on b.ID=c.CoursePeriodTimeID where c.ID='{0}'", tborderid);
            object obj = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteScalar(Kingsun.SpokenBroadcas.Common.SqlHelper.SpokenConnectionString, CommandType.Text, sql);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }
    }
}
