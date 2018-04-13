using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;

namespace CBSS.Tbx.IBLL
{
    public interface IModelService
    {
        void SaveModel(Model model);
        /// <summary>
        /// 验证模型名称是否重名(true：重名)
        /// </summary>
        /// <returns></returns>
        bool CheckModelName(int ModelID, string ModelName);
        IEnumerable<Model> GetModelList();
        IEnumerable<Model> GetModelList(out int totalaount, ModelRequest request = null);
        Model GetModel(int id);

        void DeleteModel(List<int> ids);

        int DelModelByModelID(int ModelID);
        /// <summary>
        /// 根据parentId获取子分类分页
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IEnumerable<Model> GetModelByParentID(int parentId, out int totalcount, int pageNumber, int pageSize);
        /// <summary>
        /// 根据parentId获取子Model
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IEnumerable<Model> GetModelByParentID(int parentId);
    }
}
