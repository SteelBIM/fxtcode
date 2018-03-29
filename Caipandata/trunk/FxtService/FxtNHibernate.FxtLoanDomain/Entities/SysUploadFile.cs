using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2014-01-20
 * 摘要: 新建实体类
 * 2014.02.26 修改人:李晓东
 *            新增:PageIndex,Count属性
 *            新增:贺黎亮  2014.06.11 添加BankProid,BankId属性
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities
{
    /// <summary>
    ///Sys_UploadFile
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Sys_UploadFile")]
    public class SysUploadFile : BaseTO
    {

        /// <summary>
        /// Id
        /// </summary>
        [SQLField("Id", EnumDBFieldUsage.PrimaryKey, true)]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 文件地址
        /// </summary>
        public virtual string FilePath
        {
            get;
            set;
        }
        /// <summary>
        /// 文件类型
        /// </summary>
        public virtual string FileType
        {
            get;
            set;
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public virtual int? FileSize
        {
            get;
            set;
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual string UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 执行完成页数
        /// </summary>
        public virtual int? PageIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 押品总数量
        /// </summary>
        public virtual int? Count
        {
            get;
            set;
        }
      
        /// <summary>
        /// 文件项目表Id
        /// </summary>
        public virtual int BankProid
        {
            get;
            set;
        }

        /// <summary>
        /// 银行公司Id
        /// </summary>
        public virtual int BankId
        {
            get;
            set;
        }

        

    }
}