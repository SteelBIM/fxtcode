using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;
using FXT.DataCenter.Domain.Models.FxtDataBiz;

namespace FXT.DataCenter.Domain.Services
{
    public interface IBusinessStreet
    {
        #region 商业街

        /// <summary>
        /// 查询商业街
        /// </summary>
        /// <param name="projectBiz"></param>
        /// <param name="totalCount"></param>
        /// <param name="self">是否是自己</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<Dat_Project_Biz> GetProjectBizs(Dat_Project_Biz projectBiz, int pageIndex, int pageSize, out int totalCount, bool self = true);

        /// <summary>
        /// 获取商业街
        /// </summary>
        /// <param name="cityId">当前城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <param name="areaId">行政区ID</param>
        /// <param name="self"></param>
        /// <returns></returns>
        IQueryable<Dat_Project_Biz> GetProjectBizs(int cityId, int fxtCompanyId, int areaId = -1, int subAreaId = -1, int projectId = -1, bool self = true);


        /// <summary>
        /// 根据Id查询商业街
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        Dat_Project_Biz GetProjectBizById(int projectId, int cityId, int fxtCompanyId);

        /// <summary>
        /// 获取商业街ID
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <param name="areaId">行政区ID</param>
        /// <param name="projectName">街道名称</param>
        /// <returns></returns>
        IQueryable<long> GetProjectId(int cityId, int fxtCompanyId, int areaId, string projectName);


        /// <summary>
        /// 根据商圈ID获取对应的商业街数
        /// </summary>
        /// <param name="subAreaId">商圈ID</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        int GetProjectCountsBySubAreaId(int subAreaId, int cityId, int fxtCompanyId);

        /// <summary>
        /// 新增商业街
        /// </summary>
        /// <param name="projectBiz"></param>
        /// <returns></returns>
        int AddProjectBiz(Dat_Project_Biz projectBiz);

        /// <summary>
        /// 修改商业街
        /// </summary>
        /// <param name="projectBiz"></param>
        /// <param name="currentCompanyId">当前登录的评估机构公司ID</param>
        /// <returns></returns>
        int UpdateProjectBiz(Dat_Project_Biz projectBiz, int currentCompanyId);

        /// <summary>
        /// 删除商业街
        /// </summary>
        /// <param name="projectBiz"></param>
        /// <param name="currentCompanyId">当前登录的评估机构公司ID</param>
        /// <returns></returns>
        int DeleteProjectBiz(Dat_Project_Biz projectBiz, int currentCompanyId);

        /// <summary>
        /// 验证商业街是否存在（存在，返回商业街ID，不存在，则返回0）
        /// </summary>
        /// <param name="areaId">行政区ID</param>
        /// <param name="projectId">商业街ID(新增状态时：projectId= -1)</param>
        /// <param name="projectName">商业街名称</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <returns></returns>
        long ValidateProjectBiz(int areaId, long projectId, string projectName, int cityId, int fxtCompanyId);

        /// <summary>
        /// 自定义导出
        /// </summary>
        DataTable ProjectSelfDefineExport(Dat_Project_Biz projectBiz, List<string> projectAttr, int CityId, int FxtCompanyId, bool self = true);
        #endregion

        #region 商业街道图片

        /// <summary>
        /// 获取商业街道图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <param name="self">是否为查询自己, true：查询自己</param>
        /// <returns></returns>
        IQueryable<LNK_P_Photo> GetBusinessStreetPhotoes(LNK_P_Photo lnkPPhoto, bool self = true);

        /// <summary>
        /// 获取商业街道图片单个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LNK_P_Photo GetBusinessStreetPhoto(int id, int fxtCompanyId);

        /// <summary>
        /// 添加商业街道图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <returns></returns>
        int AddBusinessStreetPhoto(LNK_P_Photo lnkPPhoto);

        /// <summary>
        /// 更新商业街道图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <param name="currentCompanyId">当前登陆的公司ID</param>
        /// <returns></returns>
        int UpdateBusinessStreetPhoto(LNK_P_Photo lnkPPhoto, int currentCompanyId);

        /// <summary>
        /// 删除商业街道图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <param name="currentCompanyId">当前登陆的公司ID</param>
        /// <returns></returns>
        int DeleteBusinessStreetPhoto(LNK_P_Photo lnkPPhoto, int currentCompanyId);

        #endregion


    }
}
