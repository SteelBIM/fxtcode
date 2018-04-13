using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class TicketBLL
    {
        TicketDAL nlr = new TicketDAL();

        /// <summary>
        /// 添加TB_Ticket信息
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public bool AddTicketInfo(List<string> list)
        {
            return nlr.AddTicketInfo(list);
        }

        /// <summary>
        ///根据书籍ID获取优惠券信息
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetTicketInfoByBookId(string courseId)
        {
            return nlr.GetTicketInfoByBookId(courseId);
        }

        /// <summary>
        ///根据用户ID获取用户优惠券信息
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetUserTicketInfoByUserId(string userId, string courseId)
        {
            return nlr.GetUserTicketInfoByUserId(userId, courseId);
        }

        /// <summary>
        ///根据用户ID获取用户优惠券信息
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage InsertUserTicketInfo(TB_UserTicket ticket)
        {
            return nlr.InsertUserTicketInfo(ticket);
        }

        /// <summary>
        /// 根据老师ID查询班级信息
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetTicketByCourseId(string courseId)
        {
            return nlr.GetTicketByCourseId(courseId);
        }

        /// <summary>
        ///根据UserID获取用户优惠券
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetUserTicketByUserId(string userId, string appId)
        {
            return nlr.GetUserTicketByUserId(userId, appId);
        }

        /// <summary>
        /// 根据UserID,totalScore获取用户是否拥有优惠券
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage IsUserTicketByUserIdAndTotalscore(string userId, string totalScore, string courseId, string appid)
        {
            return nlr.IsUserTicketByUserIdAndTotalscore(userId, totalScore, courseId, appid);
        }

        /// <summary>
        ///根据UserID获取用户所有状态优惠券使用情况
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetUserTicketListByUserId(string userId, string appid)
        {
            return nlr.GetUserTicketListByUserId(userId, appid);
        }
    }
}
