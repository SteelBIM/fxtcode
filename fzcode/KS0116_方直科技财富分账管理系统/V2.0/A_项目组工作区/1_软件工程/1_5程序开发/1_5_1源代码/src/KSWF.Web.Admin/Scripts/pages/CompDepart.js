$(function () {
    $.post("/CompDepart/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            var dataaction = "";
            if (jsonData.Merge)
            dataaction += "<button style=\"background-color:#E16965; color:#fff;border-color: #E16965; margin-right:15px; border-radius:5px;\" onclick=\"MergeDept()\"  type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>合并部门 </button>"
            if (jsonData.Move)
            dataaction += "<button style=\"background-color:#F7C727;color:#fff; border-color: #F7C727; border-radius:5px;\"  onclick=\"RemoveDept()\"   type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-export\" aria-hidden=\"true\"></span>移动部门 </button>"
            $("#toolbar").html(dataaction);
            var oTable = new TableInit(jsonData);
            oTable.Init();
        }
    });

    $.post("/CompDepart/CompDepart_GetDeptTree", { deptid: 0, isMerge: false }, function (data) {
        if (data) {
            $("#seldepttreeS").dropdownTree(data.Data);
            $("#seldepttreeF").dropdownTree(data.Data);
        } else {
            bootbox.alert("加载失败！");
        }
    })

    $('#treeview-checkable-school').treeview({
        showIcon: false,
        showCheckbox: true
    });
});

function RemoveDept() {
    //清空相关
    $("#seldepttreeS").ddTreeClearValue();
    $("#seldepttreeF").ddTreeClearValue();

    $.post("/CompDepart/CompDepart_GetDeptTree", { deptid: 0, isMerge:false }, function (data) {
        if (data) {
            $("#seldepttreeS").ddTreeReload(data.Data);
            $("#seldepttreeF").ddTreeReload(data.Data);
        } else {
            bootbox.alert("加载失败！");
        }
    })

    $('#moveModal').modal('show');
}

var nodes = [];
function MergeDept() {
    //清空相关
    $("#mergeDeptnameInput").val("");
    nodes = [];
    //加载部门树
    $.post("/CompDepart/CompDepart_GetDeptTree", { deptid: 1 }, function (data) {
        if (data) {
            $('#treeview-checkable-dept').treeview({
                data: [{ text: data.rootname, nodes: data.Data, state: { showcheckbox: true }, nodeId: 0 }],
                showIcon: false,
                showCheckbox: true,
                onNodeChecked: function (event, node) {
                    if (nodes.length>1) {
                        bootbox.alert("只能选择合并两个部门！");
                        $('#treeview-checkable-dept').treeview('uncheckNode', [node.nodeId, { silent: true }]);
                    } else {
                        nodes.push(node);
                    }
                },
                onNodeUnchecked: function (event, node) {
                    for (var i in nodes) {
                        if (nodes[i].nodeId==node.nodeId) {
                            nodes.splice(i, 1);
                        }
                    }
                }
            });
            $('#mergeModal').modal('show');
        } else {
            bootbox.alert(data.ErrorMsg);
        }
    })
}

$("#btn_moveSave").click(function () {
    var deptidS = $("#seldepttreeS").ddTreeGetValue();
    var deptidF = $("#seldepttreeF").ddTreeGetValue();
    var parentidS = $("#seldepttreeS").ddTreeGetPValue();
    var parentidF = $("#seldepttreeF").ddTreeGetPValue();

    var levelS = $("#seldepttreeS").ddTreeGetLValue();
    var levelF = $("#seldepttreeF").ddTreeGetLValue();

    if (parentidS == deptidF) {
        bootbox.alert("请选择不同的子级部门移动到上级部门中！");
        return;
    }
    if (parseInt(levelS) < parseInt(levelF)) {
        bootbox.alert("请选择子级部门移动到上级部门中！");
        return;
    }
   
    $.post("/CompDepart/CompDepart_Move", { deptidS: deptidS, parentidS: parentidS, deptidF: deptidF, parentidF: parentidF, levelS: levelS, levelF: levelF }, function (data) {
        if (data.Success) {
            bootbox.alert(data.Data);
            $("#moveModal").modal("hide");
            $("#tb_departments").bootstrapTable("refresh");
        } else {
            bootbox.alert(data.ErrorMsg);
        }
    })
});

$("#btn_mergeSave").click(function () {
    var bootstrapValidator = $("#formMerge").data('bootstrapValidator');
    bootstrapValidator.validate();
    if (!bootstrapValidator.isValid()) {
        return;
    }

    var mergeType = 1;//默认上下级，1 上下级部门 2 同级部门
    var deptidA = 0;
    var parentidA = 0;
    var districtidsA = "";
    var deptidB= 0;
    var parentidB = 0;
    var districtidsB = "";

    //判断是否有选择合并的部门，长度需要大于1，并且判断是否是同级或上下级
    if (nodes.length<2) {
        bootbox.alert("请选择需要合并的两个部门！");
        return;
    } else {
        //选取需要传递的有效相关参数
        if (nodes[0].ParentId!=nodes[1].ParentId) {
            if(nodes[0].Id!=nodes[1].ParentId && nodes[0].ParentId!=nodes[1].Id){
                bootbox.alert("请选择同级部门或者上下级部门进行合并！");
                return;
            } else {
                if (nodes[0].Id == nodes[1].ParentId) {
                    //nodes[0]是上级，node[1]是下级
                    deptidA = nodes[0].Id;
                    parentidA = nodes[0].ParentId;
                    districtidsA = nodes[0].tag;
                    deptidB = nodes[1].Id;
                    parentidB = nodes[1].ParentId;
                    districtidsB = nodes[1].tag;

                } else {
                    //nodes[0]是下级，node[1]是上级
                    deptidA = nodes[1].Id;
                    parentidA = nodes[1].ParentId;
                    districtidsA = nodes[1].tag;
                    deptidB = nodes[0].Id;
                    parentidB = nodes[0].ParentId;
                    districtidsB = nodes[0].tag;
                }
            }
        } else {
            mergeType = 2;
            deptidA = nodes[0].Id;
            parentidA = nodes[0].ParentId;
            districtidsA = nodes[0].tag;
            deptidB = nodes[1].Id;
            parentidB = nodes[1].ParentId;
            districtidsB = nodes[1].tag;
        }
    }

    var formData = new FormData($("#formMerge")[0]);
    formData.append("deptidA", deptidA);
    formData.append("parentidA", parentidA);
    formData.append("districtidsA", districtidsA);
    formData.append("deptidB", deptidB);
    formData.append("parentidB", parentidB);
    formData.append("districtidsB", districtidsB);
    formData.append("mergeType", mergeType);

    $.ajax({
        url: '/CompDepart/CompDepart_Merge',
        type: 'POST',
        data: formData,
        async: false,
        cache: false,
        contentType: false,
        enctype: 'multipart/form-data',
        processData: false,
        success: function (data) {
            if (data.Success) {
                bootbox.alert(data.Data);
                $("#mergeModal").modal("hide");
                $("#tb_departments").bootstrapTable("refresh");
            }
            else {
                bootbox.alert(data.ErrorMsg);
            }
        }
    });

});

