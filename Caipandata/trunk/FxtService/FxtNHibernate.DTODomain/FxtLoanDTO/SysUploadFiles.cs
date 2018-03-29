using FxtNHibernate.FxtLoanDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtNHibernate.DTODomain.FxtLoanDTO
{
    public class SysUploadFiles : SysUploadFile
    {
        public SysUploadFiles(SysUploadFile sysUploadFile)
        {
            this.Id = sysUploadFile.Id;
            this.Name = sysUploadFile.Name;
            this.FilePath = sysUploadFile.FilePath;
            this.FileType = sysUploadFile.FileType;
            this.FileSize = sysUploadFile.FileSize;
            this.UserId = sysUploadFile.UserId;
            this.CreateDate = sysUploadFile.CreateDate;
            this.PageIndex = sysUploadFile.PageIndex;
            this.Count = sysUploadFile.Count;
            this.BankProid = sysUploadFile.BankProid;
            this.BankId = sysUploadFile.BankId;
        }

        public int FileSuccessCount { get; set; }
    }
}
