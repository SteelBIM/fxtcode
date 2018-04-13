using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Framework.Contract.API;
using CBSS.Framework.Contract.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Web.API
{
    public class APIData
    {
        /// <summary>
        /// 请求参数判断
        /// </summary>
        /// <param name="Key">秘钥</param>
        /// <param name="Info">请求数据</param>
        /// <param name="FunName">接口名称</param
        /// <param name="FunWay">0正常,1加密,2压缩,3压缩加密</param>
        /// <returns></returns>
        public APIResponse JudgeRequestData(string Key, string Info, string FunName, int FunWay = 0)
        {
            if (string.IsNullOrEmpty(Info))
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.接口请求Info为空);
            }
            else if (string.IsNullOrEmpty(FunName))
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.接口方法名不正确);
            }
            APIResponse response = new APIResponse();
            string dataStr = Info;
            try
            {
                switch (FunWay)
                {
                    case (int)ApiFuncWayEnum.Encrypt://加密
                        APIResponse errResponse=null;
                        dataStr = Decrypt(Key,Info, out errResponse);
                        if (errResponse!=null)
                        {
                            return errResponse;
                        }
                        break;
                    case (int)ApiFuncWayEnum.Compress://压缩
                        dataStr = DecompressString(Info);
                        break;
                    case (int)ApiFuncWayEnum.CompressEncrypt://加密并压缩
                        string decompressStr = DecompressString(Info);
                        APIResponse errCEResponse = null;
                        dataStr = Decrypt(Key, decompressStr, out errCEResponse);
                        if (errCEResponse != null)
                        {
                            return errCEResponse;
                        }
                        break;
                    default:
                        break;
                }
                return APIResponse.GetResponse(dataStr) ;
            }
            catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.请求参数有误,LogLevelEnum.Error, ex);
            }
        }

        /// <summary>
        /// 解密输入参数
        /// </summary>
        /// <returns></returns>
        public string Decrypt(string key,string info,out APIResponse errResponse)
        {
            errResponse = null;
            if (string.IsNullOrEmpty(key))
            {
                errResponse= APIResponse.GetErrorResponse(ErrorCodeEnum.接口请求Key为空);
            }
            string privatKey = XMLHelper.GetAppSetting("PrivateKey");
            //RSA解密:最外层的key，得到明文key
            string keyStr = RSAHelper.decryptData(key, privatKey, "UTF-8");
            //des解密： 用明文key解密data，得到原数据datajson
            return  SecurityHelper.DecryptDES(info, keyStr);
        }

        /// <summary>
        /// 接口返回数据处理
        /// </summary>
        /// <param name="PKey"></param>
        /// <param name="Data"></param>
        /// <param name="FunName"></param>
        /// <returns></returns>
        public APIResponse HandleResponseData(string PKey, object Data, int FunWay)
        {
            APIReturnData responseData = new APIReturnData();
            string returnData = "";
            try
            {
                switch (FunWay)
                {
                    case (int)ApiFuncWayEnum.Nornal:
                        returnData = JsonConvertHelper.ToJson(Data);
                        break;
                    case (int)ApiFuncWayEnum.Encrypt:
                        responseData = EncryptReturnData(PKey, Data);
                        returnData = JsonConvertHelper.ToJson(responseData);
                        break;
                    case (int)ApiFuncWayEnum.Compress:
                     //   returnData = CompressString(JsonConvertHelper.ToJson(Data));
                        break;
                    case (int)ApiFuncWayEnum.CompressEncrypt:
                        //responseData = EncryptReturnData(PKey, Data);
                        //returnData = CompressString(JsonConvertHelper.ToJson(responseData));
                        break;
                }
                return APIResponse.GetResponse(returnData);
            }
            catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.返回参数有误,LogLevelEnum.Error,ex);
            }
        }

        /// <summary>
        /// 返回值加密
        /// </summary>
        /// <param name="PKey"></param>
        /// <param name="Data"></param>
        public APIReturnData EncryptReturnData(string PKey ,object Data)
        {
            APIReturnData responseData = new APIReturnData();
            //取出data中的key（未加密）,用来加密我自动生成的key
            //再用我生成的key去加密返回的data
            //客户端传过来的公钥
            PKey = PKey.Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace(" ", "");
            //服务端生成8位key
            string keyTmp = Guid.NewGuid().ToString().Substring(0, 8);
            //RSA加密后的key,客户端用parmModel.Key进行解密
            string keyDes = RSAHelper.encryptData(keyTmp, PKey, "UTF-8");
            string strData = JsonConvertHelper.ToJson(Data);
            //DES加密返回的data
            string dataDes = SecurityHelper.EncryptDES(strData, keyTmp);
            responseData.Key = keyDes;
            responseData.Info = dataDes;
            return responseData;
        }


        /// <summary>
        /// 压缩方法
        /// </summary>
        public string CompressString(string str)
        {
            string compressString = "";
            byte[] compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(str);
            byte[] compressAfterByte = Compress(compressBeforeByte);
            compressString = Convert.ToBase64String(compressAfterByte);
            return compressString;
        }

        public byte[] Compress(byte[] data)
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

        /// <summary>
        /// 字符串解压缩
        /// </summary>
        public string DecompressString(string str)
        {
            string compressString = "";
            byte[] compressBeforeByte = Convert.FromBase64String(str);
            byte[] compressAfterByte = Decompress(compressBeforeByte);
            compressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);
            return compressString;
        }

        public byte[] Decompress(byte[] data)
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
    }
}
