using System.ComponentModel;
/**创建人：曾智磊，2014-12-11，存储各公用code枚举
 * 修改人：曾智磊，20150107， 将预评状态和报告状态枚举从收费命名空间下移过来，给报告状态新增“已锁定”，新增枚举EnumBusinessType，EnumApprovalStatus
 * **/
namespace CAS.Entity
{    /// <summary>
    /// 预评状态
    /// </summary>
    public enum YpStateCode
    {
        /// <summary>
        /// 10007001
        /// </summary>
        撰写中 = 10007001,
        /// <summary>
        /// 10007002
        /// </summary>
        审批中 = 10007002,
        /// <summary>
        /// 10007003
        /// </summary>
        已完成 = 10007003,
        /// <summary>
        /// 10007004
        /// </summary>
        已转交or待确认 = 10007004,
        /// <summary>
        /// 10007005
        /// </summary>
        已转交 = 10007005,
        /// <summary>
        /// 10007006
        /// </summary>
        待撰写 = 10007006,
        /// <summary>
        /// 10007007
        /// </summary>
        已作废 = 10007007
    }


    /// <summary>
    /// 报告状态
    /// </summary>
    public enum ReportStateCode
    {
        /// <summary>
        /// 10009001
        /// </summary> 
        [Description("撰写中")]
        撰写中 = 10009001,
        /// <summary>
        /// 10009002
        /// </summary>
        [Description("审核中")]
        审核中 = 10009002,
        /// <summary>
        /// 10009003
        /// </summary>
        [Description("已完成")]
        已完成 = 10009003,
        /// <summary>
        /// 10009004
        /// </summary>
        [Description("已转交,待确认")]
        已转交or待确认 = 10009004,
        /// <summary>
        /// 10009005
        /// </summary>
        [Description("已转交")]
        已转交 = 10009005,
        /// <summary>
        /// 10009006
        /// </summary>
        [Description("待撰写")]
        待撰写 = 10009006,
        /// <summary>
        /// 10009007
        /// </summary>
        [Description("待分配")]
        待分配 = 10009007,
        /// <summary>
        /// 10009008
        /// </summary>
        [Description("已作废")]
        已作废 = 10009008,
        /// <summary>
        /// 10009009
        /// </summary>
        [Description("已锁定")]
        已锁定 = 10009009,
        /// <summary>
        /// 10009010
        /// </summary>
        [Description("已撤销")]
        已撤销=10009010,
        [Description("")]
        GetName=-1
    }
    /// <summary>
    /// 业务状态
    /// </summary>
    public enum EntrustStateCode
    {
        /// <summary>
        /// 10013001
        /// </summary>
        待提交 = 10013001,
        /// <summary>
        /// 10013002
        /// </summary>
        待分配 = 10013002,
        /// <summary>
        /// 10013003
        /// </summary>
        处理中 = 10013003,
        /// <summary>
        /// 10013004
        /// </summary>
        已完成 = 10013004,
        /// <summary>
        /// 10013005
        /// </summary>
        已撤销 = 10013005
    }
    /// <summary>
    /// 业务详细状态（环节）
    /// </summary>
    public enum EntrustDetailState
    {
        [Description("待分配业务")]
        待分配业务,
        撰写预评中,
        [Description("待撰写预评")]
        待撰写预评,
        [Description("预评审批中")]
        预评审批中,
        [Description("预评已完成")]
        预评已完成,
        //[Description("预评已撤销")]
        //预评已撤销,
        [Description("预评已打印")]
        预评已打印,
        [Description("预评已盖章")]
        预评已盖章,
        [Description("待分配报告")]
        待分配报告,
        撰写报告中,
        [Description("待撰写报告")]
        待撰写报告,
        [Description("报告审批中")]
        报告审批中,
        [Description("报告已完成")]
        报告已完成,
        //[Description("报告已撤销")]
        //报告已撤销,
        [Description("报告已打印")]
        报告已打印,
        [Description("报告已盖章")]
        报告已盖章,
        [Description("业务已撤销")]
        业务已撤销,
        //[Description("业务已完成")]
        //业务已完成
    }

