$(function () {
    $("#btnSearch").bind("click", function () {
        $("#hdDatabaseCallCount").val("");
        GetDataList(1);
    });

    //行政区联动片区
    $("#selectArea").change(function () {
        select_subarea();
    });

    GetDataList(1);

    /********导入Excel**********/
    $("#btnExcelIn").die();
    $("#btnExcelIn").click(function () {
        InExcel();
    });

    /*******点击批量调出***********/
    $("#btnCall").die();
    $("#btnCall").bind("click", function () {
        var dom = $("#databaseCallRowList").find(".databaseCallRow");
        var ids = GetCheckValueByFind(dom, ',');
        CallData(ids);
    });
    /*******点击全部调出***********/
    $("#btnCallAll").die();
    $("#btnCallAll").bind("click", function () {
        CallDataAll();
    });
    /*******点击新增任务***********/
    $("#btnAdd").die();
    $("#btnAdd").bind("click", function () {
        add();
    });

    /******点击全选********/
    $("#cbAll").bind("click", function () {
        var checkedValue = $(this).attr("checked");
        if (checkedValue == "checked") {
            checkedValue = true;
        }
        else {
            checkedValue = false;
        }
        $("#databaseCallRowList").find(".databaseCallRow").find("input[type='checkbox']").attr("checked", checkedValue);
        $("#cbAll").attr("checked", checkedValue);
    })
});

/***查询列表***/
function GetDataList(pageIndex) {
    var projectName = $("#txtProjectName").val();
    var areaId = $("#selectArea").val();
    var subareaId = $("#selectSubArea").val();
    var pageSize = 15;
    var isGetCount = 1;
    if ($("#hdDatabaseCallCount").val() != "") {
        isGetCount = 0;
    }
    var functionCodes = GetNowFunctionCodes();
    var paraJson = { projectName: projectName, areaId: areaId, subareaId: subareaId, functionCodes: functionCodes, pageIndex: pageIndex, pageSize: pageSize, isGetCount: isGetCount };
    $.extendAjax(
            {
                url: "/DatabaseCall/GetList_Api",
                data: paraJson,
                type: "post",
                dataType: "json"
            },
            function (data) {
                $("#databaseCallRowList").find(".databaseCallRow").remove();
                var a = $("#databaseCallRowList").find(".databaseCallRow");
                if (data != null) {
                    if (data.Result) {
                        var countShow = 0;
                        if (data.Data != null) {
                            var list = data.Data;
                            for (var i = 0; i < list.length; i++) {
                                var obj = list[i];
                                var dom = BindAllotFlowDom(obj);
                                $("#databaseCallRowList").append(dom);
                                if (countShow == 0) {
                                    countShow = obj.recordcount;
                                }
                            }
                        }
                        if ($("#hdDatabaseCallCount").val() == "") {
                            $("#hdDatabaseCallCount").val(countShow);

                        }
                        $("#countShow").html($("#hdDatabaseCallCount").val());
                        var count_data = $("#hdDatabaseCallCount").val();
                        var pageCount = parseInt(((count_data * 1) - 1) / pageSize) + 1;
                        BindPage(pageIndex, pageSize, count_data);
                        return;
                    } else {
                        alert(decodeURIComponent(data.message));
                    }
                }
            },
           { dom: "#databaseCallPanel" });
}

/**********绑定任务行html信息************/
function BindAllotFlowDom(allotFlowObj) {
    var dom = $("#databaseCallRowList").find("#databaseCallRow").clone();
    dom.find(".txt_projectname").html(decodeURIComponent(allotFlowObj.projectname));
    dom.find(".txt_detailed").click(function () {
        OpenProjectInfo(allotFlowObj.projectid);
    });
    //dom.find(".txt_projectname").attr("href", Url_DatabaseCall_ProjectInfo._StringFormat(allotFlowObj.projectid));
    dom.find(".txt_othername").html(decodeURIComponent(allotFlowObj.othername == null ? "" : allotFlowObj.othername));
    dom.find(".txt_area").html(decodeURIComponent(allotFlowObj.areaname == null ? "" : allotFlowObj.areaname));
    dom.find(".txt_subarea").html(decodeURIComponent(allotFlowObj.subareaname == null ? "" : allotFlowObj.subareaname));
    var address = "";
    if (allotFlowObj.address != null) {
        if (allotFlowObj.address.length > 18) {
            address = allotFlowObj.address.substring(0, 16) + "...";
        } else {
            address = allotFlowObj.address;
        }
    }
    dom.find(".txt_address").html(decodeURIComponent(address));
    //已导入
    if (allotFlowObj.valid == 0) {
        //dom.find(".cbPanel").hide();
        dom.find(".cbPanel").remove();
    }
    //dom.find(".txt_buildingnum").html(decodeURIComponent(allotFlowObj.buildingnum == null ? 0 : allotFlowObj.buildingnum));
    //dom.find(".txt_buildingnum").click(function () {
    //    window.location.href = Url_DatabaseCall_BuildingList._StringFormat(allotFlowObj.projectid);
    //});
    //dom.find(".txt_totalnum").html(decodeURIComponent(allotFlowObj.totalnum == null ? 0 : allotFlowObj.totalnum));
    dom.find(".cb_select").val(allotFlowObj.projectid);
    dom.addClass("databaseCallRow");
    dom.show();
    return dom;
}

