using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.FxtApi.Model;
using FxtSpider.FxtApi.Fxt.Api;
using FxtSpider.Common;
using log4net;
using FxtSpider.FxtApi.FxtApiClientManager;
using Newtonsoft.Json.Linq;

namespace FxtSpider.FxtApi.ApiManager
{
    public static class SysCodeApi
    {
        /// <summary>
        /// 别墅相关的住宅用途code
        /// </summary>
        public static List<int> PurposeTypeCodeVillaType = new List<int>();
        public static List<FxtApi_SYSCode> BuildingTypeCodeList = new List<FxtApi_SYSCode>();
        #region code
        /// <summary>
        /// 居住用途1002,普通住宅
        /// </summary>
        public const int Code1 = 1002001;
        /// <summary>
        /// ID:1002(居住用途),别墅
        /// </summary>
        public const int Code2 = 1002027;
        /// <summary>
        /// ID:1002(居住用途),独立别墅
        /// </summary>
        public const int Code8 = 1002005;
        /// <summary>
        /// ID:1002(居住用途),联排别墅
        /// </summary>
        public const int Code9 = 1002006;
        /// <summary>
        /// ID:1002(居住用途),叠加别墅
        /// </summary>
        public const int Code10 = 1002007;
        /// <summary>
        /// ID:1002(居住用途),双拼别墅
        /// </summary>
        public const int Code11 = 1002008;
        /// <summary>
        /// 小于30
        /// </summary>
        public const int Code3 = 8006001;
        /// <summary>
        /// 30~60
        /// </summary>
        public const int Code4 = 8006002;
        /// <summary>
        /// 60~90
        /// </summary>
        public const int Code5 = 8006003;
        /// <summary>
        /// 90~120
        /// </summary>
        public const int Code6 = 8006004;
        /// <summary>
        /// 大于120
        /// </summary>
        public const int Code7 = 8006005;
        /// <summary>
        /// 案例类型:买卖报盘
        /// </summary>
        public const int Code12 = 3001001;
        /// <summary>
        /// 币种:人民币
        /// </summary>
        public const int Code13 = 2002001;
        /// <summary>
        /// 建筑结构:平面
        /// </summary>
        public const int Code14 = 2005001;

        #endregion

        #region code_ID
        /// <summary>
        /// 土地用途TypeID
        /// </summary>
        public const int CodeID_1 = 1001;
        /// <summary>
        /// 居住用途TypeID
        /// </summary>
        public const int CodeID_2 = 1002;
        /// <summary>
        /// 案例类型
        /// </summary>
        public const int CodeID_3 = 3001;
        /// <summary>
        /// 建筑结构
        /// </summary>
        public const int CodeID_4 = 2005;
        /// <summary>
        /// 建筑类型
        /// </summary>
        public const int CodeID_5 = 2003;
        /// <summary>
        /// 户型
        /// </summary>
        public const int CodeID_6 = 4001;
        /// <summary>
        /// 朝向
        /// </summary>
        public const int CodeID_7 = 2004;
        /// <summary>
        /// 币种
        /// </summary>
        public const int CodeID_8 = 2002;
        /// <summary>
        /// 装修
        /// </summary>
        public const int CodeID_9 = 6026;
        #endregion
        public static readonly ILog log = LogManager.GetLogger(typeof(SysCodeApi));
        static SysCodeApi()
        {
            List<FxtApi_SYSCode> list = GetPurposeTypeCodeVillaType();
            foreach (var item in list)
            {
                PurposeTypeCodeVillaType.Add(item.Code);
            }
            BuildingTypeCodeList = GetSYSCodeById(CodeID_5);
        }
        /// <summary>
        /// 获取所有楼盘的用途信息
        /// </summary>
        /// <returns></returns>
        public static List<FxtApi_SYSCode> GetAllProjectPurposeCode(FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_SYSCode> list = new List<FxtApi_SYSCode>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {

                string name = "GetAllProjectPurposeCode";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, "", _fxtApi: fxtApi));

                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_SYSCode>();
                }
                list = FxtApi_SYSCode.ConvertToObjList(jsonStr);
                list.DecodeField<FxtApi_SYSCode>();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("GetAllProjectPurposeCode()", ex);
            }
            return list;
        }
        /// <summary>
        /// 获取别墅相关的住宅用途
        /// </summary>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static List<FxtApi_SYSCode> GetPurposeTypeCodeVillaType(FxtAPIClientExtend _fxtApi = null)
        {

            List<FxtApi_SYSCode> list = new List<FxtApi_SYSCode>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetPurposeTypeCodeVillaType";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, "", _fxtApi: fxtApi));
                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_SYSCode>();
                }
                list = FxtApi_SYSCode.ConvertToObjList(jsonStr);
                list.DecodeField<FxtApi_SYSCode>();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("GetPurposeTypeCodeVillaType()", ex);
            }
            return list;
        }

        public static List<FxtApi_SYSCode> GetSYSCodeById(int id,FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_SYSCode> list = new List<FxtApi_SYSCode>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetSYSCodeByID";
                var para = new { id = id };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_SYSCode>();
                }
                list = FxtApi_SYSCode.ConvertToObjList(jsonStr);
                list.DecodeField<FxtApi_SYSCode>();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("GetSYSCodeById(int id,FxtAPIClientExtend _fxtApi = null)", ex);
            }
            return list;
        }
        /// <summary>
        /// 根据字段code获取code信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static FxtApi_SYSCode GetSYSCodeByCode(int code, FxtAPIClientExtend _fxtApi = null)
        {
            FxtApi_SYSCode codeObj = null;
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                JObject jObjPara = new JObject();
                jObjPara.Add(new JProperty("code", code));
                string methodName = "GetSYSCodeByCode";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(methodName, jObjPara.ToJSONjss(), _fxtApi: fxtApi));
                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return codeObj;
                }
                codeObj = FxtApi_SYSCode.ConvertToObj(jsonStr);
                codeObj = codeObj.DecodeField();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetSYSCodeByCode(int code, FxtAPIClientExtend _fxtApi = null)),code={0}",
                     code), ex);
            }
            return codeObj;
        }
    }
}
