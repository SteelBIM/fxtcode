using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    [Serializable]
    public class CompanyProduct_Module
    {
        private int _id;
        /// <summary>
        /// 客户产品模块权限
        /// </summary>
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _companyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _producttypecode;
        /// <summary>
        /// 产品CODE
        /// </summary>
        public int producttypecode
        {
            get { return _producttypecode; }
            set { _producttypecode = value; }
        }
        private int _modulecode;
        /// <summary>
        /// 模块CODE
        /// </summary>
        public int modulecode
        {
            get { return _modulecode; }
            set { _modulecode = value; }
        }
        private int _parentmodulecode = 0;
        /// <summary>
        /// 上级模块CODE
        /// </summary>
        public int parentmodulecode
        {
            get { return _parentmodulecode; }
            set { _parentmodulecode = value; }
        }
        private DateTime? _startdate;
        /// <summary>
        /// 有效期开始日期
        /// </summary>
        public DateTime? startdate
        {
            get { return _startdate; }
            set { _startdate = value; }
        }
        private DateTime? _overdate;
        /// <summary>
        /// 有效期结束日期
        /// </summary>
        public DateTime? overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private string _weburl;
        /// <summary>
        /// 产品网址
        /// </summary>
        public string weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        private string _weburl1;
        /// <summary>
        /// 产品网址1
        /// </summary>
        public string weburl1
        {
            get { return _weburl1; }
            set { _weburl1 = value; }
        }
        private string _murl;
        /// <summary>
        /// 产品其他网址
        /// </summary>
        public string murl
        {
            get { return _murl; }
            set { _murl = value; }
        }
        private string _msgserver;
        /// <summary>
        /// 消息服务地址
        /// </summary>
        public string msgserver
        {
            get { return _msgserver; }
            set { _msgserver = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _appabbreviation;
        public string appabbreviation
        {
            get { return _appabbreviation; }
            set { _appabbreviation = value; }
        }
        /// <summary>
        /// code名称
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// 省名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 省ID
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 行政区名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 片区名称
        /// </summary>
        public string SubAreaName { get; set; }
        /// <summary>
        /// 片区ID
        /// </summary>
        public int SubAreaId { get; set; }

        /// <summary>
        /// 土地用途code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 公司中文名称
        /// FXTProject.dbo.DAT_Company
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 公司英文名称
        /// FXTProject.dbo.DAT_Company
        /// </summary>
        public string EnglishName { get; set; }
       
    }
}
