using System;

namespace CBSS.Web.API.Areas.HelpPage
{
    /// <summary>
    /// This represents an invalid sample on the help page. There's a display template named InvalidSample associated with this class.
    /// </summary>
    public class InvalidSample
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“InvalidSample.InvalidSample(string)”的 XML 注释
        public InvalidSample(string errorMessage)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“InvalidSample.InvalidSample(string)”的 XML 注释
        {
            if (errorMessage == null)
            {
                throw new ArgumentNullException("errorMessage");
            }
            ErrorMessage = errorMessage;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“InvalidSample.ErrorMessage”的 XML 注释
        public string ErrorMessage { get; private set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“InvalidSample.ErrorMessage”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“InvalidSample.Equals(object)”的 XML 注释
        public override bool Equals(object obj)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“InvalidSample.Equals(object)”的 XML 注释
        {
            InvalidSample other = obj as InvalidSample;
            return other != null && ErrorMessage == other.ErrorMessage;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“InvalidSample.GetHashCode()”的 XML 注释
        public override int GetHashCode()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“InvalidSample.GetHashCode()”的 XML 注释
        {
            return ErrorMessage.GetHashCode();
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“InvalidSample.ToString()”的 XML 注释
        public override string ToString()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“InvalidSample.ToString()”的 XML 注释
        {
            return ErrorMessage;
        }
    }
}