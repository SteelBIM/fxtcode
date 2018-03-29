using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_SampleAvgPrice_Week
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _areaid = 0;
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private int _subareaid = 0;
        public int subareaid
        {
            get { return _subareaid; }
            set { _subareaid = value; }
        }
        private int _projectid = 0;
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private string _avgpricedate;
        public string avgpricedate
        {
            get { return _avgpricedate; }
            set { _avgpricedate = value; }
        }
        private int _avgprice;
        public int avgprice
        {
            get { return _avgprice; }
            set { _avgprice = value; }
        }
        private int _buildingareatype = 0;
        /// <summary>
        /// 面积段
        /// </summary>
        public int buildingareatype
        {
            get { return _buildingareatype; }
            set { _buildingareatype = value; }
        }
        private int _housetype = 0;
        /// <summary>
        /// 户型
        /// </summary>
        public int housetype
        {
            get { return _housetype; }
            set { _housetype = value; }
        }
        private int _purposetype = 0;
        /// <summary>
        /// 用途
        /// </summary>
        public int purposetype
        {
            get { return _purposetype; }
            set { _purposetype = value; }
        }
        private int? _buildingdatetype;
        /// <summary>
        /// 建成年代
        /// </summary>
        public int? buildingdatetype
        {
            get { return _buildingdatetype; }
            set { _buildingdatetype = value; }
        }
        private int? _buildingtypecode;
        /// <summary>
        /// 建筑类型
        /// </summary>
        public int? buildingtypecode
        {
            get { return _buildingtypecode; }
            set { _buildingtypecode = value; }
        }
        private DateTime _createtime = DateTime.Now;
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }

    }
}