var TableInit = function (jsonData) {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_departments').bootstrapTable({
            url: '/CompDepart/CompDepart_Details',   //请求后台的URL（*）
            method: 'post',                          //请求方式（*）
            toolbar: '#toolbar',                     //工具按钮用哪个容器
            striped: true,                           //是否显示行间隔色
            cache: false,                            //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                        //是否显示分页（*）
            sortable: false,                         //是否启用排序
            sortOrder: "asc",                        //排序方式
            queryParams: oTableInit.queryParams,     //传递参数（*）
            sidePagination: "server",                //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                           //初始化加载第一页，默认第一页
            pageSize: 10,                            //每页的记录行数（*）
            pageList: [10, 25, 50, 100],             //可供选择的每页的行数（*）
            search: false,                           //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: false,                       //是否显示所有的列
            showRefresh: false,                       //是否显示刷新按钮
            minimumCountColumns: 2,                  //最少允许的列数
            clickToSelect: false,                    //是否启用点击选中行
            //height: 500,                           //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "deptid",                      //每一行的唯一标识，一般为主键列
            showToggle: false,                       //是否显示详细视图和列表视图的切换按钮
            cardView: false,                         //是否显示详细视图
            detailView: true,                        //是否显示父子表
            columns: [{
                field: 'deptid',
                visible: false
            }, {
                field: 'parentid',
                visible: false
            }, {
                field: 'deptname',
                title: '部门名称'
            }, {
                field: 'createname',
                visible: false
            }, {
                field: 'districtid',
                visible: false
            }, {
                field: 'path',
                title: '市场区域',
                formatter: function (value) {
                    return SubStr(value, 20, "...");
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (jsonData.Add) {
                        action += " <a href=\"javascript:;\"  onclick='AddDepart(" + row.deptid + ")'>新增子部门</a> ";
                    }
                    //if (jsonData.Edit) {
                    //    action += " <a href=\"javascript:;\"  onclick='EditDepart(" + row.deptid + ")'>修改</a> ";
                    //}
                    //if (jsonData.Del) {
                    //    action += " <a href=\"javascript:;\"  onclick='DelDepart(" + row.deptid + "," + row.parentid + ")'>删除</a> ";
                    //}
                    return action;
                }
            }],
            //注册加载子表的事件。注意下这里的三个参数！
            onExpandRow: function (index, row, $detail, jsonData) {
                oTableInit.InitSubTable(index, row, $detail, jsonData);
            },
            onLoadSuccess: function (data) {
                //alert("onLoadSuccess");
            },
            onRefresh: function (params) {
                //alert("onRefresh");
            }
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset //页码
        };
        return temp;
    };

    //初始化子表格(无线循环)
    oTableInit.InitSubTable = function (index, row, $detail) {
        var parentid = row.deptid;
        var cur_table = $detail.html("<table id='childTable_" + parentid + "' ></table>").find("table");
        $(cur_table).bootstrapTable({
            url: '/CompDepart/CompDepart_DetailsChildren',
            method: 'post',
            queryParams: {
                parentid: parentid
            },
            sidePagination: "server",//注意！必要参数
            clickToSelect: false,
            showHeader: false,
            detailView: true,
            uniqueId: "deptid",
            pageSize: 10,
            pageList: [10, 25],
            columns: [{
                field: 'deptid',
                visible: false
            }, {
                field: 'parentid',
                visible: false
            }, {
                field: 'deptname',
                title: '部门名称'
            }, {
                field: 'createname',
                visible: false
            }, {
                field: 'districtid',
                visible: false
            }, {
                field: 'path',
                title: '市场区域'
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (jsonData.Add) {
                        action += " <a href=\"javascript:;\"  onclick='AddDepart(" + row.deptid + ")'>新增子部门</a> ";
                    }
                    if (jsonData.Edit) {
                        action += " <a href=\"javascript:;\"  onclick='EditDepart(" + row.deptid + ")'>修改</a> ";
                    }
                    if (jsonData.Del) {
                        action += " <a href=\"javascript:;\"  onclick='DelDepart(" + row.deptid + "," + row.parentid + ")'>删除</a> ";
                    }
                    return action;
                }
            }
            ],
            //无线循环取子表，直到子表里面没有记录
            onExpandRow: function (index, row, $detail) {
                oTableInit.InitSubTable(index, row, $detail);
            }
        });
    };

    return oTableInit;
};

