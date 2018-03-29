using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using log4net;
using FxtSpider.Common.Models;
using System.Text.RegularExpressions;


namespace FxtSpider.Common
{
    public static class SaveData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SaveData));
        #region(出售房源_页面需要爬取的各字段名称)
        /// <summary>
        /// 楼盘名
        /// </summary>
        public const string key_lpm = "楼盘名";
        /// <summary>
        /// 案例时间
        /// </summary>
        public const string key_alsj = "案例时间";//*
        /// <summary>
        /// 行政区
        /// </summary>
        public const string key_xzq = "行政区";
        /// <summary>
        /// 片区
        /// </summary>
        public const string key_pq = "片区";
        /// <summary>
        /// 楼栋
        /// </summary>
        public const string key_ld = "楼栋";
        /// <summary>
        /// 房号
        /// </summary>
        public const string key_fh = "房号";
        /// <summary>
        /// 用途
        /// </summary>
        public const string key_yt = "用途";
        /// <summary>
        /// 面积
        /// </summary>
        public const string key_mj = "面积";
        /// <summary>
        /// 单价
        /// </summary>
        public const string key_dj = "单价";
        /// <summary>
        /// 案例类型
        /// </summary>
        public const string key_allx = "案例类型";
        /// <summary>
        /// 结构
        /// </summary>
        public const string key_jg = "结构";
        /// <summary>
        /// 建筑类型
        /// </summary>
        public const string key_jzlx = "建筑类型";
        /// <summary>
        /// 总价
        /// </summary>
        public const string key_zj = "总价";
        /// <summary>
        /// 所在楼层
        /// </summary>
        public const string key_szlc = "所在楼层";
        /// <summary>
        /// 总楼层
        /// </summary>
        public const string key_zlc = "总楼层";
        /// <summary>
        /// 户型
        /// </summary>
        public const string key_hx = "户型";//*
        /// <summary>
        /// 朝向
        /// </summary>
        public const string key_cx = "朝向";
        /// <summary>
        /// 装修
        /// </summary>
        public const string key_zx = "装修";
        /// <summary>
        /// 建筑年代
        /// </summary>
        public const string key_jznd = "建筑年代";
        /// <summary>
        /// 信息
        /// </summary>
        public const string key_title = "信息(备注)";
        /// <summary>
        /// 电话
        /// </summary>
        public const string key_phone = "电话";
        /// <summary>
        /// URL
        /// </summary>
        public const string key_url = "URL";
        /// <summary>
        /// 币种
        /// </summary>
        public const string key_bz = "币种";
        /// <summary>
        /// 网站来源
        /// </summary>
        public const string key_wzly = "网站来源";
        /// <summary>
        /// 地址
        /// </summary>
        public const string key_address = "地址";
        //static string key_jzlb = "建筑类别";
        //static string key_cqxz = "产权性质";
        //static string key_ptss = "配套设施";
        #endregion

        /// <summary>
        /// 数据保存到Excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="excelInfo"></param>
        public static void SaveExcel(string cityName, NewHouse newHouse )
        {
            //保存数据
            log.Debug(string.Format("Excel保存中:{0}-(url:{1}--)", cityName, newHouse.Url));

            string nowDate = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = string.Format("{0}_{1}_{2}.xls", nowDate,newHouse.Wzly, cityName);
            string path =SpiderHelp.GetConfigDire() + "DataSource\\" + fileName;


            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (newHouse == null)
            {
                return;
            }
            Excel.Application app = new Excel.Application();
            Excel.Workbook book = null;
            object missing = System.Reflection.Missing.Value;
            try
            {

                int nowRow = 0;
                bool existsFile = false;
                Excel.Worksheet sheet;
                if (File.Exists(path))
                {
                    existsFile = true;
                    app.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    book = (Excel.Workbook)app.ActiveWorkbook;
                    sheet = (Excel.Worksheet)book.Sheets[1];
                    nowRow = sheet.UsedRange.Cells.Rows.Count;
                }
                else
                {
                    app.Application.Workbooks.Add(true);
                    book = (Excel.Workbook)app.ActiveWorkbook;
                    sheet = (Excel.Worksheet)book.ActiveSheet;
                    sheet.Cells[1, 1] = SaveData.key_lpm;
                    sheet.Cells[1, 2] = SaveData.key_alsj;
                    sheet.Cells[1, 3] = SaveData.key_xzq;
                    sheet.Cells[1, 4] = SaveData.key_pq;
                    sheet.Cells[1, 5] = SaveData.key_ld;
                    sheet.Cells[1, 6] = SaveData.key_fh;
                    sheet.Cells[1, 7] = SaveData.key_yt;
                    sheet.Cells[1, 8] = SaveData.key_mj;
                    sheet.Cells[1, 9] = SaveData.key_dj;
                    sheet.Cells[1, 10] = SaveData.key_allx;
                    sheet.Cells[1, 11] = SaveData.key_jg;
                    sheet.Cells[1, 12] = SaveData.key_jzlx;
                    sheet.Cells[1, 13] = SaveData.key_zj;
                    sheet.Cells[1, 14] = SaveData.key_szlc;
                    sheet.Cells[1, 15] = SaveData.key_zlc;
                    sheet.Cells[1, 16] = SaveData.key_hx;
                    sheet.Cells[1, 17] = SaveData.key_cx;
                    sheet.Cells[1, 18] = SaveData.key_zx;
                    sheet.Cells[1, 19] = SaveData.key_jznd;
                    sheet.Cells[1, 20] = SaveData.key_title;
                    sheet.Cells[1, 21] = SaveData.key_phone;
                    sheet.Cells[1, 22] = SaveData.key_url;
                    sheet.Cells[1, 23] = SaveData.key_bz;
                    sheet.Cells[1, 24] = SaveData.key_wzly;
                    sheet.Cells[1, 25] = SaveData.key_address;
                    nowRow = 1;
                }
                sheet.Cells[nowRow + 1, 1] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Lpm) ? newHouse.Lpm : "");
                sheet.Cells[nowRow + 1, 2] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Alsj) ? newHouse.Alsj : "");
                sheet.Cells[nowRow + 1, 3] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Xzq) ? newHouse.Xzq : "");
                sheet.Cells[nowRow + 1, 4] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Pq) ? newHouse.Pq : "");
                sheet.Cells[nowRow + 1, 5] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Ld) ? newHouse.Ld : "");
                sheet.Cells[nowRow + 1, 6] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Fh) ? newHouse.Fh : "");
                sheet.Cells[nowRow + 1, 7] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Yt) ? newHouse.Yt : "");
                sheet.Cells[nowRow + 1, 8] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Mj) ? newHouse.Mj : "");
                sheet.Cells[nowRow + 1, 9] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Dj) ? newHouse.Dj : "");
                sheet.Cells[nowRow + 1, 10] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Allx) ? newHouse.Allx : "");
                sheet.Cells[nowRow + 1, 11] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Jg) ? newHouse.Jg : "");
                sheet.Cells[nowRow + 1, 12] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Jzlx) ? newHouse.Jzlx : "");
                sheet.Cells[nowRow + 1, 13] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Zj) ? newHouse.Zj : "");
                sheet.Cells[nowRow + 1, 14] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Szlc) ? newHouse.Szlc : "");
                sheet.Cells[nowRow + 1, 15] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Zlc) ? newHouse.Zlc : "");
                sheet.Cells[nowRow + 1, 16] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Hx) ? newHouse.Hx : "");
                sheet.Cells[nowRow + 1, 17] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Cx) ? newHouse.Cx : "");
                sheet.Cells[nowRow + 1, 18] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Zx) ? newHouse.Zx : "");
                sheet.Cells[nowRow + 1, 19] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Jznd) ? newHouse.Jznd : "");
                sheet.Cells[nowRow + 1, 20] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Title) ? newHouse.Title : "");
                sheet.Cells[nowRow + 1, 21] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Phone) ? newHouse.Phone : "");
                sheet.Cells[nowRow + 1, 22] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Url) ? newHouse.Url : "");
                sheet.Cells[nowRow + 1, 23] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Bz) ? newHouse.Bz : "");
                sheet.Cells[nowRow + 1, 24] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Wzly) ? newHouse.Wzly : "");
                sheet.Cells[nowRow + 1, 25] = ExcelReplaceStr(!string.IsNullOrEmpty(newHouse.Wzly) ? newHouse.Addres : "");

                //保存excel文件
                if (existsFile)
                {
                    book.Save();
                }
                else
                {
                    book.SaveCopyAs(path);
                }
                //关闭文件
                book.Close(false, missing, missing);
                //退出excel
                app.Quit();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("title:{0}--lpm:{1}--excel导入异常", newHouse.Title, newHouse.Lpm), ex);
                if (book != null)
                {
                    book.Close(false, missing, missing);
                }
                app.Quit();
                System.Threading.Thread.Sleep(2000);
                //SaveExcel(path, excelInfo);
            }
        }

        /// <summary>
        /// 过滤用于Excel的特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ExcelReplaceStr(string str)
        {
            str = str.Replace("+", ",");
            str = str.Replace("-", ",");
            str = str.Replace("*", ",");
            str = str.Replace("=", ",");
            return str;
        }
        /// <summary>
        /// 整理各列字符串格式
        /// </summary>
        /// <param name="newHouse"></param>
        /// <returns></returns>
        public static NewHouse ToColumnStr(this NewHouse newHouse)
        {
            //整理数据字符串
            newHouse.Lpm = StringHelp.TrimBlank(newHouse.Lpm).ToRemoveSpe();
            newHouse.Xzq = StringHelp.TrimBlank(newHouse.Xzq.Trim().ToRemoveSpe());
            newHouse.Jg = string.IsNullOrEmpty(StringHelp.TrimBlank(newHouse.Jg)) ? "平面" : StringHelp.TrimBlank(newHouse.Jg);
            newHouse.Zj = StringHelp.TrimBlank(newHouse.Zj);
            newHouse.Cx = StringHelp.TrimBlank(newHouse.Cx).ToRemoveSpe();
            newHouse.Phone = StringHelp.TrimBlank(newHouse.Phone).ToRemoveSpe();
            newHouse.Mj = Regex.Replace(newHouse.Mj, @"\..*", "", RegexOptions.IgnoreCase);
            newHouse.Dj = Regex.Replace(newHouse.Dj, @"\..*", "", RegexOptions.IgnoreCase);
            newHouse.Hymj = Regex.Replace(newHouse.Hymj, @"\..*", "", RegexOptions.IgnoreCase);
            newHouse.Dxsmj = Regex.Replace(newHouse.Dxsmj, @"\..*", "", RegexOptions.IgnoreCase);
            //计算数据
            newHouse.Jzlx = SpiderHelp.GetBuildingType(newHouse.Zlc);//获取计算建筑类型
            newHouse.Yt = SpiderHelp.GetHousePurposes(newHouse.Mj, newHouse.Jzlx);//获取计算用途            
            newHouse.Hx = SpiderHelp.GetHouseType(newHouse.Hx).ToRemoveSpe();
            newHouse.Alsj = newHouse.Alsj != null ? newHouse.Alsj.Trim() : newHouse.Alsj;
            if (!StringHelp.CheckStrIsDate(newHouse.Alsj))
            {
                newHouse.Alsj = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                newHouse.Alsj =Convert.ToDateTime( newHouse.Alsj).ToString("yyyy-MM-dd HH:mm:ss");
            }
            return newHouse;
        }

    }
}
