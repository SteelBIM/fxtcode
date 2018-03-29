using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_SendNeedMessageRecord")]
    public class DatSendNeedMessageRecord : BaseTO
    {
        private int _id;
        /// <summary>
        /// ID
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _type;
        /// <summary>
        /// 类型 1待办2消息3通知4离线
        /// </summary>
        public int type
        {
            get { return _type; }
            set { _type = value; }
        }
        private int _businesstype;
        /// <summary>
        /// 待办类别 7 待归档 12 盖章 8文件传阅
        /// </summary>
        public int businesstype
        {
            get { return _businesstype; }
            set { _businesstype = value; }
        }
        private long _businessid;
        /// <summary>
        /// 业务ID
        /// </summary>
        public long businessid
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
        private string _stepname;
        /// <summary>
        /// 待办类别名称
        /// </summary>
        public string stepname
        {
            get { return _stepname; }
            set { _stepname = value; }
        }
        private string _title;
        /// <summary>
        /// 标题
        /// </summary>
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _content;
        /// <summary>
        /// 内容
        /// </summary>
        public string content
        {
            get { return _content; }
            set { _content = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 公司标识
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _fromuserid;
        /// <summary>
        /// 发送人
        /// </summary>
        public int fromuserid
        {
            get { return _fromuserid; }
            set { _fromuserid = value; }
        }
        private string _touserids;
        /// <summary>
        /// 接收人,多个接收人时用逗号分隔
        /// </summary>
        public string touserids
        {
            get { return _touserids; }
            set { _touserids = value; }
        }
        private DateTime _createdon;
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }
        private int _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private bool? _valid;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        private int _objecttypecode;
        /// <summary>
        /// 业务类别  2018006 报告， 2018005 预评
        /// </summary>
        public int objecttypecode
        {
            get { return _objecttypecode; }
            set { _objecttypecode = value; }
        }
    }

}
