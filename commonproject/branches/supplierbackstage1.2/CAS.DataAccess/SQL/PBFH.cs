using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.DataAccess.SQL
{
    /// <summary>
    /// Project - Building - Floor - House 联动功能的SQL语句 kevin 2013-3-21
    /// </summary>
    public class PBFH
    {
        /// <summary>
        /// 楼盘下拉列表
        /// </summary>
        public const string ProjectDropDownList = @"
                select top @top projectid,projectname,address,saleprice,x,y,averageprice
                ,areaname=(select areaname from dbo.sys_area where areaid= p.areaid)
                ,subareaname=(select subareaname from dbo.sys_subarea where subareaid=p.subareaid)
                from @projecttable as p
                where valid=1 and cityid=@cityid and fxtcompanyid=@fxtcompanyid";
        /// <summary>
        /// 楼盘基本信息
        /// </summary>
        public const string ProjectBaseInfo = "";
        /// <summary>
        /// 楼栋下拉列表
        /// </summary>
        public const string BuildingDropDownList = @"
                select buildingid,buildingname
                from @buildingtable as b
                where valid=1 and cityid=@cityid and fxtcompanyid=@fxtcompanyid and projectid=@projectid";
        /// <summary>
        /// 楼栋基本信息
        /// </summary>
        public const string BuildingBaseInfo = "";
        /// <summary>
        /// 楼层下拉列表
        /// </summary>
        public const string FloorDropDownList = "";
        /// <summary>
        /// 房号下拉列表
        /// </summary>
        public const string HouseDropDownList = "";
        /// <summary>
        /// 房号基本信息
        /// </summary>
        public const string HouseBaseInfo = "";
    }
}
