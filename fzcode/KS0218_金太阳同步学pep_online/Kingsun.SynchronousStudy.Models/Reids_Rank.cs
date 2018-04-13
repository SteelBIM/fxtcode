using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Models
{
    public class Redis_IntDubb_Rank
    {
        public string UserId { get; set; }
        public string VideoId { get; set; }
        public double TotalScore { get; set; }
        public int Sort { get; set; }

    }

    public class Redis_IntDubb_NewRank
    {
        public string VideoId { get; set; }
        public string CreateTime { get; set; }
        public int Sort { get; set; }
    }

    public class Redis_IntDubb_VideoInfo
    {
        public string VideoId { get; set; }
        public string UserId { get; set; }
        public string TrueName { get; set; }
        public string TotalScore { get; set; }
        public List<string> NumberOfOraise { get; set; }
        public string UserImage { get; set; }
        public string CreateTime { get; set; }
    }
    public class Redis_IntDubb_VideoInfoSort
    {
        public string VideoId { get; set; }
        public string UserId { get; set; }
        public string TrueName { get; set; }
        public double TotalScore { get; set; }
        public int NumberOfOraise { get; set; }
        public string UserImage { get; set; }
        public string CreateTime { get; set; }
        public int Sort { get; set; }
    }

    public class UserIdList
    {
        public string UserId { get; set; }
    }

    public class Redis_PraiseStatistics
    {
        public string UserId { get; set; }
    }
}
