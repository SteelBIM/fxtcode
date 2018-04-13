using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    [Auditable]
    [Table("MarketClassify")]
    public class MarketClassify
    {
        public int MarketClassifyID
        {
            get;
            set;
        }

        /// <summary>
        /// 自定义市场分类名称，如果使用MOD名称，值为空
        /// </summary>
        public string MarketClassifyName
        {
            get;
            set;
        }

        /// <summary>
        /// 市场分类属性，如学科\版本\年级\册别\学段
        /// </summary>
        public string MarketClassifyProperty
        {
            get; set;
        }

        /// <summary>
        /// 市场类型ID,1同步教材,2老MOD同步教材
        /// </summary>
        public int? MarketID
        {
            get; set;
        }

        /// <summary>
        /// 父级分类ID
        /// </summary>
        public int? ParentId
        {
            get; set;
        }

        /// <summary>
        /// MOD类型，1学科2版本3年级4册别5学段
        /// </summary>
        public int? MODType
        {
            get;
            set;
        }

        /// <summary>
        /// 对应MODID
        /// </summary>
        public long? MODID
        {
            get; set;
        }
        /// <summary>
        /// MOD名称
        /// </summary>
        public string ModClassifyName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate
        {
            get; set;
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public int? CreateUser
        {
            get; set;
        }
    }
}
