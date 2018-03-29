using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class StatisticFxtCustomerMarket : BaseTO
    {
        [SQLReadOnly]
        public int userid { get; set; }

        [SQLReadOnly]
        public string usertruename { get; set; }

        [SQLReadOnly]
        public int followupcount { get; set; }

        [SQLReadOnly]
        public int telfollowupcount { get; set; }

        [SQLReadOnly]
        public int salervisitcount { get; set; }

        [SQLReadOnly]
        public int salervisitcitycount { get; set; }        

        [SQLReadOnly]
        public int custvisitcount { get; set; }

        [SQLReadOnly]
        public int signedcustcount { get; set; }

        [SQLReadOnly]
        public int signingmoney { get; set; }
    }

    public class StatisticFxtCustomerSysData : BaseTO
    {
        [SQLReadOnly]
        public int companyid { get; set; }
        [SQLReadOnly]
        public string companyname { get; set; }
        [SQLReadOnly]
        public int periodreportcount { get; set; }
        [SQLReadOnly]
        public int periodpreappraisecount { get; set; }
        [SQLReadOnly]
        public int periodquerycount { get; set; }
        [SQLReadOnly]
        public int periodwxquerycount { get; set; }
        [SQLReadOnly]
        public int periodcasquerycount { get; set; }
        [SQLReadOnly]
        public int periodwebquerycount { get; set; }
        [SQLReadOnly]
        public int periodsurveycount { get; set; }
        [SQLReadOnly]
        public int periodprojectcount { get; set; }
        [SQLReadOnly]
        public int periodbuildingcount { get; set; }
        [SQLReadOnly]
        public int periodhousecount { get; set; }
        [SQLReadOnly]
        public int periodcasecount { get; set; }
        [SQLReadOnly]
        public int totalreportcount { get; set; }
        [SQLReadOnly]
        public int totalpreappraisecount { get; set; }
        [SQLReadOnly]
        public int totalquerycount { get; set; }
        [SQLReadOnly]
        public int totalwxquerycount { get; set; }
        [SQLReadOnly]
        public int totalcasquerycount { get; set; }
        [SQLReadOnly]
        public int totalwebquerycount { get; set; }
        [SQLReadOnly]
        public int totalsurveycount { get; set; }
        [SQLReadOnly]
        public int totalprojectcount { get; set; }
        [SQLReadOnly]
        public int totalbuildingcount { get; set; }
        [SQLReadOnly]
        public int totalhousecount { get; set; }
        [SQLReadOnly]
        public int totalcasecount { get; set; }
    }
}
