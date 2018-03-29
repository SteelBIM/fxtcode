using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Estimates")]
    public class DatEstimates : BaseTO
    {
        private int _estimatesid;
        /// <summary>
        /// 测算记录ID
        /// </summary>
        [SQLField("estimatesid", EnumDBFieldUsage.PrimaryKey, true)]
        public int estimatesid
        {
            get { return _estimatesid; }
            set { _estimatesid = value; }
        }
        private int _valuationmethodcode;
        public int valuationmethodcode
        {
            get { return _valuationmethodcode; }
            set { _valuationmethodcode = value; }
        }
        private decimal _unitprice;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }
        private int _percentage;
        /// <summary>
        /// 权重、比例
        /// </summary>
        public int percentage
        {
            get { return _percentage; }
            set { _percentage = value; }
        }
        private long _excelfileid;
        /// <summary>
        /// 测算表附件ID
        /// </summary>
        public long excelfileid
        {
            get { return _excelfileid; }
            set { _excelfileid = value; }
        }
        private long? _businessid;
        /// <summary>
        /// 具体业务对象的ID
        /// </summary>
        public long? businessid
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
        private int? _businesstypecode;
        /// <summary>
        /// 业务对象类型如：预评、报告
        /// </summary>
        public int? businesstypecode
        {
            get { return _businesstypecode; }
            set { _businesstypecode = value; }
        }
        private long? _entrustid;
        public long? entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private bool _valid = true;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int? _createfilebusinesstypecode;
        /// <summary>
        /// 附件文件所创建的环节
        /// </summary>
        public int? createfilebusinesstypecode
        {
            get { return _createfilebusinesstypecode; }
            set { _createfilebusinesstypecode = value; }
        }

        /// <summary>
        /// 新增标识
        /// </summary>
        public string newaddflag { get; set; }
    }
}