//新增按钮方法
function AddDepart(deptid) {
    $("#deptnameInput").val("");
    $("#higherDepartName").val("");
    $("#higherArea").val("");
    $("#deptArea").val("");
    $("#p4deptid").val("");
    $("#p4parentid").val("");
    $("#p4districtids").val("");
    var obj = { deptid: deptid };
    $.post("/CompDepart/CompDepart_DetailsByID", obj, function (data) {
        if (data.total > 0) {
            if (data.rows[0].isend == 2 && data.deptareaCount == 1) {
                bootbox.alert("不能新增子部门！");
            } else {
                $("#higherDepartName").text(data.rows[0].deptname);
                $("#higherArea").val(data.rows[0].path);
                $("#p4deptid").val(data.rows[0].deptid);//此为父节点id
                $("#p4parentid").val(data.rows[0].parentid);//此为父节点的父id
                $("#p4level").val(data.rows[0].level);
                $('#addModal').modal('show');
                $('#formAdd').data('bootstrapValidator').resetForm(true);
            }
        }
        else {
            bootbox.alert("加载失败！");
        }
    })
}

var areadata;
//修改按钮方法
function EditDepart(deptid) {
    $("#editDeptnameInput").val("");
    $("#editDeptArea").val("");
    $("#p4editParentid").val("");
    $("#p4editDeptid").val("");
    $("#p4editDistrictids").val("");
    var obj = { deptid: deptid };
    $.post("/CompDepart/CompDepart_DetailsByID", obj, function (data) {
        if (data.total > 0) {
            $('#formEdit').data('bootstrapValidator').resetForm(true);
            $("#editDeptnameInput").val(data.rows[0].deptname);
            $("#editDeptArea").val(data.rows[0].path);
            $("#p4editParentid").val(data.rows[0].parentid);
            $("#p4editDeptid").val(data.rows[0].deptid);
            $("#p4editGrandFatherid").val(data.grandfatherid);
            $("#p4editDistrictids").val(data.districtids);
            $("#p4editlevel").val(data.rows[0].level);
            areadata = data.treeViews;
            $('#editModal').modal('show');
        }
        else {
            bootbox.alert("加载失败！");
        }
    })
}

//删除按钮方法
function DelDepart(deptid, parentid) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("确定删除~", function (result) {
        if (result) {
            $.post("/CompDepart/CompDepart_Del", { "deptid": deptid }, function (data) {
                if (data.Success) {
                    bootbox.alert(data.Data);

                    var tableId = "#childTable_" + parentid;
                    $(tableId).bootstrapTable("refresh");
                }
                else {
                    bootbox.alert(data.ErrorMsg);
                }
            })
        }
    });
}

//动态加载区域（暂为新增设置区域按钮方法）
$("#btn_setArea").click(function () {
        var deptid = $("#p4deptid").val();
        var parentid = $("#p4parentid").val();
        SetArea(deptid, parentid);
    });

//修改模板，修改区域按钮事件
$("#editSetAreaModal").click(function () {
    //var deptid = $("#p4editDeptid").val();
    var parentid = $("#p4editParentid").val();
    var grandfatherid = $("#p4editGrandFatherid").val();
    //var areadata = $("#p4editDistrictids").val();
    SetArea(parentid, grandfatherid, areadata);
});

