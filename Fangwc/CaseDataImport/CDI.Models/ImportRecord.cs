using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace CDI.Models
{
    /// <summary>ImportRecord</summary>
    /// <remarks></remarks>
    [Serializable]
    [DataObject]
    [Description("")]
    [BindTable("[FxtData_Case].[dbo].[ImportRecord]", Description = "", ConnName = "FxtData_Case", DbType = DatabaseType.SqlServer)]
    public partial class ImportRecord : IImportRecord
    {
        #region 属性
        private Int32 _ID;
        /// <summary></summary>
        [DisplayName("ID")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(1, "ID", "", null, "int", 10, 0, false)]
        public virtual Int32 ID
        {
            get { return _ID; }
            set { if (OnPropertyChanging(__.ID, value)) { _ID = value; OnPropertyChanged(__.ID); } }
        }

        private String _CityName;
        /// <summary></summary>
        [DisplayName("CityName")]
        [Description("")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "CityName", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String CityName
        {
            get { return _CityName; }
            set { if (OnPropertyChanging(__.CityName, value)) { _CityName = value; OnPropertyChanged(__.CityName); } }
        }

        private DateTime _CaseBeginDate;
        /// <summary></summary>
        [DisplayName("CaseBeginDate")]
        [Description("")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(3, "CaseBeginDate", "", null, "datetime", 3, 0, false)]
        public virtual DateTime CaseBeginDate
        {
            get { return _CaseBeginDate; }
            set { if (OnPropertyChanging(__.CaseBeginDate, value)) { _CaseBeginDate = value; OnPropertyChanged(__.CaseBeginDate); } }
        }

        private DateTime _CaseEndDate;
        /// <summary></summary>
        [DisplayName("CaseEndDate")]
        [Description("")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(4, "CaseEndDate", "", null, "datetime", 3, 0, false)]
        public virtual DateTime CaseEndDate
        {
            get { return _CaseEndDate; }
            set { if (OnPropertyChanging(__.CaseEndDate, value)) { _CaseEndDate = value; OnPropertyChanged(__.CaseEndDate); } }
        }

        private DateTime _ImportTime;
        /// <summary></summary>
        [DisplayName("ImportTime")]
        [Description("")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(5, "ImportTime", "", null, "datetime", 3, 0, false)]
        public virtual DateTime ImportTime
        {
            get { return _ImportTime; }
            set { if (OnPropertyChanging(__.ImportTime, value)) { _ImportTime = value; OnPropertyChanged(__.ImportTime); } }
        }

        private Int32 _ImportCaseNumber;
        /// <summary></summary>
        [DisplayName("ImportCaseNumber")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "ImportCaseNumber", "", null, "int", 10, 0, false)]
        public virtual Int32 ImportCaseNumber
        {
            get { return _ImportCaseNumber; }
            set { if (OnPropertyChanging(__.ImportCaseNumber, value)) { _ImportCaseNumber = value; OnPropertyChanged(__.ImportCaseNumber); } }
        }

        private Int32 _ExceptionCaseNumber;
        /// <summary></summary>
        [DisplayName("ExceptionCaseNumber")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "ExceptionCaseNumber", "", null, "int", 10, 0, false)]
        public virtual Int32 ExceptionCaseNumber
        {
            get { return _ExceptionCaseNumber; }
            set { if (OnPropertyChanging(__.ExceptionCaseNumber, value)) { _ExceptionCaseNumber = value; OnPropertyChanged(__.ExceptionCaseNumber); } }
        }

        private String _ImportUser;
        /// <summary></summary>
        [DisplayName("ImportUser")]
        [Description("")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(8, "ImportUser", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String ImportUser
        {
            get { return _ImportUser; }
            set { if (OnPropertyChanging(__.ImportUser, value)) { _ImportUser = value; OnPropertyChanged(__.ImportUser); } }
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
                    case __.CityName : return _CityName;
                    case __.CaseBeginDate : return _CaseBeginDate;
                    case __.CaseEndDate : return _CaseEndDate;
                    case __.ImportTime : return _ImportTime;
                    case __.ImportCaseNumber : return _ImportCaseNumber;
                    case __.ExceptionCaseNumber : return _ExceptionCaseNumber;
                    case __.ImportUser : return _ImportUser;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.CityName : _CityName = Convert.ToString(value); break;
                    case __.CaseBeginDate : _CaseBeginDate = Convert.ToDateTime(value); break;
                    case __.CaseEndDate : _CaseEndDate = Convert.ToDateTime(value); break;
                    case __.ImportTime : _ImportTime = Convert.ToDateTime(value); break;
                    case __.ImportCaseNumber : _ImportCaseNumber = Convert.ToInt32(value); break;
                    case __.ExceptionCaseNumber : _ExceptionCaseNumber = Convert.ToInt32(value); break;
                    case __.ImportUser : _ImportUser = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得ImportRecord字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary></summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary></summary>
            public static readonly Field CityName = FindByName(__.CityName);

            ///<summary></summary>
            public static readonly Field CaseBeginDate = FindByName(__.CaseBeginDate);

            ///<summary></summary>
            public static readonly Field CaseEndDate = FindByName(__.CaseEndDate);

            ///<summary></summary>
            public static readonly Field ImportTime = FindByName(__.ImportTime);

            ///<summary></summary>
            public static readonly Field ImportCaseNumber = FindByName(__.ImportCaseNumber);

            ///<summary></summary>
            public static readonly Field ExceptionCaseNumber = FindByName(__.ExceptionCaseNumber);

            ///<summary></summary>
            public static readonly Field ImportUser = FindByName(__.ImportUser);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得ImportRecord字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary></summary>
            public const String ID = "ID";

            ///<summary></summary>
            public const String CityName = "CityName";

            ///<summary></summary>
            public const String CaseBeginDate = "CaseBeginDate";

            ///<summary></summary>
            public const String CaseEndDate = "CaseEndDate";

            ///<summary></summary>
            public const String ImportTime = "ImportTime";

            ///<summary></summary>
            public const String ImportCaseNumber = "ImportCaseNumber";

            ///<summary></summary>
            public const String ExceptionCaseNumber = "ExceptionCaseNumber";

            ///<summary></summary>
            public const String ImportUser = "ImportUser";

        }
        #endregion
    }

    /// <summary>ImportRecord接口</summary>
    /// <remarks></remarks>
    public partial interface IImportRecord
    {
        #region 属性
        /// <summary></summary>
        Int32 ID { get; set; }

        /// <summary></summary>
        String CityName { get; set; }

        /// <summary></summary>
        DateTime CaseBeginDate { get; set; }

        /// <summary></summary>
        DateTime CaseEndDate { get; set; }

        /// <summary></summary>
        DateTime ImportTime { get; set; }

        /// <summary></summary>
        Int32 ImportCaseNumber { get; set; }

        /// <summary></summary>
        Int32 ExceptionCaseNumber { get; set; }

        /// <summary></summary>
        String ImportUser { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}