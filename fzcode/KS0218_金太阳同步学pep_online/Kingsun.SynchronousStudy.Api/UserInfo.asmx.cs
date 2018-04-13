using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudyHopeChina.Web.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;

namespace Kingsun.SynchronousStudy.Api
{
    /// <summary>
    /// UserInfo 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class UserInfo : System.Web.Services.WebService
    {

        [WebMethod]
        public bool UpdateUserInfo(int ID, string ParentMobile, string Name, string NameSpell, string ImagePath, int Gender, string IDNo, string Telephone, string School, string Grade, string ParentName, string ParentQQ, string ParentEmail, string TutorName, string TutorMobile, string TutorEmail, string TutorTelephone, string TutorPostAddress, string TutorQQ, string VedioFilePath, int IsJoin)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsImportUserInfoFromHopeChina"]))
            {
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["FZ_SynchronousStudy_Test"].ConnectionString;
                SqlSugar.SqlSugarClient db = new SqlSugar.SqlSugarClient(conStr);

                Tb_UserInfoFromHopeChinaWeb entityOfSynchronousStudy = new Tb_UserInfoFromHopeChinaWeb()
                {
                    ID = ID,
                    ParentMobile = ParentMobile,
                    Name = Name,
                    NameSpell = NameSpell,
                    ImagePath = ImagePath,
                    Gender = Gender,
                    IDNo = IDNo,
                    Telephone = Telephone,
                    School = School,
                    Grade = Grade,
                    ParentName = ParentName,
                    ParentQQ = ParentQQ,
                    ParentEmail = ParentEmail,
                    TutorName = TutorName,
                    TutorMobile = TutorMobile,
                    TutorEmail = TutorEmail,
                    TutorTelephone = TutorTelephone,
                    TutorPostAddress = TutorPostAddress,
                    TutorQQ = TutorQQ,
                    VedioFilePath = VedioFilePath,
                    IsJoin = IsJoin > 0,
                    CreateTime = DateTime.Now.ToString(),
                    IsDel = false
                };
                    return db.Update<Tb_UserInfoFromHopeChinaWeb>(entityOfSynchronousStudy);
            }
            else
            {
                return true;
            }      
        }

        [WebMethod]
        public bool InsertUserInfo(int ID, string ParentMobile, string Name, string NameSpell, string ImagePath, int Gender, string IDNo, string Telephone, string School, string Grade, string ParentName, string ParentQQ, string ParentEmail, string TutorName, string TutorMobile, string TutorEmail, string TutorTelephone, string TutorPostAddress, string TutorQQ, string VedioFilePath, int IsJoin)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsImportUserInfoFromHopeChina"]))
            {
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["FZ_SynchronousStudy_Test"].ConnectionString;
                SqlSugar.SqlSugarClient db = new SqlSugar.SqlSugarClient(conStr);

                Tb_UserInfoFromHopeChinaWeb entityOfSynchronousStudy = new Tb_UserInfoFromHopeChinaWeb()
                {
                    ID = ID,
                    ParentMobile = ParentMobile,
                    Name = Name,
                    NameSpell = NameSpell,
                    ImagePath = ImagePath,
                    Gender = Gender,
                    IDNo = IDNo,
                    Telephone = Telephone,
                    School = School,
                    Grade = Grade,
                    ParentName = ParentName,
                    ParentQQ = ParentQQ,
                    ParentEmail = ParentEmail,
                    TutorName = TutorName,
                    TutorMobile = TutorMobile,
                    TutorEmail = TutorEmail,
                    TutorTelephone = TutorTelephone,
                    TutorPostAddress = TutorPostAddress,
                    TutorQQ = TutorQQ,
                    VedioFilePath = VedioFilePath,
                    IsJoin = IsJoin > 0,
                    CreateTime = DateTime.Now.ToString(),
                    IsDel = false
                };

                    db.Insert<Tb_UserInfoFromHopeChinaWeb>(entityOfSynchronousStudy);
                    return true;
            }
            else
            {
                return true;
            }
        }
    }
}
