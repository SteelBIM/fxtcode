<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseDetails.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.CourseManagement.CourseDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>课程详情</title>

    <style>
        h2 a {
            text-decoration: none;
            color: black;
        }

        .white_content {
            display: none;
            position: absolute;
            top: 25%;
            left: 25%;
            width: 50%;
            height: 50%;
            padding: 16px;
            border: 1px solid orange;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }

        .black_overlay {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="bookID" style="display: none"></div>
        <div style="display: none" id="getFiles"><%=System.Web.Configuration.WebConfigurationManager.AppSettings["getFiles"]%></div>
        <div>
            <div>
                <h2><a href="javascript:void(0)">课程管理</a>>><a href="CourseManageMent.aspx">课程列表</a>>>课程详情</h2>
                <input type="button" id="modify" value="修改" style="float: right" />
                <input type="button" id="btnExport" value="下载资源包模板" style="float: left" />
                <input type="button" id="btnImport" value="导入资源" style="float: left" />
            </div>
            <div>
                资源包地址：<asp:DropDownList ID="ddlAddress" runat="server">
                    <asp:ListItem Value="0">http://183.47.42.221:8038/uploadfile/SyncCourse</asp:ListItem>
                    <asp:ListItem Value="1">http://tbxcdn.kingsun.cn/SynchronousStudy</asp:ListItem>
                </asp:DropDownList>
                资源包目录：<asp:TextBox ID="txtFile" runat="server"></asp:TextBox>
                模块目录：<asp:DropDownList ID="ddlModuleName" runat="server"></asp:DropDownList>
                <asp:Button ID="btnUpdate" runat="server" Text="更新" OnClick="btnUpdate_Click" />
            </div>
            <div>
                <h3 id="bookName"></h3>
            </div>
            <div>
                <table id="tbtreegrid" class="easyui-treegrid" style="width: auto;" data-options="
                url: '?action=getBookTitle',
                queryParams:{BookID:getQueryString('bookID')},
				method: 'get',
				rownumbers: true,
				idField: 'id',
				treeField: 'ModularName',
				loadFilter: myLoadFilter,
                onLoadSuccess:init">
                    <thead>
                        <tr>
                            <th field="ModularName" width="40%" style="text-align: center">模块</th>
                            <th field="Description" width="30%" style="text-align: center" formatter="GetVersionNumber">一级模块版本号</th>
                            <th field="_operate" width="30%" style="text-align: center" formatter="formatOper">操作</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
        <div id="divSort" class="white_content">
            <div>修改排序</div>
            <label id="lbFirstTitle"></label>
            <br />
            <label id="lbSecondTitle"></label>
            <table id="tbSort" style="width: 780px" class="easyui-treegrid">
                <thead>
                    <tr>
                        <th field="ModuleName" width="60%">模块名称</th>
                        <th field="ModuleID" width="40%" style="text-align: center" formatter="formatOperSort">前端排序</th>
                    </tr>
                </thead>
            </table>
            <a href="javascript:void(0)" onclick="AddModuleConfig()">确定</a>
            <a href="javascript:void(0)" onclick="closeDIV()">取消</a>
        </div>
        <div id="fade" class="black_overlay">
        </div>
        <%--添加 修改DIV--%>
        <div id="divAddCourse" style="display: none;">
            <table>
                <tr>
                    <td>
                        <span id="book"></span>
                    </td>
                </tr>
                <tr>
                    <td class="filed">上传人：</td>
                    <td>
                        <input type="text" readonly="readonly" class="single-text normal" style="width: 200px;" id="addPerson"
                            title="上传人" />
                    </td>
                </tr>
                <tr>
                    <td>添加时间:</td>
                    <td>
                        <input type="text" readonly="readonly" id="AddTime" />
                    </td>
                </tr>
                <tr>
                    <td class="filed">课程封面：</td>
                    <td id="tdCover" style="display: none">
                        <div>
                            <img alt="课程封面" id="coursesCover" src="#" style="width: 160px; height: 120px" />
                        </div>
                    </td>
                    <td>
                        <%-- <input type="file" name="file_upload" id="file_upload" />--%>
                        <input type="file" id="UploadImg" name="UploadImg" />
                        <span style="margin-left: 10px; height: 32px" id="showFileName" class="fileName"></span>
                        <input type="hidden" name="name" value="" id="fileinfo" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
    <div style="display: none" id="div_UpdateRevision">
        <iframe src="#" id="iframe_UpdateRevision" style="width: 98%; height: 98%;"></iframe>
    </div>
    <script type="text/javascript">

        function init() {
            //去掉结点前面的文件及文件夹小图标
            $(".tree-icon,.tree-file").removeClass("tree-icon tree-file");
            $(".tree-icon,.tree-folder").removeClass("tree-icon tree-folder tree-folder-open tree-folder-closed");
            $("datagrid-row datagrid-row-checked datagrid-row-selected").removeClass("datagrid-row-selected");
        }

        //获取url参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        //惰性加载数据
        function myLoadFilter(data, id) {
            var tg = $(this);
            var opts = tg.treegrid('options');
            //展开加载子节点
            opts.onBeforeExpand = function (row) {
                var parent = $('#tbtreegrid').treegrid('getLevel', row.id);
                var bookID = $("#bookID").html();
                var firstid = 0;
                var secondid = 0;
                if (row.children == undefined || row.children.length <= 0) {
                    if (row.children != undefined && row.children.length <= 0) {
                        firstid = row.id;
                    } else {
                        firstid = row._parentId;
                        secondid = row.id;
                    }
                    if (parent == 2 || parent == 1) {
                        $.ajax({
                            type: "POST",
                            url: "?action=getModularDetails&firstTitleID=" + firstid + "&secondTitleID=" + secondid + "&bookID=" + bookID,
                            cache: false,
                            async: false,
                            success: function (data) {
                                if (data != null) {
                                    tg.treegrid('append', {
                                        parent: row[opts.idField],
                                        data: $.parseJSON(data)
                                    });
                                    //$('#tbtreegrid').treegrid('expand', row.id);
                                }
                            }
                        });
                    }
                }
            }
            opts.onExpand = init;
            return data;
        }


        function formatOper(val, row) {
            var html = "";
            //isNull=="true":表示只有一级标题
            if (row.isNull == "true") {
                //因懒加载所以第三级的级别获取为0，所以投机取巧寻找第三级的上一级。如果级别为2，那本级就是第三级
                var level = $('#tbtreegrid').treegrid('getLevel', row._parentId);
                var parent = $('#tbtreegrid').treegrid('getParent', row.id);
                //如果上一级为第二级，才给第三级加载上移下移按钮。
                if (level == 0) {
                    html += '<a href="#" onclick=setModuleSort("' + getQueryString("bookid") + '","' + row.id + '","' + row._parentId + '") sort="' + row.sort + '">排序</a>&nbsp;';
                }
                if (level == 1) {
                    html += '<a href="javascript:void(0);" onclick=Update(\'' + row.ModularID + '\',\'' + parent.id + '\',\'\',\'' + getQueryString("bookid") + '\') >更新</a>&nbsp';
                    var str = CheckState(row.ModularID, parent.id, 0);
                    if (str == "") {
                        html += '<span">禁用</span>&nbsp';
                    } else {
                        html += '<a href="javascript:void(0);" onclick=ChangeState(\'' + row.ModularID + '\',\'' + parent.id + '\',\'' + 0 + '\',\'' + getQueryString("bookid") + '\')>禁用</a>&nbsp';
                    }
                    html += '<a href="javascript:void(0);" onclick=RevisionHistory(\'' + row.ModularID + '\',\'' + parent.id + '\',\'' + 0 + '\',\'' + getQueryString("bookid") + '\')>历史版本</a>&nbsp;';
                }
            } else {
                //因懒加载所以第三级的级别获取为0，所以投机取巧寻找第三级的上一级。如果级别为2，那本级就是第三级
                var level = $('#tbtreegrid').treegrid('getLevel', row._parentId);
                var parent = $('#tbtreegrid').treegrid('getParent', row.id);
                //如果上一级为第二级，才给第三级加载上移下移按钮。
                if (level == 0 && parent != null && parent._parentId == undefined) {
                    html += '<a href="#" onclick=setModuleSort("' + getQueryString("bookid") + '","' + row.id + '","' + row._parentId + '") sort="' + row.sort + '">排序</a>&nbsp;';
                }
                if (level == 2) {
                    var parent2 = $('#tbtreegrid').treegrid('getParent', row.id);
                    var parent1 = $('#tbtreegrid').treegrid('getParent', parent2.id);
                    html += '<a href="javascript:void(0);" onclick=Update(\'' + row.ModularID + '\',\'' + parent1.id + '\',\'' + parent2.id + '\',\'' + getQueryString("bookid") + '\') >更新</a>&nbsp';
                    var str = CheckState(row.ModularID, parent1.id, parent2.id);
                    if (str == "") {
                        html += '<span">禁用</span>&nbsp';
                    } else {
                        html += '<a href="javascript:void(0);" onclick=ChangeState(\'' + row.ModularID + '\',\'' + parent1.id + '\',\'' + parent2.id + '\',\'' + getQueryString("bookid") + '\')>禁用</a>&nbsp';
                    }
                    html += '<a href="javascript:void(0);" onclick=RevisionHistory(\'' + row.ModularID + '\',\'' + parent1.id + '\',\'' + parent2.id + '\',\'' + getQueryString("bookid") + '\')>历史版本</a>&nbsp;';
                }
            }
            return html;
        }

        //检查模块是否被启用
        function CheckState(modularID, firstid, secondid) {
            var msg = "";
            $.ajax({
                type: "post",
                url: "?action=getVersionNumber",
                data: { FirstTitleID: firstid, SecondTitleID: secondid, ModularID: modularID },
                async: false,
                success: function (data) {
                    if (data) {
                        var result = eval("(" + data + ")");
                        if (result != null) {
                            msg = "启用";
                        }
                    }
                }
            })
            return msg;
        }

        //更改状态
        function ChangeState(modularID, firstid, secondid, bookid) {
            if (confirm("确定禁用吗？")) {
                $.ajax({
                    type: "post",
                    url: "?action=changeState",
                    data: { FirstTitleID: firstid, SecondTitleID: secondid, ModularID: modularID },
                    async: false,
                    success: function (data) {
                        if (data) {
                            var result = eval("(" + data + ")");
                            if (result != null) {
                                if (result.obj) {
                                    location.reload();
                                } else {
                                    alert("禁用失败，请重试")
                                }
                            }
                        }
                    }
                })
            }
        }

        //更新
        //更新传值不对！读取不到一级标题二级标题的数据,改为传ID
        //FirstTitle：一级标题ID，SecondTitle：二级标题ID，ModularID:当前ID
        function Update(ModularID, FirstTitleID, SecondTitleID, BookID) {
            $('#div_UpdateRevision').attr("style", "display:block");
            $('#div_UpdateRevision').dialog({
                title: '版本更新',
                width: 600,
                height: 550,
                closed: false,
                cache: false,
                modal: true,
                buttons: [
                ]
            });
            $("#iframe_UpdateRevision").attr("src", "ModuleUpdate.aspx?ModularID=" + ModularID + "&FirstTitleID=" + FirstTitleID + "&SecondTitleID=" + SecondTitleID + "&BookID=" + BookID);
        }

        //查看历史版本
        //FirstTitle：一级标题ID，SecondTitle：二级标题ID，ModularID:当前ID
        function RevisionHistory(ModularID, FirstTitleID, SecondTitleID, BookID) {
            $('#div_UpdateRevision').attr("style", "display:block");
            $('#div_UpdateRevision').dialog({
                title: '历史版本',
                width: 900,
                height: 450,
                closed: false,
                cache: false,
                modal: true,
                closeOnEscape: false,
                open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); },
                buttons: [
                    {
                        text: '关闭',
                        handler: function () {
                            $("#div_UpdateRevision").dialog('close');
                        }
                    }
                ]
            });
            $("#iframe_UpdateRevision").attr("src", "RevisionHistory.aspx?ModularID=" + ModularID + "&FirstTitleID=" + FirstTitleID + "&SecondTitleID=" + SecondTitleID + "&BookID=" + BookID);
        }

        //关闭版本更新对话框
        function CloseDialog() {
            $('#div_UpdateRevision').dialog('close');
            location.reload();
        }

        function GetVersionNumber(val, row) {
            var html = "";
            var level = $('#tbtreegrid').treegrid('getLevel', row._parentId);
            var parent2 = $('#tbtreegrid').treegrid('getParent', row.id);
            var parent1;
            if (parent2 != null) {
                parent1 = $('#tbtreegrid').treegrid('getParent', parent2.id);
            }
            if (level == 2 || (level == 1 && parent1 == null)) {
                var firstid = 0;
                var secondid = 0;
                var modularID = row.ModularID;
                if (parent1 == null) {
                    firstid = parent2.id;
                    secondid = 0;
                } else {
                    firstid = parent1.id;
                    secondid = parent2.id;
                }
                $.ajax({
                    type: "post",
                    url: "?action=getVersionNumber",
                    data: { FirstTitleID: firstid, SecondTitleID: secondid, ModularID: modularID },
                    async: false,
                    success: function (data) {
                        if (data) {
                            var result = eval("(" + data + ")");
                            if (result != null) {
                                html += '<span>' + result.obj.ModuleVersion + '</span>&nbsp;';
                            }
                        }
                    }
                })
            }
            return html;
        }

        function init() {
            //去掉结点前面的文件及文件夹小图标
            $(".tree-icon,.tree-file").removeClass("tree-icon tree-file");
            $(".tree-icon,.tree-folder").removeClass("tree-icon tree-folder tree-folder-open tree-folder-closed");
            $("datagrid-row datagrid-row-checked datagrid-row-selected").removeClass("datagrid-row-selected");
        }

        //弹出排序窗口
        function setModuleSort(bookid, secondid, firstid) {
            $("#divSort").show();
            $("#fade").show();
            $('#tbSort').treegrid({
                url: '/Handler/ModuleData.ashx?methodData=GetModuleSort',
                queryParams: { bookid: bookid, firstid: firstid, secondid: secondid },
                idField: 'ModuleID',
                treeField: 'ModuleName',
                onLoadSuccess: init
            });
        }

        //关闭窗口
        function closeDIV() {
            $("#divSort").hide();
            $("#fade").hide();
        }

        //排序按钮
        function formatOperSort(val, row) {
            var html = "";
            //如果是第一个位置的不能上移。所以不显示上移按钮
            if (row.Sort != 0) {
                html += '<a href="#" onclick=move("up","' + row.ModuleID + '","' + row.Sort + '") sort="' + row.Sort + '">上移</a>&nbsp;';
            } else {
                html += '&nbsp;&nbsp;&nbsp;';
            }
            //最后一个位置不显示下移按钮
            if (row.Sort != row.count - 1) {
                html += '<a href="#" onclick=move("down","' + row.ModuleID + '","' + row.Sort + '") sort="' + row.Sort + '">下移</a>&nbsp;';
            }
            return html;
        }

        //排序
        function move(o, id, sortid) {//将此方法加入上下移的按钮事件即可  
            //id:选中行的ID，sortid：选中行的排序Id
            var parent = $("#tbSort").treegrid("getRoots", id);
            var levelid = "";
            var n2 = "";
            //上移
            if (o == "up") {
                if (parent.id == "undefined") {
                    alert("无法移动！");
                } else {
                    //循环获取当前选中节点同级的sort值
                    for (var i = 0; i < parent.length; i++) {
                        //获取选中节点的前一位节点id
                        if (parseInt(parent[i].Sort) + 1 == parseInt(sortid)) {
                            levelid = parent[i].ModuleID;
                        }
                    }
                    //pop:弹出节点并在移除该节点后返回包含其子节点的节点数据。
                    var n2 = $("#tbSort").treegrid("pop", id);
                    //因为是上移。所以当前选中行的sort减一
                    n2.Sort = parseInt(sortid) - 1;
                    //在ID为levelid之前添加弹出的节点
                    $("#tbSort").treegrid("insert", { before: levelid, data: n2 });
                    $("#tbSort").treegrid("select", id);
                    $('#tbSort').treegrid('update', {
                        id: levelid,
                        row: { Sort: sortid }
                    });
                    init();
                }
            } else if (o == "down") {//下移
                if (parent.id == "undefined") {
                    alert("无法移动！");
                } else {
                    //循环获取当前选中节点同级的sort值
                    for (var i = 0; i < parent.length; i++) {
                        //获取选中节点的前一位节点id
                        if (parseInt(parent[i].Sort) == parseInt(sortid) + 1) {
                            levelid = parent[i].ModuleID;
                        }
                    }

                    //pop:弹出节点并在移除该节点后返回包含其子节点的节点数据。
                    var n2 = $("#tbSort").treegrid("pop", id);
                    //因为是上移。所以当前选中行的sort减一
                    n2.Sort = parseInt(sortid) + 1;
                    //在ID为levelid之前添加弹出的节点
                    $("#tbSort").treegrid("insert", { after: levelid, data: n2 });
                    $("#tbSort").treegrid("select", id);
                    $('#tbSort').treegrid('update', {
                        id: levelid,
                        row: { Sort: sortid }
                    });
                    init();
                }
            }
        }

        //保存
        function AddModuleConfig() {
            var data = $("#tbSort").treegrid("getData");
            var firstid = "";
            var secondid = "";
            var bookid = "";
            var idList = "[";
            for (var i = 0; i < data.length; i++) {
                firstid = data[0].FirstTitleID;
                secondid = data[0].SecondTitleID;
                bookid = data[0].BookID;
                idList += "{\"BookID\":\"" + data[i].BookID + "\",\"FirstTitleID\":" + data[i].FirstTitleID + ",\"ID\":\"" + data[i].ID + "\",\"ModuleID\":\"" + data[i].ModuleID + "\",\"ModuleName\":\"" + data[i].ModuleName + "\",\"SecondTitleID\":\"" + data[i].SecondTitleID + "\",\"Sort\":\"" + data[i].Sort + "\",\"SuperiorID\":\"" + data[i].SuperiorID + "\"},";
            }
            idList = idList.substring(0, idList.length - 1) + "]";

            $.ajax({
                async: false,
                cache: false,
                type: "post",
                url: "/Handler/ModuleData.ashx",
                data: {
                    methodData: "UpdateModuleSort",
                    data: idList,
                    firstid: firstid,
                    secondid: secondid,
                    bookid: bookid
                },
                success: function (data) {
                    if (data.Result == "false") {
                        $.messager.alert("系统提示", data.msg);
                        return;
                    } else {
                        $.messager.alert("系统提示", data.msg);

                    }
                },
                error: function () { }
            });

            $("#divSort").hide();
            $("#fade").hide();
        }

    </script>
</body>
<link href="../AppTheme/js/jquery-easyui/themes/default/easyui.css" rel="stylesheet" />
<link href="../AppTheme/js/jquery-easyui/themes/icon.css" rel="stylesheet" />
<script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
<script src="../AppTheme/js/jquery.easyui.min.js"></script>
<script src="../AppTheme/js/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
<script src="../AppTheme/js/CommonDB.js"></script>
<link href="../AppTheme/js/uploadify/uploadify.css" rel="stylesheet" />
<script src="../AppTheme/js/uploadify/jquery.uploadify.min.js"></script>
<script src="../AppTheme/js/jquery.json-2.4.js"></script>
<script src="../AppTheme/js/jquery.cookie.js"></script>
<script src="../AppTheme/js/fzcontrols/jquery.form.controls.js"></script>
<script src="../AppTheme/js/FzJsControl/Plugins/Kingsun.Select.js"></script>
<script src="../Scripts/CourseDetails.js"></script>
<script src="../AppTheme/js/datagrid-detailview.js"></script>
<script src="../Scripts/Common/ajaxfileupload.js"></script>
</html>
