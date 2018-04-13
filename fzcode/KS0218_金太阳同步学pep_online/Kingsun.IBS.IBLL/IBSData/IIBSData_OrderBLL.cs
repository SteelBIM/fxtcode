using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Kingsun.IBS.Model.IBS;

namespace Kingsun.IBS.IBLL
{
    /// <summary>
    /// 订单信息
    /// </summary>
    public interface IIBSData_OrderBLL
    {
        /// <summary>
        /// 查询时间段订单，并返回财富分账订单列表
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        List<Tb_KSWFOrder> GetKSWFOrderList(DateTime starttime, DateTime endtime);

    }
}
