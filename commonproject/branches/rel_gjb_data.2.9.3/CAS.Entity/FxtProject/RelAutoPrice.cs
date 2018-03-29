using CAS.Entity.BaseDAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.FxtProject
{
    /// <summary>
    /// 押品复估结果
    /// </summary>
    public class RelAutoPrice
    {
        /// <summary>
        /// 单价
        /// </summary>
        public decimal unitprice { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        //public decimal totalprice { get; set; }
        /// <summary>
        /// 价格类型（0.不可估，1.楼盘基准均价，2.楼盘案例均价，3.关联楼盘）
        /// </summary>
        public int pricetype { get; set; }
        /// <summary>
        /// 关联楼盘
        /// </summary>
        public List<RelProject> relprojects { get; set; }

    }
    /// <summary>
    /// 关联楼盘
    /// </summary>
    public class RelProject : BaseTO
    {
        public int CityID { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 距离
        /// </summary>
        public int Distance { get; set; }
        /// <summary>
        /// 均价1
        /// </summary>
        public decimal ProjectAvgPrice1 { get; set; }
        /// <summary>
        /// 均价2
        /// </summary>
        public decimal ProjectAvgPrice2 { get; set; }
        /// <summary>
        /// 价格类型1（0.不可估，1.楼盘基准均价，2.楼盘案例均价）
        /// </summary>
        public int pricetype1 { get; set; }
        /// <summary>
        /// 价格类型2（0.不可估，1.楼盘基准均价，2.楼盘案例均价）
        /// </summary>
        public int pricetype2 { get; set; }
    }
}
