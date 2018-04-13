<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GameUserList.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.InterestDubbingGame.GameUserList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>比赛用户查询</title>
    <link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <style type="text/css">
        table input {
            width: 90%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <div>
                    <h4>同步学平台>>比赛用户查询>>比赛用户查询</h4>
                </div>
                <div class="toolBar" id="bookID">
                    <div class="rightTools">
                        <table>
                            <tr>
                                <td style="padding-right: 20px;"><a href="javascript:void(0)" id="setting" onclick="setting()" class="easyui-linkbutton">设置比赛时间</a></td>

                                <td></td>
                                <td>
                                    版本:
                                    <select id="selVersionName" style="height: 25px;">
                                        <option value="0">广东版</option>
                                        <option value="1">人教版</option>
                                    </select>
                                </td>
                                 <td></td>
                                  <td>
                                 赛段:
                                    <select id="selStage" style="height: 25px;">
                                        <option value="0">---请选择---</option>
                                        <option value="1">低学段</option>
                                        <option value="2">中学段</option>
                                        <option value="3">高学段</option>
                                    </select>
                                </td>
                                 <td style="padding-left:10px;">  联系电话：</td>
                                <td>
                                  <input type="text" id="searchValue" class="easyui-validatebox" placeholder="请输入手机号码" style="min-width: 150px; height: 20px;" /></td>
                                <td>

                                    <a href="javascript:void(0)" id="search" class="easyui-linkbutton" 
                                        data-options="iconCls:'icon-search'">搜索</a>

                                </td> 
                                <td style="padding-left:100px;">
                                      <input type="button" id="exportExcel"  value=" 导出全部 "/>
                                </td>
                            </tr>
                        </table>
                     
                    </div>
                </div>
                <div id="tbdatagrid"></div>
            </div>

            <div id="divsetting" style="display: none; border: 0px solid red;">
                <input type="hidden" id="Times" />
                <table style="padding: 10px; width: 100%; height: auto; text-align: center; border: 1px dashed gray;">
                    <tr>
                        <td colspan="4" style="text-align: center; border-bottom: 1px dashed gray;">
                            <h1 style="font-size: 16px;">设置比赛时间</h1>
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>报名开始时间:</td>
                        <td>
                            <input id="SignUpStartTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'SignUpEndTime\')}' })" />
                        </td>
                        <td>报名结束时间:</td>
                        <td>
                            <input id="SignUpEndTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'SignUpStartTime\')}' })" />
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>初赛开始时间:</td>
                        <td>
                            <input id="FirstGameStartTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'FirstGameEndTime\')}' })" />
                        </td>
                        <td>初赛结束时间:</td>
                        <td>
                            <input id="FirstGameEndTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'FirstGameStartTime\')}' })" />
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>复赛开始时间:</td>
                        <td>
                            <input id="SecondGameStartTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'SecondGameEndTime\')}' })" />
                        </td>
                        <td>复赛结束时间:</td>
                        <td>
                            <input id="SecondGameEndTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'SecondGameStartTime\')}' })" />
                        </td>
                    </tr>
                    <tr>
                        <td><span style="color: red;">*</span>决赛开始时间:</td>
                        <td>
                            <input id="FinalsStartTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'FinalsEndTime\')}' })" />
                        </td>
                        <td>决赛结束时间:</td>
                        <td>
                            <input id="FinalsEndTime" runat="server" type="text" class="Wdate" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'FinalsStartTime\')}' })" />
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
    <script src="../Scripts/Match.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#divsetting table tr td:odd").attr("style", "text-align:right;padding-right:5px;");
        });
    </script>
</body>
</html>

