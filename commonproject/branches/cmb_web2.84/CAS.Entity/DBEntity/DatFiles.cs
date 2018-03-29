using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Files")]
    public class DatFiles : BaseTO
    {
        private long _id;
        /// <summary>
        /// 附件ID
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private long _objectid;
        /// <summary>
        /// 对象ID（询价、查勘、委估对象、预评、报告）
        /// </summary>
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private int _objecttypecode;
        /// <summary>
        /// 业务节点对象类型
        /// </summary>
        public int objecttypecode
        {
            get { return _objecttypecode; }
            set { _objecttypecode = value; }
        }
        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _path;
        /// <summary>
        /// 文件路径全称（包含文件名称）
        /// </summary>
        public string path
        {
            get { return _path; }
            set { _path = value; }
        }
        private DateTime _uptime = DateTime.Now;
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime uptime
        {
            get { return _uptime; }
            set { _uptime = value; }
        }
        private string _smallimgpath;
        /// <summary>
        /// 文件缩略图路径全称（包含文件名称）
        /// </summary>
        public string smallimgpath
        {
            get { return _smallimgpath; }
            set { _smallimgpath = value; }
        }
        private int _annextypecode;
        /// <summary>
        /// 附件大类
        /// </summary>
        public int annextypecode
        {
            get { return _annextypecode; }
            set { _annextypecode = value; }
        }
        private int _annextypesubcode;
        /// <summary>
        /// 附件小类
        /// </summary>
        public int annextypesubcode
        {
            get { return _annextypesubcode; }
            set { _annextypesubcode = value; }
        }
        private string _imagetype;
        /// <summary>
        /// 照片类型（可以自定义，存文本）
        /// </summary>
        public string imagetype
        {
            get { return _imagetype; }
            set { _imagetype = value; }
        }
        private int? _filesize;
        /// <summary>
        /// 文件大小
        /// </summary>
        public int? filesize
        {
            get { return _filesize; }
            set { _filesize = value; }
        }
        private int _flietypecode;
        /// <summary>
        /// 文件类型
        /// </summary>
        public int flietypecode
        {
            get { return _flietypecode; }
            set { _flietypecode = value; }
        }
        private int? _filesubtypecode;
        /// <summary>
        /// 文件子类型
        /// </summary>
        public int? filesubtypecode
        {
            get { return _filesubtypecode; }
            set { _filesubtypecode = value; }
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
        private int? _createuserid;
        /// <summary>
        /// 创建者
        /// </summary>
        public int? createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// 是否有效
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _remark = "";

        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private string _newfileflag;
        /// <summary>
        /// 用于新增文件的标识。例如新增询价附件时标识是当前询价的附件，在询价信息天骄到数据库后再更新objectid
        /// </summary>
        public string newfileflag
        {
            get { return _newfileflag; }
            set { _newfileflag = value; }
        }
        private string _guid;
        [SQLReadOnly]
        public string guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
        private long? _entrustid;
        /// <summary>
        /// 业务编号
        /// </summary>
        public long? entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private DateTime? _filecreatedate;
        /// <summary>
        /// 文件(照片)生成时间(用于云查勘3.0照片打水印)
        /// </summary>
        public DateTime? filecreatedate
        {
            get { return _filecreatedate; }
            set { _filecreatedate = value; }
        }
        private bool? _stamped;
        /// <summary>
        /// 该附件是否已经在OA中进行附件盖章处理
        /// </summary>
        public bool? stamped
        {
            get
            {
                return _stamped;
            }
            set
            {
                _stamped = value;
            }
        }
        private string _truename=string.Empty;
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string truename
        {
            get { return _truename; }
            set { _truename = value; }
        }
        private string _source=string.Empty;
        /// <summary>
        /// 跟进来源
        /// </summary>
        public string source
        {
            get { return _source; }
            set { _source = value; }
        }

        private string _queryid;
        /// <summary>
        /// 询价编号
        /// </summary>
        public string queryid
        {
            get { return _queryid; }
            set { _queryid = value; }
        }

        private string _htmlfilepath;
        /// <summary>
        /// 相应的html文件路径
        /// </summary>
        public string htmlfilepath
        {
            get { return _htmlfilepath; }
            set { _htmlfilepath = value; }
        }
    }
}