    /// <summary>
    /// 委估对象类型
    /// </summary>
    public enum EnumObjectType
    {
        /// <summary>
        /// 1031001
        /// </summary>
        [Description("住宅")]
        住宅 = 1031001,
        /// <summary>
        /// 1031002
        /// </summary>
        [Description("办公")]
        办公 = 1031002,
        /// <summary>
        /// 1031003
        /// </summary>
        [Description("商业")]
        商业 = 1031003,
        /// <summary>
        /// 1031004
        /// </summary>
        [Description("工业")]
        工业 = 1031004,
        /// <summary>
        /// 1031005
        /// </summary>
        [Description("土地")]
        土地 = 1031005,
        /// <summary>
        /// 1031006
        /// </summary>
        [Description("资产")]
        资产 = 1031006,
        /// <summary>
        /// 1031007
        /// </summary>
        [Description("其他")]
        其他 = 1031007,
        [Description("")]
        GetName=-1
    }
    /// <summary>
    /// 业务节点对象
    /// </summary>
    public enum EnumDataType
    {
        /// <summary>
        /// 2018001
        /// </summary>
        委托 = 2018001,
        /// <summary>
        /// 2018002
        /// </summary>
        委估对象 = 2018002,
        /// <summary>
        /// 2018003
        /// </summary>
        询价 = 2018003,
        /// <summary>
        /// 2018004
        /// </summary>
        查勘 = 2018004,
        /// <summary>
        /// 2018005
        /// </summary>
        预评 = 2018005,
        /// <summary>
        /// 2018006
        /// </summary>
        报告 = 2018006,
        /// <summary>
        /// 2018010
        /// </summary>
        多套询价 = 2018010,
        /// <summary>
        /// 2018009
        /// </summary>
        普通收费 = 2018009,
        /// <summary>
        /// 2018008
        /// </summary>
        月结收费 = 2018008,
        /// <summary>
        /// 2018011
        /// </summary>
        案例数据 = 2018011,
        /// <summary>
        /// 2018016
        /// </summary>
        报告投递 = 2018016,
        /// <summary>
        /// 2018018 Alex 2016-04-20
        /// </summary>
        报告归档 = 2018018,
        /// <summary>
        /// 2018018 Alex 2016-04-20
        /// </summary>
        基础数据 = 2018019,
        /// <summary>
        /// 2018018 Alex 2016-04-20
        /// </summary>
        打印 = 2018020,
        /// <summary>
        /// 2018018 Alex 2016-04-20
        /// </summary>
        盖章 = 2018021
    }
    /// <summary>
    /// 取整的列类型
    /// </summary>
    public enum EnumRoundingColumntype
    {
        /// <summary>
        /// 单价=1
        /// </summary>
        [Description("UnitPrice")]
        单价 = 1,
        /// <summary>
        /// 总价=2
        /// </summary>
        [Description("TotalPrice")]
        总价 = 2,
        /// <summary>
        /// 净值=3
        /// </summary>
        [Description("NetPrice")]
        净值 = 3,
        /// <summary>
        /// 评估总值(主房+土地+附属))=4
        /// </summary>
        [Description("HouseTotalPrice")]
        评估总值_主房土地附属 = 4,
        /// <summary>
        /// 土地总价=5
        /// </summary>
        [Description("LandTotalPrice")]
        土地总价 = 5,
        /// <summary>
        /// 附属房屋总价=6
        /// </summary>
        [Description("SubHouseAllTotalPrice")]
        附属房屋总价 = 6,
        /// <summary>
        /// 装修总价=7
        /// </summary>
        [Description("DecorateAllTotalPrice")]
        装修总价 = 7,
        /// <summary>
        /// 动产总价=8
        /// </summary>
        [Description("MovableAllTotalPrice")]
        动产总价 = 8,
        /// <summary>
        /// 其他价值=9
        /// </summary>
        [Description("OtherValue")]
        其他价值 = 9,        
        /// <summary>
        /// 应补地价=10
        /// </summary>
        [Description("Shouldfilllandprice")]
        应补地价 = 10,
        /// <summary>
        /// 强制变现税费额=11
        /// </summary>
        [Description("liquiditytaxvalue")]
        强制变现税费额 = 11,
        /// <summary>
        /// 强制变现值=12
        /// </summary>
        [Description("liquidityvalue")]
        强制变现值 = 12,
        /// <summary>
        /// 法定优先受偿款=13
        /// </summary>
        [Description("legalpayment")]
        法定优先受偿款 = 13,
        /// <summary>
        /// 税费=14
        /// </summary>
        [Description("Tax")]
        税费 = 14,
        /// <summary>
        /// 纠错单价=21
        /// </summary>
        [Description("AdjustPrice")]
        纠错单价 = 21,
        /// <summary>
        /// 纠错总价=22
        /// </summary>
        [Description("AdjustTotalPrice")]
        纠错总价 = 22,
        /// <summary>
        /// 纠错净值=23
        /// </summary>
        [Description("adjustnetprice")]
        纠错净值 = 23,
        /// <summary>
        /// 纠错评估总值(主房+土地+附属))=24
        /// </summary>
        [Description("AdjustHouseTotalPrice")]
        纠错评估总值_主房土地附属 = 24,
        /// <summary>
        /// 业务员要求争议单价=31
        /// </summary>
        [Description("priceonrequest")]
        业务员要求争议单价 = 31,
        /// <summary>
        /// 业务员要求争议总价=32
        /// </summary>
        [Description("totalpriceonrequest")]
        业务员要求争议总价 = 32,
        /// <summary>
        /// 价格争议净值=33
        /// </summary>
        [Description("netpriceonrequest")]
        价格争议净值 = 33,
        /// <summary>
        /// 价格争议评估总价=34
        /// </summary>
        [Description("housetotalpriceonrequest")]
        价格争议评估总价 = 34,
        [Description("")]
        GetName = -1
    }
    /// <summary>
    /// 取整精确到哪位
    /// </summary>
    public enum EnumRoundingValue
    {
        /// <summary>
        /// 不取整=0
        /// </summary>
        不取整 = 0,
        /// <summary>
        /// 个位=1
        /// </summary>
        个位 = 1,
        /// <summary>
        /// 十位=10
        /// </summary>
        十位 = 10,
        /// <summary>
        /// 百位=100
        /// </summary>
        百位 = 100,
        /// <summary>
        /// 千位=1000
        /// </summary>
        千位 = 1000,
        /// <summary>
        /// 万位=10000
        /// </summary>
        万位 = 10000
    }
    /// <summary>
    /// 业务数据类型
    /// </summary>
    public enum EnumBusinessType
    {
        /// <summary>
        /// 0
        /// </summary>
        自定义表单=0,
        /// <summary>
        /// 1
        /// </summary>
        询价 = 1,
        /// <summary>
        /// 2
        /// </summary>
        预评 = 2,
        /// <summary>
        /// 3
        /// </summary>
        报告 = 3,
        /// <summary>
        /// 4
        /// </summary>
        普通报告收费 = 4,
        /// <summary>
        /// 5(此类型已无用)
        /// </summary>
        月结报告收费 = 5,
        /// <summary>
        /// 6
        /// </summary>
        多套询价 = 6//现在的人工询价
    }
    /// <summary>
    /// 审批状态
    /// </summary>
    public enum EnumApprovalStatus
    {
        /// <summary>
        /// 1
        /// </summary>
        未参与审批 = 1,
        /// <summary>
        /// 2
        /// </summary>
        正在审批中 = 2,
        /// <summary>
        /// 3
        /// </summary>
        审批完成 = 3
    }
    /// <summary>
    /// 获取表单对应的btsid
    /// </summary>
    public enum EnumBusTableSetupBTSId
    {
        /// <summary>
        /// 97
        /// </summary>
        案例数据_住宅 = 97,
        /// <summary>
        /// 98
        /// </summary>
        案例数据_办公 = 98,
        /// <summary>
        /// 99
        /// </summary>
        案例数据_商业 = 99,
        /// <summary>
        /// 100
        /// </summary>
        案例数据_工业 = 100,
        /// <summary>
        /// 101
        /// </summary>
        案例数据_土地 = 101,
        /// <summary>
        /// 102
        /// </summary>
        案例数据_资产 = 102,
        /// <summary>
        /// 103
        /// </summary>
        案例数据_其他 = 103,
        /// <summary>
        /// 104
        /// </summary>
        多套询价 = 104,//现在的人工询价
        /// <summary>
        /// 34
        /// </summary>
        单套询价 = 34,
        /// <summary>
        /// 21
        /// </summary>
        价格信息 = 21
    }
    /// <summary>
    /// 自定义表单中的表单类型
    /// </summary>
    public enum EnumFieldType
    {
        /// <summary>
        /// 20010101
        /// </summary>
        单行输入框 = 20010101,
        /// <summary>
        /// 20010102
        /// </summary>
        数字输入框 = 20010102,
        /// <summary>
        /// 20010103
        /// </summary>
        下拉框 = 20010103,
        /// <summary>
        /// 20010104
        /// </summary>
        单选框 = 20010104,
        /// <summary>
        /// 20010105
        /// </summary>
        复选框 = 20010105,
        /// <summary>
        /// 20010106
        /// </summary>
        日期 = 20010106,
        /// <summary>
        /// 20010107
        /// </summary>
        多行文本 = 20010107,
        /// <summary>
        /// 20010111
        /// </summary>
        测距仪 = 20010111,
    }
    /// <summary>
    /// 案例类型
    /// </summary>
    public enum EnumCaseType
    {
        /// <summary>
        /// 3001003
        /// </summary>
        评估案例 = 3001003,
        /// <summary>
        /// 3001010
        /// </summary>
        成交案例 = 3001010,
        /// <summary>
        /// 3001011
        /// </summary>
        报盘案例 = 3001011
    }
    /// <summary>
    /// 开票记录的状态
    /// </summary>
    public enum EnumChargeBillState
    {
        /// <summary>
        /// 1
        /// </summary>
        [Description("正常开票")]
        正常 = 1,
        /// <summary>
        /// 0
        /// </summary>
        [Description("已作废")]
        已作废 = 0,
        /// <summary>
        /// 2
        /// </summary>
        [Description("红冲")]
        红冲 = 2,
        /// <summary>
        /// 用于查询枚举描述
        /// </summary>
        [Description("")]
        GetName = -1
    }


