using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
   public interface IWaitBuildingProject
    {
       /// <summary>
       /// 根据ID获取待建楼盘
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       IQueryable<Dat_WaitProject> GetWaitProjectById(int id);

       /// <summary>
        /// 获取待建楼盘
       /// </summary>
       /// <returns></returns>
       IQueryable<Dat_WaitProject> GetWaitProject(string name, int cityid, int fxtcompanyid);

       /// <summary>
       /// 新增待建楼盘
       /// </summary>
       /// <param name="wp"></param>
       /// <returns></returns>
       int AddWaitProject(Dat_WaitProject wp);

       /// <summary>
       /// 更新待建楼盘
       /// </summary>
       /// <param name="name"></param>
       /// <returns></returns>
       int UpdateWaitProject(int id, string name);

       /// <summary>
       /// 删除待建楼盘
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       int DeleteWaitProject(int id);

       /// <summary>
       /// 获取单个待建楼盘对象
       /// </summary>
       /// <param name="name"></param>
       /// <param name="cityid"></param>
       /// <param name="fxtcompanyid"></param>
       /// <returns></returns>
       Dat_WaitProject GetSingleWaitProject(string name, int cityid, int fxtcompanyid);
    }
}
