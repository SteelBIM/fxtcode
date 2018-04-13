<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleList.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ModuleManagement.ModuleList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>模块列表</title>
    <link href="../AppTheme/themes/default/easyui.css" rel="stylesheet" />
    <link href="../AppTheme/themes/icon.css" rel="stylesheet" />
    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>

    <script src="../AppTheme/js/jquery.easyui.min.js"></script>
</head>
<body id="body">
    <form id="myform" runat="server">
        <div>
            <div>
                <h4>同步学平台>>模块管理>>模块列表</h4>
            </div>
            <input id="commitData" name="commitData" type="hidden" />
            <table id="dg" class="easyui-treegrid" style="height: auto;">
                <thead>
                    <tr>
                        <th data-options="field:'ModularName',width:465,editor:'text'">名称</th>
                        <th data-options="field:'_operate',width:265,align:'center',formatter:formatOper">操作</th>
                    </tr>
                </thead>
            </table>
        </div>
    </form>

    <script type="text/javascript">
        var lastIndex = ""; //处于编辑状态的节点的索引值
        var selectedSelfid = "";
        $('#dg').treegrid({
            url: '/Handler/GetModuleList.ashx',
            idField: 'ModularID',
            treeField: 'ModularName',
            //iconCls: 'icon-edit',
            // method: 'Post',
            rownumbers: true,
            animate: true,
            collapsible: true,
            fitColumns: true,
            width: $(window).width() - 20,
            height: $(window).height() - 95,
            showFooter: true,
            //onDblClickCell: function (field, row) {
            //    lastIndex = row.ModularID;
            //    $('#dg').treegrid('beginEdit', row.ModularID); //进行编辑
            //    init();
            //},
            //onClickRow: function (row) {
            //    $('#dg').treegrid('endEdit', lastIndex);
            //    selectedSelfid = row.ModularID;
            //    lastIndex = ""; //退出编辑后设为空 
            //    //save(row.id, row.ModularName, row.parentId, row.ModularID);
            //    init();
            //},

            loadFilter: function (rows) {
                return convert(rows);
            },
            onLoadSuccess: init
        });

        function init() {
            //去掉结点前面的文件及文件夹小图标
            $(".tree-icon,.tree-file").removeClass("tree-icon tree-file");
            $(".tree-icon,.tree-folder").removeClass("tree-icon tree-folder tree-folder-open tree-folder-closed");
            $("datagrid-row datagrid-row-checked datagrid-row-selected").removeClass("datagrid-row-selected");
        }

        function convert(rows) {
            function exists(rows, parentId) {
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].ModularID == parentId)
                        return true;
                }
            }

            var nodes = [];
            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];
                if (!exists(rows, row.SuperiorID)) {
                    nodes.push({
                        id: row.ID,
                        ModularID: row.ModularID,
                        ModularName: row.ModularName,
                        parentId: row.SuperiorID,
                        Level: row.Level
                    });
                }
            }

            var toDo = [];
            for (var i = 0; i < nodes.length; i++) {
                toDo.push(nodes[i]);
            }
            while (toDo.length) {
                var node = toDo.shift();
                for (var i = 0; i < rows.length; i++) {
                    var row = rows[i];
                    if (row.SuperiorID == node.ModularID) {
                        var child = { id: row.ID, ModularID: row.ModularID, ModularName: row.ModularName, parentId: row.SuperiorID };
                        if (node.children) {
                            node.children.push(child);
                        } else {
                            node.children = [child];
                        }
                        toDo.push(child);
                    }
                }
            }
            return nodes;
        }

        function formatOper(val, row) {
            var html = "";

            html += '<a href="#" id="edit_' + row.ModularID + '" onclick="edit()">编辑</a>&nbsp;';
            //html += '<a href="#" onclick="cancel(' + row.ModularID + ')">撤销</a>&nbsp;';  
            html += '<a href="#" style="display:none;"  id="' + row.ModularID + '" onclick="save(\'' + row.id + '\',\'' + row.ModularName + '\',' + row.parentId + ',' + row.ModularID + ')">完成</a>&nbsp;';
            html += '<a href="#" onclick="addnode()">添加同级模块</a>&nbsp;';
            html += '<a href="#" onclick="addchildnode()">添加子模块</a>&nbsp;';
            var children = $('#dg').treegrid('getChildren', row.ModularID);
            if ((row.parentId != 0 || children.length <= 0) && row.Level != "A") {
                html += '<a href="#" onclick="deletenode(' + row.ModularID + ')">删除</a>&nbsp;';
            }
            return html;
        }



        function cancel(Mid) {
            $('#dg').treegrid('cancelEdit', Mid);
        }

        function appendNode(parentId, id, superiorId, modularName) {
            $('#dg').treegrid('append', {
                parent: parentId,  //不填此项默认创建根环节
                data: [{
                    ID: id,
                    ModularID: id,
                    SuperiorID: superiorId,
                    ModularName: modularName
                }]
            });
            $('#dg').treegrid('beginEdit', id);
            $("#" + id).css('display', 'block');
            $("#edit_" + id).css('display', 'none');
        }

        //添加同级节点
        function addnode() {
            if (lastIndex != "") {
                $.messager.alert("系统提示", "请先退出当前环节的编辑！");
                return false;
            }
            var row = $('#dg').treegrid('getSelected');
            var root = $('#dg').treegrid('getRoot');
            if (root != null && row == null) {
                $.messager.alert("系统提示", "请先选中环节！");
                return;
            }

            if (root == null) {
                //无任何环节的情况下，添加根环节
                appendNode('0', '1', '0', '1');
            }
            if (row != null) {

                var level;//= $('#dg').treegrid('getLevel', row.ModularID);
                if (row._parentId == null) {
                    level = 1;
                } else {
                    level = 2;
                }
                var i;
                if (level == 1) {
                    //存在其他根环节的情况下，添加根环节
                    var roots = $('#dg').treegrid('getRoots');
                    //遍历所有同级子环节，求得同级子环节的最大值；
                    var selfmax = parseInt(row.ModularID);
                    for (var i = 0; i < roots.length; i++) {
                        if (selfmax < parseInt(roots[i].ModularID))
                            selfmax = parseInt(roots[i].ModularID);
                    }
                    appendNode('0', selfmax + 1, '0', selfmax + 1);
                } else {
                    //除了根环节外，添加同级环节
                    var parent = $('#dg').treegrid('getParent', row.ModularID);
                    //遍历所有同级子环节，求得同级子环节的最大值；
                    var selfmax = parseInt(row.ModularID);
                    var children = $('#dg').treegrid('getChildren', parent.ModularID);
                    for (var i = 0; i < children.length; i++) {
                        if (level == $('#dg').treegrid('getLevel', children[i].ModularID)) {
                            if (selfmax < parseInt(children[i].ModularID))
                                selfmax = parseInt(children[i].ModularID);
                        }
                    }
                    //添加环节
                    appendNode(row.ModularID, selfmax + 1, parent.ModularID, selfmax + 1);
                }
            }
        }

        //添加子级环节
        function addchildnode() {
            if (lastIndex != "") {
                $.messager.alert("系统提示", "请先退出当前环节的编辑！");
                return false;
            }

            var row = $('#dg').treegrid('getSelected');
            if (row == null) {
                alert("请选中环节");
                return false;
            }
            //var ss = $('#dg').treegrid('getLevel', row.id);
            var level;//= $('#dg').treegrid('getLevel', row.ModularID);
            if (row._parentId == null || row._parentId == 0) {
                level = 1;
            } else {
                level = 2;
            }
            if (level === 2) {
                $.messager.alert("系统提示", "最多只能有二层环节，已是第二层环节！");
                return false;
            }
            var children = $('#dg').treegrid('getChildren', row.ModularID);
            var selfmax = row.ModularID * 100;
            if (children.length > 0) {
                //遍历所有同级子环节，求得同级子环节的最大值；
                var childlevel = level + 1;
                for (var i = 0; i < children.length; i++) {
                    if (childlevel == $('#dg').treegrid('getLevel', children[i].ModularID)) {
                        if (selfmax < parseInt(children[i].ModularID))
                            selfmax = parseInt(children[i].ModularID);
                    }
                }
            } else {
                selfmax = parseInt(row.ModularID) * 100;
            }
            //添加环节
            appendNode(row.ModularID, selfmax + 1, row.ModularID, selfmax + 1);
        }

        //删除节点
        function deletenode(mid) {
            if (lastIndex != "") {
                $.messager.alert("系统提示", "请先退出当前环节的编辑！");
                return false;
            }
            var row = $('#dg').treegrid('getSelected');

            var level = $('#dg').treegrid('getLevel', mid);
            if (null != row || level === 1) {
                $.messager.confirm("系统提示", "您确认要删除该模块吗？", function (r) {
                    if (r) {
                        $.ajax({
                            async: false,
                            cache: false,
                            type: "post",
                            url: "/Handler/ModuleData.ashx",
                            data: {
                                methodData: "DeleteModule",
                                Mid: mid
                            },
                            success: function (data) {
                                if (data.Result == "false") {
                                    $.messager.alert("系统提示", data.msg);
                                    return;
                                } else {
                                    $('#dg').treegrid('remove', mid);
                                    $.messager.alert("系统提示", data.msg);
                                }
                            },
                            error: function () { }
                        });
                    }
                });
            } else {
                $.messager.alert("系统提示", "请先选中环节！");
                return false;
            }
        }

        //编辑
        var editingId;
        function edit() {
            if (editingId != undefined) {
                $('#dg').treegrid('select', editingId);
                $.messager.alert("系统提示", "请先退出当前环节的编辑！");
                return;
            }
            document.onkeypress = function () {
                if (event.keyCode == 13) {
                    return false;
                }
            }
            var row = $('#dg').treegrid('getSelected');
            if (row) {
                //lastIndex = row.id;
                editingId = row.ModularID;
                $('#dg').treegrid('beginEdit', editingId);
                $("#" + row.ModularID).css('display', 'block');
                $("#edit_" + row.ModularID).css('display', 'none');
            }
        }

        //保存
        function save(id, name, parentId, modularid) {
            $("#edit_" + modularid).css('display', 'block');
            $("#" + modularid).css('display', 'none');

            var c = new Array("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z");
            var rootSort = "";
            var sort = "";
            if (lastIndex !== "") {
                $.messager.alert("系统提示", "请先退出当前环节的编辑！");
                return false;
            }
            var level;//= $('#dg').treegrid('getLevel', modularid);

            var root = $('#dg').treegrid('getRoots');
            var roots = $('#dg').treegrid('getParent', modularid);
            if (roots == null) {
                level = 1;
            } else {
                level = 2;
            }
            if (level == 1) {
                for (var i = 0; i < root.length; i++) {
                    if (modularid == root[i].ModularID) {
                        sort = c[i];
                    }
                }
            }

            if (level == 2) {
                for (var i = 0; i < root.length; i++) {
                    if (parentId == root[i].ModularID) {
                        rootSort = c[i];
                    }
                }
                for (var i = 0; i < roots.children.length; i++) {
                    if (modularid == roots.children[i].ModularID) {
                        sort = rootSort + c[i];
                    }
                }
            }

            var data;
            if (roots !== null) {
                data = {
                    methodData: "SaveModule",
                    ID: id,
                    ModularName: $('#datagrid-row-r2-2-' + modularid + ' input').val(),
                    SuperiorID: parentId,
                    ModularID: modularid,
                    ParentModularID: roots.ModularID,
                    ParentID: roots.parentId,
                    rootLevel: rootSort,
                    Level: sort,
                    ParentName: roots.ModularName
                };
            } else {
                data = {
                    methodData: "SaveModule",
                    ID: id,
                    Level: sort,
                    ModularID: modularid,
                    ModularName: $('#datagrid-row-r2-2-' + modularid + ' input').val(),
                    SuperiorID: parentId
                }
            }

            $.ajax({
                async: false,
                cache: false,
                type: "post",
                url: "/Handler/ModuleData.ashx",
                data: data,
                success: function (data) {
                    if (data.Result == "false") {
                        $.messager.alert("系统提示", data.msg);
                        return;
                    } else {
                        $('#dg').treegrid('endEdit', modularid);
                        editingId = undefined;
                        init();

                        $.messager.alert("系统提示", data.msg);
                    }
                },
                error: function () { }
            });
        }
    </script>

</body>
</html>
