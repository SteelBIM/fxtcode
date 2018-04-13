using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// 用于写数据修改，添加等历史日志
    /// </summary>
    public interface IAuditable
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IAuditable.WriteLog(int, string, string, string, string, ModelBase)”的 XML 注释
        void WriteLog(int modelId, string userName, string moduleName, string tableName, string eventType, ModelBase newValues);
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IAuditable.WriteLog(int, string, string, string, string, ModelBase)”的 XML 注释
    }
}
