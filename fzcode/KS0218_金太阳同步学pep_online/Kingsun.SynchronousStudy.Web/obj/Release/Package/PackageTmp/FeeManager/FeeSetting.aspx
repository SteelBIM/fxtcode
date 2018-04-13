<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FeeSetting.aspx.cs" Inherits="Kingsun.AppLibrary.Web.FeeManager.FeeSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>计费设置</title>
    <script src="../Scripts/Common/jquery-easyui/jquery.min.js"></script>

    <script src="../Scripts/Common/jquery.json-2.4.js"></script>

    <script src="../Scripts/Common/jquery-easyui/jquery.easyui.min.js"></script>

    <script src="../Scripts/Common/easyuiCommon.js"></script>
    <link href="../Scripts/Common/jquery-easyui/themes/icon.css" rel="stylesheet" />
    <link href="../Scripts/Common/jquery-easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/Common/Common.js"></script>
    <script src="../Scripts/Common/Constant.js"></script>
    <script src="../Scripts/Management/FeeSettingManagement.js" type="text/javascript"></script>
    <script src="../Scripts/Init/FeeSettingPage.js" type="text/javascript"></script>
    <style>
        .adddiv {
            font-size: 14px;
        }

            .adddiv table {
            }

                .adddiv table tr {
                    height: 30px;
                }

                    .adddiv table tr.tip {
                        height: 20px;
                    }

            .adddiv .textbox {
                width: 240px;
                height: 26px;
                line-height: 17px;
                padding: 4px 3px;
            }

            .adddiv span.tip {
                font-size: 12px;
                color: grey;
                font-style: italic;
            }

        .grouppowertb {
            border: 1px solid #95B8E7;
        }

            .grouppowertb tr td.bottom {
                border-bottom: 1px solid #95B8E7;
                width: 80px;
            }

        .trpassword {
            display: none;
        }

        .uploadify {
            display: inline-block;
        }

        .uploadify-queue {
            display: none;
        }

        /*收费权限控制*/
        #tdpower {
        }

            #tdpower div.divpwoer {
                line-height: 25px;
                padding: 5px;
            }

            #tdpower div span.spanpwoer {
                width: 80px;
                display: block;
                float: left;
                margin-right: 10px;
                text-align: right;
            }

            #tdpower div select {
            }
    </style>
    <script type="text/javascript">
        var feepage;
        $(function () {
            feepage = new FeeSettingPage();
            feepage.Init();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="divcontent">
            <table id="tbfeesetting">
            </table>
            <div id="tb">
                <a href="javascript:void(0)" id="addFee" class="easyui-linkbutton" data-options="iconCls:'icon-add'">新增</a>
            </div>
        </div>
        <div id="adddiv">
            <div class="adddiv" style="width: 280px; height: 80px; font-size: 16px; margin: 10px;">
                <table style="margin: 20px auto; width: 430px;">


                    <tr>
                        <td>套餐类型：
                        </td>
                        <td>
                            <select id="combotype">
                            </select>
                        </td>
                    </tr>

                    <tr>
                        <td>对应版本：
                        </td>
                        <td>
                            <select id="selecttype">
                            </select>
                        </td>
                    </tr>
                    <input id="appid" type="hidden" />
                    <input id="edid" type="hidden" />
                    <%-- <tr>
                        <td style="width: 35%">教程版本：
                        </td>
                        <td style="width: 65%">
                            <input type="text" class="textbox" id="dafeename" value="" readonly="readonly" />
                        </td>
                    </tr>--%>
                     <tr>
                        <td>套餐名：
                        </td>
                        <td>
                         <input type="text" class="textbox" placeholder="套餐名称" id="FeeName" value="" />
                        </td>
                    </tr>
                    <tr class="tip">
                        <td></td>
                        <td>
                            <span class="tip"></span>
                        </td>
                    </tr>



                    <tr>
                        <td>套餐所属：
                        </td>
                        <td>
                            <select id="select1">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>套餐ID(对应苹果套餐ID,安卓套餐不填)：
                        </td>
                        <td>
                            <input type="text" class="textbox" id="appName" value="" />
                        </td>
                    </tr>

                    <tr class="tip">
                        <td></td>
                        <td>
                            <span class="tip"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>套餐时间（月）：
                        </td>
                        <td>
                            <input type="text" class="textbox" placeholder="套餐时间" maxlength="4" id="dafeetime"
                                value="" />
                        </td>
                    </tr>
                    <tr class="tip">
                        <td></td>
                        <td>
                            <span class="tip"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>价格（元）：
                        </td>
                        <td>
                            <input type="text" class="textbox" placeholder="套餐价格" maxlength="8" id="daprice"
                                value="" />
                        </td>
                    </tr>
                    <tr>
                        <td>折扣价（元）：
                        </td>
                        <td>
                            <input type="text" class="textbox" placeholder="套餐价格" maxlength="8" id="discount"
                                value="" />
                        </td>
                    </tr>

                     <tr>
                        <td>活动背景图：
                        </td>
                        <td>
                            <input type="text" class="textbox" placeholder="活动背景图" maxlength="8" id="acimg"
                                value="" />
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
        <!--初始化加载页面-->
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
        </script>
    </form>
</body>
</html>
