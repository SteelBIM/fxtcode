using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.Common;
using System.Web.Configuration;

namespace Kingsun.SynchronousStudy.Web.MessageManagement
{
    public partial class AddMessagePush : System.Web.UI.Page
    {
        public string type;
        public string id;
        public string FileServerUrl = ConfigurationManager.AppSettings["getFiles"];
        public string file = ConfigurationManager.AppSettings["FileServerUrl"];
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request.QueryString["ID"];
            type = Request.QueryString["Type"];
            if (!IsPostBack)
            {
                BindVersion();
                BindDate();
            }
        }

        private void BindDate()
        {
            if (type == "Serach")
            {
                btnAdd.Enabled = false;
                rbTitleState.Enabled = false;
                //file_upload.Attributes["style"]= "display:none";
                //file_upload.Attributes.CssStyle.Value = "display:none";
            }
            string sql = string.Format(@"SELECT *  FROM [TB_MessagePush] Where id='{0}'", id);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtTitle.Text = ds.Tables[0].Rows[0]["MessageTitle"].ToString();
                txtStartDate.Value = ds.Tables[0].Rows[0]["StartTime"].ToString();
                txtEndDate.Value = ds.Tables[0].Rows[0]["EndTime"].ToString();
                fileinfo.Value = ds.Tables[0].Rows[0]["Image"].ToString();
                fileinfo1.Value = ds.Tables[0].Rows[0]["ButtonImage"].ToString();
                ddlVersion.SelectedValue = ds.Tables[0].Rows[0]["PushEdition"].ToString();
                txtLink.Text = ds.Tables[0].Rows[0]["JumpLink"].ToString();
                txtUseTime.Text = ds.Tables[0].Rows[0]["UseTime"].ToString();
                txtNumber.Text = ds.Tables[0].Rows[0]["Number"].ToString();
                txtClassID.Text = ds.Tables[0].Rows[0]["ClassID"].ToString();
                txtDesc.Text = ds.Tables[0].Rows[0]["TestDsc"].ToString();
                Image1.ImageUrl = FileServerUrl + "?FileID=" + ds.Tables[0].Rows[0]["Image"];
                btnImg.ImageUrl = FileServerUrl + "?FileID=" + ds.Tables[0].Rows[0]["ButtonImage"];
                rbTitleState.SelectedValue = ds.Tables[0].Rows[0]["TitleState"].ToString();
            }
        }

        private void BindVersion()
        {
            string sql = @"SELECT [VersionID]
                                  ,[VersionName]
                              FROM [TB_APPManagement]";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

            ddlVersion.DataSource = ds.Tables[0];
            ddlVersion.DataValueField = ds.Tables[0].Columns[0].ColumnName;
            ddlVersion.DataTextField = ds.Tables[0].Columns[1].ColumnName;
            ddlVersion.DataBind();
            ddlVersion.Items.Insert(0, new ListItem("请选择", "0"));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim() == "")
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('标题不能为空！');</script>");
                return;
            }
            if (ddlVersion.SelectedValue == "0")
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('请选择推送版本！');</script>");
                return;
            }
            if (txtStartDate.Value.Trim() == "")
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('起始时间不能为空！');</script>");
                return;
            }
            if (txtEndDate.Value.Trim() == "")
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('结束时间不能为空！');</script>");
                return;
            }
            if (txtDesc.Text.Trim() == "")
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('文本内容不能为空！');</script>");
                return;
            }
            string sql = "";
            if (type == "Updata")
            {
                //                sql = string.Format(@"UPDATE [TB_MessagePush]
                //                                       SET [MessageTitle] = '{0}',[Image] = '{1}',[ButtonImage] = '{2}',[PushEdition] = '{3}',[JumpLink] ='{4}',
                //                                            [UseTime] = '{5}',[Number] ='{6}',[ClassID] = '{7}',[TestDsc] = '{8}',[StartTime] = '{9}',[EndTime] = '{10}',TitleState='{12}' WHERE id='{11}'",
                //                                            String2Json(txtTitle.Text), fileinfo.Value, fileinfo1.Value, ddlVersion.SelectedItem.Value, String2Json(txtLink.Text), ParseTime(ddlTime.SelectedItem.Text, ParseInt(txtUseTime.Text)),
                //                                            String2Json(txtNumber.Text), String2Json(txtClassID.Text), String2Json(txtDesc.Text), txtStartDate.Value, txtEndDate.Value, id, rbTitleState.SelectedValue);
                sql = string.Format(@"UPDATE [TB_MessagePush]
                                       SET [MessageTitle] = @MessageTitle,[Image] = @Image,[ButtonImage] = @ButtonImage,[PushEdition] = @PushEdition,[JumpLink] =@JumpLink,
                                            [UseTime] = @UseTime,[Number] =@Number,[ClassID] = @ClassID,[TestDsc] =@TestDsc,[StartTime] =@StartTime,[EndTime] = @EndTime,TitleState=@TitleState WHERE id=@id",
                                           String2Json(txtTitle.Text), fileinfo.Value, fileinfo1.Value, ddlVersion.SelectedItem.Value, String2Json(txtLink.Text), ParseTime(ddlTime.SelectedItem.Text, ParseInt(txtUseTime.Text)),
                                           String2Json(txtNumber.Text), String2Json(txtClassID.Text), String2Json(txtDesc.Text), txtStartDate.Value, txtEndDate.Value, id, rbTitleState.SelectedValue);

            }
            else
            {
                //                sql = string.Format(@"INSERT INTO [TB_MessagePush] ([MessageTitle],[Image],[ButtonImage],[PushEdition],[JumpLink],[UseTime],[Number],[ClassID],[TestDsc],[StartTime],[EndTime],[State],[TitleState])
                //     VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',1,'{11}')", String2Json(txtTitle.Text), fileinfo.Value, fileinfo1.Value, ddlVersion.SelectedItem.Value, String2Json(txtLink.Text), ParseTime(ddlTime.SelectedItem.Text, ParseInt(txtUseTime.Text)),
                //                                                                                      String2Json(txtNumber.Text), String2Json(txtClassID.Text), String2Json(txtDesc.Text), txtStartDate.Value, txtEndDate.Value, rbTitleState.SelectedValue);
                sql = string.Format(@"INSERT INTO [TB_MessagePush] ([MessageTitle],[Image],[ButtonImage],[PushEdition],[JumpLink],[UseTime],[Number],[ClassID],[TestDsc],[StartTime],[EndTime],[State],[TitleState])
     VALUES(@MessageTitle,@Image,@ButtonImage,@PushEdition,@JumpLink,@UseTime,@Number,@ClassID,@TestDsc,@StartTime,@EndTime,@State,@TitleState)", String2Json(txtTitle.Text), fileinfo.Value, fileinfo1.Value, ddlVersion.SelectedItem.Value, String2Json(txtLink.Text), ParseTime(ddlTime.SelectedItem.Text, ParseInt(txtUseTime.Text)),
                                                                                                    String2Json(txtNumber.Text), String2Json(txtClassID.Text), String2Json(txtDesc.Text), txtStartDate.Value, txtEndDate.Value, rbTitleState.SelectedValue);

                //将图片保存为永久文件
                if (!string.IsNullOrEmpty(fileinfo.Value))
                {
                    string field = file + "ConfirmHandler.ashx";
                    //get请求
                    string dd = JsonHelper.EncodeJson(fileinfo.Value);
                    string values = "t=[" + dd + "]";
                    HttpGet(field, values);
                }

                //将图片保存为永久文件
                if (!string.IsNullOrEmpty(fileinfo1.Value))
                {
                    string field = file + "ConfirmHandler.ashx";
                    //get请求
                    string dd = JsonHelper.EncodeJson(fileinfo1.Value);
                    string values = "t=[" + dd + "]";
                    HttpGet(field, values);
                }
            }
            SqlParameter[] sp =
            {
                new SqlParameter("@MessageTitle",SqlDbType.NVarChar),
                new SqlParameter("@Image",SqlDbType.NVarChar),
                new SqlParameter("@ButtonImage",SqlDbType.NVarChar),
                new SqlParameter("@PushEdition",SqlDbType.NVarChar),
                new SqlParameter("@JumpLink",SqlDbType.NVarChar),
                new SqlParameter("@UseTime",SqlDbType.Int),
                new SqlParameter("@Number",SqlDbType.Int),
                new SqlParameter("@ClassID",SqlDbType.Int),
                new SqlParameter("@TestDsc",SqlDbType.NVarChar),
                new SqlParameter("@StartTime",SqlDbType.DateTime),
                new SqlParameter("@EndTime",SqlDbType.DateTime),
                new SqlParameter("@State",SqlDbType.Int),
                new SqlParameter("@TitleState",SqlDbType.Int),
                new SqlParameter("@ID",SqlDbType.Int),
            };

            sp[0].Value = txtTitle.Text;
            sp[1].Value = fileinfo.Value == "" ? "00000000-0000-0000-0000-000000000000" : fileinfo.Value;
            sp[2].Value = fileinfo1.Value == "" ? "00000000-0000-0000-0000-000000000000" : fileinfo1.Value;
            sp[3].Value = ddlVersion.SelectedItem.Value;
            sp[4].Value = txtLink.Text;
            sp[5].Value = ParseTime(ddlTime.SelectedItem.Text, ParseInt(txtUseTime.Text));
            sp[6].Value = ParseInt(txtNumber.Text);
            sp[7].Value = ParseInt(txtClassID.Text);
            sp[8].Value = txtDesc.Text;
            sp[9].Value = txtStartDate.Value;
            sp[10].Value = txtEndDate.Value;
            sp[11].Value = 1;
            sp[12].Value = ParseInt(rbTitleState.SelectedValue);
            sp[13].Value = ParseInt(id);

            if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql, sp) > 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "", "<script type=\"text/javascript\">alert('保存成功！');</script>");
                Response.Redirect("../MessageManagement/MessagePush.aspx", true);
            }

        }

        public static int ParseTime(string obj, int time)
        {
            int i = 0;
            switch (obj)
            {
                case "分":
                    i = time * 60;
                    break;
                case "时":
                    i = time * 3600;
                    break;
                default:
                    i = time;
                    break;
            }
            return i;
        }


        /// <summary>
        /// get 请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }


        /// <summary>
        /// 转换int型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ParseInt(object obj)
        {
            int reInt = -1;
            if (obj != null)
                int.TryParse(obj.ToString(), out reInt);
            return reInt;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("../MessageManagement/MessagePush.aspx", true);
        }

        /// <summary> 
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        private static string String2Json(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    case '+':
                        sb.Append("\\n"); break;
                    case '\'':
                        sb.Append("\'\'"); break;
                    case '<':
                        sb.Append("&lt;"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

    }
}