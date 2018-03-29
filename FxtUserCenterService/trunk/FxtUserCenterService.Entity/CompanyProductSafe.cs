using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace FxtUserCenterService.Entity
{
    [Serializable]
    [TableAttribute("dbo.CompanyProduct_Safe")]
    public class CompanyProductSafe : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cpid;
        public int cpid
        {
            get { return _cpid; }
            set { _cpid = value; }
        }
        private string _weburl;
        /// <summary>
        /// Web应用的主域
        /// </summary>
        public string weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        private string _sn;
        /// <summary>
        /// 安全码：用于android与IOS应用
        /// </summary>
        public string sn
        {
            get { return _sn; }
            set { _sn = value; }
        }
        private long _appkey;
        /// <summary>
        /// 加密key
        /// </summary>
        public long appkey
        {
            get { return _appkey; }
            set { _appkey = value; }
        }
        private bool _valid;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private DateTime? _startdate;
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? startdate
        {
            get { return _startdate; }
            set { _startdate = value; }
        }
        private DateTime? _enddate;
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? enddate
        {
            get { return _enddate; }
            set { _enddate = value; }
        }

        #region 扩展属性
        /// <summary>
        /// 公司id
        /// </summary>
        [SQLReadOnly]
        public int companyid { get; set; }
        /// <summary>
        /// 公司名
        /// </summary>
        [SQLReadOnly]
        public string companyname { get; set; }
        #endregion
    }

}
