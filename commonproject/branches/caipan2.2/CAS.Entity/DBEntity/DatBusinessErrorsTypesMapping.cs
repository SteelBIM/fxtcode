using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_BusinessErrors_TypesMapping")]
    public class DatBusinessErrorsTypesMapping : BaseTO
    {
        private int _mappingid;
        [SQLField("mappingid", EnumDBFieldUsage.PrimaryKey, true)]
        public int mappingid
        {
            get { return _mappingid; }
            set { _mappingid = value; }
        }
        private int _fk_businesserrorid;
        public int fk_businesserrorid
        {
            get { return _fk_businesserrorid; }
            set { _fk_businesserrorid = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private DateTime _createdon = DateTime.Now;
        public DateTime createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }
        private int? _nodetype;
        /// <summary>
        /// 节点类型：0互审、1初审、2二审、3三审、4终审、5盖章、6复印、7正常
        /// </summary>
        public int? nodetype
        {
            get { return _nodetype; }
            set { _nodetype = value; }
        }
        private int _businesstype = 3;
        /// <summary>
        /// 3为报告
        /// </summary>
        public int businesstype
        {
            get { return _businesstype; }
            set { _businesstype = value; }
        }
        private int? _subbusinesstype;
        /// <summary>
        /// 业务类型的 细分类型null 无 1、房地产 2、资产
        /// </summary>
        public int? subbusinesstype
        {
            get { return _subbusinesstype; }
            set { _subbusinesstype = value; }
        }
    }


}
