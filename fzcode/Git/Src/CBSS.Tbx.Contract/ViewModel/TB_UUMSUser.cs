using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class TB_UUMSUser
    {
        public int? State { get; set; }

        public int? IsAvatarTrue { get; set; }

        public string GradeStartYear { get; set; }

        public string SubjectName { get; set; }

        public string AeraID { get; set; }

        public string PersonID { get; set; }

        public int? StageID { get; set; }

        /// <summary>
        /// 性别 。1代表男2代表女
        /// </summary>
        public int? Gender { get; set; }

        public string AppID { get; set; }

        public string LastLogContry { get; set; }

        public int? SchoolID { get; set; }

        public int? GradeID { get; set; }

        public string AvatarUrl { get; set; }

        public DateTime? UploadAvatarDate { get; set; }

        public int? SubjectID { get; set; }

        public int? ProID { get; set; }

        public int? CityID { get; set; }

        public string Host { get; set; }

        public DateTime? RegDate { get; set; }

        public int? LoginNum { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string Grade { get; set; }

        public string SchoolName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        /// <summary>
        /// 在第二次迭代中该字段用来标识用户
        ///0-UUMS超级管理员、1-普通用户
        /// </summary>
        public int? UserType { get; set; }

        public string UserID { get; set; }

        public string UserName { get; set; }

        public string TrueName { get; set; }

        public string PassWord { get; set; }
    }
}
