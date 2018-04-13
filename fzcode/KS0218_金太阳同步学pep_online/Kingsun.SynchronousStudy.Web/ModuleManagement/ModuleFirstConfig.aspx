<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleFirstConfig.aspx.cs" Inherits="Kingsun.SynchronousStudy.Web.ModuleManagement.ModuleFirstConfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>大模块配置</title>
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
				url: '/Handler/ModuleData.ashx?methodData=GetModModularList',
                 queryParams:{bookid:getQueryString('bookid'),ModUrl:Constant.code_mod_Url},
				method: 'get',
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
            //获取书本一、二级标题
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
            // alert(firstTitleArr);
        });


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
                    type: "saveModule",
                    module: getSelected()
                },
                success: function (data) {
                    if (data.Result == "false") {
                        $.messager.alert("系统提示", data.msg);
                        return;
                    } else {
                        $.messager.alert("系统提示", data.msg);
                        window.open("/CourseManagement/CourseManageMent.aspx", "_self");
                        //window.open("/ModuleManagement/ModuleConfig.aspx?bookid=" + bookid + "&bookname=" + getQueryString("BookName"), '_self');
                    }
                },
                error: function () { }
            });
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
                if (isNull=="false" ) {
                    var k = 0;
                    var n = 1;
                    for (var l = 0; l < completedModuleArr.length; l++) {
                        if (completedModuleArr[l].id == id.replace("check_", '')) {
                            k = l;
                            n = 0;
                            for (var i = 0; i < completedModuleArr[k].children.length; i++) {
                                if ($(('#check_' + completedModuleArr[k].children[i].id)).is(':checked'))
                                    n++;
                            }
                        }
                    }
                    if (n == 0)
                        isNull = "true";

                }
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

        //惰性加载数据
        function myLoadFilter(data, id) {

            var isnull = "false";
            var tg = $(this);
            var opts = tg.treegrid('options');

            //展开加载子节点
            opts.onBeforeExpand = function (row) {
                //var k = 0;
                //for(var l = 0;l<firstTitleArr.length;l++)
                //{
                //    if (firstTitleArr[l].id == row.id) {
                //        k = l;
                //        for (var i = 0; i < firstTitleArr[k].children.length; i++) {
                //            for (var j = 0; j < row.children.length; j++) {
                //                if (firstTitleArr[k].children[i].id == row.children[j].id)
                //                    $(('#check_' + row.children[i].id))[0].checked = true;
                //            }
                //        }
                //    }

                //}


            }
            opts.onExpand = init;

            completedModuleArr = data;
            //opts.formatter = formatOper(data);
            return data;
        }

        function LoadConfigurationModule() {
            var isnull = "false";
            var tg = $(this);
            var opts = tg.treegrid('options');



            if (firstTitleArr.length > 0) {
                for (var m = 0; m < firstTitleArr.length; m++) {
                    var id = firstTitleArr[m].id;
                    var row = $('#dg').treegrid('find', id);
                    if (firstTitleArr[m].id == row.id) {
                        $(('#check_' + firstTitleArr[m].id))[0].checked = true;
                    }
                    var k = 0;
                    for (var l = 0; l < firstTitleArr.length; l++) {
                        if (firstTitleArr[l].id == row.id) {
                            k = l;
                            for (var i = 0; i < firstTitleArr[k].children.length; i++) {
                                for (var j = 0; j < row.children.length; j++) {
                                    if (firstTitleArr[k].children[i].id == row.children[j].id)
                                        $(('#check_' + row.children[i].id))[0].checked = true;
                                }
                            }
                        }

                    }
                }
            }
            init();
        }

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
            if (r != null) return decodeURI(r[2]); return null;
        }

        //添加复选框
        function formatcheckbox(val, row) {
            return "<input type='checkbox' onclick=show('" + row.id + "') id='check_" + row.id + "' parentid='" + row.parentId + "' mouduleid='" + row.ModularID + "' isNull='" + row.isNull + "'  sort='" + row.sort + "'  modularname=\"" + row.ModularName + "\" />" + row.ModularName;
        }


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
                    //if (count1 == 0) {
                    //    $(('#check_' + s1.id))[0].checked = false;
                    //}
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


    </script>
</body>
</html>
