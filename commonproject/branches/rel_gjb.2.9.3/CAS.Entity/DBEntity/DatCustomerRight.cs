using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_CustomerRight")]
    public class DatCustomerRight : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _rights;
        /// <summary>
        /// 功能权限
        /// </summary>
        public string rights
        {
            get { return _rights; }
            set { _rights = value; }
        }
        private string _rightname;
        /// <summary>
        /// 权限名称
        /// </summary>
        public string rightname
        {
            get { return _rightname; }
            set { _rightname = value; }
        }
        private string _rightbranch;
        /// <summary>
        /// 权限下的明细分支,逗号相隔如(只看自己,看本机构)
        /// </summary>
        public string rightbranch
        {
            get { return _rightbranch; }
            set { _rightbranch = value; }
        }
        private string _rightbind;
        /// <summary>
        /// 权限绑定,值为rights的值,多个用逗号隔开.单击权限,此项包含的其他项自动选上
        /// </summary>
        public string rightbind
        {
            get { return _rightbind; }
            set { _rightbind = value; }
        }
        private bool _iscascade = false;
        /// <summary>
        /// 子权限之间是否联动到上一级,如(只看自己,看本机构),勾选看本机构后 看自己自动勾上
        /// </summary>
        public bool iscascade
        {
            get { return _iscascade; }
            set { _iscascade = value; }
        }
    }
}