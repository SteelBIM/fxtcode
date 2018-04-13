///直接到账交易物
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kingsun.SynchronousStudy.AliPay.Util
{
    public class DirectGoods:DigitalGoods
    {
        /// <summary>
        /// 直接到账独有的
        /// </summary>
        protected decimal? _totalFee;
        protected string _paymentType;

        /// <summary>
        /// 所有参数都是必须的，但是sellerEmail和sellerId可以任意填写其一,另一个设置为Null
        /// </summary>
        /// <param name="service">服务参数</param>
        /// <param name="partner">合作伙伴在支付宝的用户ID</param>
        /// <param name="sign">支付宝的Key</param>
        /// <param name="signType">加密协议</param>
        /// <param name="subject">商品名称</param>
        /// <param name="body">商品描述</param>
        /// <param name="outTradeNo">外部交易号</param>
        /// <param name="totalFee">总金额</param>
        /// <param name="quantity"></param>
        /// <param name="sellerEmail">和sellerId任意填写一个</param>
        /// <param name="sellerId"></param>
        public DirectGoods(string service, string partner, string sign, string signType, string subject, string body, string outTradeNo, decimal? totalFee, string paymentType, string sellerEmail, string sellerId)
            :base(service,partner,sign,signType,subject,outTradeNo,totalFee,1,sellerEmail,sellerId)
        {
           
            this._totalFee = totalFee;
            this._paymentType = paymentType;
            this._body = body;

        }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal? total_fee
        {
            get { return _totalFee; }
            set { _totalFee = value; }
        }
        /// <summary>
        /// 支付类型
        /// </summary>
        public string Payment_Type
        {
            get { return _paymentType; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for PaymentType", value, value.ToString());
                _paymentType = value;
            }
        }
    }
}
