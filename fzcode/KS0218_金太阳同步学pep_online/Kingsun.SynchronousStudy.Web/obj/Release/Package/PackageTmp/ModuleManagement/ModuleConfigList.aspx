<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleConfigList.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ModuleManagement.ModuleConfigList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>目录列表</title>
    <script src="../Scripts/Common/jquery-easyui/jquery.min.js"></script>

    <script src="../Scripts/Common/jquery.json-2.4.js"></script>

    <script src="../Scripts/Common/jquery-easyui/jquery.easyui.min.js"></script>

    <script src="../Scripts/Common/easyuiCommon.js"></script>
    <link href="../Scripts/Common/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <link href="../Scripts/Common/jquery-easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/Common/Common.js"></script>
    <script src="../Scripts/Common/Constant.js"></script>
    <script src="../Scripts/Management/ModuleConfigManagement.js" type="text/javascript"></script>
    <script src="../Scripts/Init/ModuleConfigListPage.js" type="text/javascript"></script>
    <script type="text/javascript">
        var Modulepage;
        $(function () {
            Modulepage = new ModuleConfigListPage();
            Modulepage.Init();
        });
    </script>
</head>
<body id="body">
    <form id="form1" runat="server">
         <div class="divcontent">
            <table id="tbConfigList">
            </table>
             <div id="tb">
                <a href="javascript:void(0)" id="addModModule"  class="easyui-linkbutton" data-options="iconCls:'icon-add'">更新目录</a>
                
            </div>
        </div>

        <div id="adddiv">
            <div class="adddiv" style="width: 280px; height: 80px; font-size: 16px; margin: 10px;">
                <table style="margin: 20px auto; width: 430px;">
                    <tr>
                        <td>一级目录：
                        </td>
                        <td>
                              <input type="text" class="textbox" id="FirstTitle" value="" style="width: 350px;" />
                        </td>
                    </tr>

                    <tr>
                        <td>二级目录：
                        </td>
                        <td>
                             <input type="text" class="textbox" id="SecondTitle" value="" style="width: 350px;" />
                        </td>
                    </tr>

                    <tr class="tip">
                        <td></td>
                        <td>
                            <span class="tip"></span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <div id='Loading' style="position: absolute; z-index: 1000; top: 0px; left: 0px; width: 100%; height: 100%; background: #E0ECFF; text-align: center;">
            <h1 style="top: 48%; position: relative;">
                <font color="#15428B">加载中···</font>
            </h1>
        </div>
        <script>
            function closes() { $("#Loading").fadeOut("normal", function () { $(this).remove(); }); }
            var pc;
            $.parser.onComplete = function () {
                if (pc) clearTimeout(pc);
                pc = setTimeout(closes, 100);
            }
            //获取url参数
            function getQueryString(name) {
                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
                var r = window.location.search.substr(1).match(reg);
                if (r != null) return decodeURI(r[2]); return null;
            }

            $("#addModModule").click(function () {
                Modulepage.UpdataCatalog(getQueryString("bookid"), getQueryString("bookname"));
            });
        </script>
    </form>
</body>
</html>
