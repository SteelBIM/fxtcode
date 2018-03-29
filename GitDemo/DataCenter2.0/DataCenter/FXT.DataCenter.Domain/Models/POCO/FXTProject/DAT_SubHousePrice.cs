using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_SubHousePrice
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _projectid;
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private int _subhousetype;
        /// <summary>
        /// 附属房屋类型
        /// </summary>
        public int subhousetype
        {
            get { return _subhousetype; }
            set { _subhousetype = value; }
        }
        private decimal? _subhouseunitprice;
        /// <summary>
        /// 附属房屋单价
        /// </summary>
        [DisplayName("单价")]
        [RegularExpression(@"^[0-9]+\.[0-9]{1,2}|[0-9]+$", ErrorMessage = "{0}必须是数字类型(两位小数)")]
        public decimal? subhouseunitprice
        {
            get { return _subhouseunitprice; }
            set { _subhouseunitprice = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _creator;
        /// <summary>
        /// 创建人
        /// </summary>
        public string creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        private DateTime _savedate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime savedate
        {
            get { return _savedate; }
            set { _savedate = value; }
        }
        private string _saveuserid;
        public string saveuserid
        {
            get { return _saveuserid; }
            set { _saveuserid = value; }
        }


        #region 扩展字段
        /// <summary>
        /// 附属房名称
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// 附属房Code
        /// </summary>
        public int Code { get; set; }
        #endregion 

    }
}
