using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.DataAccess;

namespace CAS.Logic
{
	public class ArticleBL
	{
		public static int Add(Article model)
		{
			return ArticleDA.Add(model);
		}
		public static int Update(Article model)
		{
			return ArticleDA.Update(model);
		}
		//批量更新
		public static int UpdateMul(Article model,int[] ids)
		{
			return ArticleDA.UpdateMul(model,ids);
		}
		public static int Delete(int id)
		{
			return ArticleDA.Delete(id);
		}
		public static int DeleteOnLogical(int id)
		{
			Article model = new Article();
			model.int = id;
			model.valid = 0;
			model.SetAvailableFields(new string[] { "valid" });
			return ArticleDA.Update(model);
		}
		public static Article GetArticleByPK(int id)
		{
			return ArticleDA.GetArticleByPK(id); 
		}
		public static List<Article> GetArticleList(SearchBase search, string key)
		{
			return ArticleDA.GetArticleList(search, key); 
		}
	}
}