    /// <summary>
    /// 收费记录的开票情况
    /// </summary>
    public enum EnumChargeBillDetailStatus
    {
        /// <summary>
        /// 0
        /// </summary>
        [Description("未开票")]
        未开票 = 0,
        /// <summary>
        /// 1
        /// </summary>
        [Description("正常开票")]
        正常 = 1,
        /// <summary>
        /// 2
        /// </summary>
        [Description("已作废")]
        已作废 = 2,
        /// <summary>
        /// 3
        /// </summary>
        [Description("红冲")]
        红冲 = 3,
        /// <summary>
        /// 4
        /// </summary>
        [Description("正常开票(废)")]
        正常开票废 = 4,
        /// <summary>
        /// 用于查询枚举描述
        /// </summary>
        [Description("")]
        GetName = -1
    }
    /// <summary>
    /// 估价师类型
    /// </summary>
    public enum EnumAppraiserType
    {

        /// <summary>
        /// 第一报告人=9018001
        /// </summary>
        [Description("第一报告人")]
        第一报告人 = 9018001,
        /// <summary>
        /// 参与报告人=9018002
        /// </summary>
        [Description("参与报告人")]
        参与报告人 = 9018002,
        /// <summary>
        /// 审核人=9018003
        /// </summary>
        [Description("审核人")]
        审核人 = 9018003
    }

