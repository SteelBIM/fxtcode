using System;
using CAS.Entity.BaseDAModels;
using CAS.Entity.AttributeHelper;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_QRcodeSetting")]
    public class DatQRcodeSetting : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int? _btfsid;
        public int? btfsid
        {
            get { return _btfsid; }
            set { _btfsid = value; }
        }
        private string _showname;
        /// <summary>
        /// 显示名称
        /// </summary>
        public string showname
        {
            get { return _showname; }
            set { _showname = value; }
        }
        private string _fieldvalues;
        /// <summary>
        /// 字段值(自定义)
        /// </summary>
        public string fieldvalues
        {
            get { return _fieldvalues; }
            set { _fieldvalues = value; }
        }
        private string _querycolumn;
        /// <summary>
        /// 字段值(系统查询sql)
        /// </summary>
        public string querycolumn
        {
            get { return _querycolumn; }
            set { _querycolumn = value; }
        }
        private int _biztype;
        /// <summary>
        /// 类型(报告,预评,0为通用类型)
        /// </summary>
        public int biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private bool _isshow = false;
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool isshow
        {
            get { return _isshow; }
            set { _isshow = value; }
        }
        private bool _issystemfield = false;
        /// <summary>
        /// 是否系统字段
        /// </summary>
        public bool issystemfield
        {
            get { return _issystemfield; }
            set { _issystemfield = value; }
        }
        private int _orderid = 0;
        /// <summary>
        /// 排序id
        /// </summary>
        public int orderid
        {
            get { return _orderid; }
            set { _orderid = value; }
        }
        private bool _valid = true;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }
    }
    /// <summary>
    /// 二维码显示字段
    /// </summary>
    public class QRcodeSettingShowValues
    {
        /// <summary>
        /// 显示字段
        /// </summary>
        public string showname { get; set; }
        /// <summary>
        /// 显示值
        /// </summary>
        public object fieldvalues { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }
    }
}
