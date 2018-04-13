using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.InterestDubbingGame.DAL;
using Kingsun.InterestDubbingGame.Model;

namespace Kingsun.InterestDubbingGame.BLL
{
    public class InterestDubbingGameMatchApiBLL
    {
        private InterestDubbingGameMatchApiDAL idg = new InterestDubbingGameMatchApiDAL();

        /// <summary>
        /// 获取课本剧资源
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public DataSet GetBookDramatList(string groupName)
        {
            return idg.GetBookDramatList(groupName);
        }

        /// <summary>
        /// 获取故事朗读资源
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public DataSet GetStoryReadList(string groupName)
        {
            return idg.GetStoryReadList(groupName);
        }

        /// <summary>
        /// 插入学生成绩
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public int InsertUserContentsRecord(TB_InterestDubbingGame_UserContentsRecord idgUserContent)
        {
            return idg.InsertUserContentsRecord(idgUserContent);
        }


        /// <summary>
        /// 从redis获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Redis_InterestDubbingGame_UserInfo GetUserInfo(string userId)
        {
            Redis_InterestDubbingGame_UserInfo idgUserinfo = idg.GetUserInfo(userId);
            return idgUserinfo;
        }


        /// <summary>
        /// 获取比赛报告
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetGameScore(string classId, int pageNumber)
        {
            IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
            List<StudentCount> stuCount = new List<StudentCount>();
            var userClassList=classBLL.GetUserClassRelationByNum(classId, out stuCount);
            return idg.GetGameScore(classId, pageNumber, userClassList);
        }

        /// <summary>
        /// 获取用户课本剧数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetUserBookDramaInfo(int userId)
        {
            return idg.GetUserBookDramaInfo(userId);
        }

        /// <summary>
        /// 获取用户故事朗读数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetUserStoryReadInfo(int userId)
        {
            return idg.GetUserStoryReadInfo(userId);
        }

    }
}
