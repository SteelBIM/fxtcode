using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.Web.Hosting;

namespace CAS.Common
{
    public class FileHelper
    {
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool ExistsFile(string file)
        {
            return File.Exists(file);
        }

        public static void DeleteFiles(string[] fileList)
        {
            for (int i = 0; i < fileList.Length; i++)
            {
                if (string.IsNullOrEmpty(fileList[i])) continue;
                File.Delete(fileList[i]);
            }
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string Read(string file)
        {
            string sLine = "";
            if (ExistsFile(file))
            {
                StreamReader objReader = new StreamReader(file);
                sLine = objReader.ReadToEnd();
                objReader.Close();
            }
            return sLine;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        public static void Write(string file, string content)
        {
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(content);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        public static string PicRootPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("/ProjectPic");
            }
        }
        static string RootPath = string.Empty;
        public static void CompressDirectory(string path, string compressedFilePath)
        {
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                RootPath = dir.FullName + @"\";

                FileInfo fi = new FileInfo(compressedFilePath);

                FileStream compressedFileStream = null;
                compressedFileStream = fi.Create();

                if (null != compressedFileStream)
                {
                    ZipOutputStream zipOutStream = new ZipOutputStream(compressedFileStream);
                    zipOutStream.UseZip64 = UseZip64.On;
                    CreateEntry(path, zipOutStream);
                    zipOutStream.Flush();
                    zipOutStream.Finish();
                    zipOutStream.Close();
                }
            }
        }
        /// <summary>
        /// 添加压缩文件
        /// </summary>
        /// <param name="path">压缩文件总目录</param>
        /// <param name="pathlist">要压缩的文件</param>
        /// <param name="compressedFilePath"></param>
        public static void CompressDirectory(string path, List<string> pathlist, string compressedFilePath)
        {
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                RootPath = dir.FullName + @"\";

                FileInfo fi = new FileInfo(compressedFilePath);

                FileStream compressedFileStream = null;
                compressedFileStream = fi.Create();

                if (null != compressedFileStream)
                {
                    ZipOutputStream zipOutStream = new ZipOutputStream(compressedFileStream);
                    zipOutStream.UseZip64 = UseZip64.On;
                    CreateEntry(pathlist, zipOutStream);
                    zipOutStream.Flush();
                    zipOutStream.Finish();
                    zipOutStream.Close();
                }
            }
        }

