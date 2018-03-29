using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_City_Table
    {
        private int _cityid;
        //[SQLField("cityid", EnumDBFieldUsage.PrimaryKey)]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _projecttable;
        public string projecttable
        {
            get { return _projecttable; }
            set { _projecttable = value; }
        }
        private string _buildingtable;
        public string buildingtable
        {
            get { return _buildingtable; }
            set { _buildingtable = value; }
        }
        private string _housetable;
        public string housetable
        {
            get { return _housetable; }
            set { _housetable = value; }
        }
        private string _casetable;
        public string casetable
        {
            get { return _casetable; }
            set { _casetable = value; }
        }
        private string _queryinfotable;
        public string queryinfotable
        {
            get { return _queryinfotable; }
            set { _queryinfotable = value; }
        }
        private string _reporttable;
        public string reporttable
        {
            get { return _reporttable; }
            set { _reporttable = value; }
        }
        private string _printtable;
        public string printtable
        {
            get { return _printtable; }
            set { _printtable = value; }
        }
        private string _historytable;
        public string historytable
        {
            get { return _historytable; }
            set { _historytable = value; }
        }
        private string _querytaxtable;
        public string querytaxtable
        {
            get { return _querytaxtable; }
            set { _querytaxtable = value; }
        }
        private string _casebusinesstable;
        public string casebusinesstable
        {
            get { return _casebusinesstable; }
            set { _casebusinesstable = value; }
        }
        private string _caselandtable;
        public string caselandtable
        {
            get { return _caselandtable; }
            set { _caselandtable = value; }
        }
        private string _queryadjusttable;
        public string queryadjusttable
        {
            get { return _queryadjusttable; }
            set { _queryadjusttable = value; }
        }
        private string _querytaxsoatable;
        public string querytaxsoatable
        {
            get { return _querytaxsoatable; }
            set { _querytaxsoatable = value; }
        }
        private string _queryflowtable;
        public string queryflowtable
        {
            get { return _queryflowtable; }
            set { _queryflowtable = value; }
        }
        private string _cashistorytable;
        public string cashistorytable
        {
            get { return _cashistorytable; }
            set { _cashistorytable = value; }
        }
        private string _messagetable;
        public string messagetable
        {
            get { return _messagetable; }
            set { _messagetable = value; }
        }
        private string _querychecktable;
        public string querychecktable
        {
            get { return _querychecktable; }
            set { _querychecktable = value; }
        }
        private string _reportchecktable;
        public string reportchecktable
        {
            get { return _reportchecktable; }
            set { _reportchecktable = value; }
        }
        private string _surveytable;
        public string surveytable
        {
            get { return _surveytable; }
            set { _surveytable = value; }
        }
        private string _surveybusinesstable;
        public string surveybusinesstable
        {
            get { return _surveybusinesstable; }
            set { _surveybusinesstable = value; }
        }
        private string _surveyfactorytable;
        public string surveyfactorytable
        {
            get { return _surveyfactorytable; }
            set { _surveyfactorytable = value; }
        }
        private string _surveylandtable;
        public string surveylandtable
        {
            get { return _surveylandtable; }
            set { _surveylandtable = value; }
        }
        private string _surveyofficetable;
        public string surveyofficetable
        {
            get { return _surveyofficetable; }
            set { _surveyofficetable = value; }
        }
        private string _surveyothertable;
        public string surveyothertable
        {
            get { return _surveyothertable; }
            set { _surveyothertable = value; }
        }
        private string _queryfilestable;
        public string queryfilestable
        {
            get { return _queryfilestable; }
            set { _queryfilestable = value; }
        }
        private string _surveytexttable;
        public string surveytexttable
        {
            get { return _surveytexttable; }
            set { _surveytexttable = value; }
        }
        private string _subhousepricetable;
        /// <summary>
        /// 附属房屋价格表
        /// </summary>
        public string subhousepricetable
        {
            get { return _subhousepricetable; }
            set { _subhousepricetable = value; }
        }
        private string _qprojecttable;
        public string qprojecttable
        {
            get { return _qprojecttable; }
            set { _qprojecttable = value; }
        }
        private string _reportentrusttable;
        public string reportentrusttable
        {
            get { return _reportentrusttable; }
            set { _reportentrusttable = value; }
        }
        private string _surveycasetable;
        public string surveycasetable
        {
            get { return _surveycasetable; }
            set { _surveycasetable = value; }
        }
        private string _searchhistorytable;
        public string searchhistorytable
        {
            get { return _searchhistorytable; }
            set { _searchhistorytable = value; }
        }
        private string _queryypdat;
        public string queryypdat
        {
            get { return _queryypdat; }
            set { _queryypdat = value; }
        }
        private string _queryypchecktable;
        public string queryypchecktable
        {
            get { return _queryypchecktable; }
            set { _queryypchecktable = value; }
        }
        #region 扩展字段
        /// <summary>
        /// 本机构可应用数据的机构ID，例如：25,101,102
        /// </summary>
        public string ShowCompanyId { get; set; }
        /// <summary>
        /// 本机构可应用数据的机构ID，例如：25,101,102
        /// </summary>
        public string CaseCompanyId { get; set; }
        #endregion

    }
}
