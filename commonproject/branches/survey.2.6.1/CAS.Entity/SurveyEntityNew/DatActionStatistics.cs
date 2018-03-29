using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyEntityNew
{
    /// <summary>
    /// 手机端云查勘行为统计类
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_ActionStatistics")]
    public class DatActionStatistics : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long? _sid;
        /// <summary>
        /// 查勘id
        /// </summary>
        public long? sid
        {
            get { return _sid; }
            set { _sid = value; }
        }
        private int? _fxtcompanyid;
        /// <summary>
        /// 公司id
        /// </summary>
        public int? fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _userid;
        /// <summary>
        /// 查勘员登录名
        /// </summary>
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private string _actionjson;
        /// <summary>
        /// 行为数据
        /// </summary>
        public string actionjson
        {
            get { return _actionjson; }
            set { _actionjson = value; }
        }
    }

}
