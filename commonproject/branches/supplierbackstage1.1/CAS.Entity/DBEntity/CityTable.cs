using System;
using CAS.Entity.BaseDAModels;


namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_City_Table")]
    public class CityTable : BaseTO
    {
        private int _cityid;
        [SQLField("cityid", EnumDBFieldUsage.PrimaryKey)]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }

        private string projectTable;
        /// <summary>
        /// 楼盘表
        /// </summary>
        public string ProjectTable
        {
            get { return projectTable; }
            set { projectTable = value; }
        }

        private string buildingTable;
        /// <summary>
        /// 楼栋表
        /// </summary>
        public string BuildingTable
        {
            get { return buildingTable; }
            set { buildingTable = value; }
        }

        private string _DAT_SampleAvgPrice_Month;
        /// <summary>
        /// 月样本价格表
        /// </summary>
        public string DAT_SampleAvgPrice_Month
        {
            get { return _DAT_SampleAvgPrice_Month; }
            set { _DAT_SampleAvgPrice_Month = value; }
        }

        private string _DAT_SampleAvgPrice_Week;
        /// <summary>
        /// 周样本价格表
        /// </summary>
        public string DAT_SampleAvgPrice_Week
        {
            get { return _DAT_SampleAvgPrice_Week; }
            set { _DAT_SampleAvgPrice_Week = value; }
        }

        private string _Dat_QueryFilesTable;
        public string Dat_QueryFilesTable
        {
            get { return _Dat_QueryFilesTable; }
            set { _Dat_QueryFilesTable = value; }
        }

        private string _Dat_QuerySurveyTable;
        public string Dat_QuerySurveyTable
        {
            get { return _Dat_QuerySurveyTable; }
            set { _Dat_QuerySurveyTable = value; }
        }

        private string _DAT_CaseTable;
        public string DAT_CaseTable
        {
            get { return _DAT_CaseTable; }
            set { _DAT_CaseTable = value; }
        }

        private string _DAT_QueryFlowTable;
        public string DAT_QueryFlowTable
        {
            get { return _DAT_QueryFlowTable; }
            set { _DAT_QueryFlowTable = value; }
        }

        private string _DAT_QueryTable;
        public string DAT_QueryTable
        {
            get { return _DAT_QueryTable; }
            set { _DAT_QueryTable = value; }
        }

        public string DAT_QueryLandTable { get; set; }
        public string DAT_QueryAssetTable { get; set; }

        private string _HouseTable;
        public string HouseTable
        {
            get { return _HouseTable; }
            set { _HouseTable = value; }
        }

        private string _DAT_SurveyOtherTable;
        public string DAT_SurveyOtherTable
        {
            get { return _DAT_SurveyOtherTable; }
            set { _DAT_SurveyOtherTable = value; }
        }

        private string _Dat_SurveyTextTable;
        public string Dat_SurveyTextTable
        {
            get { return _Dat_SurveyTextTable; }
            set { _Dat_SurveyTextTable = value; }
        }

        /// <summary>
        /// 查勘案例
        /// </summary>
        private string _SurveyCaseTable;
        public string SurveyCaseTable
        {
            get { return _SurveyCaseTable; }
            set { _SurveyCaseTable = value; }
        }

        /// <summary>
        /// 报告表 kevin
        /// </summary>
        private string _DAT_ReportTable;
        public string DAT_ReportTable
        {
            get { return _DAT_ReportTable; }
            set { _DAT_ReportTable = value; }
        }


        /// <summary>
        /// 报告附件表 kevin
        /// </summary>
        private string _DAT_ReportFileTable;
        public string DAT_ReportFileTable
        {
            get { return _DAT_ReportFileTable; }
            set { _DAT_ReportFileTable = value; }
        }

        /// <summary>
        /// 报告估价对象表 kevin
        /// </summary>
        private string _DAT_ReportQueryTable;
        public string DAT_ReportQueryTable
        {
            get { return _DAT_ReportQueryTable; }
            set { _DAT_ReportQueryTable = value; }
        }

        public string DAT_QueryAdjustTable { get; set; }

        public string DAT_Message { get; set; }

        public string DatMessageUser { get; set; }

        public string DAT_QueryHistory { get; set; }

        public string DAT_CASHistoryTable { get; set; }

        public string DAT_QueryTax { get; set; }

        public string QueryTaxSOATable { get; set; }

        //附属房屋价格
        private string _DAT_SubHousePrice;
        public string DAT_SubHousePrice
        {
            get { return _DAT_SubHousePrice; }
            set { _DAT_SubHousePrice = value; }
        }
        private string printtable;
        public string PrintTable
        {
            get { return printtable; }
            set { printtable = value; }
        }

        public string casetable { get; set; }

        private string casebusinesstable;
        public string CaseBusinessTable
        {
            get { return casebusinesstable; }
            set { casebusinesstable = value; }
        }
        private string caselandtable;
        public string CaseLandTable
        {
            get { return caselandtable; }
            set { caselandtable = value; }
        }
        private string queryadjusttable;
        public string QueryAdJustTable
        {
            get { return queryadjusttable; }
            set { queryadjusttable = value; }
        }
        private string querychecktable;
        public string QueryCheckTable
        {
            get { return querychecktable; }
            set { querychecktable = value; }
        }

        private string reportchecktable;
        public string ReportCheckTable
        {
            get { return reportchecktable; }
            set { reportchecktable = value; }
        }
        private string surveybusinesstable;
        public string SurveyBusinessTable
        {
            get { return surveybusinesstable; }
            set { surveybusinesstable = value; }
        }
        private string surveyfactorytable;
        public string SurveyFactoryTable
        {
            get { return surveyfactorytable; }
            set { surveyfactorytable = value; }
        }
        private string surveylandtable;
        public string SurveyLandTable
        {
            get { return surveylandtable; }
            set { surveylandtable = value; }
        }
        private string surveyofficetable;
        public string SurveyOfficeTable
        {
            get { return surveyofficetable; }
            set { surveyofficetable = value; }
        }
    }
}
