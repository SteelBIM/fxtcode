using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Common
{
    public class Thumbnail
    {
        #region 生成缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源文件路径</param>
        /// <param name="thumbnailPath">生成缩略图路径</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="mode">缩略方式，默认“H”以高度为基数按比例缩略</param>
        /// <param name="type">图片格式</param>
        /// <returns>
        ///    0  - 未知错误
        ///    1  - 成功
        ///    -1 - 图片路径错误
        ///    -2 - 图片尺寸错误
        ///    -3 - 压缩类型错误
        ///    -4 - 文件类型错误
        ///    -5 - 缩略图保存IO错误
        ///</returns>
        public static int MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ThumbnailCompressType mode, string type)
        {
            int result = 0;

            System.Drawing.Image originalImage = null;
            try
            {
                originalImage = System.Drawing.Image.FromFile(originalImagePath);
            }
            catch
            {
                result = -1;
            }

            int towidth = width;
            int toheight = height;
            if (0 == width && 0 == height)
            {
                result = -2;
            }
            else if (null != originalImage)
            {
                type = type.ToUpper();
                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;

                switch (mode)
                {
                    case ThumbnailCompressType.FreeWidthHeight://指定高宽缩放（可能变形） 
                        break;
                    case ThumbnailCompressType.BaseOnWidth://指定宽，高按比例 
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case ThumbnailCompressType.BaseOnHeight://指定高，宽按比例
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case ThumbnailCompressType.Cut://指定高宽裁减（不变形） 
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
                    case ThumbnailCompressType.BaseOnProportion://等比缩放（不变形，如果高大按高，宽大按宽缩放） 
                        if ((double)originalImage.Width / (double)towidth < (double)originalImage.Height / (double)toheight)
                        {
                            toheight = height;
                            towidth = originalImage.Width * height / originalImage.Height;
                        }
                        else
                        {
                            towidth = width;
                            toheight = originalImage.Height * width / originalImage.Width;
                        }
                        break;
                    case ThumbnailCompressType.Remove://指定高宽裁减（变形） 
                        oh = height;
                        ow = width;
                        break;                    
                    default:
                        result = -3;
                        break;
                }

                //新建一个bmp图片
                System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

                //新建一个画板
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                g.Clear(System.Drawing.Color.Transparent);

                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

                try
                {
                    //保存缩略图
                    switch (type)
                    {
                        case "JPEG":
                        case "JPG":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case "BMP":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case "GIF":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case "PNG":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        case "TIF":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Tiff);
                            break;
                        case "ICO":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Icon);
                            break;
                        case "WMF":
                            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Wmf);
                            break;
                        default:
                            result = -4;
                            break;

                    }
                }
                catch
                {
                    result = -5;
                }
                finally
                {
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                    if (result == 0) result = 1;
                }
            }

            return result;
        }
        #endregion

        public enum ThumbnailCompressType
        {
            /// <summary>
            /// 指定宽，高按比例 
            /// </summary>
            BaseOnWidth = 1,
            /// <summary>
            /// 指定高，宽按比例
            /// </summary>
            BaseOnHeight = 2,
            /// <summary>
            /// //指定高宽缩放（可能变形） 
            /// </summary>
            FreeWidthHeight = 4,
            /// <summary>
            /// 指定高宽裁减（不变形） 
            /// </summary>
            Cut = 8,
            /// <summary>
            /// 等比缩放（不变形，如果高大按高，宽大按宽缩放） 
            /// </summary>
            BaseOnProportion = 16,
            Remove=3
        }
    }
}
