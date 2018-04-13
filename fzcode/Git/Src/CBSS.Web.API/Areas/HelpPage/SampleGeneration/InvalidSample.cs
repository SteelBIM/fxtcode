using System;

namespace CBSS.Web.API.Areas.HelpPage
{
    /// <summary>
    /// This represents an invalid sample on the help page. There's a display template named InvalidSample associated with this class.
    /// </summary>
    public class InvalidSample
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��InvalidSample.InvalidSample(string)���� XML ע��
        public InvalidSample(string errorMessage)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��InvalidSample.InvalidSample(string)���� XML ע��
        {
            if (errorMessage == null)
            {
                throw new ArgumentNullException("errorMessage");
            }
            ErrorMessage = errorMessage;
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��InvalidSample.ErrorMessage���� XML ע��
        public string ErrorMessage { get; private set; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��InvalidSample.ErrorMessage���� XML ע��

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��InvalidSample.Equals(object)���� XML ע��
        public override bool Equals(object obj)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��InvalidSample.Equals(object)���� XML ע��
        {
            InvalidSample other = obj as InvalidSample;
            return other != null && ErrorMessage == other.ErrorMessage;
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��InvalidSample.GetHashCode()���� XML ע��
        public override int GetHashCode()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��InvalidSample.GetHashCode()���� XML ע��
        {
            return ErrorMessage.GetHashCode();
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��InvalidSample.ToString()���� XML ע��
        public override string ToString()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��InvalidSample.ToString()���� XML ע��
        {
            return ErrorMessage;
        }
    }
}