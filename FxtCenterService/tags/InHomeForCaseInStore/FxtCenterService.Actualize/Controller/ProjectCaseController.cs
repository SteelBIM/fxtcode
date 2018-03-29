using System;
using System.Collections.Generic;
using System.Linq;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using CAS.Entity.DBEntity;
using System.Data;
using CAS.Entity.GJBEntity;
using System.Web;
using System.IO;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using OpenPlatform.Framework.FlowMonitor;
using Newtonsoft.Json;
using FxtCommon.Openplatform.Data;
using FxtCommon.Openplatform.GrpcService;
using FxtOpenClient.ClientService;
using CAS.Entity.FxtProject;
using FxtCenterService.Common;
using System.Diagnostics;
using System.Data.Common;
using System.Data.SqlClient;
using FxtCenterService.DataAccess;
using System.Threading.Tasks;
using System.Text;
using System.IO.Compression;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>/// <summary>
        /// 上传案例
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string AddProjectCaseData(JObject funinfo, UserCheck company)
        {
            try
            {
                int cityid = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
                string value = funinfo.Value<string>("value");
                if (string.IsNullOrEmpty(value)) return "成功";
                value = DecompressString(value);
                return DatProjectCaseBL.Add(cityid, value);
            }
            catch (Exception e)
            {
                LogHelper.Error("AddProjectCaseData报错，ex=" + e.ToString());
                throw new Exception(e.Message);
            }
        }

        #region 辅助方法
        /// <summary>
        /// 字符串压缩
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(data, 0, data.Length);
                zip.Close();
                byte[] buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Close();
                return buffer;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 字符串解压缩
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(data);
                GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
                MemoryStream msreader = new MemoryStream();
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int reader = zip.Read(buffer, 0, buffer.Length);
                    if (reader <= 0)
                    {
                        break;
                    }
                    msreader.Write(buffer, 0, reader);
                }
                zip.Close();
                ms.Close();
                msreader.Position = 0;
                buffer = msreader.ToArray();
                msreader.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 字符串压缩
        /// </summary>
        public static string CompressString(string str)
        {
            string compressString = "";
            byte[] compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(str);
            byte[] compressAfterByte = Compress(compressBeforeByte);
            //compressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);  
            compressString = Convert.ToBase64String(compressAfterByte);
            return compressString;
        }

        /// <summary>
        /// 字符串解压缩
        /// </summary>
        public static string DecompressString(string str)
        {
            string compressString = "";
            //byte[] compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(str);  
            byte[] compressBeforeByte = Convert.FromBase64String(str);
            byte[] compressAfterByte = Decompress(compressBeforeByte);
            compressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);
            return compressString;
        }

        #endregion
    }
}
