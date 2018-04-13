using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    public class cfg_product
    {
        /// <summary>
        /// ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 商品编号/商品ID
        /// 同步学 对应订单 CourseID
        /// 客户端 对应订单 ClassSn
        /// </summary>
        public string productno { get; set; }

        /// <summary>
        /// 产品渠道
        ///  1:同步学
        ///  2:C++客户端
        /// </summary>
        public int channel { get; set; }

        /// <summary>
        /// 科目id
        /// </summary>
        public int subjectid { get; set; }
        
        /// <summary>
        /// 科目
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 版本id
        /// </summary>
        public int versionid { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// 年级id
        /// </summary>
        public int gradeid { get; set; }

        /// <summary>
        /// 年级
        /// </summary>
        public string grade { get; set; }

        /// <summary>
        /// 商品类别
        /// 101 E-BOOK
        /// 102 趣配音
        /// 103 读单词
        /// 201 绘本
        /// 202 练习
        /// 203 阅读
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 类别键
        /// </summary>
        public int? categorykey { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string productname { get; set; }
        /// <summary>
        /// 是否上下架
        /// </summary>
        public int isshevel { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 删除标记 0-正常 1-删除
        /// </summary>
        public int delflg { get; set; }

    }
}
