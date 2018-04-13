using System;
using System.Collections;


namespace Kingsun.SynchronousStudy.AliPay.Util
{
    #region Verify

    /// <summary>
    /// Verify object for NHibernate mapped table 'Verify'.
    /// </summary>
    public class Verify
    {
        #region Member Variables

       
        protected string _service;
        protected string _partner;
        protected string _notifyId;
        #endregion

        #region Constructors

        public Verify() { }
        /// <summary>
        /// 验证接口参数
        /// </summary>
        /// <param name="service">接口名称notify_verify</param>
        /// <param name="partner">合作伙伴在支付宝的用户ID</param>
        /// <param name="notifyId">支付宝发送的通知ID</param>
        public Verify(string service, string partner, string notifyId)
        {
            this._service = service;
            this._partner = partner;
            this._notifyId = notifyId;
        }

        #endregion

        #region Public Properties

     

        public string Service
        {
            get { return _service; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Service", value, value.ToString());
                _service = value;
            }
        }

        public string Partner
        {
            get { return _partner; }
            set
            {
                if (value != null && value.Length > 16)
                    throw new ArgumentOutOfRangeException("Invalid value for Partner", value, value.ToString());
                _partner = value;
            }
        }

        public string Notify_Id
        {
            get { return _notifyId; }
            set
            {
                if (value != null && value.Length > 10)
                    throw new ArgumentOutOfRangeException("Invalid value for NotifyId", value, value.ToString());
                _notifyId = value;
            }
        }

        #endregion
    }

    #endregion
}
