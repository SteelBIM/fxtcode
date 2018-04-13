using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class TicketDAL
    {
        readonly BaseManagement _bm = new BaseManagement();
        /// <summary>
        /// 根据书籍ID获取优惠券信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetTicketInfoByBookId(string courseId)
        {
            IList<TB_Ticket> ticket = _bm.Search<TB_Ticket>("CourseID='" + courseId + "'");
            TicketInfoList ticketinfo = new TicketInfoList();
            if (ticket != null)
            {
                foreach (var item in ticket)
                {
                    ticketinfo.Status = item.Status;
                    ticketinfo.ModularID = item.ModularID;
                    ticketinfo.StartDate = item.StartDate;
                    ticketinfo.Type = item.Type;
                    ticketinfo.CreateTime = item.CreateTime;
                    ticketinfo.ID = item.ID;
                    ticketinfo.TicketName = item.TicketName;
                    ticketinfo.CourseID = item.CourseID;
                    ticketinfo.Price = item.Price;
                    ticketinfo.EndDate = item.EndDate;
                    ticketinfo.ImgUrl = item.ImgUrl;
                    ticketinfo.sysTime = DateTime.Now;
                }
                return JsonHelper.GetResult(ticketinfo);
            }
            else
            {
                ticketinfo.Type = 1;
                return JsonHelper.GetResult(ticketinfo);
            }
        }

        /// <summary>
        /// 根据用户ID获取用户优惠券信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetUserTicketInfoByUserId(string userId, string courseId)
        {
            string where = " 1=1 ";
            if (!string.IsNullOrEmpty(courseId))
            {
                where += " AND CourseID='" + courseId + "' ";
            }
            if (!string.IsNullOrEmpty(userId))
            {
                where += " AND UserID='" + userId + "' ";
            }
            string sql = string.Format(@"SELECT  a.ID ,
                                                a.UserID ,
                                                a.OrderID ,
                                                a.TicketID ,
                                                a.Status ,
                                                a.CreateTime ,
                                                b.TicketName ,
                                                b.CourseID ,
                                                b.ModularID ,
                                                b.Price ,
                                                b.StartDate ,
                                                b.EndDate ,
                                                b.Type ,
                                                b.ImgUrl,
                                                b.Status as TicketStatus
                                        FROM    dbo.TB_UserTicket a
                                                LEFT JOIN dbo.TB_Ticket b ON a.TicketID = b.ID
                                        WHERE   {0} ORDER BY a.Status", where);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            List<UserTicketInfo> uti = JsonHelper.DataSetToIList<UserTicketInfo>(ds, 0);
            UserTicketList tick = new UserTicketList();
            List<UserTicketList> tickList = new List<UserTicketList>();
            if (uti != null)
            {
                foreach (var item in uti)
                {
                    tick.ID = item.ID;
                    tick.UserID = item.UserID;
                    tick.OrderID = item.OrderID;
                    tick.TicketID = item.TicketID;
                    tick.Status = item.Status;
                    tick.TicketName = item.TicketName;
                    tick.CourseID = item.CourseID;
                    tick.ModularID = item.ModularID;
                    tick.Price = item.Price;
                    tick.StartDate = item.StartDate;
                    tick.EndDate = item.EndDate;
                    tick.Type = item.Type;
                    tick.ImgUrl = item.ImgUrl;
                    tick.CreateTime = item.CreateTime;
                    tick.sysTime = DateTime.Now;
                    tick.TickStatus = item.TickStatus;
                    tickList.Add(tick);
                }
                return JsonHelper.GetResult(tickList);
            }
            else
            {
                return JsonHelper.GetResult("");
            }
        }

        /// <summary>
        /// 添加用户优惠券信息
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public HttpResponseMessage InsertUserTicketInfo(TB_UserTicket ticket)
        {
            if (_bm.Insert<TB_UserTicket>(ticket))
            {
                return JsonHelper.GetResult("新增成功");
            }
            else
            {
                return JsonHelper.GetErrorResult("新增失败");
            }
        }


        /// <summary>
        /// 根据课程id获取优惠券信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <returns></returns>
        public HttpResponseMessage GetTicketByCourseId(string courseId)
        {
            IList<TB_CurriculumManage> list = _bm.Search<TB_CurriculumManage>("BookID=" + courseId);
            if (list != null)
            {
                IList<TB_APPManagement> am = _bm.Search<TB_APPManagement>("VersionID=" + list[0].EditionID);
                if (am != null)
                {
                    //State:(1:启用，0:禁用)
                    IList<TB_FeeCombo> fc = _bm.Search<TB_FeeCombo>("AppID='" + am[0].ID + "' AND ComboType=1 AND State=1");
                    if (fc != null)
                    {
                        return JsonHelper.GetResult(new TB_Ticket
                        {
                            Type = 0
                        }); 
                    }
                    else
                    {
                        return JsonHelper.GetResult(new TB_Ticket
                        {
                            Type = 1
                        });
                    }
                }
                else
                {
                    return JsonHelper.GetResult(new TB_Ticket
                    {
                        Type = 1
                    });
                }
            }
            else
            {
                return JsonHelper.GetResult(new TB_Ticket
                {
                    Type = 1
                });
            }

            //_bm.Search<TB_FeeCombo>()
            //IList<TB_Ticket> tt = _bm.Search<TB_Ticket>("CourseID='" + courseId + "'");
            //if (tt != null)
            //{
            //    //foreach (var item in tt)
            //    //{
            //    //    tick.ID = item.ID;
            //    //    tick.TicketName = item.TicketName;
            //    //    tick.CourseID = item.CourseID;
            //    //    tick.Price = item.Price;
            //    //    tick.StartDate = item.StartDate;
            //    //    tick.EndDate = item.EndDate;
            //    //    tick.Type = item.Type;
            //    //    tick.ImgUrl = item.ImgUrl;
            //    //    tick.CreateTime = item.CreateTime;
            //    //}
            //    return JsonHelper.GetResult(new TB_Ticket
            //    {
            //        ID = tt[0].ID,
            //        TicketName = tt[0].TicketName,
            //        CourseID = tt[0].CourseID,
            //        Price = tt[0].Price,
            //        StartDate = tt[0].StartDate,
            //        EndDate = tt[0].EndDate,
            //        Type = tt[0].Type,
            //        ImgUrl = tt[0].ImgUrl,
            //        CreateTime = tt[0].CreateTime,
            //        Status = tt[0].Status
            //    });
            //}
            //else
            //{
            //    return JsonHelper.GetResult(new TB_Ticket
            //    {
            //        Type = 0
            //    });
            //}
        }

        /// <summary>
        /// 根据UserID,totalScore获取用户是否拥有优惠券
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="totalScore">用户成绩</param>
        /// <param name="courseId">书籍ID</param>
        /// <returns></returns>
        public HttpResponseMessage IsUserTicketByUserIdAndTotalscore(string userId, string totalScore, string courseId, string appid)
        {
            if (Convert.ToDouble(totalScore) >= 85)
            {
                IList<TB_UserTicket> tuti = _bm.Search<TB_UserTicket>("UserID='" + userId + "'");
                if (tuti != null)
                {
                    if (tuti[0].Status != 1 || tuti[0].Status != 2)
                    {
                        var utinfo = UserTicketInfoList(userId, 3);
                        return JsonHelper.GetResult(utinfo);
                    }
                }
                else
                {
                    IList<TB_Ticket> tti = _bm.Search<TB_Ticket>("CourseID='" + courseId + "' AND Type=1 AND Status=0 AND EndDate >=GETDATE() ");
                    TB_UserTicket tut = new TB_UserTicket();
                    if (tti != null)
                    {
                        tut.AppID = appid;
                        tut.ID = Guid.NewGuid();
                        tut.UserID = Convert.ToInt32(userId);
                        tut.TicketID = tti[0].ID;
                        tut.Status = 0;
                        tut.CreateTime = DateTime.Now;
                        if (_bm.Insert<TB_UserTicket>(tut))
                        {
                            return JsonHelper.GetResult(new UserTicketList
                            {
                                ID = tut.ID,
                                AppID = appid,
                                UserID = Convert.ToInt32(userId),
                                TicketID = tti[0].ID,
                                TicketName = tti[0].TicketName,
                                CourseID = tti[0].CourseID,
                                ModularID = tti[0].ModularID ?? 0,
                                Price = tti[0].Price ?? 0,
                                StartDate = tti[0].StartDate,
                                EndDate = tti[0].EndDate,
                                Type = tti[0].Type ?? 0,
                                ImgUrl = tti[0].ImgUrl,
                                CreateTime = tti[0].CreateTime,
                                TickStatus = 2,
                                sysTime = DateTime.Now,
                                IsCoupon = tti[0].Status
                            });
                        }
                    }
                }
            }
            else
            {
                IList<TB_UserTicket> tuti = _bm.Search<TB_UserTicket>("UserID='" + userId + "'");
                if (tuti != null)
                {
                    if (tuti[0].Status != 1 || tuti[0].Status != 2)
                    {
                        var utinfo = UserTicketInfoList(userId, 1);
                        return JsonHelper.GetResult(utinfo);
                    }
                }
                else
                {
                    IList<TB_Ticket> tti = _bm.Search<TB_Ticket>("CourseID='" + courseId + "' AND Type=1 ");
                    if (tti != null)
                    {
                        return JsonHelper.GetResult(new UserTicketList
                        {
                            AppID = appid,
                            IsCoupon = tti[0].Status,
                            StartDate = tti[0].StartDate,
                            EndDate = tti[0].EndDate,
                            TicketName = tti[0].TicketName,
                            Type = tti[0].Type,
                            Price = 10,
                            TickStatus = 0,
                            sysTime = DateTime.Now
                        });
                    }
                }
            }

            UserTicketList utinfos = new UserTicketList
            {
                AppID = appid,
                TickStatus = 4,
                sysTime = DateTime.Now
            };
            return JsonHelper.GetResult(utinfos);
        }

        /// <summary>
        /// 获取用户优惠卷信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="TickStatus"></param>
        /// <returns></returns>
        private static UserTicketList UserTicketInfoList(string userId, int TickStatus)
        {
            string sql = string.Format(@"SELECT  a.ID ,a.UserID ,a.OrderID,a.AppID ,a.TicketID ,a.Status ,a.CreateTime ,b.TicketName ,b.CourseID ,b.ModularID ,b.Price ,b.StartDate ,b.EndDate ,b.Type ,b.ImgUrl,b.Status as IsCoupon 
                                                    FROM    dbo.TB_UserTicket a LEFT JOIN dbo.TB_Ticket b ON a.TicketID = b.ID WHERE   a.UserID = '{0}' ORDER BY Status", userId);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            List<UserTicketInfo> uti = JsonHelper.DataSetToIList<UserTicketInfo>(ds, 0);
            UserTicketList utinfo = new UserTicketList();
            if (uti.Count > 0)
            {
                utinfo.ID = uti[0].ID;
                utinfo.UserID = uti[0].UserID;
                utinfo.OrderID = uti[0].OrderID;
                utinfo.TicketID = uti[0].TicketID;
                utinfo.Status = uti[0].Status;
                utinfo.TicketName = uti[0].TicketName;
                utinfo.CourseID = uti[0].CourseID;
                utinfo.ModularID = uti[0].ModularID;
                utinfo.Price = uti[0].Price;
                utinfo.StartDate = uti[0].StartDate;
                utinfo.EndDate = uti[0].EndDate;
                utinfo.Type = uti[0].Type;
                utinfo.ImgUrl = uti[0].ImgUrl;
                utinfo.CreateTime = uti[0].CreateTime;
                utinfo.TickStatus = TickStatus;
                utinfo.sysTime = DateTime.Now;
                utinfo.IsCoupon = uti[0].IsCoupon;
                utinfo.AppID = uti[0].AppID;
            }
            else
            {
                utinfo.Type = 1;
                utinfo.Status = 1;
                utinfo.sysTime = DateTime.Now;
            }

            return utinfo;
        }

        /// <summary>
        /// 根据UserID获取用户优惠券
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetUserTicketByUserId(string userId, string appid)
        {
            if (appid != "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385")
            {
                var utinfo = UserTicketInfoList(userId, 3);
                return JsonHelper.GetResult(utinfo);
            }
            else
            {
                UserTicketList utinfo = new UserTicketList
                {
                    TickStatus = 4,
                    sysTime = DateTime.Now
                };
                return JsonHelper.GetResult(utinfo);
            }
        }


        /// <summary>
        /// 根据UserID获取用户所有状态优惠券使用情况
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetUserTicketListByUserId(string userId, string appid)
        {
            List<UserTicketList> tickList = new List<UserTicketList>();

            if (appid != "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385")
            {
                string sql = string.Format(@"SELECT  a.ID ,
                                                a.UserID ,
                                                a.OrderID ,
                                                a.TicketID ,
                                                a.Status ,
                                                a.CreateTime ,
                                                a.AppID,
                                                b.TicketName ,
                                                b.CourseID ,
                                                b.ModularID ,
                                                b.Price ,
                                                b.StartDate ,
                                                b.EndDate ,
                                                b.Type ,
                                                b.ImgUrl,
                                                b.Status as IsCoupon 
                                        FROM    dbo.TB_UserTicket a
                                                LEFT JOIN dbo.TB_Ticket b ON a.TicketID = b.ID
                                        WHERE   a.UserID = '{0}'", userId);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                List<UserTicketInfo> uti = JsonHelper.DataSetToIList<UserTicketInfo>(ds, 0);
                UserTicketList tick = new UserTicketList();
                if (uti != null)
                {
                    foreach (var item in uti)
                    {
                        tick.AppID = item.AppID;
                        tick.ID = item.ID;
                        tick.UserID = item.UserID;
                        tick.OrderID = item.OrderID;
                        tick.TicketID = item.TicketID;
                        tick.Status = item.Status;
                        tick.TicketName = item.TicketName;
                        tick.CourseID = item.CourseID;
                        tick.ModularID = item.ModularID;
                        tick.Price = item.Price;
                        tick.StartDate = item.StartDate;
                        tick.EndDate = item.EndDate;
                        tick.Type = item.Type;
                        tick.ImgUrl = item.ImgUrl;
                        tick.CreateTime = item.CreateTime;
                        tick.sysTime = DateTime.Now;
                        tick.IsCoupon = item.IsCoupon;
                        tickList.Add(tick);
                    }
                    return JsonHelper.GetResult(tickList);
                }
            }

            UserTicketList utinfo = new UserTicketList
            {
                TickStatus = 4,
                sysTime = DateTime.Now
            };
            tickList.Add(utinfo);
            return JsonHelper.GetResult(tickList);
        }



        /// <summary>
        /// 添加TB_Ticket信息
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public bool AddTicketInfo(List<string> list)
        {
            return SqlHelper.ExecuteNonQueryTransaction(list);
        }

        public class UserTicketInfo
        {
            public Guid ID { get; set; }
            public int UserID { get; set; }
            public string OrderID { get; set; }
            public int TicketID { get; set; }
            public int Status { get; set; }
            public DateTime CreateTime { get; set; }
            public string TicketName { get; set; }
            public int CourseID { get; set; }
            public int ModularID { get; set; }
            public decimal Price { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int Type { get; set; }
            public string ImgUrl { get; set; }
            public int TickStatus { get; set; }
            public DateTime SysTime { get; set; }
            public string AppID { get; set; }
            public int IsCoupon { get; set; }
        }

        public class TicketInfoList
        {
            public int? ModularID { get; set; }
            public DateTime? StartDate { get; set; }
            public int? Type { get; set; }
            public DateTime? CreateTime { get; set; }
            public int ID { get; set; }
            public string TicketName { get; set; }
            public int CourseID { get; set; }
            public decimal? Price { get; set; }
            public DateTime? EndDate { get; set; }
            public string ImgUrl { get; set; }
            public DateTime sysTime { get; set; }
            public int Status { get; set; }
        }


        public class UserTicketList
        {
            public string AppID { get; set; }
            public Guid ID { get; set; }
            public int UserID { get; set; }
            public string OrderID { get; set; }
            public int TicketID { get; set; }
            public int Status { get; set; }
            public DateTime CreateTime { get; set; }
            public string TicketName { get; set; }
            public int CourseID { get; set; }
            public int ModularID { get; set; }
            public decimal Price { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int? Type { get; set; }
            public string ImgUrl { get; set; }
            public DateTime sysTime { get; set; }
            public int IsCoupon { get; set; }
            public int TickStatus { get; set; }//0:分数低于85分，且没有优惠卷；1：分数低于85分，有优惠卷；2：分数高于85分，且没有优惠卷；3：分数低于85分，有优惠卷
        }

    }
}
