using Kingsun.FZ_Temporary.DAL;
using Kingsun.FZ_Temporary.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.FZ_Temporary.BLL
{
    public class CollectUserInfoBLL
    {
        CollectUserInfoDAL dal = new CollectUserInfoDAL();
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCollectUserInfo(CollectUserInfoModel model)
        {
            return dal.AddCollectUserInfo(model);
        }
        /// <summary>
        /// 根据电话号码判断是否存在
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public bool CheckCollectUserInfo(string Phone)
        {
            return dal.CheckCollectUserInfo(Phone);
        }
        /// <summary>
        /// 添加验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddPhoneCode(Tb_PhoneCode model)
        {
            return dal.AddPhoneCode(model);
        }
        /// <summary>
        /// 根据Telephone获取Tb_PhoneCode信息
        /// </summary>
        /// <param name="Telephone"></param>
        /// <returns></returns>
        public DataSet GetPhoneCode(string Telephone)
        {
            return dal.GetPhoneCode(Telephone);
        }
    }
}
