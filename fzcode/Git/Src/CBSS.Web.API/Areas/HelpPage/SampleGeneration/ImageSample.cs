using System;

namespace CBSS.Web.API.Areas.HelpPage
{
    /// <summary>
    /// This represents an image sample on the help page. There's a display template named ImageSample associated with this class.
    /// </summary>
    public class ImageSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSample"/> class.
        /// </summary>
        /// <param name="src">The URL of an image.</param>
        public ImageSample(string src)
        {
            if (src == null)
            {
                throw new ArgumentNullException("src");
            }
            Src = src;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ImageSample.Src”的 XML 注释
        public string Src { get; private set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ImageSample.Src”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ImageSample.Equals(object)”的 XML 注释
        public override bool Equals(object obj)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ImageSample.Equals(object)”的 XML 注释
        {
            ImageSample other = obj as ImageSample;
            return other != null && Src == other.Src;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ImageSample.GetHashCode()”的 XML 注释
        public override int GetHashCode()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ImageSample.GetHashCode()”的 XML 注释
        {
            return Src.GetHashCode();
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ImageSample.ToString()”的 XML 注释
        public override string ToString()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ImageSample.ToString()”的 XML 注释
        {
            return Src;
        }
    }
}