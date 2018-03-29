using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_RedHead")]
	public class DatRedHead : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _filename;
		/// <summary>
		/// 文件名称
		/// </summary>
		public string filename
		{
			get{ return _filename;}
			set{ _filename=value;}
		}
		private string _filepath;
		/// <summary>
		/// 文件路径
		/// </summary>
		public string filepath
		{
			get{ return _filepath;}
			set{ _filepath=value;}
		}
		private string _username;
		/// <summary>
		/// 上传人
		/// </summary>
		public string username
		{
			get{ return _username;}
			set{ _username=value;}
		}
		private DateTime _timestr = DateTime.Now;
		/// <summary>
		/// 上传时间
		/// </summary>
		public DateTime timestr
		{
			get{ return _timestr;}
			set{ _timestr=value;}
		}

        private string _path;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string path
        {
            get { return _path; }
            set { _path = value; }
        }

	}
}