var allotStatus1 = 1035001;//待分配
var allotStatus2 = 1035002;//已分配
var allotStatus4 = 1035004;//查勘中
var allotStatus8 = 1035008;//审核已通过
var functionCode14 = 1301014;//分配任务
var functionCode15 = 1301015;//撤销任务
var functionCode16 = 1301016;//撤销查勘
var functionCode09 = 1301009;//删除全部
var state_code = 0;
var pageindex = 1;
$(function () {
    //BindTopOperateShow($("#selectStatus").attr("data-nowId"));
    //BindMenuShow();
    BindStatus();
    GetAllotFlowList(pageindex);
    $("#btnSearch").bind("click", function () {
        $("#hdAllotFlowCount").val("");
        GetAllotFlowList(pageindex);
    });

    /********点击单个任务分配********/
    $(".op_AssignAllot").live("click", function () {
        var nowAllotId = $(this).attr("data-allotId");
        AssignAllotToUser_Open(nowAllotId);
    });
    /*******点击批量分配任务***********/
    $("#btnAdd").bind("click", function () {
        var dom = $("#allotFlowRowList").find(".allotFlowRow");
        var allotIds = "";
        var checkList = dom.find("input[type='checkbox']");
        for (var i = 0; i < checkList.length; i++) {
            var isSelect = $(checkList.get(i)).attr("checked");
            var val = $(checkList.get(i)).val();
            var nowStatus = $("#allotFlowRowList").find("#allotFlowRow_" + val).find(".txt_status").attr("data-statusId");
            /**选中 并且为待分配**/
            if (isSelect == "checked" && nowStatus == allotStatus1) {
                allotIds = allotIds + val + ",";
            }
        }
        allotIds = allotIds.TrimEnd(',');
        AssignAllotToUser_Open(allotIds);
    });
    /*******点击批量删除任务***********/
    $("#btnDelete").bind("click", function () {
        DA_Confirm("提示", "是否确定删除任务", function (ok) {
            if (ok) {
                var dom = $("#allotFlowRowList").find(".allotFlowRow");
                var allotIds = "";
                var checkList = dom.find("input[type='checkbox']");
                for (var i = 0; i < checkList.length; i++) {
                    var isSelect = $(checkList.get(i)).attr("checked");
                    var val = $(checkList.get(i)).val();
                    var nowStatus = $("#allotFlowRowList").find("#allotFlowRow_" + val).find(".txt_status").attr("data-statusId");
                    /**选中 并且为待分配**/
                    if (isSelect == "checked" && (nowStatus == allotStatus1 || nowStatus == allotStatus8)) {
                        allotIds = allotIds + val + ",";
                    }
                }
                allotIds = allotIds.TrimEnd(',');
                $.post("/AllotFlowInfo/delete", { allotIds: allotIds }, function (result) {
                    if (result.result) {
                        alert(result.message);
                        GetAllotFlowList(pageindex);
                        //location.reload();
                    } else {
                        alert(result.message);
                    }
                });
            }
        });
    });

    /********点击单个撤销任务********/
    $(".op_CancelAllot").die();
    $(".op_CancelAllot").live("click", function () {
        var nowAllotId = $(this).attr("data-allotId");
        SubmitData_CancelAllotFlow(nowAllotId);
    });
    /********点击批量撤销任务********/
    $("#btnCancelAllot").die();
    $("#btnCancelAllot").live("click", function () {
        var dom = $("#allotFlowRowList").find(".allotFlowRow");
        var allotIds = "";
        var checkList = dom.find("input[type='checkbox']");
        for (var i = 0; i < checkList.length; i++) {
            var isSelect = $(checkList.get(i)).attr("checked");
            var val = $(checkList.get(i)).val();
            var nowStatus = $("#allotFlowRowList").find("#allotFlowRow_" + val).find(".txt_status").attr("data-statusId");
            /**选中 并且为已分配**/
            if (isSelect == "checked" && nowStatus == allotStatus2) {
                allotIds = allotIds + val + ",";
            }
        }
        allotIds = allotIds.TrimEnd(',');
        SubmitData_CancelAllotFlow(allotIds);
    });
    /********点击单个撤销查勘********/
    $(".op_CancelSurvey").die();
    $(".op_CancelSurvey").live("click", function () {
        var nowAllotId = $(this).attr("data-allotId");
        SubmitData_CancelSurvey(nowAllotId);
    });

    /********点击批量撤销查勘********/
    $("#btnCancelSurvey").die();
    $("#btnCancelSurvey").live("click", function () {
        var dom = $("#allotFlowRowList").find(".allotFlowRow");
        var allotIds = "";
        var checkList = dom.find("input[type='checkbox']");
        for (var i = 0; i < checkList.length; i++) {
            var isSelect = $(checkList.get(i)).attr("checked");
            var val = $(checkList.get(i)).val();
            var nowStatus = $("#allotFlowRowList").find("#allotFlowRow_" + val).find(".txt_status").attr("data-statusId");
            /**选中 并且为查勘中**/
            if (isSelect == "checked" && nowStatus == allotStatus4) {
                allotIds = allotIds + val + ",";
            }
        }
        allotIds = allotIds.TrimEnd(',');
        SubmitData_CancelSurvey(allotIds);
    });
    /********点击任务跟踪********/
    $(".allotSurvey").live("click", function () {
        var nowAllotId = $(this).attr("data-allotId");
        AllotSurvey_Open(nowAllotId);
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
        $("#allotFlowRowList").find(".allotFlowRow").find("input[type='checkbox']").attr("checked", checkedValue);
        $("#cbAll").attr("checked", checkedValue);
    })

    /******行政区联动片区********/
    $("#selectArea").change(function () {
        select_subarea();
    });
    /******点击导出********/
    $("#btnExport").bind("click", function () {
        var dom = $("#allotFlowRowList").find(".allotFlowRow");
        var allotIds = "";
        var checkList = dom.find("input[type='checkbox']");
        for (var i = 0; i < checkList.length; i++) {
            var isSelect = $(checkList.get(i)).attr("checked");
            var val = $(checkList.get(i)).val();
            var nowStatus = $("#allotFlowRowList").find("#allotFlowRow_" + val).find(".txt_status").attr("data-statusId");
            /**选中 并且为审核已通过**/
            if (isSelect == "checked" && nowStatus == allotStatus8) {
                allotIds = allotIds + val + ",";
            }
        }
        if (!allotIds) {
            alert("请选择需要导出的楼盘！");
            return false;
        }
        allotIds = allotIds.TrimEnd(',');
        window.location.href = "/AllotFlowInfo/Export?allotIds=" + allotIds;
    });
});
/******设置顶部操作按钮*******/
function BindTopOperateShow(nowStatus) {
    $("#topOperate").find(".topOperate").hide();
    //alert(CheckFunctionCodes(functionCode16));
    if ((nowStatus == allotStatus1) && CheckFunctionCodes(functionCode14))/**如果当前任务状态为待分配&&有权限***/ {
        //if ((nowStatus == allotStatus1 || nowStatus == null || nowStatus == "" || nowStatus == "0" || nowStatus == 0) && CheckFunctionCodes(functionCode14))/**如果当前任务状态为待分配&&有权限***/ {
        $("#topOperate").find("#btnAdd").show();/**显示分配任务**/
        $("#topOperate").find("#btnDelete").show();/**显示删除任务**/
    }
    else if (nowStatus == allotStatus2 && CheckFunctionCodes(functionCode15))/**如果当前任务状态为已分配&&有权限***/ {
        $("#topOperate").find("#btnCancelAllot").show();/**显示撤销任务**/
    }
    else if (nowStatus == allotStatus4 && CheckFunctionCodes(functionCode16))/**如果当前任务状态为查勘中&&有权限***/ {
        $("#topOperate").find("#btnCancelSurvey").show();/**显示撤销查勘**/
    }
    else if (nowStatus == allotStatus8 && CheckFunctionCodes(functionCode09))/**如果当前任务状态为审核已通过&&有权限***/ {
        $("#topOperate").find("#btnDelete").show();/**显示删除任务**/
    }
}
/***绑定菜单显示****/
function BindMenuShow() {
    var nowStatusCode = $("#selectStatus").attr("data-nowId");
    var menuIndex = "1_1";
    if (nowStatusCode == "1035001") {
        menuIndex = "2_1";
    }
    else if (nowStatusCode == "1035002") {
        menuIndex = "3_1";
    }
    else if (nowStatusCode == "1035004") {
        menuIndex = "3_2";
    }
    else if (nowStatusCode == "1035005") {
        menuIndex = "4_1";
    }
    else if (nowStatusCode == "1035006") {
        menuIndex = "4_2";
    }
    else if (nowStatusCode == "1035008") {
        menuIndex = "4_3";
    }
    else if (nowStatusCode == "1035009") {
        menuIndex = "4_4";
    }
    else if (nowStatusCode == "1035010") {
        menuIndex = "4_5";
    }
    SetLeftMenu(menuIndex);
}
/**********************************************************查询列表操作**********************************************/
/**绑定状态下拉框***/
function BindStatus() {
    $("#selectStatus").find(".statusInfo").remove();
    for (var i = 0; i < allotstatuslistjson.length; i++) {
        var obj = allotstatuslistjson[i];
        var select = "";
        var nowId = $("#selectStatus").attr("data-nowId");
        if (nowId == obj.code) {
            select = "selected=\"selected\"";
        }
        var statushtml = "<option value=\"" + obj.code + "\" class=\"statusInfo statusInfo_" + obj.code + "\" " + select + ">" + obj.codename + "</option>";
        $("#selectStatus").append(statushtml);
    }
}
/***查询任务列表***/
function GetAllotFlowList(pageIndex) {
    var projectName = $("#txtProjectName").val();
    var areaId = $("#selectArea").val();
    var subAreaId = $("#selectSubArea").val();
    var status = $("#selectStatus").val();//"1035004";
    var selectUserTrueName = $("#selectUserTrueName").val();
    var startDate = $("#txtStartDate").val();
    var endDate = $("#txtEndDate").val();
    if (startDate != "" && endDate != "") {
        if (new Date(startDate) > new Date(endDate)) {
            alert("结束时间不能大于开始时间");
            return false;
        }
    }
    var pageSize = 10;

    //BindTopOperateShow(status);
    var functionCodes = GetNowFunctionCodes();
    var paraJson = {
        projectName: projectName, areaId: areaId, subAreaId: subAreaId, status: status, selectUserTrueName: selectUserTrueName
        , startDate: startDate, endDate: endDate, functionCodes: functionCodes, pageIndex: pageIndex, pageSize: pageSize
    };
    $.extendAjax(
                {
                    url: "/AllotFlowInfo/AllotFlowManager_GetList_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    $("#allotFlowRowList").find(".allotFlowRow").remove();
                    if (data != null) {
                        if (!data.result) {
                            alert(decodeURIComponent(data.message));
                            return;
                        }

                        state_code = data.data.StateCode
                        BindMenuTotalCount();//绑定菜单任务数量

                        if (data.data.List != null) {
                            var projectIds = "";
                            var list = data.data.List;
                            for (var i = 0; i < list.length; i++) {
                                var obj = list[i];
                                var dom = BindAllotFlowDom(obj);
                                $("#allotFlowRowList").append(dom);
                            }

                        }

                        var allotFlowCount = $("#hdAllotFlowCount");

                        allotFlowCount.val(data.data.Count);

                        var count_data = allotFlowCount.val();
                        var pageCount = parseInt(((count_data * 1) - 1) / pageSize) + 1;
                        //alert(count_data);
                        BindPage(pageIndex, pageSize, count_data);

                        $("#allotCountShow").html(count_data);
                    }
                },
               { dom: "#allotFlowPanel" });
}
/****设置当前操作按钮****/
function BindAllotOpShow(rowDom, nowAllotId, nowStatus) {
    rowDom.find(".op_function").find(".op_function_btn").attr("data-allotId", nowAllotId);
    rowDom.find(".op_function").find(".op_function_btn").hide();
    rowDom.find(".cbPanel").hide();
    rowDom.find(".cbPanel").removeClass("showcb");
    if (nowStatus == allotStatus1 && CheckFunctionCodes(functionCode14))/**如果当前任务状态为待分配&&有分配权限***/ {
        rowDom.find(".op_AssignAllot").show();/**显示分配任务**/
        if (state_code) {
            rowDom.find(".cbPanel").show();/**显示复选框**/
            rowDom.find(".cbPanel").addClass("showcb");
        }
    }
    else if (nowStatus == allotStatus2 && CheckFunctionCodes(functionCode15))/**如果当前任务状态为已分配&&有撤销分配权限***/ {
        rowDom.find(".op_CancelAllot").show();/**显示撤销任务**/
        if (state_code) {
            rowDom.find(".cbPanel").show();/**显示复选框**/
            rowDom.find(".cbPanel").addClass("showcb");
        }
    }
    else if (nowStatus == allotStatus4 && CheckFunctionCodes(functionCode16))/**如果当前任务状态为查勘中&&有撤销查勘权限***/ {
        rowDom.find(".op_CancelSurvey").show();/**显示撤销查勘**/
        if (state_code) {
            rowDom.find(".cbPanel").show();/**显示复选框**/
            rowDom.find(".cbPanel").addClass("showcb");
        }
    }
    else if (nowStatus == allotStatus8 && CheckFunctionCodes(functionCode09))/**如果当前任务状态为审核已通过&&有撤删除全部权限***/ {
        if (state_code) {
            rowDom.find(".cbPanel").show();/**显示复选框**/
            rowDom.find(".cbPanel").addClass("showcb");
        }
    }
    $("#cbAllPanel").hide();
    if ($("#allotFlowRowList").find(".showcb").length > 0 || rowDom.find(".showcb").length > 0) {
        $("#cbAllPanel").show();
    }
}
/**********绑定任务行html信息************/
function BindAllotFlowDom(allotFlowObj) {
    var dom = $("#allotFlowRowList").find("#allotFlowRow").clone();
    dom.find(".txt_projectname").html(decodeURIComponent(allotFlowObj.projectname));
    dom.find(".txt_projectname").attr("href", Url_AllotFlowInfo_AllotDetailed_ByAllotId._StringFormat(allotFlowObj.allotid));
    var address = decodeURIComponent(allotFlowObj.address)
    if (address == "null") {
        address = "";
    }
    if (address != null) {
        if (address.length > 18) {
            address = address.substring(0, 20) + "...";
        }
    }
    dom.find(".txt_address").html(address);
    var username = (allotFlowObj.usertruename == null ? "" : decodeURIComponent(allotFlowObj.usertruename));
    if (username == "") username = (allotFlowObj.username == null ? "" : decodeURIComponent(allotFlowObj.username))
    if (allotFlowObj.allotstate == allotStatus1) {
        username = "";
    }
    var surveyusername = ((allotFlowObj.surveyusertruename == null || allotFlowObj.surveyusername == "") ? "" : decodeURIComponent(allotFlowObj.surveyusertruename));
    if (surveyusername == "") surveyusername = ((allotFlowObj.surveyusername == null || allotFlowObj.surveyusername == "") ? "" : decodeURIComponent(allotFlowObj.surveyusername));
    dom.find(".txt_info1").html("<font style='color:#02b202'>分配人:</font>" +
        //(allotFlowObj.username == null ? "" : decodeURIComponent(allotFlowObj.username)) +
        username
        + ",<font style='color:#02b202'>分配给:</font>" +
        //((allotFlowObj.surveyusername == null || allotFlowObj.surveyusername == "") ? "" : decodeURIComponent(allotFlowObj.surveyusername)) +
        surveyusername
        );
    //alert(decodeURIComponent(allotFlowObj.statedate).replace("T"," ").replace("t"," "));
    //alert(new Date("2014-06-26T10:23:22").getFullYear());
    //dom.find(".txt_info2").html(new Date(decodeURIComponent(allotFlowObj.statedate)).Format("yyyy-MM-dd HH:mm:ss"));
    dom.find(".txt_info2").html(decodeURIComponent(allotFlowObj.allotdate).replace("T", " ")
        + '&nbsp;<a href="javascript:;" class= "allotSurvey"  data-allotId="' + allotFlowObj.allotid + '">任务跟踪</a>'
        );
    var areaname = "";
    if (allotFlowObj.areaname) {
        areaname = "[" + allotFlowObj.areaname + "]";
    }
    var subareaname = "";
    if (allotFlowObj.subareaname) {
        subareaname = "[" + allotFlowObj.subareaname + "]";
    }
    var codename = "";
    if ($("#selectStatus").find(".statusInfo_" + allotFlowObj.allotstate).length > 0) {
        codename = $("#selectStatus").find(".statusInfo_" + allotFlowObj.allotstate).html();
    }
    dom.find(".txt_area").html(areaname);
    dom.find(".txt_subarea").html(subareaname);
    dom.find(".txt_status").html(codename);
    dom.find(".txt_status").attr("data-statusId", allotFlowObj.allotstate);
    dom.find(".cb_select").val(allotFlowObj.allotid);
    /****设置当前操作按钮****/
    BindAllotOpShow(dom, allotFlowObj.allotid, allotFlowObj.allotstate);
    dom.attr("id", "allotFlowRow_" + allotFlowObj.allotid).addClass("allotFlowRow").show();
    return dom;
}
/**********绑定分页****************/
function BindPage(nowIndex, pageSize, count) {
    BindPageCommon("#example", nowIndex, count, pageSize, 10,
                                      function (event, originalEvent, type, page) {
                                          pageindex = page;
                                          GetAllotFlowList(page);
                                      }, null);
}
/***************************************************************分配任务操作************************************/

