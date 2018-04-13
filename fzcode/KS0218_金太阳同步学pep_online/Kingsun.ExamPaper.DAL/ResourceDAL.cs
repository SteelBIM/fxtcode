using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;
using System.Collections.Generic;

namespace Kingsun.ExamPaper.DAL
{
    public class ResourceDAL : BaseManagement
    {
        public QTb_Resource GetResource(int resid)
        {
            return Select<QTb_Resource>(resid);
        }

        public QTb_Resource GetResource(int mod_ed, int gradeid, int bookreel)
        {
            return SelectByCondition<QTb_Resource>("EditionID=(select EditionID from QTb_Edition where MOD_ED=" + mod_ed.ToString() + " and ParentID>0) and GradeID=" + gradeid.ToString() + " and BookReel=" + bookreel.ToString());
        }
        public IList<V_Resource> GetResourcePageList(int pageIndex, int pageSize, string strWhere, string orderColumn, int orderType, out int totalCount, out int totalPages)
        {
            return GetPageList<V_Resource>("V_Resource", pageIndex, pageSize, strWhere, orderColumn, orderType, out totalCount, out totalPages);
        }
        public bool UpdateResource(QTb_Resource res)
        {
            return Update<QTb_Resource>(res);
        }

        public bool InsertResource(QTb_Resource res)
        {
            string sql = string.Format("insert into QTb_Resource(ResUrl,ResMD5,ResVersion,EditionID,GradeID,BookReel,ResUpTimes) values('{0}','{1}','{2}',{3},{4},{5},{6})",
                res.ResUrl, res.ResMD5, res.ResVersion, res.EditionID.Value, res.GradeID.Value, res.BookReel, res.ResUpTimes);
            return ExcuteSqlWithTran(sql);
        }
    }
}
