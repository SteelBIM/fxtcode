using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.LNK_P_Photo")]
    public class LNKPPhoto : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _projectid;
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private int? _phototypecode;
        public int? phototypecode
        {
            get { return _phototypecode; }
            set { _phototypecode = value; }
        }
        private string _path;
        public string path
        {
            get { return _path; }
            set { _path = value; }
        }
        private DateTime _photodate = DateTime.Now;
        public DateTime photodate
        {
            get { return _photodate; }
            set { _photodate = value; }
        }
        private string _photoname;
        public string photoname
        {
            get { return _photoname; }
            set { _photoname = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int _fxtcompanyid = 25;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }

        private long _buildingid = 0;
        public long buildingid
        {
            get { return _buildingid; }
            set { _buildingid = value; }
        }

        private decimal? _x;
        public decimal? x
        {
            get { return _x; }
            set { _x = value; }
        }

        private decimal? _y;
        public decimal? y
        {
            get { return _y; }
            set { _y = value; }
        }
        [SQLReadOnly]
        public string phototypename { get; set; }
    }
}