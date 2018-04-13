using Kingsun.IBS.BLL.FZUUMS_Relation2;
using Kingsun.IBS.BLL.RelationService;
using Kingsun.IBS.DAL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.IBS.Model.MOD;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Kingsun.IBS.Model.IBS;
using Kingsun.SynchronousStudy.Models;
using User = Kingsun.IBS.BLL.FZUUMS_UserService.User;

namespace Kingsun.IBS.BLL
{
    public class IBSData_OrderBLL
    {
        static RedisHashOtherHelper hashRedis = new RedisHashOtherHelper();
        static readonly BaseManagementOther _bmBaseDB = new BaseManagementOther();
        static RelationService.RelationService relationservice = new RelationService.RelationService();
        static FZUUMS_UserService.FZUUMS_UserServiceSoapClient userService = new FZUUMS_UserService.FZUUMS_UserServiceSoapClient();
        static MetadataService.ServiceSoapClient metadataService = new MetadataService.ServiceSoapClient();

        static IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        static IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        static IIBSData_SchClassRelationBLL schBLL = new IBSData_SchClassRelationBLL();
        static IIBSData_AreaSchRelationBLL areaBLL = new IBSData_AreaSchRelationBLL();
        static IBSOrderDAL orderDAL = new IBSOrderDAL();

        /// <summary>
        /// 查询时间段订单，并返回财富分账订单列表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public List<Tb_KSWFOrder> GetKSWFOrderList(DateTime starttime, DateTime endtime, ref int totalnum, ref int successnum)
        {
            List<Tb_KSWFOrder> KSWFOrderlist = new List<Tb_KSWFOrder>();

            var orderlist = orderDAL.SelectSearch(a => a.State == "0001" && a.CreateDate > starttime && a.CreateDate <= endtime);
            foreach (var order in orderlist)
            {
                if (string.IsNullOrEmpty(order.UserID))
                {
                    order.UserID = "0";
                }
                try
                {
                    var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(order.UserID));

                    if (user != null)
                    {
                        Tb_KSWFOrder on = new Tb_KSWFOrder();
                        if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                        {
                            user.ClassSchDetailList.ForEach(a =>
                            {
                                if (a.ClassID != null)
                                {
                                    on.GradeName = "";
                                    on.SchoolName = "";
                                    var classInfo = classBLL.GetClassUserRelationByClassId(a.ClassID);
                                    if (classInfo != null)
                                    {
                                        var tea = classInfo.ClassTchList.Where(x => x.SubjectID == 3).FirstOrDefault();
                                        if (tea != null)
                                        {
                                            on.TeacherUserID = Convert.ToInt32(tea.TchID);
                                            on.TeacherUserName = tea.TchName;
                                        }
                                        on.SchoolID = classInfo.SchID;
                                        on.GradeID = classInfo.GradeID;
                                        on.ClassID = new Guid(classInfo.ClassID);
                                        on.ClassName = classInfo.ClassName;
                                    }
                                }
                            });
                        }
                        int type = 4;
                        if (order.PayWay.Contains("微信"))
                        {
                            type = 0;
                        }
                        else if (order.PayWay.Contains("支付宝"))
                        {
                            type = 1;
                        }
                        else if (order.PayWay.Contains("苹果"))
                        {
                            type = 2;
                        }
                        on.OrderID = order.OrderID;
                        on.OrderDate = order.CreateDate;
                        on.ProductNO = "TBX_" + order.FeeComboID;
                        on.PayType = type;
                        on.Channel = 1;
                        on.UserClientIP = "";
                        on.PayAmount = order.TotalMoney ?? 0;
                        on.BuyUserID = user.iBS_UserInfo.UserID.ToString();
                        on.BuyUserPhone = user.iBS_UserInfo.TelePhone;
                        KSWFOrderlist.Add(on);
                    }
                    else
                    {
                        Tb_KSWFOrder on = new Tb_KSWFOrder();
                        int type = 4;
                        if (order.PayWay.Contains("微信"))
                        {
                            type = 0;
                        }
                        else if (order.PayWay.Contains("支付宝"))
                        {
                            type = 1;
                        }
                        else if (order.PayWay.Contains("苹果"))
                        {
                            type = 2;
                        }
                        on.OrderID = order.OrderID;
                        on.OrderDate = order.CreateDate;
                        on.ProductNO = "TBX_" + order.FeeComboID;
                        on.PayType = type;
                        on.Channel = 1;
                        on.UserClientIP = "";
                        on.PayAmount = order.TotalMoney ?? 0;
                        on.BuyUserID = order.UserID;
                        on.BuyUserPhone = "";
                        KSWFOrderlist.Add(on);
                    }
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "返回财富分账订单列表失败");
                }
            }
            totalnum = orderlist.Count();
            successnum = KSWFOrderlist.Count;

            return KSWFOrderlist;

        }

    }
}
