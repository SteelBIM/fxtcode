using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyDBEntity
{
    /// <summary>
    /// 查勘库配置表 caoq 2013-11-11
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.SYS_Config")]
    public class SYSConfig : BaseTO
    {
        private int _configid;
        /// <summary>
        /// 配置ID
        /// </summary>
        [SQLField("configid", EnumDBFieldUsage.PrimaryKey,true)]
        public int configid
        {
            get { return _configid; }
            set { _configid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID/银行ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID(允许不同城市有不同设置信息)
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _marklogourl;
        /// <summary>
        /// 水印图片
        /// </summary>
        public string marklogourl
        {
            get { return _marklogourl; }
            set { _marklogourl = value; }
        }
        private decimal? _mapheight;
        /// <summary>
        /// 一键下载地图高
        /// </summary>
        public decimal? mapheight
        {
            get { return _mapheight; }
            set { _mapheight = value; }
        }
        private decimal? _mapwidth;
        /// <summary>
        /// 一键下载地图宽
        /// </summary>
        public decimal? mapwidth
        {
            get { return _mapwidth; }
            set { _mapwidth = value; }
        }
        private int? _marklogolocation;
        /// <summary>
        /// 水印位置（0中间 1左上角、-1左下角、2右上角、-2右下角）
        /// </summary>
        public int? marklogolocation
        {
            get { return _marklogolocation; }
            set { _marklogolocation = value; }
        }
        private decimal? _marklogotransparency;
        /// <summary>
        /// 水印透明度
        /// </summary>
        public decimal? marklogotransparency
        {
            get { return _marklogotransparency; }
            set { _marklogotransparency = value; }
        }
    }
}