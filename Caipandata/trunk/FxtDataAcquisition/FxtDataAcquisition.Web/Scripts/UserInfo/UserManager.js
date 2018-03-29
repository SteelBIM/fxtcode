var uppdateRight = 0;
$(function () {
    uppdateRight = $("#hdUpdateRight").val();
    GetUserList(1);
    /*******点击搜索*****/
    $("#btnSearch").bind("click", function () {
        $("#hdUserCount").val("");
        GetUserList(1);
    });
    /********编辑行*********/
    $("#userRowList").find(".btn_edit").live("click", function () {
        var username = $(this).attr("data-username");
        EditUser(username);
    });
});
/******查询用户列表********/
function GetUserList(pageIndex) {
    var keyWord = $("#txtKeyword").val();
    if (keyWord != null && keyWord != "") {
        var _Pat = /^[\u4e00-\u9fa5,\a-zA-Z0-9]*$/;
        var bol = _Pat.test(keyWord);
        if (!bol) {
            alert("不能输入特殊字符,请重新输入");
            return;
        }
    }
    var departmentId = $("#selectDepartment").val();
    var roleId = $("#selectRole").val();
    var pageSize = 10;
    var paraJson = { keyWord: keyWord, roleId: roleId, departmentId: departmentId, pageIndex: pageIndex, pageSize: pageSize };
    $.extendAjax(
                {
                    url: "/UserInfo/UserManager_GetList_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    $("#userRowList").find(".userRow").remove();
                    if (data.result) {
                        if ($("#hdUserCount").val() == "") {
                            $("#hdUserCount").val(data.data.Count);
                        }
                        if (data.data.List != null) {
                            var projectIds = "";
                            var list = data.data.List;
                            for (var i = 0; i < list.length; i++) {
                                var userObj = list[i];
                                var dom = BindUserDom(userObj);
                                $("#userRowList").append(dom);
                            }

                        }
                        var count_data = $("#hdUserCount").val();
                        var pageCount = parseInt(((count_data * 1) - 1) / pageSize) + 1;
                        BindPage(pageIndex, pageSize, count_data);
                    } else {
                        alert(decodeURIComponent(data.message));
                    }
                },
               { dom: "#userPanel" });
}
/*******绑定用户行html信息*****/
function BindUserDom(userObj) {
    var dom = $("#userRowList").find("#userRow").clone();
    userObj.rolenames = userObj.rolenames.TrimEnd(',');
    for (var item in userObj) {
        dom.find(".txt_" + item).html(userObj[item]);
    }
    var uservalid = "正常";
    if (userObj.uservalid == 0) {
        uservalid = "禁用";
    }
    dom.find(".txt_uservalid").html(uservalid);
    dom.find(".btn_edit").hide();
    dom.find(".btn_edit").attr("data-username", userObj.username)
    if (uppdateRight == 1) {
        dom.find(".btn_edit").show();
    }
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
/*******打开编辑用户***********/
function EditUser(userName) {
    var url = "/UserInfo/EditUser?userName={0}"
    url = url._StringFormat(userName);
    $.fancybox({
        'href': url,/**"/OperationMaintenance/SetCase_Fancybox?fxtCityId=6&projectId=45772&caseId="+caseId+"&buildingTypeCode=2003004&purposeCode=1002001&areaTypeCode=8006004&date=2012-6",**/
        'width': 600,
        'height': 320,
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
/*******编辑用户成功后响应方法***********/
function EditUser_Response(data) {
    $("#userRowList").find("#userRow_" + data.username.replace("@", "-")).find(".txt_departmentname").html(data.departmentname);
    $("#userRowList").find("#userRow_" + data.username.replace("@", "-")).find(".txt_rolenames").html(data.rolename.TrimEnd(','));
}
