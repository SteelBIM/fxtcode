using System;

namespace CBSS.Web.API.Areas.HelpPage
{
    /// <summary>
    /// This represents a preformatted text sample on the help page. There's a display template named TextSample associated with this class.
    /// </summary>
    public class TextSample
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TextSample.TextSample(string)���� XML ע��
        public TextSample(string text)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TextSample.TextSample(string)���� XML ע��
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            Text = text;
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TextSample.Text���� XML ע��
        public string Text { get; private set; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TextSample.Text���� XML ע��

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TextSample.Equals(object)���� XML ע��
        public override bool Equals(object obj)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TextSample.Equals(object)���� XML ע��
        {
            TextSample other = obj as TextSample;
            return other != null && Text == other.Text;
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TextSample.GetHashCode()���� XML ע��
        public override int GetHashCode()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TextSample.GetHashCode()���� XML ע��
        {
            return Text.GetHashCode();
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TextSample.ToString()���� XML ע��
        public override string ToString()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TextSample.ToString()���� XML ע��
        {
            return Text;
        }
    }
}