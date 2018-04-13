<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CouponList.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.Order.CouponList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>优惠卷管理</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <style type="text/css">
        #divAddCoupon table tr td {
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <div>
                    <h4>同步学平台>>优惠卷管理>>优惠卷列表</h4>
                </div>
                <div class="toolBar" id="bookID">
                    <div class="rightTools">
                        <table>
                            <tr>
                                <td>
                                    <a href="javascript:void(0)" id="addCoupon" class="autobtn">添加优惠卷</a>
                                </td>
                                <td>
                                    <input type="text" id="searchValue" value="查询关键字" />
                                    <a href="javascript:void(0)" id="search">查询</a>
                                    <div style="display: none;">
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tbdatagrid"></div>
            </div>
            <%--添加DIV--%>
            <div id="divAddCoupon" style="display: none;">
                <table>
                    <tr>
                        <td>版本:</td>
                        <td>
                            <select id="selectEdition">
                            </select>
                        </td>
                        <td>卷类型:</td>
                        <td>
                            <select id="selectType">
                                <option value="0">0元购</option>
                                <option value="1">抵用卷</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>优惠卷名:</td>
                        <td>
                            <input type="text" id="txtName" />
                        </td>
                        <td>额度:</td>
                        <td>
                            <input type="text" id="txtPrice" />
                        </td>
                    </tr>
                    <tr>
                        <td>开始时间:</td>
                        <td>
                            <input id="txtStartDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                        <td>结束时间:</td>
                        <td>
                            <input id="txtEndDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                    </tr>
                    <tr>
                        <td class="filed">优惠卷图片：</td>
                        <td>
                            <input type="file" id="UploadImg" name="UploadImg" />
                        </td>
                        <td>卷状态:</td>
                        <td>
                            <select id="selectStatus">
                                <option value="0">正式使用</option>
                                <option value="1">过期</option>
                            </select>
                        </td>
                    </tr>
                </table>
            </div>
            <%--修改DIV--%>
            <div id="divUpdateCoupon" style="display: none;">
                <table style="padding:20px; width:100%; height:50%;">
                    <tr>
                        <td>开始时间:</td>
                        <td>
                            <input id="txtUpdateStartDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                        <td>结束时间:</td>
                        <td>
                            <input id="txtUpdateEndDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </td>
                    </tr>
                    <tr>
                        <td>卷状态:</td>
                        <td>
                            <select id="selectUpdateStatus">
                                <option value="0">正式使用</option>
                                <option value="1">过期</option>
                            </select>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../Scripts/Common/ajaxfileupload.js"></script>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../AppTheme/js/jquery.json-2.4.js"></script>
    <script src="../AppTheme/js/jquery.cookie.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../AppTheme/js/Common.js"></script>
    <script src="../AppTheme/js/fzcontrols/jquery.form.controls.js"></script>
    <script src="../AppTheme/js/FzJsControl/Plugins/Kingsun.Select.js"></script>
    <script src="../AppTheme/WdatePicker.js"></script>
    <script src="../Scripts/CouponList.js"></script>
</body>
</html>
