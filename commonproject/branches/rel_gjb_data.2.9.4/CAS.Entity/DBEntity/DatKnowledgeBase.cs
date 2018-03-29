using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_KnowledgeBase")]
    public class DatKnowledgeBase : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _subject;
        /// <summary>
        /// 主题
        /// </summary>
        public string subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        private long _createuserid;
        /// <summary>
        /// 创建者
        /// </summary>
        public long createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime _lastupdatedate;
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime lastupdatedate
        {
            get { return _lastupdatedate; }
            set { _lastupdatedate = value; }
        }
        private long _lastupdateuserid;
        /// <summary>
        /// 最后更新者
        /// </summary>
        public long lastupdateuserid
        {
            get { return _lastupdateuserid; }
            set { _lastupdateuserid = value; }
        }
        /// <summary>
        /// 是否有用
        /// </summary>
        private bool _valid = true;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime createdate { get; set; }
        /// <summary>
        /// 主题内容
        /// </summary>
        public string content { get; set; }
    }
}