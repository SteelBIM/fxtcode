using CBSS.Tbx.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract;
using System.Linq.Expressions;
using CBSS.Core.Utility;
using CBSS.Framework.DAL;
using CBSS.Tbx.Contract.ViewModel;

namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
        public AppVersion GetAppVersion(int id)
        {
            return repository.SelectSearch<AppVersion>(m => m.AppVersionID == id).SingleOrDefault();
        }
        public IEnumerable<v_AppVersion> GetAppVersionList(out int totalcount, AppVersionRequest request = null)
        {
            request = request ?? new AppVersionRequest();

            List<Expression<Func<v_AppVersion, bool>>> exprlist = new List<Expression<Func<v_AppVersion, bool>>>();
            if (!string.IsNullOrEmpty(request.AppID.ToString()))
                exprlist.Add(u => u.AppID == request.AppID);
            if (!string.IsNullOrEmpty(request.AppType.ToString()))
                exprlist.Add(u => u.AppType == request.AppType);

            PageParameter<v_AppVersion> pageParameter = new PageParameter<v_AppVersion>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.CreateDate;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<v_AppVersion>(pageParameter, out totalcount);
        }
        /// <summary>
        /// 保存版本
        /// </summary>
        /// <param name="model"></param>
        public int SaveAppVersion(AppVersion model)
        { 
            if (model.AppVersionID > 0)
            {
                //验证重名
                var list = repository.SelectSearch<AppVersion>(a => a.AppID == model.AppID && a.AppType == model.AppType && a.AppVersionNumber == model.AppVersionNumber && a.AppVersionID != model.AppVersionID);
                if (list != null && list.Count() > 0)
                {
                    return 2;
                }
                return repository.Update<AppVersion>(model) ? 1 : 0;
            }
            else
            {
                //验证重名
                var list = repository.SelectSearch<AppVersion>(a => a.AppID == model.AppID && a.AppType == model.AppType && a.AppVersionNumber == model.AppVersionNumber);
                if (list != null && list.Count() > 0)
                {
                    return 2;
                }
                var obj = repository.InsertReturnEntity<AppVersion>(model);
                return obj != null ? 1 : 0;
            }
        }

        public bool DelAppVersion(int AppVersionID)
        {
            return repository.Delete<AppVersion>(AppVersionID);
        }
        public IEnumerable<AppVersion> GetAppVersion(Expression<Func<AppVersion, bool>> expression)
        {
            return repository.SelectSearch<AppVersion>(expression);
        }
    }
}
