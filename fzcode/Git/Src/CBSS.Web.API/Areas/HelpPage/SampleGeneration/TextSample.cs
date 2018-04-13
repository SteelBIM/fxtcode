using System;

namespace CBSS.Web.API.Areas.HelpPage
{
    /// <summary>
    /// This represents a preformatted text sample on the help page. There's a display template named TextSample associated with this class.
    /// </summary>
    public class TextSample
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TextSample.TextSample(string)”的 XML 注释
        public TextSample(string text)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TextSample.TextSample(string)”的 XML 注释
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            Text = text;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TextSample.Text”的 XML 注释
        public string Text { get; private set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TextSample.Text”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TextSample.Equals(object)”的 XML 注释
        public override bool Equals(object obj)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TextSample.Equals(object)”的 XML 注释
        {
            TextSample other = obj as TextSample;
            return other != null && Text == other.Text;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TextSample.GetHashCode()”的 XML 注释
        public override int GetHashCode()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TextSample.GetHashCode()”的 XML 注释
        {
            return Text.GetHashCode();
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TextSample.ToString()”的 XML 注释
        public override string ToString()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TextSample.ToString()”的 XML 注释
        {
            return Text;
        }
    }
}