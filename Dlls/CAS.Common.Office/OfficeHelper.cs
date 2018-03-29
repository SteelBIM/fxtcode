using System;
using System.Collections.Generic;
using System.Text;

namespace CAS.Common.Office
{
    public class OfficeHelper
    {
        private static DBstep.iMsgServer2000 MsgObj;
        private static string mFileType;
        private static string mRecordID;
        private static string mOption;

        private static string mUserName;

        public static byte[] HandlerFile(string file)
        {
            // 在此处放置用户代码以初始化页面
            MsgObj = new DBstep.iMsgServer2000();

            MsgObj.MsgVariant(System.Web.HttpContext.Current.Request.BinaryRead(System.Web.HttpContext.Current.Request.ContentLength));
            if (MsgObj.GetMsgByName("DBSTEP").Equals("DBSTEP"))                                //如果是合法的信息包
            {
                mOption = MsgObj.GetMsgByName("OPTION");                                         //取得操作信息
                mUserName = MsgObj.GetMsgByName("USERNAME");									   //取得操作用户名称
                if (mOption.Equals("LOADFILE"))						                           //下面的代码为打开服务器数据库里的文件
                {
                    mRecordID = MsgObj.GetMsgByName("RECORDID");		                          //取得文档编号
                    mFileType = MsgObj.GetMsgByName("FILETYPE");	                              //取得文档类型
                    MsgObj.MsgTextClear();                                                    //清除文本信息

                    if (MsgObj.MsgFileLoad(file))				                          //调入文档
                    {
                        //MsgObj.MsgFileBody(mFileBody);					                  //将文件信息打包，mFileBody为从数据库中读取，类型byte[]
                        MsgObj.SetMsgByName("STATUS", "打开全文批注成功!");		              //设置状态信息
                        MsgObj.MsgError("");		                                          //清除错误信息
                    }
                    else
                    {
                        MsgObj.MsgError("打开全文批注失败!");		                          //设置错误信息
                    }
                }
                else if (mOption.Equals("SAVEFILE"))					                          //下面的代码为保存文件在服务器的数据库里
                {
                    mRecordID = MsgObj.GetMsgByName("RECORDID");		                          //取得文档编号
                    mFileType = MsgObj.GetMsgByName("FILETYPE");		                          //取得文档类型
                    //mMyDefine1=MsgObj.GetMsgByName("MyDefine1");	                          //取得客户端传递变量值 MyDefine1="自定义变量值1"
                    //mFileBody=MsgObj.MsgFileBody();	                                      //取得文档内容 mFileBody可以保存到数据库中，类型byte[]
                    MsgObj.MsgTextClear();                                                    //清除文本信息
                    if (MsgObj.MsgFileSave(file))				                          //保存文档内容
                    {
                        MsgObj.SetMsgByName("STATUS", "保存全文批注成功!");	                  //设置状态信息
                        MsgObj.MsgError("");						                          //清除错误信息
                    }
                    else
                    {
                        MsgObj.MsgError("保存全文批注失败!");		                           //设置错误信息
                    }
                    MsgObj.MsgFileClear();
                }

            }
            else
            {
                MsgObj.MsgError("客户端发送数据包错误!");
                MsgObj.MsgTextClear();
                MsgObj.MsgFileClear();
            }

            return MsgObj.MsgVariant();
        }
    }
}
