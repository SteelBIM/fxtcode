using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    /// <summary>
    /// 楼层图片接口
    /// </summary>
    public interface IBusinessFloorPhoto
    {
        /// <summary>
        /// 获取楼层图片列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IQueryable<LNK_F_Photo> GetLNK_F_PhotoList(LNK_F_Photo model, bool self = true);
        /// <summary>
        /// 获取楼层图片信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        LNK_F_Photo GetLNK_F_PhotoById(int Id, int CityId, int FxtCompanyId);
        /// <summary>
        /// 新增楼层图片
        /// </summary>
        /// <param name="modal">楼层图片model</param>
        /// <returns></returns>
        int AddLNK_F_Photo(LNK_F_Photo model);

        /// <summary>
        /// 修改楼层图片
        /// </summary>
        /// <param name="modal">楼层图片model</param>
        /// <returns></returns>
        int UpdateLNK_F_Photo(LNK_F_Photo model, int currFxtCompanyId);

       /// <summary>
        /// 删除楼层图片
       /// </summary>
       /// <param name="Id"></param>
       /// <param name="cityId"></param>
       /// <param name="fxtCompanyId"></param>
       /// <param name="userName"></param>
       /// <param name="productTypeCode"></param>
       /// <returns></returns>
        bool DeleteLNK_F_Photo(int Id, int cityId, int fxtCompanyId, string userName, int productTypeCode, int currFxtCompanyId);
    }
}
