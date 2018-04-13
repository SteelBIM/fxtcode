using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Kingsun.SynchronousStudy.App.Controllers;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.App.Common
{

    public class UserManagement : BaseManagement
    {
        // string FiedURL = WebConfigurationManager.AppSettings["FileServerUrl"].ToString();
        public string IsEnableOss = WebConfigurationManager.AppSettings["IsEnableOss"];
        public string getOssFilesURL = WebConfigurationManager.AppSettings["getOssFiles"];
        public string getFilesURL = WebConfigurationManager.AppSettings["getFiles"];
        //  UserInfoBLL userBLL = new UserInfoBLL();
        //  UserInfoDAL userDAL = new UserInfoDAL();
        CourseBLL coursebll = new CourseBLL();
        readonly BaseManagement _bm = new BaseManagement();

        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        ///// <summary>
        ///// 同步用户信息
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <returns></returns>
        //public Tb_UserInfo InitUserInfo(string userid, int IsEnableOss)
        //{
        //    Kingsun.PSO.UUMSService.FZUUMS_UserService service = new PSO.UUMSService.FZUUMS_UserService();
        //    PSO.UUMSService.User serviceInfo = service.GetUserInfoByID(ProjectConstant.AppID, userid);
        //    if (serviceInfo != null)
        //    {
        //        string sql = string.Format(@"");
        //        Tb_UserInfo userinfo = _bm.SelectByCondition<Tb_UserInfo>("UserID='" + userid + "'  AND IsUser=1");
        //        if (userinfo != null)
        //        {
        //            if (!string.IsNullOrEmpty(userinfo.TelePhone))
        //            {
        //                userinfo.TelePhone = serviceInfo.Telephone.Length > 11 ? serviceInfo.Telephone.Substring(0, 11) : serviceInfo.Telephone;
        //            }
        //            else
        //            {
        //                userinfo.TelePhone = serviceInfo.Telephone;
        //            }

        //            userinfo.TrueName = serviceInfo.TrueName;
        //            userinfo.UserName = serviceInfo.UserName;
        //            userinfo.IsEnableOss = IsEnableOss;
        //            if (serviceInfo.ProID != null) userinfo.UserRoles = serviceInfo.ProID.Value;
        //            bool s = _bm.Update<Tb_UserInfo>(userinfo);
        //            if (s == false)
        //            {
        //                return null;
        //            }
        //            if (IsEnableOss == 0)
        //            {
        //                userinfo.UserImage = userinfo.UserImage;
        //            }
        //            else
        //            {
        //                if (!string.IsNullOrEmpty(userinfo.UserImage))
        //                {
        //                    userinfo.UserImage = userinfo.IsEnableOss != 0 ? getOssFilesURL + userinfo.UserImage : getFilesURL + "?FileID=" + userinfo.UserImage;
        //                }
        //                else
        //                {
        //                    userinfo.UserImage = userinfo.UserImage;
        //                }
        //            }

        //            return userinfo;
        //        }
        //        else
        //        {
        //            Tb_UserInfo userinfoList = new Tb_UserInfo();
        //            //int uid = 0;
        //            //int.TryParse(serviceInfo.UserID.ToString(), out uid); 
        //            userinfoList.UserID = string.IsNullOrEmpty(serviceInfo.UserID.ToString()) ? 0 : Convert.ToInt32(serviceInfo.UserID.ToString());
        //            if (!string.IsNullOrEmpty(userinfoList.TelePhone))
        //            {
        //                userinfoList.TelePhone = serviceInfo.Telephone.Length > 11 ? serviceInfo.Telephone.Substring(0, 11) : serviceInfo.Telephone;
        //            }
        //            else
        //            {
        //                userinfoList.TelePhone = serviceInfo.Telephone;
        //            }
        //            userinfoList.TrueName = serviceInfo.TrueName;
        //            userinfoList.UserName = serviceInfo.UserName.ToString();
        //            if (serviceInfo.ProID != null) userinfoList.UserRoles = serviceInfo.ProID.Value;
        //            userinfoList.NickName = NickName();
        //            userinfoList.IsUser = 1;
        //            userinfoList.isLogState = "0";
        //            userinfoList.UserImage = "00000000-0000-0000-0000-000000000000";
        //            userinfoList.CreateTime = serviceInfo.RegDate;
        //            userinfoList.IsEnableOss = IsEnableOss;
        //            bool s = _bm.Insert<Tb_UserInfo>(userinfoList);
        //            if (s == false)
        //            {
        //                return null;
        //            }
        //            if (IsEnableOss == 0)
        //            {
        //                userinfoList.UserImage = userinfoList.UserImage;
        //            }
        //            else
        //            {
        //                if (!string.IsNullOrEmpty(userinfoList.UserImage))
        //                {
        //                    userinfoList.UserImage = userinfoList.IsEnableOss != 0 ? getOssFilesURL + userinfoList.UserImage : getFilesURL + "?FileID=" + userinfoList.UserImage;
        //                }
        //                else
        //                {
        //                    userinfoList.UserImage = userinfoList.UserImage;
        //                }
        //            }
        //            return userinfoList;
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 判断手机号码是否已经被注册
        /// </summary>
        /// <param name="phonenum"></param>
        /// <returns></returns>
        public bool GetUserInfoByTelephone(string phonenum)
        {
            var user = userBLL.GetUserInfoByUserOtherID(phonenum, 1);
            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 获取最新的版本
        /// </summary>
        /// <returns></returns>
        public DataSet GetAppVersion(string versionID, int type)
        {
            //string sql = @"SELECT TOP 1 VersionNumber,State,FileAddress FROM TB_ApplicationVersion where VersionType=" + type + " and  state=1 ORDER BY CreateDate DESC";
            string sql = "";
            if (type == 1)
            {
                sql = "SELECT TOP 1 ID,VersionID, VersionNumber,VersionDescription,UserName,FileAddress,FileMD5,State,FileAddress,MandatoryUpdate,CreateDate FROM TB_ApplicationVersion where versionID =" + versionID + "  and VersionType =" + type + "  and  state=1 ORDER BY CreateDate DESC";
            }
            else
            {
                sql = "SELECT TOP 1 ID,VersionID, VersionNumber,VersionDescription,UserName,FileAddress,FileMD5,State,FileAddress,MandatoryUpdate,CreateDate FROM TB_ApplicationVersion where versionID =" + versionID + "  and VersionType !=" + type + "  and  state=1 ORDER BY CreateDate DESC";
            }
            DataSet ds = ExecuteSql(sql);

            if (ds == null || ds.Tables.Count < 1)
            {
                return null;
            }
            else
            {
                if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0) //判断里面的书籍为空
                {
                    return null;
                }
                else
                {
                    return ds;
                }

            }

        }


        public List<TB_ApplicationVersion> GetDataSet(DataSet ds)
        {
            return FillData<TB_ApplicationVersion>(ds.Tables[0]);
        }

        /// <summary>
        /// 获取随机2位字母+四位随机数
        /// </summary>
        /// <returns></returns>
        public string NickName()
        {
            string[] s1 = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            Random rand = new Random();

            return "同步学" + s1[rand.Next(0, s1.Length)] + s1[rand.Next(0, s1.Length)] + rand.Next(0, 9999);
        }

    }
}