        /// <summary>
        /// COPY目录和子目录
        /// </summary>
        /// <param name="srcDir">源路径</param>
        /// <param name="tgtDir">目标路径</param>
        public static void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);
            /*
                    if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase)) 
                    { 
                        throw new Exception("父目录不能拷贝到子目录！"); 
                    } 
            */
            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteDirectory(string dir)
        {
            DirectoryInfo source = new DirectoryInfo(dir);
            if (!source.Exists)
            {
                return;
            }
            FileInfo[] files = source.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    File.Delete(files[i].FullName);
                }
                catch { }
            }

            DirectoryInfo[] dirs = source.GetDirectories();
            for (int j = 0; j < dirs.Length; j++)
            {
                DeleteDirectory(dirs[j].FullName);
            }
            try
            {
                Directory.Delete(dir);
            }
            catch { }
        }

        public static Dictionary<string, string> dic = new Dictionary<string, string>();
        /// <summary>
        /// 只打包图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="compressedFilePath"></param>
        /// <param name="sift"></param>
        /// <param name="syscode"></param>
        public static void CompressDirectory(string path, string compressedFilePath, List<string> sift)
        {
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                dic = new Dictionary<string, string>();
                DirectoryInfo dir = new DirectoryInfo(path);
                //ZIP相对位置
                RootPath = dir.FullName;
                FileStream compressedFileStream = null;
                compressedFileStream = File.Create(compressedFilePath);
                if (null != compressedFileStream)
                {
                    ZipOutputStream zipOutStream = new ZipOutputStream(compressedFileStream);
                    zipOutStream.UseZip64 = UseZip64.On;
                    //打包pic目录
                    CreateEntry(path, zipOutStream, sift);
                    zipOutStream.Flush();
                    zipOutStream.Finish();
                    zipOutStream.Close();

                }
                //个人版本全部打包
            }
        }

        /// <summary>
        /// 签章平台 打包下载所有机构统计
        /// </summary>
        /// <param name="path"></param>
        /// <param name="compressedFilePath"></param>
        /// <param name="sift"></param>
        /// <param name="syscode"></param>
        public static void CompressDirectoryAss(string path, string compressedFilePath, List<string> sift)
        {
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                dic = new Dictionary<string, string>();
                DirectoryInfo dir = new DirectoryInfo(path);
                RootPath = dir.FullName;
                FileStream compressedFileStream = null;
                compressedFileStream = File.Create(compressedFilePath);
                if (null != compressedFileStream)
                {
                    ZipOutputStream zipOutStream = new ZipOutputStream(compressedFileStream);
                    zipOutStream.UseZip64 = UseZip64.On;
                    CreateEntry(path, zipOutStream, sift);
                    zipOutStream.Flush();
                    zipOutStream.Finish();
                    zipOutStream.Close();
                    //删除用于ZIP的目录
                    DeleteDirectory(path + "\\zip");
                }
            }
        }

        /// <summary>
        /// 添加文件至压缩，文件存在则替换
        /// </summary>
        /// <param name="directoryName">zip文件目录名</param>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">文件地址</param>
        /// <param name="compressedFilePath">zip地址</param>
        public static void ReplaceCompressFile(string directoryName, string fileName, string filePath, string compressedFilePath)
        {
            if (File.Exists(compressedFilePath))
            {
                //准备写入内容
                FileStream fileStream = File.OpenRead(filePath);
                byte[] b = new byte[fileStream.Length];
                fileStream.Read(b, 0, b.Length);
                MemoryStream contentStream = new MemoryStream(b);
                fileStream.Dispose();
                contentStream.Seek(0, SeekOrigin.Begin);

                FileStream zipStream = File.Open(compressedFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                ZipFile zipFile = new ZipFile(zipStream);
                zipFile.UseZip64 = UseZip64.On;
                zipFile.BeginUpdate();

                try
                {
                    zipFile.Add(new StreamStaticDataSource(contentStream), directoryName + @"\" + fileName, CompressionMethod.Deflated, true);
                }
                finally
                {
                    if (zipFile != null)
                    {
                        if (zipFile.IsUpdating)
                            zipFile.CommitUpdate();
                        zipFile.Close();
                    }
                    if (zipStream != null)
                        zipStream.Close();
                }
                contentStream.Dispose();
            }
        }
        /// <summary>
        /// 压缩一组文件
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="outputCompressedFilePath">压缩后输出的压缩文件路径(包含文件名)</param>
        public static void CompressFile(string[] fileList, string outputCompressedFilePath)
        {
            using (ZipOutputStream s = new ZipOutputStream(File.Create(outputCompressedFilePath)))
            {
                s.UseZip64 = UseZip64.On;
                s.SetLevel(9);// 0 - store only to 9 - means best compression
                byte[] buffer = new byte[4096];
                foreach (string file in fileList)
                {
                    if (string.IsNullOrEmpty(file)) continue;
                    // Using GetFileName makes the result compatible with XP
                    // as the resulting path is not absolute.
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                    entry.IsUnicodeText = true;

                    // Setup the entry data as required.

                    // Crc and size are handled by the library for seakable streams
                    // so no need to do them here.

                    // Could also use the last write time or similar for the file.
                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);
                    using (FileStream fs = File.OpenRead(file))
                    {
                        // Using a fixed size buffer here makes no noticeable difference for output
                        // but keeps a lid on memory usage.
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }

                // Finish/Close arent needed strictly as the using statement does this automatically

                // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                // the created file would be invalid.
                s.Finish();

                // Close is important to wrap things up and unlock the file.
                s.Close();
            }
        }

        /// <summary>
        /// 压缩一组文件(文件名为：上一级目录名称+文件名称)
        /// caoq 2014-02-26
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="outputCompressedFilePath">压缩后输出的压缩文件路径(包含文件名)</param>
        public static void CompressFileContainDirName(string[] fileList, string outputCompressedFilePath)
        {
            using (ZipOutputStream s = new ZipOutputStream(File.Create(outputCompressedFilePath)))
            {
                s.UseZip64 = UseZip64.On;
                s.SetLevel(9);// 0 - store only to 9 - means best compression
                byte[] buffer = new byte[4096];
                foreach (string file in fileList)
                {
                    if (string.IsNullOrEmpty(file)) continue;

                    // Using GetFileName makes the result compatible with XP
                    // as the resulting path is not absolute.

                    //获取文件上级目录名称
                    string dirname = Path.GetDirectoryName(file);
                    if (!string.IsNullOrEmpty(dirname))
                    {
                        dirname = dirname.Substring(dirname.LastIndexOf("\\") + 1);
                    }
                    ZipEntry entry = new ZipEntry(dirname + "_" + Path.GetFileName(file));

                    entry.IsUnicodeText = true;

                    // Setup the entry data as required.

                    // Crc and size are handled by the library for seakable streams
                    // so no need to do them here.

                    // Could also use the last write time or similar for the file.
                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);
                    using (FileStream fs = File.OpenRead(file))
                    {
                        // Using a fixed size buffer here makes no noticeable difference for output
                        // but keeps a lid on memory usage.
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }

                // Finish/Close arent needed strictly as the using statement does this automatically

                // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                // the created file would be invalid.
                s.Finish();

                // Close is important to wrap things up and unlock the file.
                s.Close();
            }
        }

        /// <summary>
        /// 压缩一组文件(包含文件上一级目录)
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="outputCompressedFilePath">压缩后输出的压缩文件路径(包含文件名)</param>
        public static void CompressFileContainDir(string[] fileList, string outputCompressedFilePath)
        {
            using (ZipOutputStream s = new ZipOutputStream(File.Create(outputCompressedFilePath)))
            {
                s.UseZip64 = UseZip64.On;
                s.SetLevel(9);// 0 - store only to 9 - means best compression
                byte[] buffer = new byte[4096];
                foreach (string file in fileList)
                {
                    if (string.IsNullOrEmpty(file)) continue;

                    // Using GetFileName makes the result compatible with XP
                    // as the resulting path is not absolute.

                    //获取文件上级目录名称
                    string dirname = Path.GetDirectoryName(file);
                    if (!string.IsNullOrEmpty(dirname))
                    {
                        dirname = dirname.Substring(dirname.LastIndexOf("\\") + 1);
                    }
                    ZipEntry entry = new ZipEntry((string.IsNullOrEmpty(dirname) ? (Path.GetFileName(file)) : (dirname + @"\" + Path.GetFileName(file))));

                    entry.IsUnicodeText = true;

                    // Setup the entry data as required.

                    // Crc and size are handled by the library for seakable streams
                    // so no need to do them here.

                    // Could also use the last write time or similar for the file.
                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);
                    using (FileStream fs = File.OpenRead(file))
                    {
                        // Using a fixed size buffer here makes no noticeable difference for output
                        // but keeps a lid on memory usage.
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }

                // Finish/Close arent needed strictly as the using statement does this automatically

                // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                // the created file would be invalid.
                s.Finish();

                // Close is important to wrap things up and unlock the file.
                s.Close();
            }
        }

        public static void UnZipFile(string filePath, string subDirectory)
        {
            if (File.Exists(filePath))
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(filePath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        theEntry.IsUnicodeText = true;
                        Console.WriteLine(theEntry.Name);
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        if (!string.IsNullOrEmpty(subDirectory))
                        {
                            directoryName = subDirectory + @"\";
                        }
                        string fileName = Path.GetFileName(theEntry.Name);
                        // create directory
                        if (directoryName.Length > 0 && !Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(directoryName + theEntry.Name))
                            {

                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private static string GetRelativePath(string absolutePath)
        {
            return !string.IsNullOrEmpty(absolutePath) ? absolutePath.Replace(RootPath, "") : "";
        }

        private static void CreateEntry(string path, ZipOutputStream zipOutStream, List<string> sift)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                DirectoryInfo[] subDirArray = dirInfo.GetDirectories();

                foreach (DirectoryInfo dir in subDirArray)
                {
                    CreateEntry(dir.FullName, zipOutStream, sift);
                }
                FileInfo[] fileArray = dirInfo.GetFiles();
                if (fileArray.Length > 0)
                {
                    foreach (FileInfo file in fileArray)
                    {
                        if (file.Extension.ToLower().Contains("zip")) continue;
                        if (sift != null)
                        {
                            bool b = true;
                            for (int i = 0; i < sift.Count; i++)
                            {
                                if (file.FullName.IndexOf(sift[i]) != -1)
                                {
                                    b = false;
                                    break;
                                }
                            }
                            if (!b)
                            {
                                continue;
                            }
                        }
                        CreateEntry(file.FullName, zipOutStream, sift);
                    }
                }
                else
                {
                    //zipOutStream.PutNextEntry(new ZipEntry(GetRelativePath(path) + @"\"));
                }
            }
            else if (File.Exists(path))
            {
                zipOutStream.SetLevel(1);
                FileStream fileStream = File.OpenRead(path);
                byte[] b = new byte[fileStream.Length];
                fileStream.Read(b, 0, b.Length);

                ZipEntry zipentry = new ZipEntry(GetRelativePath(path));
                zipentry.IsUnicodeText = true;

                zipOutStream.PutNextEntry(zipentry);
                zipOutStream.Write(b, 0, b.Length);
                fileStream.Close();
                zipOutStream.CloseEntry();

            }
        }

        private static void CreateEntry(string path, ZipOutputStream zipOutStream)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                DirectoryInfo[] subDirArray = dirInfo.GetDirectories();

                foreach (DirectoryInfo dir in subDirArray)
                {
                    CreateEntry(dir.FullName, zipOutStream);
                }
                FileInfo[] fileArray = dirInfo.GetFiles();
                if (fileArray.Length > 0)
                {
                    foreach (FileInfo file in fileArray)
                    {
                        CreateEntry(file.FullName, zipOutStream);
                    }
                }
                else
                {
                    //ZipEntry entry = new ZipEntry(GetRelativePath(path) + @"\");
                    //entry.IsUnicodeText = true;
                    //zipOutStream.PutNextEntry(entry);
                }
            }
            else if (File.Exists(path))
            {

                zipOutStream.SetLevel(1);
                FileStream fileStream = File.OpenRead(path);
                byte[] b = new byte[fileStream.Length];
                fileStream.Read(b, 0, b.Length);
                ZipEntry entry = new ZipEntry(GetRelativePath(path));
                entry.IsUnicodeText = true;
                zipOutStream.PutNextEntry(entry);
                zipOutStream.Write(b, 0, b.Length);
                fileStream.Close();
                zipOutStream.CloseEntry();

            }
        }
        /// <summary>
        /// 添加压缩文件
        /// </summary>
        /// <param name="pathlist">压缩文件路径集合</param>
        /// <param name="zipOutStream"></param>
        private static void CreateEntry(List<string> pathlist, ZipOutputStream zipOutStream)
        {
            foreach (string path in pathlist)
            {
                if (Directory.Exists(path))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(path);
                    DirectoryInfo[] subDirArray = dirInfo.GetDirectories();

                    foreach (DirectoryInfo dir in subDirArray)
                    {
                        CreateEntry(dir.FullName, zipOutStream);
                    }
                    FileInfo[] fileArray = dirInfo.GetFiles();
                    if (fileArray.Length > 0)
                    {
                        foreach (FileInfo file in fileArray)
                        {
                            CreateEntry(file.FullName, zipOutStream);
                        }
                    }
                }
                else if (File.Exists(path))
                {

                    zipOutStream.SetLevel(1);
                    FileStream fileStream = File.OpenRead(path);
                    byte[] b = new byte[fileStream.Length];
                    fileStream.Read(b, 0, b.Length);
                    ZipEntry entry = new ZipEntry(GetRelativePath(path));
                    entry.IsUnicodeText = true;
                    zipOutStream.PutNextEntry(entry);
                    zipOutStream.Write(b, 0, b.Length);
                    fileStream.Close();
                    zipOutStream.CloseEntry();
                }
            }
        }

        public static int SaveFile(byte[] fileBit, string path, string fileName, string UpType)
        {
            return SaveFile(fileBit, path, fileName, UpType, false, false, false, null);
            /*
            FileStream stream = null;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                stream = new FileStream(path + "/" + fileName, FileMode.Create);
                stream.Seek(0, SeekOrigin.Begin);
                stream.Write(fileBit, 0, fileBit.Length);
                stream.Dispose();

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
            */
        }
        /// <summary>
        /// 生成token，Webform中使用
        /// </summary>
        /// <param name="token"></param>
        public static void SaveToken(string token,string content) 
        {
            
            string uploadroot = "upload/token";
            string savepath = HostingEnvironment.MapPath("/" + uploadroot + "/");
            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);
            //string type = funinfo.Value<string>("type");
            string filename = token + ".token";
            string path = Path.Combine(savepath, filename);
            StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("gb2312"));
            if (string.IsNullOrEmpty(content)) 
            {
                content = "";
            }
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// 生成token，Webform中使用
        /// </summary>
        /// <param name="token"></param>
        public static void SaveTokenForWinform(string token, string content)
        {
            string savepath = WebCommon.GetConfigSetting("filepath");
            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);
            //string type = funinfo.Value<string>("type");
            string filename = token + ".token";
            string path = Path.Combine(savepath, filename);
            StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("gb2312"));
            if (string.IsNullOrEmpty(content))
            {
                content = "";
            }
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }

        public static int SaveFile(byte[] fileBit, string path, string fileName, string UpType, bool needbase, bool needborder, bool needmark, DateTime? marktime)
        {
            FileStream stream = null;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                string savePath = path + "/" + fileName;
                string extName = fileName.Substring(fileName.LastIndexOf('.'));
                //需要保存原图
                if (UpType == "1" && needbase)
                {
                    //原图
                    savePath = path + "/" + fileName.Replace(extName, "_base" + extName);
                }
                stream = new FileStream(savePath, FileMode.Create);
                stream.Seek(0, SeekOrigin.Begin);
                stream.Write(fileBit, 0, fileBit.Length);
                stream.Dispose();

                //图片需要边框或水印
                if (UpType == "1" && (needborder || needmark))
                {
                    Image img = System.Drawing.Image.FromFile(savePath);
                    Graphics markg = Graphics.FromImage(img);
                    markg.DrawImage(img, 0, 0, img.Width, img.Height);
                    //需要黑边框
                    if (needborder)
                    {
                        markg.DrawRectangle(new Pen(Color.Black), 0, 0, img.Width - 1, img.Height - 1);
                    }
                    //需要水印
                    if (needmark && marktime != null && marktime > default(DateTime))
                    {
                        Font f = new Font("Verdana", 16);
                        Brush b = new SolidBrush(Color.Orange);
                        string str = marktime.Value.ToString("yyyy/MM/dd");
                        markg.DrawString(str, f, b, img.Width - 135, img.Height - 30);
                    }
                    markg.Dispose();
                    img.Save(path + "/" + fileName);
                    img.Dispose();
                }

                if (UpType == "1")
                {
                    string imgName120 = fileName.Replace(extName, "_120" + extName);
                    ImageUtil.MakeThumbnail(path + "/" + fileName, path + "/" + imgName120, 120, 120, ImageUtil.ThumbnailCompressType.BaseOnProportion, fileName.Substring(fileName.LastIndexOf('.') + 1));
                    //ImageUtil.GetThumbnail(path + "/" + fileName, path + "/" + imgName120, 120, 120);
                    string imgName480 = fileName.Replace(extName, "_480" + extName);
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

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static long GetFileSize(string path)
        {
            FileInfo inf = new FileInfo(path);
            FileStream fs = inf.OpenRead();
            long filesize = fs.Length;
            fs.Close();
            fs.Dispose();
            return filesize;
        }
    }

    public class ImageUtil
    {

        /// <SUMMARY>
        /// 图片缩放
        /// </SUMMARY>
        /// <PARAM name="sourceFile">图片源路径</PARAM>
        /// <PARAM name="destFile">缩放后图片输出路径</PARAM>
        /// <PARAM name="destHeight">缩放后图片高度</PARAM>
        /// <PARAM name="destWidth">缩放后图片宽度</PARAM>
        /// <RETURNS></RETURNS>
        public static bool GetThumbnail(string sourceFile, string destFile, int destHeight, int destWidth)
        {
            System.Drawing.Image imgSource = System.Drawing.Image.FromFile(sourceFile);
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;

            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }

            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.WhiteSmoke);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            try
            {
                //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICI = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[x];//设置JPEG编码
                        break;
                    }
                }

                if (jpegICI != null)
                {
                    outBmp.Save(destFile, jpegICI, encoderParams);
                }
                else
                {
                    outBmp.Save(destFile, thisFormat);
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                imgSource.Dispose();
                outBmp.Dispose();
            }
        }


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
            BaseOnProportion = 16
        }
    }

    public class StreamStaticDataSource : IStaticDataSource
    {
        private Stream _stream;

        public StreamStaticDataSource(Stream stream)
        {
            _stream = stream;
        }

        #region IStaticDataSource Members

        public Stream GetSource()
        {
            return _stream;
        }

        #endregion
    }
}
