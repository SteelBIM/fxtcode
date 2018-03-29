using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDI.Models;
using Common.Logging;
using CDI.Utils;

namespace CDI.Client
{
    public class ValidatorUtils
    {
        private static readonly ILog logger = CurrentData.Instance.Logger;

        public static int IsValidProject(List<DataProject> list, List<SYS_ProjectMatch> networkNames, DataCase data, Dictionary<string, int> cacheDict)
        {
            int projectId = -1;
            if (string.IsNullOrWhiteSpace(data.ProjectName))
            {
                return projectId;
            }
            //原始数据
            string projectName = "";
            if (data.ProjectName.Length >= 3)
            {
                projectName = data.ProjectName.Substring(0, 3);
            }
            else
            {
                projectName = data.ProjectName;
            }

            if (cacheDict.ContainsKey(projectName))
            {
                return cacheDict[projectName];
            }

            string tmp_projectName = Chinese2Spell.Convert(projectName);   //ShenZhen
            string projectNamePinyinAll = tmp_projectName.ToUpper();  //SHENZHEN
            string projectNamePinyin = Chinese2Spell.getFirstLetter(tmp_projectName);  //SZ

            foreach (DataProject dp_item in list)
            {
                logger.Debug(dp_item.ProjectName + "\t" + projectName);
                //先比较拼音首字母是否等于
                logger.Debug(dp_item.PinYin + "\t" + projectNamePinyin);
                if (dp_item.PinYin.Contains(projectNamePinyin))
                {
                    logger.Debug(dp_item.PinYinAll + "\t" + projectNamePinyinAll);
                    if (dp_item.PinYinAll.Contains(projectNamePinyinAll))
                    {
                        projectId = dp_item.ProjectId;
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(dp_item.OtherName))
                {
                    if (dp_item.OtherPinyin.Contains(projectNamePinyin))
                    {
                        logger.Debug(dp_item.OtherPinyin + "\t" + projectNamePinyinAll);
                        if (dp_item.OtherPinyinAll.Contains(projectNamePinyinAll))
                        {
                            projectId = dp_item.ProjectId;
                            break;
                        }
                    }
                }
            }
            //匹配网络名称
            if (projectId == -1 && !string.IsNullOrWhiteSpace(data.ProjectName) && networkNames != null && networkNames.Count() > 0)
            {
                var r = networkNames.FirstOrDefault(p => p.NetName == data.ProjectName);
                if (r != null)
                    projectId = r.ProjectNameId;
            }
            cacheDict[projectName] = projectId;
            return projectId;
        }

        public static bool IsValidData(City item,
            DataCase data,
            int[] arrays,
            Dictionary<string, int> PurposeMap,
            Dictionary<string, int> FrontMap,
            Dictionary<string, int> BuildingTypeMap,
            Dictionary<string, int> HouseTypeMap,
            Dictionary<string, int> StructureMap,
            Dictionary<string, int> MoneyUnitInfoMap)
        {
            //添加验证数据合法性校验
            bool isValid = true;
            //案例时间
            if (data.CaseDate == null)
            {
                isValid = false;
            }
            // 建筑面积 般删除面积在15(不含)平米以下的案例, (北京，上海，广州，深圳)
            foreach (int item1 in arrays)
            {
                if (data.CityID == item1 &&
                    data.BuildingArea < 15)
                {
                    isValid = false;
                    break;
                }
            }
            //单价
            if (data.UnitPrice <= 0 && data.TotalPrice<=0)
            {
                isValid = false;
            }
            // 总价
            if (isValid)
            {
                if (data.UnitPrice>0)
                {
                    data.TotalPrice = data.UnitPrice * data.BuildingArea;                    
                }
                else
                {
                    data.UnitPrice = data.TotalPrice / data.BuildingArea;                    
                }
            }
            // 案例类型 默认填充为“买卖报盘“
            //if (string.IsNullOrEmpty(data.CaseTypeName))
            //{
            //    isValid = false;
            //}
            //else
            //{
            data.CaseTypeCode = 3001001;
            //}
            //用途
            if (string.IsNullOrEmpty(data.PurposeName))
            {
                isValid = false;
            }
            else
            {
                foreach (KeyValuePair<string, int> map in PurposeMap)
                {
                    if (map.Key == data.PurposeName)
                    {
                        data.PurposeCode = map.Value;
                        break;
                    }
                }
                if (data.PurposeCode <= 0)
                {
                    isValid = false;
                }
            }

            //行政区
            foreach (KeyValuePair<string, int> map in item.AreaMap)
            {
                if (!string.IsNullOrEmpty(data.AreaName) &&
                    map.Key.StartsWith(data.AreaName))
                {
                    data.AreaId = map.Value;
                    data.AreaName = map.Key;
                    break;
                }
            }
            //楼层
            foreach (int d in arrays)
            {
                if ((data.CityID == d && data.FloorNumber > 80) ||
                    (data.CityID != d && data.FloorNumber > 60))
                {
                    data.FloorNumber = null;
                }

            }
            //总楼层
            foreach (int d in arrays)
            {
                if ((data.CityID == d && data.TotalFloor > 80) ||
                    (data.CityID != d && data.TotalFloor > 60))
                {
                    data.TotalFloor = null;
                }

            }

            if (data.TotalFloor < data.FloorNumber ||
                data.TotalFloor <= 0)
            {
                data.TotalFloor = null;
            }
            //朝向列
            foreach (KeyValuePair<string, int> map in FrontMap)
            {
                data.FrontCode = 0;
                if (!string.IsNullOrEmpty(data.FrontName) &&
                    map.Key==data.FrontName)
                {
                    data.FrontCode = map.Value;
                    break;
                }
            }
            //建筑类型
            if (!string.IsNullOrEmpty(data.PeiTao) &&
                data.FloorNumber == 8 &&
                data.PeiTao.Contains("电梯"))
            {
                data.BuildingTypeName = "小高层";
            }
            foreach (KeyValuePair<string, int> map in BuildingTypeMap)
            {
                if (!string.IsNullOrEmpty(data.BuildingTypeName) &&
                    (map.Key.StartsWith(data.BuildingTypeName) || map.Key.Contains(data.BuildingTypeName)))
                {
                    data.BuildingTypeCode = map.Value;
                    break;
                }
            }
            //户型
            foreach (KeyValuePair<string, int> map in HouseTypeMap)
            {
                if (!string.IsNullOrEmpty(data.HouseTypeName) &&
                    (map.Key.StartsWith(data.HouseTypeName) || map.Key.Contains(data.HouseTypeName)))
                {
                    data.HouseTypeCode = map.Value;
                    break;
                }
            }
            //户型结构
            if (!string.IsNullOrEmpty(data.StructureName))
            {
                foreach (KeyValuePair<string, int> map in StructureMap)
                {
                    if (map.Key.StartsWith(data.StructureName) ||
                        map.Key.Contains(data.StructureName))
                    {
                        data.StructureCode = map.Value;
                        break;
                    }
                }
            }
            //币种
            if (string.IsNullOrEmpty(data.MoneyUnitName))
            {
                data.MoneyUnitName = "人民币";
            }
            foreach (KeyValuePair<string, int> map in MoneyUnitInfoMap)
            {
                if (map.Key.Equals(data.MoneyUnitName) ||
                    map.Key.StartsWith(data.MoneyUnitName) ||
                    map.Key.Contains(data.MoneyUnitName))
                {
                    data.MoneyUnitCode = map.Value;
                    break;
                }
            }
            return isValid;
        }

    }
}
