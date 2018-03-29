using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.Infrastructure.Redis;

namespace FXT.DataCenter.WCF.Services
{
    public partial class ExcelUpload
    {
        public void HouseFormatConvert( string filePath, string newPath)
        {
            var excelHelper = new ExcelHandle(filePath);
            var data = excelHelper.ExcelToDataTable("Sheet1", true);

            DataTable table = new DataTable();
            #region 创建列
            table.Columns.Add("*行政区");
            table.Columns.Add("*楼盘名称");
            table.Columns.Add("*楼栋名称");
            table.Columns.Add("*房号名称");
            table.Columns.Add("*所在楼层");
            table.Columns.Add("*单元室号");
            table.Columns.Add("建筑面积");
            table.Columns.Add("套内面积");
            table.Columns.Add("户型");
            table.Columns.Add("户型结构");
            table.Columns.Add("总价");
            table.Columns.Add("单价");
            table.Columns.Add("朝向");
            table.Columns.Add("景观");
            table.Columns.Add("用途");
            table.Columns.Add("价格权重系数");
            table.Columns.Add("备注");
            table.Columns.Add("是否可估");
            table.Columns.Add("面积确认");
            table.Columns.Add("附属房屋类型");
            table.Columns.Add("附属房屋面积");
            table.Columns.Add("实际层");
            table.Columns.Add("通风采光");
            table.Columns.Add("装修");
            table.Columns.Add("是否有厨房");
            table.Columns.Add("阳台数");
            table.Columns.Add("洗手间数");
            #endregion
            #region 循环转换
            for (int i = 0; i < data.Rows.Count; i++)
            {
                //行政区
                string area = data.Rows[i][1].ToString();
                if (data.Rows[i][1] != null && !string.IsNullOrWhiteSpace(data.Rows[i][1].ToString())) { area = data.Rows[i][1].ToString(); }
                else { throw new Exception("表格数据有误，第" + (i + 1) + "行“行政区”不能为空。"); }
                //楼盘名称
                string projectName = string.Empty;
                if (data.Rows[i][3] != null && !string.IsNullOrWhiteSpace(data.Rows[i][3].ToString())) { projectName = data.Rows[i][3].ToString(); }
                else { throw new Exception("表格数据有误，第" + (i + 1) + "行“楼盘名称”不能为空。"); }
                //楼栋名称
                string buildingName = string.Empty;
                if (data.Rows[i][8] != null && !string.IsNullOrWhiteSpace(data.Rows[i][8].ToString())) { buildingName = data.Rows[i][8].ToString(); }
                else { throw new Exception("表格数据有误，第" + (i + 1) + "行“楼栋名称”不能为空。"); }
                //室号
                string houseName = string.Empty;
                if (data.Rows[i][13] != null && !string.IsNullOrWhiteSpace(data.Rows[i][13].ToString())) { houseName = data.Rows[i][13].ToString(); }
                else { throw new Exception("表格数据有误，第" + (i + 1) + "行“室号”不能为空。"); }
                //所在单元
                string UnitNo = string.Empty;
                if (data.Rows[i][10] != null) { UnitNo = data.Rows[i][10].ToString(); }
                //建筑面积
                string BuildArea = string.Empty;
                if (data.Rows[i][14] != null) { BuildArea = data.Rows[i][14].ToString(); }
                //户型结构
                string StructureCode = string.Empty;
                if (data.Rows[i][6] != null) { StructureCode = data.Rows[i][6].ToString(); }
                //朝向
                string FrontCode = string.Empty;
                if (data.Rows[i][15] != null) { FrontCode = data.Rows[i][15].ToString(); }
                //用途
                string PurposeCode = string.Empty;
                if (data.Rows[i][5] != null) { PurposeCode = data.Rows[i][5].ToString(); }
                //总楼层
                int zlc = 0;
                if (data.Rows[i][12] != null && int.TryParse(data.Rows[i][12].ToString(), out zlc)) { }
                if (zlc == 0) { throw new Exception("表格数据有误，第" + (i + 1) + "行“结束楼层”数据格式不正确。"); }
                //起始楼层
                int qslc = 0;
                if (data.Rows[i][11] != null && int.TryParse(data.Rows[i][11].ToString(), out qslc)) { }
                if (qslc == 0) { throw new Exception("表格数据有误，第" + (i + 1) + "行“起始楼层”数据格式不正确。"); }
                for (int t = qslc; t <= zlc; t++)
                {
                    DataRow row = table.NewRow();

                    row["*行政区"] = area;
                    row["*楼盘名称"] = projectName;
                    row["*楼栋名称"] = buildingName;
                    string house = UnitNo + t + houseName.Replace("‘", "");
                    row["*房号名称"] = house;
                    row["*所在楼层"] = t;
                    row["*单元室号"] = houseName;
                    row["建筑面积"] = BuildArea;
                    row["户型结构"] = StructureCode;
                    row["朝向"] = FrontCode;
                    row["用途"] = PurposeCode;
                    table.Rows.Add(row);
                }
            }
            #endregion

            using (var ms = ExcelHandle.RenderToExcel(table))
            {
                using (FileStream fs = new FileStream(newPath, FileMode.OpenOrCreate))
                {
                    BinaryWriter w = new BinaryWriter(fs);
                    w.Write(ms.ToArray());
                    fs.Close();
                }
            }
        }
    }
}
