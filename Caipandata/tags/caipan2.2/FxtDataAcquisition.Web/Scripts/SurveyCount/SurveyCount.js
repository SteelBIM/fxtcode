$(function () {
    $("#btnSearch").bind("click", function () {
        $("#hdDatabaseCallCount").val("");
        GetDataList(1);
    });

    ////行政区联动片区
    //$("#selectArea").change(function () {
    //    select_subarea();
    //});

    GetDataList(1);

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
    var departmentId = $("#selectDepartment").val();
    var selectUserTrueName = $("#selectUserTrueName").val();
    var pageSize = 15; 
    var isGetCount = 1;
    if ($("#hdDatabaseCallCount").val() != "") {
        isGetCount = 0;
    }
    var functionCodes = GetNowFunctionCodes();
    var paraJson = { pageIndex: pageIndex, pageSize: pageSize, departmentId: departmentId, uName: selectUserTrueName };
    $.extendAjax(
            {
                url: "/SurveyCount/LoadData",
                data: paraJson,
                type: "post",
                dataType: "json"
            },
            function (data) {
                $("#databaseCallRowList").find(".databaseCallRow").remove();
                var a = $("#databaseCallRowList").find(".databaseCallRow");
                if (data != null) {
                    if (data.result) {
                        var countShow = 0;
                        if (data.data != null) {
                            var list = data.data.list;
                            countShow = data.data.recordcount;
                            for (var i = 0; i < list.length; i++) {
                                var obj = list[i];
                                var dom = BindAllotFlowDom(obj);
                                $("#databaseCallRowList").append(dom);
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
    var name = allotFlowObj.surveyusertruename;
    if (allotFlowObj.surveyusertruename == "" || allotFlowObj.surveyusertruename == null){
        name = allotFlowObj.surveyusername;
        if(allotFlowObj.surveyusername == "" || allotFlowObj.surveyusername == null){
            name = "";
        }
    }

    dom.find(".txt_surveyusertruename").html(decodeURIComponent(name));
    dom.find(".txt_tosurveycount").html(decodeURIComponent(allotFlowObj.tosurveycount));
    dom.find(".txt_inthesurveycount").html(decodeURIComponent(allotFlowObj.inthesurveycount));
    dom.find(".txt_havesurveycount").html(decodeURIComponent(allotFlowObj.havesurveycount));
    dom.find(".txt_pendingapprovalcount").html(decodeURIComponent(allotFlowObj.pendingapprovalcount));
    dom.find(".txt_passedapprovalcount").html(decodeURIComponent(allotFlowObj.passedapprovalcount));
    dom.find(".txt_alreadystoragecount").html(decodeURIComponent(allotFlowObj.alreadystoragecount));
    dom.find(".txt_alreadystoragebuildingcount").html(decodeURIComponent(allotFlowObj.alreadystoragebuildingcount));
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

