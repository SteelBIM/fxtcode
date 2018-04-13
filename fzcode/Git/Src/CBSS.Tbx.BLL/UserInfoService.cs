using CBSS.Core.Utility;
using CBSS.Framework.DAL;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
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
        Repository respIbs = new Repository("IBS");
        public IEnumerable<TB_UserInfo> GetUserInfoList(out int totalcount, UserInfoRequest request = null)
        {
            request = request ?? new UserInfoRequest();

            List<Expression<Func<TB_UserInfo, bool>>> exprlist = new List<Expression<Func<TB_UserInfo, bool>>>();
            exprlist.Add(o => true);
            if (!string.IsNullOrEmpty(request.UserName))
                exprlist.Add(u => u.UserName.Contains(request.UserName.Trim()));
            if (!string.IsNullOrEmpty(request.TrueName))
                exprlist.Add(u => u.NickName.Contains(request.TrueName.Trim()));

            PageParameter<TB_UserInfo> pageParameter = new PageParameter<TB_UserInfo>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.CreateTime;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return respIbs.SelectPage<TB_UserInfo>(pageParameter, out totalcount);
        }
    }
}
