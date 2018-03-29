using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_FeedBack")]
    public class DatFeedBack : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _wxopenid;
        /// <summary>
        /// 微信OPENID
        /// </summary>
        public string wxopenid
        {
            get { return _wxopenid; }
            set { _wxopenid = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _details;
        /// <summary>
        /// 反馈内容
        /// </summary>
        public string details
        {
            get { return _details; }
            set { _details = value; }
        }

        private int _rank;
        /// <summary>
        /// 反馈内容等级:1 重要，2一般
        /// </summary>
        public int rank
        {
            get { return _rank; }
            set { _rank = value; }
        } 
    }
}