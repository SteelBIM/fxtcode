using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IBusinessStore
    {
        #region 商铺

        /// <summary>
        /// 查询商铺
        /// </summary>
        /// <param name="tenantBiz"></param>
        /// <param name="totalCount"></param>
        /// <param name="self">是否是自己</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<Dat_Tenant_Biz> GetTenantBiz(Dat_Tenant_Biz tenantBiz, int pageIndex, int pageSize, out int totalCount, bool self = true);

        /// <summary>
        /// 根据Id查询商铺
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <returns></returns>
        Dat_Tenant_Biz GetTenantBizById(int id, int cityId, int fxtCompanyId);

        /// <summary>
        /// 新增商铺
        /// </summary>
        /// <param name="tenantBiz"></param>
        /// <returns></returns>
        int AddTenantBiz(Dat_Tenant_Biz tenantBiz);

        /// <summary>
        /// 修改商铺
        /// </summary>
        /// <param name="tenantBiz"></param>
        /// <param name="currentCompanyId">当前登录的评估机构公司ID</param>
        /// <returns></returns>
        int UpdateTenantBiz(Dat_Tenant_Biz tenantBiz, int currentCompanyId);

        /// <summary>
        /// 删除商铺
        /// </summary>
        /// <param name="tenantBiz"></param>
        /// <param name="currentCompanyId">当前登录的评估机构公司ID</param>
        /// <returns></returns>
        int DeleteTenantBiz(Dat_Tenant_Biz tenantBiz, int currentCompanyId);

        #endregion


        #region 商铺图片

        /// <summary>
        /// 获取商铺图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <param name="self">是否为查询自己, true：查询自己</param>
        /// <returns></returns>
        IQueryable<LNK_H_Photo> GetBusinessStorePhotoes(LNK_H_Photo lnkPPhoto, bool self = true);

        /// <summary>
        /// 获取商铺图片单个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LNK_H_Photo GetBusinessStorePhoto(int id, int fxtCompanyId);

        /// <summary>
        /// 添加商铺图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <returns></returns>
        int AddBusinessStorePhoto(LNK_H_Photo lnkPPhoto);

        /// <summary>
        /// 更新商铺图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <param name="currentCompanyId">当前登陆的公司ID</param>
        /// <returns></returns>
        int UpdateBusinessStorePhoto(LNK_H_Photo lnkPPhoto, int currentCompanyId);

        /// <summary>
        /// 删除商铺图片
        /// </summary>
        /// <param name="lnkPPhoto">模型参数</param>
        /// <param name="currentCompanyId">当前登陆的公司ID</param>
        /// <returns></returns>
        int DeleteBusinessStorePhoto(LNK_H_Photo lnkPPhoto, int currentCompanyId);

        #endregion
    }
}
