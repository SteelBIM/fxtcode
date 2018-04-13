using System;
using System.Collections;


namespace CBSS.Core.Pay.AliPay.Util
{
	#region DigitalNotify

	/// <summary>
	/// 交易通知信息，包含了虚拟和实物两种信息
	/// </summary>
	public class NotifyEventArgs 
		{
		#region Member Variables
		
            
		
		protected string _notifyType;
		protected string _notifyId;
		protected DateTime? _notifyTime;
		protected string _sign;
		protected string _signType;
		protected string _tradeNo;
		protected string _outTradeNo;
		protected string _paymentType;
		protected string _subject;
		protected string _body;
		protected decimal? _price;
		protected int? _quantity;
		protected decimal? _totalFee;
		protected string _sellerEmail;
		protected string _sellerId;
		protected string _buyerEmail;
		protected string _buyerId;
		protected string _tradeStatus;
		protected string _refundStatus;
		protected string _logisticsStatus;
		protected DateTime? _gmtCreate;
		protected DateTime? _gmtPayment;
		protected DateTime? _gmtSendGoods;
		protected DateTime? _gmtRefund;
		protected DateTime? _gmtClose;
		protected DateTime? _gmtLogisticsModify;

        protected string exterface;
        protected string is_success;
        //实物交易独有
        protected decimal? _discount;
        protected string _useCoupon;
        protected decimal? _couponDiscount;
        protected string _isTotalFeeAdjust;
        protected string _sellerActions;
        protected string _buyerActions;
        protected string _logisticsType;
        protected decimal? _logisticsFee;
        protected string _logisticsPayment;
        protected string _receiveName;
        protected string _receiveAddress;
        protected string _receiveZip;
        protected string _receivePhone;
        protected string _receiveMobile;
		#endregion

		#region Constructors

		public NotifyEventArgs() { }

		public NotifyEventArgs( string notifyType, string notifyId, DateTime? notifyTime, string sign, string signType, string tradeNo, string outTradeNo, string paymentType, string subject, decimal? price, int? quantity, decimal? totalFee, 
            string sellerEmail, string sellerId, string buyerEmail, string buyerId, string tradeStatus,string useConpon,string isTotalFeeAdjust)
		{
			this._notifyType = notifyType;
			this._notifyId = notifyId;
			this._notifyTime = notifyTime;
			this._sign = sign;
			this._signType = signType;
			this._tradeNo = tradeNo;
			this._outTradeNo = outTradeNo;
			this._paymentType = paymentType;
			this._subject = subject;		
			this._price = price;
			this._quantity = quantity;
			this._totalFee = totalFee;
			this._sellerEmail = sellerEmail;
			this._sellerId = sellerId;
			this._buyerEmail = buyerEmail;
			this._buyerId = buyerId;
			this._tradeStatus = tradeStatus;
            this._isTotalFeeAdjust= isTotalFeeAdjust;
            this._useCoupon = useConpon;
		}

		#endregion

		#region Public Properties

	
/// <summary>
/// 
/// </summary>
		public string Notify_Type
		{
			get
            {
                if (_notifyType == null)
                {
                    throw new ArgumentNullException(_notifyType);
                } 
                return _notifyType; }
			set
			{
				_notifyType = value;
			}
		}

		public string Notify_Id
		{
            get
            {
                if (_notifyId == null)
                {
                    throw new ArgumentNullException(_notifyId);
                } return _notifyId;
            }
			set
			{
				_notifyId = value;
			}
		}

		public DateTime? Notify_Time
		{
            get
            {
                if (_notifyType == null)
                {
                    throw new ArgumentNullException(_notifyType);
                } return _notifyTime;
            }
			set { _notifyTime = value; }
		}

		public string Sign
		{
            get
            {
                if (_sign == null)
                {
                    throw new ArgumentNullException(_sign);
                } return _sign;
            }
			set
			{
				_sign = value;
			}
		}

		public string Sign_Type
		{
            get
            {
                if (_signType == null)
                {
                    throw new ArgumentNullException(_signType);
                } return _signType;
            }
			set
			{
				_signType = value;
			}
		}

