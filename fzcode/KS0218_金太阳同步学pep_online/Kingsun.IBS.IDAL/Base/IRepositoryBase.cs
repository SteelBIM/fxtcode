using Kingsun.SynchronousStudy.Common.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IDAL
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        SqlSugarClient GetInstance();

        SqlSugarClient GetInstance(string connectionString);

        #region Insert
        object Insert(TEntity entity);

        TEntity InsertReturnEntity(TEntity info, string[] array = null);

        int InsertRange(IEnumerable<TEntity> entities);

        int InsertRange(IEnumerable<TEntity> entities,bool isIdentity);

        IEnumerable<TEntity> InsertBatch(IEnumerable<TEntity> entities);
        #endregion

        #region Update
        bool Update(TEntity entity);

        bool Update(TEntity entity, Expression<Func<TEntity, bool>> includes = null);

        bool UpdateWithNull(TEntity entity, Expression<Func<TEntity, bool>> includes = null);

        bool UpdateIgnoreColumns(TEntity entity, Expression<Func<TEntity, object>> includes = null);

        bool Update(TEntity entity, string[] disableUpdateCoulums);

        bool UpdateColumns(TEntity entity, Expression<Func<TEntity, object>> includes = null);

        bool MyUpdate(TEntity entity, string tableKey, string[] notUpdateColum = null);

        bool UpdateAssign(TEntity entity, Expression<Func<TEntity, bool>> includes = null);
        #endregion

        #region DELETE
        bool Delete(object id);

        bool Delete(Expression<Func<TEntity, bool>> expr);

        bool DeleteMore(string Ids);

        void DeleteBatch(IEnumerable<TEntity> entities);
        #endregion

        #region Query
        TEntity GetByID(object id);
        IEnumerable<TEntity> ListAll();

        IEnumerable<TEntity> SqlQuery(string sql, IEnumerable<SqlParameter> pars = null);
        IEnumerable<TEntity> SelectIn(Expression<Func<TEntity, bool>> expr, string field, IEnumerable<string> Ins);
        IEnumerable<TEntity> SelectIn(string filedName, IEnumerable<string> filedlist);

        IEnumerable<TEntity> SelectSearch(Expression<Func<TEntity, bool>> expression, int topNumber, string orderby = "");

        IEnumerable<TEntity> SelectAppointField(Expression<Func<TEntity, bool>> expression, string Field);

        IEnumerable<TEntity> SelectSearch(string whereSql);

        IEnumerable<TEntity> SelectSearchs(IEnumerable<Expression<Func<TEntity, bool>>> expressions, string Flids = "", IEnumerable<string> InIds = null, string orderfile = "");

        IEnumerable<TEntity> SelectGroupBy(Expression<Func<TEntity, bool>> expression, string groupbyfields);
        IEnumerable<TEntity> SelectGroupBy(Expression<Func<TEntity, bool>> expression, string groupbyfields, string Flide, List<string> Ids);

        IEnumerable<TEntity> SelectGroupBy(string groupbyfield);

        IEnumerable<TEntity> SelectSearch(Expression<Func<TEntity, bool>> expression);

        IEnumerable<TEntity> SelectSearch(IEnumerable<Expression<Func<TEntity, bool>>> expression);

        IEnumerable<TEntity> SelectSearch(string whereSql, IEnumerable<Expression<Func<TEntity, bool>>> expression);

        int GetTotalCount(Expression<Func<TEntity, bool>> expression, string sqlwhere = "");
        int GetTotalCount(string sqlwhere);
        int GetTotalCount(IEnumerable<string> Ids, string Flide);

        int GetTotalCount(Expression<Func<TEntity, bool>> expression, IEnumerable<string> Ids, string Flide);


        dynamic SelectDynamic(string sqlstr, object param = null);

        string SelectString(string sqlstr, object param = null);

        TEntity SelectStringBySql(string sqlstr, object param = null);

        DataTable SelectDataTable(string sql, object obj = null);
        #endregion


        IEnumerable<TEntity> SelectSearch<T2>(Expression<Func<TEntity, T2, bool>> whereExpression, Expression<Func<TEntity, T2, bool>> joinOn);


        IEnumerable<TEntity> ExecuteProcedure(string procName, List<SqlParameter> pars = null);



    }
}
