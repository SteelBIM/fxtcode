using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Knowledge_SubjectType")]
    public class DatKnowledgeSubjectType : BaseTO
    {
        /// <summary>
        /// 主题类
        /// </summary>
        private int _typecode = 0;
        [SQLField("typecode", EnumDBFieldUsage.PrimaryKey,true)]
        public int typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private string _typename = "";
        /// <summary>
        /// 主题类名
        /// </summary>
        public string typename
        {
            get { return _typename; }
            set { _typename = value; }
        }
        private int? _parentcode;
        /// <summary>
        /// 父级id
        /// </summary>
        public int? parentcode
        {
            get { return _parentcode; }
            set { _parentcode = value; }
        }
        private string _description;
        /// <summary>
        /// 说明
        /// </summary>
        public string description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}
