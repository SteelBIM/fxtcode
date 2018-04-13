using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.DAL
{
    public class PersonalCenterDAL
    {
        readonly BaseManagement _bm = new BaseManagement();
        private string BaseDB = System.Configuration.ConfigurationManager.ConnectionStrings["KingsunBaseDBConnectionStr"].ConnectionString;

        /// <summary>
        /// 获取个人中心订单列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetOrderListByUserId(string UserId)
        {
            string sql = string.Format(@"SELECT DISTINCT
                                                a.OrderID ,
                                                a.TotalMoney ,
                                                a.CreateDate ,
                                                a.CourseID ,
                                                a.ModuleID,
                                                b.[Month] ,
                                                c.TeachingNaterialName ,
                                                d.StartDate ,
                                                d.EndDate ,
                                                c.EditionID ,
                                                c.TextbookVersion
                                        FROM    TB_Order a
                                                LEFT JOIN dbo.TB_FeeCombo b ON a.FeeComboID = b.ID
                                                LEFT JOIN dbo.TB_CurriculumManage c ON a.CourseID = c.BookID
                                                LEFT JOIN TB_UserMember d ON d.TbOrderID = a.ID
                                        WHERE   a.State = 0001
                                                AND a.UserID = '{0}'
                                        ORDER BY a.CreateDate desc", UserId);
            DataSet ds = SqlHelper.ExecuteDataset(BaseDB, CommandType.Text, sql);
            List<orderinfo> csn = JsonHelper.DataSetToIList<orderinfo>(ds, 0);
            List<orderinfo> orderinfo = new List<orderinfo>();
            string bid = "";
            foreach (var item in csn)
            {
                string ModuleID;
                if (string.IsNullOrEmpty(item.ModuleID))
                {
                    ModuleID = "1";
                }
                else
                {
                    ModuleID = item.ModuleID;
                }

                if (bid == item.CourseID)
                {
                    orderinfo oi = new orderinfo();

                    oi.CourseID = item.CourseID;
                    oi.OrderID = item.OrderID;
                    oi.TotalMoney = item.TotalMoney;
                    oi.CreateDate = item.CreateDate;
                    oi.Month = item.Month;
                    oi.TeachingNaterialName = item.TeachingNaterialName;
                    oi.StartDate = item.StartDate.AddMonths(item.Month);
                    oi.EndDate = item.EndDate.AddMonths(item.Month);
                    oi.EditionID = item.EditionID;
                    oi.TextbookVersion = item.TextbookVersion;
                    oi.ModuleID = ModuleID;
                    orderinfo.Add(oi);
                }
                else
                {
                    bid = item.CourseID;
                    orderinfo oi = new orderinfo();
                    oi.CourseID = item.CourseID;
                    oi.OrderID = item.OrderID;
                    oi.TotalMoney = item.TotalMoney;
                    oi.CreateDate = item.CreateDate;
                    oi.Month = item.Month;
                    oi.TeachingNaterialName = item.TeachingNaterialName;
                    oi.StartDate = item.StartDate;
                    oi.EndDate = item.EndDate;
                    oi.EditionID = item.EditionID;
                    oi.TextbookVersion = item.TextbookVersion;
                    oi.ModuleID = ModuleID;
                    orderinfo.Add(oi);
                }
            }
            return JsonHelper.GetResult(orderinfo, "操作成功");
        }

        public class orderinfo
        {
            public string OrderID { get; set; }
            public decimal TotalMoney { get; set; }
            public DateTime CreateDate { get; set; }
            public string CourseID { get; set; }
            public int Month { get; set; }
            public string TeachingNaterialName { get; set; }

            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public int EditionID { get; set; }
            public string TextbookVersion { get; set; }

            public string ModuleID { get; set; }
        }
    }
}
