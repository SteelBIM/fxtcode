using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.SurveyEntityNew
{
    /// <summary>
    /// 查勘添加实体
    /// </summary>
    public class SurveyAddEntity
    {
        public int cityid { get; set; }
        public int typecode { get; set; }
        public string names { get; set; }
        public string address { get; set; }
        public string contactname { get; set; }
        public int surveyclass { get; set; }
        public long sid { get; set; }
        /// <summary>
        /// 查勘员id
        /// </summary>
        public string userid { get; set; }
        public int? areaid { get; set; }
        public string contactphone { get; set; }
        public string bankcompanyname { get; set; }
        public string bankdepartmentname { get; set; }
        public string bankqueryuser { get; set; }
        public string bankphone { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string workers { get; set; }
        public string workersname { get; set; }

        public string workersphone { get; set; }
        public string remarks { get; set; }
        public decimal? buildingarea { get; set; }
        public long entrustid { get; set; }
        public string entrust { get; set; }
        public long objectid { get; set; }
        public string projectname { get; set; }
        public string buildingname { get; set; }
        public string floornumber { get; set; }
        public string housename { get; set; }
        public int? phototemplateid { get; set; }
        public int subcompanyid { get; set; }
        public string subcompanyname { get; set; }
        public string username { get; set; }
        public string areaname { get; set; }
        public string createusername { get; set; }

        public string assignusername { get; set; }
        public string assignuserid { get; set; }
        public string assigndate { get; set; }

        public string cancelusername { get; set; }
        public int? templateid { get; set; }
        public List<mobileFields> customfields { get; set; }//List<mobileFields>
        public double? x { get; set; }
        public double? y { get; set; }
        public double? loclng { get; set; }
        public double? loclat { get; set; }
        public string nettype { get; set; }
        public int statecode { get; set; }
        /// <summary>
        /// 查勘用户电话
        /// </summary>
        public string surveyusermobile { get; set; }
        /// <summary>
        /// 分配人电话
        /// </summary>
        public string assignphone { get; set; }

        /// <summary>
        /// 开始查勘时间
        /// </summary>
        public DateTime? begintime { get; set; }


        public DateTime? completetime { get; set; }

        public int systypecode { get; set; }
        /// <summary>
        /// 查勘备注
        /// </summary>
        public string surveyremarks { get; set; }

    }
}
