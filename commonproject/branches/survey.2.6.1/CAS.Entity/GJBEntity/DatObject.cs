using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// 银行联系人
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
