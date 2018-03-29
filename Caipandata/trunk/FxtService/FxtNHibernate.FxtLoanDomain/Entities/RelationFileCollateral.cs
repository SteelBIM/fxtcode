using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2014-02-26
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities
{
    /// <summary>
    ///Relation_File_Collateral
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Relation_File_Collateral")]
    public class RelationFileCollateral : BaseTO
    {

        /// <summary>
        /// 押品附件与已标准化押品关系--自增 ID
        /// </summary>
        [SQLField("ID", EnumDBFieldUsage.PrimaryKey, true)]
        public virtual int ID
        {
            get;
            set;
        }
        /// <summary>
        /// 文件编号ID
        /// </summary>
        public virtual int UploadFileId
        {
            get;
            set;
        }
        /// <summary>
        /// 押品编号ID
        /// </summary>
        public virtual int CollateralId
        {
            get;
            set;
        }

    }
}