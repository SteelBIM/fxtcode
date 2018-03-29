using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.DataAccess;

namespace CAS.Logic
{
	public class DATHouseBL
	{
		public static int Add(DATHouse model)
		{
			return DATHouseDA.Add(model);
		}
		public static int Update(DATHouse model)
		{
			return DATHouseDA.Update(model);
		}
		//批量更新
		public static int UpdateMul(DATHouse model,int[] ids)
		{
			return DATHouseDA.UpdateMul(model,ids);
		}
		public static int Delete(int id)
		{
			return DATHouseDA.Delete(id);
		}
		public static int DeleteOnLogical(int id)
		{
			DATHouse model = new DATHouse();
			model.int = id;
			model.valid = 0;
			model.SetAvailableFields(new string[] { "valid" });
			return DATHouseDA.Update(model);
		}
		public static DATHouse GetDATHouseByPK(int id)
		{
			return DATHouseDA.GetDATHouseByPK(id); 
		}
		public static List<DATHouse> GetDATHouseList(SearchBase search, string key)
		{
			return DATHouseDA.GetDATHouseList(search, key); 
		}
	}
}