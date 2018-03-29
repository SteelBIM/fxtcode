using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// 楼栋相关公司表
    /// </summary>
    public class LNK_B_Company
    {

        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 楼栋相关公司表
        /// </summary>
        public long BuildingId { get; set; }
        /// <summary>
        /// 公司类型
        /// </summary>
        public long CompanyId { get; set; }
        /// <summary>
        /// 公司角色
        /// </summary>
        public int CompanyType { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public int FxtCompanyId { get; set; }

    }
}
