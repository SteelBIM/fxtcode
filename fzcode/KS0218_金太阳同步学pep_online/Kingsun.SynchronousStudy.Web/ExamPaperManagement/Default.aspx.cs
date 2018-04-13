using System;
using System.Linq;
using Kingsun.PSO;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Web.ExamPaperManagement;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.IBS.Model;
using System.Collections.Generic;

namespace Kingsun.SynchronousStudy.Web.ExamPaperManagement
{
    public partial class _Default : BasePage
    {

        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        public string menuHtml = "";
        public ClientUserinfo UserInfo = new ClientUserinfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo = CurrentUser;
            List<PowerList> tmpArray = userBLL.GetUserPowerList(UserInfo.UserID, AppSetting.AppID);

            if (tmpArray != null)
            {
                //先获取父目录
                List<PowerList> topArray = tmpArray.Where(a => a.ParentID == tmpArray[0].funID).ToList();
                for (int i = 0; i < topArray.Count; i++)
                {
                    if (topArray[i].IsShow.GetValueOrDefault() == 0)
                    {
                        continue;
                    }
                    menuHtml += "<li><a href=\"javascript:void(0)\" title=\"" + topArray[i].Name + "\" class=\"collapsed\" target=\"iframe1\">" + topArray[i].Name + "</a><ul>";
                    //获取相应的子目录并排序
                    List<PowerList> childArray = tmpArray.Where(s => s.ParentID == topArray[i].funID).OrderBy(s => s.Order).ToList();
                    for (int j = 0; j < childArray.Count; j++)
                    {
                        if (childArray[j].IsShow.GetValueOrDefault() == 0)
                        {
                            continue;
                        }
                        menuHtml += "<li><a href=\"" + childArray[j].LinkUrl + "\" target=\"iframe1\"><i></i>" + childArray[j].Name + "</a></li>";
                    }
                    menuHtml += "</ul></li>";
                }
            }
        }
    }
}