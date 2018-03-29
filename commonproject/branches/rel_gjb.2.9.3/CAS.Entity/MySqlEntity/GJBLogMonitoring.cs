using CAS.Entity.BaseDAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace CAS.Entity.MySqlEntity
{

    [Serializable]
    public class GJBLogMonitoringEntrustInfo
    {
        /// <summary>
        /// 业务编号
        /// </summary>
        public long EntrustId { get; set; }
        /// <summary>
        /// 报告ID
        /// </summary>
        public long ReportId { get; set; }
        /// <summary>
        /// 预评ID
        /// </summary>
        public long YpId { get; set; }
        /// <summary>
        /// 人工询价ID（多套）
        /// </summary>
        public long QId { get; set; }
        /// <summary>
        /// 生成操作时用到的模板ID
        /// </summary>
        public long TemplateId { get; set; }
        /// <summary>
        /// 委估对象id
        /// </summary>
        public long ObjectId { get; set; }
        /// <summary>
        /// 收费ID
        /// </summary>
        public long ChargeEntrustId { get; set; }
        /// <summary>
        /// 归档ID
        /// </summary>
        public long BackupId { get; set; }
    }

    /// <summary>
    /// 估价宝各页面或功能耗时监控
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_LogMonitoring")]
    public class GJBLogMonitoring : BaseTO
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        [SQLField("Id", EnumDBFieldUsage.PrimaryKey, true)]
        public long Id { get; set; }
        /// <summary>
        /// 站点类型：gjb、gjbapi、gjbcas、gjbwx(使用常量类GJBLogEnum.WebType)
        /// </summary>
        public string WebType { get; set; }
        /// <summary>
        /// 开始请求时间
        /// </summary>
        public DateTime TimeStart { get; set; }
        /// <summary>
        /// 结束请求时间
        /// </summary>
        public DateTime TimeEnd { get; set; }
        /// <summary>
        /// 总耗时，单位：毫秒
        /// </summary>
        public int TimeRange { get; set; }
        /// <summary>
        /// 请求的url
        /// </summary>
        public string RequestUrl { get; set; }
        private int opFunctionType=0;
        /// <summary>
        /// 操作的功能类型,如果是操作的功能则存当前值，比如报告生成等(使用枚举GJBLogEnum.OpFunctionType)
        /// </summary>
        public int OpFunctionType
        {
            get { return opFunctionType; }
            set { opFunctionType = value; }
        }
        /// <summary>
        /// 业务相关信息(json格式)，存entrustId、reportId、qId等
        /// </summary>
        public string EntrustInfo { get; set; }

        [SQLReadOnly]
        /// <summary>
        /// 业务相关信息封装后的实体（自动封装和解析,用于读取操作）
        /// </summary>
        public GJBLogMonitoringEntrustInfo EntrustInfoEntity 
        { 
            get
            {
                GJBLogMonitoringEntrustInfo result = null;
                if(!string.IsNullOrEmpty(this.EntrustInfo))
                {

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    try
                    {
                        jss.MaxJsonLength = int.MaxValue;
                        result = jss.Deserialize<GJBLogMonitoringEntrustInfo>(this.EntrustInfo);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return result;
            }
            set 
            {
                string result = null;
                if (value != null)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = 10240000;
                    try
                    {
                        result = jss.Serialize(value);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                EntrustInfo = result;
            }
        }
        /// <summary>
        /// 其他内容
        /// </summary>
        public string TextContent { get; set; }
        private int companyId = 0;
        /// <summary>
        /// 当前评估机构ID，如果没有则传0
        /// </summary>
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
        /// <summary>
        /// 机构Code
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 当前用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserTrueName { get; set; }
        /// <summary>
        /// 当前用户IP
        /// </summary>
        public string UserIP { get; set; }
        /// <summary>
        /// 客户端系统
        /// </summary>
        public string OS { get; set; }
        /// <summary>
        /// 客户端系统位数
        /// </summary>
        public string OSBit { get; set; }
        /// <summary>
        /// 客户端浏览器名称
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// 客户端浏览器版本号
        /// </summary>
        public string BrowserVersion { get; set; }
        /// <summary>
        /// 客户端浏览器内核信息
        /// </summary>
        public string UserAgent { get; set; }
        private DateTime createTime = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
    }
}
