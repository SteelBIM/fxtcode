using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace FXT.DataCenter.Infrastructure.Common.CommonWeb
{
    /// <summary>
    /// 页面相关辅助类
    /// </summary>
    public class PageHelper
    {
        #region 页面显示字符串时，处理换行空格问题

        /// <summary>
        /// 处理页面显示时，换行、空格等HTML元素问题
        /// </summary>
        /// <param name="content">原内容</param>
        /// <returns>替换后的内容</returns>
        public static string ToHtmlString(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = content.Replace("\r", "<br/>");
                content = content.Replace(" ", "&nbsp;");

                return content;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region 文本分页处理方法

        /// <summary>
        /// 对文本进行分页(需要文本分页的页面不能添加IsPostBack的判断)
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="lblPageInfo">分页显示区</param>
        /// <param name="pageSize">一页文字数量</param>
        /// <param name="txtTextPageNo">存放页码的隐藏域</param>
        /// <returns>分页后的内容</returns>
        public static string TextPage(string content, Label lblPageInfo,int pageSize,HiddenField txtTextPageNo)
        {
            string strReturn = "";
            //文章每页大小
            int tempPageSize = pageSize;
            //设置第一页为初始页
            int currentPage = 1;
            // 总页数
            int totalPage = 0;
            //文章长度
            int articlelength = content.Length;

            if (tempPageSize < articlelength)
            {
                //如果每页大小大于文章长度时就不用分页了
                if (articlelength % tempPageSize == 0)
                {
                    totalPage = articlelength / tempPageSize;
                }
                else
                {
                    totalPage = articlelength / tempPageSize + 1;
                }
                if ( !string.IsNullOrEmpty(txtTextPageNo.Value))
                {
                    // 设置当前页码
                    try
                    {
                        //处理不正常的地址栏的值
                        currentPage = Convert.ToInt32(txtTextPageNo.Value);
                        if (currentPage > totalPage)
                            currentPage = totalPage;
                        if (currentPage <= 0)
                            currentPage = 1;
                    }
                    catch
                    {
                    }
                }
                // 设置获取当前页的大小
                if (currentPage < totalPage)
                {
                    tempPageSize = currentPage < totalPage ? tempPageSize : (articlelength - tempPageSize * (currentPage - 1));
                    strReturn += content.Substring(tempPageSize * (currentPage - 1), tempPageSize);
                }
                else if (currentPage == totalPage)
                {
                    int mm_intPageSize = articlelength - tempPageSize * (currentPage - 1);
                    strReturn += content.Substring(articlelength - mm_intPageSize);
                }

                string strPageInfo = "";
                for (int i = 1; i <= totalPage; i++)
                {
                    if (i == currentPage)
                        strPageInfo += "[" + i + "]";
                    else
                        strPageInfo += " <a href='javascript:setTextPageNo(" + i + ")'>[" + i + "]</a> ";
                }
                if (currentPage > 1)
                    strPageInfo = "<a href='javascript:setTextPageNo(" + (currentPage-1) + ")'>上一页</a>" + strPageInfo;
                if (currentPage < totalPage)
                    strPageInfo += "<a href='javascript:setTextPageNo(" + (currentPage + 1) + ")'>下一页</a>";
                //输出显示各个页码
                lblPageInfo.Text = "<p></p>" + strPageInfo;

                // 向页面添加脚本
                ScriptHelper.RegisterClientScriptBlock("function setTextPageNo(num) { document.getElementById('" + txtTextPageNo.ClientID + "').value = num; document.forms[0].submit(); }");
            }
            else
            {
                strReturn += content;
            }
            return strReturn;
        }


        #endregion
    }
}
