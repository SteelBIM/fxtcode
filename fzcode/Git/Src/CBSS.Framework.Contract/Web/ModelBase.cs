using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Framework.Contract
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModelBase”的 XML 注释
    public class ModelBase
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModelBase”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModelBase.ModelBase()”的 XML 注释
        public ModelBase()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModelBase.ModelBase()”的 XML 注释
        {
            CreateTime = DateTime.Now;
        }
        
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModelBase.ID”的 XML 注释
        public virtual int ID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModelBase.ID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModelBase.CreateTime”的 XML 注释
        public virtual DateTime CreateTime { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModelBase.CreateTime”的 XML 注释
    }
}
