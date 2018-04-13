var ClassList = function () {
    var Current = this;
    this.UserID = Common.QueryString.GetValue("UserID");
    this.Init = function () {
        //获取班级列表
        Current.GetClassList();

    };

    //获取班级列表 
    this.GetClassList = function () {
        if (Current.UserID != undefined) {
            var obj = { UserID: Current.UserID, ClassIDs: "" };//"33299631"
            $.post("../Handler/WeChatHandler.ashx?queryKey=getuserclassbyuserid", obj, function (data) {
                if (data) {
                    var result = JSON.parse(data);
                    if (result.Success) {
                        //加载下拉班级列表  <li>一年级1班</li>
                        var classhtml = "";
                        $.each(result.ClassList, function () {
                            classhtml += "<li ClassID=\"" + this.Id + "\" ClassNum=\"" + this.ClassNum + "\" GradeID=\"" + this.GradeId + "\"><a href=\"Volume.aspx?ClassID=" + this.Id + "&GradeID=" + this.GradeId + "\">" + this.ClassName + "</a></li>";
                        });
                        $("#div_Class").html(classhtml);
                    }
                    else {
                        window.location.href = "ClassMent.aspx?UserID=" + Current.UserID;
                    }
                }
            });
        }
    };
};

var classlistInit;
$(function () {
    classlistInit = new ClassList();
    classlistInit.Init();
});