using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace FXT.DataCenter.Common.CommonWeb
{
    /// <summary>
    /// 缩略图操作辅助类
    /// </summary>
    public class ThumbnailHelper
    {
        public ThumbnailHelper()
        {
        }

        /// <summary> 
        /// 生成缩略图 静态方法 
        /// </summary> 
        /// <param name="pathImageFrom"> 源图的路径(含文件名及扩展名) </param> 
        /// <param name="pathImageTo"> 生成的缩略图所保存的路径(含文件名及扩展名) 
        /// 注意：扩展名一定要与生成的缩略图格式相对应 </param> 
        /// <param name="width"> 欲生成的缩略图 "画布" 的宽度(像素值) </param> 
        /// <param name="height"> 欲生成的缩略图 "画布" 的高度(像素值) </param> 
        public static void GenThumbnail(string pathImageFrom, string pathImageTo, int width, int height)
        {
            Image imageFrom = null;
            try
            {
                imageFrom = Image.FromFile(pathImageFrom);
            }
            catch
            {
                //throw; 
            }
            if (imageFrom == null)
            {
                return;
            }
            // 源图宽度及高度 
            int imageFromWidth = imageFrom.Width;
            int imageFromHeight = imageFrom.Height;
            // 生成的缩略图实际宽度及高度 
            int bitmapWidth = width;
            int bitmapHeight = height;
            // 生成的缩略图在上述"画布"上的位置 
            int X = 0;
            int Y = 0;
            // 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置 
            if (bitmapHeight * imageFromWidth > bitmapWidth * imageFromHeight)
            {
                bitmapHeight = imageFromHeight * width / imageFromWidth;
                Y = (height - bitmapHeight) / 2;
            }
            else
            {
                bitmapWidth = imageFromWidth * height / imageFromHeight;
                X = (width - bitmapWidth) / 2;
            }
            // 创建画布 
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            // 用白色清空 
            g.Clear(Color.White);
            // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // 指定高质量、低速度呈现。 
            g.SmoothingMode = SmoothingMode.HighQuality;
            // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。 
            g.DrawImage(imageFrom, new Rectangle(X, Y, bitmapWidth, bitmapHeight), new Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);
            try
            {
                //经测试 .jpg 格式缩略图大小与质量等最优 
                bmp.Save(pathImageTo, ImageFormat.Jpeg);
            }
            catch
            {
            }
            finally
            {
                //显示释放资源 
                imageFrom.Dispose();
                bmp.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 生成缩略图 静态方法 (自动填充满新图)
        /// </summary>
        /// <param name="stream">原图的流</param>
        /// <param name="pathImageTo">生成的缩略图所保存的路径(含文件名及扩展名) 
        /// 注意：扩展名一定要与生成的缩略图格式相对应</param>
        /// <param name="width"> 欲生成的缩略图 "画布" 的宽度(像素值)</param>
        /// <param name="height">欲生成的缩略图 "画布" 的高度(像素值) </param>
        public static void GenThumbnail(Stream stream, string pathImageTo, int width, int height)
        {
            GenThumbnail(stream, pathImageTo, width, height, true);
        }

        /// <summary>
        /// 生成缩略图 静态方法 
        /// </summary>
        /// <param name="stream">原图的流</param>
        /// <param name="pathImageTo">生成的缩略图所保存的路径(含文件名及扩展名) 
        /// 注意：扩展名一定要与生成的缩略图格式相对应</param>
        /// <param name="width"> 欲生成的缩略图 "画布" 的宽度(像素值)</param>
        /// <param name="height">欲生成的缩略图 "画布" 的高度(像素值) </param>
        /// <param name="isFill">是否填充。true，自动填充满；false：按照比率缩放图片，有可能有白色的背景</param>
        public static void GenThumbnail(Stream stream, string pathImageTo, int width, int height, bool isFill)
        {
            Image imageFrom = null;
            try
            {
                imageFrom = Image.FromStream(stream);
            }
            catch
            {
                //throw; 
            }
            if (imageFrom == null)
            {
                return;
            }
            // 源图宽度及高度 
            int imageFromWidth = imageFrom.Width;
            int imageFromHeight = imageFrom.Height;
            // 生成的缩略图实际宽度及高度 
            int bitmapWidth = width;
            int bitmapHeight = height;
            // 生成的缩略图在上述"画布"上的位置 
            int X = 0;
            int Y = 0;
            // 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置 
            if (bitmapHeight * imageFromWidth > bitmapWidth * imageFromHeight)
            {
                bitmapHeight = imageFromHeight * width / imageFromWidth;
                Y = (height - bitmapHeight) / 2;
            }
            else
            {
                bitmapWidth = imageFromWidth * height / imageFromHeight;
                X = (width - bitmapWidth) / 2;
            }
            // 创建画布 
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            // 用白色清空 
            g.Clear(Color.White);
            // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // 指定高质量、低速度呈现。 
            g.SmoothingMode = SmoothingMode.HighQuality;
            if (isFill)
            {
                // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。 
                g.DrawImage(imageFrom, new Rectangle(0, 0, width, height), new Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);
            }
            else
            {
                // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。 
                g.DrawImage(imageFrom, new Rectangle(X, Y, bitmapWidth, bitmapHeight), new Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);
            }
            try
            {
                //经测试 .jpg 格式缩略图大小与质量等最优 
                bmp.Save(pathImageTo, ImageFormat.Jpeg);
            }
            catch
            {
            }
            finally
            {
                //显示释放资源 
                imageFrom.Dispose();
                bmp.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="src">来源页面,可以是相对地址或者绝对地址</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>字节数组</returns>
        public static byte[] MakeThumbnail(string src, double width, double height)
        {
            Image image;

            // 相对路径从本机直接读取
            if (src.ToLower().IndexOf("http://") == -1)
            {
                src = HttpHelper.CurrentServer.MapPath(src);
                image = Image.FromFile(src, true);
            }
            else // 绝对路径从 Http 读取
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(src);
                req.Method = "GET";
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream receiveStream = resp.GetResponseStream();
                image = Image.FromStream(receiveStream);
                resp.Close();
                receiveStream.Close();
            }
            double newWidth, newHeight;
            if (image.Width > image.Height)
            {
                newWidth = width;
                newHeight = image.Height * (newWidth / image.Width);
            }
            else
            {
                newHeight = height;
                newWidth = (newHeight / image.Height) * image.Width;
            }
            if (newWidth > width)
            {
                newWidth = width;
            }
            if (newHeight > height)
            {
                newHeight = height;
            }
            //取得图片大小
            Size size = new Size((int)newWidth, (int)newHeight);
            //新建一个bmp图片
            Image bitmap = new Bitmap(size.Width, size.Height);
            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            //清空一下画布
            g.Clear(Color.White);
            //在指定位置画图
            g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);

            ////文字水印
            //System.Drawing.Graphics G = System.Drawing.Graphics.FromImage(bitmap);
            //System.Drawing.Font f = new Font("宋体", 10);
            //System.Drawing.Brush b = new SolidBrush(Color.Black);
            //G.DrawString("文字水印的测试", f, b, 10, 10);
            //G.Dispose();

            ////图片水印
            //System.Drawing.Image copyImage = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("Bird.gif"));
            //Graphics a = Graphics.FromImage(bitmap);
            //a.DrawImage(copyImage, new Rectangle(bitmap.Width - copyImage.Width, bitmap.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
            //copyImage.Dispose();
            //a.Dispose();
            //copyImage.Dispose();

            //保存高清晰度的缩略图
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = stream.GetBuffer();
            g.Dispose();
            image.Dispose();
            bitmap.Dispose();
            return buffer;
        }
        #region 图片加水印
        /// <summary>
        /// 图片加文字水印
        /// </summary>
        /// <param name="imagePath">源图片路径</param>
        /// <param name="newPath">生成新文件</param>
        /// <returns></returns>
        public static string MakeTextSY(string imagePath, string newPath, string SYText)
        {
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
                Graphics g = Graphics.FromImage(image);
                g.DrawImage(image, 0, 0, image.Width, image.Height);
                float x = 10, y = 10;
                string addText = SYText;
                int tlen = addText.Length;
                Font f = null;
                //以1个中文字为基础测的计算的X坐标点
                if (image.Width < 121)
                {
                    f = new Font("华文彩云", 8, GraphicsUnit.Pixel);//每个字8个像素
                    x = (image.Width - (tlen * 11)) / 2;
                    if (x < 0) x = 0;
                    y = image.Height / 2 - 5;
                    if (y < 0) y = 0;
                }
                else if (image.Width < 201)
                {
                    f = new Font("华文彩云", 10, GraphicsUnit.Pixel);//11
                    x = (image.Width - (tlen * 16)) / 2;
                    if (x < 0) x = 0;
                    y = image.Height / 2 - 5;
                    if (y < 0) y = 0;
                }
                else if (image.Width < 401)
                {
                    f = new Font("华文彩云", 15, GraphicsUnit.Pixel);//16
                    x = (image.Width - (tlen * 22)) / 2;
                    if (x < 0) x = 0;
                    y = image.Height / 2 - 10;
                    if (y < 0) y = 0;
                }
                else if (image.Width < 801)
                {
                    f = new Font("华文彩云", 26, GraphicsUnit.Pixel);//27
                    x = (image.Width - (tlen * 35)) / 2;
                    if (x < 0) x = 0;
                    y = image.Height / 2 - 20;
                    if (y < 0) y = 0;
                }
                else if (image.Width < 1366)
                {
                    f = new Font("华文彩云", 45, GraphicsUnit.Pixel);//47
                    x = (image.Width - (tlen * 60)) / 2;
                    if (x < 0) x = 0;
                    y = image.Height / 2 - 40;
                    if (y < 0) y = 0;
                }
                else if (image.Width < 2049)
                {
                    f = new Font("华文彩云", 70, GraphicsUnit.Pixel);//74
                    x = (image.Width - (tlen * 90)) / 2;
                    if (x < 0) x = 0;
                    y = image.Height / 2 - 60;
                    if (y < 0) y = 0;
                }
                else if (image.Width < 3000)
                {
                    f = new Font("华文彩云", 110, GraphicsUnit.Pixel);//122
                    x = (image.Width - (tlen * 135)) / 2;
                    if (x < 0) x = 0;
                    y = image.Height / 2 - 80;
                    if (y < 0) y = 0;
                }
                else
                {
                    f = new Font("华文彩云", 150, GraphicsUnit.Pixel);//176
                    x = (image.Width - (tlen * 190)) / 2;
                    if (x < 0) x = 0;
                    y = image.Height / 2 - 100;
                    if (y < 0) y = 0;
                }
                Brush b = new SolidBrush(Color.Gray);

                g.DrawString(addText, f, b, x, y);
                g.Save();
                g.Dispose();
                image.Save(newPath);
                image.Dispose();
                return newPath;
            }
            catch (Exception er)
            {
                return "";
            }
        }
        #endregion
    }
}