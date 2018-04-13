using System;
using System.Collections;


namespace CBSS.Core.Pay.AliPay.Util
{
    #region BuyParam

    /// <summary>
    /// BuyParam��Ĭ�ϱ����Ϊ��Utf��8
    /// </summary>
    public class DigitalGoods
    {
        #region Member Variables


        protected string _service;
        protected string _partner;
        protected string _notifyUrl;
        protected string _returnUrl;
        protected string _agent;
        protected string _inputCharset;
        protected string _sign;
        protected string _signType;
        protected string _subject;
        protected string _body;
        protected string _outTradeNo;
        protected decimal? _price;
        protected int? _quantity;
        protected string _showUrl;
        protected string _sellerEmail;
        protected string _sellerId;
        protected string _buyerEmail;
        protected string _buyerId;
        protected string _buyerMsg;
        #endregion

        #region Constructors

        public DigitalGoods() { }

        /// <summary>
        /// ���в������Ǳ���ģ�����sellerEmail��sellerId����������д��һ,��һ������λNull
        /// </summary>
        /// <param name="service">create_digital_goods_trade_p</param>
        /// <param name="partner">���������֧�������û�ID</param>
        /// <param name="sign">֧������Key</param>
        /// <param name="signType"></param>
        /// <param name="subject">��Ʒ����</param>
        /// <param name="outTradeNo">�ⲿ���׺�</param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="sellerEmail">��sellerId������дһ��</param>
        /// <param name="sellerId"></param>
        public DigitalGoods(string service, string partner, string sign, string signType, string subject, string outTradeNo, decimal? price, int? quantity, string sellerEmail, string sellerId)
        {
            this._service = service;
            this._partner = partner;
            this._sign = sign;
            this._signType = signType;
            this._subject = subject;
            this._outTradeNo = outTradeNo;
            this._price = price;
            this._quantity = quantity;
            this._sellerEmail = sellerEmail;
            this._sellerId = sellerId;
        }
        public DigitalGoods(string service, string partner, string notifyUrl, string returnUrl, string agent, string inputCharset, string sign, string signType, string subject, string body, string outTradeNo, decimal? price, int? quantity, string showUrl, string sellerEmail, string sellerId, string buyerEmail, string buyerId, string buyerMsg)
            : this(service, partner, sign, signType, subject, outTradeNo, price, quantity, sellerEmail, sellerId)
        {

            this._notifyUrl = notifyUrl;
            this._returnUrl = returnUrl;
            this._agent = agent;
            this._inputCharset = inputCharset;
            this._body = body;
            this._showUrl = showUrl;
            this._buyerEmail = buyerEmail;
            this._buyerId = buyerId;
            this._buyerMsg = buyerMsg;
        }

        #endregion

        #region Public Properties


