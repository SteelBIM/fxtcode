using System;

namespace Kingsun.IBS.Model
{
   public class MOD2IBS_TempStu
   {
       private bool _isexcuted = false;
       /// <summary>
       ///  
       /// </summary>
       public int  UserID { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public string UserNum { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public string  UserName { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public string  UserPwd { get; set; }


       /// <summary>
       /// 头像
       /// </summary>
       public string UserImage { get; set; }

       /// <summary>
       /// 用户角色
       /// </summary>
       public int UserRoles { get; set; }
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
       /// 用户状态 0-正常 1-异常
       /// </summary>
       public int State { get; set; }

       /// <summary>
       /// 个人中心学校ID
       /// </summary>
       public int? SchoolID { get; set; }
       /// <summary>
       /// 个人中心学校名称
       /// </summary>
       public string SchoolName { get; set; }
       /// <summary>
       /// 是否被禁用
       /// </summary>
       public int IsUser { get; set; }       
       /// <summary>
       ///  
       /// </summary>
       public int?  UserType { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public string  TrueName { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public string  TelePhone { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public DateTime?  Regdate { get; set; }

       public string AppID { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public Guid?  ClassID { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public string  ClassName { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public string  ClassNum { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public int?  GradeID { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public int?  SubjectID { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public int?  SchID { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public string  SchName { get; set; }

       /// <summary>
       ///  
       /// </summary>
       public int?  AreaID { get; set; }
   }

    public class MOD_User
    {
        public string UserID { get; set; }

        public string TrueName { get; set; }
    }
}
