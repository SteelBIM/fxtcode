using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace CDI.Models
{
    /// <summary>可访问网卡地址表</summary>
    [Serializable]
    [DataObject]
    [Description("可访问网卡地址表")]
    [BindIndex("Index_2", false, "MAC")]
    [BindIndex("Index_3", false, "ProductCode")]
    [BindIndex("PK_MACADDRESS", true, "ID")]
    [BindTable("[FxtProduct].[dbo].[MacAddress]", Description = "可访问网卡地址表", ConnName = "FxtProduct", DbType = DatabaseType.SqlServer)]
    public partial class MacAddress : IMacAddress
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

        private String _Owner;
        /// <summary>拥有人</summary>
        [DisplayName("拥有人")]
        [Description("拥有人")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "Owner", "拥有人", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Owner
        {
            get { return _Owner; }
            set { if (OnPropertyChanging(__.Owner, value)) { _Owner = value; OnPropertyChanged(__.Owner); } }
        }

        private String _Mac;
        /// <summary>MAC地址</summary>
        [DisplayName("MAC地址")]
        [Description("MAC地址")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(3, "MAC", "MAC地址", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Mac
        {
            get { return _Mac; }
            set { if (OnPropertyChanging(__.Mac, value)) { _Mac = value; OnPropertyChanged(__.Mac); } }
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

        private String _CreateBy;
        /// <summary>创建人</summary>
        [DisplayName("创建人")]
        [Description("创建人")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "CreateBy", "创建人", null, "nvarchar(50)", 0, 0, true)]
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
        [BindColumn(6, "CreateDT", "创建时间", null, "datetime", 3, 0, false)]
        public virtual DateTime CreateDT
        {
            get { return _CreateDT; }
            set { if (OnPropertyChanging(__.CreateDT, value)) { _CreateDT = value; OnPropertyChanged(__.CreateDT); } }
        }

        private String _UpdateBy;
        /// <summary>修改人</summary>
        [DisplayName("修改人")]
        [Description("修改人")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(7, "UpdateBy", "修改人", null, "nvarchar(50)", 0, 0, true)]
        public virtual String UpdateBy
        {
            get { return _UpdateBy; }
            set { if (OnPropertyChanging(__.UpdateBy, value)) { _UpdateBy = value; OnPropertyChanged(__.UpdateBy); } }
        }

        private DateTime _UpdateDT;
        /// <summary>修改时间</summary>
        [DisplayName("修改时间")]
        [Description("修改时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(8, "UpdateDT", "修改时间", null, "datetime", 3, 0, false)]
        public virtual DateTime UpdateDT
        {
            get { return _UpdateDT; }
            set { if (OnPropertyChanging(__.UpdateDT, value)) { _UpdateDT = value; OnPropertyChanged(__.UpdateDT); } }
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
                    case __.Owner : return _Owner;
                    case __.Mac : return _Mac;
                    case __.ProductCode : return _ProductCode;
                    case __.CreateBy : return _CreateBy;
                    case __.CreateDT : return _CreateDT;
                    case __.UpdateBy : return _UpdateBy;
                    case __.UpdateDT : return _UpdateDT;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Owner : _Owner = Convert.ToString(value); break;
                    case __.Mac : _Mac = Convert.ToString(value); break;
                    case __.ProductCode : _ProductCode = Convert.ToString(value); break;
                    case __.CreateBy : _CreateBy = Convert.ToString(value); break;
                    case __.CreateDT : _CreateDT = Convert.ToDateTime(value); break;
                    case __.UpdateBy : _UpdateBy = Convert.ToString(value); break;
                    case __.UpdateDT : _UpdateDT = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得可访问网卡地址表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>自动ID</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>拥有人</summary>
            public static readonly Field Owner = FindByName(__.Owner);

            ///<summary>MAC地址</summary>
            public static readonly Field Mac = FindByName(__.Mac);

            ///<summary>关联产品</summary>
            public static readonly Field ProductCode = FindByName(__.ProductCode);

            ///<summary>创建人</summary>
            public static readonly Field CreateBy = FindByName(__.CreateBy);

            ///<summary>创建时间</summary>
            public static readonly Field CreateDT = FindByName(__.CreateDT);

            ///<summary>修改人</summary>
            public static readonly Field UpdateBy = FindByName(__.UpdateBy);

            ///<summary>修改时间</summary>
            public static readonly Field UpdateDT = FindByName(__.UpdateDT);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得可访问网卡地址表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>自动ID</summary>
            public const String ID = "ID";

            ///<summary>拥有人</summary>
            public const String Owner = "Owner";

            ///<summary>MAC地址</summary>
            public const String Mac = "Mac";

            ///<summary>关联产品</summary>
            public const String ProductCode = "ProductCode";

            ///<summary>创建人</summary>
            public const String CreateBy = "CreateBy";

            ///<summary>创建时间</summary>
            public const String CreateDT = "CreateDT";

            ///<summary>修改人</summary>
            public const String UpdateBy = "UpdateBy";

            ///<summary>修改时间</summary>
            public const String UpdateDT = "UpdateDT";

        }
        #endregion
    }

    /// <summary>可访问网卡地址表接口</summary>
    public partial interface IMacAddress
    {
        #region 属性
        /// <summary>自动ID</summary>
        Int32 ID { get; set; }

        /// <summary>拥有人</summary>
        String Owner { get; set; }

        /// <summary>MAC地址</summary>
        String Mac { get; set; }

        /// <summary>关联产品</summary>
        String ProductCode { get; set; }

        /// <summary>创建人</summary>
        String CreateBy { get; set; }

        /// <summary>创建时间</summary>
        DateTime CreateDT { get; set; }

        /// <summary>修改人</summary>
        String UpdateBy { get; set; }

        /// <summary>修改时间</summary>
        DateTime UpdateDT { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}