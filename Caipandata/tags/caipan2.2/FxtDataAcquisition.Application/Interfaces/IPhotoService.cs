using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface IPhotoService
    {
        /// <summary>
        /// 获取组织好的正式图片数据路径目录
        /// </summary>
        /// <param name="basePath">根目录</param>
        /// <param name="companyId"></param>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        string GetProjectPhotoPath(string basePath, int companyId, int projectid, int cityid);
        /// <summary>
        /// 获取组织好的断点临时图片数据路径目录
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="allotId"></param>
        /// <param name="companyId"></param>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <param name="typeCode"></param>
        /// <param name="fileName">当前文件名</param>
        /// <param name="returnFileName">输出组织好的文件名</param>
        /// <returns></returns>
        string GetProjectPhotoPathTemporary(string basePath, long allotId, int companyId, int projectid, int cityid, int typeCode, string fileName, out string returnFileName);
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        bool Delete(int projectId, int? buildingId, int cityId, int companyId);
        /// <summary>
        /// 根据断点续传的临时文件名获取照片信息
        /// </summary>
        /// <param name="fileName"></param>
        int GetProjectPhotoInfoByFileName(string fileName);
    }
}
