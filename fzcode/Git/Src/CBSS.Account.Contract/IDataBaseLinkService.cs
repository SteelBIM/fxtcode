﻿
using CBSS.Main.Contract.ViewModel.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Main.Contract
{
    public interface IDataBaseLinkService
    {
        #region 获取数据
        /// <summary>
        /// 库连接列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<DataBaseLinkEntity> GetList();
        /// <summary>
        /// 库连接实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DataBaseLinkEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除库连接
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存库连接表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="databaseLinkEntity">库连接实体</param>
        /// <returns></returns>
        void SaveForm(string keyValue, DataBaseLinkEntity databaseLinkEntity);
        #endregion
    }
}