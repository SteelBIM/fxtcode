/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var ClassListInit = function () {
    var current = this;
    this.SchoolID = ''; //学校ID
    this.UserID = ''; //用户ID
    this.DeleteIDStr = [];
    this.Telephone = '';
    this.LogTime = 0;
    this.ClassNum = 0;
    this.trueName = "";
    this.vName = "";
    this.selectNum = 0;
    this.APPID = "";

    //function popup(aaa) {
    //    var black = document.createElement("div");
    //    var black1 = document.createElement("div");
    //    var black2 = document.createElement("div");
    //    var black3 = document.createElement("p");
    //    var black4 = document.createTextNode(aaa);
    //    black.className = 'zong';
    //    black1.className = 'zhezhao';
    //    black2.className = 'hezi';
    //    black3.appendChild(black4);
    //    black2.appendChild(black3);
    //    black.appendChild(black1);
    //    black.appendChild(black2);
    //    document.body.appendChild(black);
    //    black1.onclick = function () {
    //        black.parentNode.removeChild(black);

    //    };
    //};


    this.Init = function () {
        var loginState = "";
        current.UserID = window.Common.QueryString.GetValue("UserID");
        loginState = window.Common.QueryString.GetValue("UserID");
        current.APPID = window.Common.QueryString.GetValue("AppID");
        current.IsEnableOss = window.Common.QueryString.GetValue("IsEnableOss");
        //current.GetUserInfo();
        if (loginState === "" || loginState == null || loginState === "undefined") {
            //window.location.href = "Login.aspx?Type=2";
        } else {
            //Current.GetUserIDByPhoneNumber();
            //current.GetUserInfo();
            current.GetClassList();

        }
    };

    //获取老师班级列表
    this.GetClassList = function () {
        current.DeleteIDStr = [];
        var obj = { UserID: current.UserID, SubjectID: 3 };
        $.post("../Handler/WeChatHandler.ashx?queryKey=queryClassList", obj, function (data) {
            if (data) {
                var result = JSON.parse(data); //eval("(" + data + ")")
                if (result.Success) {
                    var classList = result.ClassList;
                    current.ClassNum = classList.length;

                    if (classList.length <= 0) {
                        window.location.href = "CreateClass.aspx?UserID=" + current.UserID;
                        return;
                    }

                    current.DeleteIDStr = [];
                    current.SchoolID = classList[0].SchoolID;
                    var html = "";
                    for (var i = 0, length = classList.length; i < length; i++) {
                        html += '<div class="line-wrapper">';
                        html += '<div class="line-scroll-wrapper">';
                        html += '<div class="quanxuan">';
                        html += '<img id="' + classList[i].Id + '" src="../AppTheme/images/xuan1.png" alt="" />';
                        html += '</div>';
                        html += '<div class="line-normal-wrapper">';
                        html += '<a class="a_a" href="ClassDetail.aspx?ClassID=' + classList[i].Id + '&ClassName=' + encodeURI(encodeURI(classList[i].ClassName)) + '&UserID=' + current.UserID + '&AppID=' + current.APPID + '">';
                        html += '<div class="line-normal-left-wrapper">';
                        html += '<div class="line-normal-avatar-wrapper">';
                        html += '<img src="../AppTheme/images/tou.png" />';
                        html += '</div>';
                        html += '<div class="line-normal-info-wrapper">';
                        html += '<div class="line-normal-user-name"> ' + classList[i].ClassName + '</div>';
                        html += '</div>';
                        html += '</div>';
                        html += '<div class="line-normal-icon-wrapper">';
                        html += '<img src="../AppTheme/images/zongpeo.png" /><span>' + classList[i].StudentNum + '人</span>'; //<b>新增0人<img src="images/tishi.png" alt="" /></b>
                        html += '</div></a>';
                        html += '</div>';
                        html += '<div class="line-btn-delete">';
                        html += '<a id="' + classList[i].Id + '"><img src="../AppTheme/images/delete.png" alt="" /><p>删除</p></a>';
                        html += '</div>';
                        html += '</div>';
                        html += '</div>';
                    }
                    $(".classList").html('');
                    $(html).prependTo(".classList");

                    $(".line-wrapper .quanxuan img").click(function () {
                        if ($(this).attr("src") === "../AppTheme/images/xuan1.png") {
                            current.selectNum++;
                            current.DeleteIDStr.push($(this).attr("id"));
                            $(this).attr("src", "../AppTheme/images/xuan2.png");
                        } else {
                            current.selectNum--;
                            var index = current.DeleteIDStr.indexOf($(this).attr("id"));
                            current.DeleteIDStr.splice(index, 1); //删除班级ID
                            $(this).attr("src", "../AppTheme/images/xuan1.png");
                        }
                        $("#classNum").html(current.selectNum);
                    });

                    $(".line-btn-delete a").click(function () {
                        current.DeleteIDStr = [];
                        var id = $(this).attr("id");
                        current.DeleteIDStr.push(id);
                        $(".box3").css("display", "block");
                    });
                    window.init();
                    current.CancelEdit();
                } else {
                    //$(".classList").html('');
                    ////判断是否为首次登陆
                    //if (current.LogTime === 0) {
                    window.location.href = "CreateClass.aspx?UserID=" + current.UserID + "&AppID=" + current.APPID;
                    //}
                }
            } else {
                popup("发送请求失败，请重新发送");
            }
        });
    };
    $("#cancelDeleteClass").click(function () {
        $(".box3").css("display", "none");
    });
    $("#deleteClass").click(function () {
        current.DeleteClass();
    });

    //删除班级
    this.DeleteClass = function () {
        if (current.DeleteIDStr.length > 0) {
            var idStr = '';
            for (var i = 0, length = current.DeleteIDStr.length; i < length; i++) {
                if (i === current.DeleteIDStr.length - 1) {
                    idStr += current.DeleteIDStr[i];
                } else {
                    idStr += current.DeleteIDStr[i] + ',';
                }
            }
            var obj = { IDStr: idStr, UserID: current.UserID, SubjectID: 3 };
            $.post("../Handler/WeChatHandler.ashx?queryKey=deleteClassList", obj, function (data) {
                if (data) {
                    var result = eval("(" + data + ")");
                    if (result.Success) {
                        $(".box2").css("display", "none");
                        $(".box3").css("display", "none");
                        current.LogTime++;
                        current.CancelEdit();
                        current.GetClassList();
                        //调用移动端接口
                        var mdata = {
                            //传递的参数json
                            "data": {
                                "Count": result.Count
                            }
                        };
                        //调用移动端的方法
                        window.WebViewJavascriptBridge.callHandler(
                            'GetClassCount', mdata, function (responseData) {

                            }
                        );
                        if (result.Count <= 0) {
                            window.location.href = "CreateClass.aspx?UserID=" + current.UserID + "&AppID=" + current.APPID;
                        } else {
                            window.location.reload();
                        }
                    } else {
                        popup(result.ErrMsg);
                    }
                } else {
                    popup("发送请求失败，请重新发送");
                }
            });
        } else {
            popup("请先选中需要删除的班级");
        }
    };

    //编辑班级按钮
    $("#edit").click(function () {
        $(".line-wrapper .quanxuan").css("display", "block");
        $(".dianji").css("display", "none");
        $(".dianji1").css("display", "block");
        $(".footer .yaoqing").css("display", "none");
        $(".footer .shanchu").css("display", "block");
        $(".line-btn-delete a").css("display", "none");
    });

    //取消编辑
    $("#cancel").click(function () {
        current.CancelEdit();
    });

    $(".shanchu").click(function () {
        $(".box2").css("display", "block");
    });

    //取消编辑
    this.CancelEdit = function () {
        $(".line-wrapper .quanxuan").css("display", "none");
        $(".dianji").css("display", "block");
        $(".dianji1").css("display", "none");
        $(".footer .yaoqing").css("display", "block");
        $(".footer .shanchu").css("display", "none");
        $(".line-btn-delete a").css("display", "block");
        var imgArr = $(".line-wrapper .quanxuan img");
        for (var i = 0; i < imgArr.length; i++) {
            if ($(imgArr[i]).attr("src") === "../AppTheme/images/xuan2.png") {
                $(imgArr[i]).attr("src", "../AppTheme/images/xuan1.png");
            }
        }
        current.DeleteIDStr = [];
        $("#classNum").html('0');
        current.selectNum = 0;
    };

    //删除班级（多选）
    $("#deleteClasses").click(function () {
        current.DeleteClass();
    });
    $("#cancelDeleteClasses").click(function () {
        $(".box2").css("display", "none");
    });

    $("#InviteStu").click(function () {

        var obj = { UserID: current.UserID, APPID: current.APPID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=getusername", obj, function (objdata) {
            if (objdata) {
                var result = JSON.parse(objdata);
                if (result.Success) {
                    current.trueName = result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName;
                    current.vName = result.VersionName;

                    var url = "183.47.42.221:8001";
                    //调用移动端接口
                    var data = {
                        //传递的参数json
                        "data": {
                            "shareUrl": "http://" + url + "/StudyReportManagement/InviteStudent.aspx?UserID=" + current.UserID + "&AppID=" + current.APPID,
                            "text": "加入班级",
                            "title": "我是英语老师" + current.trueName + ",请下载金太阳同步学" + current.vName + ",赶紧加入班级让我们一起学习"
                        }
                    };
                    //调用移动端的方法
                    window.WebViewJavascriptBridge.callHandler(
                        'shareThirdparty', data, function (responseData) {

                        }
                    );
                }
            }
        });

    });

    $("#me").click(function () {
        window.location.href = "TeacherInfo.aspx?UserID=" + current.UserID + "&AppID=" + current.APPID;
    });

    $("#close").click(function () {
        //调用移动端接口
        var data = {

        };
        //调用移动端的方法
        window.WebViewJavascriptBridge.callHandler(
            'finish', data, function (responseData) {

            }
        );
    });

    //$("#InviteStu").click(function () {
    //    var obj = { UserID: current.UserID, APPID: current.APPID };
    //    $.post("../Handler/WeChatHandler.ashx?queryKey=getusername", obj, function (data) {
    //        if (data) {
    //            var result = JSON.parse(data); //eval("(" + data + ")");
    //            if (result.Success) {
    //                $("#teaName_1").html(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
    //                $("#teaName_2").html(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
    //                current.UserTrueName = result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName;
    //                current.SchoolID = result.UserInfo.SchoolID;
    //                if (result.UserInfo.AvatarUrl !== '00000000-0000-0000-0000-000000000000') {
    //                    $("#userImg").attr("src", window.Constant.file_Url + 'GetFiles.ashx?FileID=' + result.UserInfo.AvatarUrl);
    //                }

    //                if (result.VersionName != null) {
    //                    $("#edition").html("金太阳同步学" + result.VersionName);
    //                    $("#overlay").css("display", "block");
    //                } else {
    //                    $("#overlay").css("display", "block");
    //                }
    //            }
    //        }
    //    });
    //});

    //$("#overlay").click(function () {
    //    $(this).css("display", "none");
    //});

    $("#creatClass").click(function () {
        var obj = { UserID: current.UserID, APPID: current.APPID };
        $.post("../Handler/WeChatHandler.ashx?queryKey=getusername", obj, function (data) {
            if (data) {
                var result = JSON.parse(data);//eval("(" + data + ")");
                if (result.Success) {
                    current.SchoolID = result.UserInfo.SchoolID;
                    if (result.UserInfo.SchoolID != null && result.UserInfo.SchoolID !== "" && result.UserInfo.SchoolID != 0) {
                        window.location.href = "ChooseClass.aspx?UserID=" + current.UserID + "&SchoolID=" + current.SchoolID;
                    } else {
                        window.location.href = "AddInformation.aspx?UserID=" + current.UserID;
                    }
                }
            }
        });
    });

};
var classListInit;
$(function () {
    classListInit = new ClassListInit();
    classListInit.Init();
});