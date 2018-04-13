using CBSS.Tbx.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using System.Linq.Expressions;
using CBSS.Core.Utility;
using System.Data;

namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
        /// <summary>
        /// 获取图片库
        /// </summary>
        /// <param name="totalaount"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<ModelImgLibrary> GetModelImgLibraryList(out int totalcount, ModelImgLibraryRequest request = null)
        {
            request = request ?? new ModelImgLibraryRequest();
            List<Expression<Func<ModelImgLibrary, bool>>> exprlist = new List<Expression<Func<ModelImgLibrary, bool>>>();
            exprlist.Add(o => true);
            if (!string.IsNullOrEmpty(request.ModelID.ToString()))
                exprlist.Add(u => u.ModelID == request.ModelID);

            PageParameter<ModelImgLibrary> pageParameter = new PageParameter<ModelImgLibrary>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.CreateDate;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<ModelImgLibrary>(pageParameter, out totalcount);
        }
        /// <summary>
        ///  保存图片(0:失败，1:成功，2:重名)
        /// </summary>
        /// <param name="model"></param>
        public int SaveModelImgLibrary(ModelImgLibrary model)
        {
            if (model.ModelImgLibraryID > 0)
            {
                //验证重名
                var list = repository.SelectSearch<ModelImgLibrary>(a => a.ImgName == model.ImgName && a.ModelImgLibraryID != model.ModelImgLibraryID);
                if (list != null && list.Count() > 0)
                {
                    return 2;
                }
                return repository.Update<ModelImgLibrary>(model)? 1 : 0;
            }
            else
            {
                //验证重名
                var list = repository.SelectSearch<ModelImgLibrary>(a => a.ImgName == model.ImgName);
                if (list != null && list.Count() > 0)
                {
                    return 2;
                }
                return Convert.ToInt32(repository.Insert<ModelImgLibrary>(model).ToString()) > 0 ? 1 : 0;
            }
        } 
        /// <summary>
        /// 根据ID获取图片
        /// </summary>
        /// <param name="ModelImgLibraryID"></param>
        /// <returns></returns>
        public ModelImgLibrary GetModelImgLibrary(int ModelImgLibraryID)
        {
            return repository.SelectSearch<ModelImgLibrary>(m => m.ModelImgLibraryID == ModelImgLibraryID).SingleOrDefault();
        }
        public DataTable GetModelImgLibraryList(int ModuleID)
        {
          return  repository.SelectDataTable("select a.* from dbo.ModelImgLibrary a left join Module b on a.ModelID=b.ModelID where ModuleID='"+ ModuleID + "'"); 
        }
        /// <summary>
        /// 根据ModelImgLibraryID删除图片库信息
        /// </summary>
        /// <param name="ModelImgLibraryID"></param>
        /// <returns></returns>
        public bool DeleteModelImgLibrary(List<int> ids)
        {
            return repository.DeleteMore<ModelImgLibrary>(ids.Select(a=>a.ToString()).ToArray()); 
        }
    }
}
