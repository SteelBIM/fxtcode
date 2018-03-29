using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_CaseLand
    {
        [ExcelExportIgnore]
        public string opValue { get; set; }
        [ExcelExportIgnore]
        public int pageIndex { get; set; }

        private long? _caseid;
        //[SQLField("caseid", EnumDBFieldUsage.PrimaryKey, true)]
        [DisplayName("案例ID")]
        [ExcelExportIgnore]
        public long? caseid
        {
            get { return _caseid; }
            set { _caseid = value; }
        }

        private string _areaname;
        [DisplayName("*行政区")]
        public string areaname
        {
            get { return _areaname; }
            set { _areaname = value; }
        }

        private int? _areaid;
        [DisplayName("行政区ID")]
        [Required(ErrorMessage = "行政区不能为空")]
        [Range(1, 10000, ErrorMessage = "请选择行政区")]
        [ExcelExportIgnore]
        public int? areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }

        private int? _subareaid;
        [DisplayName("片区")]
        [ExcelExportIgnore]
        public int? subareaid
        {
            get { return _subareaid; }
            set { _subareaid = value; }
        }
        [DisplayName("片区")]
        public string SubAreaName { get; set; }

        private string _landno;
        [DisplayName("*宗地号")]
        [Required(ErrorMessage = "宗地号不能为空")]
        public string landno
        {
            get { return _landno; }
            set { _landno = value; }
        }

        private DateTime _casedate;
        [DisplayName("*案例日期")]
        [Required(ErrorMessage = "案例日期不能为空")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime casedate
        {
            get { return _casedate; }
            set { _casedate = value; }
        }

        private int? _bargaintypecode;
        [DisplayName("买卖方式ID")]
        [ExcelExportIgnore]
        public int? bargaintypecode
        {
            get { return _bargaintypecode; }
            set { _bargaintypecode = value; }
        }
        [DisplayName("买卖方式")]
        public string bargainTypeName
        {
            get
            {
                if (bargaintypecode != null)
                {

                    switch (bargaintypecode)
                    {
                        case 3004001:
                            return "招标";
                        case 3004002:
                            return "拍卖";
                        case 3004003:
                            return "挂牌";
                        case 3004004:
                            return "协议";
                        default:
                            return "";
                    }
                }
                return "";
            }
        }

        private decimal? _buildunitprice;
        [DisplayName("建面地价_元每平方米")]
        public decimal? buildunitprice
        {
            get { return _buildunitprice; }
            set { _buildunitprice = value; }
        }

        private decimal? _landunitprice;
        [DisplayName("土地单价_元每平方米")]
        public decimal? landunitprice
        {
            get { return _landunitprice; }
            set { _landunitprice = value; }
        }

        private decimal? _landarea;
        [DisplayName("土地面积_平方米")]
        public decimal? landarea
        {
            get { return _landarea; }
            set { _landarea = value; }
        }

        private decimal? _minbargainprice;
        [DisplayName("起价_万元")]
        public decimal? minbargainprice
        {
            get { return _minbargainprice; }
            set { _minbargainprice = value; }
        }

        private decimal? _buildingarea;
        [DisplayName("建筑面积_平方米")]
        public decimal? buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }

        private DateTime? _startusabledate;
        [DisplayName("起始日期")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? startusabledate
        {
            get { return _startusabledate; }
            set { _startusabledate = value; }
        }

        private DateTime? _enddate;
        [DisplayName("土地使用结束日期")]
        public DateTime? enddate
        {
            get { return _enddate; }
            set { _enddate = value; }
        }

        private int? _landpurposecode;
        [DisplayName("土地用途ID")]
        [ExcelExportIgnore]
        public int? landpurposecode
        {
            get { return _landpurposecode; }
            set { _landpurposecode = value; }
        }
        [DisplayName("土地用途")]
        public string LandUseName
        {
            get
            {
                if (landpurposecode != null)
                {
                    switch (landpurposecode)
                    {
                        case 1001001:
                            return "居住";
                        case 1001002:
                            return "居住(别墅)";
                        case 1001003:
                            return "居住(洋房)";
                        case 1001004:
                            return "商业";
                        case 1001005:
                            return "办公";
                        case 1001006:
                            return "工业";
                        case 1001007:
                            return "商业、居住";
                        case 1001008:
                            return "商业、办公";
                        case 1001009:
                            return "办公、居住";
                        case 1001010:
                            return "停车场";
                        case 1001011:
                            return "酒店";
                        case 1001012:
                            return "加油站";
                        case 1001013:
                            return "综合";
                        case 1001014:
                            return "其他";
                        default:
                            return "";
                    }
                }
                return "";
            }

        }

        private string _landpurposedesc;
        [DisplayName("规划用途")]
        public string landpurposedesc
        {
            get { return _landpurposedesc; }
            set { _landpurposedesc = value; }
        }

        private string _winner;
        [DisplayName("竞得者")]
        public string winner
        {
            get { return _winner; }
            set { _winner = value; }
        }

        private DateTime? _windate;
        [DisplayName("竞得时间")]
        public DateTime? windate
        {
            get { return _windate; }
            set { _windate = value; }
        }

        private string _seller;
        [DisplayName("卖方(出让方)")]
        public string seller
        {
            get { return _seller; }
            set { _seller = value; }
        }

        private decimal? _dealtotalprice;
        [DisplayName("成交价_万元")]
        public decimal? dealtotalprice
        {
            get { return _dealtotalprice; }
            set { _dealtotalprice = value; }
        }

        private DateTime? _dealdate;
        [DisplayName("成交日期")]
        public DateTime? dealdate
        {
            get { return _dealdate; }
            set { _dealdate = value; }
        }

        private int? _bargainstatecode;
        [DisplayName("成交状态ID")]
        [ExcelExportIgnore]
        public int? bargainstatecode
        {
            get { return _bargainstatecode; }
            set { _bargainstatecode = value; }
        }
        [DisplayName("成交状态")]
        public string BargainStateName
        {
            get
            {
                if (bargainstatecode != null)
                {
                    switch (bargainstatecode)
                    {
                        case 3003001:
                            return "已成交";
                        case 3003002:
                            return "未成交";
                        default:
                            return "";
                    }
                }
                return "";
            }
        }

        private int? _developdegreecode;
        [DisplayName("土地开发程度ID")]
        [ExcelExportIgnore]
        public int? developdegreecode
        {
            get { return _developdegreecode; }
            set { _developdegreecode = value; }
        }
        [DisplayName("土地开发程度")]
        public string DevelopDegreeName
        {
            get
            {
                if (developdegreecode != null)
                {
                    switch (developdegreecode)
                    {
                        case 3005001:
                            return "三通一平";
                        case 3005002:
                            return "五通一平";
                        case 3005003:
                            return "六通一平";
                        case 3005004:
                            return "七通一平";
                        case 3005005:
                            return "生地";
                        case 3005006:
                            return "八通一平";
                        case 3005007:
                            return "熟地";
                        default:
                            return "";
                    }
                }
                return "";
            }
        }

        private string _landaddress;
        [DisplayName("地址")]
        public string landaddress
        {
            get { return _landaddress; }
            set { _landaddress = value; }
        }

        private string _bargainedby;
        [DisplayName("交易机构")]
        public string bargainedby
        {
            get { return _bargainedby; }
            set { _bargainedby = value; }
        }
        
        private string _remark;
        [DisplayName("备注")]
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        private DateTime? _bargaindate;
        [DisplayName("挂牌日期")]
        public DateTime? bargaindate
        {
            get { return _bargaindate; }
            set { _bargaindate = value; }
        }

        private int? _usableyear;
        [DisplayName("土地使用年限")]
        public int? usableyear
        {
            get { return _usableyear; }
            set { _usableyear = value; }
        }

        private decimal? _cubagerate;
        [DisplayName("容积率")]
        public decimal? cubagerate
        {
            get { return _cubagerate; }
            set { _cubagerate = value; }
        }

        private decimal? _mincubagerate;
        [DisplayName("最小容积率")]
        public decimal? mincubagerate
        {
            get { return _mincubagerate; }
            set { _mincubagerate = value; }
        }

        private decimal? _maxcubagerate;
        [DisplayName("最大容积率")]
        public decimal? maxcubagerate
        {
            get { return _maxcubagerate; }
            set { _maxcubagerate = value; }
        }

        private decimal? _coverrate;
        [DisplayName("覆盖率")]
        public decimal? coverrate
        {
            get { return _coverrate; }
            set { _coverrate = value; }
        }

        private decimal? _maxcoverrate;
        [DisplayName("最大覆盖率")]
        public decimal? maxcoverrate
        {
            get { return _maxcoverrate; }
            set { _maxcoverrate = value; }
        }

        private string _sourcephone;
        [DisplayName("联系电话")]
        public string sourcephone
        {
            get { return _sourcephone; }
            set { _sourcephone = value; }
        }

        private int? _landsourcecode;
        [DisplayName("土地来源ID")]
        [ExcelExportIgnore]
        public int? landsourcecode
        {
            get { return _landsourcecode; }
            set { _landsourcecode = value; }
        }
        [DisplayName("土地来源")]
        public string LandSourceName
        {
            get
            {
                if (landsourcecode != null)
                {

                    switch (landsourcecode)
                    {
                        case 3002001:
                            return "政府出让";
                        case 3002002:
                            return "企业转让";
                        case 3002003:
                            return "行政划拨";
                        case 3002004:
                            return "旧城改造";
                        default:
                            return "";
                    }
                }
                return "";
            }
        }

        [ExcelExportIgnore]
        public int? useTypeCode { get; set; }
        [DisplayName("使用权性质")]
        public string UseTypeCodeName
        {
            get
            {
                if (useTypeCode != null)
                {

                    switch (useTypeCode)
                    {
                        case 1022001:
                            return " 国家所有";
                        case 1022002:
                            return "集体所有";
                        default:
                            return "";
                    }
                }
                return "";
            }
        }

        private int? _heightlimited;
        [DisplayName("高度限制")]
        [RegularExpression(@"^[0-9]{0,9}$", ErrorMessage = "{0}必须是整数")]
        public int? heightlimited
        {
            get { return _heightlimited; }
            set { _heightlimited = value; }
        }

        private string _planlimited;
        [DisplayName("规划限制")]
        public string planlimited
        {
            get { return _planlimited; }
            set { _planlimited = value; }
        }

        private string _landusestatus;
        [DisplayName("土地利用状况")]
        public string landusestatus
        {
            get { return _landusestatus; }
            set { _landusestatus = value; }
        }

        private int? _landclass;
        [ExcelExportIgnore]
        [DisplayName("土地等级ID")]
        public int? landclass
        {
            get { return _landclass; }
            set { _landclass = value; }
        }
        [DisplayName("土地等级")]
        public string LandClassName
        {
            get
            {
                if (landclass != null)
                {
                    switch (landclass)
                    {
                        case 1209001:
                            return "一级";
                        case 1209002:
                            return "二级";
                        case 1209003:
                            return "三级";
                        case 1209004:
                            return "四级";
                        case 1209005:
                            return "五级";
                        case 1209006:
                            return "六级";
                        case 1209007:
                            return "七级";
                        default:
                            return "";
                    }
                }
                return "";
            }
        }

        private decimal? _greenrage;
        [DisplayName("绿化率")]
        public decimal? greenrage
        {
            get { return _greenrage; }
            set { _greenrage = value; }
        }

        private decimal? _mingreenrage;
        [DisplayName("最小绿化率")]
        public decimal? mingreenrage
        {
            get { return _mingreenrage; }
            set { _mingreenrage = value; }
        }

        private DateTime? _arrangestartdate;
        [DisplayName("约定开工日期")]
        public DateTime? arrangestartdate
        {
            get { return _arrangestartdate; }
            set { _arrangestartdate = value; }
        }

        private DateTime? _arrangeenddate;
        [DisplayName("约定竣工日期")]
        public DateTime? arrangeenddate
        {
            get { return _arrangeenddate; }
            set { _arrangeenddate = value; }
        }

        private string _creator;
        [ExcelExportIgnore]
        [DisplayName("创建者")]
        public string creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        private DateTime _createdate = DateTime.Now;
        [ExcelExportIgnore]
        [DisplayName("创建时间")]
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

        private int? _oldid;
        [ExcelExportIgnore]
        public int? oldid
        {
            get { return _oldid; }
            set { _oldid = value; }
        }

        private int _valid = 1;
        [ExcelExportIgnore]
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        private int _cityid = 6;
        [DisplayName("城市ID")]
        [ExcelExportIgnore]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }

        private int _fxtcompanyid = 25;
        [DisplayName("评估机构")]
        [ExcelExportIgnore]
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }

        private DateTime? _savedatetime;
        [ExcelExportIgnore]
        [DisplayName("最后修改时间")]
        public DateTime? savedatetime
        {
            get { return _savedatetime; }
            set { _savedatetime = value; }
        }

        private string _saveuser;
        [ExcelExportIgnore]
        [DisplayName("最后修改者")]
        public string saveuser
        {
            get { return _saveuser; }
            set { _saveuser = value; }
        }

        private string _sourcename;
        [DisplayName("来源名称")]
        public string sourcename
        {
            get { return _sourcename; }
            set { _sourcename = value; }
        }

        private string _sourcelink;
        [DisplayName("来源链接")]
        public string sourcelink
        {
            get { return _sourcelink; }
            set { _sourcelink = value; }
        }
        
        [ExcelExportIgnore]
        public List<string> Description { get; set; }
        [ExcelExportIgnore]
        public DateTime? DateFrom { get; set; }
        [ExcelExportIgnore]
        public DateTime? DateTo { get; set; }
        [ExcelExportIgnore]
        public DateTime? CaseDateFrom { get; set; }
        [ExcelExportIgnore]
        public DateTime? CaseDateTo { get; set; }

    }

}
