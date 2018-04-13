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

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ImageSample.Src���� XML ע��
        public string Src { get; private set; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ImageSample.Src���� XML ע��

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ImageSample.Equals(object)���� XML ע��
        public override bool Equals(object obj)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ImageSample.Equals(object)���� XML ע��
        {
            ImageSample other = obj as ImageSample;
            return other != null && Src == other.Src;
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ImageSample.GetHashCode()���� XML ע��
        public override int GetHashCode()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ImageSample.GetHashCode()���� XML ע��
        {
            return Src.GetHashCode();
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ImageSample.ToString()���� XML ע��
        public override string ToString()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ImageSample.ToString()���� XML ע��
        {
            return Src;
        }
    }
}