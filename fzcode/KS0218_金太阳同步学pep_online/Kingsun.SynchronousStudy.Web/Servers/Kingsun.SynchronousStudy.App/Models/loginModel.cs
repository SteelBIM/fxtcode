using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.App.Models
{
    public class loginModel
    {
        //public string UserName { get; set; }
        //public string Password { get; set; }
        //public string PhoneNum { get; set; }
        //public string LoginIP { get; set; }
        //public string TelePhone { get; set; }
        //public string MessageCode { get; set; }
        //public string UserNum { get; set; }
        public string EquipmentID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string UserID { get; set; }
        public string TelePhone { get; set; }
        public string MachineCode { get; set; }
        public string MachineModel { get; set; }
        public string MessageCode { get; set; }
        public string OldPasswrod { get; set; }
        public string UserNum { get; set; }
        public string UserType { get; set; }
        public int IsEnableOss { get; set; }
        public string DeviceType { get; set; }
        public string IPAddress { get; set; }

        public string NickName { get; set; }

        public string UserImage { get; set; }

        /// <summary>
        /// app版本
        /// </summary>
        public string Versions { get; set; }

        /// <summary>
        /// 下载渠道
        /// </summary>
        public int DownloadChannel { get; set; }
    }

    public class ClassModel
    {
        public string UserID { get; set; }
        public string ClassID { set; get; }
        public string ClassName { set; get; }
        public Guid ID { set; get; }
        public string Remark { set; get; }
        public string SchoolName { set; get; }
        public int SubjectID { set; get; }
    }

    public class Schooles
    {
        
       public int? SchoolID{set;get;}
       public string SchoolName{set;get;}
       public bool? IsHot { set; get; }
       public string ImgUrl { set; get; }
       public string WebSite { set; get; }
       public string IsHotTime { set; get; }
       public string Description { set; get; }
       public string SchoolMasterName { set; get; }
       public string SchoolAddres { set; get; }
       public string SchoolTelephone { set; get; }
       public char? Deleted { set; get; }
       public DateTime AddTime { set; get; }
       public int? SegmentID { set; get; }
       public string UserID { set; get; }
       public int? DistrictID { set; get; }
       public int? TownsID { set; get; }
    }

    /// <summary>
    /// 服务处理结果输出
    /// </summary>
    public class KinResponses
    {
        public Schooles Schooles;
        /// <summary>
        /// 请求方法
        /// </summary>
        public string RequestID
        {
            get;
            set;
        }
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success
        {
            get;
            set;
        }
        /// <summary>
        /// 业务数据
        /// </summary>
        public object Data
        {
            get;
            set;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get;
            set;
        }

        /// <summary>
        /// 按错误数据创建输出对象
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static KingResponse GetErrorResponse(string errorMsg, KingRequest request = null)
        {
            KingResponse response = new KingResponse();
            response.Success = false;
            response.ErrorMsg = errorMsg;
            if (request != null)
            {
                response.RequestID = request.ID;
            }
            else
            {
                response.RequestID = "";
            }
            response.Data = null;
            return response;
        }

        /// <summary>
        /// 按错误数据获取错误输出字符串
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string GetErrorResponseString(string errorMsg, KingRequest request = null)
        {
            return JsonHelper.EncodeJson(GetErrorResponse(errorMsg, request));
        }

        public static KingResponse GetResponse(KingRequest request, object data)
        {
            KingResponse response = new KingResponse();
            response.Success = true;
            response.ErrorMsg = "";
            if (request != null)
            {
                response.RequestID = request.ID;
            }
            else
            {
                response.RequestID = "";
            }
            response.Data = data;
            return response;
        }



    }




}