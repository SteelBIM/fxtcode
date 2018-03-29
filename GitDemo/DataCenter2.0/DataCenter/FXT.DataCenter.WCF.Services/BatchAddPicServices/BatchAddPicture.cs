using System;
using System.Configuration;
using System.IO;
using System.ServiceModel.Activation;
using System.Web;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using FXT.DataCenter.WCF.Contract;
using System.Net.Http;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WCF.Services.BatchAddPicServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BatchAddPicture : IBatchAddPicture
    {
        #region 住宅图片批量导入(楼盘，楼栋)

        public void ProjectPictures(string zipFilePath, string unZipFilePath, string userId, int cityId, int fxtCompanyId, string taskName)
        {
            IImportTask importTask = new ImportTask();
            var taskId = 0;

            var succNum = 0; //成功数量
            try
            {
                IDAT_Project project = new DAT_ProjectDAL();
                IDropDownList dropDownList = new DropDownList();

                //在任务列表创建一条记录
                var task = new DAT_ImportTask
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.楼盘图片批量上传,
                    CityID = cityId,
                    FXTCompanyId = fxtCompanyId,
                    CreateDate = DateTime.Now,
                    Creator = userId,
                    IsComplete = 0,
                    SucceedNumber = 0,
                    DataErrNumber = 0,
                    NameErrNumber = 0,
                    FilePath = ""
                };
                taskId = importTask.AddTask(task);


                //图片裁剪的高度和宽度
                var width = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailWidth"]);
                var height = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailHeight"]);

                //解压到当前目录
                CompressionHelper.Decompression(zipFilePath);

                //获取解压文件夹的所有子目录
                var subDirectories = FileHelper.FindSubDirectories(unZipFilePath);

                var index = 0; //记录图片大小超过500K的数量
                foreach (var item in subDirectories)
                {
                    var itemName = FileHelper.GetDirectoryName(item);

                    var splitArray = itemName.Split('#');
                    if (splitArray.Length < 3) continue;

                    var areaName = splitArray[0];
                    var projectName = splitArray[1];
                    var pictureTypeName = splitArray[2];

                    var areaId = dropDownList.GetAreaIdByName(cityId, areaName);
                    var projectId = project.IsProjectExist(cityId, areaId, fxtCompanyId, projectName);
                    var photoTypeCode = dropDownList.GetCodeByName(pictureTypeName, SYS_Code_Dict._图片类型_住宅);

                    if (projectId <= 0)
                    {
                        index++;
                        LogHelper.WriteLog("ProjectPictures", "", userId, cityId, fxtCompanyId, new Exception("楼盘不存在！"));
                        continue;
                    }

                    if (photoTypeCode <= 0)
                    {
                        index++;
                        LogHelper.WriteLog("ProjectPictures", "", userId, cityId, fxtCompanyId, new Exception("图片类型不存在！"));
                        continue;
                    }

                    var IsOss = ConfigurationHelper.IsOss;
                    var virtualPath = "/ProjectPic/" + cityId + "/" + projectId + "/";
                    //var directoryPath = MapPath(virtualPath);

                    //if (!Directory.Exists(directoryPath))
                    //    Directory.CreateDirectory(directoryPath);

                    var files = new DirectoryInfo(item).GetFiles();
                    foreach (var file in files)
                    {
                        if (file.Length > 524288)
                        {
                            index++;
                            continue;
                        }
                        SavePicture(file, virtualPath, height, width, photoTypeCode, projectId, 0, cityId, fxtCompanyId, project, IsOss);

                        file.Delete();
                        succNum++;
                    }
                }
                if (index > 0)
                {
                    FailPicture(importTask, taskId, unZipFilePath, "P.zip", succNum);
                }
                else
                {
                    importTask.UpdateTask(taskId, succNum, 0, 0, "", 1);
                }
            }
            catch (Exception ex)
            {
                FailPicture(importTask, taskId, unZipFilePath, "P.zip", succNum);
                LogHelper.WriteLog("ProjectPictures", "", userId, cityId, fxtCompanyId, ex);
            }
        }

        public void BuildingPictures(string zipFilePath, string unZipFilePath, string userId, int cityId, int fxtCompanyId, string taskName)
        {
            IImportTask importTask = new ImportTask();
            var taskId = 0;
            var succNum = 0;//成功数量
            try
            {
                IDAT_Project project = new DAT_ProjectDAL();
                IDAT_Building building = new DAT_BuildingDAL();
                IDropDownList dropDownList = new DropDownList();

                //在任务列表创建一条记录
                var task = new DAT_ImportTask
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.楼栋图片批量上传,
                    CityID = cityId,
                    FXTCompanyId = fxtCompanyId,
                    CreateDate = DateTime.Now,
                    Creator = userId,
                    IsComplete = 0,
                    SucceedNumber = 0,
                    DataErrNumber = 0,
                    NameErrNumber = 0,
                    FilePath = ""
                };
                taskId = importTask.AddTask(task);


                //图片裁剪的高度和宽度
                var width = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailWidth"]);
                var height = Convert.ToInt32(ConfigurationManager.AppSettings["thumbnailHeight"]);

                //解压到当前目录
                CompressionHelper.Decompression(zipFilePath);

                //获取解压文件夹的所有子目录
                var subDirectories = FileHelper.FindSubDirectories(unZipFilePath);

                var index = 0; //记录图片大小超过500K的数量

                foreach (var item in subDirectories)
                {
                    var itemName = FileHelper.GetDirectoryName(item);

                    var splitArray = itemName.Split('#');
                    if (splitArray.Length < 4) continue;

                    var areaName = splitArray[0];
                    var projectName = splitArray[1];
                    var buildingName = splitArray[2];
                    var pictureTypeName = splitArray[3];

                    var areaId = dropDownList.GetAreaIdByName(cityId, areaName);
                    var projectId = project.IsProjectExist(cityId, areaId, fxtCompanyId, projectName);
                    var buildingId = building.GetBuildingId(projectId, buildingName, cityId, fxtCompanyId);
                    var photoTypeCode = dropDownList.GetCodeByName(pictureTypeName, SYS_Code_Dict._图片类型_住宅);

                    if (projectId <= 0)
                    {
                        index++;
                        LogHelper.WriteLog("BuildingPictures", "", userId, cityId, fxtCompanyId, new Exception("楼盘不存在！"));
                        continue;
                    }

                    if (buildingId <= 0)
                    {
                        index++;
                        LogHelper.WriteLog("BuildingPictures", "", userId, cityId, fxtCompanyId, new Exception("楼栋不存在！"));
                        continue;
                    }

                    if (photoTypeCode <= 0)
                    {
                        index++;
                        LogHelper.WriteLog("BuildingPictures", "", userId, cityId, fxtCompanyId, new Exception("图片类型不存在！"));
                        continue;
                    }

                    var IsOss = ConfigurationHelper.IsOss;
                    var virtualPath = "/ProjectPic/" + cityId + "/" + projectId + "/" + buildingId + "/";
                    //var directoryPath = MapPath(virtualPath);

                    //if (!Directory.Exists(directoryPath))
                    //    Directory.CreateDirectory(directoryPath);

                    var files = new DirectoryInfo(item).GetFiles();
                    foreach (var file in files)
                    {
                        if (file.Length > 524288)
                        {
                            index++;
                            continue;
                        }
                        SavePicture(file, virtualPath, height, width, photoTypeCode, projectId, buildingId, cityId, fxtCompanyId, project, IsOss);
                        file.Delete();
                        succNum++;
                    }
                }
                if (index > 0)
                {
                    FailPicture(importTask, taskId, unZipFilePath, "B.zip", succNum);
                }
                else
                {
                    importTask.UpdateTask(taskId, succNum, 0, 0, "", 1);
                }
            }
            catch (Exception ex)
            {
                FailPicture(importTask, taskId, unZipFilePath, "B.zip", succNum);
                LogHelper.WriteLog("BuildingPictures", "", userId, cityId, fxtCompanyId, ex);
            }
        }

        private static void SavePicture(FileInfo file, string virtualPath, int height, int width, int photoTypeCode, int projectId, int buildingId, int cityId, int fxtCompanyId, IDAT_Project project, bool IsOss)
        {
            var fileExtension = Path.GetExtension(file.Name);//扩展名 如：.jpg
            var fileName = file.Name.Substring(0, file.Name.IndexOf('.'));//扩展名前的名称
            var fileNewName = new Random().Next(111111, 999999) + DateTime.Now.ToString("yyyyMMddmmHHssfff") + fileExtension;

            var virtualFilePath = Path.Combine(virtualPath, fileNewName);//服务器虚拟路径
            var physicalFilePath = MapPath(virtualFilePath);//服务器物理路径

            if (IsOss)
            {
                var fs = file.OpenRead();
                StreamContent content = new StreamContent(fs);
                var result = OssHelper.UpFileAsync(content, virtualFilePath);
            }
            else
            {
                var directoryPath = MapPath(virtualPath);

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                //保存原图
                file.CopyTo(physicalFilePath);

                //保存缩略图
                var thumbnailPath = physicalFilePath.Insert(physicalFilePath.LastIndexOf(".", StringComparison.Ordinal), "_t");
                ImageHandler.MakeThumbnail(physicalFilePath, thumbnailPath, width, height, "H");

            }
            //////保存原图
            ////file.CopyTo(physicalFilePath);

            //////保存缩略图
            ////var thumbnailPath = physicalFilePath.Insert(physicalFilePath.LastIndexOf(".", StringComparison.Ordinal), "_t");
            ////ImageHandler.MakeThumbnail(physicalFilePath, thumbnailPath, width, height, "H");

            ////修改OSS存储
            //var fs = file.OpenRead();
            //StreamContent content = new StreamContent(fs);
            //var result = OssHelper.UpFileAsync(content, virtualFilePath);

            //保存该条图片信息到数据库
            project.AddProjectPhoto(cityId, fxtCompanyId, projectId, photoTypeCode, virtualFilePath, fileName, buildingId);
        }

        private static void FailPicture(IImportTask importTask, int taskId, string filePath, string suffix, int succNum)
        {
            CompressionHelper.Compression(filePath, filePath + suffix);

            //var tmpRootDir = AppDomain.CurrentDomain.BaseDirectory;//获取程序根目录
            //var relativePath = (filePath + suffix).Replace(tmpRootDir, ""); //转换成相对路径
            //relativePath = relativePath.Replace(@"\", @"/");
            var indexPath = (filePath + suffix).IndexOf("NeedHandledFiles");
            var relativePath = string.Empty;
            if (indexPath >= 0)
            {
                relativePath = (filePath + suffix).Substring(indexPath);
                relativePath = relativePath.Replace(@"\", @"/");
            }
            importTask.UpdateTask(taskId, succNum, 0, 0, relativePath, -1);
        }

        #endregion

        #region 帮助程序

        /// <summary>
        /// 映射网站的根目录
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string MapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            strPath = strPath.Replace("/", "\\");
            if (strPath.StartsWith("\\"))
            {
                strPath = strPath.TrimStart('\\');
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        }

        #endregion
    }
}
