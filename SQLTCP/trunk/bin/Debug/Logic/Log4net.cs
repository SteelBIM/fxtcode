using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.DataAccess;

namespace CAS.Logic
{
	public class Log4netBL
	{
		public static int Add(Log4net model)
		{
			return Log4netDA.Add(model);
		}
		public static int Update(Log4net model)
		{
			return Log4netDA.Update(model);
		}
		//批量更新
		public static int UpdateMul(Log4net model,int[] ids)
		{
			return Log4netDA.UpdateMul(model,ids);
		}
		public static int Delete(int id)
		{
			return Log4netDA.Delete(id);
		}
		public static int DeleteOnLogical(int id)
		{
			Log4net model = new Log4net();
			model.int = id;
			model.valid = 0;
			model.SetAvailableFields(new string[] { "valid" });
			return Log4netDA.Update(model);
		}
		public static Log4net GetLog4netByPK(int id)
		{
			return Log4netDA.GetLog4netByPK(id); 
		}
		public static List<Log4net> GetLog4netList(SearchBase search, string key)
		{
			return Log4netDA.GetLog4netList(search, key); 
		}
	}
}