using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("View_UserUserBookHistory")]
    public partial class View_UserUserBookHistory : Kingsun.DB.Action
    {
        [FieldAttribute("TelePhone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TelePhone { get; set; }

        [FieldAttribute("BookHistoryID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookHistoryID { get; set; }

        [FieldAttribute("UserUseBookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? UserUseBookID { get; set; }

        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserID { get; set; }

        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        [FieldAttribute("NickName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string NickName { get; set; }

        [FieldAttribute("UserImage", null, EnumFieldUsage.CommonField, DbType.Guid)]
        public Guid? UserImage { get; set; }

        [FieldAttribute("UserRoles", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? UserRoles { get; set; }

        [FieldAttribute("TrueName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TrueName { get; set; }

    }
}
