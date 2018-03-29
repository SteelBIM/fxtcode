using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace CDI.Models
{
    /// <summary>记录用户登录状态</summary>
    [Serializable]
    [DataObject]
    [Description("记录用户登录状态")]
    [BindIndex("Index_2", false, "Token")]
    [BindIndex("Index_3", false, "UserName")]
    [BindIndex("PK_USERLOGINSTATE", true, "ID")]
    [BindTable("[FxtProduct].[dbo].[UserLoginState]", Description = "记录用户登录状态", ConnName = "FxtProduct", DbType = DatabaseType.SqlServer)]
    public partial class UserLoginState : IUserLoginState
    {
        #region 属性
        private Int32 _ID;
        /// <summary>自动ID</summary>
        [DisplayName("自动ID")]
        [Description("自动ID")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "ID", "自动ID", null, "int", 10, 0, false)]
        public virtual Int32 ID
        {
            get { return _ID; }
            set { if (OnPropertyChanging(__.ID, value)) { _ID = value; OnPropertyChanged(__.ID); } }
        }

        private String _Token;
        /// <summary>用户Token</summary>
        [DisplayName("用户Token")]
        [Description("用户Token")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(2, "Token", "用户Token", null, "nvarchar(100)", 0, 0, true)]
        public virtual String Token
        {
            get { return _Token; }
            set { if (OnPropertyChanging(__.Token, value)) { _Token = value; OnPropertyChanged(__.Token); } }
        }

        private String _UserName;
        /// <summary>用户名</summary>
        [DisplayName("用户名")]
        [Description("用户名")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(3, "UserName", "用户名", null, "nvarchar(50)", 0, 0, true)]
        public virtual String UserName
        {
            get { return _UserName; }
            set { if (OnPropertyChanging(__.UserName, value)) { _UserName = value; OnPropertyChanged(__.UserName); } }
        }

        private String _ProductCode;
        /// <summary>关联产品</summary>
        [DisplayName("关联产品")]
        [Description("关联产品")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "ProductCode", "关联产品", null, "nvarchar(50)", 0, 0, true)]
        public virtual String ProductCode
        {
            get { return _ProductCode; }
            set { if (OnPropertyChanging(__.ProductCode, value)) { _ProductCode = value; OnPropertyChanged(__.ProductCode); } }
        }

        private DateTime _LoginDate;
        /// <summary>登录时间</summary>
        [DisplayName("登录时间")]
        [Description("登录时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(5, "LoginDate", "登录时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LoginDate
        {
            get { return _LoginDate; }
            set { if (OnPropertyChanging(__.LoginDate, value)) { _LoginDate = value; OnPropertyChanged(__.LoginDate); } }
        }

        private DateTime _ExpireDate;
        /// <summary>过期时间</summary>
        [DisplayName("过期时间")]
        [Description("过期时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(6, "ExpireDate", "过期时间", null, "datetime", 3, 0, false)]
        public virtual DateTime ExpireDate
        {
            get { return _ExpireDate; }
            set { if (OnPropertyChanging(__.ExpireDate, value)) { _ExpireDate = value; OnPropertyChanged(__.ExpireDate); } }
        }

        private String _CreateBy;
        /// <summary>创建人</summary>
        [DisplayName("创建人")]
        [Description("创建人")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(7, "CreateBy", "创建人", null, "nvarchar(50)", 0, 0, true)]
        public virtual String CreateBy
        {
            get { return _CreateBy; }
            set { if (OnPropertyChanging(__.CreateBy, value)) { _CreateBy = value; OnPropertyChanged(__.CreateBy); } }
        }

        private DateTime _CreateDT;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(8, "CreateDT", "创建时间", null, "datetime", 3, 0, false)]
        public virtual DateTime CreateDT
        {
            get { return _CreateDT; }
            set { if (OnPropertyChanging(__.CreateDT, value)) { _CreateDT = value; OnPropertyChanged(__.CreateDT); } }
        }
        #endregion

        #region 获取/设置 字段值
        /// <summary>
        /// 获取/设置 字段值。
        /// 一个索引，基类使用反射实现。
        /// 派生实体类可重写该索引，以避免反射带来的性能损耗
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case __.ID : return _ID;
                    case __.Token : return _Token;
                    case __.UserName : return _UserName;
                    case __.ProductCode : return _ProductCode;
                    case __.LoginDate : return _LoginDate;
                    case __.ExpireDate : return _ExpireDate;
                    case __.CreateBy : return _CreateBy;
                    case __.CreateDT : return _CreateDT;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Token : _Token = Convert.ToString(value); break;
                    case __.UserName : _UserName = Convert.ToString(value); break;
                    case __.ProductCode : _ProductCode = Convert.ToString(value); break;
                    case __.LoginDate : _LoginDate = Convert.ToDateTime(value); break;
                    case __.ExpireDate : _ExpireDate = Convert.ToDateTime(value); break;
                    case __.CreateBy : _CreateBy = Convert.ToString(value); break;
                    case __.CreateDT : _CreateDT = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得记录用户登录状态字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>自动ID</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户Token</summary>
            public static readonly Field Token = FindByName(__.Token);

            ///<summary>用户名</summary>
            public static readonly Field UserName = FindByName(__.UserName);

            ///<summary>关联产品</summary>
            public static readonly Field ProductCode = FindByName(__.ProductCode);

            ///<summary>登录时间</summary>
            public static readonly Field LoginDate = FindByName(__.LoginDate);

            ///<summary>过期时间</summary>
            public static readonly Field ExpireDate = FindByName(__.ExpireDate);

            ///<summary>创建人</summary>
            public static readonly Field CreateBy = FindByName(__.CreateBy);

            ///<summary>创建时间</summary>
            public static readonly Field CreateDT = FindByName(__.CreateDT);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得记录用户登录状态字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>自动ID</summary>
            public const String ID = "ID";

            ///<summary>用户Token</summary>
            public const String Token = "Token";

            ///<summary>用户名</summary>
            public const String UserName = "UserName";

            ///<summary>关联产品</summary>
            public const String ProductCode = "ProductCode";

            ///<summary>登录时间</summary>
            public const String LoginDate = "LoginDate";

            ///<summary>过期时间</summary>
            public const String ExpireDate = "ExpireDate";

            ///<summary>创建人</summary>
            public const String CreateBy = "CreateBy";

            ///<summary>创建时间</summary>
            public const String CreateDT = "CreateDT";

        }
        #endregion
    }

    /// <summary>记录用户登录状态接口</summary>
    public partial interface IUserLoginState
    {
        #region 属性
        /// <summary>自动ID</summary>
        Int32 ID { get; set; }

        /// <summary>用户Token</summary>
        String Token { get; set; }

        /// <summary>用户名</summary>
        String UserName { get; set; }

        /// <summary>关联产品</summary>
        String ProductCode { get; set; }

        /// <summary>登录时间</summary>
        DateTime LoginDate { get; set; }

        /// <summary>过期时间</summary>
        DateTime ExpireDate { get; set; }

        /// <summary>创建人</summary>
        String CreateBy { get; set; }

        /// <summary>创建时间</summary>
        DateTime CreateDT { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}