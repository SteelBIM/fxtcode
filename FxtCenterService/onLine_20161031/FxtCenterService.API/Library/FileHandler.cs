using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using CAS.Common;

namespace FxtCenterService.API
{
    public class FileHandler
    {
        public int SaveFile(byte[] fileBit, string path, string fileName, string UpType, bool needmark, DateTime marktime)
        {
            FileStream stream = null;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                string savePath = path + "/" + fileName;
                //需要水印
                if (needmark && marktime > default(DateTime))
                {
                    //原图
                    savePath = path + "/" + fileName.Split('.')[0] + "_base." + fileName.Split('.')[1];
                }
                stream = new FileStream(savePath, FileMode.Create);
                stream.Seek(0, SeekOrigin.Begin);
                stream.Write(fileBit, 0, fileBit.Length);
                stream.Dispose();

                //保存图片(加水印，边框)
                if (needmark && marktime > default(DateTime))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(savePath);
                    Graphics markg = Graphics.FromImage(img);
                    markg.DrawImage(img, 0, 0, img.Width, img.Height);
                    //画图片的边框线
                    markg.DrawRectangle(new Pen(Color.Black), 0, 0, img.Width - 1, img.Height - 1);
                    Font f = new Font("Verdana", 16);
                    Brush b = new SolidBrush(Color.Orange);
                    string str = marktime.ToString("yyyy/MM/dd");
                    markg.DrawString(str, f, b, img.Width - 135, img.Height - 30);
                    markg.Dispose();

                    img.Save(path + "/" + fileName);
                    img.Dispose();
                }


                if (UpType == "1")
                {
                    string imgName120 = fileName.Split('.')[0] + "_120." + fileName.Split('.')[1];
                    ImageUtil.MakeThumbnail(path + "/" + fileName, path + "/" + imgName120, 120, 120, ImageUtil.ThumbnailCompressType.BaseOnProportion, fileName.Substring(fileName.LastIndexOf('.') + 1));
                    //ImageUtil.GetThumbnail(path + "/" + fileName, path + "/" + imgName120, 120, 120);
                    string imgName480 = fileName.Split('.')[0] + "_480." + fileName.Split('.')[1];
                    ImageUtil.MakeThumbnail(path + "/" + fileName, path + "/" + imgName480, 480, 480, ImageUtil.ThumbnailCompressType.BaseOnProportion, fileName.Substring(fileName.LastIndexOf('.') + 1));
                    //ImageUtil.GetThumbnail(path + "/" + fileName, path + "/" + imgName480, 480, 480);
                }
            }
            catch (Exception e)
            {
                Console.ReadKey();
                throw e;
            }
            finally
            {
                stream.Close();
            }
            return 0;

        }

        public byte[] GetFile(string Path)
        {
            byte[] bit = new byte[300000];
            FileStream stream = null;
            try
            {
                stream = new FileStream(Path, FileMode.Open);
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(bit, 0, bit.Length);
                stream.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                stream.Close();
            }
            return bit;
        }
    }
}