using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
    public interface IModelImgLibrary
    {
        /// <summary>
        /// 获取图片库
        /// </summary>
        /// <param name="totalaount"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<ModelImgLibrary> GetModelImgLibraryList(out int totalaount, ModelImgLibraryRequest request = null);

        /// <summary>
        /// 保存图片(0:失败，1:成功，2:重名)
        /// </summary>
        /// <param name="model"></param>
        int SaveModelImgLibrary(ModelImgLibrary model);
        /// <summary>
        /// 根据ID获取图片
        /// </summary>
        /// <param name="ModelImgLibraryID"></param>
        /// <returns></returns>
        ModelImgLibrary GetModelImgLibrary(int ModelImgLibraryID);
        DataTable GetModelImgLibraryList(int ModuleID);

        /// <summary>
        /// 根据ModelImgLibraryID删除图片库信息
        /// </summary>
        /// <param name="ModelImgLibraryID"></param>
        /// <returns></returns>
        bool DeleteModelImgLibrary(List<int> ids);
    }
}
