using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Dat_PeiTao
    {
        public int id { get; set; }
        public int cityid { get; set; }
        public int areaid { get; set; }
        public string schoolname { get; set; }
        public string address { get; set; }
        public int typecode { get; set; }
        public decimal? buildingarea { get; set; }
        public int classcode { get; set; }
        public string east { get; set; }
        public string west { get; set; }
        public string south { get; set; }
        public string north { get; set; }
        public decimal? x { get; set; }
        public decimal? y { get; set; }
        public int valid { get; set; }
        public string alias { get; set; }
        public string details { get; set; }
    }
}
