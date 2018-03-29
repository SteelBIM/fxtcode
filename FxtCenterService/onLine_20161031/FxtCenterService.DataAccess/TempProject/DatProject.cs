using CAS.Entity.BaseDAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.DataAccess.TempProject
{
    [Serializable]
    [TableAttribute("dbo.DH_Data_Project")]
    public class DatProject : BaseTO
    {
        /// <summary>
        /// 临时楼盘编号
        /// </summary>
        public int ProjectId{get;set;}
        /// <summary>
        /// 城市编号
        /// </summary>
        public int CityID{get;set;}
        /// <summary>
        /// 区域编号
        /// </summary>
        public int AreaID{get;set;}
        /// <summary>
        /// 临时楼盘名称
        /// </summary>
        public string ProjectName{get;set;}
        public string othername	{get;set;}
        /// <summary>
        /// 拼音
        /// </summary>
        public string PinYin{get;set;}
        /// <summary>
        /// 地址
        /// </summary>
        public string Address{get;set;}
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark{get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime{get;set;}
        public string macaddress{get;set;}
        /// <summary>
        /// 1有效 0删除
        /// </summary>
        public int Valid{get;set;}
        public string areaname{get;set;}
        /// <summary>
        /// 数据中心楼盘编号
        /// </summary>
        public int FxtProjectId{get;set;}
        /// <summary>
        /// 数据中心楼盘名称
        /// </summary>
        public string FxtProjectName{get;set;}
        public int NewProjectId{get;set;}
    }
}
