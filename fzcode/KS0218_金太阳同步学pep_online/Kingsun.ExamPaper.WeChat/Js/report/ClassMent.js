var ClassMent = function () {
    var Current = this;
    this.UserID = Common.QueryString.GetValue("UserID");
    this.Init = function () {

    };

    $("#create").click(function () {
        var obj = { UserID: Current.UserID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=GetUserInfoByUserId", obj, function (data) {
            if (data) {
                var result = JSON.parse(data);
                if (result.Success) {
                    window.location.href = "../ClassManagement/CreateClass.aspx?UserID=" + result.UserInfo.UserID + "&SchoolID=" + result.UserInfo.SchoolID;
                } else {
                    return false;
                }
            }
        });
    });
};

var classmentInit;
$(function () {
    classmentInit = new ClassMent();
    classmentInit.Init();
});