using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Core.Utility
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“StringValueAttribute”的 XML 注释
    public class StringValueAttribute : Attribute
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“StringValueAttribute”的 XML 注释
    {
        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        private readonly int _SortIndex;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“StringValueAttribute.SortIndex”的 XML 注释
        public int SortIndex
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“StringValueAttribute.SortIndex”的 XML 注释
        {
            get { return _SortIndex; }
        }
        #endregion

        #region Constructor
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“StringValueAttribute.StringValueAttribute(int, string)”的 XML 注释
        public StringValueAttribute(int sortIndex, string value)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“StringValueAttribute.StringValueAttribute(int, string)”的 XML 注释
        {
            this._SortIndex = sortIndex;
            this.StringValue = value;
        }

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        #endregion
    }
}
