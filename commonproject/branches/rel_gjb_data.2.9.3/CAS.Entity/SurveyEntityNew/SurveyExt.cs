using System;
using System.Collections.Generic;
using CAS.Entity.BaseDAModels;
using CAS.Entity.SurveyDBEntity;
using System.Linq;

namespace CAS.Entity.SurveyEntityNew
{
    /// <summary>
    /// 查勘库查勘扩展实体类
    /// </summary>
    public class SurveyExt : DatSurvey
    {
        /// <summary>
        /// 查勘类型 1026
        /// </summary>
        public string typecodename { get; set; }

        /// <summary>
        /// 查勘员姓名
        /// </summary>
        public string usertruename { get; set; }
        /// <summary>
        /// 客户经理姓名
        /// </summary>
        public string workertruename { get; set; }
        /// <summary>
        /// 分配人姓名
        /// </summary>
        public string assigntruename { get; set; }
        /// <summary>
        /// 查勘等级
        /// </summary>
        public string surveyclassname { get; set; }
        /// <summary>
        /// 录入人姓名
        /// </summary>
        public string createtruename { get; set; }
        /// <summary>
        /// 撤销人姓名
        /// </summary>
        public string canceltruename { get; set; }
        public string areaname { get; set; }
        public int imagesnumber { get; set; }
        public int videonumber { get; set; }
        //public int gps { get; set; }
        /// <summary>
        /// 图片列表
        /// </summary>
        public List<DatFiles> images { get; set; }

        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string projectname { get; set; }
        /// <summary>
        /// 同楼盘数据
        /// </summary>
        public int projectcount { get; set; }
        [SQLReadOnly]
        public string buildingname { get; set; }
        [SQLReadOnly]
        public string floornumber { get; set; }
        [SQLReadOnly]
        public string housename { get; set; }
        [SQLReadOnly]
        public string surveyusermobile { get; set; }
        /// <summary>
        /// 签到时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? signtime { get; set; }

        /// <summary>
        /// 查勘类型 Java查勘返回字段
        /// </summary>
        [SQLReadOnly]
        public string typeName { get; set; }
        /// <summary>
        /// 询价编号 Alex 2016-04-22
        /// </summary>
        [SQLReadOnly]
        public string queryid { get; set; }
        ///// <summary>
        ///// 业务编号链接 Alex 2016-04-22
        ///// </summary>
        //[SQLReadOnly]
        //public string linkentrustid { get; set; }
        /// <summary>
        /// 单套询价id  Alex 2016-04-25
        /// </summary>
        [SQLReadOnly]
        public long? qid { get; set; }
        /// <summary>
        /// 人工询价id  Alex 2016-04-25
        /// </summary>
        [SQLReadOnly]
        public long? moreqid { get; set; }
          
        /// <summary>
        /// 业务id  Alex 2016-04-25
        /// </summary>
        [SQLReadOnly]
        public long? eqid { get; set; }
        /// <summary>
        /// 询价来源  Alex 2016-04-25
        /// </summary>
        [SQLReadOnly]
        public string source { get; set; }
        /// <summary>
        /// 询价类别 Alex 2016-04-25
        /// </summary>
        [SQLReadOnly]
        public int? querytypecode { get; set; }
        /// <summary>
        /// 是否为估价宝业务编号 Alex 2016-05-16
        /// </summary>
        [SQLReadOnly]
        public int? isgjbentrust { get; set; }

        /// <summary>
        /// 当前环节
        /// </summary>
        [SQLReadOnly]
        public string detailstatus { get; set; }
        /// <summary>
        /// 当前处理人
        /// </summary>
        [SQLReadOnly]
        public string currenthandleruser { get; set; }
        /// <summary>
        /// 业务编号字符串类型，用于前台显示
        /// </summary>
        [SQLReadOnly]
        public string entrustidstr { get; set; }
        /// <summary>
        /// 查勘状态名称
        /// </summary>
        [SQLReadOnly]
        public string statecodename { get; set; }

        /// <summary>
        /// 评估目的
        /// </summary>
        [SQLReadOnly]
        public string estimatePurpose { get; set; }
        /// <summary>
        /// 询价、业务状态 Alex 2016-10-14 
        /// </summary>
        [SQLReadOnly]
        public int entruststatecode { get; set; }
        /// <summary>
        /// 业务创建人 Alex 2016-11-09
        /// </summary>
        [SQLReadOnly]
        public string businesscreateid { get; set; }
        /// <summary>
        /// 业务创建时间 Alex 2016-11-09
        /// </summary>
 
        [SQLReadOnly]
        public string businesscreatedate { get;set; }
        /// <summary>
        /// 询价创建人 Alex 2016-11-09
        /// </summary>
        [SQLReadOnly]
        public string enquirycreateid { get; set; }
        /// <summary>
        /// 询价创建时间 Alex 2016-11-09
        /// </summary>
        [SQLReadOnly]
        public string enquirycreatedate { get; set; }
        /// <summary>
        /// 业务询价创建人姓名 Alex 2016-11-22
        /// </summary>
        [SQLReadOnly]
        public string businesscreateusername { get; set; }
        /// <summary>
        /// 询价和业务创建时间 Alex 2016-11-22
        /// </summary>
        [SQLReadOnly]
        public string businessenquirydate { get; set; }
        /// <summary>
        /// 询价删除标识 Alex 2017-01-20
        /// </summary>
        [SQLReadOnly]
        public int queryvalid { get; set; }
    }
}
