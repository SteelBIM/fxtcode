using System;
using System.IO;

using Aspose.Words;
using Aspose.Words.Drawing;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Diagnostics;
namespace CAS.Common.Aspose
{
    public class CompressImages
    {
        /// <summary>
        /// 压缩包含图片的word文档 默认像素150、质量90
        /// </summary>
        /// <param name="wordPath">文档路径</param>
        /// <returns>是否成功</returns>
        public static bool CompressWordImages(string wordPath)
        {
            // 220ppi Print - said to be excellent on most printers and screens.
            // 150ppi Screen - said to be good for web pages and projectors.
            // 96ppi Email - said to be good for minimal document size and sharing.
            int desiredPpi = 150;
            // In .NET this seems to be a good compression / quality setting.
            int jpegQuality = 80;
            return CompressWordImages(wordPath, desiredPpi, jpegQuality);
        }
        /// <summary>
        /// 压缩包含图片的word文档
        /// </summary>
        /// <param name="wordPath">文档路径</param>
        /// <param name="desiredPpi">像素 参考值 220ppi、150ppi、96ppi</param>
        /// <param name="jpegQuality">质量</param>
        /// <returns>是否成功</returns>
        public static bool CompressWordImages(string wordPath, int desiredPpi, int jpegQuality)
        {
            try
            {
                Document doc = new Document(wordPath);
                // Resample images to desired ppi and save.
                int count = CompressFile(doc, desiredPpi, jpegQuality);

                if (count > 0)
                {
                    doc.Save(wordPath);
                    // Verify that the first image was compressed by checking the new Ppi.
                    //doc = new Document(wordPath);
                    //Shape shape = (Shape)doc.GetChild(NodeType.Shape, 0, true);
                    //double imagePpi = shape.ImageData.ImageSize.WidthPixels / ConvertUtil.PointToInch(shape.SizeInPoints.Width);
                    //return imagePpi > 150;
                }
                return count > 0;//没有照片需要压缩
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 检查文件是否被盖章(wmf或emf图片标志）
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static bool ValidFile(Document doc)
        {
            // Convert VML shapes.
            foreach (Shape vmlShape in doc.GetChildNodes(NodeType.Shape, true))
            {
                ImageType imageType = vmlShape.ImageData.ImageType;
                if (imageType.Equals(ImageType.Wmf) || imageType.Equals(ImageType.Emf))
                    return false;
            }
            // Convert DrawingML shapes.
            foreach (DrawingML dmlShape in doc.GetChildNodes(NodeType.DrawingML, true))
            {
                ImageType imageType = dmlShape.ImageData.ImageType;
                if (imageType.Equals(ImageType.Wmf) || imageType.Equals(ImageType.Emf))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Resamples all images in the document that are greater than the specified PPI (pixels per inch) to the specified PPI
        /// and converts them to JPEG with the specified quality setting.
        /// </summary>
        /// <param name="doc">The document to process.</param>
        /// <param name="desiredPpi">Desired pixels per inch. 220 high quality. 150 screen quality. 96 email quality.</param>
        /// <param name="jpegQuality">0 - 100% JPEG quality.</param>
        /// <returns></returns>
        public static int CompressFile(Document doc, int desiredPpi, int jpegQuality)
        {
            int count = 0;
            if (true)//ValidFile(doc)
            {
                // Convert VML shapes.
                foreach (Shape vmlShape in doc.GetChildNodes(NodeType.Shape, true))
                {
                    // It is important to use this method to correctly get the picture shape size in points even if the picture is inside a group shape.
                    SizeF shapeSizeInPoints = vmlShape.SizeInPoints;
                    if (ResampleCore(vmlShape.ImageData, shapeSizeInPoints, desiredPpi, jpegQuality))
                        count++;
                }

                // Convert DrawingML shapes.
                foreach (DrawingML dmlShape in doc.GetChildNodes(NodeType.DrawingML, true))
                {
                    // In MS Word the size of a DrawingML shape is always in points at the moment.
                    SizeF shapeSizeInPoints = dmlShape.Size;
                    if (ResampleCore(dmlShape.ImageData, shapeSizeInPoints, desiredPpi, jpegQuality))
                        count++;
                }
            }
            else
            {
                count = -1;//被盖章
            }
            return count;
        }

        /// <summary>
        /// Resamples one VML or DrawingML image
        /// </summary>
        private static bool ResampleCore(IImageData imageData, SizeF shapeSizeInPoints, int ppi, int jpegQuality)
        {
            // The are actually several shape types that can have an image (picture, ole object, ole control), let's skip other shapes.
            if (imageData == null)
                return false;

            // An image can be stored in the shape or linked from somewhere else. Let's skip images that do not store bytes in the shape.
            byte[] originalBytes = imageData.ImageBytes;
            if (originalBytes == null)
                return false;

            // Ignore metafiles, they are vector drawings and we don't want to resample them.
            ImageType imageType = imageData.ImageType;
            /*bug:对透明的png图片压缩还有问题，会把背景变黑，修改人:rock,20150928*/
            if (imageType.Equals(ImageType.Wmf) || imageType.Equals(ImageType.Emf) || imageType.Equals(ImageType.Png))
                return false;

            try
            {
                double shapeWidthInches = ConvertUtil.PointToInch(shapeSizeInPoints.Width);
                double shapeHeightInches = ConvertUtil.PointToInch(shapeSizeInPoints.Height);
                // Calculate the current PPI of the image.
                //ImageSize imageSize = imageData.ImageSize;
                //double currentPpiX = imageSize.WidthPixels / shapeWidthInches;
                //double currentPpiY = imageSize.HeightPixels / shapeHeightInches;

                // Let's resample only if the current PPI is higher than the requested PPI (e.g. we have extra data we can get rid of).
                //if ((currentPpiX <= ppi) || (currentPpiY <= ppi))
                //{
                //    return false;
                //}               

                using (Image srcImage = imageData.ToImage())
                {
                    // Create a new image of such size that it will hold only the pixels required by the desired ppi.
                    int dstWidthPixels = (int)(shapeWidthInches * ppi);
                    int dstHeightPixels = (int)(shapeHeightInches * ppi);
                    using (Bitmap dstImage = new Bitmap(dstWidthPixels, dstHeightPixels))
                    {
                        // Drawing the source image to the new image scales it to the new size.
                        using (Graphics gr = Graphics.FromImage(dstImage))
                        {
                            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            gr.DrawImage(srcImage, 0, 0, dstWidthPixels, dstHeightPixels);
                        }

                        // Create JPEG encoder parameters with the quality setting.
                        ImageCodecInfo encoderInfo = GetEncoderInfo(ImageFormat.Jpeg);
                        EncoderParameters encoderParams = new EncoderParameters();
                        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, jpegQuality);

                        // Save the image as JPEG to a memory stream.
                        MemoryStream dstStream = new MemoryStream();
                        dstImage.Save(dstStream, encoderInfo, encoderParams);

                        // If the image saved as JPEG is smaller than the original, store it in the shape.
                        // Console.WriteLine("Original size {0}, new size {1}.", originalBytes.Length, dstStream.Length);
                        if (dstStream.Length < originalBytes.Length)
                        {
                            dstStream.Position = 0;
                            imageData.SetImage(dstStream);
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Catch an exception, log an error and continue if cannot process one of the images for whatever reason.
                Console.WriteLine("Error processing an image, ignoring. " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Gets the codec info for the specified image format. Throws if cannot find.
        /// </summary>
        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].FormatID == format.Guid)
                    return encoders[i];
            }

            throw new Exception("Cannot find a codec.");
        }
    }
}
