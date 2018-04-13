using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract
{ 
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Rds_UserLoginRecords”的 XML 注释
    public class Rds_UserLoginRecords
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Rds_UserLoginRecords”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Rds_UserLoginRecords.records”的 XML 注释
        public List<Rds_UserLoginRecord> records { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Rds_UserLoginRecords.records”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Rds_UserLoginRecords.Rds_UserLoginRecords()”的 XML 注释
        public Rds_UserLoginRecords()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Rds_UserLoginRecords.Rds_UserLoginRecords()”的 XML 注释
        {
            records = new List<Contract.Rds_UserLoginRecord>();
        }
    }

    /// <summary>
    /// 用户登陆记录队列
    /// </summary>
    public class Rds_UserLoginRecord
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
        /// 状态 0正常登陆 1注册 2第一次登陆
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateData { get; set; }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Rds_UserLoginRecord.AppID”的 XML 注释
        public string AppID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Rds_UserLoginRecord.AppID”的 XML 注释
        
    }
}
