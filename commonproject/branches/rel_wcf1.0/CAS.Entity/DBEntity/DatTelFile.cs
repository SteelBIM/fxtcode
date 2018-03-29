using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_TelFile")]
	public class DatTelFile : BaseTO
	{
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _titlestr;
        /// <summary>
        /// 文件标题
        /// </summary>
        public string titlestr
        {
            get { return _titlestr; }
            set { _titlestr = value; }
        }
        private string _fromuser;
        /// <summary>
        /// 发送人
        /// </summary>
        public string fromuser
        {
            get { return _fromuser; }
            set { _fromuser = value; }
        }
        private int? _fromuserid;
        /// <summary>
        /// 发送用户ID
        /// </summary>
        public int? fromuserid
        {
            get { return _fromuserid; }
            set { _fromuserid = value; }
        }
        private DateTime _timestr = DateTime.Now;
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime timestr
        {
            get { return _timestr; }
            set { _timestr = value; }
        }
        private string _contentstr;
        /// <summary>
        /// 详细说明
        /// </summary>
        public string contentstr
        {
            get { return _contentstr; }
            set { _contentstr = value; }
        }
        private string _fujianstr;
        /// <summary>
        /// 附件文件
        /// </summary>
        public string fujianstr
        {
            get { return _fujianstr; }
            set { _fujianstr = value; }
        }

	}
}