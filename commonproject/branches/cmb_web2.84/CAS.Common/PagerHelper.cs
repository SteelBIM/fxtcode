using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
namespace CAS.Common
{
    /// <summary>
    /// CommonPager 的摘要说明
    /// 可用于无刷新或通过js函数来翻页
    /// hy 2007-5-31
    /// </summary>
    public class PagerHelper
    {
        private bool _show;
        private bool _showRecordCount;
        private int _recordCount;
        private bool _showPageCount;
        private int _pageCount;
        private bool _showFirstPage;
        private string _firstPageText;
        private bool _showLastPage;
        private string _lastPageText;
        private bool _showNextPage;
        private string _nextPageText;
        private bool _showPrevPage;
        private string _prevPageText;
        private int _pageSize;
        private int _curPage;
        private string _commandScript;
        private string _pageCountText;
        private string _recordCountText;
        private string[] _orderItems;
        private string[] _orderValues;
        private string _orderScript;
        private string _orderParams;
        private string _selectedValue;

        private bool _showPageNumbers;
        private string _pageNumberFormat;
        private int _pageNumbersCount;

        //2008-4-21 hy 加入当前页链接分页
        private string _currentUrl;
        private bool _useUrl;

        public PagerHelper()
        {
            _show = true;
            _showPageCount = false;
            _pageCount = 1;
            _showRecordCount = true;
            _recordCount = 0;
            _showFirstPage = true;
            _firstPageText = "|<"; //首页
            _showLastPage = true;
            _lastPageText = ">|"; //末页
            _showNextPage = true;
            _nextPageText = ">>"; //下一页
            _showPrevPage = true;
            _prevPageText = "<<"; //上一页
            _pageSize = 5;
            _curPage = 1;
            _commandScript = "void()";
            _pageCountText = "当前页[{0}]/共{1}页";
            _recordCountText = "共{0}条记录";
            _orderItems = null;
            _orderValues = null;
            _selectedValue = "";
            _showPageNumbers = false;
            _pageNumbersCount = 0;
            _pageNumberFormat = "";

            //2008-4-21 hy
            _useUrl = false;
            _currentUrl = HttpContext.Current.Request.Url.ToString().ToLower();
            if (_currentUrl.IndexOf("://") > 0)
            {
                _currentUrl = _currentUrl.Remove(_currentUrl.IndexOf("://"), _currentUrl.IndexOf("://") + 3);
            }
            _currentUrl = _currentUrl.Substring(_currentUrl.IndexOf("/"));

            if (_currentUrl.IndexOf("pagenumber") > 0)
            {
                _currentUrl = buildurl(_currentUrl, "pagenumber");
            }
            if (_currentUrl.IndexOf("?") > 0)
            {
                _currentUrl += "&pagenumber=";
            }
            else
            {
                _currentUrl += "?pagenumber=";
            }

        }

        private string buildurl(string url, string param)
        {
            string url1 = url;
            if (url.IndexOf(param) > 0)
            {
                if (url.IndexOf("&", url.IndexOf(param) + param.Length) > 0)
                {
                    url1 = url.Substring(0, url.IndexOf(param) - 1) + url.Substring(url.IndexOf("&", url.IndexOf(param) + param.Length) + 1);
                }
                else
                {
                    url1 = url.Substring(0, url.IndexOf(param) - 1);
                }
                return url1;
            }
            else
            {
                return url1;
            }
        }

        //是否使用当前链接翻页（非局部刷新）
        public bool UseUrl
        {
            get { return _useUrl; }
            set
            {
                _useUrl = value;
                if (_useUrl)
                    _curPage = Convert.ToInt32(HttpContext.Current.Request["pagenumber"] == null ? "1" : HttpContext.Current.Request["pagenumber"]);
            }
        }

        //首页文字
        public string FirstPageText
        {
            get { return _firstPageText; }
            set { _firstPageText = value; }
        }
        //末页文字
        public string LastPageText
        {
            get { return _lastPageText; }
            set { _lastPageText = value; }
        }
        //下一页文字


        public string NextPageText
        {
            get { return _nextPageText; }
            set { _nextPageText = value; }
        }
        //上一页文字


        public string PrevPageText
        {
            get { return _prevPageText; }
            set { _prevPageText = value; }
        }
        //javascript函数
        public string CommandScript
        {
            get { return _commandScript; }
            set { _commandScript = value; }
        }
        //显示首页
        public bool ShowFirstPage
        {
            get { return _showFirstPage; }
            set { _showFirstPage = value; }
        }
        //显示末页
        public bool ShowLastPage
        {
            get { return _showLastPage; }
            set { _showLastPage = value; }
        }
        //显示下一页