/**********打开分配任务****************/
function AssignAllotToUser_Open(allotIds) {
    if (allotIds == null || allotIds == "") {
        DA_Alert("请选择未分配的任务", function () { });
        return;
    }
    var url = Url_AllotFlowInfo_AssignAllotToUser_ByAllotIds._StringFormat(allotIds);
    $.fancybox({
        'href': url,/**"/OperationMaintenance/SetCase_Fancybox?fxtCityId=6&projectId=45772&caseId="+caseId+"&buildingTypeCode=2003004&purposeCode=1002001&areaTypeCode=8006004&date=2012-6",**/
        'width': 600,
        'height': 330,
        'padding': 0,
        'overlayShow': true,
        'autoScale': false,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'type': 'iframe',
        'onClosed': function () {
            GetAllotFlowList(1);

        }
    });
    $.fancybox.die();
}
/**********打开任务跟踪****************/
function AllotSurvey_Open(allotId) {
    if (!allotId) {
        DA_Alert("请选择任务", function () { });
        return;
    }
    var url = Url_AllotSurvey_Index + "?allotId=" + allotId;
    $.fancybox({
        'href': url,
        'width': 600,
        'height': 530,
        'padding': 0,
        'overlayShow': true,
        'autoScale': false,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'type': 'iframe',
        'onClosed': function () {
            //GetAllotFlowList(1);
        }
    });
    $.fancybox.die();
}
/**********打开分配任务_响应(该任务状态改为"已分配",显示"撤销任务"操作按钮)****************/
function AssignAllotToUser_Response(allotIds) {
    var codename = "";
    if ($("#selectStatus").find(".statusInfo_" + allotStatus2).html() != "") {
        codename = $("#selectStatus").find(".statusInfo_" + allotStatus2).html();
    }
    if (allotIds != null && allotIds != "") {
        var _ids = allotIds.split(',');
        for (var i = 0; i < _ids.length; i++) {

            $("#allotFlowRowList").find("#allotFlowRow_" + _ids[i]).find(".txt_status").html(codename);
            $("#allotFlowRowList").find("#allotFlowRow_" + _ids[i]).find(".txt_status").attr("data-statusId", allotStatus2);
            BindAllotOpShow($("#allotFlowRowList").find("#allotFlowRow_" + _ids[i]), _ids[i], allotStatus2);
        }
        var status = $("#selectStatus").val();
        //if (status != "0" && status != 0)
        {
            $("#hdAllotFlowCount").val("");
            GetAllotFlowList(1);
        }
    }
}
/***************************************************************撤销任务操作************************************/
/******提交撤销任务******/
function SubmitData_CancelAllotFlow(allotIds) {
    if (allotIds.split(',').length < 1 || allotIds == null || allotIds == "") {
        DA_Alert("请选择任务", function () { });
        return;
    }
    DA_Confirm("提示", "是否确定撤销任务", function (ok) {
        if (ok) {
            var paraJson = { allotIds: allotIds };
            $.extendAjax(
                        {
                            url: "/AllotFlowInfo/AllotFlowManager_CancelAllotFlow_Api",
                            data: paraJson,
                            type: "post",
                            dataType: "json"
                        },
                        function (data) {
                            if (!data.result) {
                                DA_Alert(decodeURIComponent(data.message), function () { });
                                return;
                            }
                            else {
                                alert("操作成功!");
                                SubmitData_CancelAllotFlow_Response(allotIds);

                            }
                        },
                       { dom: "#allotFlowPanel" });
        }
    });
}
/******撤销任务后,该任务状态改为"待分配",显示"分配任务"操作按钮*/
function SubmitData_CancelAllotFlow_Response(allotIds) {
    var codename = "";
    if ($("#selectStatus").find(".statusInfo_" + allotStatus1).html() != "") {
        codename = $("#selectStatus").find(".statusInfo_" + allotStatus1).html();
    }
    if (allotIds != null && allotIds != "") {
        var _ids = allotIds.split(',');
        for (var i = 0; i < _ids.length; i++) {
            $("#allotFlowRowList").find("#allotFlowRow_" + _ids[i]).find(".txt_status").html(codename);
            $("#allotFlowRowList").find("#allotFlowRow_" + _ids[i]).find(".txt_status").attr("data-statusId", allotStatus1);
            BindAllotOpShow($("#allotFlowRowList").find("#allotFlowRow_" + _ids[i]), _ids[i], allotStatus1);
        }
        var status = $("#selectStatus").val();
        //if (status != "0" && status != 0)
        {
            $("#hdAllotFlowCount").val("");
            GetAllotFlowList(1);
        }
    }
}

