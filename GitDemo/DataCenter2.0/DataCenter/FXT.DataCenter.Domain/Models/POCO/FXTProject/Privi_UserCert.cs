using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_UserCert
    {
        private string _userid;
        //[SQLField("userid", EnumDBFieldUsage.PrimaryKey)]
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _fdccertno;
        /// <summary>
        /// 房地产估价师证书号码
        /// </summary>
        public string fdccertno
        {
            get { return _fdccertno; }
            set { _fdccertno = value; }
        }
        private string _fdccertregno;
        /// <summary>
        /// 房地产估价师注册号码
        /// </summary>
        public string fdccertregno
        {
            get { return _fdccertregno; }
            set { _fdccertregno = value; }
        }
        private DateTime? _fdccertregdate;
        /// <summary>
        /// 房地产估价师注册时间
        /// </summary>
        public DateTime? fdccertregdate
        {
            get { return _fdccertregdate; }
            set { _fdccertregdate = value; }
        }
        private int _fdccertvalid = 1;
        /// <summary>
        /// 房地产估价师证状态
        /// </summary>
        public int fdccertvalid
        {
            get { return _fdccertvalid; }
            set { _fdccertvalid = value; }
        }
        private string _tdcertno;
        /// <summary>
        /// 土地估价师证书号码
        /// </summary>
        public string tdcertno
        {
            get { return _tdcertno; }
            set { _tdcertno = value; }
        }
        private string _tdcertregno;
        /// <summary>
        /// 土地估价师注册号码
        /// </summary>
        public string tdcertregno
        {
            get { return _tdcertregno; }
            set { _tdcertregno = value; }
        }
        private DateTime? _tdcertregdate;
        /// <summary>
        /// 土地产估价师注册时间
        /// </summary>
        public DateTime? tdcertregdate
        {
            get { return _tdcertregdate; }
            set { _tdcertregdate = value; }
        }
        private int _tdcertvalid = 1;
        /// <summary>
        /// 土地估价师证状态
        /// </summary>
        public int tdcertvalid
        {
            get { return _tdcertvalid; }
            set { _tdcertvalid = value; }
        }
        private string _zccertno;
        /// <summary>
        /// 资产价师证书号码
        /// </summary>
        public string zccertno
        {
            get { return _zccertno; }
            set { _zccertno = value; }
        }
        private string _zccertregno;
        /// <summary>
        /// 资产价师注册号码
        /// </summary>
        public string zccertregno
        {
            get { return _zccertregno; }
            set { _zccertregno = value; }
        }
        private DateTime? _zccertregdate;
        /// <summary>
        /// 资产估价师注册时间
        /// </summary>
        public DateTime? zccertregdate
        {
            get { return _zccertregdate; }
            set { _zccertregdate = value; }
        }
        private int _zccertvalid = 1;
        /// <summary>
        /// 资产估价师证状态
        /// </summary>
        public int zccertvalid
        {
            get { return _zccertvalid; }
            set { _zccertvalid = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _creator;
        public string creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        private DateTime _savedate = DateTime.Now;
        public DateTime savedate
        {
            get { return _savedate; }
            set { _savedate = value; }
        }
        private string _saveuser;
        public string saveuser
        {
            get { return _saveuser; }
            set { _saveuser = value; }
        }

    }
}
