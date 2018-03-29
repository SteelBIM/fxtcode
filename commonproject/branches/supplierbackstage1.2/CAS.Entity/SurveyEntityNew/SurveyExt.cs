using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;
using CAS.Entity.SurveyDBEntity;

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
    }
}
