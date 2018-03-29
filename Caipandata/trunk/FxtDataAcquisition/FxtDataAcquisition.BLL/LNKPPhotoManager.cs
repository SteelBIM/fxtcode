using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Data;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class LNKPPhotoManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(LNKPPhotoManager));
        /// <summary>
        /// 获取楼盘照片个数
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int GetLNKPPhotoCountByProjectId(int projectId, int cityId, int companyId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            int count = 0;
            try
            {
                string sql = "{0} Valid=1 and CityId={1} and ProjectId={2} and FxtCompanyId={3} ";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_COUNT(NHibernateUtility.TableName_LNKPPhoto), cityId, projectId, companyId);
                object obj = db.DB.GetCustomSQLQueryUniqueResult<object>(sql, null);
                count = Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                log.Error("获取照片个数", ex);
            }
            db.Close();
            return count;
        }
        /// <summary>
        /// 获取楼盘照片
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<LNKPPhoto> GetLNKPPhotoByProjectId(int projectId, int cityId, int companyId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} Valid=1 and CityId={1} and ProjectId={2} and FxtCompanyId={3} ";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_LNKPPhoto), cityId, projectId, companyId);
                IList<LNKPPhoto> list = db.DB.GetCustomSQLQueryList<LNKPPhoto>(sql, null).ToList();
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        public static bool Insert(IList<LNKPPhoto> list, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                bool result = db.DB.Create<LNKPPhoto>(list);
                db.Close();
                return result;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static bool Delete(int projectId, int? buildingId, int cityId, int companyId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = string.Format("update {0} set Valid=0 where ProjectId={1} and CityId={2} and FxtCompanyId={3} and BuildingId {4}",
                  NHibernateUtility.TableName_LNKPPhoto, projectId, cityId, companyId, buildingId == null ? " is null " : "=" + Convert.ToInt32(buildingId));
                bool isDel = db.DB.DeleteBySQL(sql);
                db.Close();
                return isDel;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 获取要新增照片的实体
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="photoTypeCode"></param>
        /// <param name="path"></param>
        /// <param name="photoName"></param>
        /// <returns></returns>
        public static LNKPPhoto GetInsertLNKPPhotoObj(int projectId, int? buildingId, int cityId, int companyId, int? photoTypeCode, string path, string photoName)
        {
            LNKPPhoto obj = new LNKPPhoto { ProjectId = projectId, PhotoTypeCode = photoTypeCode, Path = path, PhotoDate = DateTime.Now, PhotoName = photoName, CityId = cityId, Valid = 1, FxtCompanyId = companyId, BuildingId = Convert.ToInt32(buildingId) };
            return obj;
        }
        public static void test()
        {
            DataBase db = new DataBase();
            try
            {
                string sql = "select projectid , sum(id) as cou  from LNK_P_Appendage group by projectId ";
                IList<testavg> li = db.DB.GetCustomSQLQuerytest<testavg>(sql, typeof(testavg), null).List<testavg>();
                db.Close();
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        #region
        /// <summary>
        /// 获取组织好的正式图片数据路径目录
        /// </summary>
        /// <param name="basePath">根目录</param>
        /// <param name="companyId"></param>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static string GetProjectPhotoPath(string basePath, int companyId, int projectid, int cityid)
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
        public static string GetProjectPhotoPath_Temporary(string basePath, long allotId, int companyId, int projectid, int cityid, int typeCode, string fileName, out string returnFileName)
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
        /// <param name="photoTypeCode"></param>
        public static void GetProjectPhotoInfoByFileName(string fileName, out int? photoTypeCode)
        {
            photoTypeCode = SYSCodeManager.PHOTOTYPECODE_9;
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
        }
        #endregion
    }
    public class testavg
    {
        public int projectid
        {
            get;
            set;
        }
        public int cou
        {
            get;
            set;
        }
    }
}