        public bool ShowNextPage
        {
            get { return _showNextPage; }
            set { _showNextPage = value; }
        }
        //显示上一页


        public bool ShowPrevPage
        {
            get { return _showPrevPage; }
            set { _showPrevPage = value; }
        }
        //显示本身 
        public bool Show
        {
            get { return _show; }
            set { _show = value; }
        }
        //显示记录数


        public bool ShowRecordCount
        {
            get { return _showRecordCount; }
            set { _showRecordCount = value; }
        }
        //记录数


        public int RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        }
        //显示页总数
        public bool ShowPageCount
        {
            get { return _showPageCount; }
            set { _showPageCount = value; }
        }
        //最后输出


        public string OutputHTML
        {
            get { return getOutputHTML(); }
        }
        //页大小


        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
        //当前页


        public int CurPage
        {
            get { return _curPage; }
            set { _curPage = value; }
        }
        //记录数文字


        public string RecordCountText
        {
            get { return _recordCountText; }
            set { _recordCountText = value; }
        }
        //页数文字
        public string PageCountText
        {
            get { return _pageCountText; }
            set { _pageCountText = value; }
        }


        //排序显示文本
        public string[] OrderItems
        {
            get { return _orderItems; }
            set { _orderItems = value; }
        }

        //排序字段
        public string[] OrderValues
        {
            get { return _orderValues; }
            set { _orderValues = value; }
        }

        //select的事件

        public string OrderScript
        {
            get { return _orderScript; }
            set { _orderScript = value; }
        }

        public string OrderParams
        {
            get { return _orderParams; }
            set { _orderParams = value; }
        }

        /// <summary>
        /// select选中项的值

        /// </summary>
        public string SelectedValue
        {
            get { return _selectedValue; }
            set { _selectedValue = value; }
        }

        /// <summary>
        /// 显示页数
        /// </summary>
        public bool ShowPageNumbers
        {
            get { return _showPageNumbers; }
            set { _showPageNumbers = value; }
        }

        /// <summary>
        /// 页数显示分隔数

        /// </summary>
        public int PageNumbersCount
        {
            get { return _pageNumbersCount; }
            set { _pageNumbersCount = value; }
        }

        /// <summary>
        /// 数据显示格式
        /// </summary>
        public string PageNumberFormat
        {
            get { return _pageNumberFormat; }
            set { _pageNumberFormat = value; }
        }

