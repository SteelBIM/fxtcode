using System;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Entrust : DatEntrust
    {
        /// <summary>
        /// 分支机构名称
        /// </summary>
        [SQLReadOnly]
        public string subcompanyname { get; set; }

        /// <summary>
        /// 委托类型名称
        /// </summary>
        [SQLReadOnly]
        public string biztypename { get; set; }


        [SQLReadOnly]
        public string statecodename { get; set; }

        //[SQLReadOnly]
        //public int provinceid { get; set; }

        /// <summary>
        /// 提交人
        /// </summary>
        [SQLReadOnly]
        public string submitusername { get; set; }

        /// <summary>
        /// 分配人
        /// </summary>
        [SQLReadOnly]
        public string assignusername { get; set; }

        /// <summary>
        /// 估价师
        /// </summary>
        [SQLReadOnly]
        public string appraisersusername { get; set; }

        /// <summary>
        /// 业务员姓名
        /// </summary>
        [SQLReadOnly]
        public string businessusername { get; set; }

        /// <summary>
        /// 业务跟进人姓名
        /// </summary>
        [SQLReadOnly]
        public string businessfollowername { get; set; }

        /// <summary>
        /// 业务员电话
        /// </summary>
        [SQLReadOnly]
        public string businessuserphone { get; set; }
        /// <summary>
        /// 报告使用方
        /// </summary>
        [SQLReadOnly]
        public string reportuserfullname { get; set; }

        /// <summary>
        /// 由于js会把最后一位显示为0或者显示错误，所以直接按字符串输出
        /// </summary>
        [SQLReadOnly]
        public string entrustid_str { get { return entrustid.ToString(); } }

        /// <summary>
        /// 委托对象
        /// </summary>
        [SQLReadOnly]
        public string projectfullname { get; set; }
        /// <summary>
        /// 报告类型名称
        /// </summary>
        [SQLReadOnly]
        public string reporttypename { get; set; }

        /// <summary>
        /// 委托客户名称
        /// </summary>
        [SQLReadOnly]
        public string bankname { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [SQLReadOnly]
        public string querytypename { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [SQLReadOnly]
        public string address { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        [SQLReadOnly]
        public string buildingarea { get; set; }

        /// <summary>
        /// 权利人性质
        /// </summary>
        [SQLReadOnly]
        public string ownertypename { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        [SQLReadOnly]
        public string salemanname { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [SQLReadOnly]
        public string contactuser { get; set; }



        /// <summary>
        /// 预评撰写人
        /// </summary>
        [SQLReadOnly]
        public string ypwriter { get; set; }

        /// <summary>
        /// 报告撰写人
        /// </summary>
        [SQLReadOnly]
        public string reportwriter { get; set; }

        /// <summary>
        /// 预评节点
        /// </summary>
        [SQLReadOnly]
        public string ypjiedian { get; set; }

        /// <summary>
        /// 报告节点
        /// </summary>
        [SQLReadOnly]
        public string reportjiedian { get; set; }

        /// <summary>
        /// 预评审批人
        /// </summary>
        [SQLReadOnly]
        public string ypshenpiuser { get; set; }
        /// <summary>
        /// 报告审批人
        /// </summary>
        [SQLReadOnly]
        public string reportshenpiuser { get; set; }
        /// <summary>
        /// 转交状态(报告)
        /// </summary>
        [SQLReadOnly]
        public int rassignstate { get; set; }
        /// <summary>
        /// 转出人  谁转的(报告)
        /// </summary>
        [SQLReadOnly]
        public string rassignoutperson { get; set; }
        /// <summary>
        /// 转入人 转给谁(报告)
        /// </summary>
        [SQLReadOnly]
        public string rassigninperson { get; set; }

        /// <summary>
        /// 转交状态(预评)
        /// </summary>
        [SQLReadOnly]
        public int yassignstate { get; set; }
        /// <summary>
        /// 转出人  谁转的(预评)
        /// </summary>
        [SQLReadOnly]
        public string yassignoutperson { get; set; }
        /// <summary>
        /// 转入人 转给谁(预评)
        /// </summary>
        [SQLReadOnly]
        public string yassigninperson { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 主体预评、报告是否操作"修改报告"
        /// </summary>
        public bool? changedreport { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 预评撰写人
        /// </summary>
        public int? ypwriteruserid { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 报告撰写人
        /// </summary>
        public int? writerid { get; set; }

        /// <summary>
        /// 权利人
        /// </summary>
        [SQLReadOnly]
        public string owner { get; set; }

        /// <summary>
        /// 查勘状态名
        /// </summary>
        [SQLReadOnly]
        public string surveystatename { get; set; }
        /// <summary>
        ///业务类型 预评或者报告 
        /// </summary>
        [SQLReadOnly]
        public int businesstype { get; set; }

        /// <summary>
        ///银行名称 
        /// </summary>
        [SQLReadOnly]
        public string bankcompanyname { get; set; }
        /// <summary>
        ///查勘id集合 1,2,3 
        /// </summary>
        [SQLReadOnly]
        public string sids { get; set; }
        [SQLReadOnly]
        public decimal querytotalprice { get; set; }
        [SQLReadOnly]
        public string querytotalarea { get; set; }
        [SQLReadOnly]
        public decimal totalprice { get; set; }
        [SQLReadOnly]
        public decimal realityreceive { get; set; }
        [SQLReadOnly]
        public decimal receivable { get; set; }
        /// <summary>
        /// 收费状态
        /// </summary>
        [SQLReadOnly]
        public string chargestatename { get; set; }
        /// <summary>
        /// 坏账业务ID
        /// </summary>
        [SQLReadOnly]
        public int chargebadid { get; set; }
        /// <summary>
        /// 业务处于哪个阶段(报告:2\预评:1\null:0)
        /// </summary>
        [SQLReadOnly]
        public string entruststagename { get; set; }
        /// <summary>
        /// 当前撰写人，根据业务阶段判断
        /// </summary>
        [SQLReadOnly]
        public string entrustwriter { get; set; }

        /// <summary>
        /// 业务状态[显示]
        /// </summary>
        [SQLReadOnly]
        public string statename { get; set; }

        [SQLReadOnly]
        public DateTime? biddate { get; set; }
        ///////////////////////////////////////////////////
        /// <summary>
        /// 报告状态
        /// </summary>
        [SQLReadOnly]
        public int reportstate { get; set; }
        /// <summary>
        /// 预评状态
        /// </summary>
        [SQLReadOnly]
        public int ypstatecode { get; set; }
        [SQLReadOnly]
        public long ypid { get; set; }
        /// <summary>
        /// 登记人姓名
        /// </summary>
        [SQLReadOnly]
        public string createusername { get; set; }

        [SQLReadOnly]
        public long reportid { get; set; }
        /// <summary>
        /// 预评状态名称
        /// </summary>
        [SQLReadOnly]
        public string ypstatecodename { get; set; }
        /// <summary>
        /// 预评时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? ypdate
        {
            get;
            set;
        }
        /// <summary>
        /// 报告编号
        /// </summary>
        [SQLReadOnly]
        public string reportno { get; set; }
        /// <summary>
        /// 预评创建时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? dycreatedate
        {
            get;
            set;
        }
        /// <summary>
        /// 预评编号
        /// </summary>
        [SQLReadOnly]
        public string ypnumber { get; set; }
        /// <summary>
        /// 报告状态名称
        /// </summary>
        [SQLReadOnly]
        public string reportstatename { get; set; }
        /// <summary>
        /// 报告完成时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? drcompletedate { get; set; }
        /// <summary>
        /// 报告创建时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? drcreatedate { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        [SQLReadOnly]
        public string provincename { get; set; }

        /// <summary>
        /// 行政区名称
        /// </summary>
        [SQLReadOnly]
        public string areaname { get; set; }
        /// <summary>
        /// 片区名称
        /// </summary>
        [SQLReadOnly]
        public string subareaname { get; set; }
        /// <summary>
        /// 环节 Alex 2016-04-21
        /// </summary>
        [SQLReadOnly]
        public string detailstatus { get; set; }
        /// <summary>
        /// 当前处理人 Alex 2016-04-21
        /// </summary>
        [SQLReadOnly]
        public string currenthandleruser { get; set; }
        #region 打印与盖章字段
        /// <summary>
        /// 打印模块中的打印份数
        /// </summary>
        [SQLReadOnly]
        public int? newprintnumber { get; set; }
        /// <summary>
        /// 打印模块中的打印状态
        /// </summary>
        [SQLReadOnly]
        public bool? newprintstatus { get; set; }
        /// <summary>
        /// 盖章模块中的盖章时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? newsealtime { get; set; }
        /// <summary>
        /// 盖章模块中的盖章状态
        /// </summary>
        [SQLReadOnly]
        public bool? newsealstatus { get; set; }
        /// <summary>
        /// 盖章模块中的盖章人id
        /// </summary>
        [SQLReadOnly]
        public int? newsealuserid { get; set; }
        /// <summary>
        /// 盖章模块中的盖章人名称
        /// </summary>
        [SQLReadOnly]
        public string newsealusername { get; set; }
        /// <summary>
        /// 盖章模块中的盖章人备注
        /// </summary>
        [SQLReadOnly]
        public string newsealremark { get; set; }
        #endregion
        /// <summary>
        /// 报告阶段
        /// </summary>
        [SQLReadOnly]
        public int? reportstage { get; set; }
        /// <summary>
        /// 预评id或者报告id
        /// </summary>
        [SQLReadOnly]
        public long? id { get; set; }
        /// <summary>
        /// 撰写人
        /// </summary>
        [SQLReadOnly]
        public string writename { get; set; }
        /// <summary>
        /// 报告或者预评完成时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? completedate { get; set; }
        /// <summary>
        /// 报告或者预评完成时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? approvaldate { get; set; }
        /// <summary>
        /// 完成状态名称，统计明细页面使用 Alex 2016-09-27
        /// </summary>
        [SQLReadOnly]
        public string complatestatus { get; set; }
        /// <summary>
        /// 用户id  统计明细页面使用 Alex 2016-09-27
        /// </summary>
        [SQLReadOnly]
        public int userid { get; set; }
        /// <summary>
        /// 打印时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? newprinttime { get; set; }
        /// <summary>
        /// 打印人
        /// </summary>
        [SQLReadOnly]
        public int newprintuser { get; set; }
        /// <summary>
        /// 打印人中文
        /// </summary>
        [SQLReadOnly]
        public string newprintusername { get; set; }
    }

    public class CAS_DatEntrust : DatEntrust
    {

        ///////////////////////////////////////////////////
        /// <summary>
        /// 报告状态
        /// </summary>
        [SQLReadOnly]
        public int reportstate { get; set; }
        /// <summary>
        /// 预评状态
        /// </summary>
        [SQLReadOnly]
        public int ypstatecode { get; set; }
        [SQLReadOnly]
        public long ypid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [SQLReadOnly]
        public string createusername { get; set; }

        [SQLReadOnly]
        public long reportid { get; set; }
        /// <summary>
        /// 预评状态名称
        /// </summary>
        [SQLReadOnly]
        public string ypstatecodename { get; set; }
        /// <summary>
        /// 预评时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? ypdate
        {
            get;
            set;
        }
        /// <summary>
        /// 报告编号
        /// </summary>
        [SQLReadOnly]
        public string reportno { get; set; }
        /// <summary>
        /// 预评创建时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? dycreatedate
        {
            get;
            set;
        }
        /// <summary>
        /// 预评编号
        /// </summary>
        [SQLReadOnly]
        public string ypnumber { get; set; }
        /// <summary>
        /// 报告状态名称
        /// </summary>
        [SQLReadOnly]
        public string reportstatename { get; set; }
        /// <summary>
        /// 报告完成时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? drcompletedate { get; set; }
        /// <summary>
        /// 报告创建时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? drcreatedate { get; set; }
        /// <summary>
        /// 预评第三方ID
        /// </summary>
        [SQLReadOnly]
        public long dyetid { get; set; }
        /// <summary>
        /// 预评第三方是否已完成
        /// </summary>
        [SQLReadOnly]
        public bool dyiscomplate { get; set; }
        /// <summary>
        /// 预评第三方业务阶段
        /// </summary>
        [SQLReadOnly]
        public string dyexternalstatetext { get; set; }
        /// <summary>
        /// 预评第三方发送时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? dychangedon { get; set; }
        /// <summary>
        /// 报告第三方ID
        /// </summary>
        [SQLReadOnly]
        public long dretid { get; set; }
        /// <summary>
        /// 报告第三方是否已完成
        /// </summary>
        [SQLReadOnly]
        public bool driscomplate { get; set; }
        /// <summary>
        /// 报告第三方业务阶段
        /// </summary>
        [SQLReadOnly]
        public string drexternalstatetext { get; set; }
        /// <summary>
        /// 报告第三方发送时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? drchangedon { get; set; }
        /// <summary>
        /// 报告类型名称
        /// </summary>
        [SQLReadOnly]
        public string reporttypename { get; set; }

        /// <summary>
        /// 预评单价
        /// </summary>
        [SQLReadOnly]
        public decimal? dyunitprice { get; set; }
        /// <summary>
        /// 预评总价
        /// </summary>
        [SQLReadOnly]
        public decimal? dytotalprice { get; set; }
        /// <summary>
        /// 预评税费
        /// </summary>
        [SQLReadOnly]
        public decimal? dytax { get; set; }
        /// <summary>
        /// 预评净值
        /// </summary>
        [SQLReadOnly]
        public decimal? dynetprice { get; set; }
        /// <summary>
        /// 报告单价
        /// </summary>
        [SQLReadOnly]
        public decimal? drunitprice { get; set; }
        /// <summary>
        /// 报告总价
        /// </summary>
        [SQLReadOnly]
        public decimal? drquerytotalprice { get; set; }
        /// <summary>
        /// 报告税费
        /// </summary>
        [SQLReadOnly]
        public decimal? drtax { get; set; }
        /// <summary>
        /// 报告净值
        /// </summary>
        [SQLReadOnly]
        public decimal? drnetprice { get; set; }

        #region 机构信息
        /// <summary>
        /// 机构名 
        /// </summary>
        [SQLReadOnly]
        public string qcustomercompanyname
        {
            get;
            set;
        }
        /// <summary>
        /// 机构id 
        /// </summary>
        [SQLReadOnly]
        public int? qcustomercompanyid
        {
            get;
            set;
        }
        /// <summary>
        /// 分支机构名 
        /// </summary>
        [SQLReadOnly]
        public string qbankbranchname
        {
            get;
            set;
        }
        /// <summary>
        /// 分支机构id 
        /// </summary>
        [SQLReadOnly]
        public int? qbankbranchid
        {
            get;
            set;
        }
        /// <summary>
        /// 支行名 
        /// </summary>
        [SQLReadOnly]
        public string qbanksubbranchname
        {
            get;
            set;
        }
        /// <summary>
        /// 支行id 
        /// </summary>
        [SQLReadOnly]
        public int? qbanksubbranchid
        {
            get;
            set;
        }
        #endregion
    }

    [Serializable]
    /// <summary>
    /// 业务统计条目相关的变量
    /// </summary>
    public class DatEntrustTongJi : BaseTO
    {
        public int daifenpei { get; set; }
    }
}


