using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Collections;

namespace CAS.Common.Aspose
{
    /// <summary>
    /// aspose.cells解决不了03中获取图片，所以引入npoi来获取
    /// </summary>
    public class Xls
    {
        public IWorkbook workbook;
        FileStream input;
        public Xls(string openTargetFile)
        {
            input = File.OpenRead(openTargetFile);
            if (input.Length > 0)
            {
                if (openTargetFile.EndsWith(".xls"))
                {
                    workbook = new HSSFWorkbook(input);
                }
            }
        }

        public Xls(string openTargetFile, bool fileShare)
        {
            if (!fileShare)
            {
                input = File.OpenRead(openTargetFile);
            }
            else
            {
                input = new FileStream(openTargetFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);        //共享锁打开文件
            }
            if (input.Length > 0)
            {
                if (openTargetFile.EndsWith(".xls"))
                {
                    workbook = new HSSFWorkbook(input);
                }
            }
        }
        /// <summary>
        /// 获得名称的总个数
        /// </summary>
        /// <returns></returns>
        public int NumberOfNames
        {
            get
            {
                return workbook.NumberOfNames;
            }
        }
        /// <summary>
        /// 获取excel中的图片 excel 2003
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public List<byte[]> GetPictureData(string sheetName)
        {
            List<byte[]> imgData = new List<byte[]>();
            //读取excel中图片
            ISheet sheet = workbook.GetSheet(sheetName);
            if (null != sheet)
            {
                IList pictures = sheet.Workbook.GetAllPictures();
                foreach (IPictureData pic in pictures)
                {
                    string ext = pic.SuggestFileExtension();//获取扩展名
                    imgData.Add(pic.Data);
                }
            }
            return imgData;
        }
        /// <summary>
        /// 释放对象
        /// byte-侯湘岳
        /// </summary>
        public void Close()
        {
            if (null != input)
                input.Close();
        }
    }
}
