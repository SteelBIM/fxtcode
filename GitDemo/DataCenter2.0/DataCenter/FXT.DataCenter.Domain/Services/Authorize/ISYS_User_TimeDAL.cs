using FXT.DataCenter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Services
{

    /// <summary>
    /// 为SYS_User_Time表提供增删改查操作
    /// </summary>
    public interface ISYS_User_TimeDAL
    {
        /// <summary>
        /// 根据用户名查找，未找到返回null
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>未找到返回null</returns>
        SYS_User_Time Find(string userName);

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>成功返回1，失败返回0</returns>
        int Insert(SYS_User_Time entity);

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回更新的记录数</returns>
        int Update(SYS_User_Time entity);

        /// <summary>
        /// 删除指定用户记录
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>返回删除的记录数</returns>
        int Delete(string userName);

    }
}
