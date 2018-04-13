using Kingsun.InterestDubbingGame.Common;
using Kingsun.InterestDubbingGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.DAL
{
    public class TB_InterestDubbingGame_MatchTimeDAL : InterestDubbingBaseManagement
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public IList<TB_InterestDubbingGame_MatchTime> QueryList()
        {
            IList<TB_InterestDubbingGame_MatchTime> list = Search<TB_InterestDubbingGame_MatchTime>(" 1=1 ORDER BY CreateTime DESC");
            return list;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertData(TB_InterestDubbingGame_MatchTime model)
        {
            return Insert(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateData(TB_InterestDubbingGame_MatchTime model)
        {
            return Update(model);
        }

        /// <summary>
        /// 根据id找到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TB_InterestDubbingGame_MatchTime GetModel(int id)
        {
            return SelectByCondition<TB_InterestDubbingGame_MatchTime>(string.Format("ID={0}", id));
        }
    }
}
