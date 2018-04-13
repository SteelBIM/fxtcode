using Kingsun.InterestDubbingGame.Common;
using Kingsun.InterestDubbingGame.Model;
using Kingsun.SynchronousStudy.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.DAL
{
    public class TB_InterestDubbingGame_PushMsgDAL : InterestDubbingBaseManagement
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 根据条件获TB_InterestDubbingGame_PushMsg
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<TB_InterestDubbingGame_PushMsg> GetList(string strWhere, string orderby = "")
        {
            try
            {
                return Search<TB_InterestDubbingGame_PushMsg>(strWhere, orderby);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据ID修改状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public bool UpdateState(string ID, string State)
        {
            try
            {
                string sql = string.Format("update TB_InterestDubbingGame_PushMsg set State='{1}' where ID='{0}'", ID, State);
                int count = SqlHelper.ExecuteNonQuery(AppSetting.InterestDubbingGameConnectionStr, CommandType.Text, sql);
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }
        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public bool Del(string ID)
        {
            try
            {
                string sql = string.Format("delete TB_InterestDubbingGame_PushMsg   where ID='{0}'", ID);
                int count = SqlHelper.ExecuteNonQuery(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, sql);
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(TB_InterestDubbingGame_PushMsg model)
        {
            try
            {
                return Insert<TB_InterestDubbingGame_PushMsg>(model);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(TB_InterestDubbingGame_PushMsg model)
        {
            try
            {
                return Update<TB_InterestDubbingGame_PushMsg>(model);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return false;
            }
        }
    }
}
