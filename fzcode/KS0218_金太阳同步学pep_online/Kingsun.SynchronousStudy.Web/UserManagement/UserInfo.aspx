<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.UserManagement.UserInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>用户列表</title>
    <script src="../Scripts/Common/jquery-1.4.4.min.js"></script>
    <script src="../Scripts/Common/jquery.cookie.js"></script>
    <script src="../Scripts/Common/jquery.json-2.4.js"></script>
    <script src="../Scripts/Common/jquery-easyui/jquery.easyui.min-1.4.js"></script>
    <link href="../Scripts/Common/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <script src="../Scripts/Common/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../Scripts/Common/Common.js"></script>
    <script src="../Scripts/Common/Constant.js"></script>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <script src="../AppTheme/WdatePicker.js"></script>
</head>
<body>
    <div>
        <h4>同步学平台>>用户管理>>用户列表</h4>
    </div>

    <form id="form1" runat="server">

        <div style="margin-left: 20px; margin-right: 20px;">
            <div>
                <table style="width: 100%;">
                    <tr>
                        <td style="text-align: left;">
                            <a href="#" class="easyui-linkbutton" id="kvipsbtn" style="outline-style: none;">开通会员</a>
                        </td>
                        <td style="text-align: right;">
                            <asp:DropDownList ID="ddlVersion" runat="server">
                                
                            </asp:DropDownList>
                            <input type="text" id="txtSearchKey" style="color: Gray" value="请输入要查询的用户名或者手机号" />
                            注册时间段：<input id="txtStartDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndDate\')}', dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                            <input id="txtEndDate" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'txtStartDate\')}', dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                            <a href="#" class="easyui-linkbutton" id="btnSearch" style="outline-style: none;">搜索</a>
                        </td>
                    </tr>
                </table>
            </div>

            <div>

                <table id="tbdatagrid">
                </table>
            </div>
        </div>
    </form>


    <div id="divAddVersion">
        <input type="hidden" id="hidCourseidSelect" value="" />
        <table cellpadding="3" style="margin: 20px;">
            <tr>
                <td style="text-align: right;">开通手机号码<span style="color: Red">*</span>：
                </td>
                <td>
                    <input type="text" style="" id="phone" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">开通套餐类型<span style="color: Red">*</span>：
                </td>
                <td>
                    <select id="type" name="select_subject" class="easyui-combobox" data-options="valueField:'id',textField:'text',editable:false">
                        <option label="—请选择套餐—" value="">—请选择套餐—</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">开通月份<span style="color: Red">*</span>：
                </td>
                <td>
                    <input type="text" style="" id="months" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <input type="button" class="easyui-linkbutton" value="开  通" id="save" style="width: 80px; height: 24px; margin-left: 0px;" />

                </td>
            </tr>
        </table>
    </div>
    <script src="../AppTheme/js/jquery-easyui/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="../Scripts/Management/FeeSettingManagement.js"></script>
    <script src="../Scripts/UserInfoInit.js"></script>
</body>
</html>
