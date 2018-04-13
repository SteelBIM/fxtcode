using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    public class order_pinfo
    {

        public Guid? guid { get; set; }

        public Guid? o_guid { get; set; }

        public string o_pno { get; set; }

        /// <summary>
        /// 来源订单商品名称
        /// </summary>
        public string p_name { get; set; }

        /// <summary>
        /// 来源订单商品的科目ID
        /// </summary>
        public int? p_subjectid { get; set; }

        /// <summary>
        /// 来源订单商品的科目
        /// </summary>
        public string p_subject { get; set; }

        /// <summary>
        /// 来源订单年级ID
        /// </summary>
        public int? gradeid { get; set; }

        /// <summary>
        /// 来源订单年级名称
        /// </summary>
        public string gradename { get; set; }

        /// <summary>
        /// 来源订单商品的版本ID
        /// </summary>
        public int? p_versionid { get; set; }

        /// <summary>
        /// 来源订单商品的版本
        /// </summary>
        public string p_version { get; set; }

        /// <summary>
        /// 来源订单商品的类别ID
        /// </summary>
        public int? p_categorykey { get; set; }

        /// <summary>
        /// 来源订单商品的类别
        /// </summary>
        public string p_category { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal p_price { get; set; }

    }
}