function SetArea(deptid, parentid, areadata) { 

    if (areadata != null && areadata != "") {
            $('#treeview-checkable-checked').treeview({
                data: [{ text: "已选区域", nodes: areadata, state: { checked: false }, nodeId: 0 }],
                showIcon: false,
                showCheckbox: true,
                onNodeChecked: function (event, node) {
                    if (node.nodes) {
                        var selectNodes = getNodeIdArr(node);
                        $('#treeview-checkable-checked').treeview('checkNode', [selectNodes, {
                            silent: true
                        }]);
                    } else {
                        var parentNode = $('#treeview-checkable-checked').treeview('getNode', 0);
                        var otherChecked = true;
                        if (parentNode.nodes) {
                            for (x in parentNode.nodes) {
                                if (parentNode.nodes[x].state.checked == false) {
                                    otherChecked = false;
                                }
                            }
                        }
                        if (otherChecked) {
                            $('#treeview-checkable-checked').treeview('checkNode', [0, {
                                silent: true
                            }]);
                        }
                    }
                },
                onNodeUnchecked: function (event, node) {
                    if (node.nodes) {
                        var selectNodes = getNodeIdArr(node);
                        $('#treeview-checkable-checked').treeview('uncheckNode', [selectNodes, {
                            silent: true
                        }]);
                    } else {
                        $('#treeview-checkable-checked').treeview('uncheckNode', [0, {
                            silent: true
                        }]);
                    }
                }
            });
        } else {
            $('#treeview-checkable-checked').treeview({
                showIcon: false,
                showCheckbox: true
            });
        }

        $("#searchkey").val("");

        $('#treeview-checkable-school').treeview({
            showIcon: false,
            showCheckbox: true
        });
      
        addcloud();
   
        $.post("/CompDepart/CompDepart_GetAreas", { deptid: deptid , parentid: parentid }, function (data) { 
            if (data.Success) {
                $('#treeview-checkable').treeview({
                    data: data.Data,
                    showIcon: false,
                    showCheckbox: true,
                    onNodeExpanded: function (event, node) {
                        var dataChild = { parentid: node.tag, deptid: deptid };
                        $.post("/CompDepart/CompDepart_GetChildrenAreas", dataChild, function (data) {
                            if (data.Success) {
                                $('#treeview-checkable').treeview("deleteChildrenNode", node.nodeId);
                                $('#treeview-checkable').treeview("addNode", [node.nodeId, { node: data.Data }]);

                                //如果父节点选中,默认子节点全部选中
                                if (node.state.checked) {
                                    var newNode = $('#treeview-checkable').treeview('getNode', node.nodeId);
                                    var selectNodes = getNodeIdArr(newNode);
                                    if (selectNodes) {
                                        $('#treeview-checkable').treeview('checkNode', [selectNodes, { silent: true }]);
                                    }
                                }
                            } else {
                                bootbox.alert(data.ErrorMsg);
                            }
                        });
                    },
                    onNodeChecked: function (event, node) {
                        //判断是否是加载学校的父级区域，做为全选按钮
                        var schoolParentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                        var onloadAreaId = schoolParentNode.ParentId;
                        if (node.nodeId == onloadAreaId) {
                            $('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                            checkedNodes(schoolParentNode, $('#treeview-checkable-school'));
                        }
                        //父节点选中，选中所有子节点,递归
                        checkedNodes(node, $('#treeview-checkable'));

                        //子节点选中, 如果其他所有子节点已经选中,选中父节点,递归
                        checkParentNode(node, $('#treeview-checkable'));
                    },
                    onNodeUnchecked: function (event, node) {
                        var schoolParentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                        var onloadAreaId = schoolParentNode.ParentId;
                        if (node.nodeId == onloadAreaId) {
                            $('#treeview-checkable-school').treeview('uncheckNode', [0, { silent: true }]);
                            uncheckedNodes(schoolParentNode, $('#treeview-checkable-school'));
                        }

                        //父节点取消，取消所有子节点都,递归
                        uncheckedNodes(node, $('#treeview-checkable'));

                        //子节点取消，父节点也取消,因为一定没选全部子节点都选中
                        uncheckParentNode(node, $('#treeview-checkable'));
                    },
                    onNodeSelected: function (event, node) {
                        //没有子节点的节点选中，加载学校树
                        if (!node.nodes) {
                            if (node.tag.indexOf('?') == -1)//不包含
                            {
                                var ischeckbox = node.state.checked;
                                var showcheckbox = true;
                                if (node.state.showcheckbox == false) {
                                    showcheckbox = false;
                                }
                                addcloud();
                                $.post("/CompDepart/CompDepart_GetSchools", { tag: node.tag, deptid: deptid }, function (data) {
                                    if (data.Success) {
                                        $('#treeview-checkable-school').treeview({
                                            data: [{ text: "全选", nodes: data.Data, ParentId: node.nodeId, AreaId: node.Id, state: { showcheckbox: showcheckbox } }],
                                            showIcon: false,
                                            showCheckbox: true,
                                            onNodeChecked: function (event, node) {
                                                var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                                                var areaParentNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId);
                                                if (node.nodes) {//全选
                                                    //var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                                                    //$('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                                                    if (parentNode.nodes) {//全选
                                                        
                                                        for (x in parentNode.nodes) {
                                                            if (!parentNode.nodes[x].state.showcheckbox) {
                                                                $('#treeview-checkable-school').treeview('checkNode', [parentNode.nodes[x].nodeId, { silent: true }]);
                                                            }
                                                        }
                                                        //removecloud();
                                                    }
                                                    //if (!parentNode) {//当前点击的节点可被选中 则选中该节点
                                                    $('#treeview-checkable').treeview('checkNode', [node.ParentId, { silent: true }]);
                                                    checkParentNode(areaParentNode, $('#treeview-checkable'));
                                                    //}

                                                } else {
                                                    //var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                                                    var otherChecked = true;
                                                    if (parentNode.nodes) {
                                                        for (x in parentNode.nodes) {
                                                            if (parentNode.nodes[x].state.checked == false) {
                                                                otherChecked = false;
                                                            }
                                                        }
                                                    }
                                                    //var areaParentNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId);
                                                    if (otherChecked) {
                                                        $('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                                                        //if (!isschoolselted) {
                                                        $('#treeview-checkable').treeview('checkNode', [areaParentNode.nodeId, { silent: true }]);
                                                        checkParentNode(areaParentNode, $('#treeview-checkable'));
                                                        //}
                                                    }
                                                }
                                            },
                                            onNodeUnchecked: function (event, node) {
                                                if (node.nodes) {
                                                    var selectNodes = getNodeIdArr(node);
                                                    $('#treeview-checkable-school').treeview('uncheckNode', [selectNodes, { silent: true }]);
                                                    $('#treeview-checkable').treeview('uncheckNode', [node.ParentId, { silent: true }]);
                                                } else {
                                                    $('#treeview-checkable-school').treeview('uncheckNode', [0, { silent: true }]);
                                                }
                                                var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                                                var parentAreaNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId)
                                                $('#treeview-checkable').treeview('uncheckNode', [parentAreaNode.nodeId, { silent: true }]);
                                                uncheckParentNode(parentAreaNode, $('#treeview-checkable'));
                                            }
                                        });

                                        if (ischeckbox) {
                                            var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                                            $('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                                            if (parentNode.nodes) {//全选
                                                for (x in parentNode.nodes) {
                                                    $('#treeview-checkable-school').treeview('checkNode', [parentNode.nodes[x].nodeId, { silent: true }]);
                                                }
                                            }
                                        }
                                        //removecloud();
                                    } else {
                                        //bootbox.alert(data.ErrorMsg);
                                    }
                                    removecloud();
                                });
                               // removecloud();
                            } else {
                                bootbox.alert("请直接选择学校！");
                            }
                        }
                    }
                });
                //removecloud();
            }
            else {
                //removecloud();
                alert(data.ErrorMsg);
            }
        })
        removecloud();
}

function checkParentNode(node, $tree) {
    var parentNode = $tree.treeview('getParent', node);
    if (parentNode.nodeId) {
        var otherChecked = true;
        if (parentNode.nodes) {
            for (x in parentNode.nodes) {
                if (parentNode.nodes[x].state.checked == false) {
                    otherChecked = false;
                }
            }
        }
        if (otherChecked) {
            $tree.treeview('checkNode', [parentNode.nodeId, { silent: true }]);
            checkParentNode(parentNode, $tree);
        }
    }
}

