using CBSS.Cfgmanager.Contract.DataModel;
using CBSS.Cfgmanager.IBLL;
using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CBSS.Cfgmanager.BLL
{
    public partial class CfgmanagerService : ICfgmanagerService
    {
        public IEnumerable<Log4net> GetLog4net(out int totalcount, Log4netRequest request = null)
        {
            request = request ?? new Log4netRequest();
            List<Expression<Func<Log4net, bool>>> exprlist = new List<Expression<Func<Log4net, bool>>>();
            exprlist.Add(o => true);
            if (!string.IsNullOrEmpty(request.Level))
                exprlist.Add(u => u.Level.Contains(request.Level.Trim()));
            if (!string.IsNullOrEmpty(request.Logger) && request.Logger != "...请选择...")
                exprlist.Add(u => u.Logger.Contains(request.Logger.Trim()));
            PageParameter<Log4net> pageParameter = new PageParameter<Log4net>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.Id;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repositoryLog.SelectPage<Log4net>(pageParameter, out totalcount);
        }
        /// <summary>
        /// 删除接口信息
        /// </summary>
        /// <param name="ids"></param>
        public bool DeleteLog4net(List<int> ids)
        {
            string[] stringIDs = ids.Select(o => o.ToString()).ToArray();
            using (TransactionScope scope = new TransactionScope())
            {
                bool flag = repositoryLog.DeleteMore<Log4net>(stringIDs);
                if (flag == false)
                {
                    return false;
                }
                string Id = string.Join(",", stringIDs);
                string str = repositoryLog.SelectString("delete Log4net where Id in(" + Id + ")");
                scope.Complete();
                return true;
            }
        }
        public Log4net GetLog4net(int Id)
        {
            var model = repositoryLog.SelectSearch<Log4net>(a => a.Id == Id).First();
            return model;
        }
    }
}
