using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{

    [Serializable]
    [TableAttribute("dbo.DAT_Case")]
    public class DATCase : BaseTO
    {
        private int _caseid;
        /// <summary>
        /// ID
        /// </summary>
        [SQLField("caseid", EnumDBFieldUsage.PrimaryKey, true)]
        public int caseid
        {
            get { return _caseid; }
            set { _caseid = value; }
        }
        private int _projectid;
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private int? _buildingid;
        /// <summary>
        /// 楼宇ID
        /// </summary>
        public int? buildingid
        {
            get { return _buildingid; }
            set { _buildingid = value; }
        }
        private int? _houseid;
        /// <summary>
        /// 物业ID
        /// </summary>
        public int? houseid
        {
            get { return _houseid; }
            set { _houseid = value; }
        }
        private int? _companyid;
        /// <summary>
        /// 公司ID
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private DateTime _casedate;
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime casedate
        {
            get { return _casedate; }
            set { _casedate = value; }
        }
        private int _purposecode = 1002001;
        /// <summary>
        /// 用途Code
        /// </summary>
        public int purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }
        /// <summary>
        /// 用途
        /// </summary>
        [SQLReadOnly]
        public string purpose { get; set; }
        private int? _floornumber;
        /// <summary>
        /// 楼层
        /// </summary>
        public int? floornumber
        {
            get { return _floornumber; }
            set { _floornumber = value; }
        }
        private string _buildingname;
        public string buildingname
        {
            get { return _buildingname; }
            set { _buildingname = value; }
        }
        private string _houseno;
        /// <summary>
        /// 房号
        /// </summary>
        public string houseno
        {
            get { return _houseno; }
            set { _houseno = value; }
        }
        private decimal? _buildingarea;
        /// <summary>
        /// 建筑面积
        /// </summary>
        public decimal? buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private decimal? _usablearea;
        /// <summary>
        /// 使用面积
        /// </summary>
        public decimal? usablearea
        {
            get { return _usablearea; }
            set { _usablearea = value; }
        }
        private int? _frontcode;
        /// <summary>
        /// 朝向Code
        /// </summary>
        public int? frontcode
        {
            get { return _frontcode; }
            set { _frontcode = value; }
        }
        private decimal? _unitprice;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }
        private int _moneyunitcode = 2002001;
        /// <summary>
        /// 货币单位Code
        /// </summary>
        public int moneyunitcode
        {
            get { return _moneyunitcode; }
            set { _moneyunitcode = value; }
        }
        private int? _sightcode;
        /// <summary>
        /// 景观Code
        /// </summary>
        public int? sightcode
        {
            get { return _sightcode; }
            set { _sightcode = value; }
        }
        private int _casetypecode = 3001001;
        /// <summary>
        /// 案例类型
        /// </summary>
        public int casetypecode
        {
            get { return _casetypecode; }
            set { _casetypecode = value; }
        }
        private int? _structurecode;
        /// <summary>
        /// 建筑结构Code
        /// </summary>
        public int? structurecode
        {
            get { return _structurecode; }
            set { _structurecode = value; }
        }
        private int? _buildingtypecode;
        /// <summary>
        /// 建筑类型Code
        /// </summary>
        public int? buildingtypecode
        {
            get { return _buildingtypecode; }
            set { _buildingtypecode = value; }
        }
        private int? _housetypecode;
        /// <summary>
        /// 户型Code
        /// </summary>
        public int? housetypecode
        {
            get { return _housetypecode; }
            set { _housetypecode = value; }
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
        private string _creator;
        /// <summary>
        /// 创建人
        /// </summary>
        public string creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private decimal? _totalprice;
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? totalprice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }
        private int? _oldid;
        public int? oldid
        {
            get { return _oldid; }
            set { _oldid = value; }
        }
        private int? _cityid;
        public int? cityid
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
        /// <summary>
        /// 案例所有者
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int? _totalfloor;
        /// <summary>
        /// 总层数
        /// </summary>
        public int? totalfloor
        {
            get { return _totalfloor; }
            set { _totalfloor = value; }
        }
        private int? _remainyear;
        /// <summary>
        /// 剩余年限
        /// </summary>
        public int? remainyear
        {
            get { return _remainyear; }
            set { _remainyear = value; }
        }
        private decimal? _depreciation;
        /// <summary>
        /// 成新率
        /// </summary>
        public decimal? depreciation
        {
            get { return _depreciation; }
            set { _depreciation = value; }
        }
        private int? _fitmentcode;
        /// <summary>
        /// 装修情况
        /// </summary>
        public int? fitmentcode
        {
            get { return _fitmentcode; }
            set { _fitmentcode = value; }
        }
        private int _surveyid = 0;
        /// <summary>
        /// 查勘ID
        /// </summary>
        public int surveyid
        {
            get { return _surveyid; }
            set { _surveyid = value; }
        }
        private DateTime? _savedatetime;
        public DateTime? savedatetime
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
        private string _sourcename;
        /// <summary>
        /// 来源
        /// </summary>
        public string sourcename
        {
            get { return _sourcename; }
            set { _sourcename = value; }
        }
        private string _sourcelink;
        /// <summary>
        /// 来源链接
        /// </summary>
        public string sourcelink
        {
            get { return _sourcelink; }
            set { _sourcelink = value; }
        }
        private string _sourcephone;
        /// <summary>
        /// 来源电话
        /// </summary>
        public string sourcephone
        {
            get { return _sourcephone; }
            set { _sourcephone = value; }
        }



        private int _areaid = 0;
        /// <summary>
        /// 行政区
        /// </summary>
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private string _areaname;
        /// <summary>
        /// 行政区名称
        /// </summary>
        public string areaname
        {
            get { return _areaname; }
            set { _areaname = value; }
        }
        private string _buildingdate;
        /// <summary>
        /// 建筑年代
        /// </summary>
        public string buildingdate
        {
            get { return _buildingdate; }
            set { _buildingdate = value; }
        }
        private string _zhuangxiu;
        /// <summary>
        /// 装修
        /// </summary>
        public string zhuangxiu
        {
            get { return _zhuangxiu; }
            set { _zhuangxiu = value; }
        }
        private string _subhouse;
        /// <summary>
        /// 附属物业
        /// </summary>
        public string subhouse
        {
            get { return _subhouse; }
            set { _subhouse = value; }
        }
        private string _peitao;
        /// <summary>
        /// 配套设施
        /// </summary>
        public string peitao
        {
            get { return _peitao; }
            set { _peitao = value; }
        }

    }


}
