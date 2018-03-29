using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterServiceOpen.Actualize
{
    /// <summary>
    /// 通过代理方法名查找实际方法
    /// 为了防止代理方法名冲突，key存放在常量里
    /// </summary>
    public class MethodDictionary
    {
        private const string ghnlfvq = "ghnlfvq";//获取楼层列表forVQ
        private const string ghlfvq = "ghlfvq";//获取房号列表forVQ
        private const string gblfvq = "gblfvq";//获取楼栋列表forVQ
        private const string gpafvq = "gpafvq";//楼盘自动估价forVQ
        private const string gplfvq = "gplfvq";//获取楼盘列表forVQ
        private const string ghafvq = "ghafvq";//房号自动估价forVQ

        private const string provincelist = "provincelist";//省份列表
        private const string citylist = "citylist";//获取城市列表 

        private const string ghnlfout = "ghnlfout";//获取楼层列表forOUT

        private const string splistforcasout = "splistforcasout";//获取楼盘列表ForCAS_OUT
        private const string buildinglistforcasout = "buildinglistforcasout";//获取楼栋下拉列表ForCAS_OUT
        private const string housefloorlistforcasout = "housefloorlistforcasout";//获取楼层列表ForCAS_OUT
        private const string houselistforcasout = "houselistforcasout";//获取房号列表ForCAS_OUT
        private const string casautopriceforcasout = "casautopriceforcasout";//自动估价，不往数据中心插入自动估价记录ForCAS_OUT
        /// <summary>
        /// 存放字典
        /// </summary>
        public static readonly Dictionary<string, string> MethodDic = new Dictionary<string, string>
            {
                //方法注释 QueryFromNoInternal  
                {ghnlfvq,"GetHouseNoList_MCAS_ForVQ"},//20151012
                {ghlfvq,"GetHouseDropDownList_MCAS_ForVQ"},//20151012
                {gblfvq,"GetBuildingBaseInfoList_MCAS_ForVQ"},//20151012
                {gpafvq,"GetMCASProjectAutoPrice_ForVQ"},//20151012
                {gplfvq,"GetProjectDropDownList_MCAS_ForVQ"},//20151012
                {ghafvq,"GetMCASHouseAutoPrice_ForVQ"},//20151012

                {provincelist,"GetProvinceList"},
                {citylist,"GetSYSCityList"},
                {ghnlfout,"GetHouseNoList_MCAS_ForOUT"},//20160317 tanql

                {splistforcasout,"GetProjectListByKey_ForCAS_OUT"},//20160329 tanql
                {buildinglistforcasout,"GetBuildingList_ForCAS_OUT"},//20160329 tanql
                {housefloorlistforcasout,"GetHouseNoList_ForCAS_OUT"},//20160329 tanql
                {houselistforcasout,"GetHouseList_ForCAS_OUT"},//20160329 tanql
                {casautopriceforcasout,"GetCASEValueByPId_ForCAS_OUT"},//20160329 tanql

            };
    }
}
