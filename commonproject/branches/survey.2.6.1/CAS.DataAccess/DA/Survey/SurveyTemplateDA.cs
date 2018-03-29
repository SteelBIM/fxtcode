using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Common;
using CAS.Entity.DBEntity;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity;

namespace CAS.DataAccess.DA.Survey
{
    /// <summary>
    /// 查勘模板DA
    /// </summary>
    public class SurveyTemplateDA : Base
    {
        public static int Add(SurveyTemplate model)
        {
            return InsertFromEntity<SurveyTemplate>(model);
        }
        public static int Update(SurveyTemplate model)
        {
            return UpdateFromEntity<SurveyTemplate>(model);
        }
        public static int Delete(int id)
        {
            return DeleteByPrimaryKey<SurveyTemplate>(id);
        }
        public static SurveyTemplate GetSurveyTemplateByPK(int id)
        {
            SetEntityTable<SurveyTemplate>(TableByCity.SYS_SurveyTemplate);
            return ExecuteToEntityByPrimaryKey<SurveyTemplate>(id);
        }
        public static List<SurveyTemplate> GetSurveyTemplateList(SearchBase search)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
            if (search.SurveyTypeCode > 0) {
                search.Where += " and SurveyTypeCode=@SurveyTypeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@SurveyTypeCode", search.SurveyTypeCode, SqlDbType.Int));
            }
            string sql = "select * from " + TableByCity.SYS_SurveyTemplate + " with(nolock) where 1=1 ";
			sql = HandleSQL(search, sql);
			return ExecuteToEntityList<SurveyTemplate>(sql, System.Data.CommandType.Text, parameters);
		}
    }
}
