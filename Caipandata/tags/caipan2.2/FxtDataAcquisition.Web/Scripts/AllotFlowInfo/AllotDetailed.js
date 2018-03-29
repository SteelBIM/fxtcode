var NowProjectId = 0;
var NowAllotId = 0;
var NowStatusId = 0;
var NowCityName = "";
$(function () {

    $("#selectCityPanel").find("input").attr("disabled", "disabled");
    $("#selectCityPanel").find("select").attr("disabled", "disabled");
    NowProjectId = $("#hdProjectId").val() * 1;
    NowAllotId = $("#hdAllotId").val() * 1;
    NowStatusId = $("#hdStatus").val() * 1;
    BindCode();
    BindStep();
    GetProjectInfo();
    $("#buildingList").find(".buildingRow").live("click", function () {
        var buildingId = $(this).attr("data-Id");
        GetHouse(buildingId);
    });
    $("#openProjectInfo").bind("click", function () {
        OpenProjectInfo(NowProjectId, NowAllotId);
    });
    $("#openBuildingInfo").bind("click", function () {
        var buildingId = $("#openBuildingInfo").attr("data-Id");
        OpenBuildingInfo(buildingId, NowAllotId);
    });
    $("#addBuildingInfo").bind("click", function () {
        OpenBuildingInfo(0, NowAllotId, true);
    });
    $("#deleteBuildingInfo").bind("click", function () {
        var buildingId = $("#openBuildingInfo").attr("data-Id");
        deleteBuildingInfo(buildingId, NowAllotId);
    });
    $("#houseList").find(".op_editHouse").live("click", function () {
        //var fxtcompanyId = $("#buildingInfo").find(".op_editHouseList").attr("data-fxtcompanyid");
        //var cityId = $("#buildingInfo").find(".op_editHouseList").attr("data-cityid");
        //var allotId = $("#buildingInfo").find(".op_editHouseList").attr("data-allotId");
        //window.open("/House/HouseDetails?allotId=" + allotId + "&buildingId=" + buildingId + "&fxtcompanyId=" + fxtcompanyId + "&cityId=" + cityId);
        var buildingId = $("#openBuildingInfo").attr("data-Id");
        var houseId = $(this).attr("data-Id");
        OpenHouseInfo(buildingId, houseId, NowAllotId);
    });
    $(".op_addHouseInfo").live("click", function () {
        var buildingId = $("#openBuildingInfo").attr("data-Id");
        OpenHouseInfo(buildingId, 0, NowAllotId);
    });

    $(".op_editHouseList").live("click", function () {
        var buildingId = $(this).attr("data-buildingid");
        var fxtcompanyId = $(this).attr("data-fxtcompanyid");
        var cityId = $(this).attr("data-cityid");
        var allotId = $(this).attr("data-allotId");
        window.open("/House/HouseDetails?allotId=" + allotId + "&buildingId=" + buildingId + "&fxtcompanyId=" + fxtcompanyId + "&cityId=" + cityId);
    });
    $(".op_deleteHouseInfo").bind("click", function () {
        deleteHouseInfo();
    });

    /****执行审核之类的操作****/
    $(".opSubmit").live("click", function () {
        var action = $(this).attr("data-action");
        SubmitOpDate(action);
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
        $("#houseList").find(".houseRow").find("input[type='checkbox']").attr("checked", checkedValue);
        $("#cbAll").attr("checked", checkedValue);
    })

});
function SubmitOpDate(action) {
    var title = "是否确定提交结果";
    if (action == "mynotaudit") {
        title = "是否确定自审不通过?自审不通过后任务将撤回到已分配(待查勘)状态";
    }
    else if (action == "notaudit") {
        title = "是否确定审核不通过?审核不通过后任务将撤回到已查勘状态";
    }
    else if (action == "importdata") {
        title = "";
        //获取楼盘、楼栋、房号数
        $.ajax({
            url: "/AllotFlowInfo/GetProjectBuildingHouseTotal",
            data: { projectId: NowProjectId },
            cache: false,
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.result) {
                    title += "该楼盘在数据中心已存在，楼栋数：" + data.data.buildingtotal + "，房号数：" + data.data.housetotal + "。 ";
                }
            }
        })

        title += "是否确定将信息导入到运维中心";
    }
    var remark = $(".txt_" + action + "_remark").val();
    DA_Confirm("提示", title, function (ok) {
        if (ok) {
            var paraJson = { allotId: NowAllotId, actionType: action, remark: remark };
            $.extendAjax(
                        {
                            url: "/AllotFlowInfo/AllotDetailed_SubmitData_Api",
                            data: paraJson,
                            type: "post",
                            dataType: "json"
                        },
                        function (data) {
                            if (data != null) {
                                if (data.result != 1) {

                                    if (data.errorType == 0) {
                                        DA_Alert(decodeURIComponent(data.message), function () { });
                                    }
                                    return;
                                }
                                else {
                                    alert("操作成功!");
                                    window.location.href = window.location.href;
                                }
                            }
                        },
                       { dom: "#dataPanel" });
        }
    });
}
/****绑定步骤*****/
function BindStep() {
    $("#step_status").find(".step_status").each(function () {
        var stepStatus = $(this).attr("data-status") * 1;
        if (NowStatusId >= stepStatus) {
            $(this).addClass("active");
        }
    });
}
/*****获取楼盘基本信息*******/
function GetProjectInfo() {
    var paraJson = { projectId: NowProjectId };
    $.extendAjax(
                {
                    url: "/AllotFlowInfo/AllotDetailed_GetProjectInfo_Api",
                    data: paraJson,
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
                        if (data.detail != null) {
                            $("#projectName").html(data.detail.projectname);
                            $("#photoCount").html(data.detail.photocount);
                            var dingwei = "未定";
                            if (data.detail.x > 0 && data.detail.y > 0) {
                                dingwei = "已定";
                            }
                            $("#dingwei").html(dingwei);
                            $("#statusName").html(data.detail.statusname);
                            if (data.detail.buildinglist != null) {
                                if (data.detail.buildinglist.length > 0) {
                                    $("#buildingInfo").show();
                                }
                                for (var i = 0; i < data.detail.buildinglist.length; i++) {
                                    var obj = data.detail.buildinglist[i];
                                    var dom = $("#buildingList").find("#buildingRow").clone();
                                    dom.show().attr("id", "buildingRow_" + obj.buildingid).addClass("buildingRow");
                                    dom.html(obj.buildingname);
                                    dom.attr("data-Id", obj.buildingid).attr("data-Json", JSON.stringify(obj));
                                    $("#buildingList").append(dom);
                                }
                                if (data.detail.buildinglist.length > 0) {
                                    GetHouse(data.detail.buildinglist[0].buildingid);
                                }
                            }
                        }
                    }
                },
               { dom: "#dataPanel" });
}
/***绑定楼栋预览信息***/
function BindBuildingJson(jsonObj) {
    /**
    $("#txt_buildingname").val(jsonObj.buildingname);
    $("#txt_doorplate").val(jsonObj.doorplate);
    $("#txt_elevatorrate").val(jsonObj.elevatorrate);
    $("#txt_totalfloor").val(jsonObj.totalfloor);
    $("#txt_othername").val(jsonObj.othername);
    $("#txt_averageprice").val(jsonObj.averageprice);
    $("#txt_builddate").val(jsonObj.builddate);
    $("#txt_pricedetail").val(jsonObj.pricedetail);
    **/
    $("#txt_buildingname").html(jsonObj.buildingname);
    $("#del_buildingname").text(jsonObj.buildingname);
    $("#txt_doorplate").html(jsonObj.doorplate);
    $("#txt_elevatorrate").html(jsonObj.elevatorrate);
    $("#txt_totalfloor").html(jsonObj.totalfloor);
    $("#txt_othername").html(jsonObj.othername);
    $("#txt_averageprice").html(jsonObj.averageprice);
    $("#txt_builddate").html(jsonObj.builddate);
    $("#txt_pricedetail").html(jsonObj.pricedetail);

    $(".op_editHouseList").attr("data-buildingid", jsonObj.buildingid);
    $(".op_editHouseList").attr("data-fxtcompanyid", jsonObj.fxtcompanyid);
    $(".op_editHouseList").attr("data-cityid", jsonObj.cityid);

    /***是都带电梯****/
    var iselevator = jsonObj.iselevator;
    if (iselevator == 1) {
        $("#txt_iselevator").html("是");
    }
    else if (iselevator == 0) {
        $("#txt_iselevator").html("否");
    }
    /***绑定建筑结构***/
    for (var i = 0; i < structureCodeListJson.length; i++) {
        var obj = structureCodeListJson[i];
        if (jsonObj.structurecode == obj.code) {
            $("#txt_structurecode").html(obj.codename);
        }
    }
    /***绑定位置***/
    for (var i = 0; i < locationCodeListJson.length; i++) {
        var obj = locationCodeListJson[i];
        if (jsonObj.locationcode == obj.code) {
            $("#txt_locationcode").html(obj.codename);
        }
    }
    /***绑定景观***/
    for (var i = 0; i < sightCodeListJson.length; i++) {
        var obj = sightCodeListJson[i];
        if (jsonObj.sightcode == obj.code) {
            $("#txt_sightcode").html(obj.codename);
        }
    }
}
/******查询房号列表**********/
function GetHouse(buildingId) {
    /***绑定当前楼栋ID***/
    $("#openBuildingInfo").attr("data-Id", buildingId);
    /***选中样式**/
    $("#buildingList").find(".buildingRow").addClass("btn-white");
    $("#buildingList").find("#buildingRow_" + buildingId).removeClass("btn-white");
    /***获取楼栋简单信息***/
    var buildingDom = $("#buildingList").find("#buildingRow_" + buildingId);
    var buildingJson = JSON.parse(buildingDom.attr("data-Json"));
    BindBuildingJson(buildingJson);
    /******获取房号信息******/
    var paraJson = { buildingId: buildingId };
    $("#houseList").find(".houseRow").remove();
    $.extendAjax(
                {
                    url: "/AllotFlowInfo/AllotDetailed_GetHouseList_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    if (data != null) {
                        if (!data.result) {
                            alert(decodeURIComponent(data.message));
                            return;
                        } else {
                            //data.data.sort(function (a, b) {
                            //    return a.floorno > b.floorno ? 1 : a.floorno == b.floorno ? 0 : -1;
                            //});
                            for (var i = 0; i < data.data.length; i++) {
                                var obj = data.data[i];
                                var dom = $("#houseList").find("#houseRow").clone();
                                dom.show().attr("id", "houseRow_" + obj.houseid).addClass("houseRow");
                                /**dom.find(".txt_housename").html(obj.housename);**/
                                dom.find(".txt_floorno").html(obj.floorno);
                                dom.find(".txt_endfloorno").html(obj.endfloorno);
                                dom.find(".txt_unitno").html(obj.unitno);
                                dom.find(".txt_houseno").html(obj.houseno);
                                dom.find(".txt_housetype").html(GetCodeNameByCode(obj.housetypecode));
                                dom.find(".txt_front").html(GetCodeNameByCode(obj.frontcode));
                                dom.find(".txt_sight").html(GetCodeNameByCode(obj.sightcode));
                                dom.find(".txt_remark").html(obj.remark);
                                dom.find(".op_editHouse").attr("data-Id", obj.houseid);
                                dom.find(".cb_select").val(obj.houseid);
                                //dom.find(".op_editHouseList").attr("data-buildingid", obj.buildingid);
                                //dom.find(".op_editHouseList").attr("data-Id", obj.houseid);
                                //dom.find(".op_editHouseList").attr("data-fxtcompanyid", obj.fxtcompanyid);
                                //dom.find(".op_editHouseList").attr("data-cityid", obj.cityid);
                                $("#houseList").append(dom);
                            }
                        }
                    }
                },
               { dom: "#dataHousePanel" });
}
/************************************************code查询*****************************************/
/****绑定code到页面***/
function BindCode() {
    for (var i = 0; i < frontCodeListJson.length; i++) {
        var obj = frontCodeListJson[i];
        var html = "<span class=\"code_" + obj.code + " id_" + obj.id + "\">" + obj.codename + "</span>";
        $("#codeList").append(html);
    }
    for (var i = 0; i < sightCodeListJson.length; i++) {
        var obj = sightCodeListJson[i];
        var html = "<span class=\"code_" + obj.code + " id_" + obj.id + "\">" + obj.codename + "</span>";
        $("#codeList").append(html);
    }
    for (var i = 0; i < houseTypeCodeListJson.length; i++) {
        var obj = houseTypeCodeListJson[i];
        var html = "<span class=\"code_" + obj.code + " id_" + obj.id + "\">" + obj.codename + "</span>";
        $("#codeList").append(html);
    }
    for (var i = 0; i < allotstatuslistjson.length; i++) {
        var obj = allotstatuslistjson[i];
        var html = "<span class=\"code_" + obj.code + " id_" + obj.id + "\">" + obj.codename + "</span>";
        $("#codeList").append(html);
    }
}
/***根据code获取code名称**/
function GetCodeNameByCode(code) {
    var codeObj = $("#codeList").find(".code_" + code);
    if (codeObj.length < 1) {
        return "";
    }
    return codeObj.html();
}

