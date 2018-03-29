using System;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Object : DatObject
    {
        [SQLReadOnly]
        public string typecodename { get; set; }

        [SQLReadOnly]
        public string ownertypecodename { get; set; }

        [SQLReadOnly]
        public int provinceid { get; set; }

        /// <summary>
        /// 1-从询价添加,2-从委估对象添加的价格信息
        /// </summary>
        [SQLReadOnly]
        public int dataorigintype { get; set; }

        /// <summary>
        /// 询价Id
        /// </summary>
        [SQLReadOnly]
        public long qid { get; set; }
        [SQLReadOnly]
        public decimal unitprice { get; set; }
        [SQLReadOnly]
        public decimal totalprice { get; set; }
        [SQLReadOnly]
        public decimal tax { get; set; }
        [SQLReadOnly]
        public decimal netprice { get; set; }
        [SQLReadOnly]
        public decimal legalpayment { get; set; }
        [SQLReadOnly]
        public decimal shouldfilllandprice { get; set; }
        [SQLReadOnly]
        public string entrustid_str { get { return entrustid.ToString(); } }

        [SQLReadOnly]
        public string action { get; set; }
        [SQLReadOnly]
        public string cityname { get; set; }
        /// <summary>
        /// 行政区名称
        /// </summary>
        [SQLReadOnly]
        public string areaname { get; set; }

        /// <summary>
        /// 查勘状态
        /// </summary>
        [SQLReadOnly]
        public int surveystatecode { get; set; }

        /// <summary>
        /// 查勘完成时间
        /// </summary>
        [SQLReadOnly]
        public DateTime surveycompletetime { get; set; }

        /// <summary>
        /// 评估总面积
        /// </summary>
        [SQLReadOnly]
        public string SumTotalArea { get; set; }
        /// <summary>
        /// 评估总价
        /// </summary>
        [SQLReadOnly]
        public decimal SumTotalPrice { get; set; }
        /// <summary>
        /// 总税费
        /// </summary>
        [SQLReadOnly]
        public decimal SumTotalTax { get; set; }
        /// <summary>
        /// 总净值
        /// </summary>
        [SQLReadOnly]
        public decimal SumTotalNetPrice { get; set; }
        /// <summary>
        /// 主房总价
        /// </summary>
        [SQLReadOnly]
        public decimal totalhouseprice { get; set; }
        /// <summary>
        /// 土地总价
        /// </summary>
        [SQLReadOnly]
        public decimal totallandprice { get; set; }
        /// <summary>
        /// 附属房屋总价
        /// </summary>
        [SQLReadOnly]
        public decimal totalsubhouseprice { get; set; }
        /// <summary>
        /// 委托方
        /// </summary>
        [SQLReadOnly]
        public string clientname { get; set; }
        /// <summary>
        /// 委托方电话
        /// </summary>
        [SQLReadOnly]
        public string clientphone { get; set; }
        /// <summary>
        /// 是否争议
        /// </summary>
        [SQLReadOnly]
        public bool ispricedispute { get; set; }
        /// <summary>
        /// 回价时间
        /// </summary>
        [SQLReadOnly]
        public DateTime biddate { get; set; }
        /// <summary>
        /// 价格说明
        /// </summary>
        [SQLReadOnly]
        public string priceremark { get; set; }
        /// <summary>
        /// 委托银行
        /// </summary>
        [SQLReadOnly]
        public string bankfullname { get; set; }
        /// <summary>
        /// 业务联系人
        /// </summary>
        [SQLReadOnly]
        public string bankcontact { get; set; }
        /// <summary>
        /// 银行联系电话
        /// </summary>
        [SQLReadOnly]
        public string bankcontactphone { get; set; }
        /// <summary>
        /// 业务环节
        /// </summary>
        [SQLReadOnly]
        public string entrustlink { get; set; }
        /// <summary>
        /// 业务id
        /// </summary>
        [SQLReadOnly]
        public long eid { get; set; }

        /// <summary>
        /// 土地面积
        /// </summary>
        [SQLReadOnly]
        public string landarea { get; set; }

        /// <summary>
        /// 强制变现值
        /// </summary>
        [SQLReadOnly]        
        public decimal liquidityvalue { get; set; }

        /// <summary>
        /// 强制变现税费额
        /// </summary>
        [SQLReadOnly]
        public decimal liquiditytaxvalue { get; set; }
        // Alex 增加下列字段 2015-11-09
        /// <summary>
        /// 纠错价格
        /// </summary>
        [SQLReadOnly]
        public decimal adjustprice { get; set; }
        /// <summary>
        /// 纠错价格总价
        /// </summary>
        [SQLReadOnly]
        public decimal  adjusttotalprice{get;set;}

        /// <summary>
        /// 纠错价格税费
        /// </summary>
        [SQLReadOnly]
        public decimal adjusttax {get;set;}

        /// <summary>
        /// 纠错价格净值
        /// </summary>
        [SQLReadOnly]
        public decimal adjustnetprice {get;set;}
        
        /// <summary>
        /// 价格纠错评估总价
        /// </summary>
        [SQLReadOnly]
        public decimal adjusthousetotalprice {get;set;}
        
        /// <summary>
        /// 业务员求单价
        /// </summary>
        [SQLReadOnly]
        public decimal priceonrequest {get;set;}

        /// <summary>
        /// 业务员要求总价
        /// </summary>
        [SQLReadOnly]
        public decimal totalpriceonrequest {get;set;}
         
        /// <summary>
        /// 价格争议评估总价
        /// </summary>
        [SQLReadOnly]
        public decimal housetotalpriceonrequest {get;set;}
        
        /// <summary>
        /// 价格争议税费
        /// </summary>
        [SQLReadOnly]
        public decimal taxonrequest { get; set; }
        

        /// <summary>
        /// 价格争议净值
        /// </summary>
        [SQLReadOnly]
        public decimal netpriceonrequest { get; set; }
        // Alex 增加以下四个字段  2015-12-28
        /// <summary>
        /// 建筑年代
        /// </summary>
        [SQLReadOnly]
        public string buildingdate { get; set; }

        /// <summary>
        /// 房间数量
        /// </summary>
        [SQLReadOnly]
        public int? housecount { get; set; }

        /// <summary>
        /// 用途
        /// </summary>
        [SQLReadOnly]
        public string purpose { get; set; }

        /// <summary>
        /// 拟贷金额
        /// </summary>
        [SQLReadOnly]
        public decimal? prepareloanamount { get; set; }
        // Alex 增加 省份名称 片区名称 2016-02-23
        /// <summary>
        /// 省份名称
        /// </summary>
        [SQLReadOnly]
        public string provincename { get; set; }
        /// <summary>
        /// 片区名称
        /// </summary>
        [SQLReadOnly]
        public string subareaname { get; set; }
        /// <summary>
        /// 预评单价 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? ypunitprice { get; set; }
        /// <summary>
        /// 预评总价 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? totalpricebyyp { get; set; }
        /// <summary>
        /// 预评税费 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? yptax { get; set; }
        /// <summary>
        /// 预评净值 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? ypnetprice { get; set; }
        /// <summary>
        /// 预评法定优先受偿款 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? yplegalpayment { get; set; }
        /// <summary>
        /// 预评强制变现值 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? ypliquidityvalue { get; set; }
        /// <summary>
        /// 预评强制变现税费值 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? ypliquiditytaxvalue { get; set; }
        /// <summary>
        /// 预评应补地价 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? ypshouldfilllandprice { get; set; }
        /// <summary>
        /// 报告单价 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? rpunitprice { get; set; }
        /// <summary>
        /// 报告总价 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? totalpricebyrp { get; set; }
        /// <summary>
        /// 报告税费 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? rptax { get; set; }
        /// <summary>
        /// 报告净值 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? rpnetprice { get; set; }
        /// <summary>
        /// 报告法定优先受偿款 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? rplegalpayment { get; set; }
        /// <summary>
        /// 报告强制变现值 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? rpliquidityvalue { get; set; }
        /// <summary>
        /// 报告强制变现税费值 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? rpliquiditytaxvalue { get; set; }
        /// <summary>
        /// 报告应补地价 Alex 2016-03-12 
        /// </summary>
        [SQLReadOnly]
        public decimal? rpshouldfilllandprice { get; set; }
        /// <summary>
        /// 土地单价 Alex 2016-05-11
        /// </summary>
        [SQLReadOnly]
        public decimal? landunitprice { get; set; }
        /// <summary>
        /// 土地总价 Alex 2016-05-11
        /// </summary>
        [SQLReadOnly]
        public decimal? landtotalprice { get; set; }
        /// <summary>
        /// 预评正常单价
        /// </summary>
        [SQLReadOnly]
        public decimal? ypnormalprice { get; set; }
       
       
        /// <summary>
        /// 预评住宅评估总值
        /// </summary>
         [SQLReadOnly]
        public decimal? yphousetotalprice { get; set; }

        /// <summary>
        /// 报告正常估价单价
        /// </summary>
        [SQLReadOnly]
        public decimal? rpnormalprice { get; set; }

        /// <summary>
        /// 报告住宅评估总值
        /// </summary>
        [SQLReadOnly]
        public decimal? rphousetotalprice { get;set; }

        /// <summary>
        /// 预评土地总价
        /// </summary>
        [SQLReadOnly]
        public decimal? yplandtotalprice { get; set; }
        /// <summary>
        /// 报告土地总价
        /// </summary>
        [SQLReadOnly]
        public decimal? rplandtotalprice { get; set; }
        /// <summary>
        /// 预评土地单价
        /// </summary>
        [SQLReadOnly]
        public  decimal? yplandunitprice { get; set; }
        /// <summary>
        /// 报告土地单价
        /// </summary>
        [SQLReadOnly]
        public decimal? rplandunitprice { get; set; }
        /// <summary>
        /// 产权人id  由于很多地方会有多次保存 例如业务测算时会保存一次业务保存时又保存一次 导致产权人保存了两个
        /// </summary>
        [SQLReadOnly]
        public string ownerid { get; set; }
    }


    /// <summary>
    /// 汇总委估对象的信息
    /// <!--byte 2014-12-25-->
    /// </summary>
    public class SumObjectsInfo : BaseTO
    {
        /// <summary>
        /// 建筑面积
        /// </summary>
        [SQLReadOnly]
        public string buildingarea
        {
            get;
            set;
        }
        /// <summary>
        /// 评估总值
        /// </summary>
        [SQLReadOnly]
        public decimal totalprice
        {
            get;
            set;
        }
        /// <summary>
        /// 主房总价
        /// </summary>
        [SQLReadOnly]
        public decimal totalhouseprice
        {
            get;
            set;
        }
        /// <summary>
        /// 土地总价
        /// </summary>
        [SQLReadOnly]
        public decimal totallandprice
        {
            get;
            set;
        }
        /// <summary>
        /// 附属房屋总价
        /// </summary>
        [SQLReadOnly]
        public decimal totalsubhouseprice
        {
            get;
            set;
        }
        /// <summary>
        /// 总净值
        /// </summary>
        [SQLReadOnly]
        public decimal netprice
        {
            get;
            set;
        }
        /// <summary>
        /// 总税费
        /// </summary>
        [SQLReadOnly]
        public decimal tax
        {
            get;
            set;
        }
        /// <summary>
        /// 法定优先受偿款总额
        /// </summary>
        [SQLReadOnly]
        public decimal legalpayment
        {
            get;
            set;
        }
        /// <summary>
        /// 应补地价总额
        /// </summary>
        [SQLReadOnly]
        public decimal shouldfilllandprice
        {
            get;
            set;
        }
        /// <summary>
        /// 土地总面积
        /// </summary>
        [SQLReadOnly]
        public string landarea
        {
            get;
            set;
        }
    }
}
