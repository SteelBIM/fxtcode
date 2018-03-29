using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    /// <summary>
    /// 楼栋图片接口
    /// </summary>
    public interface IBusinessBuildingPhoto
    {
        /// <summary>
        /// 获取楼栋图片列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IQueryable<LNK_B_Photo> GetLNK_B_PhotoList(LNK_B_Photo model, bool self = true);
        /// <summary>
        /// 获取楼栋图片信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        LNK_B_Photo GetLNK_B_PhotoById(int Id, int CityId, int FxtCompanyId);
        /// <summary>
        /// 新增楼栋图片
        /// </summary>
        /// <param name="modal">楼栋图片model</param>
        /// <returns></returns>
        int AddLNK_B_Photo(LNK_B_Photo model);

        /// <summary>
        /// 修改楼栋图片
        /// </summary>
        /// <param name="modal">楼栋图片model</param>
        /// <returns></returns>
        int UpdateLNK_B_Photo(LNK_B_Photo model, int currFxtCompanyId);

         /// <summary>
       /// 删除楼栋图片
       /// </summary>
       /// <param name="Id">图片ID</param>
       /// <param name="CityId">城市ID</param>
       /// <param name="FxtCompanyId">评估机构ID</param>
       /// <param name="userId">用户ID</param>
       /// <param name="ProductTypeCode">产品Code</param>
       /// <returns></returns>
        bool DeleteLNK_B_Photo(int Id, int CityId, int FxtCompanyId, string userId, int ProductTypeCode, int currFxtCompanyId);
    }
}
