$(function () {
    $("#btnSearch").bind("click", function () {
        $("#hdDatabaseCallCount").val("");
        GetDataList(1);
    });

    GetDataList(1);
});

/***查询列表***/
function GetDataList(pageIndex) {
    var buildingName = $("#txtBuildingName").val();
    var projectId = $("#projectId").val();
    var pageSize = 15;
    var isGetCount = 1;
    if ($("#hdDatabaseCallCount").val() != "") {
        isGetCount = 0;
    }
    var functionCodes = GetNowFunctionCodes();
    var paraJson = { buildingName: buildingName, projectId: projectId, functionCodes: functionCodes, pageIndex: pageIndex, pageSize: pageSize, isGetCount: isGetCount };
    $.extendAjax(
            {
                url: "/DatabaseCall/GetBuildingList_Api",
                data: paraJson,
                type: "post",
                dataType: "json"
            },
            function (data) {
                $("#databaseCallRowList").find(".databaseCallRow").remove();
                var a = $("#databaseCallRowList").find(".databaseCallRow");
                if (data != null) {
                    if (data.result != 1) {
                        if (data.errorType == 0) {
                            alert(decodeURIComponent(data.message));
                        }
                        return;
                    }

                    var countShow = 0;
                    if (data.detail.List != null) {
                        var projectIds = "";
                        var list = data.detail.List;
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
                    //alert(count_data);
                    BindPage(pageIndex, pageSize, count_data);
                }
            },
           { dom: "#databaseCallPanel" });
}

/**********绑定任务行html信息************/
function BindAllotFlowDom(allotFlowObj) {
    var dom = $("#databaseCallRowList").find("#databaseCallRow").clone();
    dom.find(".txt_buildingname").html(decodeURIComponent(allotFlowObj.buildingname));
    dom.find(".txt_buildingname").attr("href", Url_DatabaseCall_BuildingInfo._StringFormat(allotFlowObj.buildingid));
    dom.find(".txt_unitsnumber").html(decodeURIComponent(allotFlowObj.unitsnumber == null ? "" : allotFlowObj.unitsnumber));
    dom.find(".txt_totalfloor").html(decodeURIComponent(allotFlowObj.totalfloor == null ? "" : allotFlowObj.totalfloor));
    dom.find(".txt_totalnumber").html(decodeURIComponent(allotFlowObj.totalnumber == null ? "" : allotFlowObj.totalnumber));
    dom.find(".txt_totalnumber").click(function () {
        window.location.href = Url_DatabaseCall_HouseList._StringFormat(allotFlowObj.projectid);
    });
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
