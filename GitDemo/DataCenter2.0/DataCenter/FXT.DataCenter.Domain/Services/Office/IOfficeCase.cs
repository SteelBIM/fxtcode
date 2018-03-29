using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
   public interface IOfficeCase
    {
        /// <summary>
        /// 获取商业案例集合
        /// </summary>
        /// <param name="datCaseOffice">参数对象</param>
        /// <param name="totalCount"></param>
        /// <param name="self">默认查询自己</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
       IQueryable<DatCaseOffice> GetOfficeCases(DatCaseOffice datCaseOffice, int pageIndex, int pageSize, out int totalCount, bool self = true);

        /// <summary>
        /// 获取商业单个对象
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
       DatCaseOffice GetOfficeCase(int id);

        /// <summary>
        /// 添加商业案例
        /// </summary>
       /// <param name="datCaseOffice">参数对象</param>
        /// <returns></returns>
       int AddOfficeCase(DatCaseOffice datCaseOffice);

        /// <summary>
        /// 修改商业案例
        /// </summary>
       /// <param name="datCaseOffice">参数对象</param>
        /// <returns></returns>
       int UpdateOfficeCase(DatCaseOffice datCaseOffice);

        /// <summary>
        /// 删除商业案例
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
       int DeleteOfficeCase(DatCaseOffice datCaseOffice);

        /// <summary>
        ///  删除重复案例
        /// </summary>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="caseDateFrom">案例开始时间</param>
        /// <param name="caseDateTo">案例结束时间</param>
        /// <returns></returns>
       int DeleteSameOfficeCase(int fxtCompanyId, int cityId, DateTime caseDateFrom, DateTime caseDateTo, string saveUser);
    }
}
