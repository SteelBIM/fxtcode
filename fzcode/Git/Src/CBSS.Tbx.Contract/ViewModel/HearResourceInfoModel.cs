using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class HearResourceInfoModel :IComparable<HearResourceInfoModel>
    {
        public int? SerialNumber { set; get; }
        public int? TextSerialNumber { set; get; }
        public double? TotalScore { set; get; }
        public double? AverageScore { set; get; }
        public string TextDesc { set; get; }
        public DateTime? CreateDate { set; get; }
        public string VideoFileID { set; get; }
        public int CompareTo(HearResourceInfoModel obj)
        {
            if (this.SerialNumber == obj.SerialNumber)
            {
                return Convert.ToInt32(this.TextSerialNumber - obj.TextSerialNumber);
            }
            else
            {
                return Convert.ToInt32(this.SerialNumber - obj.SerialNumber);
            }
        }
    }
}
