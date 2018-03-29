using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace FXT.DataCenter.WCF.Contract
{
    [ServiceContract]
   public interface IBatchAddPicture
    {
        /// <summary>
        /// 批量添加楼盘图片
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径 如 D:\指定目录\1.zip</param>
        /// <param name="unZipFilePath">解压文件夹目录 如 D:\指定目录\1 </param>
        /// <param name="userId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="taskName"></param>
        [OperationContract(IsOneWay = true)]
        void ProjectPictures(string zipFilePath, string unZipFilePath,string userId,int cityId,int fxtCompanyId,string taskName);

        /// <summary>
        /// 批量添加楼栋图片
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径 如 D:\指定目录\1.zip</param>
        /// <param name="unZipFilePath">解压文件夹目录 如 D:\指定目录\1 </param>
        /// <param name="userId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="taskName"></param>
        [OperationContract(IsOneWay = true)]
        void BuildingPictures(string zipFilePath, string unZipFilePath, string userId, int cityId, int fxtCompanyId, string taskName);
    }
}
