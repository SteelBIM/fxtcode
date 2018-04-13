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
       /// ͷ��
       /// </summary>
       public string UserImage { get; set; }

       /// <summary>
       /// �û���ɫ
       /// </summary>
       public int UserRoles { get; set; }
       /// <summary>
       /// �Ƿ���oss�ļ���0����1���ǣ�
       /// </summary>
       public int IsEnableOss { get; set; }

       /// <summary>
       /// �û���ʶ��
       /// </summary>
       public string Token { get; set; }

       /// <summary>
       /// �豸����
       /// </summary>
       public int DeviceType { get; set; }

       /// <summary>
       /// �豸��
       /// </summary>
       public string EquipmentID { get; set; }

       /// <summary>
       /// IP��ַ
       /// </summary>
       public string IPAddress { get; set; }

       /// <summary>
       /// �Ƿ��һ�ε�½
       /// </summary>
       public string isLogState { get; set; }

       /// <summary>
       /// �û�״̬ 0-���� 1-�쳣
       /// </summary>
       public int State { get; set; }

       /// <summary>
       /// ��������ѧУID
       /// </summary>
       public int? SchoolID { get; set; }
       /// <summary>
       /// ��������ѧУ����
       /// </summary>
       public string SchoolName { get; set; }
       /// <summary>
       /// �Ƿ񱻽���
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
