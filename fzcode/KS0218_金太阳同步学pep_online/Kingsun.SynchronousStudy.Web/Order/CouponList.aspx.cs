using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.Order
{
    public partial class CouponList : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string Action = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ActionInit();
            }
        }
        TicketBLL tickBll = new TicketBLL();
        CouponBll bll = new CouponBll();
        APPManagementBLL appVersionBll = new APPManagementBLL();
        private void ActionInit()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Action"]))//获取form的Action中的参数 
                {
                    Action = Request.QueryString["Action"].Trim().ToLower();//去掉空格并变小写 
                }
                else
                {
                    return;
                }
                switch (Action)
                {
                    case "query":
                        string strWhere = "";
                        int totalcount = 0;
                        List<CouponListModel> couponList = new List<CouponListModel>();
                        if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                        {
                            var obj1 = new { rows = couponList, total = totalcount };
                            Response.Write(JsonHelper.EncodeJson(obj1));
                            Response.End();
                        }
                        int pageindex = int.Parse(Request.Form["page"].ToString());
                        int pagesize = int.Parse(Request.Form["rows"].ToString());
                        if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                        {
                            strWhere = Request.QueryString["queryStr"];
                        }
                        strWhere += " and Status=0";
                        couponList = bll.GetTicketListByStrWhere(strWhere);
                        if (couponList == null)
                        {
                            couponList = new List<CouponListModel>();
                        }
                        else
                        {
                            totalcount = couponList.Count;
                            IList<CouponListModel> removelist = new List<CouponListModel>();
                            for (int i = 0; i < couponList.Count; i++)
                            {
                                if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                                {
                                    removelist.Add(couponList[i]);
                                }
                            }
                            if (removelist != null && removelist.Count > 0)
                            {
                                for (int i = 0; i < removelist.Count; i++)
                                {
                                    couponList.Remove(removelist[i]);
                                }
                            }
                        }
                        var obj = new { rows = couponList, total = totalcount };
                        Response.Write(JsonHelper.EncodeJson(obj));
                       
                        break;
                    case "save":
                        save();
                        Response.End();
                        break;
                    case "existssave":
                        existssave();
                        Response.End();
                        break;
                    case "update":
                        update();
                        Response.End();
                        break;
                    case "del":
                        del();
                        Response.End();
                        break;
                    case "getappversionlist":
                        GetAppVersionList();
                        Response.End();
                        break;
                    case "getticketmodel":
                        GetTicketModel();
                        Response.End();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            Response.End();
        }
        public void del()
        {
            try
            {
                string Edition = Request["EditionID"];
                bool flag = bll.DelTicketInfo(Edition);
                if (flag)
                {
                    Response.Write("{'state':'1','msg':'删除成功'}");
                }
                else
                {
                    Response.Write("{'state':'0','msg':'删除失败'}");
                }
            }
            catch (Exception)
            {
                Response.Write("{'state':'0','msg':'删除失败'}");
            }
        }
        /// <summary>
        /// 修改卷（时间和状态）
        /// </summary>
        public void update()
        {
            try
            {
                string Edition = Request.Form["Edition"];
                string StartDate = Request.Form["StartDate"];
                string EndDate = Request.Form["EndDate"];
                int Status = Convert.ToInt32(Request.Form["Status"]);
                //1.判断当前版本新添加的时间是否有重合 
                //bool existsTime = bll.CheckExistsTime(Edition, StartDate, EndDate, "or '" + EndDate + "'<GETDATE()");
                //if (existsTime || Status == 1)//没有重合  
                //{
                bool flag = bll.UpdateTicket(Edition, StartDate, EndDate, Status);
                if (flag)
                {
                    Response.Write("{'state':'1','msg':'修改成功'}");
                }
                else
                {
                    Response.Write("{'state':'1','msg':'修改失败'}");
                }
                //}
                //else
                //{
                //    Response.Write("{'state':'2','msg':'当前版本的使用卷的时间有重叠'}");
                //}
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'操作异常'}");
            }
        }
        /// <summary>
        /// 卷存在改状态，并保存
        /// </summary>
        public void existssave()
        {
            try
            {
                string Edition = Request.Form["Edition"];
                string selectType = Request.Form["selectType"];
                string TicketName = Request.Form["TicketName"];
                string Price = Request.Form["Price"];
                string StartDate = Request.Form["StartDate"];
                string EndDate = Request.Form["EndDate"];
                string ImgUrl = Request.Form["ImgUrl"];
                string selectStatus = Request.Form["selectStatus"];
                //1.判断当前版本新添加的时间是否有重合
                bool existsTime = bll.CheckExistsTime(Edition, StartDate, EndDate, "");
                if (existsTime)//没有重合  
                {
                    //2.修改状态，并保存
                    List<TB_CurriculumManage> curList = bll.GetCurriculumManageList(Edition);
                    if (curList != null && curList.Count > 0)
                    {
                        List<string> list = new List<string>();
                        string sql_update = "update TB_Ticket set Status=1 where CourseID in(select BookID from TB_CurriculumManage where EditionID='" + Edition + "') ";
                        list.Add(sql_update);
                        foreach (var item in curList)
                        {
                            string sql = string.Format(@"insert into TB_Ticket(TicketName,CourseID,ModularID,Price,StartDate,EndDate,Type,ImgUrl,Status)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                 TicketName, Convert.ToInt32(item.BookID), 14, Convert.ToDecimal(Price), Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), selectType, ImgUrl, selectStatus);
                            list.Add(sql);
                        }
                        bool flag = tickBll.AddTicketInfo(list);
                        if (flag)
                        {
                            Response.Write("{'state':'1','msg':'添加成功'}");
                        }
                        else
                        {
                            Response.Write("{'state':'0','msg':'添加失败'}");
                        }
                    }
                    else
                    {
                        Response.Write("{'state':'0','msg':'没有找到该版本下的书册'}");
                    }
                }
                else
                {
                    Response.Write("{'state':'2','msg':'当前版本的使用卷的时间有重叠'}");
                }

            }
            catch (Exception ex)
            {
                Response.Write("{'state':'0','msg':'" + ex + "'}");
            }
        }
        public void save()
        {
            try
            {
                string Edition = Request.Form["Edition"];
                string selectType = Request.Form["selectType"];
                string TicketName = Request.Form["TicketName"];
                string Price = Request.Form["Price"];
                string StartDate = Request.Form["StartDate"];
                string EndDate = Request.Form["EndDate"];
                string ImgUrl = Request.Form["ImgUrl"];
                string selectStatus = Request.Form["selectStatus"];
                List<CouponListModel> existsList = bll.GetTicketListByStrWhere(" and EditionID=" + Edition + "");
                if (existsList.Count > 0)
                {
                    Response.Write("{'state':'2','msg':'当前版本的使用卷正在使用，是否停用？'}");
                    return;
                }
                List<TB_CurriculumManage> curList = bll.GetCurriculumManageList(Edition);
                if (curList != null && curList.Count > 0)
                {
                    List<string> list = new List<string>();
                    foreach (var item in curList)
                    {
                        string sql = string.Format(@"insert into TB_Ticket(TicketName,CourseID,ModularID,Price,StartDate,EndDate,Type,ImgUrl,Status)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                             TicketName, Convert.ToInt32(item.BookID), 14, Convert.ToDecimal(Price), Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), selectType, ImgUrl, selectStatus);
                        list.Add(sql);
                    }
                    bool flag = tickBll.AddTicketInfo(list);
                    if (flag)
                    {
                        Response.Write("{'state':'1','msg':'添加成功'}");
                    }
                    else
                    {
                        Response.Write("{'state':'0','msg':'添加失败'}");
                    }
                }
                else
                {
                    Response.Write("{'state':'0','msg':'没有找到该版本下的书册'}");
                }
            }
            catch (Exception ex)
            {
                Response.Write("{'state':'0','msg':'" + ex + "'}");
            }
        }
        public void GetAppVersionList()
        {
            try
            {
                IList<TB_APPManagement> app = appVersionBll.QueryAPPList();
                string json = JsonHelper.EncodeJson(app);
                Response.Write(json);
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write("");
            }

        }
        public void query()
        {

        }
        /// <summary>
        /// 根据版本ID获取优惠卷信息
        /// </summary>
        public void GetTicketModel()
        {
            try
            {
                string Edition = Request.Form["Edition"];
                List<CouponListModel> couponList = bll.GetTicketListByStrWhere("and EditionID='" + Edition + "' and Status=0");
                string json = JsonHelper.EncodeJson(couponList);
                Response.Write(json);
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write("");
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile == false)//HasFile用来检查FileUpload是否有指定文件 
            {
                Response.Write("<script>alert('请您选择Excel文件')</script> ");
                return;//当无文件时,返回 
            }
            string myFileName = "";
            string filename = FileUpload1.FileName;//获取Execle文件名  DateTime日期函数
            string IsXls = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
            if (IsXls != ".xls")
            {
                if (IsXls != ".xlsx")
                {
                    Response.Write("<script>alert('只可以选择Excel文件')</script>");
                    return;//当选择的不是Excel文件时,返回
                }
                else
                {
                    myFileName = filename.Substring(0, filename.Length - 5);
                }
            }
            else
            {
                myFileName = filename.Substring(0, filename.Length - 4);
            }

            string savePath = Server.MapPath(("~/upload/") + filename);//Server.MapPath 获得虚拟服务器相对路径
            FileUpload1.SaveAs(savePath);                        //SaveAs 将上传的文件内容保存在服务器上
            DataSet ds = ExcelSqlConnection(savePath, filename, myFileName, IsXls);           //调用自定义方法
            DataRow[] dr = ds.Tables[0].Select();            //定义一个DataRow数组
            int rowsnum = ds.Tables[0].Rows.Count;
            if (rowsnum == 0)
            {
                Response.Write("<script>alert('Excel表为空表,无数据!')</script>");   //当Excel表为空时,对用户进行提示
            }
            else
            {
                for (int i = 0; i < dr.Length / 3; i++)  //循环根节点，根据模板判断，根节点数等于行数/3（dr.Length/3）
                {
                    string DocumentParentClassified = dr[i * 3]["Document Parent Classified"].ToString();
                    //cD.AddparentclassifiedDate(((i * 3) + 1), DocumentParentClassified);//向根节点表中插入数据
                    for (int j = i * 3; j < (i + 1) * 3; j++)//循环枝干节点
                    {
                        string DocumentChildClassified = dr[j]["Document Child Classified"].ToString();
                        //cD.AddchildclassifiedDate(((i * 3) + 1), j + 1, DocumentChildClassified);//向枝干节点表中插入数据3
                        string S = dr[j]["S"].ToString();
                        string M = dr[j]["M"].ToString();
                        string L = dr[j]["L"].ToString();
                        //cD.AddclicktimesDate(j + 1, S, M, L);
                        for (int k = 2; k <= 4; k++)//循环叶子节点
                        {
                            string DocumentDefiniteClassified = dr[j][k].ToString();
                            if (DocumentDefiniteClassified == "")
                            {
                                DocumentDefiniteClassified = "*";
                            }
                            int state = 1;
                            int num = j * 3 + k;
                            // cD.AdddefiniteclassifiedDate(((i * 3) + 1), j + 1, num, DocumentDefiniteClassified, state);//向叶子节点表中插入数据
                        }
                    }
                }
                Response.Write("<script>alert('Excle表导入成功!');</script>");
            }
        }

        #region 连接Excel  读取Excel数据   并返回DataSet数据集合
        ///<summary>连接Excel  读取Excel数据   并返回DataSet数据集合62       
        /// </summary>63        
        /// <param name="filepath">Excel服务器路径</param>64      
        /// <param name="tableName">Excel表名称</param>65       
        /// <returns></returns>
        public static System.Data.DataSet ExcelSqlConnection(string filepath, string tableName, string FileName, string isxls)
        {
            string strCon = "";
            if (isxls == ".xls")
            {
                strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
            }
            else
            {
                strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
            }
            OleDbConnection ExcelConn = new OleDbConnection(strCon);
            try
            {
                string strCom = string.Format("SELECT * FROM [{0}$]", FileName);
                ExcelConn.Open();
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, ExcelConn);
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "[" + tableName + "$]");
                ExcelConn.Close();
                return ds;
            }
            catch
            {
                ExcelConn.Close();
                return null;
            }
        }
        #endregion
    }

}