		public string Trade_No
		{
            get
            {
                if (_tradeNo == null)
                {
                    throw new ArgumentNullException(_tradeNo);
                } return _tradeNo;
            }
			set
			{
				_tradeNo = value;
			}
		}

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
				_outTradeNo = value;
			}
		}

		public string Payment_Type
		{
            get
            {
                if (_paymentType == null)
                {
                    throw new ArgumentNullException(_paymentType);
                } return _paymentType;
            }
			set
			{
				_paymentType = value;
			}
		}

		public string Subject
		{
            get
            {
                if (_subject == null)
                {
                    throw new ArgumentNullException(_subject);
                } return _subject;
            }
			set
			{
				if ( value != null && value.Length > 256)
					throw new ArgumentOutOfRangeException("Invalid value for Subject", value, value.ToString());
				_subject = value;
			}
		}

		public string Body
		{
			get { return _body; }
			set
			{
				if ( value != null && value.Length > 400)
					throw new ArgumentOutOfRangeException("Invalid value for Body", value, value.ToString());
				_body = value;
			}
		}

		public decimal? Price
		{
            get
            {
               // _price = decimal?.Round(_price, 2);
               return _price;
            }
			set {
                if (value < 0.01m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(_quantity.ToString(), "必须为0.01在100000000.00之间");
                } 
                _price = value;
            }
		}

		public int? Quantity
		{
            get
            {
               
                return _quantity;
            }
			set 
            {
                if (value < 0 || value  > 1000000)
                {
                    throw new ArgumentNullException(_quantity.ToString(), "必须为小于1000000的正整数");
                } 
                _quantity = value;
            }
		}

		public decimal? Total_Fee
		{
            get
            {
                //_totalFee = (decimal?)decimal.Round(_totalFee, 2);
                return _totalFee;
            }
			set 
            {
                if (value  < 0.01m || value  > 100000000.00m)
                {
                    throw new ArgumentNullException(_totalFee.ToString(), "必须为0.01在100000000.00之间");
                }
                _totalFee = value;
            }
		}

		public string Seller_Email
		{
            get
            {
                if (_sellerEmail == null)
                {
                    throw new ArgumentNullException(_sellerEmail);
                } return _sellerEmail;
            }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for SellerEmail", value, value.ToString());
				_sellerEmail = value;
			}
		}

		public string Seller_Id
		{
            get
            {
                if (_sellerId == null)
                {
                    throw new ArgumentNullException(_sellerId);
                } return _sellerId;
            }
			set
			{
				if ( value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for SellerId", value, value.ToString());
				_sellerId = value;
			}
		}

		public string Buyer_Email
		{
            get
            {
                if (_buyerEmail == null)
                {
                    throw new ArgumentNullException(_buyerEmail);
                } return _buyerEmail;
            }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for BuyerEmail", value, value.ToString());
				_buyerEmail = value;
			}
		}

		public string Buyer_Id
		{
            get
            {
                if (_buyerId == null)
                {
                    throw new ArgumentNullException(_buyerId);
                } return _buyerId;
            }
			set
			{
				if ( value != null && value.Length > 30)
					throw new ArgumentOutOfRangeException("Invalid value for BuyerId", value, value.ToString());
				_buyerId = value;
			}
		}

		public string Trade_Status
		{
            get
            {
                if (_tradeStatus == null)
                {
                    throw new ArgumentNullException(_tradeStatus);
                } return _tradeStatus;
            }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for TradeStatus", value, value.ToString());
				_tradeStatus = value;
			}
		}

		public string Refund_Status
		{
			get { return _refundStatus; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for RefundStatus", value, value.ToString());
				_refundStatus = value;
			}
		}

		public string Logistics_Status
		{
			get { return _logisticsStatus; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for LogisticsStatus", value, value.ToString());
				_logisticsStatus = value;
			}
		}

		public DateTime? Gmt_Create
		{
			get { return _gmtCreate; }
			set { _gmtCreate = value; }
		}

		public DateTime? Gmt_Payment
		{
			get { return _gmtPayment; }
			set { _gmtPayment = value; }
		}

		public DateTime? Gmt_SendGoods
		{
			get { return _gmtSendGoods; }
			set { _gmtSendGoods = value; }
		}

		public DateTime? Gmt_Refund
		{
			get { return _gmtRefund; }
			set { _gmtRefund = value; }
		}

		public DateTime? Gmt_Close
		{
			get { return _gmtClose; }
			set { _gmtClose = value; }
		}

		public DateTime? Gmt_Logistics_Modify
		{
			get { return _gmtLogisticsModify; }
			set { _gmtLogisticsModify = value; }
		}

        public string Is_success
        {
            get
            {
                return is_success;
            }
            set
            {
                is_success = value;
            }
        }

        public string Exterface
        {
            get { return exterface; }
            set { exterface = value; }
        }

        //实物交易独有
     
        public decimal? Discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        public string Use_Coupon
        {
            get { return _useCoupon; }
            set { _useCoupon = value; }
        }

        public decimal? Coupon_Discount
        {
            get { return _couponDiscount; }
            set { _couponDiscount = value; }
        }

        public string Is_Total_Fee_Adjust
        {
            get { return _isTotalFeeAdjust; }
            set { _isTotalFeeAdjust = value; }
        }

        public string Seller_Actions
        {
            get { return _sellerActions; }
            set
            {
                if (value != null && value.Length > 150)
                    throw new ArgumentOutOfRangeException("Invalid value for SellerActions", value, value.ToString());
                _sellerActions = value;
            }
        }

        public string Buyer_Actions
        {
            get { return _buyerActions; }
            set
            {
                if (value != null && value.Length > 150)
                    throw new ArgumentOutOfRangeException("Invalid value for BuyerActions", value, value.ToString());
                _buyerActions = value;
            }
        }

        public string Logistics_Type
        {
            get { return _logisticsType; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for LogisticsType", value, value.ToString());
                _logisticsType = value;
            }
        }

        public decimal? Logistics_Fee
        {
            get { return _logisticsFee; }
            set { _logisticsFee = value; }
        }

        public string Logistics_Payment
        {
            get { return _logisticsPayment; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for LogisticsPayment", value, value.ToString());
                _logisticsPayment = value;
            }
        }

        public string Receive_Name
        {
            get { return _receiveName; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ReceiveName", value, value.ToString());
                _receiveName = value;
            }
        }

        public string Receive_Address
        {
            get { return _receiveAddress; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ReceiveAddress", value, value.ToString());
                _receiveAddress = value;
            }
        }

        public string Receive_Zip
        {
            get { return _receiveZip; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ReceiveZip", value, value.ToString());
                _receiveZip = value;
            }
        }

        public string Receive_Phone
        {
            get { return _receivePhone; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ReceivePhone", value, value.ToString());
                _receivePhone = value;
            }
        }

        public string Receive_Mobile
        {
            get { return _receiveMobile; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ReceiveMobile", value, value.ToString());
                _receiveMobile = value;
            }
        }

		#endregion
	}


	#endregion
}
