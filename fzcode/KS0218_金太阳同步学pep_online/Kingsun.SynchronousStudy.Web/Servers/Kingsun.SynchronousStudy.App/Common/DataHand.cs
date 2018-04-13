using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Kingsun.SynchronousStudy.App.Controllers;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
namespace Kingsun.SynchronousStudy.App.Common
{
    public class DataHand : BaseManagement
    {
        ModuleConfigurationBLL moduleconfig = new ModuleConfigurationBLL();
        ModularSortBLL modularSortBLL = new ModularSortBLL();
        VersionChangeBLL versionbll = new VersionChangeBLL();

        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        /// <summary>
        /// 获取书籍的第一条记录
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool UpdateUserID(string userid, int bookid)
        {
            var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(userid));
            if (user != null)
            {
                user.BookID = bookid;
                return userBLL.Update(user);
            }
            else
            {
                return false;
            }
        }

        public List<TB_ModuleSort> GetModuleSort(string bookid)
        {
            //  string wheres = "select top 1 * from TB_ModuleConfiguration where BookID =" + bookid + " order by CreateDate desc";
            // string wheres = "select top 1 * from TB_ModuleConfiguration where BookID =" + bookid + " order by ID ";
            //string wheres = "select top 1  TB_ModuleSort.ID,TB_ModuleSort.BookID,TeachingNaterialName,TB_ModuleSort.FirstTitleID,FirstTitle,TB_ModuleSort.SecondTitleID,SecondTitle,CreateDate  from TB_ModuleSort left join TB_VersionChange on"
            //+ " TB_ModuleSort.BookID = TB_VersionChange.BooKID  where TB_VersionChange.BooKID ="+ bookid +" and TB_VersionChange.State=1";
            string wheres = " select top 1  TB_ModuleSort.ID,TB_ModuleSort.BookID,TeachingNaterialName,TB_ModuleSort.FirstTitleID,FirstTitle,TB_ModuleSort.SecondTitleID,SecondTitle,CreateDate  from TB_ModuleSort left join TB_VersionChange on"
  + " TB_ModuleSort.BookID = TB_VersionChange.BooKID and TB_VersionChange.FirstTitleID=TB_ModuleSort.FirstTitleID and TB_VersionChange.SecondTitleID= TB_ModuleSort.SecondTitleID where TB_VersionChange.BooKID =" + bookid + " and TB_VersionChange.State=1";

            DataSet ds = moduleconfig.GetModuleConfigurationByWhere(wheres);
            if (ds != null)
            {
                TB_ModuleConfiguration moduleconfigs = new TB_ModuleConfiguration();
                DataTable dt = ds.Tables[0];
                foreach (DataRow r in dt.Rows)
                {
                    moduleconfigs.ID = StringToInt(r["ID"].ToString());
                    moduleconfigs.BookID = StringToInt(r["BookID"].ToString());
                    moduleconfigs.FirstTitileID = StringToInt(r["FirstTitleID"].ToString());
                    moduleconfigs.SecondTitleID = StringToInt(r["SecondTitleID"].ToString());
                }
                //string where = "ModuleID=" + moduleconfigs.ID + " and BooKID=" + moduleconfigs.BookID + " and FirstTitleID=" + moduleconfigs.FirstTitileID + " and State=1";
                //if (moduleconfigs.SecondTitleID != null)
                //{
                //    where += " and SecondTitleID= " + moduleconfigs.SecondTitleID;
                //}
                //where += " order by CreateDate";
                //TB_VersionChange newversion = versionbll.GetNewVersionChange(where);
                string where = "BookID=" + moduleconfigs.BookID + " and FirstTitleID =" + moduleconfigs.FirstTitileID;
                if (moduleconfigs.SecondTitleID != null)
                {
                    where += " and SecondTitleID =" + moduleconfigs.SecondTitleID;
                }
                List<TB_ModuleSort> modularlist = modularSortBLL.GetModuleList(where) as List<TB_ModuleSort>;
                if (modularlist != null)
                {
                    return modularlist;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 默认只传书本id的时间
        /// </summary>
        /// <param name="bookid"></param>
        /// <returns></returns>
        public VersionChangeModuleSort GetVerModuleSort(string bookid)
        {
            // string wheres = " select top 1  TB_ModuleSort.ID,TB_ModuleSort.BookID,TB_VersionChange.ModuleID,TeachingNaterialName,TB_ModuleSort.FirstTitleID,FirstTitle,TB_ModuleSort.SecondTitleID,SecondTitle,CreateDate  from TB_ModuleSort left join TB_VersionChange on"
            //+ " TB_ModuleSort.BookID = TB_VersionChange.BooKID and TB_VersionChange.FirstTitleID=TB_ModuleSort.FirstTitleID and TB_VersionChange.SecondTitleID= TB_ModuleSort.SecondTitleID where TB_VersionChange.BooKID =" + bookid + " and TB_VersionChange.State=1";

            string where = "select top 1 TB_ModuleSort.ID,TB_ModuleSort.BookID,TB_VersionChange.ModuleID,TB_VersionChange.ModuleName,TB_ModuleSort.Sort,TB_ModuleSort.SuperiorID,TeachingNaterialName,TB_ModuleSort.FirstTitleID,FirstTitle,TB_ModuleSort.SecondTitleID,"
  + " SecondTitle,TB_VersionChange.ModuleAddress,MD5,IncrementalPacketAddress,IncrementalPacketMD5,ModuleVersion,TB_ModuleSort.Sort as VersionSort,CreateDate  from TB_ModuleSort left join TB_VersionChange on"
  + " TB_ModuleSort.BookID = TB_VersionChange.BooKID and TB_VersionChange.FirstTitleID=TB_ModuleSort.FirstTitleID and TB_VersionChange.SecondTitleID= TB_ModuleSort.SecondTitleID where TB_VersionChange.BooKID =" + bookid + " and TB_VersionChange.State=1";


            DataSet ds = moduleconfig.GetModuleConfigurationByWhere(where);
            VersionChangeModuleSort listverModul = new VersionChangeModuleSort();

            if (ds != null)
            {
                TB_ModuleConfiguration moduleconfigs = new TB_ModuleConfiguration();
                DataTable dt = ds.Tables[0];
                foreach (DataRow r in dt.Rows)
                {
                    listverModul.BookID = r["BookID"].ToString();
                    listverModul.ModuleID = r["ModuleID"].ToString();
                    listverModul.ModuleName = r["ModuleName"].ToString();
                    listverModul.ModuleSort = r["Sort"].ToString();
                    listverModul.SuperiorID = r["SuperiorID"].ToString();
                    listverModul.FirstTitleID = r["FirstTitleID"].ToString();
                    listverModul.FirstTitle = r["FirstTitle"].ToString();
                    listverModul.SecondTitleID = r["SecondTitleID"].ToString();
                    listverModul.SecondTitle = r["SecondTitle"].ToString();
                    listverModul.ModuleAddress = r["ModuleAddress"].ToString();
                    listverModul.MD5 = r["MD5"].ToString();
                    listverModul.IncrementalPacketAddress = r["IncrementalPacketAddress"].ToString();
                    listverModul.IncrementalPacketMD5 = r["IncrementalPacketMD5"].ToString();
                    listverModul.ModuleVersion = r["ModuleVersion"].ToString();
                    listverModul.VersionSort = r["Sort"].ToString();
                }

                if (listverModul != null)
                {
                    return listverModul;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 合并返回模块及下载地址
        /// </summary>
        /// <param name="modularlist"></param>
        /// <param name="newversionlist"></param>
        /// <returns></returns>
        public List<VersionChangeModuleSort> GetModuleSortVersionChange(List<TB_ModuleSort> modularlist, List<TB_VersionChange> newversionlist)
        {
            List<TB_VersionChange> newversionlistsorts = null;
            if (modularlist != null)
            {
                if (newversionlist != null)
                {
                    List<TB_VersionChange> newversionlistsort = (from c in newversionlist
                                                                 orderby c.ID descending //ascending  //倒序
                                                                 select c).ToList();
                    //根据条件去重（选择版本号最大的）
                    newversionlistsorts = newversionlistsort.GroupBy(x => new { /*在这里放入你视为重复的列，用逗号分割，比如*/ x.ModuleID, x.BooKID, x.FirstTitleID, x.SecondTitleID }).Select(x => x.First()).ToList();
                }
                else
                {
                    newversionlistsorts = null;
                }
                List<VersionChangeModuleSort> listvermodule = new List<VersionChangeModuleSort>();
                foreach (TB_ModuleSort module in modularlist)
                {
                    VersionChangeModuleSort verchangemodule = new VersionChangeModuleSort();
                    verchangemodule.BookID = module.BookID.ToString();
                    verchangemodule.ModuleID = module.ModuleID.ToString();
                    verchangemodule.ModuleName = module.ModuleName;
                    verchangemodule.ModuleSort = module.Sort.ToString();
                    verchangemodule.SuperiorID = module.SuperiorID.ToString();
                    verchangemodule.FirstTitleID = module.FirstTitleID.ToString();
                    verchangemodule.FirstTitle = "";
                    verchangemodule.SecondTitleID = module.SecondTitleID.ToString();
                    verchangemodule.SecondTitle = "";
                    verchangemodule.ModuleAddress = "";
                    verchangemodule.MD5 = "";
                    verchangemodule.IncrementalPacketAddress = "";
                    verchangemodule.IncrementalPacketMD5 = "";
                    verchangemodule.ModuleVersion = "";
                    verchangemodule.VersionSort = "";

                    bool sucess = false;//标记循环中是否已经添加那条信息
                    if (newversionlistsorts != null)
                    {
                        foreach (TB_VersionChange version in newversionlistsorts)
                        {
                            if ((module.ModuleID.ToString() == version.ModuleID.ToString()) && (module.BookID.ToString() == version.BooKID.ToString()) && module.FirstTitleID == version.FirstTitleID && module.SecondTitleID == version.SecondTitleID)
                            {
                                verchangemodule.FirstTitle = version.FirstTitle;
                                verchangemodule.SecondTitle = version.SecondTitle;
                                verchangemodule.ModuleAddress = version.ModuleAddress;
                                verchangemodule.MD5 = version.MD5;
                                verchangemodule.IncrementalPacketAddress = version.IncrementalPacketAddress;
                                verchangemodule.IncrementalPacketMD5 = version.IncrementalPacketMD5;
                                verchangemodule.ModuleVersion = version.ModuleVersion;
                                verchangemodule.VersionSort = module.Sort.ToString();
                                listvermodule.Add(verchangemodule);
                                sucess = true;
                            }
                        }
                    }
                    if (!sucess) //意思已经在循环中添加过了
                    {
                        listvermodule.Add(verchangemodule);
                    }
                }

                // ///2016/8/8 新加
                // ///

                //// 把二级的数据清除 
                //  List<VersionChangeModuleSort> listvermodules =  listvermodule.Where((x, i) => listvermodule.FindIndex(z => z.ModuleID == x.SuperiorID) == i).ToList();
                //  List<object> listobject = new List<object>();
                //  //把重复的整合
                //  foreach (VersionChangeModuleSort verchangeSorts in listvermodules) 
                //  {
                //      List<object> objlist = new List<object>();
                //      foreach (VersionChangeModuleSort verchangeSort in listvermodule)
                //      {  
                //          if (verchangeSorts.ModuleID == verchangeSort.SuperiorID)
                //          {
                //              if (!objlist.Contains(verchangeSorts))
                //              {
                //                  objlist.Add(verchangeSorts);
                //              }
                //          }
                //      }
                //      object objs = new
                //      {
                //         BookID =verchangeSorts.BookID,
                //         ModuleID =verchangeSorts.ModuleID,
                //         ModuleName =verchangeSorts.ModuleName,
                //         ModuleSort =verchangeSorts.ModuleSort,
                //         SuperiorID =verchangeSorts.SuperiorID,
                //         FirstTitleID=verchangeSorts.FirstTitleID,
                //         FirstTitle =verchangeSorts.FirstTitle,
                //         SecondTitleID =verchangeSorts.SecondTitleID,
                //         SecondTitle=verchangeSorts.SecondTitle,
                //         ModuleAddress =verchangeSorts.ModuleAddress,
                //         MD5=verchangeSorts.MD5,
                //         IncrementalPacketAddress=verchangeSorts.IncrementalPacketAddress,
                //         IncrementalPacketMD5 =verchangeSorts.IncrementalPacketMD5,
                //         ModuleVersion =verchangeSorts.ModuleVersion,
                //         VersionSort = verchangeSorts.VersionSort,
                //         listVersionChangeModuleSort=objlist
                //      };
                //     if (!listobject.Contains(objs)) 
                //     {
                //         listobject.Add(objs);
                //     }           
                //  }

                return listvermodule;
            }
            else
            {
                return null;
            }
        }


        public List<object> GetModuleSortVersionChanges(List<TB_ModuleSort> modularlist, List<TB_VersionChange> newversionlist)
        {
            List<TB_VersionChange> newversionlistsorts = null;
            if (modularlist != null)
            {
                if (newversionlist != null)
                {
                    List<TB_VersionChange> newversionlistsort = (from c in newversionlist
                                                                 orderby c.ID descending //ascending  //倒序
                                                                 select c).ToList();
                    //根据条件去重（选择版本号最大的）
                    newversionlistsorts = newversionlistsort.GroupBy(x => new { /*在这里放入你视为重复的列，用逗号分割，比如*/ x.ModuleID, x.BooKID, x.FirstTitleID, x.SecondTitleID }).Select(x => x.First()).ToList();
                }
                else
                {
                    newversionlistsorts = null;
                }


                List<VersionChangeModuleSort> listvermodule = new List<VersionChangeModuleSort>();
                foreach (TB_ModuleSort module in modularlist)
                {
                    VersionChangeModuleSort verchangemodule = new VersionChangeModuleSort();
                    verchangemodule.BookID = module.BookID.ToString();
                    verchangemodule.ModuleID = module.ModuleID.ToString();
                    verchangemodule.ModuleName = module.ModuleName;
                    verchangemodule.ModuleSort = module.Sort.ToString();
                    verchangemodule.SuperiorID = module.SuperiorID.ToString();
                    verchangemodule.FirstTitleID = module.FirstTitleID.ToString();
                    verchangemodule.FirstTitle = "";
                    verchangemodule.SecondTitleID = module.SecondTitleID.ToString();
                    verchangemodule.SecondTitle = "";
                    verchangemodule.ModuleAddress = "";
                    verchangemodule.MD5 = "";
                    verchangemodule.IncrementalPacketAddress = "";
                    verchangemodule.IncrementalPacketMD5 = "";
                    verchangemodule.ModuleVersion = "";
                    verchangemodule.VersionSort = "";

                    bool sucess = false;//标记循环中是否已经添加那条信息
                    if (newversionlistsorts != null)
                    {
                        foreach (TB_VersionChange version in newversionlistsorts)
                        {

                            string moduleSecondID = module.SecondTitleID.ToString();
                            string versionSecondID = version.SecondTitleID.ToString();

                            //if (moduleSecondID == "") 
                            //{
                            //   moduleSecondID = "0";
                            //}
                            //if (versionSecondID == "")
                            //{
                            //    versionSecondID = "0";
                            //}
                            if ((module.ModuleID.ToString() == version.ModuleID.ToString()) && (module.BookID.ToString() == version.BooKID.ToString()) && module.FirstTitleID == version.FirstTitleID && moduleSecondID == versionSecondID)
                            {
                                verchangemodule.FirstTitle = version.FirstTitle;
                                verchangemodule.SecondTitle = version.SecondTitle;
                                verchangemodule.ModuleAddress = version.ModuleAddress;
                                verchangemodule.MD5 = version.MD5;
                                verchangemodule.IncrementalPacketAddress = version.IncrementalPacketAddress;
                                verchangemodule.IncrementalPacketMD5 = version.IncrementalPacketMD5;
                                verchangemodule.ModuleVersion = version.ModuleVersion;
                                verchangemodule.VersionSort = module.Sort.ToString();
                                listvermodule.Add(verchangemodule);
                                sucess = true;
                            }
                        }
                    }
                    if (!sucess) //意思已经在循环中添加过了
                    {
                        listvermodule.Add(verchangemodule);
                    }
                }

                ///2016/8/8 新加
                // 把二级的数据清除 

                List<object> listobject = new List<object>();
                //把重复的整合

                List<VersionChangeModuleSort> listvew = new List<VersionChangeModuleSort>();
                //获取子模块重复信息
                foreach (VersionChangeModuleSort verchangeSorts in listvermodule)
                {
                    foreach (VersionChangeModuleSort verchangeSort in listvermodule)
                    {

                        if (verchangeSorts.ModuleID == verchangeSort.SuperiorID) //此为一级标题
                        {
                            listvew.Add(verchangeSort);
                        }
                    }
                }

                for (int i = listvermodule.Count; i >= 0; i--)
                {
                    foreach (VersionChangeModuleSort verchangeSort in listvew)
                    {

                        if (listvermodule.Contains(verchangeSort))
                        {
                            listvermodule.Remove(verchangeSort);
                        }
                    }
                }

                string s = "1";
                string sql = string.Format(@"SELECT [State] FROM [TB_VersionChange] WHERE BooKID IS NULL ");
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["State"].ToString() == "False")
                    {
                        s = "0";
                    }
                }
                listvermodule = listvermodule.OrderBy(i => i.ModuleSort).ToList();
                foreach (VersionChangeModuleSort listmode in listvermodule)
                {
                    List<object> objlist = new List<object>();
                    foreach (VersionChangeModuleSort verchangeSort in listvew)
                    {
                        if (listmode.ModuleID == verchangeSort.SuperiorID)
                        {
                            objlist.Add(verchangeSort);
                        }
                    }

                    object objs = new
                    {
                        BookID = listmode.BookID,
                        ModuleID = listmode.ModuleID,
                        ModuleName = listmode.ModuleName,
                        ModuleSort = listmode.ModuleSort,
                        SuperiorID = listmode.SuperiorID,
                        FirstTitleID = listmode.FirstTitleID,
                        FirstTitle = listmode.FirstTitle,
                        SecondTitleID = listmode.SecondTitleID,
                        SecondTitle = listmode.SecondTitle,
                        ModuleAddress = listmode.ModuleAddress,
                        MD5 = listmode.MD5,
                        IncrementalPacketAddress = listmode.IncrementalPacketAddress,
                        IncrementalPacketMD5 = listmode.IncrementalPacketMD5,
                        ModuleVersion = listmode.ModuleVersion,
                        VersionSort = listmode.VersionSort,
                        ActiveState = listmode.ActiveState,
                        IsActive = s,
                        listVersionChangeModuleSort = objlist
                    };
                    if (!listobject.Contains(objs))
                    {
                        listobject.Add(objs);
                    }
                }

                return listobject;
            }
            else
            {
                return null;
            }
        }

        public List<object> GetRJTBXVersionChanges(List<tbmodulesort> modularlist, List<versionchange> newversionlist)
        {
            List<versionchange> newversionlistsorts = null;
            if (modularlist != null)
            {
                if (newversionlist != null)
                {
                    List<versionchange> newversionlistsort = (from c in newversionlist
                                                              orderby c.ID descending //ascending  //倒序
                                                              select c).ToList();
                    //根据条件去重（选择版本号最大的）
                    newversionlistsorts = newversionlistsort.GroupBy(x => new { /*在这里放入你视为重复的列，用逗号分割，比如*/ x.ModuleID, x.BooKID, x.FirstTitleID, x.SecondTitleID }).Select(x => x.First()).ToList();
                }
                else
                {
                    newversionlistsorts = null;
                }


                List<VersionChangeModuleSort> listvermodule = new List<VersionChangeModuleSort>();
                foreach (tbmodulesort module in modularlist)
                {
                    VersionChangeModuleSort verchangemodule = new VersionChangeModuleSort();
                    verchangemodule.BookID = module.BookID.ToString();
                    verchangemodule.ModuleID = module.ModuleID.ToString();
                    verchangemodule.ModuleName = module.ModuleName;
                    verchangemodule.ModuleSort = module.Sort.ToString();
                    verchangemodule.SuperiorID = module.SuperiorID.ToString();
                    verchangemodule.FirstTitleID = module.FirstTitleID.ToString();
                    verchangemodule.FirstTitle = "";
                    verchangemodule.SecondTitleID = module.SecondTitleID.ToString();
                    verchangemodule.SecondTitle = "";
                    verchangemodule.ModuleAddress = "";
                    verchangemodule.MD5 = "";
                    verchangemodule.IncrementalPacketAddress = "";
                    verchangemodule.IncrementalPacketMD5 = "";
                    verchangemodule.ModuleVersion = "";
                    verchangemodule.VersionSort = "";

                    bool sucess = false;//标记循环中是否已经添加那条信息
                    if (newversionlistsorts != null)
                    {
                        foreach (versionchange version in newversionlistsorts)
                        {

                            string moduleSecondID = module.SecondTitleID.ToString();
                            string versionSecondID = version.SecondTitleID.ToString();

                            //if (moduleSecondID == "") 
                            //{
                            //   moduleSecondID = "0";
                            //}
                            //if (versionSecondID == "")
                            //{
                            //    versionSecondID = "0";
                            //}
                            if ((module.ModuleID.ToString() == version.ModuleID.ToString()) && (module.BookID.ToString() == version.BooKID.ToString()) && module.FirstTitleID == version.FirstTitleID && moduleSecondID == versionSecondID)
                            {
                                verchangemodule.FirstTitle = version.FirstTitle;
                                verchangemodule.SecondTitle = version.SecondTitle;
                                verchangemodule.ModuleAddress = version.ModuleAddress;
                                verchangemodule.MD5 = version.MD5;
                                verchangemodule.IncrementalPacketAddress = version.IncrementalPacketAddress;
                                verchangemodule.IncrementalPacketMD5 = version.IncrementalPacketMD5;
                                verchangemodule.ModuleVersion = version.ModuleVersion;
                                verchangemodule.VersionSort = module.Sort.ToString();
                                listvermodule.Add(verchangemodule);
                                sucess = true;
                            }
                        }
                    }
                    if (!sucess) //意思已经在循环中添加过了
                    {
                        listvermodule.Add(verchangemodule);
                    }
                }

                ///2016/8/8 新加
                // 把二级的数据清除 

                List<object> listobject = new List<object>();
                //把重复的整合

                List<VersionChangeModuleSort> listvew = new List<VersionChangeModuleSort>();
                //获取子模块重复信息
                foreach (VersionChangeModuleSort verchangeSorts in listvermodule)
                {
                    foreach (VersionChangeModuleSort verchangeSort in listvermodule)
                    {

                        if (verchangeSorts.ModuleID == verchangeSort.SuperiorID) //此为一级标题
                        {
                            listvew.Add(verchangeSort);
                        }
                    }
                }

                for (int i = listvermodule.Count; i >= 0; i--)
                {
                    foreach (VersionChangeModuleSort verchangeSort in listvew)
                    {

                        if (listvermodule.Contains(verchangeSort))
                        {
                            listvermodule.Remove(verchangeSort);
                        }
                    }
                }

                string s = "1";
                string sql = string.Format(@"SELECT [State] FROM [TB_VersionChange] WHERE BooKID IS NULL ");
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["State"].ToString() == "False")
                    {
                        s = "0";
                    }
                }

                foreach (VersionChangeModuleSort listmode in listvermodule)
                {
                    List<object> objlist = new List<object>();
                    foreach (VersionChangeModuleSort verchangeSort in listvew)
                    {
                        if (listmode.ModuleID == verchangeSort.SuperiorID)
                        {
                            objlist.Add(verchangeSort);
                        }
                    }

                    object objs = new
                    {
                        BookID = listmode.BookID,
                        ModuleID = listmode.ModuleID,
                        ModuleName = listmode.ModuleName,
                        ModuleSort = listmode.ModuleSort,
                        SuperiorID = listmode.SuperiorID,
                        FirstTitleID = listmode.FirstTitleID,
                        FirstTitle = listmode.FirstTitle,
                        SecondTitleID = listmode.SecondTitleID,
                        SecondTitle = listmode.SecondTitle,
                        ModuleAddress = listmode.ModuleAddress,
                        MD5 = listmode.MD5,
                        IncrementalPacketAddress = listmode.IncrementalPacketAddress,
                        IncrementalPacketMD5 = listmode.IncrementalPacketMD5,
                        ModuleVersion = listmode.ModuleVersion,
                        VersionSort = listmode.VersionSort,
                        ActiveState = listmode.ActiveState,
                        IsActive = s,
                        listVersionChangeModuleSort = objlist
                    };
                    if (!listobject.Contains(objs))
                    {
                        listobject.Add(objs);
                    }
                }

                return listobject;
            }
            else
            {
                return null;
            }
        }


        public int StringToInt(string inputStr)
        {
            int id = 0;
            if (!string.IsNullOrEmpty(inputStr))
            {
                id = int.Parse(inputStr);
            }
            return id;
        }

    }
    public class VersionChangeModuleSort
    {
        public string BookID { set; get; }
        public string ModuleID { set; get; }
        public string ModuleName { set; get; }
        public string ModuleSort { set; get; }
        public string SuperiorID { set; get; }
        public string FirstTitleID { set; get; }
        public string FirstTitle { set; get; }
        public string SecondTitleID { set; get; }
        public string SecondTitle { set; get; }
        public string ModuleAddress { set; get; }
        public string MD5 { set; get; }
        public string IncrementalPacketAddress { set; get; }
        public string IncrementalPacketMD5 { set; get; }
        public string ModuleVersion { set; get; }
        public string VersionSort { set; get; }
        public int ActiveState { set; get; }
    }

    public class APPManagement
    {
        public string AppID;
    }
}