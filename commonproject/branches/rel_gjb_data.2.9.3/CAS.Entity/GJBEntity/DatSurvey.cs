using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Survey:DatSurvey
    {
        /// <summary>
        /// 查勘对象名
        /// </summary>
        [SQLReadOnly]
        public string projectname { get; set; }
        /// <summary>
        /// 查勘状态
        /// </summary>
        [SQLReadOnly]
        public string statecodename { get; set; }

        /// <summary>
        /// 查勘对象类型
        /// </summary>
        [SQLReadOnly]
        public string typecodename { get; set; }

        /// <summary>
        /// 查勘员
        /// </summary>
        [SQLReadOnly]
        public string username { get; set; }

        /// <summary>
        /// 分支机构名称
        /// </summary>
        [SQLReadOnly]
        public string subcompanyname { get; set; }

        /// <summary>
        /// 分配人
        /// </summary>
        [SQLReadOnly]
        public string assignusername { get; set; }

        /// <summary>
        /// 查勘等级
        /// </summary>
        [SQLReadOnly]
        public string surveyclassname { get; set; }

        /// <summary>
        /// 询价ID
        /// </summary>
        [SQLReadOnly]
        public long queryid { get; set; }

        /// <summary>
        /// 客户全称
        /// </summary>
        [SQLReadOnly]
        public string customercompanyfullname { get; set; }
        /// <summary>
        /// 客户全称
        /// </summary>
        [SQLReadOnly]
        public int querystatecode { get; set; }
        /// <summary>
        /// 项目全称
        /// </summary>
        [SQLReadOnly]
        public string projectfullname { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        [SQLReadOnly]
        public int approvalstatus { get; set; }
        /// <summary>
        /// 业务员姓名
        /// </summary>
        [SQLReadOnly]
        public string workersname { get; set; }

        // Alex 增加  城市、行政区 片区 2015-09-16
        /// <summary>
        /// 城市
        /// </summary>
        [SQLReadOnly]
        public string provincename { get; set; }

        /// <summary>
        /// 行政区
        /// </summary>
        [SQLReadOnly]
        public string areaname { get; set; }

        /// <summary>
        /// 片区
        /// </summary>
        [SQLReadOnly]
        public string subareaname { get; set; }

        /// <summary>
        /// 询价编号
        /// </summary>
        [SQLReadOnly]
        public string querynumber { get; set; }
    }
}
