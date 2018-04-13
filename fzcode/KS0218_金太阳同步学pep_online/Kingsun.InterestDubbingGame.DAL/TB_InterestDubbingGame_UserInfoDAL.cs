using Kingsun.InterestDubbingGame.Common;
using Kingsun.InterestDubbingGame.Model;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Kingsun.InterestDubbingGame.DAL
{
    public class TB_InterestDubbingGame_UserInfoDAL : InterestDubbingBaseManagement
    {
        public string getOssFilesURL = WebConfigurationManager.AppSettings["getOssFiles"];
        public string getFilesURL = WebConfigurationManager.AppSettings["getFiles"];
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_InterestDubbingGame_UserInfo> GetListByWhere(string where)
        {
            IList<TB_InterestDubbingGame_UserInfo> list = Search<TB_InterestDubbingGame_UserInfo>(where);
            return list;
        }

        /// <summary>
        /// 返回配音报名人数的统计数据
        /// </summary>
        /// <param name="where">不传参数返回所有；条件必须带AND</param>
        /// <returns></returns>
        public DataSet BackStatsData(string where)
        {
            string _str = string.Format(@"SELECT GradeID, COUNT(ID) total
                            from  [TB_InterestDubbingGame_UserInfo]
                            where {0}
                            group by GradeID", where);
            return ExecuteSql(_str);
        }

        /// <summary>
        /// 根据条件返回实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TB_InterestDubbingGame_UserInfo BackModel(string where)
        {
            return Search<TB_InterestDubbingGame_UserInfo>(where).FirstOrDefault();
        }
        /// <summary>
        /// 获取报名用户信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<SearchUserInfo> GetSearchUserInfo(string sqlWhere)
        {
            string sql = @" select *,
 case when a.GradeID<=2 then '低学段' when a.GradeID >2 and a.GradeID<=4 then '中学段' else '高学段' end 'Stage',
 ''as TotalScore
 ,(select DubbingFilePath from TB_InterestDubbingGame_UserContentsRecord where UserID=a.UserID and Type=0)'DubbingAddress',
 (select DubbingFilePath from TB_InterestDubbingGame_UserContentsRecord where UserID=a.UserID and Type=1)'ReadAddress'
 from dbo.TB_InterestDubbingGame_UserInfo a  where " + sqlWhere;
            DataSet set = ExecuteSql(sql);
            List<SearchUserInfo> list = JsonHelper.DataSetToIList<SearchUserInfo>(set, 0);
            return list;
        }
        /// <summary>
        /// 获取报名用户信息，返回DataTable
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public DataTable GetSearchUserInfoTable(string sqlWhere)
        {
            string sql = @" select *,
 case when a.GradeID<=2 then '低学段' when a.GradeID >2 and a.GradeID<=4 then '中学段' else '高学段' end 'Stage',
 ''as TotalScore
 ,(select DubbingFilePath from TB_InterestDubbingGame_UserContentsRecord where UserID=a.UserID and Type=0)'DubbingAddress',
 (select DubbingFilePath from TB_InterestDubbingGame_UserContentsRecord where UserID=a.UserID and Type=1)'ReadAddress'
 from dbo.TB_InterestDubbingGame_UserInfo a  where " + sqlWhere;
            DataTable dt = ExecuteSql(sql).Tables[0];
            return dt;
        }
        /// <summary>
        /// 根据UserID获取用户信息
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        public List<UserInfoModel> GetUserInfoModelByUserID(List<string> UserIDs)
        {
            try
            {
                if (UserIDs != null && UserIDs.Count > 0)
                {
                    string str = "";
                    str = string.Join(",", UserIDs);
                    string sql = "select UserID,UserImage,IsEnableOss from ITSV_Base.[FZ_SynchronousStudy].dbo.[Tb_userinfo]  where UserID in(" + str + ")";
                    DataSet set = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    List<UserInfoModel> listUserInfoModel = JsonHelper.DataSetToIList<UserInfoModel>(set, 0);
                    if (listUserInfoModel != null && listUserInfoModel.Count > 0)
                    {
                        List<UserInfoModel> list = new List<UserInfoModel>();
                        foreach (var item in listUserInfoModel)
                        {
                            if (item.IsEnableOss == 0)
                            {
                                item.UserImage = getFilesURL + "?FileID=" + item.UserImage;
                            }
                            else
                            {
                                item.UserImage = getOssFilesURL + item.UserImage;
                            }
                            list.Add(item);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Info("获取用户头像出错:" + ex.Message + ex.StackTrace);
                return null;
            }
            return null;
        }
    }
    public class UserInfoModel
    {
        public int  UserID { get; set; }
        public string UserImage { get; set; }
        /// <summary>
        /// 是否是oss文件（0：否，1：是）
        /// </summary>
        public int IsEnableOss { get; set; }
    }
}
