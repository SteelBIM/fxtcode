using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Common;
using CAS.Entity.DBEntity;
using CAS.DataAccess.BaseDAModels;

namespace CAS.DataAccess
{
	public class ArticleDA : Base 
	{
		public static int Add(Article model)
		{
			return InsertFromEntity<Article>(model);
		}
		public static int Update(Article model)
		{
			return UpdateFromEntity<Article>(model);
		}
		//批量更新
		public static int UpdateMul(Article model,int[] ids)
		{
			return UpdateFromIds<Article>(model,ids);
		}
		public static int Delete(int id)
		{
			return DeleteByPrimaryKey<Article>(id);
		}
		public static Article GetArticleByPK(int id)
		{
			return ExecuteToEntityByPrimaryKey<Article>(id);
		}
		public static List<Article> GetArticleList(SearchBase search, string key)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			if (!string.IsNullOrEmpty(key))
			{
				search.Where += " and <search field> like @key escape '$'";
				parameters.Add(SqlHelper.GetSqlParameter("@key", "%" + SQLFilterHelper.EscapeLikeString(key, "$") + "%", SqlDbType.NVarChar, <search field length>));
			}
			string sql = SQL.Article.ArticleList;
			sql = HandleSQL(search, sql);
			return ExecuteToEntityList<Article>(sql, System.Data.CommandType.Text, parameters);
		}
	}
}
