using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.API.Model
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Pay_Model”的 XML 注释
    public class Pay_Model
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Pay_Model”的 XML 注释
    {

    }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PayWay”的 XML 注释
    public class PayWay
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PayWay”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PayWay.PayWayID”的 XML 注释
        public int PayWayID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PayWay.PayWayID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PayWay.PayWayName”的 XML 注释
        public string PayWayName { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PayWay.PayWayName”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PayWay.Description”的 XML 注释
        public string Description { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PayWay.Description”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostPayList”的 XML 注释
    public class PostPayList
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostPayList”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostPayList.AppID”的 XML 注释
        public string AppID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostPayList.AppID”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostComboNew”的 XML 注释
    public class PostComboNew
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostComboNew”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostComboNew.AppID”的 XML 注释
        public string AppID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostComboNew.AppID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostComboNew.ModuleID”的 XML 注释
        public int ModuleID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostComboNew.ModuleID”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID”的 XML 注释
    public class PostOrderID
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.PayWay”的 XML 注释
        public int? PayWay { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.PayWay”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.UserPhone”的 XML 注释
        public string UserPhone { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.UserPhone”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.GoodID”的 XML 注释
        public int? GoodID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.GoodID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.UserID”的 XML 注释
        public long? UserID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.UserID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.TotalPrice”的 XML 注释
        public double? TotalPrice { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.TotalPrice”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.AppID”的 XML 注释
        public int? AppID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.AppID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.channel”的 XML 注释
        public int? channel { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.channel”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.packageName”的 XML 注释
        public string packageName { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.packageName”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.PayMoney”的 XML 注释
        public double? PayMoney { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.PayMoney”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.OriginalPrice”的 XML 注释
        public double? OriginalPrice { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.OriginalPrice”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.Quantity”的 XML 注释
        public int? Quantity { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.Quantity”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostOrderID.Remark”的 XML 注释
        public string Remark { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostOrderID.Remark”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID”的 XML 注释
    public class AppleOrderID
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.UserPhone”的 XML 注释
        public string UserPhone { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.UserPhone”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.GoodID”的 XML 注释
        public int? GoodID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.GoodID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.UserID”的 XML 注释
        public int UserID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.UserID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.TotalPrice”的 XML 注释
        public double? TotalPrice { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.TotalPrice”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.AppID”的 XML 注释
        public int? AppID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.AppID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.channel”的 XML 注释
        public int? channel { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.channel”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.PayMoney”的 XML 注释
        public double? PayMoney { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.PayMoney”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.OriginalPrice”的 XML 注释
        public double? OriginalPrice { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.OriginalPrice”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.Quantity”的 XML 注释
        public int? Quantity { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.Quantity”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.Remark”的 XML 注释
        public string Remark { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.Remark”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.AppleTicket”的 XML 注释
        public string AppleTicket { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.AppleTicket”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.BookID”的 XML 注释
        public int BookID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.BookID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AppleOrderID.ModuleID”的 XML 注释
        public int ModuleID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AppleOrderID.ModuleID”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo”的 XML 注释
    public class PostPaySucessInfo
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo”的 XML 注释
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo.UserID”的 XML 注释
        public int UserID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo.UserID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo.BookID”的 XML 注释
        public int BookID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo.BookID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo.ModuleID”的 XML 注释
        public int ModuleID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo.ModuleID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo.packageName”的 XML 注释
        public string packageName { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo.packageName”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo.channel”的 XML 注释
        public int channel { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PostPaySucessInfo.channel”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApplePayInfo”的 XML 注释
    public class ApplePayInfo
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApplePayInfo”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.AppleOrderID”的 XML 注释
        public string AppleOrderID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.AppleOrderID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.AppleTicket”的 XML 注释
        public string AppleTicket { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.AppleTicket”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.CourseID”的 XML 注释
        public string CourseID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.CourseID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.FeeComboID”的 XML 注释
        public Guid FeeComboID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.FeeComboID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.UserID”的 XML 注释
        public string UserID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.UserID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.TotalMoney”的 XML 注释
        public string TotalMoney { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.TotalMoney”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.IsYX”的 XML 注释
        public string IsYX { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.IsYX”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.AppID”的 XML 注释
        public string AppID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ApplePayInfo.AppID”的 XML 注释
    }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Stateclass”的 XML 注释
    public class Stateclass
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Stateclass”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Stateclass.Success”的 XML 注释
        public bool Success { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Stateclass.Success”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Stateclass.isSanBox”的 XML 注释
        public string isSanBox { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Stateclass.isSanBox”的 XML 注释
    }

}