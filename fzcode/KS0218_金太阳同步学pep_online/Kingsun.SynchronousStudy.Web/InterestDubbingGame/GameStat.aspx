<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GameStat.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.InterestDubbingGame.GameStat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>配音赛事统计</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <div>
                    <h4>同步学平台>>配音赛事统计>>配音赛事统计</h4>
                </div>
                <div class="toolBar">
                    <div class="rightTools">
                        <table>
                            <tr>
                                <td>
                                    <input id="start" placeholder="请选择开始时间" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'end\')}' })" />
                                    <input id="end" placeholder="请选择结束时间" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'start\')}' })" />
                                </td>
                                <td>

                                    <a href="javascript:void(0)" id="search" class="easyui-linkbutton"
                                        data-options="iconCls:'icon-search'">搜索</a>

                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tbdatagrid"></div>
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
    <script src="../Scripts/Stat.js?v=1.1"></script>
    <script type="text/javascript">
        $(function () {
            $("#divsetting table tr td:odd").attr("style", "text-align:right;padding-right:5px;");
        });
    </script>
</body>
</html>
