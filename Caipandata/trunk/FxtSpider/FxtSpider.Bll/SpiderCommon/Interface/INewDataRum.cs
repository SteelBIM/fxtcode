using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Models;
using System.Collections;

namespace FxtSpider.Bll.SpiderCommon.Interface
{
    /// <summary>
    /// 楼盘案例运行接口
    /// </summary>
    public interface INewDataRum
    {
        /// <summary>
        /// 开始
        /// </summary>
        void start();
        /// <summary>
        /// 城市
        /// </summary>
        string  CityName
        {
            get;
            set;
        }
        /// <summary>
        /// 城市ID
        /// </summary>
        int CityId
        {
            get;
            set;
        }
        /// <summary>
        /// 规则编号
        /// </summary>
        int? RegexNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 主用途
        /// </summary>
        int? BasePurposeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 主用案例类型
        /// </summary>
        int? BaseCaseTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 根据列表页url获取详细信息url
        /// </summary>
        /// <param name="hotUrl">列表页域名</param>
        /// <param name="houseUrl">列表页url</param>
        /// <param name="rate">爬取频率(毫秒)</param>
        /// <param name="pageCheckRate">页面监测频率(毫秒)</param>
        void SpiderHouse(string hostUrl, string pageListUrl, int rate, int pageCheckRate);
        /// <summary>
        /// 保存当前数据
        /// </summary>
        /// <param name="cityName">当前城市</param>
        /// <param name="newHouse"></param>
        void SaveNowData(NewHouse newHouse);
        Queue<string> Url_workload
        {
            get;
            set;
        }
    }
}
