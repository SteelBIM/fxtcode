using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtSpider.Bll.SpiderCommon.Models
{
    /// <summary>
    /// 新房源案例实体
    /// </summary>
    public class NewHouse
    {

        private string lpm;
        /// <summary>
        /// 楼盘名
        /// </summary>
        public string Lpm
        {
            get { return lpm; }
            set { lpm = value; }
        }
        private string alsj;
        /// <summary>
        /// 案例时间
        /// </summary>
        public string Alsj
        {
            get { return alsj; }
            set { alsj = value; }
        }
        private string xzq;
        /// <summary>
        /// 行政区
        /// </summary>
        public string Xzq
        {
            get { return xzq; }
            set { xzq = value; }
        }
        private string pq;
        /// <summary>
        /// 片区
        /// </summary>
        public string Pq
        {
            get { return pq; }
            set { pq = value; }
        }
        private string ld;
        /// <summary>
        /// 楼栋
        /// </summary>
        public string Ld
        {
            get { return ld; }
            set { ld = value; }
        }
        private string fh;
        /// <summary>
        /// 房号
        /// </summary>
        public string Fh
        {
            get { return fh; }
            set { fh = value; }
        }
        private string yt;
        /// <summary>
        /// 用途
        /// </summary>
        public string Yt
        {
            get { return yt; }
            set { yt = value; }
        }
        private string mj;
        /// <summary>
        /// 面积
        /// </summary>
        public string Mj
        {
            get { return mj; }
            set { mj = value; }
        }
        private string dj;
        /// <summary>
        /// 单价
        /// </summary>
        public string Dj
        {
            get { return dj; }
            set { dj = value; }
        }
        private string allx;
        /// <summary>
        /// 案例类型
        /// </summary>
        public string Allx
        {
            get { return allx; }
            set { allx = value; }
        }
        private string jg;
        /// <summary>
        /// 结构
        /// </summary>
        public string Jg
        {
            get { return jg; }
            set { jg = value; }
        }
        private string jzlx;
        /// <summary>
        /// 建筑类型
        /// </summary>
        public string Jzlx
        {
            get { return jzlx; }
            set { jzlx = value; }
        }
        private string zj;
        /// <summary>
        /// 总价
        /// </summary>
        public string Zj
        {
            get { return zj; }
            set { zj = value; }
        }
        private string szlc;
        /// <summary>
        /// 所在楼层
        /// </summary>
        public string Szlc
        {
            get { return szlc; }
            set { szlc = value; }
        }
        private string zlc;
        /// <summary>
        /// 总楼层
        /// </summary>
        public string Zlc
        {
            get { return zlc; }
            set { zlc = value; }
        }
        private string hx;
        /// <summary>
        /// 户型
        /// </summary>
        public string Hx
        {
            get { return hx; }
            set { hx = value; }
        }
        private string cx;
        /// <summary>
        /// 朝向
        /// </summary>
        public string Cx
        {
            get { return cx; }
            set { cx = value; }
        }
        private string zx;
        /// <summary>
        /// 装修
        /// </summary>
        public string Zx
        {
            get { return zx; }
            set { zx = value; }
        }
        private string jznd;
        /// <summary>
        /// 建筑年代
        /// </summary>
        public string Jznd
        {
            get { return jznd; }
            set { jznd = value; }
        }
        private string title;
        /// <summary>
        /// 信息(备注)
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }        
        private string phone;
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        private string url;
        /// <summary>
        /// URL
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        private string bz;
        /// <summary>
        /// 币种
        /// </summary>
        public string Bz
        {
            get { return bz; }
            set { bz = value; }
        }
        private string wzly;
        /// <summary>
        /// 网站来源
        /// </summary>
        public string Wzly
        {
            get { return wzly; }
            set { wzly = value; }
        }
        private string addres;
        /// <summary>
        /// 地址
        /// </summary>
        public string Addres
        {
            get { return addres; }
            set { addres = value; }
        }
        private string jzxs;
        /// <summary>
        /// 建筑形式
        /// </summary>
        public string Jzxs
        {
            get { return jzxs; }
            set { jzxs = value; }
        }
        private string hymj;
        /// <summary>
        /// 花园面积
        /// </summary>
        public string Hymj
        {
            get { return hymj; }
            set { hymj = value; }
        }
        private string tjg;
        /// <summary>
        /// 厅结构
        /// </summary>
        public string Tjg
        {
            get { return tjg; }
            set { tjg = value; }
        }
        private string cwsl;
        /// <summary>
        /// 车位数量
        /// </summary>
        public string Cwsl
        {
            get { return cwsl; }
            set { cwsl = value; }
        }
        private string ptss;
        /// <summary>
        /// 配套设施
        /// </summary>
        public string Ptss
        {
            get { return ptss; }
            set { ptss = value; }
        }
        private string dxsmj;
        /// <summary>
        /// 地下室面积
        /// </summary>
        public string Dxsmj
        {
            get { return dxsmj; }
            set { dxsmj = value; }
        }
        private string comName;
        /// <summary>
        /// 中介公司
        /// </summary>
        public string ComName
        {
            get { return comName; }
            set { comName = value; }
        }
        private string comArea;
        /// <summary>
        /// 门店
        /// </summary>
        public string ComArea
        {
            get { return comArea; }
            set { comArea = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_lpm">楼盘名</param>
        /// <param name="_alsj">案例时间</param>
        /// <param name="_xzq">行政区</param>
        /// <param name="_pq">片区</param>
        /// <param name="_ld">楼栋</param>
        /// <param name="_fh">房号</param>
        /// <param name="_yt">用途</param>
        /// <param name="_mj">面积</param>
        /// <param name="_dj">单价</param>
        /// <param name="_allx">案例类型</param>
        /// <param name="_jg">结构</param>
        /// <param name="_jzlx">建筑类型</param>
        /// <param name="_zj">总价</param>
        /// <param name="_szlc">所在楼层</param>
        /// <param name="_zlc">总楼层</param>
        /// <param name="_hx">户型</param>
        /// <param name="_cx">朝向</param>
        /// <param name="_zx">装修</param>
        /// <param name="_jznd">建筑年代</param>
        /// <param name="_title">信息(备注)</param>
        /// <param name="_phone">电话</param>
        /// <param name="_url">URL</param>
        /// <param name="_bz">币种</param>
        /// <param name="_wzly">网站来源</param>
        /// <param name="_jzxs">建筑形式</param>
        /// <param name="_hymj">花园面积</param>
        /// <param name="_tjg">厅结构</param>
        /// <param name="_cwsl">车位数量</param>
        /// <param name="_ptss">配套设施</param>
        /// <param name="_comName">中介公司</param>
        /// <param name="_comArea">门店</param>
        public NewHouse(string _lpm, string _alsj, string _xzq, string _pq, string _ld, string _fh, string _yt, string _mj, string _dj,
            string _allx, string _jg, string _jzlx, string _zj, string _szlc, string _zlc, string _hx, string _cx, string _zx, string _jznd,
            string _title, string _phone, string _url, string _bz, string _wzly, string _addres, string _jzxs, string _hymj, string _tjg, string _cwsl, string _ptss, string _dxsmj, string _comName, string _comArea)
        {
            this.lpm = _lpm;
            this.alsj = _alsj;
            this.xzq = _xzq;
            this.pq = _pq;
            this.ld = _ld;
            this.fh = _fh;
            this.yt = _yt;
            this.mj = _mj;
            this.dj = _dj;
            this.allx = _allx;
            this.jg = _jg;
            this.jzlx = _jzlx;
            this.zj = _zj;
            this.szlc = _szlc;
            this.zlc = _zlc;
            this.hx = _hx;
            this.cx = _cx;
            this.zx = _zx;
            this.jznd = _jznd;
            this.title = _title;
            this.phone = _phone;
            this.url = _url;
            this.bz = _bz;
            this.wzly = _wzly;
            this.addres = _addres;
            this.jzxs = _jzxs;
            this.hymj = _hymj;
            this.tjg = _tjg;
            this.cwsl = _cwsl;
            this.ptss = _ptss;
            this.dxsmj = _dxsmj;
            this.comName = _comName;
            this.comArea = _comArea;
        }
    }
}
