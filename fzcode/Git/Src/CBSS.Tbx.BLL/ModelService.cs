using CBSS.Framework.DAL;
using CBSS.Core.Utility;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CBSS.Tbx.IBLL;

namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
        public void SaveModel(Model model)
        {

            if (model.ModelID > 0)
            {
                repository.Update<Model>(model);
            }
            else
            {
                repository.Insert<Model>(model);
            }
        }

        //public IEnumerable<Model> GetModelList(ModelRequest request = null)
        //{
        //    request = request ?? new ModelRequest();

        //    var model = repository.ListAll<Model>();

        //    if (!string.IsNullOrEmpty(request.ModelName))
        //    {
        //        model = model.Where(u => u.ModelName.Contains(request.ModelName));
        //    }

        //    return model.OrderByDescending(m => m.ModelID).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
        //}

        public IEnumerable<Model> GetModelList()
        {
            return repository.ListAll<Model>();
        }

        public IEnumerable<Model> GetModelList(out int totalcount, ModelRequest request = null)
        {
            request = request ?? new ModelRequest();

            List<Expression<Func<Model, bool>>> exprlist = new List<Expression<Func<Model, bool>>>();
            exprlist.Add(o => true);
            if (!string.IsNullOrEmpty(request.ModelName))
                exprlist.Add(u => u.ModelName.Contains(request.ModelName.Trim()));

            PageParameter<Model> pageParameter = new PageParameter<Model>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.Sort;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<Model>(pageParameter, out totalcount);
        }
      

        public Model GetModel(int id)
        {
            return repository.SelectSearch<Model>(m => m.ModelID == id).SingleOrDefault();
        }

        public void DeleteModel(List<int> ids)
        {
            string[] stringIDs = ids.Select(o => o.ToString()).ToArray();
            repository.DeleteMore<Model>(stringIDs);
        }
        /// <summary>
        /// 根据ModelID删除模型(0：已经生成具体的模块的模型则不能删除，1：删除成功，2：删除失败)
        /// </summary>
        /// <param name="ModelID"></param>
        /// <returns></returns>
        public int DelModelByModelID(int ModelID)
        {
            IEnumerable<Module> listModule = repository.SelectSearch<Module>(a => a.ModelID == ModelID);
            IEnumerable<Model> listModel = repository.SelectSearch<Model>(a => a.ParentID == ModelID);
            if ((listModule != null && listModule.Count() > 0) || (listModel != null && listModel.Count() > 0))
            {
                return 0;
            }
            else
            {
                return repository.Delete<Model>(ModelID) ? 1 : 2;
            }
        }
        /// <summary>
        /// 根据parentId获取子分类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IEnumerable<Model> GetModelByParentID(int parentId, out int totalcount, int pageNumber, int pageSize)
        { 
            List<Expression<Func<Model, bool>>> exprlist = new List<Expression<Func<Model, bool>>>();
            exprlist.Add(o => true); 
            exprlist.Add(o => o.ParentID == parentId);

            PageParameter<Model> pageParameter = new PageParameter<Model>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = pageNumber;
            pageParameter.PageSize = pageSize;
            pageParameter.OrderColumns = p => p.Sort;
            pageParameter.IsOrderByASC = 1;
            pageParameter.OrderColumns2 = p => p.CreateDate;
            pageParameter.IsOrderByASC2 = 0;
            totalcount = 0;
            return repository.SelectPage<Model>(pageParameter, out totalcount);

            //var model = repository.SelectSearch<Model>(o => o.ParentID == parentId).OrderBy(a=>a.Sort);
            //return model.ToList();
        }
        /// <summary>
        /// 根据parentId获取子Model
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IEnumerable<Model> GetModelByParentID(int parentId)
        { 
            var model = repository.SelectSearch<Model>(o => o.ParentID == parentId && o.Status==1).OrderBy(a => a.Sort);
            return model.ToList();
        }
        /// <summary>
        /// 验证模型名称是否重名(true：重名)
        /// </summary>
        /// <returns></returns>
        public bool CheckModelName(int ModelID, string ModelName)
        {
            if (ModelID>0)//修改
            {
                var list = repository.SelectSearch<Model>(a => a.ModelName == ModelName && a.ModelID != ModelID);
                if (list != null && list.Count() > 0)
                {
                    return true;
                }
            }
            else//新增
            {
                var list = repository.SelectSearch<Model>(a => a.ModelName == ModelName);
                if (list != null && list.Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