/**********绑定分页****************/
function BindPage(nowIndex, pageSize, count) {
    BindPageCommon("#example", nowIndex, count, pageSize, 10,
                                      function (event, originalEvent, type, page) {
                                          GetDataList(page);
                                      }, null);
}

/**********获取当前用户对此页面所用户的权限****************/
function GetNowFunctionCodes() {
    var doms = $("#divFunctionCodes").find(".functioncode");
    var codes = "";
    for (var i = 0; i < doms.length; i++) {
        var val = $(doms.get(i)).text();
        codes = codes + val + ",";

    }
    codes = codes.TrimEnd(',');
    return codes;
}

//片区下拉列表
function select_subarea() {
    var areaid = $("#selectArea").val();
    $.get('/AllotFlowInfo/GetSubAreaSelect', { areaid: areaid }, function (data) {
        $("#selectSubArea").val(null).trigger("change");
        $("#selectSubArea").html("");
        $.each(data, function (i, item) {
            if (item.Selected) {
                $("#selectSubArea").append($("<option selected=\"selected\"></option>").val(item.Value).html(item.Text));
            } else {
                $("#selectSubArea").append($("<option></option>").val(item.Value).html(item.Text));
            }
        });
    });
}

/******调出数据******/
function CallData(ids) {
    if (!confirm("是否确定导入选中数据")) {
        return;
    }
    if (ids == null || ids == "") {
        alert("请选择要导入的数据");
        return;
    }
    var paraJson = { ids: ids };
    $.extendAjax(
                {
                    url: "/DatabaseCall/Call",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                        if (!data.result) {
                                alert(data.message);
                            return;
                        }
                        else {
                            if (data.message) {
                                alert(data.message);
                            } else {
                                AlertFancybox("导入成功");
                            }
                            BindMenuTotalCount();//绑定菜单任务数量
                            //RemoveDepartmentByIds(departmentIds);
                        }
                },
               { dom: "#databaseCallPanel" });
}

/******调出全部数据******/
function CallDataAll() {
    if (!confirm("是否确定导入当前城市中所有的楼盘数据")) {
        return;
    }

    $.extendAjax(
                {
                    url: "/DatabaseCall/CallAll",
                    data: {},
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    if (data != null) {
                        if (data.result != 1) {
                            if (data.errorType == 0) {
                                alert(decodeURIComponent(data.message));
                            }
                            return;
                        }
                        else {
                            if (data.message) {
                                alert(decodeURIComponent(data.message));
                            } else {
                                AlertFancybox("导入成功");
                            }
                            BindMenuTotalCount();//绑定菜单任务数量
                            //RemoveDepartmentByIds(departmentIds);
                        }
                    }
                },
               { dom: "#databaseCallPanel" });
}

/***楼盘信息****/
function OpenProjectInfo(projectId) {
    var url = Url_DatabaseCall_ProjectInfo._StringFormat(projectId);
    $.fancybox({
        'href': url,
        'width': 900,
        'height': 600,
        'padding': 0,
        'overlayShow': true,
        'autoScale': false,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'type': 'iframe',
        'onClosed': function () {
        }
    });
}

/***新增任务****/
function add() {
    var url = Url_DatabaseCall_Add;
    $.fancybox({
        'href': url,
        'width': 900,
        'height': 600,
        'padding': 0,
        'overlayShow': true,
        'autoScale': false,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'type': 'iframe',
        'onClosed': function () {
            BindMenuTotalCount();//绑定菜单任务数量
        }
    });
}

/**********导入Excel*******************/
function InExcel() {
    var url = "/AllotFlowInfo/ExcelIn";
    $.fancybox({
        'href': url,
        'width': 350,
        'height': 180,
        'padding': 0,
        'overlayShow': true,
        'autoScale': false,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'type': 'iframe',
        'onClosed': function () {
            BindMenuTotalCount();//绑定菜单任务数量
            //GetAllotFlowList(1);
        }
    });
}