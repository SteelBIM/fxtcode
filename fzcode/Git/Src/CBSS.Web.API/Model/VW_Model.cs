using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseActivate.Web.API.Model
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Telephone”的 XML 注释
    public class Telephone
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Telephone”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Telephone.telephone”的 XML 注释
        public string telephone { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Telephone.telephone”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TelephoneAndCode”的 XML 注释
    public class TelephoneAndCode
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TelephoneAndCode”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TelephoneAndCode.telephone”的 XML 注释
        public string telephone { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TelephoneAndCode.telephone”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TelephoneAndCode.code”的 XML 注释
        public string code { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TelephoneAndCode.code”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo”的 XML 注释
    public class MobileLoginInfo : Telephone
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.code”的 XML 注释
        public string code { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.code”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.equipmentID”的 XML 注释
        public string equipmentID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.equipmentID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.deviceType”的 XML 注释
        public string deviceType { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.deviceType”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.ipAddress”的 XML 注释
        public string ipAddress { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.ipAddress”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.appID”的 XML 注释
        public string appID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.appID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.isEnableOss”的 XML 注释
        public int isEnableOss { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.isEnableOss”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.appChannelID”的 XML 注释
        public string appChannelID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.appChannelID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.versionNumber”的 XML 注释
        public string versionNumber { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MobileLoginInfo.versionNumber”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModifyPassWordInfo”的 XML 注释
    public class ModifyPassWordInfo : Telephone
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModifyPassWordInfo”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ModifyPassWordInfo.password”的 XML 注释
        public string password { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ModifyPassWordInfo.password”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginInfo”的 XML 注释
    public class LoginInfo
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginInfo”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginInfo.appID”的 XML 注释
        public string appID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginInfo.appID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginInfo.userName”的 XML 注释
        public string userName { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginInfo.userName”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginInfo.passWord”的 XML 注释
        public string passWord { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginInfo.passWord”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginInfo.machineCode”的 XML 注释
        public string machineCode { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginInfo.machineCode”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginInfo.machineModel”的 XML 注释
        public string machineModel { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginInfo.machineModel”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginInfo.equipmentID”的 XML 注释
        public string equipmentID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginInfo.equipmentID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginInfo.deviceType”的 XML 注释
        public string deviceType { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginInfo.deviceType”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginInfo.ipAddress”的 XML 注释
        public string ipAddress { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginInfo.ipAddress”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginInfo.appChannelID”的 XML 注释
        public string appChannelID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginInfo.appChannelID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginInfo.versionNumber”的 XML 注释
        public string versionNumber { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginInfo.versionNumber”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AreaID”的 XML 注释
    public class AreaID
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AreaID”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AreaID.areaID”的 XML 注释
        public string areaID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AreaID.areaID”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserID”的 XML 注释
    public class UserID
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserID”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserID.userID”的 XML 注释
        public string userID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserID.userID”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserNumAndUserID”的 XML 注释
    public class UserNumAndUserID : UserID
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserNumAndUserID”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserNumAndUserID.userNum”的 XML 注释
        public string userNum { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserNumAndUserID.userNum”的 XML 注释
    }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ParentIDAndAppID”的 XML 注释
    public class ParentIDAndAppID
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ParentIDAndAppID”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ParentIDAndAppID.parentID”的 XML 注释
        public int parentID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ParentIDAndAppID.parentID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ParentIDAndAppID.appID”的 XML 注释
        public Guid appID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ParentIDAndAppID.appID”的 XML 注释
    }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ClassifyID”的 XML 注释
    public class ClassifyID
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ClassifyID”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ClassifyID.classifyID”的 XML 注释
        public int classifyID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ClassifyID.classifyID”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo”的 XML 注释
    public class TeacherInfo
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.userID”的 XML 注释
        public int userID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.userID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.userName”的 XML 注释
        public string userName { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.userName”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.areaID”的 XML 注释
        public int areaID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.areaID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.area”的 XML 注释
        public string area { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.area”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.cityID”的 XML 注释
        public int cityID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.cityID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.city”的 XML 注释
        public string city { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.city”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.provinceID”的 XML 注释
        public int provinceID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.provinceID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.province”的 XML 注释
        public string province { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.province”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.subjectID”的 XML 注释
        public int subjectID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.subjectID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.subject”的 XML 注释
        public string subject { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.subject”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.schoolID”的 XML 注释
        public int schoolID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.schoolID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TeacherInfo.schoolName”的 XML 注释
        public string schoolName { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TeacherInfo.schoolName”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserImage”的 XML 注释
    public class UserImage
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserImage”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserImage.userID”的 XML 注释
        public string userID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserImage.userID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserImage.nickName”的 XML 注释
        public string nickName { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserImage.nickName”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserImage.isEnableOss”的 XML 注释
        public int isEnableOss { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserImage.isEnableOss”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserImage.userImage”的 XML 注释
        public string userImage { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserImage.userImage”的 XML 注释

    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“VersionInfo”的 XML 注释
    public class VersionInfo
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“VersionInfo”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“VersionInfo.appID”的 XML 注释
        public string appID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“VersionInfo.appID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“VersionInfo.versionNumber”的 XML 注释
        public string versionNumber { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“VersionInfo.versionNumber”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“VersionInfo.appType”的 XML 注释
        public int appType { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“VersionInfo.appType”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AreaInfo”的 XML 注释
    public class AreaInfo
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AreaInfo”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AreaInfo.ID”的 XML 注释
        public int ID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AreaInfo.ID”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“GetWordsModel”的 XML 注释
    public class GetWordsModel
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“GetWordsModel”的 XML 注释
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string SecretKeyId { get; set; }
        /// <summary>
        /// 目录ID
        /// </summary>
        public string CatalogueId { get; set; }
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BookID”的 XML 注释
    public class BookID
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BookID”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“BookID.bookID”的 XML 注释
        public string bookID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“BookID.bookID”的 XML 注释

    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“CheckModulePermissionModel”的 XML 注释
    public class CheckModulePermissionModel
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“CheckModulePermissionModel”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“CheckModulePermissionModel.moduleIds”的 XML 注释
        public List<int> moduleIds { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“CheckModulePermissionModel.moduleIds”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“CheckModulePermissionModel.appID”的 XML 注释
        public string appID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“CheckModulePermissionModel.appID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“CheckModulePermissionModel.userID”的 XML 注释
        public long? userID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“CheckModulePermissionModel.userID”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MarketBookCatalogID”的 XML 注释
    public class MarketBookCatalogID
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MarketBookCatalogID”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MarketBookCatalogID.marketBookCatalogID”的 XML 注释
        public string marketBookCatalogID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MarketBookCatalogID.marketBookCatalogID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MarketBookCatalogID.appID”的 XML 注释
        public string appID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MarketBookCatalogID.appID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MarketBookCatalogID.userID”的 XML 注释
        public long userID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MarketBookCatalogID.userID”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“GetArticleModel”的 XML 注释
    public class GetArticleModel
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“GetArticleModel”的 XML 注释
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string secretKeyId { get; set; }
        /// <summary>
        /// 目录ID
        /// </summary>
        public int catalogueId { get; set; }
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UseAppRecord”的 XML 注释
    public class UseAppRecord
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UseAppRecord”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UseAppRecord.UserID”的 XML 注释
        public string UserID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UseAppRecord.UserID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UseAppRecord.UseAppTime”的 XML 注释
        public int UseAppTime { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UseAppRecord.UseAppTime”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UseAppRecord.AppChannelID”的 XML 注释
        public string AppChannelID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UseAppRecord.AppChannelID”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UseAppRecord.AppID”的 XML 注释
        public string AppID { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UseAppRecord.AppID”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UseAppRecord.VersionNumber”的 XML 注释
        public string VersionNumber { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UseAppRecord.VersionNumber”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“InputReportPar”的 XML 注释
    public class InputReportPar
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“InputReportPar”的 XML 注释
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 目录ID
        /// </summary>
        public int CatalogID { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModuleID { get; set; }
    }


    public class UserInfoRequest
    {
        public int UserID { get; set; }

        public string TrueName { get; set; }

        public string SchoolID { get;set; }

        public string SchoolName { get; set; }

        public string APPID { get; set; }

        public string SubjectID { get; set; }

        public string IDStr { get; set; }

        public string ClassID { get; set; }

        public int StudentID { get; set; }
    }


    public class ClassByTelephone: TelephoneAndCode
    {
        public int Type { get; set; }
    }
    //班级信息
    public class ClassInfo
    {
        public string Id { get; set; }
        public string ClassNum { get; set; }
        public string ClassName { get; set; }
        public int StudentNum { get; set; }
        public int? SchoolId { get; set; }
        public int GradeId { get; set; }
    }
}