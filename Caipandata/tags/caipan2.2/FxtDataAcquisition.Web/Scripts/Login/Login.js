$(function () {


    $('body').bind('keyup', function (event) {
        if (event.keyCode == 13) {
            $("#btnSubmit").click();
        }
    });
    $("#btnSubmit").bind("click", function () {
        SubmitLogin();
    });
    if ($("#hdLastPageType").val() == "open") {
        parent.OpenLogin(function (d) {
        });
    }
});
function SubmitLogin() {
    var openType = $("#hdOpenType").val();
    var userName = $("#txtUserName").val();
    var pwd = $("#txtPwd").val();
    if (userName == null || userName == "" || pwd == null || pwd == "") {
        return;
    }
    var paraJson = { userName: userName, pwd: pwd };
    $("#btnSubmit").val("提交中...");
    $.extendAjax(
                {
                    url: "/Login/Login_SubmitDate_Api",
                    data: paraJson,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    $("#btnSubmit").val("提交");
                    if (data != null) {
                        if (data.Result != 1) {
                            alert(decodeURIComponent(data.Message));
                            return;
                        }
                        else {
                            if (openType == "page") {
                                if (data.Data.cityid > 0 ) {
                                    window.location.href = "/Home/Index";
                                } else {
                                    OpenSelectCity(function () {
                                    });
                                }
                            }
                            else {
                                if (data.Data.islastuser == 1) {
                                    parent.$.fancybox.close();
                                }
                                else {
                                    if (data.Data.cityid > 0) {
                                        window.location.href = "/Home/Index";
                                    } else {
                                        OpenSelectCity(function () {
                                        });
                                    }
                                    /*parent.window.location.href=Url_AllotFlowInfo_AllotFlowManager;*/
                                }
                            }

                        }
                    }
                },
               { dom: "#SubmitPanel" });

}

function OpenSelectCity(callback) {
    $.extendAjax(
               {
                   url: "/Shared/Partial_SelectCity",
                   data: { callbackUrl: "/Home/Index" },
                   type: "post",
                   dataType: "html"
               },
               function (data) {
                   $("#SubmitPanel").hide();
                   $("#titleUserInfo").hide();
                   $("#titleCityInfo").show();
                   $("#loginPanel").append(data);

               },
              { dom: "#SubmitPanel" });
}