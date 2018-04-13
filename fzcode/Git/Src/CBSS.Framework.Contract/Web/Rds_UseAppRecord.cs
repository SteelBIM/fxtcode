using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Rds_UseAppRecords”的 XML 注释
    public class Rds_UseAppRecords
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Rds_UseAppRecords”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Rds_UseAppRecords.records”的 XML 注释
        public List<Rds_UseAppRecord> records { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Rds_UseAppRecords.records”的 XML 注释
    }

    /// <summary>
    /// 用户打开APP记录队列
    /// </summary>
    public class Rds_UseAppRecord
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 应用渠道ID
        /// </summary>
        public string AppChannelID { get; set; }
        /// <summary>
        /// 应用版本号
        /// </summary>
        public string AppVersionNumber { get; set; }

        /// <summary>
        /// 使用APP时间（单位：S）
        /// </summary>
        public int UseAppTime { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateData { get; set; }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Rds_UseAppRecord.AppID”的 XML 注释
        public string AppID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Rds_UseAppRecord.AppID”的 XML 注释
        
    }
}
