using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.DAL
{
    public class UserStatisticsDal
    {
        BaseManagement manage = new BaseManagement();
        public List<UserStatisticsModel> GetUserStatistics(string where) 
        {
            List<UserStatisticsModel> list = new List<UserStatisticsModel>();
            
            SqlParameter[] ps =
            {
                new SqlParameter("@Where", SqlDbType.VarChar)
            };
            ps[0].Value = where;

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure,
                "Get_UserStatistics", ps);
            if (ds.Tables[0].Rows.Count > 0) 
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++) 
                {
                    UserStatisticsModel userSt = new UserStatisticsModel();
                    userSt.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());
                    userSt.UserID = Convert.ToInt32(ds.Tables[0].Rows[i]["UserID"].ToString());
                    userSt.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                    userSt.NickName = ds.Tables[0].Rows[i]["NickName"].ToString();
                    userSt.TelePhone = ds.Tables[0].Rows[i]["TelePhone"].ToString();
                    userSt.VersionName = ds.Tables[0].Rows[i]["VersionName"].ToString();
                    userSt.Number = Convert.ToInt32(ds.Tables[0].Rows[i]["Number"].ToString());
                    userSt.UseTime = Convert.ToInt32(ds.Tables[0].Rows[i]["UseTime"].ToString());
                    if (ds.Tables[0].Rows[i]["CreateTime"] is DBNull){
                        userSt.CreateTime=null;
                    }else
                    {
                        userSt.CreateTime = DateTime.Parse(ds.Tables[0].Rows[i]["CreateTime"].ToString());
                    }
                    userSt.LoginTime = DateTime.Parse(ds.Tables[0].Rows[i]["LoginTime"].ToString());
                    list.Add(userSt);
                }
            }
            return list;
        }

    }
}
