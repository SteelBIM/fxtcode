using System.ComponentModel;

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
    /// 业务阶段
    /// </summary>
    public enum BusinessStageType
    {
        /// <summary>
        /// 预评==2018005
        /// </summary>
        预评 = 2018005,
        /// <summary>
        /// 报告==2018006
        /// </summary>
        报告 = 2018006
    }
    /// <summary>
    /// 收费形式枚举
    /// </summary>
    public enum ChargeCountType
    {
        /// <summary>
        /// 累进制==1
        /// </summary>
        累进制 = 1,
        /// <summary>
        /// 非累进制==2
        /// </summary>
        非累进制 = 2
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
    /// 审批条件（对应表SYS_PrivilegeApprove字段ApproveType||Dat_EntrustCharge字段PrivilegeType）
    /// 修改人：曾智磊，2014-11-13，字段改为优惠方式用
    /// </summary>
    public enum ApproveSettingType
    {
        /// <summary>
        /// 无优惠方式==-1
        /// </summary>
        [Description("无优惠方式")]
        无优惠方式 = -1,
        /// <summary>
        /// SYS_PrivilegeApprove字段ApproveType 任一==0
        /// </summary>
        [Description("任一")]
        任一 = 0,
        /// <summary>
        /// 按最低收费==1
        /// </summary>
        [Description("按最低收费")]
        按最低收费 = 1,
        /// <summary>
        /// 按折扣==2
        /// </summary>
        [Description("按折扣")]
        按折扣 = 2,
        /// <summary>
        /// 按优惠金额==3
        /// </summary>
        [Description("按优惠金额")]
        按优惠金额 = 3,
        /// <summary>
        /// 免费==4
        /// </summary>
        [Description("免费")]
        免费 = 4,
        /// <summary>
        /// 免费==5
        /// </summary>
        [Description("一口价")]
        一口价 = 5,
        [Description("")]
        GetName = -2
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
    /// 支出类型
    /// </summary>
    public enum ChargeOutType
    {
        /// <summary>
        /// 其它
        /// </summary>
        其它 = 1,
        /// <summary>
        /// 返利
        /// </summary>
        返利 = 2,
        /// <summary>
        /// 提成
        /// </summary>
        提成 = 3
        
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

    /// <summary>
    /// 票据类型
    /// </summary>
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
        //预收费 = 10016001,
        //待收费 = 10016002,
        未收费 = 10016001,
        未收完 = 10016002,
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
