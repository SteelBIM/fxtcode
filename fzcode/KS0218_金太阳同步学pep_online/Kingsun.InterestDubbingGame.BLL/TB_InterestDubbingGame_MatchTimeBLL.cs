using Kingsun.InterestDubbingGame.DAL;
using Kingsun.InterestDubbingGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.BLL
{
    public class TB_InterestDubbingGame_MatchTimeBLL
    {
        TB_InterestDubbingGame_MatchTimeDAL dal = new TB_InterestDubbingGame_MatchTimeDAL();
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        public IList<TB_InterestDubbingGame_MatchTime> GetList()
        {
            IList<TB_InterestDubbingGame_MatchTime> list = dal.QueryList();
            return list;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public IList<TB_InterestDubbingGame_MatchTime> QueryList()
        {
            return dal.QueryList();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertData(TB_InterestDubbingGame_MatchTime model)
        {
            return dal.InsertData(model);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateData(TB_InterestDubbingGame_MatchTime model)
        {
            return dal.UpdateData(model);
        }
        /// <summary>
        /// 根据id找到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TB_InterestDubbingGame_MatchTime GetModel(int id)
        {
            return dal.GetModel(id);
        }
    }
}
