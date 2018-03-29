using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Sys_QueryYPCol
    {
        private int _id;
        /// <summary>
        /// 询价补充信息（预估单）列名管理表
        /// </summary>
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _tid;
        /// <summary>
        /// 预估单模板ID
        /// </summary>
        public int tid
        {
            get { return _tid; }
            set { _tid = value; }
        }
        private string _cname;
        /// <summary>
        /// 列名
        /// </summary>
        public string cname
        {
            get { return _cname; }
            set { _cname = value; }
        }
        private string _showname;
        /// <summary>
        /// 在页面显示的名称
        /// </summary>
        public string showname
        {
            get { return _showname; }
            set { _showname = value; }
        }
        private string _ctype;
        /// <summary>
        /// 列的数据类型
        /// </summary>
        public string ctype
        {
            get { return _ctype; }
            set { _ctype = value; }
        }
        private int? _clegth;
        public int? clegth
        {
            get { return _clegth; }
            set { _clegth = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
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
        private int _companyid = 0;
        /// <summary>
        /// 客户ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _cvalues;
        /// <summary>
        /// 列的预设值，例如下拉框、复选框等，各值用逗号隔开
        /// </summary>
        public string cvalues
        {
            get { return _cvalues; }
            set { _cvalues = value; }
        }
        private int _isquerytablecol = 0;
        /// <summary>
        /// 是否询价表的列,委估对象的名称、单价、面积、总价、税费、净值 必须是询价表的列
        /// </summary>
        public int isquerytablecol
        {
            get { return _isquerytablecol; }
            set { _isquerytablecol = value; }
        }
        private string _querytablecol;
        /// <summary>
        /// 询价表对应的列名（非询价表的列也可以有对应列）
        /// </summary>
        public string querytablecol
        {
            get { return _querytablecol; }
            set { _querytablecol = value; }
        }
    }
}
