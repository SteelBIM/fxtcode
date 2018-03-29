using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model = FxtNHibernate.FxtLoanDomain.Entities;

namespace FxtNHibernate.DTODomain.FxtLoanDTO
{
    public class DataCollaterals : Model.DataCollateral
    {
        public DataCollaterals()
        { }
        public DataCollaterals(Model.DataCollateral model)
        {
            this.Id = model.Id;
            this.Number = model.Number;
            this.BankId = model.BankId;
            this.Branch = model.Branch;
            this.PurposeCode = model.PurposeCode;
            this.PurposeName = model.PurposeName;
            this.Name = model.Name;
            this.BuildingArea = model.BuildingArea;
            this.BuildingAreaCode = model.BuildingAreaCode;
            this.Address = model.Address;
            this.BranchCityId = model.BranchCityId;
            this.ProjectId = model.ProjectId;
            this.ProvinceId = model.ProvinceId;
            this.CityId = model.CityId;
            this.AreaId = model.AreaId;
            this.SubAreaId = model.SubAreaId;
            this.BuildingId = model.BuildingId;
            this.RoomId = model.RoomId;
            this.ProjectName = model.ProjectName;
            this.ProjectAddress = model.ProjectAddress;
            this.Installment = model.Installment;
            this.ProjectType = model.ProjectType;
            this.Road = model.Road;
            this.RoadNumber = model.RoadNumber;
            this.BuildingNumber = model.BuildingNumber;
            this.FloorNumber = model.FloorNumber;
            this.RoomNumber = model.RoomNumber;
            this.Clerk = model.Clerk;
            this.Agency = model.Agency;
            this.FirstPay = model.FirstPay;
            this.MonthsPay = model.MonthsPay;
            this.LoanDate = model.LoanDate;
            this.LoanYear = model.LoanYear;
            this.LoanInterestRate = model.LoanInterestRate;
            this.LoanCardinalNumber = model.LoanCardinalNumber;
            this.LoanAmount = model.LoanAmount;
            this.LoanBalance = model.LoanBalance;
            this.LoanPeople = model.LoanPeople;
            this.LoanSex = model.LoanSex;
            this.LoanAge = model.LoanAge;
            this.LoanCardID = model.LoanCardID;
            this.LoanCensus = model.LoanCensus;
            this.LoanProfession = model.LoanProfession;
            this.LoanEducationalBackground = model.LoanEducationalBackground;
            this.LoanMarriage = model.LoanMarriage;
            this.LoanSalary = model.LoanSalary;
            this.CreditRating = model.CreditRating;
            this.LoanCompany = model.LoanCompany;
            this.LineOfcredit = model.LineOfcredit;
            this.Status = model.Status;
            this.CreateDate = model.CreateDate;
            this.MatchStatus = model.MatchStatus;
            this.OldRate = model.OldRate;
            this.GuaranteePrice = model.GuaranteePrice;
            this.OldMortgageRates = model.OldMortgageRates;
            this.UploadFileId = model.UploadFileId;
            this.BankProjectId = model.BankProjectId;
        }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string AreaName { get; set; }
        /// <summary>
        /// 楼盘是否匹配
        /// </summary>
        public bool ProjectNameMatch { get; set; }
        /// <summary>
        /// 楼栋是否匹配
        /// </summary>
        public bool BuildingNumberMatch { get; set; }
        /// <summary>
        /// 楼层是否匹配
        /// </summary>
        public bool FloorNumberMatch { get; set; }
        /// <summary>
        /// 房号是否匹配
        /// </summary>
        public bool RoomNumberMatch { get; set; }
    }
}
