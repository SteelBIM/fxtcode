using FxtCenterServiceOpen.Actualize.VQOperater;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FxtCenterServiceOpen.Hosting
{
    public partial class ReSend : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string path = Server.MapPath("") + "\\upload\\VQCenterData";
                    DirectoryInfo dir = new DirectoryInfo(path);
                    FileInfo[] inf = dir.GetFiles();
                    foreach (FileInfo file in inf)
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();

                        dt.ReadXml(file.FullName);

                        int nub = 0;
                        string error = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (Convert.ToInt32(dr["state"]) == 0)
                            {
                                nub++;
                                error += nub + ":" + dr["error"].ToString();
                            }
                        }
                        //显示未发送的
                        if (nub > 0)
                        {
                            Response.Write("<div>文件名:#" + file.Name + "#  未发送:" + nub + "  文件位置:" + file.FullName + "</div>");
                            Response.Write("<div>异常:" + error + "</div>");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write("<div>异常" + ex.ToString() + "</div>");
            }
        }

        protected void BtnSendFileInfo_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Server.MapPath("") + "\\upload\\VQCenterData\\VQCenterData" + txtSendFileName.Text;
                if (File.Exists(path))
                {
                    int sendCount = 0;
                    int sendSuccess = 0;
                    CallOperaterCenter.NotSendDataToCenter(path, out sendCount, out sendSuccess);

                    Response.Write("<div>发送数:" + sendCount + "  成功数:" + sendSuccess + " 如发送不成功请人工检查数据问题</div>");
                }
                else
                {
                    Response.Write("<div>文件不存在: " + path + "</div>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<div>异常" + ex.ToString() + "</div>");
            }
        }
    }
}