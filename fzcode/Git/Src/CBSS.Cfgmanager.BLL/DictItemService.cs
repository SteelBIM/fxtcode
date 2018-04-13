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
        public IEnumerable<Sys_DictItem> GetAllSys_DictItem(out int totalcount, ApiFunctionRequest request = null)
        {
            request = request ?? new ApiFunctionRequest();
            List<Expression<Func<Sys_DictItem, bool>>> exprlist = new List<Expression<Func<Sys_DictItem, bool>>>();
            exprlist.Add(o => true);
            if (!string.IsNullOrEmpty(request.ApiFunctionName))
                exprlist.Add(u => u.DictName.Contains(request.ApiFunctionName.Trim()));

            if (request.SystemCode > 0)
                exprlist.Add(u => u.SystemCode == request.SystemCode);

            PageParameter<Sys_DictItem> pageParameter = new PageParameter<Sys_DictItem>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.CreateDate;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<Sys_DictItem>(pageParameter, out totalcount);
        }
        public bool DeleteDictItem(List<int> ids)
        {
            string[] stringIDs = ids.Select(o => o.ToString()).ToArray();
            using (TransactionScope scope = new TransactionScope())
            {
                bool flag = repository.DeleteMore<Sys_DictItem>(stringIDs);
                if (flag == false)
                {
                    return false;
                }
                string DictItemID = string.Join(",", stringIDs);
                string str = repository.SelectString("delete Sys_DictItem where DictItemID in(" + DictItemID + ")");
                scope.Complete();
                return true;
            }
        }

        public void SaveDictItem(Sys_DictItem model)
        {
            if (!string.IsNullOrEmpty(model.DictItemID.ToString())&& model.DictItemID!=0)
                repository.Update<Sys_DictItem>(model);
            else
                repository.Insert<Sys_DictItem>(model);
        }

        public List<Sys_DictItem> GetDictItemList(string DictItemName, int SystemCode)
        {
            if (SystemCode > 0)
            {
                return repository.SelectAppointField<Sys_DictItem>(a => a.DictName.Contains(DictItemName) && a.SystemCode == SystemCode, "DictItemID").ToList();
            }
            return repository.SelectAppointField<Sys_DictItem>(a => a.DictName.Contains(DictItemName), "DictItemID").ToList();
        }
        public List<Sys_DictItem> GetDictItemList(string DictItemIDs)
        {
            return repository.SelectSearch<Sys_DictItem>("DictItemID in(" + DictItemIDs + ")").ToList();
        }
        public Sys_DictItem GetDictItem(int DictItemID)
        {
            return repository.SelectSearch<Sys_DictItem>(u => u.DictItemID == DictItemID).SingleOrDefault();
        }
    }
}
