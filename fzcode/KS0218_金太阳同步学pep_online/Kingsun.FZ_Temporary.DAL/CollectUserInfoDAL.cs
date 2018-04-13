using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.FZ_Temporary.Model;
using Kingsun.FZ_Temporary.Common;
using System.Data;
using System.Reflection;
using log4net;


namespace Kingsun.FZ_Temporary.DAL
{
    public class CollectUserInfoDAL
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCollectUserInfo(CollectUserInfoModel model)
        {
            try
            {
                string sql = string.Format("insert into CollectUserInfo(Name,TelePhone,FromUrl)values('{0}','{1}','{2}')", model.Name, model.TelePhone, model.FromUrl);
                int count = SqlHelper.ExecuteNonQuery(AppSetting.ConnectionString, CommandType.Text, sql);
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }
        /// <summary>
        /// 根据电话号码判断是否存在
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public bool CheckCollectUserInfo(string Phone)
        {
            try
            {
                string sql = string.Format("select COUNT(1) from dbo.CollectUserInfo where TelePhone='{0}'", Phone);
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(AppSetting.ConnectionString, CommandType.Text, sql));
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }
        /// <summary>
        /// 添加验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddPhoneCode(Tb_PhoneCode model)
        {
            try
            {
                string sql = string.Format("insert into Tb_PhoneCode(TelePhone,Code)values('{0}','{1}')", model.TelePhone, model.Code);
                int count = SqlHelper.ExecuteNonQuery(AppSetting.ConnectionString, CommandType.Text, sql);
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }
        /// <summary>
        /// 根据Telephone获取Tb_PhoneCode信息
        /// </summary>
        /// <param name="Telephone"></param>
        /// <returns></returns>
        public DataSet GetPhoneCode(string Telephone)
        {
            try
            {
                string sql = string.Format("  SELECT TOP 1 * FROM dbo.Tb_PhoneCode WHERE TelePhone='{0}'  AND EndDate>'{1}' AND State=1  ORDER BY EndDate DESC", Telephone, DateTime.Now);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                return ds;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return null;
            }
        }
    }
}

