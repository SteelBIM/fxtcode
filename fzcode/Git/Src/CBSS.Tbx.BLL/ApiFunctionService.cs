using CBSS.Framework.DAL;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using CBSS.Core.Utility;

namespace CBSS.Tbx.BLL
{
    public class ApiFunctionService : IApiFunctionService
    {

        Repository repository = new Repository("Cfgmanager");
        /// <summary>
        /// 保存接口信息
        /// </summary>
        /// <returns></returns>
        public bool SaveApiFunction(Sys_ApiFunction apiFun, List<Sys_ApiFunctionParam> param)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                int ApiFunctionID = 0;
                if (apiFun.ApiFunctionID != 0)//修改
                {
                    bool flag = repository.Update<Sys_ApiFunction>(apiFun);
                    if (flag)
                    {
                        repository.SelectString("delete Sys_ApiFunctionParam where ApiFunctionID=" + apiFun.ApiFunctionID + "");
                        ApiFunctionID = apiFun.ApiFunctionID;
                    }
                    else
                    {
                        return false;
                    }
                }
                else//新增
                {
                    object obj = repository.Insert<Sys_ApiFunction>(apiFun);
                    if (obj == null || Convert.ToInt32(obj) == 0)
                    {
                        return false;
                    }
                    ApiFunctionID = Convert.ToInt32(obj);
                }
                List<Sys_ApiFunctionParam> list = new List<Sys_ApiFunctionParam>();
                foreach (var item in param)
                {
                    item.CreateDate = DateTime.Now;
                    item.ApiFunctionID = ApiFunctionID;
                    list.Add(item);
                    repository.Insert(item);
                }
                //repository.InsertRange(list);
                scope.Complete();
                return true;
            }
        }
        /// <summary>
        /// 根据ApiFunctionID获取ApiFunction实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_ApiFunction GetApiFunction(int ApiFunctionID)
        {
            return repository.SelectSearch<Sys_ApiFunction>(u => u.ApiFunctionID == ApiFunctionID).SingleOrDefault();
        }
        /// <summary>
        /// 根据ApiFunctionIDs获取接口集合
        /// </summary>
        /// <param name="ApiFunctionIDs"></param>
        /// <returns></returns>
        public List<Sys_ApiFunction> GetApiFunctionList(string ApiFunctionIDs)
        {
            return repository.SelectSearch<Sys_ApiFunction>("ApiFunctionID in(" + ApiFunctionIDs + ")").ToList();
        }
        public List<Sys_ApiFunction> GetApiFunctionList(string ApiFunctionName, string SystemCode)
        {
            return repository.SelectAppointField<Sys_ApiFunction>(a => a.ApiFunctionName.Contains(ApiFunctionName) && a.SystemCode.Contains(SystemCode), "ApiFunctionID").ToList();
        }
        /// <summary>
        /// 根据ApiFunctionIDs获取接口参数集合
        /// </summary>
        /// <param name="ApiFunctionIDs"></param>
        /// <returns></returns>
        public List<Sys_ApiFunctionParam> GetApiFunctionParamList(string ApiFunctionIDs)
        {
            return repository.SelectSearch<Sys_ApiFunctionParam>("ApiFunctionID in(" + ApiFunctionIDs + ")").ToList();
        }
        /// <summary>
        /// 根据ApiFunctionID获取ApiFunctionParam集合实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Sys_ApiFunctionParam> GetApiFunctionParam(int ApiFunctionID)
        {
            return repository.SelectSearch<Sys_ApiFunctionParam>(a => a.ApiFunctionID == ApiFunctionID).ToList();
        }
        /// <summary>
        /// 获取接口信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Sys_ApiFunction> GetAllSys_ApiFunction(out int totalcount, ApiFunctionRequest request = null)
        {
            request = request ?? new ApiFunctionRequest();
            List<Expression<Func<Sys_ApiFunction, bool>>> exprlist = new List<Expression<Func<Sys_ApiFunction, bool>>>();
            exprlist.Add(o => true);
            if (!string.IsNullOrEmpty(request.ApiFunctionName))
                exprlist.Add(u => u.ApiFunctionName.Contains(request.ApiFunctionName.Trim()));

            if (!string.IsNullOrEmpty(request.SystemCode))
                exprlist.Add(u => u.SystemCode.Contains(request.SystemCode.Trim()));

            PageParameter<Sys_ApiFunction> pageParameter = new PageParameter<Sys_ApiFunction>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.CreateDate;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<Sys_ApiFunction>(pageParameter, out totalcount);

            //var roles = repository.ListAll<Sys_ApiFunction>(); 
            //if (!string.IsNullOrEmpty(request.ApiFunctionName))
            //{
            //    roles = roles.Where(u => u.ApiFunctionName.Contains(request.ApiFunctionName.Trim()));
            //}
            //else if (!string.IsNullOrEmpty(request.SystemCode))
            //{
            //    roles = roles.Where(u => u.SystemCode.Contains(request.SystemCode.Trim()));
            //}
            //else if (!string.IsNullOrEmpty(request.SystemCode) && !string.IsNullOrEmpty(request.ApiFunctionName))
            //{
            //    roles = roles.Where(u => u.ApiFunctionName.Contains(request.ApiFunctionName.Trim()) && u.SystemCode.Contains(request.SystemCode.Trim()));
            //} 
            //return roles.OrderByDescending(u => u.ApiFunctionID).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
        }
        /// <summary>
        /// 删除接口信息
        /// </summary>
        /// <param name="ids"></param>
        public bool DeleteApiFunction(List<int> ids)
        {
            string[] stringIDs = ids.Select(o => o.ToString()).ToArray();
            using (TransactionScope scope = new TransactionScope())
            {
                bool flag = repository.DeleteMore<Sys_ApiFunction>(stringIDs);
                if (flag == false)
                {
                    return false;
                }
                string ApiFunctionID = string.Join(",", stringIDs);
                string str = repository.SelectString("delete Sys_ApiFunctionParam where ApiFunctionID in(" + ApiFunctionID + ")");
                scope.Complete();
                return true;
            }
        }
    }
}
