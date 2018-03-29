using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class LNK_P_Appendage
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _appendagecode;
        /// <summary>
        /// 配套类型
        /// </summary>
        [DisplayName("配套类型")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int appendagecode
        {
            get { return _appendagecode; }
            set { _appendagecode = value; }
        }
        private int _projectid;
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private decimal? _area;
         [DisplayName("配套面积")]
        [RegularExpression(@"^[0-9]+\.[0-9]{1,4}|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
        public decimal? area
        {
            get { return _area; }
            set { _area = value; }
        }
        private string _p_aname;
        /// <summary>
        /// 配套信息
        /// </summary>
        [DisplayName("配套信息")]
        public string p_aname
        {
            get { return _p_aname; }
            set { _p_aname = value; }
        }
        private bool _isinner=true;
        [DisplayName("是否内部")]
        public bool isinner
        {
            get { return _isinner; }
            set { _isinner = value; }
        }
        private int? _cityid;
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _classcode;
        /// <summary>
        /// 配套等级
        /// </summary>
        public int classcode
        {
            get { return _classcode; }
            set { _classcode = value; }
        }

        #region 扩展字段
        /// <summary>
        /// 配套类型名称
        /// </summary>
        public string AppendageName { get; set; }
         /// <summary>
        /// 配套等级名称
        /// </summary>
        public string ClassName { get; set; }
        #endregion

    }
}
