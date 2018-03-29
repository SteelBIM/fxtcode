$(function () {
    $("#btnSubmit").bind("click", function () {
        SubmitData();
    });
});
function SubmitData() {
    $("#errorAlert").hide();
    var userName = $("#txtUserName").val();
    var trueName = $("#txtTrueName").val();
    var departmentId = $("#selectDepartment").val();
    if (!departmentId) {
        alert("请选择小组或创建小组");
        return false;
    }
    var roleIds = GetCheckValueArrayByFind($("#cbRoleIds"), ',');
    var paraJson = { userName: userName, trueName: trueName, departmentId: departmentId, roleIds: roleIds };
    $("#btnSubmit").val("提交中...");
    $.extendAjax(
                {
                    url: "/UserInfo/EditUser_SubmitData_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    $("#btnSubmit").val("提交");

                    if (!data.result) {
                        $("#errorAlert").show();
                        $("#errorAlert").html(decodeURIComponent(data.message));
                        return;
                    }
                    else {
                        alert("保存成功!");
                        parent.EditUser_Response(data.data);
                        parent.$.fancybox.close();
                    }
                },
               { dom: "#SubmitPanel" });
}

function LocationDer(url) {
    parent.$.fancybox.close();
    parent.window.location.href = "/#" + url + "";
}