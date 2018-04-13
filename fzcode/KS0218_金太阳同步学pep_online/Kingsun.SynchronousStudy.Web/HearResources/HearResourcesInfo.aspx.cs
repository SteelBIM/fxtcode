using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.HearResources
{
    public partial class HearResourcesInfo : System.Web.UI.Page
    {
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();

        HearResourcesBLL hearResourcesBLL = new HearResourcesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                InitAction(Request.QueryString["action"].ToLower());
            }
        }
        private void InitAction(string action)
        {
            switch (action)
            {
                case "getlistenspeakachievement":
                    GetListenSpeakAchievement();
                    break;
                //case "download":
                //    DownLoad();
                //    break;
            }
        }

        #region
        /// <summary>
        /// 获取用户分享详情
        /// </summary>
        private void GetListenSpeakAchievement()
        {
            try
            {
                int userID = int.Parse(Request.Form["UserID"]);
                int bookID = int.Parse(Request.Form["BookID"]);
                int firstTitleID = int.Parse(Request.Form["FirstTitleID"]);
                int secondTitleID = int.Parse(Request.Form["SecondTitleID"]);
                int firstModularID = int.Parse(Request.Form["FirstModularID"]);
                int secondModularID = int.Parse(Request.Form["SecondModularID"]);
                string where = "1=1";
                where += " and BookID = " + bookID;
                where += " and FirstTitleID = " + firstTitleID;
                if (secondTitleID != 0)
                {
                    where += " and SecondTitleID = " + secondTitleID;
                }
                where += " and FirstModularID = " + firstModularID;
                where += " and SecondModularID = " + secondModularID;
                var user = userBLL.GetUserInfoByUserId(userID);
                Tb_UserInfo tbuser = null;
                if (user != null) 
                {
                    tbuser = new Tb_UserInfo();
                    tbuser.UserID = Convert.ToInt32(user.UserID);
                    tbuser.TrueName = user.TrueName;
                    tbuser.UserName = user.UserName;
                    tbuser.UserRoles = user.UserRoles;
                    tbuser.TelePhone = user.TelePhone;
                    tbuser.NickName = user.TrueName;
                    tbuser.IsUser = user.IsUser;
                    tbuser.isLogState = user.isLogState;
                    tbuser.IsEnableOss = user.IsEnableOss;
                    tbuser.CreateTime = user.Regdate;
                    tbuser.AppId = user.AppID;
                    tbuser.UserImage = user.UserImage;
                }
                

                IList<TB_HearResources> hearResourcesList = hearResourcesBLL.GetModuleList(where);
                List<HearResourcesInfoModel> resourcesList = new List<HearResourcesInfoModel>();
                if (hearResourcesList != null && hearResourcesList.Count > 0)
                {
                    string questString = "";
                    for (int i = 0, length = hearResourcesList.Count; i < length; i++)
                    {
                        questString = "SELECT TOP 1 * FROM [FZ_HearResources].[dbo].[TB_UserHearResources] where 1=1";
                        questString += " and UserID = " + userID;
                        questString += " and BookID = " + bookID;
                        questString += " and SerialNumber = " + hearResourcesList[i].SerialNumber;
                        questString += " and FirstTitleID = " + firstTitleID;
                        if (secondTitleID != 0)
                        {
                            questString += " and SecondTitleID = " + secondTitleID;
                        }
                        if (hearResourcesList[i].TextSerialNumber != null)
                        {
                            questString += " and TextSerialNumber = " + hearResourcesList[i].TextSerialNumber;
                        }
                        questString += " ORDER BY AverageScore desc";
                        DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, questString);
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
                        {
                            TB_UserHearResources hearResourInfo = DataSetToIList<TB_UserHearResources>(ds, 0)[0];
                            HearResourcesInfoModel resourcesInfo = new HearResourcesInfoModel();
                            resourcesInfo.TextDesc = hearResourcesList[i].TextDesc;
                            resourcesInfo.TextSerialNumber = hearResourInfo.TextSerialNumber;
                            resourcesInfo.SerialNumber = hearResourInfo.SerialNumber;
                            resourcesInfo.TotalScore = hearResourInfo.TotalScore;
                            resourcesInfo.AverageScore = hearResourInfo.AverageScore;
                            resourcesInfo.CreateDate = hearResourInfo.CreateTime;
                            resourcesInfo.VideoFileID = hearResourInfo.VideoFileID;
                            resourcesList.Add(resourcesInfo);
                        }
                    }
                    resourcesList.Sort();
                }

                
                Response.Write(JsonHelper.EncodeJson( new
                {
                    Success = true,
                    ResourcesList = resourcesList,
                    UserInfo = tbuser
                }));

            }
            catch (Exception ex)
            {
                Response.Write(JsonHelper.EncodeJson(new { Msg = ex, Success = false }));
            }
            finally
            {
                Response.End();
            }

        }

        /// <summary>
        /// 通过文件ID从文件服务器下载文件
        /// </summary>
        private void DownLoad()
        {
            string FileID = Request.Form["FileID"];
            string FileName = "";
            string path = Server.MapPath("/HearResourcesShareResult/audio/");
            string FullFilePath = System.Configuration.ConfigurationManager.AppSettings["FileServerUrl"].ToString() + "GetFiles.ashx?FileID=";
            //string FullFilePath = @"http://file.kingsun.cn/GetFiles.ashx?FileID=";
            IList<bool> boolList = new List<bool>();
            if (FileID.Contains(','))
            {
                string[] FileArr = FileID.Split(',');
                string filePath = "";
                for (int i = 0, length = FileArr.Length; i < length; i++)
                {
                    filePath = FullFilePath;
                    FileName = path + FileArr[i] + ".mp3";
                    filePath += FileArr[i];
                    try
                    {
                        WebClient wc = new WebClient();
                        wc.Headers["User-Agent"] = "blah";
                        wc.DownloadFile(filePath, FileName);
                    }
                    catch (Exception)
                    {
                        //
                    }
                    finally
                    {
                        if (File.Exists(FileName))
                        {
                            boolList.Add(true);
                        }
                        else
                        {
                            boolList.Add(false);
                        }
                    }

                }

                Response.Write(JsonHelper.EncodeJson(new { Success = true, BoolList = boolList }));
                Response.End();
            }
            else
            {
                FileName = path + FileID + ".mp3";
                FullFilePath += FileID;
                try
                {
                    WebClient wc = new WebClient();
                    wc.Headers["User-Agent"] = "blah";
                    wc.DownloadFile(FullFilePath, FileName);
                    boolList.Add(true);
                    Response.Write(JsonHelper.EncodeJson(new { Success = true, BoolList = boolList }));
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    boolList.Add(false);
                    Response.Write(JsonHelper.EncodeJson(new { Success = false, BoolList = boolList, Msg = msg }));
                }
                finally
                {
                    Response.End();
                }
            }
        }

        #endregion

        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="ds">DataSet</param> 
        /// <param name="tableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;

            DataTable p_Data = ds.Tables[tableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
    }

    partial class HearResourcesInfoModel : IComparable<HearResourcesInfoModel>
    {
        public int? SerialNumber { set; get; }
        public int? TextSerialNumber { set; get; }
        public double? TotalScore { set; get; }
        public double? AverageScore { set; get; }
        public string TextDesc { set; get; }
        public DateTime? CreateDate { set; get; }
        public string VideoFileID { set; get; }

        //排序
        public int CompareTo(HearResourcesInfoModel obj)
        {
            if (this.SerialNumber == obj.SerialNumber)
            {
                return Convert.ToInt32(this.TextSerialNumber - obj.TextSerialNumber);
            }
            else
            {
                return Convert.ToInt32(this.SerialNumber - obj.SerialNumber);
            }
        }
    }
}