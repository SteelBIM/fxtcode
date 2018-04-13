using Kingsun.InterestDubbingGame.DAL;
using Kingsun.InterestDubbingGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.BLL
{
    public class TB_InterestDubbingGame_PushMsgBLL
    {
        TB_InterestDubbingGame_PushMsgDAL dal = new TB_InterestDubbingGame_PushMsgDAL();
        /// <summary>
        /// 根据条件获TB_InterestDubbingGame_PushMsg
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<TB_InterestDubbingGame_PushMsg> GetList(string strWhere, string orderby = "")
        {
            return dal.GetList(strWhere, orderby);
        }
        /// <summary>
        /// 根据ID修改状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public bool UpdateState(string ID, string State)
        {
            return dal.UpdateState(ID, State);
        }
        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public bool Del(string ID)
        {
            return dal.Del(ID);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(TB_InterestDubbingGame_PushMsg model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(TB_InterestDubbingGame_PushMsg model)
        {
            return dal.Update(model);
        }
    }
}
