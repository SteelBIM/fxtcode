using FXT.DataCenter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Services
{
    public interface IOfficePeiTao
    {
        /// <summary>
        /// 获取办公商务配套列表
        /// </summary>
        /// <returns></returns>
        IQueryable<DatOfficePeiTao> GetOfficePeiTaos(DatOfficePeiTao datOfficePeiTao, int pageIndex, int pageSize, out int totalCount, bool self);
        /// <summary>
        /// 通过办公商务配套id获取配套信息
        /// </summary>
        /// <returns></returns>
        DatOfficePeiTao GetPeiTaoById(long officePeiTao, int fxtCompanyId);
        ///// <summary>
        ///// 通过办公商务配套id获取配套信息
        ///// </summary>
        ///// <returns></returns>
        //IQueryable<DatOfficePeiTaoTenant> GetTenantNameList(int fxtCompanyId, int cityId);
        /// <summary>
        /// 验证办公商务配套是否存在（存在，返回办公配套ID，不存在，则返回0）
        /// </summary>
        /// <param name="PeiTaoID"></param>
        /// <param name="PeiTaoName"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        bool IsExistOfficePeiTao(long PeiTaoID, string PeiTaoName, long projectId, int cityId, int fxtCompanyId);
        /// <summary>
        /// 修改商务配套信息
        /// </summary>
        /// <returns></returns>
        int UpdateOfficePeiTao(DatOfficePeiTao datOfficePeiTal, int currentCompanyId);
        /// <summary>
        /// 增加商务配套信息
        /// </summary>
        /// <param name="datOfficePeiTao"></param>
        /// <returns></returns>
        int AddOfficePeiTao(DatOfficePeiTao datOfficePeiTao);
        /// <summary>
        /// 删除商务配套信息
        /// </summary>
        /// <param name="datOfficePeiTao"></param>
        /// <param name="currentCompanyId"></param>
        /// <param name="ProductTypeCode"></param>
        /// <returns></returns>
        int DeleteOfficePeiTao(DatOfficePeiTao datOfficePeiTao, int currentCompanyId, int ProductTypeCode);

        /// <summary>
        /// 获取办公商务配套Id
        /// </summary>
        /// <returns></returns>
        long GetPeiTaoIdByName(string peitaoName, long projectId, int cityId, int companyId);

    }
}
