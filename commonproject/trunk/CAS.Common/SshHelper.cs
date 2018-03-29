using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Renci.SshNet;
using System.IO;

namespace CAS.Common
{
    //public class BonaFileServiceConfig
    //{
    //    //public static readonly int FILE_MAX_LENGTH = 1 * 1024 * 1024 * 10;		// 最大不超过10M;
    //    //public static readonly String INET_URL_ADDR = "113.108.40.3";
    //    //public static readonly String IMG_SERVER_ADDR = "113.108.40.3";
    //    //public static readonly String IMG_SERVER_PORT = "7999";

    //    public static readonly String SCP_PATH = "/images"; 			// 文件目录 
    //    public static readonly String SCP_ADDR = "113.108.40.3";		// ip地址 
    //    public static readonly int SCP_PORT = 22022;				    // 端口号 
    //    public static readonly String SCP_USERNAME = "nginx"; 				// 用户名 
    //    public static readonly String SCP_PASSWD = "Bona#3edc";			// 密码 

    //    public static readonly String SCP_TypePath = "Weixin";			

    //    //public static readonly int FTP_PORT = 21;
    //    //public static readonly String FTP_ADDR = "10.253.40.122";
    //    //public static readonly String FTP_WORK_DIR = "/images";
    //    //public static readonly String FTP_USERNAME = "nginx";
    //    //public static readonly String FTP_PASSWORD = "Bona#3edc";
    //    //public static readonly int FTP_BUFFER_SIZE = 1024 * 1024 * 2;
    //    //public static readonly String FTP_CONTROL_ENCODING = "UTF-8";



    //}

    public class BonaFileServiceConfig
    {
        //public static readonly int FILE_MAX_LENGTH = 1 * 1024 * 1024 * 10;		// 最大不超过10M;
        //public static readonly String INET_URL_ADDR = "113.108.40.3";
        //public static readonly String IMG_SERVER_ADDR = "113.108.40.3";
        //public static readonly String IMG_SERVER_PORT = "7999";

        public static readonly String SCP_PATH = "/images"; 			// 文件目录 
        public static readonly String SCP_ADDR = "113.108.40.3";		// ip地址 测试地址10.253.40.122
        public static readonly int SCP_PORT = 22022;				    // 端口号 
        public static readonly String SCP_USERNAME = "nginx"; 				// 用户名 
        public static readonly String SCP_PASSWD = "Bona#3edc";			// 密码 

        public static readonly String SCP_TypePath = "Weixin";		

        //public static readonly int FTP_PORT = 21;
        //public static readonly String FTP_ADDR = "10.253.40.122";
        //public static readonly String FTP_WORK_DIR = "/images";
        //public static readonly String FTP_USERNAME = "nginx";
        //public static readonly String FTP_PASSWORD = "Bona#3edc";
        //public static readonly int FTP_BUFFER_SIZE = 1024 * 1024 * 2;
        //public static readonly String FTP_CONTROL_ENCODING = "UTF-8";



    }
    public class SshHelper
    {
        string server = BonaFileServiceConfig.SCP_ADDR;
        string username = BonaFileServiceConfig.SCP_USERNAME;
        string password = BonaFileServiceConfig.SCP_PASSWD;
        int port = BonaFileServiceConfig.SCP_PORT;
        string rootpath = BonaFileServiceConfig.SCP_PATH;

        string typepath = BonaFileServiceConfig.SCP_TypePath;

        /// <summary>
        /// 上传到scp服务器，返回服务器文件路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public string UploadFileToBonaBySCP(string filePath)
        {
            string scpFilePath = string.Empty;
            try
            {
                using (var scp = new ScpClient(server, port, username, password))
                {
                    scp.Connect();

                    FileInfo file = new FileInfo(filePath);

                    string fullPath = rootpath + "/" + typepath + "/" + DateTime.Now.ToString("yyyyMMdd");

                    int code = MkdirFromDateFormate(fullPath);


                    scp.Upload(file, fullPath);

                    scp.Disconnect();

                    scpFilePath = typepath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + file.Name;
                }

            }
            catch (Exception e1)
            {
                LogHelper.Error(e1);
            }

            return scpFilePath;
        }


        /**
	 * 在linux上创建目录
	 * @param conn
	 * @param prefix
	 * @return
	 * @throws Exception 
	 */
        private int MkdirFromDateFormate(String path)
        {
            int exitStatus = -1;
            try
            {
                var ssh = new SshClient(server, port, username, password);
                ssh.Connect();

                // ssh.RunCommand("cd " + BonaFileServiceConfig.SCP_PATH + " && mkdir -p " + path + " && pwd");

                var cmd = ssh.CreateCommand("cd " + BonaFileServiceConfig.SCP_PATH + " && mkdir -p " + path + " && pwd");

                var rusult = cmd.Execute();

                exitStatus = cmd.ExitStatus;

                cmd.Dispose();

                ssh.Disconnect();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);

            }

            //ssh

            //Session sshSession = conn.

            //sshSession.execCommand("cd " + CommonConstants.FileUpload.SCP_PATH + " && mkdir -p " + path + " && pwd");




            //InputStream stdOut = new StreamGobbler(sshSession.getStdout());
            //String outStr = processStream(stdOut, "UTF-8");
            //InputStream stdErr = new StreamGobbler(sshSession.getStderr());
            //String outErr = processStream(stdErr, "UTF-8");

            //sshSession.waitForCondition(ChannelCondition.EXIT_STATUS, 10000);

            //logger.info("outStr=" + outStr);
            //logger.info("outErr=" + outErr);
            //exitStatus = sshSession.getExitStatus();
            //sshSession.close();
            //logger.info("status: " + exitStatus);
            return exitStatus;
        }
    }
}