using CAS.Entity.BaseDAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.DataAccess.TempProject
{
    [Serializable]
    [TableAttribute("dbo.DH_Data_Building")]
    public class DatBuilding : BaseTO
    {
        /// <summary>
        /// 楼栋编号
        /// </summary>
        public int BuildingId{get;set;}
        /// <summary>
        /// 城市编号
        /// </summary>
        public int CityID{get;set;}
        /// <summary>
        /// 临时楼栋名称
        /// </summary>
        public string BuildingName{get;set;}
        /// <summary>
        /// 临时楼盘编号
        /// </summary>
        public int ProjectId{get;set;}
        /// <summary>
        /// 正式库楼盘编号
        /// </summary>
        public int FxtProjectId{get;set;}
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark{get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime{get;set;}
        public string macaddress{get;set;}
        public int Valid{get;set;}
        public string pinyin{get;set;}
        /// <summary>
        /// 合并前的临时楼栋编号
        /// </summary>
        public int oldbuildingid{get;set;}
        /// <summary>
        /// 数据中心楼栋编号
        /// </summary>
        public int FxtBuildingId{get;set;}
        /// <summary>
        /// 数据中心楼栋名称
        /// </summary>
        public string FxtBuildingName { get; set; }
    }
}
