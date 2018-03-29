using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CAS.Common.MVC4
{
    public class CodeHelper
    {
        /// <summary>
        /// 获取计算税费所需条件的codes
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static int[] TaxesTemplateCode(string condition)
        {
            int[] str = new int[5];
            switch (condition[0])
            {
                case '0':
                    str[0] = 4002001; break;
                case '1':
                    str[0] = 4002002; break;
                default:
                    str[0] = 4002003; break;
            }
            switch (condition[1])
            {
                case '0':
                    str[1] = 4003001; break;
                case '1':
                    str[1] = 4003002; break;
                default:
                    str[1] = 4003003; break;
            }
            switch (condition[2])
            {
                case '0':
                    str[2] = 4004001; break;
                case '1':
                    str[2] = 4004002; break;
                default:
                    str[2] = 4004003; break;
            }
            switch (condition[3])
            {
                case '0':
                    str[3] = 4005001; break;
                case '1':
                    str[3] = 4005002; break;
                default:
                    str[3] = 4005003; break;
            }
            switch (condition[4])
            {
                case '0':
                    str[4] = 4006001; break;
                case '1':
                    str[4] = 4006002; break;
                default:
                    str[4] = 4006003; break;
            }
            return str;
        }

        /// <summary>
        /// 根据建筑面积获取面积段CODE
        /// </summary>
        /// <param name="buildingarea">面积</param>
        /// <returns></returns>
        public static int GetBuildingAreaType(double buildingarea)
        {
            int buildingareatype = 0;
            if (buildingarea > 120)
            {
                buildingareatype = 8006005;
            }
            else if (buildingarea > 90 && buildingarea <= 120)
            {
                buildingareatype = 8006004;
            }
            else if (buildingarea > 60 && buildingarea <= 90)
            {
                buildingareatype = 8006003;
            }
            else if (buildingarea > 30 && buildingarea <= 60)
            {
                buildingareatype = 8006002;
            }
            else
            {
                buildingareatype = 8006001;
            }
            return buildingareatype;
        }

        /// <summary>
        /// 根据建筑面积获取面积段CODE
        /// </summary>
        /// <param name="buildingarea">面积</param>
        /// <returns></returns>
        public static string GetBuildingAreaTypeName(double buildingarea)
        {
            string buildingareatypename = "";
            if (buildingarea > 120)
            {
                buildingareatypename = "> 120";
            }
            else if (buildingarea > 90 && buildingarea <= 120)
            {
                buildingareatypename = "90-120";
            }
            else if (buildingarea > 60 && buildingarea <= 90)
            {
                buildingareatypename = "60-90";
            }
            else if (buildingarea > 30 && buildingarea <= 60)
            {
                buildingareatypename = "30-60";
            }
            else
            {
                buildingareatypename = "< 30";
            }
            return buildingareatypename;
        }
        
        /// <summary>
        /// 获取建筑类型Code
        /// </summary>
        /// <param name="totalfloor">总楼层</param>
        /// <returns></returns>
        public static int GetBuildingTypeCode(int totalfloor)
        {
            int buildingtypecode = 0;
            if (totalfloor <= 3)
            {
                buildingtypecode = 2003001; //低层
            }
            else if (totalfloor > 3 && totalfloor < 8)
            {
                buildingtypecode = 2003002; //多层
            }
            else if (totalfloor >= 8 && totalfloor <= 12)
            {
                buildingtypecode = 2003003;//小高层
            }
            else
            {
                buildingtypecode = 2003004;//高层
            }
            return buildingtypecode;
        }

        /// <summary>
        /// 获取建筑类型Code名称
        /// </summary>
        /// <param name="totalfloor">总楼层</param>
        /// <returns></returns>
        public static string GetBuildingTypeCodeName(int totalfloor)
        {
            string buildingtypecodename = "";
            if (totalfloor <= 3)
            {
                buildingtypecodename = "低层";
            }
            else if (totalfloor > 3 && totalfloor < 8)
            {
                buildingtypecodename = "多层";
            }
            else if (totalfloor >= 8 && totalfloor <= 12)
            {
                buildingtypecodename = "小高层";
            }
            else
            {
                buildingtypecodename = "高层";
            }
            return buildingtypecodename;
        }
    }
}
