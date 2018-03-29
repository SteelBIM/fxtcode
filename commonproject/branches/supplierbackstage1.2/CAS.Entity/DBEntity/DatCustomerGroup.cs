using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_CustomerGroup")]
    public class DatCustomerGroup : BaseTO
    {
        private int _groupid;
        /// <summary>
        /// 客户组ID
        /// </summary>
        [SQLField("groupid", EnumDBFieldUsage.PrimaryKey)]
        public int groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }
        private string _groupname;
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string groupname
        {
            get { return _groupname; }
            set { _groupname = value; }
        }
        private int _branchcompanyid;
        /// <summary>
        /// 分公司ID，0表示通用组
        /// </summary>
        public int branchcompanyid
        {
            get { return _branchcompanyid; }
            set { _branchcompanyid = value; }
        }
        private string _rights;
        /// <summary>
        /// 权限
        /// </summary>
        public string rights
        {
            get { return _rights; }
            set { _rights = value; }
        }
        private DateTime _createdate=DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private bool _valid;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int _productcode;
        /// <summary>
        /// 产品Code
        /// </summary>
        public int productcode
        {
            get { return _productcode; }
            set { _productcode = value; }
        }
        private string _showmenu;
        /// <summary>
        /// 控制菜单在不同账号的显示
        /// </summary>
        public string showmenu
        {
            get { return _showmenu; }
            set { _showmenu = value; }
        }
    }
}