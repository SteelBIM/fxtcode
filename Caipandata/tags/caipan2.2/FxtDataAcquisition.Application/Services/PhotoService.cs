using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Application.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhotoService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public bool Delete(int projectId, int? buildingId, int cityId, int companyId)
        {
            var ups = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == projectId && m.CityId == cityId && m.FxtCompanyId == companyId && m.Valid == 1);
            if (ups != null && ups.Count() > 0)
            {
                foreach (var item in ups)
                {
                    item.Valid = 0;
                    _unitOfWork.P_PhotoRepository.Update(item);
                }
                return _unitOfWork.Commit() > 0;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获取组织好的正式图片数据路径目录
        /// </summary>
        /// <param name="basePath">根目录</param>
        /// <param name="companyId"></param>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public string GetProjectPhotoPath(string basePath, int companyId, int projectid, int cityid)
        {

            string searchPath = new StringBuilder().Append(basePath).Append("/").Append(companyId)
                                 .Append("/").Append(DateTime.Now.ToString("yyyy-MM-dd"))
                                 .Append("/").Append(projectid).Append("_").Append(cityid).ToString();
            return searchPath;
        }

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
        public string GetProjectPhotoPathTemporary(string basePath, long allotId, int companyId, int projectid, int cityid, int typeCode, string fileName, out string returnFileName)
        {
            if (typeCode == 0)
            {
                typeCode = SYSCodeManager.PHOTOTYPECODE_9;
            }
            returnFileName = fileName;
            string searchPath = new StringBuilder().Append(basePath).Append("/").Append(companyId)
               .Append("/").Append(allotId)
               .Append("/").Append(projectid).Append("_").Append(cityid).ToString();
            returnFileName = typeCode.ToString() + "_" + fileName;
            //log.Info("图片路径：" + returnFileName + ",projectid:" + projectid + ",typeCode:" + typeCode);
            return searchPath;
        }

        /// <summary>
        /// 根据断点续传的临时文件名获取照片信息
        /// </summary>
        /// <param name="fileName"></param>
        public int GetProjectPhotoInfoByFileName(string fileName)
        {
            int photoTypeCode = SYSCodeManager.PHOTOTYPECODE_9;
            if (!string.IsNullOrEmpty(fileName))
            {
                string str = fileName.Split('_')[0];
                if (str.CheckInteger())
                {
                    photoTypeCode = Convert.ToInt32(str);
                }
            }
            if (Convert.ToInt32(photoTypeCode) == 0)
            {
                photoTypeCode = SYSCodeManager.PHOTOTYPECODE_9;
            }
            return photoTypeCode;
        }
    }
}
