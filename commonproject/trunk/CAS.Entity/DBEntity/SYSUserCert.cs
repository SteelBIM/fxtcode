using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_UserCert")]
    public class SYSUserCert : BaseTO
    {
        private int _userid;
        [SQLField("userid", EnumDBFieldUsage.PrimaryKey)]
        public int userid
        {
            get { return _userid; }
            set { _userid = value; }
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
        private DateTime? _fdccertvaliddate;
        /// <summary>
        /// 房地产估价师有效时间
        /// </summary>
        public DateTime? fdccertvaliddate
        {
            get { return _fdccertvaliddate; }
            set { _fdccertvaliddate = value; }
        }
        private string _fdccertfilepath;
        /// <summary>
        /// 房地产估价师证附件
        /// </summary>
        public string fdccertfilepath
        {
            get { return _fdccertfilepath; }
            set { _fdccertfilepath = value; }
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
        private DateTime? _tdcertvaliddate;
        /// <summary>
        /// 土地产估价师有效时间
        /// </summary>
        public DateTime? tdcertvaliddate
        {
            get { return _tdcertvaliddate; }
            set { _tdcertvaliddate = value; }
        }
        private string _tdcertfilepath;
        /// <summary>
        /// 土地估价师证附件
        /// </summary>
        public string tdcertfilepath
        {
            get { return _tdcertfilepath; }
            set { _tdcertfilepath = value; }
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
        private DateTime? _zccertvaliddate;
        /// <summary>
        /// 资产估价师有效时间
        /// </summary>
        public DateTime? zccertvaliddate
        {
            get { return _zccertvaliddate; }
            set { _zccertvaliddate = value; }
        }
        private string _zccertfilepath;
        /// <summary>
        /// 资产估价师证附件
        /// </summary>
        public string zccertfilepath
        {
            get { return _zccertfilepath; }
            set { _zccertfilepath = value; }
        }
        private int? _fdcisremind=0;
        public int? fdcisremind
        {
            get { return _fdcisremind; }
            set { _fdcisremind = value; }
        }
        private int? _tdisremind=0;
        public int? tdisremind
        {
            get { return _tdisremind; }
            set { _tdisremind = value; }
        }
        private int? _zcisremind=0;
        public int? zcisremind
        {
            get { return _zcisremind; }
            set { _zcisremind = value; }
        }
        private int? _fdcisremindfromsystem=0;
        /// <summary>
        /// 系统提醒-房地产
        /// </summary>
        public int? fdcisremindfromsystem
        {
            get { return _fdcisremindfromsystem; }
            set { _fdcisremindfromsystem = value; }
        }
        private int? _tdisremindfromsystem=0;
        /// <summary>
        /// 系统提醒-土地
        /// </summary>
        public int? tdisremindfromsystem
        {
            get { return _tdisremindfromsystem; }
            set { _tdisremindfromsystem = value; }
        }
        private int? _zcisremindfromsystem=0;
        /// <summary>
        /// 系统提醒-资产
        /// </summary>
        public int? zcisremindfromsystem
        {
            get { return _zcisremindfromsystem; }
            set { _zcisremindfromsystem = value; }
        }
        private int? _fdcisremindtomanager = 0;
        /// <summary>
        /// 系统提醒-房地产  给证书管理员发送
        /// </summary>
        public int? fdcisremindtomanager
        {
            get { return _fdcisremindtomanager; }
            set { _fdcisremindtomanager = value; }
        }
        private int? _tdisremindtomanager = 0;
        /// <summary>
        /// 系统提醒-土地 给证书管理员发送
        /// </summary>
        public int? tdisremindtomanager
        {
            get { return _tdisremindtomanager; }
            set { _tdisremindtomanager = value; }
        }
        private int? _zcisremindtomanager = 0;
        /// <summary>
        /// 系统提醒-资产 给证书管理员发送
        /// </summary>
        public int? zcisremindtomanager
        {
            get { return _zcisremindtomanager; }
            set { _zcisremindtomanager = value; }
        }
        private string _fdworkyear;
        /// <summary>
        /// 房地产从业年限
        /// </summary>
        public string fdworkyear
        {
            get { return _fdworkyear; }
            set { _fdworkyear = value; }
        }
        private int? _fdregisterstate;
        /// <summary>
        /// 房地产注册状态
        /// </summary>
        public int? fdregisterstate
        {
            get { return _fdregisterstate; }
            set { _fdregisterstate = value; }
        }
        private string _fdnationalclasshour;
        /// <summary>
        /// 房地产业继续教育课时 全国课时
        /// </summary>
        public string fdnationalclasshour
        {
            get { return _fdnationalclasshour; }
            set { _fdnationalclasshour = value; }
        }
        private string _fdlocalclasshour;
        /// <summary>
        /// 房地产业继续教育课时 地方课时
        /// </summary>
        public string fdlocalclasshour
        {
            get { return _fdlocalclasshour; }
            set { _fdlocalclasshour = value; }
        }
        private string _tdworkyear;
        /// <summary>
        /// 土地从业年限
        /// </summary>
        public string tdworkyear
        {
            get { return _tdworkyear; }
            set { _tdworkyear = value; }
        }
        private int? _tdregisterstate;
        /// <summary>
        /// 土地注册状态
        /// </summary>
        public int? tdregisterstate
        {
            get { return _tdregisterstate; }
            set { _tdregisterstate = value; }
        }
        private string _tdnationalclasshour;
        /// <summary>
        /// 土地继续教育课时 全国课时
        /// </summary>
        public string tdnationalclasshour
        {
            get { return _tdnationalclasshour; }
            set { _tdnationalclasshour = value; }
        }
        private string _tdlocalclasshour;
        /// <summary>
        /// 土地继续教育课时 地方课时
        /// </summary>
        public string tdlocalclasshour
        {
            get { return _tdlocalclasshour; }
            set { _tdlocalclasshour = value; }
        }
    }
}