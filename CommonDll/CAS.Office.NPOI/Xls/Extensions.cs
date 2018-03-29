using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;

namespace CAS.Office.NPOI
{
    /// <summary>
    /// NPOI 扩展类
    /// byte 2014-12-5
    /// </summary>
    internal static class Extensions
    {
        public static void InsertRow(this ISheet sheet, int rowCount, int columnCount)
        {
            for (int row = 0; row < rowCount; row++)
            {
                var tempRow = sheet.CreateRow(row);
                
                for (int column = 0; column < columnCount; column++)
                {                    
                    tempRow.CreateCell(column);
                }
            }
        }
    }
}