function uncheckParentNode(node, $tree) {
    var parentNode = $tree.treeview('getParent', node);
    if (parentNode.nodeId) {
        $tree.treeview('uncheckNode', [parentNode.nodeId, { silent: true }]);
        uncheckParentNode(parentNode, $tree);
    }
}

$("#searchbtn").click(function () {
    var text = $.trim($("#searchkey").val());

    var ts = $('#treeview-checkable-school').treeview("getAllNode");
    if (ts.length == 0) {
        bootbox.alert("请先选择学校所在区域。");
    } else {
        //$('#treeview-checkable-school').treeview('search', [text, {
        //    ignoreCase: true,     // case insensitive
        //    exactMatch: false,    // like or equals
        //    revealResults: true   // reveal matching nodes
        //}])

        //var firstSearchResult = $(".search-result").first();
        //var nodeNumber = firstSearchResult.attr("data-nodeid");
        //var offset = nodeNumber * 38;
        //$("#scroll").scrollTop(offset);

        var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
        var parentAreaId = parentNode.AreaId;
        var parentId = parentNode.ParentId;
        var areaParentNode = $('#treeview-checkable').treeview('getNode', parentId);
        var showcheckbox = true;
        if (areaParentNode.state.showcheckbox == false) {
            showcheckbox = false;
        }

        $.post("/CompDepart/CompDepart_GetSearchSchools", {
            searchKey: text, areaId: parentAreaId
        }, function (data) {
            if (data.Success) {
                $('#treeview-checkable-school').treeview({
                    data: [{
                        text: "全选", nodes: data.Data, ParentId: parentId, AreaId: parentAreaId, state: {
                            showcheckbox: showcheckbox
                        }
                    }],
                    showIcon: false,
                    showCheckbox: true,
                    onNodeChecked: function (event, node) {
                        var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                        var areaParentNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId);
                        if (node.nodes) {//全选
                            //var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                            //$('#treeview-checkable-school').treeview('checkNode', [0, { silent: true }]);
                            if (parentNode.nodes) {//全选
                                for (x in parentNode.nodes) {
                                    if (!parentNode.nodes[x].state.showcheckbox) {
                                        $('#treeview-checkable-school').treeview('checkNode', [parentNode.nodes[x].nodeId, {
                                            silent: true
                                        }]);
                                    }
                                }
                            }
                            //if (!parentNode) {//当前点击的节点可被选中 则选中该节点
                            //    $('#treeview-checkable').treeview('checkNode', [node.ParentId, { silent: true }]);
                            //    checkParentNode(areaParentNode, $('#treeview-checkable'));
                            //}

                        } else {
                            var otherChecked = true;
                            if (parentNode.nodes) {
                                for (x in parentNode.nodes) {
                                    if (parentNode.nodes[x].state.checked == false) {
                                        otherChecked = false;
                                    }
                                }
                            }
                            if (otherChecked) {
                                $('#treeview-checkable-school').treeview('checkNode', [0, {
                                    silent: true
                                }]);
                                //if (!isschoolselted) {
                                //$('#treeview-checkable').treeview('checkNode', [areaParentNode.nodeId, { silent: true }]);
                                //checkParentNode(areaParentNode, $('#treeview-checkable'));
                                //}
                            }
                        }
                    },
                    onNodeUnchecked: function (event, node) {
                        if (node.nodes) {
                            var selectNodes = getNodeIdArr(node);
                            $('#treeview-checkable-school').treeview('uncheckNode', [selectNodes, {
                                silent: true
                            }]);
                            $('#treeview-checkable').treeview('uncheckNode', [node.ParentId, {
                                silent: true
                            }]);
                        } else {
                            $('#treeview-checkable-school').treeview('uncheckNode', [0, {
                                silent: true
                            }]);
                        }
                        //var parentNode = $('#treeview-checkable-school').treeview('getNode', 0);
                        //var parentAreaNode = $('#treeview-checkable').treeview('getNode', parentNode.ParentId)
                        //$('#treeview-checkable').treeview('uncheckNode', [parentAreaNode.nodeId, { silent: true }]);
                        //uncheckParentNode(parentAreaNode, $('#treeview-checkable'));
                    }
                });
            }
        });
    }

});

//默认树的节点点击事件
function itemOnclick(target) {

}

function uncheckedNodes(node, $tree) {
    if (node.nodes) {
        var selectNodes = getNodeIdArr(node);
        if (selectNodes) {
            $tree.treeview('uncheckNode', [selectNodes, { silent: true }]);
        }
        for (x in node.nodes) {
            uncheckedNodes(node.nodes[x], $tree);
        }
    }
}

function checkedNodes(node, $tree) {
    if (node.nodes) {
        var selectNodes = getNodeIdArr(node);
        if (selectNodes) {
            $tree.treeview('checkNode', [selectNodes, {
                silent: true
            }]);
        }
        for (x in node.nodes) {
            checkedNodes(node.nodes[x], $tree);
        }
    }
}

function getNodeIdArr(node) {
    var ts = [];
    if (node.nodes) {
        for (x in node.nodes) {
            ts.push(node.nodes[x].nodeId)
        }
    } else {
        ts.push(node.nodeId);
    }
    return ts;
}

//设置区域确定按钮事件（暂为新增设置区域确定按钮方法）
$("#btn_confirmArea").click(function () {
    var ts = $('#treeview-checkable-checked').treeview("getAllNode");
    if (ts.length <= 1) {
        bootbox.alert("请选择区域。");
    } else {
        var parentid = $("#p4deptid").val();//新增节点的父id,是父节点的deptid
        var i = 1;
        var areas = "";
        var districtid = "";

        var node = $("#treeview-checkable-checked").treeview('getNode', 0);
        if (node.nodes) {
            for (z in node.nodes) {
                if (i == 1) {
                    areas += node.nodes[z].text;
                    districtid += node.nodes[z].tag;
                    i += 1;
                } else {
                    areas += "、" + node.nodes[z].text;
                    districtid += "," + node.nodes[z].tag;
                }
            }
        }

        $("#deptArea").val(areas);
        $("#p4districtids").val(districtid);

        $("#editDeptArea").val(areas);
        $("#p4editDistrictids").val(districtid);

        $('#setAreaModal').modal('hide');
        $('#formAdd').data('bootstrapValidator').updateStatus('deptArea', 'NOT_VALIDATED', null).validateField('deptArea');
    }
});

