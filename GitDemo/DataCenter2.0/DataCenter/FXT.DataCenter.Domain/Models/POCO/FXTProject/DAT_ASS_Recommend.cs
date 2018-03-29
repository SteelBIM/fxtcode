using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_ASS_Recommend
    {
        private int _rid;
        /// <summary>
        /// ID
        /// </summary>
        //[SQLField("rid", EnumDBFieldUsage.PrimaryKey, true)]
        public int rid
        {
            get { return _rid; }
            set { _rid = value; }
        }
        private byte _robjecttype;
        /// <summary>
        /// 推荐对象类型 1机构/2个人
        /// </summary>
        public byte robjecttype
        {
            get { return _robjecttype; }
            set { _robjecttype = value; }
        }
        private int _rcompanyid;
        /// <summary>
        /// 推荐对象机构ID
        /// </summary>
        public int rcompanyid
        {
            get { return _rcompanyid; }
            set { _rcompanyid = value; }
        }
        private string _ruserid;
        /// <summary>
        /// 推荐对象个人ID
        /// </summary>
        public string ruserid
        {
            get { return _ruserid; }
            set { _ruserid = value; }
        }
        private byte _rsourcetype;
        /// <summary>
        /// 推荐来源类型 1协会/2评估机构/3运维中心
        /// </summary>
        public byte rsourcetype
        {
            get { return _rsourcetype; }
            set { _rsourcetype = value; }
        }
        private int _rsourceid;
        /// <summary>
        /// 推荐来源公司ID（协会/评估机构/运维中心）
        /// </summary>
        public int rsourceid
        {
            get { return _rsourceid; }
            set { _rsourceid = value; }
        }
        private DateTime _rsourcedate = DateTime.Now;
        /// <summary>
        /// 推荐时间（来源推荐）
        /// </summary>
        public DateTime rsourcedate
        {
            get { return _rsourcedate; }
            set { _rsourcedate = value; }
        }
        private byte _rstatus = ((1));
        /// <summary>
        /// 推荐状态 1待推荐/2已推荐/3未推荐
        /// </summary>
        public byte rstatus
        {
            get { return _rstatus; }
            set { _rstatus = value; }
        }
        private string _audituserid;
        /// <summary>
        /// 平台审核推荐人
        /// </summary>
        public string audituserid
        {
            get { return _audituserid; }
            set { _audituserid = value; }
        }
        private DateTime? _auditdate;
        /// <summary>
        /// 平台审核推荐时间
        /// </summary>
        public DateTime? auditdate
        {
            get { return _auditdate; }
            set { _auditdate = value; }
        }

    }
}
