<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleConfig.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ModuleManagement.ModuleConfig" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>模块配置</title>
    <link href="/AppTheme/themes/default/easyui.css" rel="stylesheet" />
    <link href="/AppTheme/themes/icon.css" rel="stylesheet" />
    <script src="../AppTheme/js/jquery-1.8.0.min.js"></script>
    <script src="../AppTheme/js/jquery.easyui.min.js"></script>
    <script src="../AppTheme/js/Common.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../AppTheme/js/jquery.cookie.js"></script>
    <script src="../AppTheme/js/fzcontrols/jquery.form.controls.js"></script>
    <script src="../AppTheme/js/FzJsControl/Plugins/Kingsun.Select.js"></script>
    <script src="../AppTheme/js/datagrid-detailview.js"></script>
    <style>
        /*.tree-folder-open {
            background-image: none!important;
        }

        .tree-file {
            background-image: none!important;
        }*/
    </style>

</head>
<body id="body">
    <form id="myform" runat="server">
        <div>
            <div>
                <h4>同步学平台>>课程管理>>课程列表>>配置模块</h4>
            </div>
            <label id="lbBookName"></label>
            <br />
        </div>
        <div>
            <table id="dg" class="easyui-treegrid" style="height: auto;" data-options="
				url: '/Handler/ModuleData.ashx?methodData=GetModularList',
                 queryParams:{bookid:getQueryString('bookid'),ModUrl:Constant.code_mod_Url},
				method: 'post',
				rownumbers: true,
				idField: 'id',
				treeField: 'ModularName',
				loadFilter: myLoadFilter,
               onLoadSuccess:LoadConfigurationModule,
                ">
                <thead>
                    <tr>
                        <th field="ModularName" width="60%" formatter="formatcheckbox">模块名称</th>
                        <%--<th data-options="field:'ModularName',width:465,editor:'text'"></th>--%>
                        <%--<th field="_operate" width="40%" style="text-align: center" formatter="formatOper">前端排序</th>--%>
                        <%--<th data-options="field:'_operate',width:265,align:'center',formatter:formatOper">前端排序</th>--%>
                    </tr>
                </thead>
            </table>
            <a href="#" onclick="AddModuleConfig()">确定</a>
            <a href="/CourseManagement/CourseManageMent.aspx" target="_self">取消</a>
        </div>
    </form>
    <script type="text/javascript">
        var UnitID = "";
        var UnitArr = [];//单元数据
        var Section = "";
        var idList = "";
        var bookid = "";
        var ss = "";
        var moduleArr = [];//书本模块数组
        var secondTitleID = "";
        var firstTitleArr = [];
        var completedModuleArr = [];

        $(function () {
            $("#lbBookName").text(getQueryString("BookName"));
            bookid = getQueryString("bookid");

            //获取书本一、二级标题（万里20160726）
            var obj = { BookID: bookid };
            $.ajax({
                url: "/Handler/ModuleData.ashx?methodData=GetBookTitle",
                type: "POST",
                data: obj,
                async: false,
                success: function (data) {
                    if (data != null && data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            firstTitleArr.push(data[i]);
                        }
                    }
                }
            });
            $.ajax({
                url: "/Handler/ModuleData.ashx?methodData=GetCompletedModule",
                type: "POST",
                data: obj,
                async: false,
                success: function (data) {
                    if (data.result) {
                        for (var i = 0; i < data.obj.length; i++) {
                            completedModuleArr.push(data.obj[i]);
                        }
                    }
                }
            })
        });

        function init() {
            //去掉结点前面的文件及文件夹小图标
            $(".tree-icon,.tree-file").removeClass("tree-icon tree-file");
            $(".tree-icon,.tree-folder").removeClass("tree-icon tree-folder tree-folder-open tree-folder-closed");
            $("datagrid-row datagrid-row-checked datagrid-row-selected").removeClass("datagrid-row-selected");
        }

        //获取选中的结点
        function getSelected() {
            idList = "[";
            var sum = 0;
            $("input:checked").each(function () {
                var id = $(this).attr("id");
                var sortid = $(this).attr("sort");
                var mouduleid = $(this).attr("mouduleid");
                var name = $(this).attr("modularname");
                var isNull = $(this).attr("isNull");
                var Level = $('#dg').treegrid('getLevel', id.replace("check_", ''));
                var parent = $('#dg').treegrid('getParent', id.replace("check_", ''));
                if (isNull == "true") {
                    if (Level == 3) {
                        idList += "{\"id\":\"" + id.replace("check_", '') + "\",\"modularname\":\"" + name + "\",\"mouduleid\":\"" + mouduleid + "\",\"parentid\":\"" + (parent == null ? "" : parent.ModularID) + "\",\"sortid\":\"" + sortid + "\",\"level\":\"" + Level + "\",\"isNull\":\"" + isNull + "\"},";
                    } else {
                        idList += "{\"id\":\"" + id.replace("check_", '') + "\",\"modularname\":\"" + name + "\",\"mouduleid\":\"" + mouduleid + "\",\"parentid\":\"" + (parent == null ? "" : parent.id) + "\",\"sortid\":\"" + sortid + "\",\"level\":\"" + Level + "\",\"isNull\":\"" + isNull + "\"},";
                    }
                } else {
                    if (Level == 4) {
                        idList += "{\"id\":\"" + id.replace("check_", '') + "\",\"modularname\":\"" + name + "\",\"mouduleid\":\"" + mouduleid + "\",\"parentid\":\"" + (parent == null ? "" : parent.ModularID) + "\",\"sortid\":\"" + sortid + "\",\"level\":\"" + Level + "\",\"isNull\":\"" + isNull + "\"},";
                    } else {
                        idList += "{\"id\":\"" + id.replace("check_", '') + "\",\"modularname\":\"" + name + "\",\"mouduleid\":\"" + mouduleid + "\",\"parentid\":\"" + (parent == null ? "" : parent.id) + "\",\"sortid\":\"" + sortid + "\",\"level\":\"" + Level + "\",\"isNull\":\"" + isNull + "\"},";
                    }
                }

                sum++;
            })
            if (sum > 0) {
                idList = idList.substring(0, idList.length - 1) + "]";
            } else {
                idList += "]";
            }

            return idList;
        }

        //勾选节点
        function show(checkid) {
            var s = '#check_' + checkid;
            var level = $('#dg').treegrid('getLevel', checkid);
            /*选子节点*/
            var nodes = $("#dg").treegrid("getChildren", checkid);
            for (i = 0; i < nodes.length; i++) {

                $(('#check_' + nodes[i].id))[0].checked = $(s)[0].checked;
            }
            //选上级节点
            /*
            通过getParent函数获取上一级。然后循环上一级所属的children集合是否有被选中的。如果没有就取消勾选
            */
            if (!$(s)[0].checked) {
                var parent = $("#dg").treegrid("getParent", checkid);
                if (parent == null) return false;
                $('#check_' + parent.id)[0].checked = true;
                if (level == 2) {
                    var s1 = $("#dg").treegrid("getParent", checkid);
                    var count1 = 0;
                    for (i = 0; i < s1.children.length; i++) {
                        if ($('#check_' + s1.children[i].id)[0].checked) {
                            count1++;
                        }
                    }
                    if (count1 == 0) {
                        $(('#check_' + s1.id))[0].checked = false;
                    }
                }
                if (level == 3) {
                    var s2 = $("#dg").treegrid("getParent", checkid);
                    var s1 = $("#dg").treegrid("getParent", s2.id);
                    var count1 = 0;
                    var count2 = 0;
                    for (j = 0; j < s2.children.length; j++) {
                        if ($('#check_' + s2.children[j].id)[0].checked) {
                            count2++;
                        }
                    }
                    if (count2 == 0) {
                        $(('#check_' + s2.id))[0].checked = false;
                        for (i = 0; i < s1.children.length; i++) {
                            if ($('#check_' + s1.children[i].id)[0].checked) {
                                count1++;
                            }
                        }
                        if (count1 == 0) {
                            $(('#check_' + s1.id))[0].checked = false;
                        }

                    }
                }
                if (level == 4) {
                    var s3 = $("#dg").treegrid("getParent", checkid);
                    var s2 = $("#dg").treegrid("getParent", s3.id);
                    var s1 = $("#dg").treegrid("getParent", s2.id);
                    var count1 = 0;
                    var count2 = 0;
                    var count3 = 0;
                    for (j = 0; j < s3.children.length; j++) {
                        if ($('#check_' + s3.children[j].id)[0].checked) {
                            count3++;
                        }
                    }
                    if (count3 == 0) {
                        $(('#check_' + s3.id))[0].checked = false;
                        for (j = 0; j < s2.children.length; j++) {
                            if ($('#check_' + s2.children[j].id)[0].checked) {
                                count2++;
                            }
                        }
                        if (count2 == 0) {
                            $(('#check_' + s2.id))[0].checked = false
                            for (i = 0; i < s1.children.length; i++) {
                                if ($('#check_' + s1.children[i].id)[0].checked) {
                                    count1++;
                                }
                            }
                            if (count1 == 0) {
                                $(('#check_' + s1.id))[0].checked = false;
                            }

                        }
                    }
                }
            } else {
                var parent = $("#dg").treegrid("getParent", checkid);
                if (parent == null) return false;
                var flag = true;
                for (j = 0; j < parent.children.length; j++) {
                    if (!$('#check_' + parent.children[j].id)[0].checked) {
                        flag = true;
                        break;
                    }
                }
                if (flag) {
                    if (level == 3) {
                        $(('#check_' + parent.id))[0].checked = true;
                        $(('#check_' + parent._parentId))[0].checked = true;
                    } else if (level == 4) {
                        var ss = $("#dg").treegrid("getParent", parent._parentId);
                        $(('#check_' + parent.id))[0].checked = true;
                        $(('#check_' + parent._parentId))[0].checked = true;
                        $(('#check_' + ss.id))[0].checked = true;
                    }
                    else {
                        $(('#check_' + parent.id))[0].checked = true;
                    }
                }
            }
        }

        //获取url参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return decodeURI(r[2]); return null;
        }

        //加载配置模块（wanli 20160728）
        function LoadConfigurationModule() {
            var isnull = "false";
            var tg = $(this);
            var opts = tg.treegrid('options');
            if (firstTitleArr.length > 0) {
                for (var i = 0; i < firstTitleArr.length; i++) {
                    //查找指定节点并返回节点数据
                    if (firstTitleArr[i].children.length > 0 && firstTitleArr[i].isNull == "false") {
                        $('#dg').treegrid('expand', firstTitleArr[i].id);
                        for (var j = 0; j < firstTitleArr[i].children.length; j++) {
                            var id = firstTitleArr[i].children[j].id;
                            var row = $('#dg').treegrid('find', id);
                            $.ajax({
                                type: "POST",
                                url: "/Handler/ModuleData.ashx?methodData=ModularToJson&parentid=" + id + "&isnull=" + isnull,
                                cache: false,
                                async: false,
                                success: function (data) {
                                    if (data != null) {
                                        //tg.treegrid('append', {
                                        //    parent: row.id,
                                        //    data: data
                                        //});
                                        $('#dg').treegrid('expand', id);
                                        for (var m = 0; m < completedModuleArr.length; m++) {
                                            if (completedModuleArr[m].SecondTitleID == row.id && completedModuleArr[m].SuperiorID == row.id) {
                                                for (var n = 0; n < data.length; n++) {
                                                    if (data[n].ModularName == completedModuleArr[m].ModuleName) {
                                                        $('#check_' + data[n].id)[0].checked = true;
                                                        show(data[n].id);
                                                    }
                                                }
                                            }
                                        }
                                        //$(('#check_' + id))[0].checked = true;
                                        for (var k = 0; k < data.length; k++) {
                                            if (data[k].children.length > 0) {
                                                $('#dg').treegrid('expand', data[k].id);
                                            }
                                        }
                                    }
                                }
                            });
                        }
                    } else {
                        isnull = "true";
                        var id = firstTitleArr[i].id;
                        var row = $('#dg').treegrid('find', id);
                        $.ajax({
                            type: "POST",
                            url: "/Handler/ModuleData.ashx?methodData=ModularToJson&parentid=" + id + "&isnull=" + isnull,
                            cache: false,
                            async: false,
                            success: function (data) {
                                if (data != null) {
                                    tg.treegrid('append', {
                                        parent: row.id,
                                        data: data
                                    });
                                    $('#dg').treegrid('expand', id);
                                    for (var m = 0; m < completedModuleArr.length; m++) {
                                        if (completedModuleArr[m].FirstTitleID == row.id && completedModuleArr[m].SuperiorID == row.id) {
                                            for (var n = 0; n < data.length; n++) {
                                                if (data[n].ModularName == completedModuleArr[m].ModuleName) {
                                                    $('#check_' + data[n].id)[0].checked = true;
                                                    show(data[n].id);
                                                }
                                            }
                                        }
                                    }
                                    for (var j = 0; j < data.length; j++) {
                                        if (data[j].children.length > 0) {
                                            $('#dg').treegrid('expand', data[j].id);
                                        }
                                    }
                                }
                            }
                        });
                    }
                }
            }
            init();
        }

        //惰性加载数据
        function myLoadFilter(data, id) {
            var isnull = "false";
            var tg = $(this);
            var opts = tg.treegrid('options');
            //展开加载子节点
            opts.onBeforeExpand = function (row) {
                var parent = $('#dg').treegrid('getLevel', row.id);
                if (parent == 1) {
                    var flag = false;
                    for (var i = 0; i < firstTitleArr.length; i++) {
                        if (firstTitleArr[i].ModularName == row.ModularName) {
                            flag = true;
                        }
                    }
                    if (flag) {
                        return;
                    }
                    if (row.children != undefined && row.children.length <= 0) {
                        isnull = "true";
                        $.ajax({
                            type: "POST",
                            url: "/Handler/ModuleData.ashx?methodData=ModularToJson&parentid=" + row.id + "&isnull=" + isnull,
                            cache: false,
                            async: false,
                            success: function (data) {
                                if (data != null) {
                                    tg.treegrid('append', {
                                        parent: row[opts.idField],
                                        data: data
                                    });
                                    for (var i = 0; i < data.length; i++) {
                                        if (data[i].children.length > 0) {
                                            $('#dg').treegrid('expand', data[i].id);
                                        }
                                    }
                                    show(row.id);
                                }
                            }
                        });
                    }
                    else {
                        for (var i = 0; i < row.children.length; i++) {
                            $.ajax({
                                type: "POST",
                                url: "/Handler/ModuleData.ashx?methodData=ModularToJson&parentid=" + row.children[i].id + "&isnull=" + isnull,
                                cache: false,
                                async: false,
                                success: function (data) {
                                    if (data != null) {
                                        tg.treegrid('append', {
                                            parent: row.children[i].id,
                                            data: data
                                        });
                                        $('#dg').treegrid('expand', row.children[i].id);
                                        for (var j = 0; j < data.length; j++) {
                                            if (data[j].children.length > 0) {
                                                $('#dg').treegrid('expand', data[j].id);
                                            }
                                        }
                                        show(row.id);
                                    }
                                }
                            });
                        }
                    }
                    firstTitleArr.push(row);
                }
                if (parent == 2 && row.children == undefined) {
                    $.ajax({
                        type: "POST",
                        url: "/Handler/ModuleData.ashx?methodData=ModularToJson&parentid=" + row.id + "&isnull=false",
                        cache: false,
                        async: false,
                        success: function (data) {
                            if (data != null) {
                                tg.treegrid('append', {
                                    parent: row.id,
                                    data: data
                                });
                                $('#dg').treegrid('expand', row.id);
                                for (var j = 0; j < data.length; j++) {
                                    if (data[j].children.length > 0) {
                                        $('#dg').treegrid('expand', data[j].id);
                                    }
                                }
                                show(row.id);
                            }
                        }
                    });
                }
            }
            opts.onExpand = init;
            //opts.formatter = formatOper(data);
            return data;
        }

        //排序按钮
        function formatOper(val, row) {
            var html = "";
            //因懒加载所以第三级的级别获取为0，所以投机取巧寻找第三级的上一级。如果级别为2，那本级就是第三级
            var level = $('#dg').treegrid('getLevel', row._parentId);
            var parent = $('#dg').treegrid('getParent', row.id);
            //如果上一级为第二级，才给第三级加载上移下移按钮。
            if (level == 2) {
                //如果是第一个位置的不能上移。所以不显示上移按钮
                if (row.sort != 0) {
                    html += '<a href="#" onclick=move("up","' + row.id + '","' + row.sort + '") sort="' + row.sort + '">上移</a>&nbsp;';
                } else {
                    html += '&nbsp;&nbsp;&nbsp;';
                }
                //最后一个位置不显示下移按钮
                if (row.sort != parent.children.length - 1) {
                    html += '<a href="#" onclick=move("down","' + row.id + '","' + row.sort + '") sort="' + row.sort + '">下移</a>&nbsp;';
                }
            }
            return html;
        }

        //添加复选框
        function formatcheckbox(val, row) {
            return "<input type='checkbox' onclick=show('" + row.id + "') id='check_" + row.id + "' parentid='" + row.parentId + "' mouduleid='" + row.ModularID + "' isNull='" + row.isNull + "'  sort='" + row.sort + "'  modularname=\"" + row.ModularName + "\" />" + row.ModularName;
        }

        //保存
        function AddModuleConfig() {
            $.ajax({
                async: false,
                cache: false,
                type: "post",
                url: "/Handler/ModuleData.ashx",
                data: {
                    methodData: "AddModuleConfig",
                    TeachingNaterialName: $("#lbBookName").text(),
                    BookId: getQueryString("bookid"),
                    type: "saveAll",
                    module: getSelected()
                },
                success: function (data) {
                    if (data.Result == "false") {
                        $.messager.alert("系统提示", data.msg);
                        return;
                    } else {
                        $.messager.alert("系统提示", data.msg);
                        window.open("/CourseManagement/CourseManageMent.aspx", "_self");
                    }
                },
                error: function () { }
            });
        }

        //排序
        function move(o, id, sortid) {//将此方法加入上下移的按钮事件即可  
            //id:选中行的ID，sortid：选中行的排序Id
            var parent = $("#dg").treegrid("getParent", id);
            var levelid = "";
            //上移
            if (o == "up") {
                if (parent.id == "undefined") {
                    alert("无法移动！");
                } else {
                    //循环获取当前选中节点同级的sort值
                    for (var i = 0; i < parent.children.length; i++) {
                        //获取选中节点的前一位节点id
                        if (parseInt(parent.children[i].sort) + 1 == parseInt(sortid)) {
                            levelid = parent.children[i].id;
                        }
                    }
                    //pop:弹出节点并在移除该节点后返回包含其子节点的节点数据。
                    var n2 = $("#dg").treegrid("pop", id);
                    //因为是上移。所以当前选中行的sort减一
                    n2.sort = parseInt(sortid) - 1;
                    //在ID为levelid之前添加弹出的节点
                    $("#dg").treegrid("insert", { before: levelid, data: n2 });
                    $("#dg").treegrid("select", id);
                    $('#dg').treegrid('update', {
                        id: levelid,
                        row: { sort: sortid }
                    });
                    init();
                }
            } else if (o == "down") {//下移
                if (parent.id == "undefined") {
                    alert("无法移动！");
                } else {
                    //循环获取当前选中节点同级的sort值
                    for (var i = 0; i < parent.children.length; i++) {
                        //获取选中节点的前一位节点id
                        if (parseInt(parent.children[i].sort) == parseInt(sortid) + 1) {
                            levelid = parent.children[i].id;
                        }
                    }
                    //pop:弹出节点并在移除该节点后返回包含其子节点的节点数据。
                    var n2 = $("#dg").treegrid("pop", id);
                    //因为是上移。所以当前选中行的sort减一
                    n2.sort = parseInt(sortid) + 1;
                    //在ID为levelid之前添加弹出的节点
                    $("#dg").treegrid("insert", { after: levelid, data: n2 });
                    $("#dg").treegrid("select", id);
                    $('#dg').treegrid('update', {
                        id: levelid,
                        row: { sort: sortid }
                    });
                    init();
                }
            }
        }


    </script>
</body>
</html>