/********************************************各数据详细信息***************************************/

/***楼盘信息****/
function OpenProjectInfo(projectId, allotId) {
    var url = Url_AllotFlowInfo_EditProject_ByProjectIdAndAllotId._StringFormat(projectId, allotId);
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
            location.href = "/AllotFlowInfo/AllotDetailed?allotId=" + allotId;
            //location.reload(true);
        }
    });
}
/***楼栋信息****/
function OpenBuildingInfo(buildingId, allotId, re) {
    var url = Url_AllotFlowInfo_EditBuilding_ByBuildingIdAndAllotId._StringFormat(buildingId, allotId);
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
            if (re) {
                location.href = "/AllotFlowInfo/AllotDetailed?allotId=" + allotId;
                //location.reload(true);
            }
        }
    });
}

/***删除楼栋信息****/
function deleteBuildingInfo(buildingId, allotId) {
    var buildingname = $("#txt_buildingname").text();
    DA_Confirm("删除", "确定要删除楼栋（" + buildingname + "）吗？", function (ok) {
        if (ok) {
            $.post("/AllotFlowInfo/DeleteBuilding", { buildingId: buildingId }, function (data) {
                if (data.result) {
                    AlertFancybox2(data.message, function () {
                        location.reload(true);
                    });
                } else {
                    alert(data.message);
                }
            });
        }
    });
}