//右移加载已选区域按钮事件
$("#addNode").click(function () {
    //-------------------第一颗树---------------------------
    //获取全部节点，如果全部选中，提示错误
    var nodes = [];
    nodes = $('#treeview-checkable').treeview("getAllNode");
    var allNodeCheck = true;
    for (n in nodes) {
        if (nodes[n].state.checked == false) {
            allNodeCheck = false;
            break;
        }
    }
    if (allNodeCheck == true) {
        bootbox.alert("子区域不能全部选中！");
        return;
    }

    var checkeds = [];
    checkeds = $('#treeview-checkable').treeview("getChecked");
    //去重复
    var uniqueCheckeds = [];
    $.each(checkeds, function (i, el) {
        if ($.inArray(el, uniqueCheckeds) === -1) uniqueCheckeds.push(el);
    });
    var needCheckeds = [];
    //排除逻辑, 1 所有的兄弟节点都选中,所有相关节点(这个逻辑需再考察)
    for (x in uniqueCheckeds) {
        var parentNode = $('#treeview-checkable').treeview('getParent', uniqueCheckeds[x]);
        var allSiblingsChecked = true;
        if (parentNode.nodes) {
            for (y in parentNode.nodes) {
                if (parentNode.nodes[y].state.checked == false) {
                    allSiblingsChecked = false;
                    break;
                }
            }
            if (allSiblingsChecked == false) {
                needCheckeds.push(uniqueCheckeds[x]);
            } else {
                needCheckeds.push(parentNode);
            }
        } else {
            needCheckeds.push(uniqueCheckeds[x]);
        }
    }

    var uniqueNeedCheckeds = [];
    $.each(needCheckeds, function (i, el) {
        if ($.inArray(el, uniqueNeedCheckeds) === -1) uniqueNeedCheckeds.push(el);
    });

    var uniqueCheckedsSchool = [];
    //--------学校树中的选中节点---------------------------
    var schoolParentNode = $('#treeview-checkable-school').treeview('getNode', 0);
    if (schoolParentNode.nodeId != undefined) {
        var areaParentNode = $('#treeview-checkable').treeview('getNode', schoolParentNode.ParentId);//是否有学校已被选择 
        if (areaParentNode.state.checked == false) {
            if (schoolParentNode.state.checked == true) {
                if (schoolParentNode.nodes != null) {
                    for (var n in schoolParentNode.nodes) {
                        if (schoolParentNode.nodes[n].state.checked == true)
                            uniqueNeedCheckeds.push(schoolParentNode.nodes[n]);
                    }
                }
            } else {
                var checkedsSchool = $('#treeview-checkable-school').treeview("getChecked");
                //去重复
                $.each(checkedsSchool, function (i, el) {
                    if ($.inArray(el, uniqueCheckedsSchool) === -1 && el.nodeId != 0) uniqueCheckedsSchool.push(el);
                });
            }
        }
    }

    //if (schoolParentNode.state != undefined) {
    //    var isparent = areaParentNode.state.showcheckbox; //为true直接选中学校
    //    //是否全选
    //    if (isparent == true) {
    //        var checkedsSchool = $('#treeview-checkable-school').treeview("getChecked");
    //        //去重复
    //        $.each(checkedsSchool, function (i, el) {
    //            if ($.inArray(el, uniqueCheckedsSchool) === -1 && el.nodeId != 0) uniqueCheckedsSchool.push(el);
    //        });
    //    } else {
    //        if (schoolParentNode.state.checked == false) {
    //            var checkedsSchool = $('#treeview-checkable-school').treeview("getChecked");
    //            //去重复
    //            $.each(checkedsSchool, function (i, el) {
    //                if ($.inArray(el, uniqueCheckedsSchool) === -1 && el.nodeId != 0) uniqueCheckedsSchool.push(el);
    //            });
    //        } else {
    //            //学校的父区域是否有选中，没选中选中右移，选中直接右移
    //            var area = $('#treeview-checkable-school').treeview('getNode', 1).ParentId;
    //            var hasArea = false;
    //            for (var n in uniqueNeedCheckeds) {
    //                if (uniqueNeedCheckeds[n].Id == area) {
    //                    hasArea = true;
    //                }
    //            }
    //            if (hasArea == false) {
    //                var areaNode = $('#treeview-checkable').treeview('getNode', schoolParentNode.ParentId);
    //                uniqueNeedCheckeds.push(areaNode);
    //            }
    //        }
    //    }
    //}



    var removeChildCheckeds = [];//第一颗树的选中节点
    //去除父级的子级,第一颗树------------------------
    for (var k = 0; k < uniqueNeedCheckeds.length; k++) {
        var isNotChild = true;
        for (var l = 0; l < uniqueNeedCheckeds.length; l++) {
            if (uniqueNeedCheckeds[k].parentId == uniqueNeedCheckeds[l].nodeId) {
                isNotChild = false;
            }
        }
        if (isNotChild) {
            removeChildCheckeds.push(uniqueNeedCheckeds[k]);
        }
    }

    var needAddNodes = [];
    if (removeChildCheckeds.length > 0) {
        for (var i = 0; i < removeChildCheckeds.length; i++) {
            needAddNodes.push(removeChildCheckeds[i]);
        }
    }

    if (uniqueCheckedsSchool.length > 0) {
        for (var i = 0; i < uniqueCheckedsSchool.length; i++) {
            needAddNodes.push(uniqueCheckedsSchool[i]);
        }
    }

    var newTreeData = [];
    for (var i = 0; i < needAddNodes.length; i++) {
        newTreeData[i] = { tag: needAddNodes[i].tag, text: needAddNodes[i].text, state: needAddNodes[i].state };
    }

    //$.each(checkeds, function (i, el) {
    //    el.state.checked = false;
    //});

    //-------------加载treeview-checkable-checked------
    //覆盖
    if (newTreeData.length == 0) {
        bootbox.alert("请选择区域。");
    } else {
        var firstChildNode = $('#treeview-checkable-checked').treeview('getNode', 1);
        if (!firstChildNode.nodeId) {
            $('#treeview-checkable-checked').treeview({
                data: [{ text: "已选区域", nodes: newTreeData, state: { checked: true }, nodeId: 0 }],
                showIcon: false,
                showCheckbox: true,
                onNodeChecked: function (event, node) {
                    if (node.nodes) {
                        var selectNodes = getNodeIdArr(node);
                        $('#treeview-checkable-checked').treeview('checkNode', [selectNodes, { silent: true }]);
                    } else {
                        var parentNode = $('#treeview-checkable-checked').treeview('getNode', 0);
                        var otherChecked = true;
                        if (parentNode.nodes) {
                            for (x in parentNode.nodes) {
                                if (parentNode.nodes[x].state.checked == false) {
                                    otherChecked = false;
                                }
                            }
                        }
                        if (otherChecked) {
                            $('#treeview-checkable-checked').treeview('checkNode', [0, { silent: true }]);
                        }
                    }
                },
                onNodeUnchecked: function (event, node) {
                    if (node.nodes) {
                        var selectNodes = getNodeIdArr(node);
                        $('#treeview-checkable-checked').treeview('uncheckNode', [selectNodes, { silent: true }]);
                    } else {
                        $('#treeview-checkable-checked').treeview('uncheckNode', [0, { silent: true }]);
                    }
                }
            });
        } else {
            //追加node
            //判断追加的，在树中有没有相同的，有没有子级，有没有父级
            var checkedParentNode = $('#treeview-checkable-checked').treeview('getNode', 0);
            var uniqueNewTreeData = [];
            for (a in newTreeData) {
                var needAdd = true;
                for (s in checkedParentNode.nodes) {
                    if (checkedParentNode.nodes[s].text == newTreeData[a].text) {
                        needAdd = false;//给出提示
                        //bootbox.alert("不能添加相同的区域或学校！" + newTreeData[a].text + "！");
                        //return;
                        break;
                    }

                    //xuexiao
                    var schoolinfo = checkedParentNode.nodes[s].tag.split("?");
                    var re = schoolinfo[0].split("|");
                    var reCompare2 = newTreeData[a].tag.split("?");

                    if (schoolinfo.length >= 3) {
                        var reSch = newTreeData[a].tag.split("|");
                        if (reCompare2.length < 3) {
                            if (re[0] == reSch[0]) {
                                needAdd = false;//给出提示，学校已选择,qu
                                bootbox.alert("区域内学校已选择！不能再添加" + newTreeData[a].text + "！");
                                return;
                            }
                            if (re[0].substring(0, 4) + "00000" == reSch[0]) {
                                needAdd = false;//给出提示，学校已选择,shi
                                bootbox.alert("区域内学校已选择！不能再添加" + newTreeData[a].text + "！");
                                return;
                            }
                            if (re[0].substring(0, 2) + "0000000" == reSch[0]) {
                                needAdd = false;//给出提示，学校已选择,shen
                                bootbox.alert("区域内学校已选择！不能再添加" + newTreeData[a].text + "！");
                                return;
                            }
                        }
                    } else {
                        //shen
                        if (re[0].substring(2, 9) == '0000000') {
                            var rePro = reCompare2[0].split("|");
                            if (reCompare2.length >= 3) {
                                if (re[0].substring(0, 6) == rePro[0].substring(0, 6)) {
                                    needAdd = false;//xue
                                    bootbox.alert("省份已选择！不能再添加省份内的学校，" + newTreeData[a].text + "！");
                                    return;
                                }
                            } else {
                                if (rePro[0].substring(4, 9) == '00000') {
                                    if (re[0].substring(0, 2) == rePro[0].substring(0, 2)) {
                                        needAdd = false;//给出提示，学校已选择,shi
                                        bootbox.alert("省份已选择！不能再添加省份内的城市，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                                if (rePro[0].substring(6, 9) == '000') {
                                    if (re[0].substring(0, 2) == rePro[0].substring(0, 2)) {
                                        needAdd = false;//给出提示，学校已选择,qu
                                        bootbox.alert("省份已选择！不能再添加省份内的区域，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                            }
                        }
                        //shi
                        if (re[0].substring(4, 9) == '00000') {
                            var reCity = reCompare2[0].split("|");
                            if (reCompare2.length >= 3) {
                                if (re[0].substring(0, 4) == reCity[0].substring(0, 4)) {
                                    needAdd = false;//xue
                                    bootbox.alert("城市已选择！不能再添加城市内的学校，" + newTreeData[a].text + "！");
                                    return;
                                }
                            } else {
                                if (reCity[0].substring(2, 9) == '0000000') {
                                    if (re[0].substring(0, 2) + "0000000" == reCity[0]) {
                                        needAdd = false;//给出提示，学校已选择,shen
                                        bootbox.alert("城市已选择！不能再添加城市所在省份，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                                if (reCity[0].substring(6, 9) == '000') {
                                    if (re[0].substring(0, 4) == reCity[0].substring(0, 4)) {
                                        needAdd = false;//给出提示，学校已选择,qu
                                        bootbox.alert("城市已选择！不能再添加城市内的区域，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                            }
                        }
                        //qu          
                        if (re[0].substring(6, 9) == '000') {
                            var reDir = reCompare2[0].split("|");
                            if (reCompare2.length >= 3) {
                                if (reDir[0] == re[0]) {
                                    needAdd = false;
                                    bootbox.alert("区域已选择！不能再添加区域内的学校，" + newTreeData[a].text + "！");
                                    return;
                                }
                            } else {
                                if (reDir[0].substring(2, 9) == '0000000') {
                                    if (re[0].substring(0, 2) + "0000000" == reDir[0]) {
                                        needAdd = false;//给出提示，学校已选择,shen
                                        bootbox.alert("区域已选择！不能再添加区域所在省份，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                                if (reDir[0].substring(4, 9) == '00000') {
                                    if (re[0].substring(0, 4) + '00000' == reDir[0]) {
                                        needAdd = false;//给出提示，学校已选择,shi
                                        bootbox.alert("区域已选择！不能再添加区域所在城市，" + newTreeData[a].text + "！");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                if (needAdd) {
                    uniqueNewTreeData.push(newTreeData[a]);
                }
            }
            if (uniqueNewTreeData.length > 0) {
                $('#treeview-checkable-checked').treeview('checkNode', [0, { silent: true }]);
                $('#treeview-checkable-checked').treeview('addNode', [0, { node: uniqueNewTreeData }]);
            }
        }
    }
});

//左移清空已选区域按钮事件
$("#removeNode").click(function () {
    //移除选中的节点
    var checkeds = [];
    checkeds = $('#treeview-checkable-checked').treeview("getChecked");
    //去重复
    var uniqueCheckeds = [];
    $.each(checkeds, function (i, el) {
        if ($.inArray(el, uniqueCheckeds) === -1) uniqueCheckeds.push(el);
    });
    for (var i = 0; i < uniqueCheckeds.length; i++) {
        if (uniqueCheckeds[i].nodeId != 0) {
            $('#treeview-checkable-checked').treeview("deleteNode", uniqueCheckeds[i].nodeId);//deleteNode方法有bug
        }
    }
    $('#treeview-checkable-checked').treeview('uncheckNode', [0, { silent: true }]);
});

//保存按钮事件,新增
$("#btn_save").click(function () {
    var bootstrapValidator = $("#formAdd").data('bootstrapValidator');
    bootstrapValidator.validate();
    if (!bootstrapValidator.isValid()) {
        return;
    }

    var deptname = $("#deptnameInput").val();
    //var deptArea = $("#deptArea").val();
    if (deptname == "") {
        bootbox.alert("请输入部门名称！");
        return;
    }
    //if (deptArea == "") {
    //    bootbox.alert("请设置负责区域！");
    //    return;
    //}
    var obj = $("#formAdd").serialize();
    $.post("/CompDepart/CompDepart_SaveAdd", obj, function (data) {
        if (data.Success) {
            bootbox.alert(data.Data);
            $("#addModal").modal("hide");
            var parentid = $("#p4deptid").val();
            var tableId = "#childTable_" + parentid;
            $(tableId).bootstrapTable("refresh");
        }
        else {
            bootbox.alert(data.ErrorMsg);
        }
    })
});

//保存按钮事件,修改
$("#btn_editSave").click(function () {
    var bootstrapValidator = $("#formEdit").data('bootstrapValidator');
    bootstrapValidator.validate();
    if (!bootstrapValidator.isValid()) {
        return;
    }
    var editDeptnameInput = $("#editDeptnameInput").val();
    if (editDeptnameInput == "") {
        bootbox.alert("请输入部门名称！");
    }
    var obj = $("#formEdit").serialize();
    $.post("/CompDepart/CompDepart_SaveEdit", obj, function (data) {
        if (data.Success) {
            bootbox.alert(data.Data);
            $("#editModal").modal("hide");
            var parentid = $("#p4editParentid").val();
            var tableId = "#childTable_" + parentid;
            $(tableId).bootstrapTable("refresh");
        }
        else {
            bootbox.alert(data.ErrorMsg);
        }
    })
});

$("#formAdd").bootstrapValidator({
    feedbackIcons: {
        valid: 'glyphicon glyphicon-ok',
        invalid: 'glyphicon glyphicon-remove',
        validating: 'glyphicon glyphicon-refresh'
    },
    excluded: [':disabled'],//reserForm方法必要参数
    fields: {
        deptname: {
            message: '请输入部门名称',
            validators: {
                notEmpty: {
                    message: '请输入部门名称'
                },
                regexp: {
                    regexp: /^[\u4e00-\u9fa5_a-zA-Z0-9_\s]{1,30}$/,
                    message: '部门名称长度不能超过30个汉字'
                }
            }

        }
        //,
        //deptArea: {
        //    message: '请设置负责区域！',
        //    validators: {
        //        notEmpty: {
        //            message: '请设置负责区域！'
        //        }
        //    }
        //}
    }
});

$("#formEdit").bootstrapValidator({
    feedbackIcons: {
        valid: 'glyphicon glyphicon-ok',
        invalid: 'glyphicon glyphicon-remove',
        validating: 'glyphicon glyphicon-refresh'
    },
    excluded: [':disabled'],
    fields: {
        deptname: {
            message: '请输入部门名称',
            validators: {
                notEmpty: {
                    message: '请输入部门名称'
                },
                stringLength: {
                    min: 1, max: 30, message: '部门名称长度必须大于1且小于30个字符'
                },
            }
        }
    }
});

$("#formMerge").bootstrapValidator({
    feedbackIcons: {
        valid: 'glyphicon glyphicon-ok',
        invalid: 'glyphicon glyphicon-remove',
        validating: 'glyphicon glyphicon-refresh'
    },
    excluded: [':disabled'],
    fields: {
        deptname: {
            message: '请输入部门名称',
            validators: {
                notEmpty: {
                    message: '请输入部门名称'
                },
                stringLength: {
                    min: 1, max: 30, message: '部门名称长度必须大于1且小于30个字符'
                },
            }
        }
    }
});

