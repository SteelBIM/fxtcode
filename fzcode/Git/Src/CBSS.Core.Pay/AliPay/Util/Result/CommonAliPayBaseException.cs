using System;
using System.Collections.Generic;
using System.Text;

namespace CBSS.Core.Pay.AliPay.Util.Result
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonAliPayBaseException : ApplicationException
    {
        public CommonAliPayBaseException(string message, int errCode)
            : base(message)
        {
            this.errCode = errCode;
        }
        public CommonAliPayBaseException(string message, Exception inner)
            : base(message, inner)
        { }
        public CommonAliPayBaseException()
        { }
        public CommonAliPayBaseException(int errCode)
        {
            this.errCode = errCode;
        }
        private int errCode;
        public int ErrCode
        {
            get { return errCode; }
            set { this.errCode = value; }
        }
    }
}
