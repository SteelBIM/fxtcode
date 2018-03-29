using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_Case
    {
        [DisplayName("*楼盘名称")]
        [Required(ErrorMessage = "楼盘不能为空")]
        public string ProjectName { get; set; }

        [DisplayName("行政区")]
        public string AreaName { get; set; }

        private string _buildingname;
        [DisplayName("楼栋名称")]
        public string buildingname
        {
            get
            {
                if (_buildingname == null)
                {
                    _buildingname = "";
                }
                return _buildingname;

            }
            set { _buildingname = value; }
        }

        [DisplayName("楼层")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public int? floornumber { get; set; }

        private string _houseno;
        [DisplayName("房号")]
        public string houseno
        {
            get
            {
                if (_houseno == null)
                {
                    _houseno = "";
                }

                return _houseno;
            }
            set { _houseno = value; }
        }

        [DisplayName("总楼层")]
        public int? totalfloor { get; set; }

        [DisplayName("*案例时间")]
        [Required(ErrorMessage = "案例时间不能为空")]
        public DateTime casedate { get; set; }

        [DisplayName("*用途")]
        public string PurposeName { get; set; }

        private decimal? _buildingarea;
        [DisplayName("*建筑面积")]
        [Required(ErrorMessage = "建筑面积不能为空")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }

        private decimal? _unitprice;
        [DisplayName("*单价")]
        [Required(ErrorMessage = "单价不能为空")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }

        private decimal? _totalprice;
        [DisplayName("*总价")]
        [Required(ErrorMessage = "总价不能为空")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? totalprice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }

        [DisplayName("*案例类型")]
        public string CaseTypeName { get; set; }

        [DisplayName("朝向")]
        public string FrontName { get; set; }

        [DisplayName("建筑类型")]
        public string BuildingTypeName { get; set; }

        [DisplayName("户型")]
        public string HouseTypeName { get; set; }

        [DisplayName("户型结构")]
        public string StructureName { get; set; }

        private string _buildingdate;
        [DisplayName("建筑年代")]
        public string buildingdate
        {
            get
            {
                if (_buildingdate == null)
                {
                    _buildingdate = "";
                }
                return _buildingdate;

            }
            set { _buildingdate = value; }
        }

        private string _zhuangxiu;
        [DisplayName("装修")]
        public string zhuangxiu
        {
            get
            {
                if (_zhuangxiu == null)
                {
                    _zhuangxiu = "";
                }
                return _zhuangxiu;
            }
            set { _zhuangxiu = value; }
        }

        private decimal? _usablearea;
        [DisplayName("使用面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? usablearea
        {
            get { return _usablearea; }
            set { _usablearea = value; }
        }

        private int? _remainyear;
        [DisplayName("剩余年限")]
        public int? remainyear
        {
            get { return _remainyear; }
            set { _remainyear = value; }
        }

        private decimal? _depreciation;

        [DisplayName("成新率")]
        public decimal? depreciation
        {
            get { return _depreciation; }
            set { _depreciation = value; }
        }

        [DisplayName("币种")]
        public string MoneyUnitCodeName { get; set; }

        private string _subhouse;
        [DisplayName("附属房屋")]
        public string subhouse
        {
            get
            {
                if (_subhouse == null)
                {
                    _subhouse = "";
                }
                return _subhouse;

            }
            set { _subhouse = value; }
        }

        private string _peitao;
        [DisplayName("配套")]
        public string peitao
        {
            get
            {
                if (_peitao == null)
                {
                    _peitao = "";
                }
                return _peitao;
            }
            set { _peitao = value; }
        }

        private string _sourcename;
        [DisplayName("来源")]
        public string sourcename
        {
            get
            {
                if (_sourcename == null)
                {
                    _sourcename = "";
                }
                return _sourcename;
            }
            set { _sourcename = value; }
        }

        private string _sourcelink;
        [DisplayName("链接")]
        public string sourcelink
        {
            get
            {
                if (_sourcelink == null)
                {
                    _sourcelink = "";
                }
                return _sourcelink;
            }
            set { _sourcelink = value; }
        }

        private string _sourcephone;
        [DisplayName("电话")]
        public string sourcephone
        {
            get
            {
                if (_sourcephone == null)
                {
                    _sourcephone = "";
                }
                return _sourcephone;

            }
            set { _sourcephone = value; }
        }

        private string _remark;
        [DisplayName("备注")]
        public string remark
        {
            get
            {
                if (_remark == null)
                {
                    _remark = "";
                }
                return _remark;
            }
            set { _remark = value; }
        }

        private string _creator;
        [DisplayName("录入人")]
        public string creator
        {
            get
            {
                if (_creator == null)
                {
                    _creator = "";
                }
                return _creator;
            }
            set { _creator = value; }
        }

        #region

        [ExcelExportIgnore]
        [DisplayName("案例ID")]
        public int? caseid { get; set; }

        private int _areaid = 0;
        [ExcelExportIgnore]
        [DisplayName("行政区")]
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }

        [ExcelExportIgnore]
        [Required(ErrorMessage = "楼盘不能为空")]
        public int projectid { get; set; }

        [ExcelExportIgnore]
        public int? buildingid { get; set; }

        [ExcelExportIgnore]
        public int? houseid { get; set; }

        [ExcelExportIgnore]
        [DisplayName("公司ID")]
        public int? companyid { get; set; }

        private int _purposecode = 1002001;
        [ExcelExportIgnore]
        [DisplayName("用途")]
        [Required(ErrorMessage = "居住用途不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择居住用途")]
        public int purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }

        private int? _frontcode;
        [ExcelExportIgnore]
        [DisplayName("朝向")]
        public int? frontcode
        {
            get { return _frontcode; }
            set { _frontcode = value; }
        }

        private int _moneyunitcode = 2002001;
        [ExcelExportIgnore]
        [DisplayName("货币单位")]
        public int moneyunitcode
        {
            get { return _moneyunitcode; }
            set { _moneyunitcode = value; }
        }

        private int? _sightcode;
        [ExcelExportIgnore]
        [DisplayName("景观")]
        public int? sightcode
        {
            get { return _sightcode; }
            set { _sightcode = value; }
        }

        private int _casetypecode = 3001001;
        [ExcelExportIgnore]
        [DisplayName("案例类型")]
        [Required(ErrorMessage = "案例类型不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择案例类型")]
        public int casetypecode
        {
            get { return _casetypecode; }
            set { _casetypecode = value; }
        }

        private int? _structurecode;
        [ExcelExportIgnore]
        [DisplayName("建筑结构")]
        public int? structurecode
        {
            get { return _structurecode; }
            set { _structurecode = value; }
        }

        private int? _buildingtypecode;
        [ExcelExportIgnore]
        [DisplayName("建筑类型")]
        public int? buildingtypecode
        {
            get { return _buildingtypecode; }
            set { _buildingtypecode = value; }
        }

        private int? _housetypecode;
        [ExcelExportIgnore]
        [DisplayName("户型")]
        public int? housetypecode
        {
            get { return _housetypecode; }
            set { _housetypecode = value; }
        }

        private DateTime _createdate = DateTime.Now;
        [ExcelExportIgnore]
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

        private int? _fitmentcode;
        [ExcelExportIgnore]
        [DisplayName("装修情况")]
        public int? fitmentcode
        {
            get { return _fitmentcode; }
            set { _fitmentcode = value; }
        }

        private int _surveyid = 0;
        [ExcelExportIgnore]
        public int surveyid
        {
            get { return _surveyid; }
            set { _surveyid = value; }
        }

        private int? _oldid;
        [ExcelExportIgnore]
        public int? oldid
        {
            get { return _oldid; }
            set { _oldid = value; }
        }

        private int? _cityid;
        [ExcelExportIgnore]
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }

        private int _valid = 1;
        [ExcelExportIgnore]
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        private int _fxtcompanyid = 25;
        [DisplayName("案例所在公司")]
        [ExcelExportIgnore]
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }

        private DateTime? _savedatetime;
        [ExcelExportIgnore]
        public DateTime? savedatetime
        {
            get { return _savedatetime; }
            set { _savedatetime = value; }
        }

        private string _saveuser;
        [ExcelExportIgnore]
        public string saveuser
        {
            get
            {
                if (_saveuser == null)
                {
                    _saveuser = "";
                }
                return _saveuser;
            }
            set { _saveuser = value; }
        }

        [ExcelExportIgnore]//楼盘的创建者，鉴于分辨案例库的基础楼盘权限
        public string projectCreator { get; set; }
        [ExcelExportIgnore]//楼盘的创建者，鉴于分辨案例库的基础楼盘权限
        public int projectFxtcompanyid { get; set; }
        #endregion
    }
}
