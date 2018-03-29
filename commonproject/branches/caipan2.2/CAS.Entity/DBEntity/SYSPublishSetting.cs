using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_PublishSetting")]
    public class SYSPublishSetting : BaseTO
    {
        private int _id;
        /// <summary>
        /// 主键
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long _functioncode;
        /// <summary>
        /// 功能code 2017
        /// </summary>
        public long functioncode
        {
            get { return _functioncode; }
            set { _functioncode = value; }
        }
        private bool _isenable=false;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool isenable
        {
            get { return _isenable; }
            set { _isenable = value; }
        }
        private string _functiondesc;
        /// <summary>
        /// 功能描述
        /// </summary>
        public string functiondesc
        {
            get { return _functiondesc; }
            set { _functiondesc = value; }
        }		
        private bool? _issystemmust;
        public bool? issystemmust
        {
            get { return _issystemmust; }
            set { _issystemmust = value; }
        }
    }
}