/***************************************************************撤销查勘操作************************************/
/******提交撤销查勘******/
function SubmitData_CancelSurvey(allotIds) {
    if (allotIds.split(',').length < 1 || allotIds == null || allotIds == "") {
        DA_Alert("请选择任务", function () { });
        return;
    }
    DA_Confirm("提示", "是否确定撤销查勘", function (ok) {
        if (ok) {
            var paraJson = { allotIds: allotIds };
            $.extendAjax(
                        {
                            url: "/AllotFlowInfo/AllotFlowManager_CancelSurvey_Api",
                            data: paraJson,
                            type: "post",
                            dataType: "json"
                        },
                        function (data) {
                            if (!data.result) {
                                DA_Alert(decodeURIComponent(data.message), function () { });
                                return;
                            }
                            else {
                                alert("操作成功!");
                                SubmitData_CancelSurvey_Response(allotIds);

                            }
                        },
                       { dom: "#allotFlowPanel" });
        }
    });
}
/******撤销查勘后,该任务状态改为"已分配",显示"撤销任务"操作按钮*/
function SubmitData_CancelSurvey_Response(allotIds) {
    var codename = "";
    if ($("#selectStatus").find(".statusInfo_" + allotStatus2).html() != "") {
        codename = $("#selectStatus").find(".statusInfo_" + allotStatus2).html();
    }
    if (allotIds != null && allotIds != "") {
        var _ids = allotIds.split(',');
        for (var i = 0; i < _ids.length; i++) {
            $("#allotFlowRowList").find("#allotFlowRow_" + _ids[i]).find(".txt_status").html(codename);
            $("#allotFlowRowList").find("#allotFlowRow_" + _ids[i]).find(".txt_status").attr("data-statusId", allotStatus2);
            BindAllotOpShow($("#allotFlowRowList").find("#allotFlowRow_" + _ids[i]), _ids[i], allotStatus2);
        }
        var status = $("#selectStatus").val();
        //if (status != "0" && status != 0)
        {
            $("#hdAllotFlowCount").val("");
            GetAllotFlowList(1);
        }
    }
}
/***************************************************************Common************************************/
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
/**********验证当前用户是否有指定操作权限****************/
function CheckFunctionCodes(functionCode) {
    var doms = $("#divFunctionCodes").find(".functioncode_" + functionCode);
    if (doms.length > 0) {
        return true;
    }
    return false;
}

//片区下拉列表
function select_subarea() {
    var areaid = $("#selectArea").val();
    $.get('/AllotFlowInfo/GetSubAreaSelect', { areaid: areaid }, function (data) {
        $("#selectSubArea").val(null).trigger("change");
        $("#selectSubArea").html("");
        $.each(data, function (i, item) {
            if (item.Selected) {
                $("#selectSubArea").append($("<option selected=\"selected\" class=subareaId_" + item.Value + "></option>").val(item.Value).html(item.Text));
            } else {
                $("#selectSubArea").append($("<option class=subareaId_" + item.Value + "></option>").val(item.Value).html(item.Text));
            }
        });
    });
}