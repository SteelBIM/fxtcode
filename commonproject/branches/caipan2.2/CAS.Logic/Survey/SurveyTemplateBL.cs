using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity;
using CAS.DataAccess;
using CAS.Entity.DBEntity;
using CAS.DataAccess.DA.Survey;
using CAS.Common;

namespace CAS.Logic.Survey
{
    public class SurveyTemplateBL
    {
        public static int Add(SurveyTemplate model)
        {
            return SurveyTemplateDA.Add(model);
        }
        public static int Update(SurveyTemplate model)
        {
            return SurveyTemplateDA.Update(model);
        }
        public static int Delete(int id)
        {
            return SurveyTemplateDA.Delete(id);
        }
        public static SurveyTemplate GetSurveyTemplateByPK(int id)
        {
            return SurveyTemplateDA.GetSurveyTemplateByPK(id);
        }
        public static List<SurveyTemplate> GetSurveyTemplateList(SearchBase search)
        {
            return SurveyTemplateDA.GetSurveyTemplateList(search);
        }
    }
}