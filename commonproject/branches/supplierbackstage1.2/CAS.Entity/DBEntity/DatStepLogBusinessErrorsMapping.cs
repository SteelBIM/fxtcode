using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_StepLog_BusinessErrorsMapping")]
	public class DatStepLogBusinessErrorsMapping : BaseTO
	{
		private int _mappingid;
		[SQLField("mappingid", EnumDBFieldUsage.PrimaryKey, true)]
		public int mappingid
		{
			get{ return _mappingid;}
			set{ _mappingid=value;}
		}
		private int _fk_stepchecklogid;
		public int fk_stepchecklogid
		{
			get{ return _fk_stepchecklogid;}
			set{ _fk_stepchecklogid=value;}
		}
		private int _fk_typesmappingid;
        public int fk_typesmappingid
		{
			get{ return _fk_typesmappingid;}
			set{ _fk_typesmappingid=value;}
		}
	}
}