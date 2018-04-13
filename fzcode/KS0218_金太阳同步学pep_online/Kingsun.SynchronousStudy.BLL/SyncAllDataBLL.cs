using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;
using System.Data;
using Kingsun.DB;
using Kingsun.IBS.Model.IBSLearnReport;
using NPOI.SS.Formula.Functions;

namespace Kingsun.SynchronousStudy.BLL
{
    public class SyncAllDataBLL
    {
        private RedisListHelper listRedis = new RedisListHelper();



        public void SyncOrderInfo2BaseDB()
        {
            var listCount = listRedis.Count("Order2BaseDBQueue");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            int Changetype = 0;
            int DataType = 0;
            for (int i = 0; i < Count; i++)
            {
                var model = listRedis.RemoveStartFromList("Order2BaseDBQueue");
                try
                {
                    if (!string.IsNullOrEmpty(model))
                    {
                        TB_Order order = JsonHelper.DecodeJson<TB_Order>(model);
                        order.CreateDate=DateTime.Now;
                        BaseManagement bm = new BaseManagement();
                        var dbOrder =bm.Select<TB_Order>(order.ID);
                        if (dbOrder == null)
                        {
                            bm.Insert(order);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "SyncOrderInfo2BaseDB异常！Data="+model.ToJson());
                }
            }
        }



        public void SyncOrderInfo()
        {
            try
            {
                string connect_com = System.Configuration.ConfigurationManager.ConnectionStrings["kingsunconstr"].ConnectionString;
                string sql = @"
                    ----深圳---
                    insert into TB_Order(
	                      ID, OrderID, TotalMoney, CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount
                    )
                    select   MIN(CAST(ID AS VARCHAR(50))) ID,OrderID, TotalMoney, MIN(CreateDate) CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount
                    from ITSV_SZ.FZ_SynchronousStudy.dbo.TB_Order
                    where CreateDate>=Convert(varchar(100),dateadd(day, -1, getdate()),23) 
	                    and OrderID not in (select OrderID from TB_Order where CreateDate>=Convert(varchar(100),dateadd(day, -1, getdate()),23) )
                    group by OrderID, TotalMoney, CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount

                    ----北京---
                    insert into TB_Order(
	                      ID, OrderID, TotalMoney, CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount
                    )
                    select   MIN(CAST(ID AS VARCHAR(50))) ID,OrderID, TotalMoney, MIN(CreateDate) CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount
                    from ITSV_BJ.FZ_SynchronousStudy.dbo.TB_Order
                    where CreateDate>=Convert(varchar(100),dateadd(day, -1, getdate()),23) 
	                    and OrderID not in (select OrderID from TB_Order where CreateDate>=Convert(varchar(100),dateadd(day, -1, getdate()),23) )
                    group by OrderID, TotalMoney, CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount

                    ----其它---
                    insert into TB_Order(
	                      ID, OrderID, TotalMoney, CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount
                    )
                    select   MIN(CAST(ID AS VARCHAR(50))) ID,OrderID, TotalMoney, MIN(CreateDate) CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount
                    from ITSV_OT.FZ_SynchronousStudy.dbo.TB_Order
                    where CreateDate>=Convert(varchar(100),dateadd(day, -1, getdate()),23) 
	                    and OrderID not in (select OrderID from TB_Order where CreateDate>=Convert(varchar(100),dateadd(day, -1, getdate()),23) )
                    group by OrderID, TotalMoney, CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount

                    ----人教---
                    insert into TB_Order(
	                      ID, OrderID, TotalMoney, CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount
                    )
                    select   MIN(CAST(ID AS VARCHAR(50))) ID,OrderID, TotalMoney, MIN(CreateDate) CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount
                    from ITSV_RJ.FZ_SynchronousStudy.dbo.TB_Order
                    where CreateDate>=Convert(varchar(100),dateadd(day, -1, getdate()),23) 
	                    and OrderID not in (select OrderID from TB_Order where CreateDate>=Convert(varchar(100),dateadd(day, -1, getdate()),23) )
                    group by OrderID, TotalMoney, CreateDate, State, UserID, PayWay, FeeComboID, CourseID, IsDiscount";

                int count = SqlHelper.ExecuteNonQueryTimeOut(connect_com, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "同步订单出错");
            }
        }

        public void SyncUserInfo()
        {
            try
            {
                string connect_com = System.Configuration.ConfigurationManager.ConnectionStrings["kingsunconstr"].ConnectionString;
                string sql = @"
                    ----深圳---
                    insert into ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo(
	                     UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, CreateTime, IsUser, isLogState, IsEnableOss, AppId
                    )
                    select  UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, MIN(CreateTime) CreateTime, IsUser, isLogState, IsEnableOss, AppId 
                    from ITSV_SZ.FZ_SynchronousStudy.dbo.Tb_UserInfo
                    where CreateTime>=Convert(varchar(100),dateadd(day, -1, getdate()),23) 
                        and UserID not in (select UserID from Tb_UserInfo where CreateTime>Convert(varchar(100),dateadd(day, -1, getdate()),23))
                    group by UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, IsUser, isLogState, IsEnableOss, AppId

                    ----北京---
                    insert into ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo(
	                     UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, CreateTime, IsUser, isLogState, IsEnableOss, AppId
                    )
                    select  UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, MIN(CreateTime) CreateTime, IsUser, isLogState, IsEnableOss, AppId 
                    from ITSV_BJ.FZ_SynchronousStudy.dbo.Tb_UserInfo
                    where CreateTime>=Convert(varchar(100),dateadd(day, -1, getdate()),23)
                        and UserID not in (select UserID from Tb_UserInfo where CreateTime>Convert(varchar(100),dateadd(day, -1, getdate()),23))
                    group by UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, IsUser, isLogState, IsEnableOss, AppId

                    ----其它---
                    insert into ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo(
	                     UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, CreateTime, IsUser, isLogState, IsEnableOss, AppId
                    )
                    select  UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, MIN(CreateTime) CreateTime, IsUser, isLogState, IsEnableOss, AppId 
                    from ITSV_OT.FZ_SynchronousStudy.dbo.Tb_UserInfo
                    where CreateTime>=Convert(varchar(100),dateadd(day, -1, getdate()),23)
                        and UserID not in (select UserID from Tb_UserInfo where CreateTime>Convert(varchar(100),dateadd(day, -1, getdate()),23))
                    group by UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, IsUser, isLogState, IsEnableOss, AppId

                    ----人教---
                    insert into ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo(
	                     UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, CreateTime, IsUser, isLogState, IsEnableOss, AppId
                    )
                    select  UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, MIN(CreateTime) CreateTime, IsUser, isLogState, IsEnableOss, AppId 
                    from ITSV_RJ.FZ_SynchronousStudy.dbo.Tb_UserInfo
                    where CreateTime>=Convert(varchar(100),dateadd(day, -1, getdate()),23)
                        and UserID not in (select UserID from Tb_UserInfo where CreateTime>Convert(varchar(100),dateadd(day, -1, getdate()),23))
                    group by UserID, UserName, NickName, UserImage, UserRoles, TrueName, TelePhone, BookID, IsUser, isLogState, IsEnableOss, AppId";

                int count = SqlHelper.ExecuteNonQueryTimeOut(connect_com, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "同步用户出错");
            }
        }

        private void AddOrderInfo(string connect_com, DataSet dsorderinfo, string version)
        {
            if (dsorderinfo != null && dsorderinfo.Tables[0].Rows.Count > 0)
            {
                DataTable table = dsorderinfo.Tables[0];
                if (table != null && table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["OrderID"] != null && !string.IsNullOrEmpty(row["OrderID"].ToString()))
                        {
                            TB_Order orderinfo = new TB_Order()
                            {
                                ID = Guid.NewGuid(),
                                OrderID = ReturnString(row["OrderID"]),
                                TotalMoney = Convert.ToDecimal(row["TotalMoney"]),
                                CreateDate = ReturnDateTime(row["CreateDate"]),
                                State = ReturnString(row["State"]),
                                UserID = ReturnString(row["UserID"]),
                                PayWay = ReturnString(row["PayWay"]),
                                FeeComboID = new Guid(row["FeeComboID"].ToString()),
                                CourseID = ReturnString(row["CourseID"]),
                                IsDiscount = ReturnInt(row["IsDiscount"])
                            };
                            string sql = string.Format(@" 
                            if not exists(select * from TB_Order where OrderID ='{1}')
                            insert into TB_Order
                            (
	                            ID, OrderID, TotalMoney, CreateDate, State, 
                                UserID, PayWay, FeeComboID, CourseID, IsDiscount
                            )
                            values
                            (
	                            '{0}', '{1}', '{2}', '{3}', '{4}', 
                                '{5}', '{6}', '{7}','{8}', '{9}'
                            ) ", orderinfo.ID, orderinfo.OrderID, orderinfo.TotalMoney, orderinfo.CreateDate, orderinfo.State, orderinfo.UserID, orderinfo.PayWay, orderinfo.FeeComboID, orderinfo.CourseID, orderinfo.IsDiscount);
                            int count = SqlHelper.ExecuteNonQuery(connect_com, CommandType.Text, sql);
                        }
                    }
                }
            }

        }

        private static int UserOrderNumber(string _branchstr, string sql)
        {
            object obj = SqlHelper.ExecuteScalar(_branchstr, CommandType.Text, sql);
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                return (int)SqlHelper.ExecuteScalar(_branchstr, CommandType.Text, sql);
            return 0;
        }
        private static int ReturnInt(object Id)
        {
            if (Id != null && Id != DBNull.Value)
                return Convert.ToInt32(Id);
            return 0;
        }

        private static string ReturnString(object Name)
        {
            if (Name != null && Name != DBNull.Value)
                return Name.ToString();
            return "";
        }

        private static DateTime ReturnDateTime(object time)
        {
            if (time != null && time != DBNull.Value)
                return Convert.ToDateTime(time);
            return DateTime.Now;
        }
    }
}
