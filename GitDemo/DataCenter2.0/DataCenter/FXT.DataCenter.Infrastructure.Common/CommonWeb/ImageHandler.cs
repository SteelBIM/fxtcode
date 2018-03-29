using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FXT.DataCenter.Infrastructure.Common.CommonWeb
{
    /// <summary>
    /// 定义生成不变形的缩略图的类(缩略图是个正方形，原图按缩略图的尽寸进行成比例的缩小并在水平和垂直方向居中合成）
    /// </summary>
    public class ImageHandler
    {
        public static string AllowExt = ".jpe|.jpeg|.jpg|.png|.tif|.tiff|.bmp|.gif";
        /// <summary>
        /// 获取图片编码信息
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /// <summary>
        /// 验证图片格式
        /// </summary>
        /// <param name="sExt"></param>
        /// <returns></returns>
        public static bool CheckValidExt(string sExt)
        {
            string AllowExt = ".jpe|.jpeg|.jpg|.png|.tif|.tiff|.bmp|.gif";
            bool flag = false;
            string[] aExt = AllowExt.Split('|');
            foreach (string filetype in aExt)
            {
                if (filetype.ToLower() == sExt)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="savePath"></param>
        /// <param name="ici"></param>
        public static void SaveImage(System.Drawing.Image image, string savePath, ImageCodecInfo ici)
        {
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)90));
            image.Save(savePath, ici, parameters);
            parameters.Dispose();
        }

        /// <summary>
        /// 生成缩略图，等比缩放，空白地方以白色填充
        /// </summary>
        /// <param name="sourceImagePath"></param>
        /// <param name="thumbnailImagePath"></param>
        /// <param name="thumbnailImageWidth"></param>
        public static void ToThumbnailImages_FixSize(string sourceImagePath, string thumbnailImagePath, int thumbnailImageWidth)
        {
            Hashtable htmimes = new Hashtable();
            htmimes[".jpeg"] = "image/jpeg";
            htmimes[".jpg"] = "image/jpeg";
            htmimes[".png"] = "image/png";
            htmimes[".tif"] = "image/tiff";
            htmimes[".tiff"] = "image/tiff";
            htmimes[".bmp"] = "image/bmp";
            htmimes[".gif"] = "image/gif";

            string SourceImagePath = sourceImagePath;
            string ThumbnailImagePath = thumbnailImagePath;
            int ThumbnailImageWidth = thumbnailImageWidth;
            string sExt = SourceImagePath.Substring(SourceImagePath.LastIndexOf(".")).ToLower();
            if (SourceImagePath.ToString() == System.String.Empty) throw new NullReferenceException("SourceImagePath is null!");
            if (!CheckValidExt(sExt))
            {
                throw new ArgumentException("原图片文件格式不正确,支持的格式有[ " + AllowExt + " ]", "SourceImagePath");
            }
            //System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(SourceImagePath));
            System.Drawing.Image image = System.Drawing.Image.FromFile(SourceImagePath);
            int num = ((ThumbnailImageWidth / 4) * 4);
            int width = image.Width;
            int height = image.Height;

            if ((((double)width) / ((double)height)) >= 1.0f)
            {
                num = ((height * ThumbnailImageWidth) / width);
            }
            else
            {
                ThumbnailImageWidth = ((width * num) / height);
            }
            if ((ThumbnailImageWidth < 1) || (num < 1))
            {
                return;
            }
            Bitmap bitmap = new Bitmap(thumbnailImageWidth, thumbnailImageWidth, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//补插模式
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; //光滑度
            graphics.Clear(Color.White); //清空绘图区并用白色填充
            int x1 = 0;
            int y1 = 0;
            x1 = (thumbnailImageWidth - ThumbnailImageWidth) / 2;
            y1 = (thumbnailImageWidth - num) / 2;
            graphics.DrawImage(image, new Rectangle(x1, y1, ThumbnailImageWidth, num));
            image.Dispose();
            try
            {
                string savepath = (ThumbnailImagePath == null ? SourceImagePath : ThumbnailImagePath);
                SaveImage(bitmap, savepath, GetCodecInfo((string)htmimes[sExt]));
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                bitmap.Dispose();
                graphics.Dispose();
            }
        }

        /// <summary>
        /// 生成缩略图，等比缩放，宽和高至少有一方的值等于基值
        /// </summary>
        /// <param name="sourceImagePath"></param>
        /// <param name="thumbnailImagePath"></param>
        /// <param name="thumbnailImageWidth"></param>
        public static void ToThumbnailImages(string sourceImagePath, string thumbnailImagePath, int thumbnailImageWidth)
        {
            Hashtable htmimes = new Hashtable();
            htmimes[".jpeg"] = "image/jpeg";
            htmimes[".jpg"] = "image/jpeg";
            htmimes[".png"] = "image/png";
            htmimes[".tif"] = "image/tiff";
            htmimes[".tiff"] = "image/tiff";
            htmimes[".bmp"] = "image/bmp";
            htmimes[".gif"] = "image/gif";

            string SourceImagePath = sourceImagePath;
            string ThumbnailImagePath = thumbnailImagePath;
            int ThumbnailImageWidth = thumbnailImageWidth;
            string sExt = SourceImagePath.Substring(SourceImagePath.LastIndexOf(".")).ToLower();
            if (SourceImagePath.ToString() == System.String.Empty) throw new NullReferenceException("SourceImagePath is null!");
            if (!CheckValidExt(sExt))
            {
                throw new ArgumentException("原图片文件格式不正确,支持的格式有[ " + AllowExt + " ]", "SourceImagePath");
            }
            //System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(SourceImagePath));
            System.Drawing.Image image = System.Drawing.Image.FromFile(SourceImagePath);
            int num = ((ThumbnailImageWidth / 4) * 4); //取整
            int width = image.Width;
            int height = image.Height;

            if (width > height)
            {
                if (width > num)//宽和高都小于基值
                {
                    height = (height * num) / width;
                    width = num;
                }
            }
            else if (width == height)
            {
                if (width >= num) //宽和高都小于基值
                {
                    height = num;
                    width = num;
                }
            }
            else
            {
                if (height > num) //宽和高都小于基值
                {
                    width = (width * num) / height;
                    height = num;
                }
            }
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//补插模式
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; //光滑度
            int x1 = 0;
            int y1 = 0;
            graphics.DrawImage(image, new Rectangle(x1, y1, width, height));
            image.Dispose();
            try
            {
                string savepath = (ThumbnailImagePath == null ? SourceImagePath : ThumbnailImagePath);
                SaveImage(bitmap, savepath, GetCodecInfo((string)htmimes[sExt]));
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                bitmap.Dispose();
                graphics.Dispose();
            }
        }

        /// <summary>
        /// 生成缩略图,等比缩放，空白地方以白色填充
        /// </summary>
        /// <param name="sourceImagePath"></param>
        /// <param name="thumbnailImagePath"></param>
        /// <param name="thumbnailImageWidth"></param>
        /// <param name="thumbnailImageHeight"></param>
        public static void ToThumbnailImages(string sourceImagePath, string thumbnailImagePath, int thumbnailImageWidth, int thumbnailImageHeight)
        {
            Hashtable htmimes = new Hashtable();
            htmimes[".jpeg"] = "image/jpeg";
            htmimes[".jpg"] = "image/jpeg";
            htmimes[".png"] = "image/png";
            htmimes[".tif"] = "image/tiff";
            htmimes[".tiff"] = "image/tiff";
            htmimes[".bmp"] = "image/bmp";
            htmimes[".gif"] = "image/gif";

            string SourceImagePath = sourceImagePath;
            string ThumbnailImagePath = thumbnailImagePath;
            int ThumbnailImageWidth = thumbnailImageWidth;
            string sExt = SourceImagePath.Substring(SourceImagePath.LastIndexOf(".")).ToLower();
            if (SourceImagePath.ToString() == System.String.Empty) throw new NullReferenceException("SourceImagePath is null!");
            if (!CheckValidExt(sExt))
            {
                throw new ArgumentException("原图片文件格式不正确,支持的格式有[ " + AllowExt + " ]", "SourceImagePath");
            }
            //System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(SourceImagePath));
            System.Drawing.Image image = System.Drawing.Image.FromFile(SourceImagePath);
            int num = thumbnailImageHeight;
            int width = image.Width;
            int height = image.Height;

            if ((((double)width) / ((double)height)) >= (((double)thumbnailImageWidth) / ((double)thumbnailImageHeight)))
            {
                num = ((height * ThumbnailImageWidth) / width);
            }
            else
            {
                ThumbnailImageWidth = ((width * num) / height);
            }
            if ((ThumbnailImageWidth < 1) || (num < 1))
            {
                return;
            }
            Bitmap bitmap = new Bitmap(thumbnailImageWidth, thumbnailImageHeight, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            //graphics.InterpolationMode=System.Drawing.Drawing2D.InterpolationMode.High;
            //graphics.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.Clear(Color.White);
            int x1 = 0;
            int y1 = 0;
            x1 = (thumbnailImageWidth - ThumbnailImageWidth) / 2;
            y1 = (thumbnailImageHeight - num) / 2;
            graphics.DrawImage(image, new Rectangle(x1, y1, ThumbnailImageWidth, num));
            image.Dispose();
            try
            {
                string savepath = (ThumbnailImagePath == null ? SourceImagePath : ThumbnailImagePath);
                SaveImage(bitmap, savepath, GetCodecInfo((string)htmimes[sExt]));
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                bitmap.Dispose();
                graphics.Dispose();
            }
        }

        #region  生成缩略图
        /// <summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径（物理路径）</param> 
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式("HW":指定高宽缩放（可能变形）,"W":指定宽，高按比例,"H":指定高，宽按比例,"Cut":指定高宽裁减（不变形）)</param> 
        public static bool MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            if (!File.Exists(originalImagePath)) return false;
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            System.Drawing.Image bitmap = null;
            System.Drawing.Graphics g = null;
            try
            {
                int towidth = width;
                int toheight = height;
                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;
                switch (mode)
                {
                    case "HW"://指定高宽缩放（可能变形） 
                        break;
                    case "W"://指定宽，高按比例 
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case "H"://指定高，宽按比例 
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case "Cut"://指定高宽裁减（不变形） 
                        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * height / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    default:
                        break;
                }
                //新建一个bmp图片 
                bitmap = new System.Drawing.Bitmap(towidth, toheight);
                //新建一个画板 
                g = System.Drawing.Graphics.FromImage(bitmap);
                //设置高质量插值法 
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度 
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //清空画布并以透明背景色填充 
                g.Clear(System.Drawing.Color.Transparent);
                //g.Clear(System.Drawing.Color.Red);
                //在指定位置并且按指定大小绘制原图片的指定部分 
                g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);
                //以jpg格式保存缩略图 
                FileInfo file = new FileInfo(thumbnailPath);

                EncoderParameter p;
                EncoderParameters ps;
                ps = new EncoderParameters(1);
                p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L);
                ps.Param[0] = p;

                if (file.Exists)
                {
                    if (!file.IsReadOnly)
                    {
                        try
                        {
                            File.Delete(thumbnailPath);
                            bitmap.Save(thumbnailPath, GetCodecInfo("image/jpeg"), ps);
                        }
                        catch { };
                    }
                }
                else
                {
                    bitmap.Save(thumbnailPath, GetCodecInfo("image/jpeg"), ps);
                }
                return true;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        #endregion
        /// <summary>
        /// 检查图片是否存在
        /// </summary>
        /// <param name="webResourceAddress">图片地址</param>
        /// <returns></returns>
        public static bool CheckImage(string webResourceAddress)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(webResourceAddress));
                req.Method = "HEAD";
                req.Timeout = 1000;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                bool StatusCode = (res.StatusCode == HttpStatusCode.OK);
                res.Close();
                req.Abort();
                return StatusCode;
            }
            catch (WebException ex)
            {
                //System.Diagnostics.Trace.Write(wex.Message);
                return false;
            }
        }
        #region 在图片上增加水印
        /**/
        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        public static void AddWater(string Path, string Path_sy)
        {
            try
            {
                string addText = "I'm the One Only";
                System.Drawing.Image image = System.Drawing.Image.FromFile(Path);

                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
                g.DrawImage(image, 0, 0, image.Width, image.Height);
                System.Drawing.Font f = new System.Drawing.Font("Verdana", 60);
                System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Silver);

                g.DrawString(addText, f, b, 35, 35);
                g.Dispose();

                image.Save(Path_sy);
                image.Dispose();
            }
            catch (Exception ex)
            {
                //SqlDb.WriteLog("打文字水印", ex);//写日志
            }
        }
        /**/
        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public static void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
                System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
                g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
                g.Dispose();

                image.Save(Path_syp);
                image.Dispose();
            }
            catch (Exception ex)
            {
                //SqlDb.WriteLog("打文字水印",ex);//写日志
            }
        }
        public static string getRandnum(int randnumlength)
        {
            System.Random Randnum = new System.Random(unchecked((int)DateTime.Now.Ticks));

            StringBuilder sb = new StringBuilder(randnumlength);
            for (int i = 0; i < randnumlength; i++)
            {
                sb.Append(Randnum.Next(0, 9));
            }
            return sb.ToString();
        }
        #endregion
        #region 下载网络图片
        /// <summary>
        /// 从图片地址下载图片到本地磁盘
        /// </summary>
        /// <param name="ToLocalPath">图片本地磁盘地址</param>
        /// <param name="Url">图片网址</param>
        /// <returns></returns>
        public static bool SavePhotoFromUrl(string FileName, string Url)
        {
            bool value = false;
            WebResponse response = null;
            Stream stream = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                response = request.GetResponse();
                stream = response.GetResponseStream();

                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    value = ImageHandler.SaveBinaryFile(response, FileName);
                }

            }
            catch (Exception err)
            {
                string aa = err.ToString();
            }
            return value;
        }

        /// <summary>
        /// Save a binary file to disk.
        /// </summary>
        /// <param name="response">The response used to save the file</param>
        // 将二进制文件保存到磁盘
        public static bool SaveBinaryFile(WebResponse response, string FileName)
        {
            bool Value = true;
            byte[] buffer = new byte[1024];

            try
            {
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }
                Stream outStream = System.IO.File.Create(FileName);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch
            {
                Value = false;
            }
            return Value;
        }
        #endregion
    }
}
