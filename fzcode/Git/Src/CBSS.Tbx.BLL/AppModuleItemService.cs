using CBSS.Core.Utility;
using CBSS.Tbx.Contract;
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
        public IEnumerable<v_AppModuleItem> GetAppModuleItemList(out int totalcount, AppModuleItemRequest request = null)
        {
            request = request ?? new AppModuleItemRequest();
            var test = repository.SelectSearch<v_AppModuleItem>("");
            List<Expression<Func<v_AppModuleItem, bool>>> exprlist = new List<Expression<Func<v_AppModuleItem, bool>>>();
            exprlist.Add(o => true);
            exprlist.Add(u => u.AppID == request.AppID);
            if (!string.IsNullOrEmpty(request.ModuleName))
                exprlist.Add(u => u.ModuleName.Contains(request.ModuleName.Trim()));

            PageParameter<v_AppModuleItem> pageParameter = new PageParameter<v_AppModuleItem>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.AppModuleItemID;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<v_AppModuleItem>(pageParameter, out totalcount);
        }
    }
}
