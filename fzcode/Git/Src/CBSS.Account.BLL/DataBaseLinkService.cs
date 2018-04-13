using CBSS.Framework.DAL;
using CBSS.Main.Contract;
using CBSS.Main.Contract.ViewModel.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Main.BLL
{
    public class DataBaseLinkService : IDataBaseLinkService
    {
        #region 获取数据

        Repository repository = new Repository();
        /// <summary>
        /// 库连接列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataBaseLinkEntity> GetList()
        {
            return repository.ListAll<DataBaseLinkEntity>().OrderBy(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 库连接实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataBaseLinkEntity GetEntity(string keyValue)
        {
            return repository.GetByID<DataBaseLinkEntity>(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除库连接
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            repository.Delete<DataBaseLinkEntity>(keyValue);
        }
        /// <summary>
        /// 保存库连接表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="databaseLinkEntity">库连接实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DataBaseLinkEntity databaseLinkEntity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                databaseLinkEntity.Modify(keyValue);
                repository.Update(databaseLinkEntity);
            }
            else
            {
                databaseLinkEntity.Create();
                repository.Insert(databaseLinkEntity);
            }
        }
        #endregion
    }
}
