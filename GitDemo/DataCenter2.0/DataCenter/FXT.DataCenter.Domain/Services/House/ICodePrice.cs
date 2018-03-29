using FXT.DataCenter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodePrice
    {
        /// <summary>
        /// 查询单个
        /// </summary>
        /// <param name="codeName"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        sys_CodePrice FindByCodeName(string codeName, int cityId, int fxtCompanyId);
        /// <summary>
        /// 查询全部
        /// </summary>
        /// <param name="typeCode">系统配置表Code</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        IQueryable<sys_CodePrice> FindAllByTypeCode(int typeCode, int cityId, int fxtCompanyId);

        /// <summary>
        /// 修改影响价格的百分比
        /// </summary>
        /// <param name="price"></param>
        /// <param name="price">影响价格的百分比</param>
        /// <returns></returns>
        int UpdateCodePrice(int codePriceId, string price);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">机构ID</param>
        /// <returns></returns>
        IQueryable<sys_CodePrice> FindAll(int cityId, int fxtCompanyId);

        int AddCodePrice(sys_CodePrice price);

        int DeleteCodePrice(int cityid, int fxtcompanyid, int TypeCode);

        DataTable ExportFrontCode(int typeCode, int cityId, int fxtCompanyId);
        DataTable ExportSightCode(int typeCode, int cityId, int fxtCompanyId);
        DataTable ExportVDCode(int typeCode, int cityId, int fxtCompanyId);
        DataTable ExportFitmentCode(int typeCode, int cityId, int fxtCompanyId);
        DataTable ExportBuildingAreaCode(int typeCode, int cityId, int fxtCompanyId);
    }
}
