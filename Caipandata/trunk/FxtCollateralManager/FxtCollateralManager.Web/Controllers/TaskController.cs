using FxtCollateralManager.Common;
using FxtCollateralManager.Common.FxtAPI;
using FxtCommonLibrary.LibraryUtils;
using FxtNHibernate.DTODomain.FxtLoanDTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


/*作者:贺黎亮
 * 时间:2014.04.24
 *    摘要:新建 任务
  * 时间:2014.06.20
 *    摘要:修改押品复估导出方法
 *
 * 
 * **/
namespace FxtCollateralManager.Web.Controllers
{
    public class TaskController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 任务执行日志列表
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskLog()
        {
            return View();
        }

        //获取任务列表
        public ActionResult GetTaskList(int pageIndex = 0, int pageSize = 0
            , int id = 0, string projectname = "", int bankid = 0, int status = -1)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("pageIndex", pageIndex);
                obj.Add("pageSize", pageSize);
                obj.Add("id", id);
                obj.Add("key", projectname);
                obj.Add("bankid", bankid);
                obj.Add("status", status);
                obj.Add("orderProperty", "  id desc ");
                obj.Add("customerid", Public.LoginInfo.CustomerId);
                obj.Add("customertype", Public.LoginInfo.CustomerType);
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetTaskList, Utils.Serialize(obj)).ToString().ToLower());
            }
        }

        //获取任务日志列表
        public ActionResult GetTaskLogList(int pageIndex = 0, int pageSize = 0
            , int taskid = 0, string key = "")
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("pageIndex", pageIndex);
                obj.Add("pageSize", pageSize);
                obj.Add("taskid", taskid);
                obj.Add("key", key);
                obj.Add("orderProperty", "  id desc ");
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _GetTaskLogList, Utils.Serialize(obj)).ToString().ToLower());
            }
        }



        //修改任务状态
        public ActionResult EditTaskStatus(int id, int status)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject obj = new JObject();
                obj.Add("status", status);
                obj.Add("id", id);
                return Json(client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _EditTaskStatus, Utils.Serialize(obj)).ToString().ToLower());
            }
        }



        //复估导出
        public FileResult TaskExport(int uploadfileid)
        {
            string[] array = getDownFile(string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddffffff")));
            List<JObject> _explistData = new List<JObject>();
            var sbHtml = new StringBuilder();
            Dictionary<string, int> diccol = new Dictionary<string, int>();
            diccol.Add("系统编号", 100);
            diccol.Add("押品编号", 100);
            diccol.Add("押品名称", 300);
            diccol.Add("押品地址", 300);
            diccol.Add("楼盘名", 150);
            diccol.Add("银行", 100);
            diccol.Add("楼盘", 100);
            diccol.Add("楼栋", 100);
            diccol.Add("房号", 100);
            diccol.Add("自动复估值", 100);
            diccol.Add("现复估值", 100);
            diccol.Add("楼盘均价", 100);
            diccol.Add("楼栋均价", 100);
            diccol.Add("人工复估值", 100);
            using (FxtAPIClient client = new FxtAPIClient())
            {
                string data = string.Empty;
                JObject obj = new JObject();
                obj.Add("uploadfileid", uploadfileid);
                obj.Add("customerid", Public.LoginInfo.CustomerId);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                    "C", _TaskExport, Utils.Serialize(obj));
                List<JObject> _listData = null;
                if (Utils.Deserialize<int>(Utils.GetJObjectValue(result, "type")) == 1)
                {
                    _listData = Utils.Deserialize<List<JObject>>(Utils.GetJObjectValue(result, "data"));
                }
                sbHtml = ExcelHelper.ExcelToXml(_listData, "sheet1", diccol);
            }
            byte[] fileContents = Encoding.UTF8.GetBytes(sbHtml.ToString());
            FileContentResult file = File(fileContents, "application/ms-excel", array[1]);
            return file;
        }


        //导入复估押品
        [HttpPost]
        public ActionResult TaskExcelUp(HttpPostedFileBase file, string uploadfileid)
        {
            dynamic dy = saveFile(file);
            if (dy.flag)//上传成功
            {
                DataSet ds = new DataSet();
                string strConn = string.Empty;
                string path = dy.filepath + "\\" + dy.filename;
                ExcelHelper excel = new ExcelHelper(path);
                object[,] excelobj = excel.ReadComplexExcel(0, 0);
                Utils.DeleteFile(path);
                using (FxtAPIClient client = new FxtAPIClient())
                {
                    JObject obj = new JObject();
                    obj.Add("objResolve", Utils.Serialize(excelobj));
                    obj.Add("uploadfileid", uploadfileid);
                    obj.Add("rows", excelobj.GetLength(0));
                    obj.Add("cols", excelobj.GetLength(1));
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "C", _TaskExcelUp, Utils.Serialize(obj));
                }

            }
            return Json(result);
        }

        //保存文件到物理磁盘
        protected dynamic saveFile(HttpPostedFileBase file)
        {
            try
            {
                string DirectoryName = Utils.ServerMapPath(GetUploadUrl()),
                       NewFileName = FileNewName(Path.GetExtension(file.FileName)),
                       FileName = Path.Combine(DirectoryName, NewFileName);
                if (!Directory.Exists(DirectoryName))
                {
                    Directory.CreateDirectory(DirectoryName);
                }

                file.SaveAs(FileName);
                return new
                {
                    flag = true,
                    filepath = DirectoryName,
                    filename = NewFileName
                };
            }
            catch (Exception exe)
            {
                return new
                {
                    flag = false,
                    message = exe.Message
                };
            }
        }


    }
}
