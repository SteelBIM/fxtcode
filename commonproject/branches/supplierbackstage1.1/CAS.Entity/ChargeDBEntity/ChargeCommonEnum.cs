using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.DBEntity
{
    /// <summary>
    /// 收费标准类型枚举
    /// </summary>
    public enum ChargeStandard
    {
        /// <summary>
        /// 国家标准==1
        /// </summary>
        国家标准 = 1,
        /// <summary>
        /// 自定义标准==2
        /// </summary>
        自定义标准 = 2
    }
    /// <summary>
    /// 收费类型
    /// </summary>
    public enum ChargeType
    {
        /// <summary>
        /// 房地产
        /// </summary>
        房地产 = 10010001,
        /// <summary>
        /// 土地
        /// </summary>
        土地 = 10010002,
        /// <summary>
        /// 资产
        /// </summary>
        资产 = 10010003,
    }

    /// <summary>
    /// 公司收费类型
    /// </summary>
    public enum CompanyChargeType
    {

        /// <summary>
        ///按费率
        /// </summary>
        按费率 = 1,
        /// <summary>
        /// 按金额
        /// </summary>
        按金额 = 2

    }
    /// <summary>
    /// 审批条件
    /// </summary>
    public enum ApproveSettingType
    {
        /// <summary>
        /// 不需要审批
        /// </summary>
        No = 1,
        /// <summary>
        /// 全部需要审批
        /// </summary>
        All = 2,
        /// <summary>
        /// 少于最低最低收费才审批
        /// </summary>
        LessThanMin = 3,
        /// <summary>
        /// 自定义审批条件
        /// </summary>
        Custom = 4
    }

    /// <summary>
    /// 收入支出枚举
    /// </summary>
    public enum ChargeInOrOut
    {
        /// <summary>
        /// 收入
        /// </summary>
        In = 1,
        /// <summary>
        /// 支出
        /// </summary>
        Out = 2

    }

    /// <summary>
    /// 收费方法
    /// </summary>
    public enum ChargeWay
    {
        /// <summary>
        ///现金
        /// </summary>
        现金 = 1,
        /// <summary>
        /// 支票
        /// </summary>
        支票 = 2,
        /// <summary>
        /// 转账
        /// </summary>
        转账 = 3,
        /// <summary>
        /// 其他
        /// </summary>
        其他 = 4
    }
    public enum TicketTitle
    {
        /// <summary>
        /// 对公
        /// </summary>
        ToPublic = 20010401,
        /// <summary>
        /// 个人
        /// </summary>
        ToPersonal = 20010402
    }

    public enum TicketType
    {
        /// <summary>
        /// 收据
        /// </summary>
        Receipt = 1,
        /// <summary>
        /// 专用发票
        /// </summary>
        SpecialInvoice = 2,
        /// <summary>
        /// 普通发票
        /// </summary>
        OrdinaryInvoice = 3,
        /// <summary>
        /// 增值税发票
        /// </summary>
        ValueAddedTaxInvoice = 4
    }

    /// <summary>
    /// 收费状态
    /// </summary>
    public enum ChargeState
    {
        预收费 = 10016001,
        待收费 = 10016002,
        待结单 = 10016003,
        已结单 = 10016004,
        待退费 = 10016005,
        已退费 = 10016006,
        已结算 = 10016007,
        待结算 = 10016008
    }

    public enum ChargeViewType
    {
        看自己的=1,
        看部门的=2,
        看全部的=3
    }

}
