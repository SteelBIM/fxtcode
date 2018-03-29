using CAS.Entity.BaseDAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.DataAccess.TempProject
{
    [Serializable]
    [TableAttribute("dbo.DH_Data_House")]
    public class DatHouse : BaseTO
    {
        /// <summary>
        /// 临时房号编号
        /// </summary>
        public int HouseId{get;set;}
        /// <summary>
        /// 城市编号
        /// </summary>
        public int CityID{get;set;}
        /// <summary>
        /// 楼栋编号
        /// </summary>
        public int BuildingId{get;set;}
        /// <summary>
        /// 正式库楼栋编号
        /// </summary>
        public int FxtBuildingId{get;set;}
        /// <summary>
        /// 房号名称
        /// </summary>
        public string HouseName{get;set;}
        /// <summary>
        /// 楼层
        /// </summary>
        public string FloorNo{get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime{get;set;}
        /// <summary>
        /// 创建人
        /// </summary>
        public string MacAddress{get;set;}
        public int Valid{get;set;}
        public string pinyin{get;set;}
        /// <summary>
        /// 合并前的临时房号
        /// </summary>
        public int oldhouseid{get;set;}
        /// <summary>
        /// 数据中心房号编号
        /// </summary>
        public int FxtHouseId{get;set;}
        /// <summary>
        /// 数据中心房号
        /// </summary>
        public string FxtHouseName{get;set;}
    }
}
