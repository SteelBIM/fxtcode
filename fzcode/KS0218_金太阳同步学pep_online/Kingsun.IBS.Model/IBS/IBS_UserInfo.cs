using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class IBS_UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserNum { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>0
        /// 密码
        /// </summary>
        public string UserPwd { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string UserImage { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public int UserRoles { get; set; }

        /// <summary>
        /// 用户身份
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 是否是oss文件（0：否，1：是）
        /// </summary>
        public int IsEnableOss { get; set; }

        /// <summary>
        /// 用户标识号
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public int DeviceType { get; set; }        

        /// <summary>
        /// 设备号
        /// </summary>
        public string EquipmentID { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 是否第一次登陆
        /// </summary>
        public string isLogState { get; set; }

        /// <summary>
        /// 用户学校ID
        /// </summary>
        public int? SchoolID { get; set; }

        /// <summary>
        /// 用户学校名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 用户状态 0-正常 1-异常
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 是否被禁用
        /// </summary>
        public int IsUser { get; set; }        

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string TelePhone { get; set; }

        /// <summary>
        /// 联系电话，默认手机号
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? Regdate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// 班级学校列表，学生一条数据，老师多条数据
        /// </summary>
        public List<ClassSch> ClassSchList { get; set; }

        public IBS_UserInfo() 
        {
            ClassSchList = new List<ClassSch>();
        }
    }

    /// <summary>
    /// 班级学校，学生一条数据，老师多条数据
    /// </summary>
    public class ClassSch
    {
        /// <summary>
        /// 班级ID
        /// </summary>
        public string ClassID { get; set; }

        /// <summary>
        /// 年级ID
        /// </summary>
        public int GradeID { get; set; }

        /// <summary>
        /// 学校ID
        /// </summary>
        public int SchID { get; set; }
        /// <summary>
        /// 学科ID（针对老师）
        /// </summary>
        public int SubjectID { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        public int AreaID { get; set; }
    }

    public class ClassInfoByUserID
    {
        public int UserID { get; set; }

        public string ClassID { get; set; }

        public string ClassNum { get; set; }
    }
}