function OpenBuildingInfo_Response(jsonObj, buildingId) {
    if (jsonObj != null && jsonObj != "") {
        BindBuildingJson(jsonObj);
        $("#buildingList").find("#buildingRow_" + buildingId).attr("data-Json", JSON.stringify(jsonObj));
        $("#buildingList").find("#buildingRow_" + buildingId).html(jsonObj.buildingname);
    }
}
/***房号信息****/
function OpenHouseInfo(buildingId, houseId, allotId) {
    var url = "/AllotFlowInfo/EditHouse?buildingId=" + buildingId + "&houseId=" + houseId + "&allotId=" + allotId;
    $.fancybox({
        'href': url,
        'width': 900,
        'height': 320,
        'padding': 0,
        'overlayShow': true,
        'autoScale': false,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'type': 'iframe',
        'onClosed': function () {
            //location.href = "/AllotFlowInfo/AllotDetailed?allotId=" + allotId + "&buildingId=" + buildingId;
            GetHouse(buildingId);
            //location.reload(true);
        }
    });
}

/***删除房号信息****/
function deleteHouseInfo() {
    var dom = $("#houseList").find(".houseRow");
    var allotIds = [];
    var checkList = dom.find("input[type='checkbox']");
    for (var i = 0; i < checkList.length; i++) {
        var isSelect = $(checkList.get(i)).attr("checked");
        var val = $(checkList.get(i)).val();
        /**选中 并且为待分配**/
        if (isSelect == "checked") {
            allotIds.push(val);
        }
    }
    if (allotIds.length < 1) {
        AlertFancybox2("请选择要删除的项！", function () {
        });
        return false;
    }
    DA_Confirm("删除", "确定要删除单元室号吗？", function (ok) {
        if (ok) {
            $.post("/AllotFlowInfo/DeleteHouse", { allotIds: allotIds }, function (data) {
                if (data.result) {
                    AlertFancybox2(data.message, function () {
                        location.reload(true);
                    });
                } else {
                    alert(data.message);
                }
            });
        }
    });
}

