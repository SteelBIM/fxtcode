using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_MobilePush")]
	public class DatMobilePush : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _username;
		/// <summary>
		/// 用Id
		/// </summary>
		public string username
		{
			get{ return _username;}
			set{ _username=value;}
		}
		private string _andphshuserid;
		/// <summary>
		/// 推送服务id,百度推送平台的推送id

		/// </summary>
		public string andphshuserid
		{
			get{ return _andphshuserid;}
			set{ _andphshuserid=value;}
		}
		private string _iosphshuserid;
		/// <summary>
		/// 推送服务id,苹果推送平台的推送id

		/// </summary>
		public string iosphshuserid
		{
			get{ return _iosphshuserid;}
			set{ _iosphshuserid=value;}
		}
		private string _channelid;
		/// <summary>
		///  推送服务渠道id只有android才有 iphone为0
		/// </summary>
		public string channelid
		{
			get{ return _channelid;}
			set{ _channelid=value;}
		}
		private int? _producttypecode;
		/// <summary>
		/// 产品类型
		/// </summary>
		public int? producttypecode
		{
			get{ return _producttypecode;}
			set{ _producttypecode=value;}
		}

        /// <summary>
        /// 设备类型
        /// </summary>
        [SQLReadOnly]
        public string splatype { get; set; }
        /// <summary>
        /// 设备Id
        /// </summary>
        [SQLReadOnly]
        public string phshuserid { get; set; }
	}
}