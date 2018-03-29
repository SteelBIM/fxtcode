using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll;
using log4net;
using System.Threading;
using FxtSpider.FxtApi.ApiManager;
using FxtSpider.FxtApi;
using FxtSpider.Common;

namespace FxtSpider.DataPubSource.RunModel
{
    /// <summary>
    /// 案例整理入库运行类(根据网站,城市)
    /// </summary>
    public class CaseDataUpload
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CaseDataUpload));
        public string 城市名称
        {
            get;
            set;
        }
        public string 网站名称
        {
            get;
            set;
        }
        public int 一次上传个数
        {
            get;
            set;
        }
        /// <summary>
        /// 频率 毫秒
        /// </summary>
        public int 频率
        {
            get;
            set;
        }
        public CaseDataUpload(string _城市名称, string _网站名称, int _一次上传个数, int _频率)
        {
            this.城市名称 = _城市名称;
            this.网站名称 = _网站名称;
            this.一次上传个数 = _一次上传个数;
            this.频率 = _频率;
        }
        public void start()
        {
            ThreadStart ts = new ThreadStart(this.Upload);
            Thread m_thread = new Thread(ts);
            m_thread.Start();
        }
        private void Upload()
        {
            //根据城市名称,网站名称获取ID
            int? cityId = null;
            int? webId = null;
            if (this.城市名称 != "所有") { cityId = CityManager.GetCityIdByCityName(this.城市名称); }
            if (this.网站名称 != "所有") { webId = WebsiteManager.GetWebIdByWebName(this.网站名称); }
            if(cityId==0)
            {
                log.Debug(string.Format("传入城市名称错误:(城市名称:{0},网站名称:{1})", this.城市名称 == null ? "null" : this.城市名称, this.网站名称 == null ? "null" : this.网站名称));
                return;
            }
            if(webId==0)
            {
                log.Debug(string.Format("传入网站名称错误:(城市名称:{0},网站名称:{1})", this.城市名称 == null ? "null" : this.城市名称, this.网站名称 == null ? "null" : this.网站名称));
                return;
            }
            //开始数据上传
            while (true)
            {
                System.Threading.Thread.Sleep(this.频率);
                //查询需要整理的案例
                log.Debug(string.Format("查询需要整理的案例数据:(城市名称:{0},网站名称:{1})",
                    (this.城市名称 == null ? "null" : this.城市名称) + "$" +cityId.ToString2(),
                    (this.网站名称 == null ? "null" : this.网站名称) + "$" + webId.ToString2()));
                List<VIEW_案例信息_城市表_网站表> list = CaseLogManager.获取当前要整理入库的案例(cityId, webId, this.一次上传个数);
                if (list == null || list.Count < 1)
                {
                    log.Debug(string.Format("案例数据已整理发布完成:(城市名称:{0},网站名称:{1})",
                        (this.城市名称 == null ? "null" : this.城市名称) + "$" + cityId.ToString2(),
                        (this.网站名称 == null ? "null" : this.网站名称) + "$" + webId.ToString2()));
                    break;
                }
                //数据上传
                log.Debug(string.Format("已查询到需要整理的案例数据{0}个,以下数据发布中......", list.Count));
                #region 多个上传
                List<案例库上传信息过滤表> 过滤案例List = null;
                Dictionary<long, int> dic = new Dictionary<long, int>();
                list.ForEach(delegate(VIEW_案例信息_城市表_网站表 _obj)
                {
                    log.Debug(string.Format("发布数据中:(ID:{0},城市名称:{1},网站名称:{2})",_obj.ID,_obj.城市,_obj.网站));
                });
                if (!CaseApi.发布需要整理的数据到服务器(list, out 过滤案例List, out dic))
                {
                    log.Debug(string.Format("发布需要整理的数据到服务器_异常:(案例ID个数:{0})",list.Count));
                    continue;
                }
                //记录过滤掉的案例ID
                log.Debug(string.Format("获取到以下要过滤的案例:(过滤ID数组个数:{0})",过滤案例List == null ? 0 : 过滤案例List.Count));
                
                if (过滤案例List != null && 过滤案例List.Count > 0)
                {
                    过滤案例List.ForEach(delegate(案例库上传信息过滤表 _obj)
                    {
                        log.Debug(string.Format("过滤掉的案例:(ID:{0},城市ID:{1},网站ID:{2})", _obj.案例ID, _obj.城市ID, _obj.网站ID.ToString2()));
                    });
                    log.Debug(string.Format("将上传时被过滤的信息记录到表中:(城市名称:{0},网站名称:{1},案例ID个数:{2},过滤ID数组个数:{3})",
                        (this.城市名称 == null ? "null" : this.城市名称) + "$" + cityId.ToString2(),
                        (this.网站名称 == null ? "null" : this.网站名称) + "$" + webId.ToString2(),
                         list.Count, 过滤案例List.Count));
                    if (!CaseFilterLogManager.将上传时被过滤的信息记录到表中(过滤案例List))
                    {
                        log.Debug(string.Format("将上传时被过滤的信息记录到表中_异常:(城市名称:{0},网站名称:{1},案例ID个数:{2},过滤ID数组个数:{3})",
                            (this.城市名称 == null ? "null" : this.城市名称) + "$" + cityId.ToString2(),
                            (this.网站名称 == null ? "null" : this.网站名称) + "$" + webId.ToString2(),
                             list.Count, 过滤案例List.Count));
                        break;
                    }
                }
                //统计入库失败的案例个数
                log.Debug(string.Format("统计入库失败的案例个数:(城市名称:{0},网站名称:{1},案例ID:{2},案例ID个数:{3})",
                         (this.城市名称 == null ? "null" : this.城市名称) + "$" + cityId.ToString2(),
                         (this.网站名称 == null ? "null" : this.网站名称) + "$" + webId.ToString2(),
                         Convert.ToInt64(list[list.Count - 1].ID), list.Count));
                string message = "";
                if (!ProjectCaseCountManager.UpdateNotImportCaseCount(list, 过滤案例List, out message))
                {
                    log.Debug(string.Format("统计入库失败的案例个数_系统异常:(案例ID个数{0})", list.Count));
                    break;
                }
                //记录上传成功的案例ID
                log.Debug(string.Format("将当前已经整理入库的案例记录表中:(城市名称:{0},网站名称:{1},案例ID:{2},案例ID个数:{3})",
                         (this.城市名称 == null ? "null" : this.城市名称) + "$" + cityId.ToString2(),
                         (this.网站名称 == null ? "null" : this.网站名称) + "$" + webId.ToString2(),
                         Convert.ToInt64(list[list.Count - 1].ID), list.Count));
                if (!CaseLogManager.将当前已经整理入库的案例记录表中(list, dic))
                {
                    log.Debug(string.Format("将当前已经整理入库的案例记录表中_系统异常:(城市名称:{0},网站名称:{1},案例ID:{2},案例ID个数:{3})",
                             (this.城市名称 == null ? "null" : this.城市名称) + "$" + cityId.ToString2(),
                             (this.网站名称 == null ? "null" : this.网站名称) + "$" + webId.ToString2(),
                             Convert.ToInt64(list[list.Count-1].ID),list.Count));
                    break;
                }
                log.Debug(string.Format("当前案例ID数据记录完成:(城市名称:{0},网站名称:{1},案例ID:{2},案例ID个数:{3})",
                         (this.城市名称 == null ? "null" : this.城市名称) + "$" + cityId.ToString2(),
                         (this.网站名称 == null ? "null" : this.网站名称) + "$" + webId.ToString2(),
                           Convert.ToInt64(list[list.Count - 1].ID), list.Count));
                #endregion
            }
        }
    }
}