        private int _Width = 100;
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        //组合字符串，输出最终翻页的html代码
        private string getOutputHTML()
        {
            if (!_show) return "";
            if (_recordCount == 0 && !_show) return "";

            if (_recordCount > 0)
            {
                if (_recordCount % _pageSize == 0)
                {
                    _pageCount = _recordCount / _pageSize;
                }
                else
                {
                    _pageCount = _recordCount / _pageSize + 1;
                }
            }
            if (_pageCount <= 0) _pageCount = 1;

            string str = "";
            //style='width:" + Width.ToString() + "px'
            str += "<div class='Page_Nav'>";
            if (_showFirstPage)
            {
                if (_curPage > 1)
                {
                    if (!_useUrl)
                    {
                        str += string.Format("<a href='javascript:void(0)' onclick='javascript:{0}({1})' title='第一页'>{2}</a>", _commandScript, 1, _firstPageText);
                    }
                    else
                    {
                        str += string.Format("<a href='{0}{1}' title='第一页'>{2}</a>", _currentUrl, 1, _firstPageText);
                    }
                }
                else
                {
                    // str += string.Format("{0}", _firstPageText);
                }
            }
            if (_showPrevPage)
            {
                if (_curPage > 1)
                {
                    if (!_useUrl)
                    {
                        str += string.Format("<a href='javascript:void(0)' onclick='javascript:{0}({1})' title='上一页'>{2}</a>", _commandScript, _curPage - 1, _prevPageText);
                    }
                    else
                    {
                        str += string.Format("<a href='{0}{1}' title='上一页'>{2}</a>", _currentUrl, _curPage - 1, _prevPageText);
                    }
                }
                else
                {
                    //str += string.Format("{0}", _prevPageText);
                }
            }
            //显示页数
            string txtNum = "";
            if (_showPageNumbers && _pageCount > 0)
            {
                if (_pageCount > _pageNumbersCount)
                {
                    int begin = (_curPage - 1) / _pageNumbersCount;

                    if (_curPage > _pageNumbersCount)
                    {
                        if (!_useUrl)
                        {
                            str += string.Format("<a href='javascript:void(0)' onclick='javascript:{0}({1})' title='上{2}页'>...</a>", _commandScript, _pageNumbersCount * (begin), _pageNumbersCount);
                        }
                        else
                        {
                            str += string.Format("<a href='{0}{1}' title='上{2}页'>...</a>", _currentUrl, _pageNumbersCount * (begin), _pageNumbersCount);
                        }
                    }

                    for (int i = begin * _pageNumbersCount + 1; i <= (begin + 1) * PageNumbersCount; i++)
                    {
                        if (_pageCount < i) break;
                        txtNum = i.ToString();
                        if (_pageNumberFormat != "")
                            txtNum = string.Format(_pageNumberFormat, i);
                        if (i == _curPage)
                        {
                            str += string.Format("<strong>{0}</strong>", txtNum);
                        }
                        else
                        {
                            if (!_useUrl)
                            {
                                str += string.Format("<a href='javascript:void(0)' onclick='javascript:{0}({1})' title='第{1}页'>{1}</a>", _commandScript, txtNum);
                            }
                            else
                            {
                                str += string.Format("<a href='{0}{1}' title='第{1}页'>{1}</a>", _currentUrl, txtNum);
                            }
                        }
                    }

                    if (_pageCount > (begin + 1) * _pageNumbersCount)
                    {
                        if (!_useUrl)
                        {
                            str += string.Format("<a href='javascript:void(0)' onclick='javascript:{0}({1})' title='下{2}页'>...</a>", _commandScript, _pageNumbersCount * (begin + 1) + 1, _pageNumbersCount);
                        }
                        else
                        {
                            str += string.Format("<a href='{0}{1}' title='下{2}页'>...</a>", _currentUrl, _pageNumbersCount * (begin + 1) + 1, _pageNumbersCount);
                        }
                    }
                }
                else//不足指定页数
                {
                    if (_recordCount == 0) str = "";
                    else
                    {
                        for (int i = 1; i <= _pageCount; i++)
                        {
                            txtNum = i.ToString();
                            if (_pageNumberFormat != "")
                                txtNum = string.Format(_pageNumberFormat, i);
                            if (i == _curPage)
                            {
                                str += string.Format("<strong>{0}</strong>", txtNum);
                            }
                            else
                            {
                                if (!_useUrl)
                                {
                                    str += string.Format("<a href='javascript:void(0)' onclick='javascript:{0}({1})' title='第{1}页'>{1}</a>", _commandScript, txtNum);
                                }
                                else
                                {
                                    str += string.Format("<a href='{0}{1}' title='第{1}页'>{1}</a>", _currentUrl, txtNum);
                                }
                            }
                        }
                    }
                }
            }
            if (_showNextPage)
            {
                if (_curPage < _pageCount && _pageCount > 1)
                {
                    if (!_useUrl)
                    {
                        str += string.Format("<a href='javascript:void(0)' onclick='javascript:{0}({1})' title='下一页'>{2}</a>", _commandScript, _curPage + 1, _nextPageText);
                    }
                    else
                    {
                        str += string.Format("<a href='{0}{1}' title='下一页'>{2}</a>", _currentUrl, _curPage + 1, _nextPageText);
                    }
                }
                else
                {
                    //str += string.Format("{0}", _nextPageText);
                }
            }
            if (_showLastPage)
            {
                if (_curPage < _pageCount && _pageCount > 1)
                {
                    if (!_useUrl)
                    {
                        str += string.Format("<a href='javascript:void(0)' onclick='javascript:{0}({1})' title='最后一页'>{2}</a>", _commandScript, _pageCount, _lastPageText);
                    }
                    else
                    {
                        str += string.Format("<a href='{0}{1}' title='最后一页'>{2}</a>", _currentUrl, _pageCount, _lastPageText);
                    }
                }
                else
                {
                    //str += string.Format("{0}", _lastPageText);
                }
            }
            if (_showPageCount)
            {
                str += string.Format("<span class='pagecount'>" + _pageCountText + "</span>", _curPage, _pageCount);
            }
            if (_showRecordCount)
            {
                str += string.Format("<span class='recordcount'>" + _recordCountText + "</span>", _recordCount);
            }
            if (_orderItems != null && _orderValues != null)
            {
                str += "<span>按";
                str += string.Format("<select id='sortid' name='sortid' onchange='javascript:{0}({1},this)'>", _orderScript, _orderParams);
                string selected = "";
                for (int i = 0; i < _orderItems.Length; i++)
                {
                    if (_orderValues[i] == _selectedValue) selected = "selected";
                    else selected = "";
                    str += string.Format("<option value='{0}' {1}>{2}</option>", _orderValues[i], selected, _orderItems[i]);
                }
                str += "</select>";
                str += "排序</span>";
            }

            str += "</div>";

            return str;
        }
    }
}