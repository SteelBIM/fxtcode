using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [Description("预评已撤销")]
        预评已撤销,
        [Description("待分配报告")]
        待分配报告,
        撰写报告中,
        [Description("待撰写报告")]
        待撰写报告,
        [Description("报告审批中")]
        报告审批中,
        [Description("报告已完成")]
        报告已完成,
        [Description("报告已撤销")]
        报告已撤销,
        [Description("业务已完成")]
        业务已完成
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
        案例数据 = 2018011
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
        多套询价 = 6
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
        多套询价 = 104,
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

}
