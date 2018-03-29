using CAS.Entity.BaseDAModels;
using System;

namespace CAS.Entity.FxtProject
{
    [Table("fxtproject.dbo.SYS_ProjectMatch")]
    [Serializable]
    public class ProjectMatch : BaseTO
    {
        private int _Id;

        private int _cityid;

        private int _projectnameid;

        private string _netareaname;

        private string _netname;

        private int _areaid;

        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this._Id = value;
            }
        }

        public int cityid
        {
            get
            {
                return this._cityid;
            }
            set
            {
                this._cityid = value;
            }
        }

        public int projectnameid
        {
            get
            {
                return this._projectnameid;
            }
            set
            {
                this._projectnameid = value;
            }
        }

        public string netareaname
        {
            get
            {
                return this._netareaname;
            }
            set
            {
                this._netareaname = value;
            }
        }

        public string netname
        {
            get
            {
                return this._netname;
            }
            set
            {
                this._netname = value;
            }
        }

        public int areaid
        {
            get
            {
                return this._areaid;
            }
            set
            {
                this._areaid = value;
            }
        }
    }
}
