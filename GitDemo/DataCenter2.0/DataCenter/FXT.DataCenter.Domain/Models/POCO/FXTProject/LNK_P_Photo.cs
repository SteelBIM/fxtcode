using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FXT.DataCenter.Domain.Models.FxtProject
{
    public class LNK_P_Photo
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _projectid;
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private int? _phototypecode;
        [DisplayName("图片类型")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int? phototypecode
        {
            get { return _phototypecode; }
            set { _phototypecode = value; }
        }
        private string _path;
        [DisplayName("图片")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string path
        {
            get { return _path; }
            set { _path = value; }
        }
        private DateTime _photodate = DateTime.Now;
        public DateTime photodate
        {
            get { return _photodate; }
            set { _photodate = value; }
        }
        private string _photoname;
        [DisplayName("图片名称")]
        public string photoname
        {
            get { return _photoname; }
            set { _photoname = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int _fxtcompanyid = 25;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private long _buildingid = 0;
        public long buildingid
        {
            get { return _buildingid; }
            set { _buildingid = value; }
        }
        private decimal? _x;
        public decimal? x
        {
            get { return _x; }
            set { _x = value; }
        }
        private decimal? _y;
        public decimal? y
        {
            get { return _y; }
            set { _y = value; }
        }

        public string PhotoTypeCodeName { get; set; }
        public string BuildingName { get; set; }
    }
}
