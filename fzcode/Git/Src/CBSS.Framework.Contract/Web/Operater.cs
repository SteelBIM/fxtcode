using System;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// 记录操作历史
    /// </summary>
    public class Operater
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Operater.Operater()”的 XML 注释
        public Operater()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Operater.Operater()”的 XML 注释
        {
            this.Name = "Anonymous";
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Operater.Name”的 XML 注释
        public string Name { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Operater.Name”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Operater.IP”的 XML 注释
        public string IP { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Operater.IP”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Operater.Time”的 XML 注释
        public DateTime Time { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Operater.Time”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Operater.Token”的 XML 注释
        public Guid Token { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Operater.Token”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Operater.UserId”的 XML 注释
        public int UserId { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Operater.UserId”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Operater.Method”的 XML 注释
        public string Method { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Operater.Method”的 XML 注释
    }
}
