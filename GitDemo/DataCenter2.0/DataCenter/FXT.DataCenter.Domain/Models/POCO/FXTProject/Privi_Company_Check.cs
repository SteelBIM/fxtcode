using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Company_Check
    {
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        //[SQLField("fxtcompanyid", EnumDBFieldUsage.PrimaryKey)]
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _checktypecode = 5012002;
        /// <summary>
        /// 审批类型
        /// </summary>
        //[SQLField("checktypecode", EnumDBFieldUsage.PrimaryKey)]
        public int checktypecode
        {
            get { return _checktypecode; }
            set { _checktypecode = value; }
        }
        private int _cityid;
        //[SQLField("cityid", EnumDBFieldUsage.PrimaryKey)]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _checkcalssuser1;
        /// <summary>
        /// 一级审批人(组)
        /// </summary>
        public string checkcalssuser1
        {
            get { return _checkcalssuser1; }
            set { _checkcalssuser1 = value; }
        }
        private string _checkcalssuser2;
        public string checkcalssuser2
        {
            get { return _checkcalssuser2; }
            set { _checkcalssuser2 = value; }
        }
        private string _checkcalssuser3;
        public string checkcalssuser3
        {
            get { return _checkcalssuser3; }
            set { _checkcalssuser3 = value; }
        }
        private int _checkcalsstype1 = 5013001;
        /// <summary>
        /// 一级审批类型(人,组)
        /// </summary>
        public int checkcalsstype1
        {
            get { return _checkcalsstype1; }
            set { _checkcalsstype1 = value; }
        }
        private int _checkcalsstype2 = 0;
        public int checkcalsstype2
        {
            get { return _checkcalsstype2; }
            set { _checkcalsstype2 = value; }
        }
        private int _checkcalsstype3 = 0;
        public int checkcalsstype3
        {
            get { return _checkcalsstype3; }
            set { _checkcalsstype3 = value; }
        }
        private int _ischeck = 1;
        /// <summary>
        /// 是否需要审核
        /// </summary>
        public int ischeck
        {
            get { return _ischeck; }
            set { _ischeck = value; }
        }
        private int _iscountersign = 0;
        /// <summary>
        /// 是否会签
        /// </summary>
        public int iscountersign
        {
            get { return _iscountersign; }
            set { _iscountersign = value; }
        }
        private int _outtimenumber1 = 30;
        /// <summary>
        /// 超时提示时间
        /// </summary>
        public int outtimenumber1
        {
            get { return _outtimenumber1; }
            set { _outtimenumber1 = value; }
        }
        private int _outtimenumber2 = 30;
        /// <summary>
        /// 超时提示时间
        /// </summary>
        public int outtimenumber2
        {
            get { return _outtimenumber2; }
            set { _outtimenumber2 = value; }
        }
        private int _outtimenumber3 = 30;
        /// <summary>
        /// 超时提示时间
        /// </summary>
        public int outtimenumber3
        {
            get { return _outtimenumber3; }
            set { _outtimenumber3 = value; }
        }
        private int _outtimetype1 = 5014001;
        /// <summary>
        /// 超时提示单位(分钟,小时,天)
        /// </summary>
        public int outtimetype1
        {
            get { return _outtimetype1; }
            set { _outtimetype1 = value; }
        }
        private int _outtimetype2 = 5014001;
        /// <summary>
        /// 超时提示单位(分钟,小时,天)
        /// </summary>
        public int outtimetype2
        {
            get { return _outtimetype2; }
            set { _outtimetype2 = value; }
        }
        private int _outtimetype3 = 5014001;
        /// <summary>
        /// 超时提示单位(分钟,小时,天)
        /// </summary>
        public int outtimetype3
        {
            get { return _outtimetype3; }
            set { _outtimetype3 = value; }
        }
        private int _issendmessage1 = 0;
        /// <summary>
        /// 是否发手机信息
        /// </summary>
        public int issendmessage1
        {
            get { return _issendmessage1; }
            set { _issendmessage1 = value; }
        }
        private int _issendmessage2 = 0;
        /// <summary>
        /// 是否发手机信息
        /// </summary>
        public int issendmessage2
        {
            get { return _issendmessage2; }
            set { _issendmessage2 = value; }
        }
        private int _issendmessage3 = 0;
        /// <summary>
        /// 是否发手机信息
        /// </summary>
        public int issendmessage3
        {
            get { return _issendmessage3; }
            set { _issendmessage3 = value; }
        }
        private decimal _queryprice1 = 0M;
        /// <summary>
        /// 审批生效条件：评估总额大于此值时生效
        /// </summary>
        public decimal queryprice1
        {
            get { return _queryprice1; }
            set { _queryprice1 = value; }
        }
        private decimal _queryprice2 = 0M;
        /// <summary>
        /// 审批生效条件：评估总额大于此值时生效
        /// </summary>
        public decimal queryprice2
        {
            get { return _queryprice2; }
            set { _queryprice2 = value; }
        }
        private decimal _queryprice3 = 0M;
        /// <summary>
        /// 审批生效条件：评估总额大于此值时生效
        /// </summary>
        public decimal queryprice3
        {
            get { return _queryprice3; }
            set { _queryprice3 = value; }
        }
        private decimal _adjustpercent1 = 0M;
        /// <summary>
        /// 审批生效条件：评估价格调整幅度大于此百分比值时生效
        /// </summary>
        public decimal adjustpercent1
        {
            get { return _adjustpercent1; }
            set { _adjustpercent1 = value; }
        }
        private decimal _adjustpercent2 = 0M;
        public decimal adjustpercent2
        {
            get { return _adjustpercent2; }
            set { _adjustpercent2 = value; }
        }
        private decimal _adjustpercent3 = 0M;
        public decimal adjustpercent3
        {
            get { return _adjustpercent3; }
            set { _adjustpercent3 = value; }
        }

    }
}
