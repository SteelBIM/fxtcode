using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace CBSS.Core.Utility
{
    public class ZipHelper
    {
        public static int avg = 1024 * 1024 * 100;//100MB写一次  
        #region 压缩文件 和 文件夹
        /// <summary>
        /// 压缩文件 和 文件夹
        /// </summary>
        /// <param name="FileToZip">待压缩的文件或文件夹，全路径格式</param>
        /// <param name="ZipedFile">压缩后生成的压缩文件名，全路径格式</param>
        /// <returns>压缩是否成功</returns>
        public static bool Zip(string FileToZip, string ZipedFile)
        {
            return Zip(FileToZip, ZipedFile, "");
        }
        /// <summary>
        /// 压缩文件 和 文件夹，不压缩顶级目录
        /// </summary>
        /// <param name="FolderToZip">待压缩的文件夹，全路径格式</param>
        /// <param name="ZipedFile">压缩后生成的压缩文件名，全路径格式</param>
        /// <returns>压缩是否成功</returns>
        public static bool ZipNo(string FolderToZip, string ZipedFile)
        {
            if (!Directory.Exists(FolderToZip))
                return false;
            if (ZipedFile == string.Empty)
            {
                //如果为空则文件名为待压缩的文件名加上.rar
                ZipedFile = FolderToZip + ".zip";
            }
            ZipOutputStream s = new ZipOutputStream(File.Create(ZipedFile));
            s.SetLevel(6);
            string[] filenames = Directory.GetFiles(FolderToZip);
            ZipEntry entry = null;
            FileStream fs = null;
            foreach (string file in filenames)
            {
                //压缩文件
                fs = File.OpenRead(file);
                byte[] buffer = new byte[avg];
                entry = new ZipEntry(Path.GetFileName(file));
                entry.DateTime = DateTime.Now;
                entry.Size = fs.Length;
                s.PutNextEntry(entry);
                for (int i = 0; i < fs.Length; i += avg)
                {
                    if (i + avg > fs.Length)
                    {
                        //不足100MB的部分写剩余部分
                        buffer = new byte[fs.Length - i];
                    }
                    fs.Read(buffer, 0, buffer.Length);
                    s.Write(buffer, 0, buffer.Length);
                }
            }
            if (fs != null)
            {
                fs.Close();
                fs = null;
            }
            if (entry != null)
                entry = null;
            GC.Collect();
            GC.Collect(1);
            //压缩目录
            string[] folders = Directory.GetDirectories(FolderToZip);
            foreach (string folder in folders)
            {
                if (!ZipFileDictory(folder, s, "")) { };
            }
            s.Finish();
            s.Close();
            return true;
        }
        /// <summary>
        /// 压缩文件 和 文件夹
        /// </summary>
        /// <param name="FileToZip">待压缩的文件或文件夹，全路径格式</param>
        /// <param name="ZipedFile">压缩后生成的压缩文件名，全路径格式</param>
        /// <param name="Password">压缩密码</param>
        /// <returns>压缩是否成功</returns>
        public static bool Zip(string FileToZip, string ZipedFile, string Password)
        {
            if (Directory.Exists(FileToZip))
            {
                return ZipFileDictory(FileToZip, ZipedFile, Password);
            }
            else if (File.Exists(FileToZip))
            {
                return ZipFile(FileToZip, ZipedFile, Password);
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 解压
        /// <summary>  
        /// 功能：解压zip格式的文件。  
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径，全路径格式</param>  
        /// <param name="unZipDir">解压文件存放路径,全路径格式，为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>  
        /// <param name="err">出错信息</param>  
        /// <returns>解压是否成功</returns>  
        public static bool UnZip(string zipFilePath, string unZipDir)
        {
            if (zipFilePath == string.Empty)
            {
                throw new System.IO.FileNotFoundException("压缩文件不不能为空！");
            }
            if (!File.Exists(zipFilePath))
            {
                throw new System.IO.FileNotFoundException("压缩文件: " + zipFilePath + " 不存在!");
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), "");
            if (!unZipDir.EndsWith("//"))
                unZipDir += "//";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(unZipDir + directoryName);
                        }
                        if (!directoryName.EndsWith("//"))
                            directoryName += "//";
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
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
                    }//while  
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }//解压结束 
        #endregion

        #region 压缩目录

        /// <summary>
        /// 压缩目录
        /// </summary>
        /// <param name="FolderToZip">待压缩的文件夹，全路径格式</param>
        /// <param name="ZipedFile">压缩后的文件名，全路径格式，如果为空则文件名为待压缩的文件名加上.rar</param>
        /// <returns></returns>
        private static bool ZipFileDictory(string FolderToZip, string ZipedFile, string Password)
        {
            bool res;
            if (!Directory.Exists(FolderToZip))
                return false;
            if (ZipedFile == string.Empty)
            {
                //如果为空则文件名为待压缩的文件名加上.rar
                ZipedFile = FolderToZip + ".zip";
            }
            ZipOutputStream s = new ZipOutputStream(File.Create(ZipedFile));
            s.SetLevel(6);
            if (!string.IsNullOrEmpty(Password.Trim()))
                s.Password = Password.Trim();
            res = ZipFileDictory(FolderToZip, s, "");
            s.Finish();
            s.Close();
            return res;
        }

        #endregion

        #region 压缩文件

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="FileToZip">要进行压缩的文件名</param>
        /// <param name="ZipedFile">压缩后生成的压缩文件名，如果为空则文件名为待压缩的文件名加上.rar</param>
        /// <returns>压缩是否成功</returns>
        private static bool ZipFile(string FileToZip, string ZipedFile, string Password)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(FileToZip))
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + FileToZip + " 不存在!");
            if (ZipedFile == string.Empty)
            {
                //如果为空则文件名为待压缩的文件名加上.rar
                ZipedFile = FileToZip + ".zip";
            }
            FileStream ZipFile = null;
            ZipOutputStream ZipStream = null;
            ZipEntry ZipEntry = null;
            bool res = true;
            ZipFile = File.Create(ZipedFile);
            ZipStream = new ZipOutputStream(ZipFile);
            ZipEntry = new ZipEntry(Path.GetFileName(FileToZip));
            ZipStream.PutNextEntry(ZipEntry);
            ZipStream.SetLevel(6);
            if (!string.IsNullOrEmpty(Password.Trim()))
                ZipStream.Password = Password.Trim();
            try
            {
                ZipFile = File.OpenRead(FileToZip);
                byte[] buffer = new byte[avg];
                for (int i = 0; i < ZipFile.Length; i += avg)
                {
                    if (i + avg > ZipFile.Length)
                    {
                        //不足100MB的部分写剩余部分
                        buffer = new byte[ZipFile.Length - i];
                    }
                    ZipFile.Read(buffer, 0, buffer.Length);
                    ZipStream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception)
            {
                res = false;
            }
            finally
            {
                if (ZipEntry != null)
                {
                    ZipEntry = null;
                }
                if (ZipStream != null)
                {
                    ZipStream.Finish();
                    ZipStream.Close();
                }
                if (ZipFile != null)
                {
                    ZipFile.Close();
                    ZipFile = null;
                }
                GC.Collect();
                GC.Collect(1);
            }

            return res;
        }

        #endregion

        #region 递归压缩文件夹方法
        /// <summary>
        /// 递归压缩文件夹方法
        /// </summary>
        /// <param name="FolderToZip"></param>
        /// <param name="s"></param>
        /// <param name="ParentFolderName"></param>
        private static bool ZipFileDictory(string FolderToZip, ZipOutputStream s, string ParentFolderName)
        {
            bool res = true;
            string[] folders, filenames;
            ZipEntry entry = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();
            try
            {
                //创建当前文件夹
                entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/"));  //加上 “/” 才会当成是文件夹创建
                s.PutNextEntry(entry);
                s.Flush();
                //先压缩文件，再递归压缩文件夹
                filenames = Directory.GetFiles(FolderToZip);
                foreach (string file in filenames)
                {
                    //打开压缩文件
                    fs = File.OpenRead(file);
                    byte[] buffer = new byte[avg];
                    entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/" + Path.GetFileName(file)));
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    s.PutNextEntry(entry);
                    for (int i = 0; i < fs.Length; i += avg)
                    {
                        if (i + avg > fs.Length)
                        {
                            //不足100MB的部分写剩余部分
                            buffer = new byte[fs.Length - i];
                        }
                        fs.Read(buffer, 0, buffer.Length);
                        s.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch (Exception)
            {
                res = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                if (entry != null)
                    entry = null;
                GC.Collect();
                GC.Collect(1);
            }
            folders = Directory.GetDirectories(FolderToZip);
            foreach (string folder in folders)
            {
                if (!ZipFileDictory(folder, s, Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip))))
                    return false;
            }
            return res;
        }

        #endregion

        #region 解压
        /// <summary>  
        /// 功能：解压zip格式的文件。  
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径，全路径格式</param>  
        /// <param name="unZipDir">解压文件存放路径,全路径格式，为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>  
        /// <param name="err">出错信息</param>  
        /// <returns>解压是否成功</returns>  
        public static bool UnZip2(string zipFilePath, string unZipDir)
        {
            
            if (zipFilePath == string.Empty)
            {
                throw new System.IO.FileNotFoundException("压缩文件不不能为空！");
            }
            if (!File.Exists(zipFilePath))
            {
                throw new System.IO.FileNotFoundException("压缩文件: " + zipFilePath + " 不存在!");
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), "");
            if (!unZipDir.EndsWith("//"))
                unZipDir += "//";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(unZipDir + directoryName);
                        }
                        if (!directoryName.EndsWith("//"))
                            directoryName += "//";
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
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
                    }//while  
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
            return true;
        }//解压结束 
        #endregion

        /// <summary> 
        /// 获取指定文件夹下所有子目录及文件 
        /// </summary> 
        /// <param name="Path">详细路径</param> 
        public static string GetFoldAll(string Path)
        {
            string str = "";
            DirectoryInfo thisOne = new DirectoryInfo(Path);
            str = ListTreeShow(thisOne, 0, str);
            return str;
        }
        /// <summary> 
        /// 获取指定文件夹下所有子目录及文件函数 
        /// </summary> 
        /// <param name="theDir">指定目录</param> 
        /// <param name="nLevel">默认起始值,调用时,一般为0</param> 
        /// <param name="Rn">用于迭加的传入值,一般为空</param> 
        /// <returns></returns> 
        public static string ListTreeShow(DirectoryInfo theDir, int nLevel, string Rn)//递归目录 文件 
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories();//获得目录 
            foreach (DirectoryInfo dirinfo in subDirectories)
            {
                if (nLevel == 0)
                {
                    Rn += "├";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "│ ";
                    }
                    Rn += _s + "├";
                }
                Rn += "<b>" + dirinfo.Name.ToString() + "</b><br />";
                FileInfo[] fileInfo = dirinfo.GetFiles(); //目录下的文件 
                foreach (FileInfo fInfo in fileInfo)
                {
                    if (nLevel == 0)
                    {
                        Rn += "│ ├";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "│ ";
                        }
                        Rn += _f + "│ ├";
                    }

                    if (System.IO.Path.GetExtension(fInfo.Name) == ".zip")
                    {
                        UnZip2(dirinfo.FullName + "\\" + fInfo, "");
                        File.Delete(dirinfo.FullName + "\\" + fInfo);
                    }
                    Rn += fInfo.Name.ToString() + " <br />";
                }
                Rn = ListTreeShow(dirinfo, nLevel + 1, Rn);

            }
            return Rn;
        }

        /**************************************** 
        * 函数名称：GetFoldAll(string Path) 
        * 功能说明：获取指定文件夹下所有子目录及文件(下拉框形) 
        * 参 数：Path:详细路径 
        * 调用示列： 
        * string strDirlist = Server.MapPath("templates"); 
        * this.Literal2.Text = EC.FileObj.GetFoldAll(strDirlist,"tpl",""); 
        *****************************************/
        /// <summary> 
        /// 获取指定文件夹下所有子目录及文件(下拉框形) 
        /// </summary> 
        /// <param name="Path">详细路径</param> 
        ///<param name="DropName">下拉列表名称</param> 
        ///<param name="tplPath">默认选择模板名称</param> 
        public static string GetFoldAll(string Path, string DropName, string tplPath)
        {
            string strDrop = "<select name=\"" + DropName + "\" id=\"" + DropName + "\"><option value=\"\">--请选择详细模板--</option>";
            string str = "";
            DirectoryInfo thisOne = new DirectoryInfo(Path);
            str = ListTreeShow(thisOne, 0, str, tplPath);
            return strDrop + str + "</select>";
        }
        /// <summary> 
        /// 获取指定文件夹下所有子目录及文件函数 
        /// </summary> 
        /// <param name="theDir">指定目录</param> 
        /// <param name="nLevel">默认起始值,调用时,一般为0</param> 
        /// <param name="Rn">用于迭加的传入值,一般为空</param> 
        /// <param name="tplPath">默认选择模板名称</param> 
        /// <returns></returns> 
        public static string ListTreeShow(DirectoryInfo theDir, int nLevel, string Rn, string tplPath)//递归目录 文件 
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories();//获得目录 
            foreach (DirectoryInfo dirinfo in subDirectories)
            {
                Rn += "<option value=\"" + dirinfo.Name.ToString() + "\"";
                if (tplPath.ToLower() == dirinfo.Name.ToString().ToLower())
                {
                    Rn += " selected ";
                }
                Rn += ">";
                if (nLevel == 0)
                {
                    Rn += "┣";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "│ ";
                    }
                    Rn += _s + "┣";
                }
                Rn += "" + dirinfo.Name.ToString() + "</option>";

                FileInfo[] fileInfo = dirinfo.GetFiles(); //目录下的文件 
                foreach (FileInfo fInfo in fileInfo)
                {
                    Rn += "<option value=\"" + dirinfo.Name.ToString() + "/" + fInfo.Name.ToString() + "\"";
                    if (tplPath.ToLower() == fInfo.Name.ToString().ToLower())
                    {
                        Rn += " selected ";
                    }
                    Rn += ">";
                    if (nLevel == 0)
                    {
                        Rn += "│ ├";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "│ ";
                        }
                        Rn += _f + "│ ├";
                    }
                    Rn += fInfo.Name.ToString() + "</option>";
                }
                Rn = ListTreeShow(dirinfo, nLevel + 1, Rn, tplPath);

            }
            return Rn;
        }
    }
}
