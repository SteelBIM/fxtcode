using CBSS.Core.Utility;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
        /// <summary>
        /// 通过皮肤版本ID获取皮肤版本数据
        /// </summary>
        /// <param name="id">皮肤版本ID</param>
        /// <returns></returns>
        public AppSkinVersion GetAppSkinVersion(int id)
        {
            return repository.SelectSearch<AppSkinVersion>(m => m.SkinVersionID == id).SingleOrDefault();
        }
        /// <summary>
        /// 通过应用ID 分页获取皮肤版本数据
        /// </summary>
        /// <param name="totalcount">数据总量</param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<V_AppSkinVersion> GetAppSkinVersionList(out int totalcount, AppVersionRequest request = null)
        {
            request = request ?? new AppVersionRequest();

            List<Expression<Func<V_AppSkinVersion, bool>>> exprlist = new List<Expression<Func<V_AppSkinVersion, bool>>>();
            if (!string.IsNullOrEmpty(request.AppID.ToString()))
                exprlist.Add(u => u.AppID == request.AppID);
            if (!string.IsNullOrEmpty(request.AppType.ToString()))
                exprlist.Add(u => u.AppType == request.AppType);

            PageParameter<V_AppSkinVersion> pageParameter = new PageParameter<V_AppSkinVersion>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.CreateDate;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;

            return repository.SelectPage<V_AppSkinVersion>(pageParameter, out totalcount);
        }
        /// <summary>
        /// 更新皮肤版本
        /// </summary>
        /// <param name="model"></param>
        public void SaveAppSkinVersion(AppSkinVersion model)
        {
            if (model.SkinVersionID > 0)
                repository.Update<AppSkinVersion>(model);
            else
                repository.Insert<AppSkinVersion>(model);
        }
    }
}
