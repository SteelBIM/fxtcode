using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtProject
{
    /// <summary>
    /// 楼栋、房号总数
    /// </summary>
    public class DatProjectTotal: BaseTO
    {
        /// <summary>
        /// 楼栋总数
        /// </summary>
        public int buildingtotal { get; set; }
        /// <summary>
        /// 房号总数
        /// </summary>
        public int housetotal { get; set; }
    }
}
