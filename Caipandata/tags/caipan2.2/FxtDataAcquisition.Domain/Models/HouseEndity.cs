using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain.Models
{
    /// <summary>
    /// 房号编辑Model
    /// </summary>
    public class HouseEndity
    {
        /// <summary>
        /// 添加房号
        /// </summary>
        public List<HouseOperate> addHouse { get; set; }
        /// <summary>
        /// 更新房号
        /// </summary>
        public List<HouseOperate> updateHouse { get; set; }
        /// <summary>
        /// 删除房号
        /// </summary>
        public List<HouseOperate> deleteHouse { get; set; }
    }
}
