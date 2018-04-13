using Kingsun.InterestDubbingGame.DAL;
using Kingsun.InterestDubbingGame.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.BLL
{
    public class TB_InterestDubbingGame_UserInfoBLL
    {
        TB_InterestDubbingGame_UserInfoDAL dal = new TB_InterestDubbingGame_UserInfoDAL();

        /// <summary>
        /// 条件查询
        /// </summary>    
        public IList<TB_InterestDubbingGame_UserInfo> GetListByWhere(string where)
        {
            return dal.GetListByWhere(where);
        }
        /// <summary>
        /// 获取用户查询信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<SearchUserInfo> GetSearchUserInfo(string sqlWhere)
        {
            return dal.GetSearchUserInfo(sqlWhere);
        }
         /// <summary>
        /// 获取报名用户信息，返回DataTable
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public DataTable GetSearchUserInfoTable(string sqlWhere)
        {
            return dal.GetSearchUserInfoTable(sqlWhere);
        }
          /// <summary>
        /// 根据UserID获取用户信息
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        public List<UserInfoModel> GetUserInfoModelByUserID(List<string> UserIDs)
        {
            return dal.GetUserInfoModelByUserID(UserIDs);
        }
    }
}
