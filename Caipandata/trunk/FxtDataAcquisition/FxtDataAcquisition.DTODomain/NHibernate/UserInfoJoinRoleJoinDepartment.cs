using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.NHibernate.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.DTODomain.NHibernate
{
    public class UserInfoJoinRoleJoinDepartment
    {

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public virtual string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty(PropertyName = "truename")]
        public virtual string TrueName
        {
            get;
            set;
        }
        /// <summary>
        /// 电话
        /// </summary>
        [JsonProperty(PropertyName = "mobile")]
        public virtual string Mobile
        {
            get;
            set;
        }
        /// <summary>
        /// 机构名
        /// </summary>
        [JsonProperty(PropertyName = "companyname")]
        public virtual string CompanyName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "uservalid")]
        public virtual int UserValid
        {
            get;
            set;
        }
        /// <summary>
        /// 多个角色名称(由逗号分隔)
        /// </summary>
        [JsonProperty(PropertyName = "rolenames")]
        public virtual string RoleNames
        {
            get;
            set;
        }
        /// <summary>
        /// DepartmentId
        /// </summary>
        [JsonProperty(PropertyName = "departmentid")]
        public virtual int? DepartmentId
        {
            get;
            set;
        }
        /// <summary>
        /// DepartmentName
        /// </summary>
        [JsonProperty(PropertyName = "departmentname")]
        public virtual string DepartmentName
        {
            get;
            set;
        }
        public UserInfoJoinRoleJoinDepartment(string userName, string trueName,string mobile, string companyName, int userValid,  string roleNames, int? departmentId, string departmentName)
        {
            this.UserName = userName;
            this.TrueName = trueName;
            this.Mobile = mobile;
            this.CompanyName = companyName;
            this.UserValid = userValid;
            this.RoleNames = roleNames;
            this.DepartmentId = departmentId;
            this.DepartmentName = departmentName;
        }
        /// <summary>
        /// 不按小组和角色查询时,用此方法转换
        /// </summary>
        /// <param name="userList">主数据</param>
        /// <param name="roleList"></param>
        /// <param name="departmentList"></param>
        /// <returns></returns>
        public static List<UserInfoJoinRoleJoinDepartment> GetList(List<UserCenter_UserInfo> userList, List<View_UserJoinRoleJoinDepartmen> roleList, IList<View_UserJoinDepartment> departmentList)
        {
            List<UserInfoJoinRoleJoinDepartment> list = new List<UserInfoJoinRoleJoinDepartment>();
            if (userList != null)
            {
                foreach (UserCenter_UserInfo userInfo in userList)
                {
                    int? departmentId = null;
                    string departmentName = null;
                    string roleNames = null;
                    View_UserJoinDepartment departmentInfo = departmentList.Where(obj => obj.UserName == userInfo.UserName).FirstOrDefault();
                    if (departmentInfo != null)
                    {
                        departmentId = departmentInfo.DepartmentID;
                        departmentName = departmentInfo.DepartmentName;
                    }
                    List<View_UserJoinRoleJoinDepartmen> nowRoleList = roleList.Where(obj => obj.UserName == userInfo.UserName).ToList();
                    if (nowRoleList != null)
                    {
                        StringBuilder sb = new StringBuilder("");
                        foreach (View_UserJoinRoleJoinDepartmen roleInfo in nowRoleList)
                        {
                            sb.Append(roleInfo.RoleName).Append(",");
                        }
                        roleNames = sb.ToString().TrimEnd(',');
                    }
                    UserInfoJoinRoleJoinDepartment obj2 = new UserInfoJoinRoleJoinDepartment(userInfo.UserName, userInfo.TrueName,userInfo.Mobile, userInfo.CompanyName, userInfo.UserValid, roleNames, departmentId, departmentName);
                    list.Add(obj2);
                }
            }
            return list;
        }
        /// <summary>
        /// 按小组和角色查询时,用此方法转换
        /// </summary>
        /// <param name="departmentList">主数据</param>
        /// <param name="userList"></param>
        /// <param name="roleList"></param>
        /// <returns></returns>
        public static List<UserInfoJoinRoleJoinDepartment> GetList( IList<View_UserJoinDepartment> departmentList,List<UserCenter_UserInfo> userList, List<View_UserJoinRoleJoinDepartmen> roleList)
        {
            List<UserInfoJoinRoleJoinDepartment> list = new List<UserInfoJoinRoleJoinDepartment>();
            if (departmentList != null)
            {
                foreach (View_UserJoinDepartment departmentInfo in departmentList)
                {
                    int? departmentId = departmentInfo.DepartmentID;
                    string departmentName = departmentInfo.DepartmentName;
                    string roleNames = null;
                    string trueName=null;
                    string companyName=null;
                    int userValid = 0;
                    string mobile = "";
                    UserCenter_UserInfo userInfo = userList.Where(obj => obj.UserName == departmentInfo.UserName).FirstOrDefault();
                    if (userInfo != null)
                    {
                        trueName = userInfo.TrueName;
                        companyName = userInfo.CompanyName;
                        userValid = userInfo.UserValid;
                        mobile = userInfo.Mobile;
                    }
                    List<View_UserJoinRoleJoinDepartmen> nowRoleList = roleList.Where(obj => obj.UserName == departmentInfo.UserName).ToList();
                    if (nowRoleList != null)
                    {
                        StringBuilder sb = new StringBuilder("");
                        foreach (View_UserJoinRoleJoinDepartmen roleInfo in nowRoleList)
                        {
                            sb.Append(roleInfo.RoleName).Append(",");
                        }
                        roleNames = sb.ToString().TrimEnd(',');
                    }
                    UserInfoJoinRoleJoinDepartment obj2 = new UserInfoJoinRoleJoinDepartment(departmentInfo.UserName, trueName,mobile, companyName, userValid, roleNames, departmentId, departmentName);
                    list.Add(obj2);
                }
            }
            return list;
        }
        /// <summary>
        /// 只按角色查询时,用此方法转换
        /// </summary>
        /// <param name="roleBaseList">主数据</param>
        /// <param name="departmentList"></param>
        /// <param name="userList"></param>
        /// <param name="roleList"></param>
        /// <returns></returns>
        public static List<UserInfoJoinRoleJoinDepartment> GetList( List<View_UserJoinRoleJoinDepartmen> roleBaseList,IList<View_UserJoinDepartment> departmentList, List<UserCenter_UserInfo> userList, List<View_UserJoinRoleJoinDepartmen> roleList)
        {
            List<UserInfoJoinRoleJoinDepartment> list = new List<UserInfoJoinRoleJoinDepartment>();
            if (roleBaseList != null)
            {
                foreach (View_UserJoinRoleJoinDepartmen roleBaseInfo in roleBaseList)
                {
                    int? departmentId = null;
                    string departmentName = null;
                    string roleNames = null;
                    string trueName = null;
                    string companyName = null;
                    int userValid = 0;
                    string mobile = "";
                    UserCenter_UserInfo userInfo = userList.Where(obj => obj.UserName == roleBaseInfo.UserName).FirstOrDefault();
                    if (userInfo != null)
                    {
                        trueName = userInfo.TrueName;
                        companyName = userInfo.CompanyName;
                        userValid = userInfo.UserValid;
                        mobile = userInfo.Mobile;
                    }
                    View_UserJoinDepartment departmentInfo = departmentList.Where(obj => obj.UserName == roleBaseInfo.UserName).FirstOrDefault();
                    if (departmentInfo != null)
                    {
                        departmentId = departmentInfo.DepartmentID;
                        departmentName = departmentInfo.DepartmentName;
                    }
                    List<View_UserJoinRoleJoinDepartmen> nowRoleList = roleList.Where(obj => obj.UserName == roleBaseInfo.UserName).ToList();
                    if (nowRoleList != null)
                    {
                        StringBuilder sb = new StringBuilder("");
                        foreach (View_UserJoinRoleJoinDepartmen roleInfo in nowRoleList)
                        {
                            sb.Append(roleInfo.RoleName).Append(",");
                        }
                        roleNames = sb.ToString().TrimEnd(',');
                    }
                    UserInfoJoinRoleJoinDepartment obj2 = new UserInfoJoinRoleJoinDepartment(roleBaseInfo.UserName, trueName,mobile, companyName, userValid, roleNames, departmentId, departmentName);
                    list.Add(obj2);
                }
            }
            return list;
        }
    }
}