    /// <summary>
    /// 案例来源
    /// </summary>
    public enum BizType
    {
        /// <summary>
        /// 测算2020001
        /// </summary>
        [Description("测算")]
        测算,
        /// <summary>
        /// 询价2020002
        /// </summary>
        [Description("询价")]
        询价,
        /// <summary>
        ///预评 2020003
        /// </summary>
        [Description("预评")]
        预评,
         /// <summary>
        /// 报告2020004
        /// </summary>
        [Description("报告")]
        报告
    }

    /// <summary>
    /// 分支机构类型
    /// </summary>
    public enum EnumSubCompanyType
    {
        /// <summary>
        /// 1
        /// </summary>
        [Description("分支机构")]
        分支机构 = 1,
        /// <summary>
        /// 2
        /// </summary>
        [Description("办事处")]
        办事处 = 2,
        /// <summary>
        /// 用于查询枚举描述
        /// </summary>
        [Description("")]
        GetName = -1
    }
    /// <summary>
    /// 管理状态
    /// </summary>
    public enum EnumSubCompanyManagerStatus
    {
        /// <summary>
        /// 1
        /// </summary>
        [Description("正常")]
        正常 = 1,
        /// <summary>
        /// 2
        /// </summary>
        [Description("筹备中")]
        筹备中 = 2,
        /// <summary>
        /// 3
        /// </summary>
        [Description("已注销")]
        已注销 = 3,
        /// <summary>
        /// 用于查询枚举描述
        /// </summary>
        [Description("")]
        GetName = -1
    }
    /// <summary>
    /// 分支机构/部门
    /// </summary>
    public enum EnumSubCompanyDataCode
    {
        /// <summary>
        /// 501607001
        /// </summary>
        [Description("分支机构_部门")]
        分支机构_部门 = 501607001
    }

    public enum EnumPublishSettingFunctionCode
    {
        /// <summary>
        /// 2017001
        /// </summary>
        [Description("aspose数据模版功能")]
        Aspose数据模版功能 = 2017001,
        /// <summary>
        /// 2017002
        /// </summary>
        [Description("照片生成按照等比例缩放")]
        照片生成按照等比例缩放 = 2017002,
        /// <summary>
        /// 2017003
        /// </summary>
        [Description("分配选择人员显示工作量")]
        分配选择人员显示工作量 = 2017003,
        /// <summary>
        /// 2017004
        /// </summary>
        [Description("消息提醒是否需要置顶窗口")]
        消息提醒是否需要置顶窗口 = 2017004,
        /// <summary>
        /// 2017005
        /// </summary>
        [Description("报告是否自动生成")]
        报告是否自动生成 = 2017005,
        /// <summary>
        /// 2017006
        /// </summary>
        [Description("自动更新案例附件")]
        自动更新案例附件 = 2017006
    }
}
