$(function () {
    var nowDepartment = $("#hdNowDepartment").val();
    var viewType = $("#hdViewType").val();
    var nowUserName = $("#hdNowUserName").val();
    $("#selectDepartment").val(nowDepartment);
    if (viewType == "all") {

    }
    else if (viewType == "department") {
        $("#selectDepartment").attr("disabled", "disabled");
    }
    else {
        $("#txtKeyword").val(nowUserName);
        $("#txtKeyword").attr("disabled", "disabled");
    }
    $("#btnSubmit").bind("click", function () {
        SubmitData();
    });
    /***点击查询***/
    $("#btnSearch").bind("click", function () {
        $("#hdUserCount").val("");
        GetUserList(1);
    });
    GetUserList(1);
});

/******查询用户列表********/
function GetUserList(pageIndex) {
    var keyWord = $("#txtKeyword").val();
    if (keyWord != null && keyWord != "") {
        var _Pat = /^[\u4e00-\u9fa5,\a-zA-Z0-9,\@]*$/;
        var bol = _Pat.test(keyWord);
        if (!bol) {
            alert("不能输入特殊字符,请重新输入");
            return;
        }
    }
    var departmentId = $("#selectDepartment").val();
    var roleId = "";
    if (departmentId == 0 || departmentId == "0") {
        departmentId = "";
    }
    if (roleId == 0 || roleId == "0") {
        roleId = "";
    }
    var pageSize = 10;
    var isGetCount = 1;
    if ($("#hdUserCount").val() != "") {
        isGetCount = 0;
    }
    var paraJson = { keyWord: keyWord, roleId: roleId, departmentId: departmentId, pageIndex: pageIndex, pageSize: pageSize, isGetCount: isGetCount };
    $.extendAjax(
                {
                    url: "/AllotFlowInfo/AssignAllotToUser_GetList_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    $("#userRowList").find(".userRow").remove();
                    if (data != null) {
                        if (data.result != 1) {
                            if (data.errorType == 0) {
                                alert(decodeURIComponent(data.message));
                            }
                            return;
                        }
                        if ($("#hdUserCount").val() == "") {
                            $("#hdUserCount").val(data.detail.Count);
                        }
                        if (data.detail.List != null) {
                            var projectIds = "";
                            var list = data.detail.List;
                            for (var i = 0; i < list.length; i++) {
                                var userObj = list[i];
                                var dom = BindUserDom(userObj);
                                $("#userRowList").append(dom);
                            }

                        }
                        var count_data = $("#hdUserCount").val();
                        var pageCount = parseInt(((count_data * 1) - 1) / pageSize) + 1;
                        BindPage(pageIndex, pageSize, count_data);
                    }
                },
               { dom: "#userPanel" });
}
/*******绑定用户行html信息*****/
function BindUserDom(userObj) {
    var dom = $("#userRowList").find("#userRow").clone();
    for (var item in userObj) {
        dom.find(".txt_" + item).html(userObj[item]);
    }
    var uservalid = "正常";
    if (userObj.uservalid == 0) {
        uservalid = "禁用";
    }
    dom.find(".txt_uservalid").html(uservalid);
    dom.find(".cb_select").val(userObj.username + "," + userObj.truename);
    dom.attr("id", "userRow_" + userObj.username.replace("@", "-")).addClass("userRow").show();
    return dom;
}
/**绑定分页**/
function BindPage(nowIndex, pageSize, count) {
    BindPageCommon("#example", nowIndex, count, pageSize, 10,
                                      function (event, originalEvent, type, page) {
                                          GetUserList(page);
                                      }, null);
}


function SubmitData() {
    var selectUserInfo = $("input[name=selectUserName]:checked").val();
    var allotIds = $("#hdAllotIds").val();
    if ($("input[name=selectUserName]:checked").length < 1 || allotIds == null || allotIds == "") {
        DA_Alert("请选择用户和任务", function () { });
        return;
    }

    var selectUserName = selectUserInfo.split(',')[0];
    var selectUserTrueName = selectUserInfo.split(',')[1];

    var paraJson = { allotIds: allotIds, selectUserName: selectUserName, selectUserTrueName: selectUserTrueName };
    $("#btnSubmit").val("提交中...");
    $.extendAjax(
                {
                    url: "/AllotFlowInfo/AssignAllotToUser_SubmitData_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    $("#btnSubmit").val("提交");
                    if (!data.result) {
                        DA_Alert(decodeURIComponent(data.message), function () { });
                        return;
                    }
                    else {
                        alert("保存成功!");
                        parent.AssignAllotToUser_Response(allotIds);
                        parent.$.fancybox.close();
                    }
                },
               { dom: "#SubmitPanel" });
}