using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SharpCompress.Archive;
using SharpCompress.Archive.Zip;
using SharpCompress.Common;
using SharpCompress.Reader;


namespace FXT.DataCenter.Infrastructure.Common.Common
{
    /// <summary>
    /// Zip 压缩文件
    /// </summary>
    public class CompressionHelper
    {
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="sourceFile">源压缩文件</param>
        /// <param name="destPath">解压路径(默认为当前路径)</param>
        public static void Decompression(string sourceFile, string destPath = null)
        {

            if (string.IsNullOrEmpty(sourceFile)) throw new Exception("源文件不存在！");

            var extension = Path.GetExtension(sourceFile);

            if (string.IsNullOrEmpty(extension)) throw new Exception("源文件不是有效的压缩文件格式！");

            if (string.IsNullOrEmpty(destPath))
                destPath = sourceFile.Replace(Path.GetFileName(sourceFile), Path.GetFileNameWithoutExtension(sourceFile));

            if (!destPath.EndsWith("//")) destPath += "//";

            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            if (extension.ToUpper().Equals(".RAR"))
            {
                DecompressionRar(sourceFile, destPath);
            }

            if (extension.ToUpper().Equals(".ZIP"))
            {
                DecompressionZip(sourceFile, destPath);
            }
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="destFile">压缩的文件</param>
        public static void Compression(string sourcePath, string destFile)
        {

            using (var archive = ZipArchive.Create())
            {
                archive.AddAllFromDirectory(sourcePath);
                archive.SaveTo(destFile, new CompressionInfo { Type = CompressionType.Deflate });
            }

        }

        private static void DecompressionRar(string sourceFile, string destPath)
        {
            using (Stream stream = File.OpenRead(sourceFile))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        reader.WriteEntryToDirectory(destPath, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                }
            }
        }

        private static void DecompressionZip(string sourceFile, string destPath)
        {
            var archive = ArchiveFactory.Open(sourceFile);
            foreach (var entry in archive.Entries)
            {
                if (!entry.IsDirectory)
                {
                    entry.WriteToDirectory(destPath, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                }
            }
        }
    }
}
