using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxtCommonLibrary.LibraryLucene;
/**
 * 作者:李晓东
 * 摘要 2014.01.27  新建
 *                  修改人:李晓东  2014.01.27
 *                  修改:ReadWordbase,ReadWordbase 因被调用类修改,把相关所使用到的类命名空间更换
 * **/
namespace FxtCollateralManager.Common
{
    public class Participle
    {
        /// <summary>
        /// 得到拆分字中的某一个
        /// </summary>
        /// <param name="strKey">字符串</param>
        /// <param name="readNumber">下标</param>
        /// <returns>字符串</returns>
        public static string ReadWordbase(string strKey,int readNumber)
        {
            string readWord = string.Empty, strToken = string.Empty;
            List<string> list = new ChineseAnalyzer().GetTokenList(strKey);
            int i = 0;
            foreach (var item in list) {
                if (i == readNumber) break;
                readWord = item;
                i++;
            }
            return readWord;
        }

        /// <summary>
        /// 得到所有拆分字
        /// </summary>
        /// <param name="strKey">字符串</param>
        /// <returns>数组</returns>
        public static string[] ReadWordbase(string strKey)
        {
            string readWord = string.Empty, strToken = string.Empty;
            List<string> list = new ChineseAnalyzer().GetTokenList(strKey);
            return list.ToArray();
        }
    }
}
