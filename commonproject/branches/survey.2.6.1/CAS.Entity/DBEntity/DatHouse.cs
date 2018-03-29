using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.DAT_House")]
    public class DATHouse : BaseTO
    {
        private int _houseid;
        [SQLField("houseid", EnumDBFieldUsage.PrimaryKey, true)]
        public int houseid
        {
            get { return _houseid; }
            set { _houseid = value; }
        }
        private int _buildingid;
        public int buildingid
        {
            get { return _buildingid; }
            set { _buildingid = value; }
        }
        private string _housename;
        public string housename
        {
            get { return _housename; }
            set { _housename = value; }
        }
        private int? _housetypecode;
        public int? housetypecode
        {
            get { return _housetypecode; }
            set { _housetypecode = value; }
        }
        private string _housetypecodename;
        public string housetypecodename
        {
            get { return _housetypecodename; }
            set { _housetypecodename = value; }
        }
        private int _floorno;
        public int floorno
        {
            get { return _floorno; }
            set { _floorno = value; }
        }
        private string _unitno;
        public string unitno
        {
            get { return _unitno; }
            set { _unitno = value; }
        }
        private decimal? _buildarea;
        public decimal? buildarea
        {
            get { return _buildarea; }
            set { _buildarea = value; }
        }
        private int? _frontcode;
        public int? frontcode
        {
            get { return _frontcode; }
            set { _frontcode = value; }
        }
        private int? _sightcode;
        public int? sightcode
        {
            get { return _sightcode; }
            set { _sightcode = value; }
        }
        private decimal? _unitprice;
        public decimal? unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }
        private decimal? _saleprice;
        public decimal? saleprice
        {
            get { return _saleprice; }
            set { _saleprice = value; }
        }
        private decimal? _weight;
        public decimal? weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        private string _photoname;
        public string photoname
        {
            get { return _photoname; }
            set { _photoname = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int? _structurecode;
        public int? structurecode
        {
            get { return _structurecode; }
            set { _structurecode = value; }
        }
        private decimal? _totalprice;
        public decimal? totalprice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }
        private int _purposecode = 1002001;
        public int purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }
        private string _purposecodename;
        public string purposecodename
        {
            get { return _purposecodename; }
            set { _purposecodename = value; }
        }
        private int _isevalue = 1;
        public int isevalue
        {
            get { return _isevalue; }
            set { _isevalue = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _oldid;
        public string oldid
        {
            get { return _oldid; }
            set { _oldid = value; }
        }
        private DateTime _createtime = DateTime.Now;
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private DateTime _savedatetime = DateTime.Now;
        public DateTime savedatetime
        {
            get { return _savedatetime; }
            set { _savedatetime = value; }
        }
        private string _saveuser;
        public string saveuser
        {
            get { return _saveuser; }
            set { _saveuser = value; }
        }
        private int _fxtcompanyid = 25;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _isshowbuildingarea = 1;
        public int isshowbuildingarea
        {
            get { return _isshowbuildingarea; }
            set { _isshowbuildingarea = value; }
        }
        private decimal? _innerbuildingarea;
        public decimal? innerbuildingarea
        {
            get { return _innerbuildingarea; }
            set { _innerbuildingarea = value; }
        }
        private DateTime? _builddate;
        public DateTime? builddate
        {
            get { return _builddate; }
            set { _builddate = value; }
        }
        [SQLReadOnly]
        public decimal projecaverageprice { get; set; }
        /// <summary>
        /// 用于转换：别名
        /// </summary>
        [SQLReadOnly]
        public string ob_othername { get; set; }
        /// <summary>
        /// 用于排序：开头数字
        /// </summary>
        [SQLReadOnly]
        public int ob_startnum { get; set; }
        /// <summary>
        /// 用于排序：开头字母
        /// </summary>
        [SQLReadOnly]
        public string ob_starletter { get; set; }
        /// <summary>
        /// 用于排序：文字
        /// </summary>
        [SQLReadOnly]
        public string ob_text { get; set; }
        /// <summary>
        /// 用于排序：数字
        /// </summary>
        [SQLReadOnly]
        public int ob_number { get; set; }
    }
}