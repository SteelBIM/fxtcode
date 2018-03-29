using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using FxtService.Common;
using System.Linq.Expressions;
using FxtNHibernate.DATProjectDomain.Entities;

/**
 * 作者: 李晓东
 * 时间: 2013.11.27
 * 摘要: 新建Wcf (契约)Contract IFxtspider(接口)
 *       2013.12.05 增加 GetDatProject 修改人:李晓东
 * **/
namespace FxtService.Contract.FxtSpiderInterface
{
    [ServiceContract()]
    public interface IFxtspider
    {
        /// <summary>
        /// 抓取导入
        /// </summary>
        /// <param name="data">JSON数据</param>
        /// <param name="ip">IP地址</param>
        /// <param name="validate">验证</param>
        /// <returns></returns>
        //[OperationContract]
        string SpiderExport(string data);

        /// <summary>
        /// 获得楼盘信息
        /// </summary>
        /// <param name="cityId">城市</param>
        /// <param name="projectName"></param>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        //[OperationContract]
        string GetDatProject(int cityId, string projectName, string sDate, string eDate, int pageSize, int pageIndex);
    }
}