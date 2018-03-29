using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;

namespace FxtCenterService.Logic
{
    public class SYSCodeBL
    {
        /// <summary>
        /// 获取CODE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codename"></param>
        /// <returns></returns>
        public static SYSCode GetCode(int id, string codename)
        {
            return SYSCodeDA.GetCode(id, codename);
        }

        /// <summary>
        /// 获取CODE列表
        /// </summary>
        /// <param name="codeinfo"></param>
        /// <returns></returns>
        public static List<SYSCode> GetCodeList(Dictionary<int, string> codeinfo)
        {
            return SYSCodeDA.GetCodeList(codeinfo);
        }

        /// <summary>
        /// 根据编号获取syscode
        /// yinpc
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<CAS.Entity.SurveyDBEntity.SYSCode> GetSYSCodeList(int code)
        {
            return SYSCodeDA.GetSYSCodeList(code);
        }

        /// <summary>
        /// 根据类型获取syscode 
        /// yinpc
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<CAS.Entity.SurveyDBEntity.SYSCode> GetSYSCodeListByDictType(int type)
        {
            return SYSCodeDA.GetSYSCodeListByDictType(type);
        }
    }
}
