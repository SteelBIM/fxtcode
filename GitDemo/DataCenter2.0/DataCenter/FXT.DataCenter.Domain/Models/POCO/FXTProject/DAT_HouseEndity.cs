using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// 房号编辑Model
    /// </summary>
    public class DAT_HouseEndity
    {
        /// <summary>
        /// 添加房号
        /// </summary>
        public List<DAT_HouseOperate> addHouse { get; set; }
        /// <summary>
        /// 更新房号
        /// </summary>
        public List<DAT_HouseOperate> updateHouse { get; set; }
        /// <summary>
        /// 删除房号
        /// </summary>
        public List<DAT_HouseOperate> deleteHouse { get; set; }
    }
}
