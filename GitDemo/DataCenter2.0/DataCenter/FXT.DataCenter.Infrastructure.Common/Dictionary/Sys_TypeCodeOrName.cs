using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Infrastructure.Common.Dictionary
{
    public class Sys_TypeCodeOrName
    {
        /// <summary>
        /// 获取使用权性质code
        /// </summary>
        /// <param name="codeName">使用权性质名称</param>
        /// <returns></returns>
        public static int GetUseTypeCode(string codeName)
        {
            if (!string.IsNullOrEmpty(codeName.Trim()))
            {
                int code;
                switch (codeName)
                {
                    case "国家":
                        code = 6030001;
                        break;
                    case "集体":
                        code = 6030002;
                        break;
                    default:
                        code = -1;
                        break;
                }
                return code;
            }
            return -1;
        }

        /// <summary>
        /// 获取使用权性质名称
        /// </summary>
        /// <param name="code">使用权性质code</param>
        /// <returns></returns>
        public static string GetUseTypeName(int code)
        {
            if (code > 0)
            {
                string Name;
                switch (code)
                {
                    case 6030001:
                        Name = "国家";
                        break;
                    case 6030002:
                        Name = "集体";
                        break;
                    default:
                        Name = "";
                        break;
                }
                return Name;
            }
            return "";
        }
        /// <summary>
        /// 获取使用权类型code
        /// </summary>
        /// <param name="codeName">使用权类型名称</param>
        /// <returns></returns>
        public static int GetLandTypeCode(string codeName)
        {
            if (!string.IsNullOrEmpty(codeName.Trim()))
            {
                int code;
                switch (codeName)
                {
                    case "政府出让":
                        code = 3002001;
                        break;
                    case "企业转让":
                        code = 3002002;
                        break;
                    case "行政划拨":
                        code = 3002003;
                        break;
                    case "旧城改造":
                        code = 3002004;
                        break;
                    default:
                        code = -1;
                        break;
                }
                return code;
            }
            return -1;
        }

        /// <summary>
        /// 获取使用权类型名称
        /// </summary>
        /// <param name="code">使用权类型code</param>
        /// <returns></returns>
        public static string GetLandTypeName(int code)
        {
            if (code > 0)
            {
                string codeName;
                switch (code)
                {
                    case 3002001:
                        codeName = "政府出让";
                        break;
                    case 3002002:
                        codeName = "企业转让";
                        break;
                    case 3002003:
                        codeName = "行政划拨";
                        break;
                    case 3002004:
                        codeName = "旧城改造";
                        break;
                    default:
                        codeName = "";
                        break;
                }
                return codeName;
            }
            return "";
        }

        /// <summary>
        /// 根据土地等级Code获取土地等级名称
        /// </summary>
        /// <param name="code">土地等级Code</param>
        /// <returns></returns>
        public static string GetLandClassName(int code)
        {
            if (code > 0)
            {
                string name;
                switch (code)
                {
                    case 1209001:
                        name = "一级";
                        break;
                    case 1209002:
                        name = "二级";
                        break;
                    case 1209003:
                        name = "三级";
                        break;
                    case 1209004:
                        name = "四级";
                        break;
                    case 1209005:
                        name = "五级";
                        break;
                    case 1209006:
                        name = "六级";
                        break;
                    case 1209007:
                        name = "七级";
                        break;
                    default:
                        name = "";
                        break;
                }
                return name;
            }
            return "";
        }
        /// <summary>
        /// 根据土地等级名称获取土地等级Code
        /// </summary>
        /// <param name="codeName">土地等级名称</param>
        /// <returns></returns>
        public static int GetLandClassCode(string codeName)
        {
            if (!string.IsNullOrEmpty(codeName.Trim()))
            {
                int code;
                switch (codeName)
                {
                    case "一级":
                        code = 1209001;
                        break;
                    case "二级":
                        code = 1209002;
                        break;
                    case "三级":
                        code = 1209003;
                        break;
                    case "四级":
                        code = 1209004;
                        break;
                    case "五级":
                        code = 1209005;
                        break;
                    case "六级":
                        code = 1209006;
                        break;
                    case "七级":
                        code = 1209007;
                        break;
                    default:
                        code = -1;
                        break;
                }
                return code;
            }
            return -1;
        }

        /// <summary>
        /// 土地形状Code
        /// </summary>
        /// <param name="codeName">土地形状名称</param>
        /// <returns></returns>
        public static int GetLandShapeCode(string codeName)
        {
            if (!string.IsNullOrEmpty(codeName.Trim()))
            {
                int code;
                switch (codeName.Trim())
                {

                    case "矩形":
                        code = 6035001;
                        break;
                    case "梯形":
                        code = 6035002;
                        break;
                    case "不规则形状":
                        code = 6035003;
                        break;
                    default:
                        code = -1;
                        break;
                }
                return code;
            }
            return -1;

        }
        /// <summary>
        /// 土地形状Name
        /// </summary>
        /// <param name="codeName">土地形状名称</param>
        /// <returns></returns>
        public static string GetLandShapeName(int code)
        {
            if (code > 0)
            {
                string codeName;
                switch (code)
                {
                    case 6035001:
                        codeName = "矩形";
                        break;
                    case 6035002:
                        codeName = "梯形";
                        break;
                    case 6035003:
                        codeName = "不规则形状";
                        break;
                    default:
                        codeName = "";
                        break;
                }
                return codeName;
            }
            return "";

        }


        /// <summary>
        /// 开发程度Code
        /// </summary>
        /// <param name="codeName">开发程度名称</param>
        /// <returns></returns>
        public static int GetDevelopmentCode(string codeName)
        {
            if (!string.IsNullOrEmpty(codeName.Trim()))
            {
                int code;
                switch (codeName.Trim())
                {

                    case "三通一平":
                        code = 3005001;
                        break;
                    case "五通一平":
                        code = 3005002;
                        break;
                    case "六通一平":
                        code = 3005003;
                        break;
                    case "七通一平":
                        code = 3005004;
                        break;
                    case "生地":
                        code = 3005005;
                        break;
                    case "八通一平":
                        code = 3005006;
                        break;
                    case "熟地":
                        code = 3005007;
                        break;
                    default:
                        code = -1;
                        break;
                }
                return code;
            }
            return -1;

        }
        /// <summary>
        /// 开发程度Name
        /// </summary>
        /// <param name="codeName">开发程度名称</param>
        /// <returns></returns>
        public static string GetDevelopmentName(int code)
        {
            if (code > 0)
            {
                string codeName;
                switch (code)
                {
                    case 3005001:
                        codeName = "三通一平";
                        break;
                    case 3005002:
                        codeName = "五通一平";
                        break;
                    case 3005003:
                        codeName = "六通一平";
                        break;
                    case 3005004:
                        codeName = "七通一平";
                        break;
                    case 3005005:
                        codeName = "生地";
                        break;
                    case 3005006:
                        codeName = "八通一平";
                        break;
                    case 3005007:
                        codeName = "熟地";
                        break;
                    default:
                        codeName = "";
                        break;
                }
                return codeName;
            }
            return "";

        }

        /// <summary>
        /// 环境质量Code
        /// </summary>
        /// <param name="codeName">环境质量名称</param>
        /// <returns></returns>
        public static int GetEnvironmentCode(string codeName)
        {
            if (!string.IsNullOrEmpty(codeName.Trim()))
            {
                int code;
                switch (codeName.Trim())
                {

                    case "优":
                        code = 1012001;
                        break;
                    case "良":
                        code = 1012002;
                        break;
                    case "一般":
                        code = 1012003;
                        break;
                    case "差":
                        code = 1012004;
                        break;
                    case "很差":
                        code = 1012005;
                        break;
                    default:
                        code = -1;
                        break;
                }
                return code;
            }
            return -1;

        }
        /// <summary>
        /// 环境质量Name
        /// </summary>
        /// <param name="codeName">环境质量名称</param>
        /// <returns></returns>
        public static string GetEnvironmentName(int code)
        {
            if (code > 0)
            {
                string codeName;
                switch (code)
                {
                    case 1012001:
                        codeName = "优";
                        break;
                    case 1012002:
                        codeName = "良";
                        break;
                    case 1012003:
                        codeName = "一般";
                        break;
                    case 1012004:
                        codeName = "差";
                        break;
                    case 1012005:
                        codeName = "很差";
                        break;
                    default:
                        codeName = "";
                        break;
                }
                return codeName;
            }
            return "";

        }

         /// <summary>
        /// 土地用途Code
        /// </summary>
        /// <param name="codeName">土地用途名称</param>
        /// <returns></returns>
        public static int GetPurposeCode(string codeName)
        {
            if (!string.IsNullOrEmpty(codeName.Trim()))
            {
                int code;
                switch (codeName.Trim())
                {

                    case "居住":
                        code = 1001001;
                        break;
                    case "居住(别墅)":
                        code = 1001002;
                        break;
                    case "居住(洋房)":
                        code = 1001003;
                        break;
                    case "商业":
                        code = 1001004;
                        break;
                    case "办公":
                        code = 1001005;
                        break;
                    case "工业":
                        code = 1001006;
                        break;
                    case "居住商业、居住":
                        code = 1001007;
                        break;
                    case "商业、办公":
                        code = 1001008;
                        break;
                    case "办公、居住":
                        code = 1001009;
                        break;
                    case "停车场":
                        code = 1001010;
                        break;
                    case "酒店":
                        code = 1001011;
                        break;
                    case "加油站":
                        code = 1001012;
                        break;
                    case "综合":
                        code = 1001013;
                        break;
                    case "其他":
                        code = 1001014;
                        break;
                    default:
                        code = -1;
                        break;
                }
                return code;
            }
            return -1;
        }


        /// <summary>
        /// 土地用途Name
        /// </summary>
        /// <param name="codeName">土地用途code</param>
        /// <returns></returns>
        public static string GetPurposeName(string code)
        {
            if (!string.IsNullOrEmpty(code.Trim()))
            {
                if (code == "-1")
                {
                    return "";
                }
                else
                {
                    if (code.Contains(","))
                    {
                        string name = "";
                        for (int i = 0; i < code.Split(',').Length; i++)
                        {
                            name += PlanPurpose(code.Split(',')[i])+",";
                        }
                        return name.Substring(0, name.Length - 1);
                    }
                    else
                    {
                        return PlanPurpose(code);
                    }
                }
            }
            else
            {
                return "";
            }
           
        }

        private static string PlanPurpose(string code)
        {
            string codeName;
            switch (code)
            {
                case "1001001":
                    codeName = "居住";
                    break;
                case "1001002":
                    codeName = "居住(别墅)";
                    break;
                case "1001003":
                    codeName = "居住(洋房)";
                    break;
                case "1001004":
                    codeName = "商业";
                    break;
                case "1001005":
                    codeName = "办公";
                    break;
                case "1001006":
                    codeName = "工业";
                    break;
                case "1001007":
                    codeName = "居住商业、居住";
                    break;
                case "1001008":
                    codeName = "商业、办公";
                    break;
                case "1001009":
                    codeName = "办公、居住";
                    break;
                case "1001010":
                    codeName = "停车场";
                    break;
                case "1001011":
                    codeName = "酒店";
                    break;
                case "1001012":
                    codeName = "加油站";
                    break;
                case "1001013":
                    codeName = "综合";
                    break;
                case "1001014":
                    codeName = "其他";
                    break;
                default:
                    codeName = "";
                    break;
            }
            return codeName;
        }


        /// <summary>
        /// 获取建筑类型名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetBuildingTypeName(int code)
        {
            if (code > 0)
            {
                string codeName;
                switch (code)
                {
                    case 2003001:
                        codeName = "低层";
                        break;
                    case 2003002:
                        codeName = "多层";
                        break;
                    case 2003003:
                        codeName = "小高层";
                        break;
                    case 2003004:
                        codeName = "高层";
                        break;
                    default:
                        codeName = "";
                        break;
                }
                return codeName;
            }
            return "";
        }

        /// <summary>
        /// 获取建筑类型Code
        /// </summary>
        /// <param name="codeName">建筑类型名称</param>
        /// <returns></returns>
        public static int GetBuildingTypeCode(string codeName)
        {
            if (!string.IsNullOrEmpty(codeName.Trim()))
            {
                int code;
                switch (codeName.Trim())
                {

                    case "低层":
                        code = 2003001;
                        break;
                    case "多层":
                        code = 2003002;
                        break;
                    case "小高层":
                        code = 2003003;
                        break;
                    case "高层":
                        code = 2003004;
                        break;
                    default:
                        code = -1;
                        break;
                }
                return code;
            }
            return -1;

        }
    }
}