        /// <summary>
        /// create_digital_goods_trade_p
        /// </summary>
        public string Service
        {
            get
            {
                if (_service == null)
                {
                    throw new ArgumentNullException(_service);
                }
                return _service;
            }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Service", value, value.ToString());
                _service = value;
            }
        }
        /// <summary>
        /// ���������֧�������û�ID
        /// </summary>
        public string Partner
        {
            get
            {
                if (_partner == null)
                {
                    throw new ArgumentNullException(_partner);
                }
                return _partner;
            }
            set
            {
                if (value != null && value.Length > 16)
                    throw new ArgumentOutOfRangeException("Invalid value for Partner", value, value.ToString());
                _partner = value;
            }
        }
        /// <summary>
        /// ֪ͨ����URL,���������첽���ش������Ľӿڡ���Щ�������޷��������ش������ģ���ô��Ҫͨ�����URL���������첽���ظ�������顣
        /// </summary>
        public string Notify_Url
        {
            get { return _notifyUrl; }
            set
            {
               // if (value != null && value.Length > 50)
                //    throw new ArgumentOutOfRangeException("Invalid value for Notify_Url", value, value.ToString());
                _notifyUrl = value;
            }
        }
        /// <summary>
        /// �������URL�����������������ش������Ľӿڡ�֧������������������������������ظ����URL
        /// </summary>
        public string Return_Url
        {
            get { return _returnUrl; }
            set
            {
                //if (value != null && value.Length > 50)
                //    throw new ArgumentOutOfRangeException("Invalid value for Return_Url", value, value.ToString());
                _returnUrl = value;
            }
        }
        /// <summary>
        /// ���һЩ������վ�Ľ��ף���һ���ġ�����������ϵ�������̿����ڽ����д��ݸò������������������ݡ����ﴫ�͵�ֵ����ʹ�ô���������֧�����ʻ���PartnerID
        /// </summary>
        public string Agent
        {
            get { return _agent; }
            set
            {
                if (value != null && value.Length > 16)
                    throw new ArgumentOutOfRangeException("Invalid value for Agent", value, value.ToString());
                _agent = value;
            }
        }
        /// <summary>
        /// �������ϵͳ��֧����ϵͳ֮�佻����Ϣʱʹ�õı����ַ���,Ĭ��ΪUtf��8
        /// </summary>
        public string _Input_Charset
        {
            get
            {
                if (_inputCharset == null)
                    return "utf-8";
                return _inputCharset;
            }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for InputCharset", value, value.ToString());
                _inputCharset = value;
            }
        }
        /// <summary>
        /// �ڴ����뽻�װ�ȫУ���루key������ͬ��֧�����ĵ���Sign
        /// </summary>
        public string Sign
        {
            get
            {
                if (_sign == null)
                {
                    throw new ArgumentNullException(_sign);
                }
                return _sign;
            }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Sign", value, value.ToString());
                _sign = value;
            }
        }
        /// <summary>
        /// ���ܲ������㷨��Md5��ֻʵ����Md5����DSA
        /// </summary>
        public string Sign_Type
        {
            get
            {
                if (_signType == null)
                {
                    throw new ArgumentNullException(_signType);
                }
                return _signType;
            }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Sign_Type", value, value.ToString());
                _signType = value;
            }
        }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public string Subject
        {
            get
            {
                if (_subject == null)
                {
                    throw new ArgumentNullException(_subject);
                }
                return _subject;
            }
            set
            {
                if (value != null && value.Length > 256)
                    throw new ArgumentOutOfRangeException("Invalid value for Subject", value, value.ToString());
                _subject = value;
            }
        }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public string Body
        {
            get { return _body; }
            set
            {
                if (value != null && value.Length > 400)
                    throw new ArgumentOutOfRangeException("Invalid value for Body", value, value.ToString());
                _body = value;
            }
        }
        /// <summary>
        /// �ⲿ���׺�,Ҫ��֤Ψһ���Ƽ�GUID
        /// </summary>
        public string Out_Trade_No
        {
            get
            {
                if (_outTradeNo == null)
                {
                    throw new ArgumentNullException(_outTradeNo);
                } return _outTradeNo;
            }
            set
            {
                if (value != null && value.Length > 64)
                    throw new ArgumentOutOfRangeException("Invalid value for OutTradeNo", value, value.ToString());
                _outTradeNo = value;
            }
        }
        /// <summary>
        /// ��Ʒ���ۣ�������λС��������Ϊ0.01��100000000.00֮��,���С��������λ����������
        /// </summary>
        public decimal? Price
        {
            get
            {
                _price = decimal.Round((decimal)_price, 2);
                return (decimal?)_price;
            }
            set
            {
                if (value < 0.01m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(_quantity.ToString(), "����Ϊ0.01��100000000.00֮��");
                }
                _price = value;
            }
        }
        /// <summary>
        /// ������������������λ��������
        /// </summary>
        public int? Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (value < 0 || value > 1000000)
                {
                    throw new ArgumentNullException(_quantity.ToString(), "����ΪС��1000000��������");
                }
                _quantity = value;
            }
        }
        /// <summary>
        /// ��Ʒչʾ��ַ
        /// </summary>
        public string Show_Url
        {
            get { return _showUrl; }
            set
            {
                if (value != null && value.Length > 400)
                    throw new ArgumentOutOfRangeException("Invalid value for ShowUrl", value, value.ToString());
                _showUrl = value;
            }
        }
        /// <summary>
        /// ������֧������ע��Email
        /// </summary>
        public string Seller_Email
        {
            get
            {
                if (_sellerEmail == null && _sellerId == null)
                {
                    throw new ArgumentNullException(_sellerEmail, "Seller_Id,Seller_Email����ͬʱΪ��");
                }
                return _sellerEmail;
            }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for SellerEmail", value, value.ToString());

                _sellerEmail = value;
            }
        }
        /// <summary>
        /// ������֧������ע��ID
        /// </summary>
        public string Seller_Id
        {
            get
            {
                if (_sellerEmail == null && _sellerId == null)
                {
                    throw new ArgumentNullException(_sellerId, "Seller_Id,Seller_Email����ͬʱΪ��");
                }
                return _sellerId;
            }
            set
            {
                if (value != null && value.Length > 30)
                    throw new ArgumentOutOfRangeException("Invalid value for SellerId", value, value.ToString());

                _sellerId = value;
            }
        }
        /// <summary>
        /// �����֧������ע��Email
        /// </summary>
        public string Buyer_Email
        {
            get { return _buyerEmail; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for BuyerEmail", value, value.ToString());
                _buyerEmail = value;
            }
        }
        /// <summary>
        /// �����֧������ע��ID��
        /// </summary>
        public string Buyer_Id
        {
            get { return _buyerId; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for BuyerId", value, value.ToString());
                _buyerId = value;
            }
        }
        /// <summary>
        /// �������
        /// </summary>
        public string Buyer_Msg
        {
            get { return _buyerMsg; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("Invalid value for BuyerMsg", value, value.ToString());
                _buyerMsg = value;
            }
        }

        #endregion
    }

    #endregion
}
