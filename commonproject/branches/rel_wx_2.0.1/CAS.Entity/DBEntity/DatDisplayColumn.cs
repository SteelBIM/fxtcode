using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_DisplayColumn")]
    public class DatDisplayColumn : BaseTO
    {
        private int _displayid;
        [SQLField("displayid", EnumDBFieldUsage.PrimaryKey)]
        public int displayid
        {
            get { return _displayid; }
            set { _displayid = value; }
        }
        private int _code;
        /// <summary>
        /// SYS_Code_Id:2018
        /// </summary>
        public int code
        {
            get { return _code; }
            set { _code = value; }
        }
        private string _displayname;
        public string displayname
        {
            get { return _displayname; }
            set { _displayname = value; }
        }
        private int? _displaywidth;
        public int? displaywidth
        {
            get { return _displaywidth; }
            set { _displaywidth = value; }
        }
        private string _displayalign;
        public string displayalign
        {
            get { return _displayalign; }
            set { _displayalign = value; }
        }
        private string _propertyname;
        /// <summary>
        /// 属性名称,注意值是小写。
        /// </summary>
        public string propertyname
        {
            get { return _propertyname; }
            set { _propertyname = value; }
        }
        private string _process;
        /// <summary>
        /// 客户端显示列的JS处理方法
        /// </summary>
        public string process
        {
            get { return _process; }
            set { _process = value; }
        }
        private string _querycolumn;
        /// <summary>
        /// 查询列，带入SQL语句中的。
        /// </summary>
        public string querycolumn
        {
            get { return _querycolumn; }
            set { _querycolumn = value; }
        }
        private bool _isneed = false;
        /// <summary>
        /// 是否是必要支持系统运行下去的属性。如果为必须则不显示在客户端的列表中，只是作为查询时，返回的数据。
        /// </summary>
        public bool isneed
        {
            get { return _isneed; }
            set { _isneed = value; }
        }
        private string _format;
        public string format
        {
            get { return _format; }
            set { _format = value; }
        }
    }
}