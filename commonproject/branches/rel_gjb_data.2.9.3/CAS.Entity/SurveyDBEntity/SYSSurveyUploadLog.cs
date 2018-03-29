using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyDBEntity
{
    /// <summary>
    /// 查勘库上传跟进表 caoq 2013-11-11
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.SYS_SurveyUpload_Log")]
    public class SYSSurveyUploadLog : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private DateTime _starttime;
        /// <summary>
        /// 上传开始时间
        /// </summary>
        public DateTime starttime
        {
            get { return _starttime; }
            set { _starttime = value; }
        }
        private DateTime? _endtime;
        /// <summary>
        /// 上传结束时间（为空可能是断网）
        /// </summary>
        public DateTime? endtime
        {
            get { return _endtime; }
            set { _endtime = value; }
        }
        private string _nettype;
        /// <summary>
        /// 网络类型
        /// </summary>
        public string nettype
        {
            get { return _nettype; }
            set { _nettype = value; }
        }
        private int _imagecount;
        /// <summary>
        /// 文件名
        /// </summary>
        public int imagecount
        {
            get { return _imagecount; }
            set { _imagecount = value; }
        }
        private int _imagesize;
        /// <summary>
        /// 文件大小
        /// </summary>
        public int imagesize
        {
            get { return _imagesize; }
            set { _imagesize = value; }
        }
        private int _videocount;
        /// <summary>
        /// 文件类型6051
        /// </summary>
        public int videocount
        {
            get { return _videocount; }
            set { _videocount = value; }
        }
        private int _videosize;
        public int videosize
        {
            get { return _videosize; }
            set { _videosize = value; }
        }
        private long _surveyid;
        /// <summary>
        /// 关联对象ID
        /// </summary>
        public long surveyid
        {
            get { return _surveyid; }
            set { _surveyid = value; }
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
        private string _userid;
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _subcompanyid = 0;
        /// <summary>
        /// 分支机构ID
        /// </summary>
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }

    }
}