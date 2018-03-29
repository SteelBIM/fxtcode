using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyDBEntity
{
    /// <summary>
    /// 查勘库版本升级表 caoq 2013-11-11
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.SYS_Upgrade")]
    public class SYSUpgrade : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private DateTime _updatedate;
        public DateTime updatedate
        {
            get { return _updatedate; }
            set { _updatedate = value; }
        }
        private string _path;
        public string path
        {
            get { return _path; }
            set { _path = value; }
        }
        private decimal _num;
        public decimal num
        {
            get { return _num; }
            set { _num = value; }
        }
        private int _typecode;
        /// <summary>
        /// 软件类型：个人版，机构版
        /// </summary>
        public int typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private string _detail;
        public string detail
        {
            get { return _detail; }
            set { _detail = value; }
        }
        private int _filesize = 0;
        public int filesize
        {
            get { return _filesize; }
            set { _filesize = value; }
        }
    }
}