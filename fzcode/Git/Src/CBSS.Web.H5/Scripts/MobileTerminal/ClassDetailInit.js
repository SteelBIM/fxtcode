/// <reference path="../../../AppTheme/js/jquery-1.11.2.min.js" />

var ClassDetailInit = function () {
    var Current = this;
    this.ClassID = ''; //班级ID
    this.DeleteIDStr = [];

    function popup(aaa) {
        var black = document.createElement("div");
        var black1 = document.createElement("div");
        var black2 = document.createElement("div");
        var black3 = document.createElement("p");
        var black4 = document.createTextNode(aaa);
        black.className = 'zong';
        black1.className = 'zhezhao';
        black2.className = 'hezi';
        black3.appendChild(black4);
        black2.appendChild(black3);
        black.appendChild(black1);
        black.appendChild(black2);
        document.body.appendChild(black);
        black1.onclick = function () {
            black.parentNode.removeChild(black);

        }
    };

    this.Init = function () {
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.ClassID = Common.QueryString.GetValue("ClassID");
        Current.ClassName = decodeURI(Common.QueryString.GetValue("ClassName"));
        Current.APPID = window.Common.QueryString.GetValue("AppID");
        //if (Current.UserID == null || Current.UserID == "" || Current.UserID == undefined || Current.UserID == "undefined") {
        //  //  window.location.href = "Login.aspx?Type=2";
        //}
        document.title = Current.ClassName;
        Current.GetStuList();
    };

    //获取班级学生信息
    this.GetStuList = function () {
        Current.DeleteIDStr = [];
        var data = {
            ClassID: Current.ClassID ,
            PKey: "", RTime: Common.DateNow()
        };
        var obj = { FunName: "getStuListByClassID", Info: $.toJSON(data), FunWay: "0", Flag: "", Key: "" }
        $.post("http://192.168.3.1:8026/dc/active", $.toJSON(obj), function (data) {
            var result = eval("(" + data + ")");
            if (result.Success) {
                var html = '';
                var studentList = result.ClassList;
                $("#studentNum").html("班级成员（" + studentList.length + "人）");
                for (var i = 0, length = studentList.length; i < length; i++) {
                    html += '<div class="line-wrapper">';
                    html += '<div class="line-scroll-wrapper">';
                    html += '<div class="quanxuan">';
                    html += '<img id="' + studentList[i].UserId + '" src="../../Content/AppTheme/images/xuan1.png" alt="" /></div>';
                    html += '<div class="line-normal-wrapper">';
                    html += '<div class="line-normal-left-wrapper">';
                    html += '<div class="line-normal-avatar-wrapper">';
                    html += '<img src="' + (studentList[i].AvatarUrl == "00000000-0000-0000-0000-000000000000" ? "../../Content/AppTheme/images/tou.png" : studentList[i].AvatarUrl) + '" />';
                    html += '</div>';
                    html += '<div class="line-normal-info-wrapper">';
                    html += '<div class="line-normal-user-name">' + (studentList[i].TrueName == "" ? "暂未填写" : studentList[i].TrueName) + '</div>';
                    html += '</div>';
                    html += '</div>';
                    html += '</div>';
                    html += '<div class="line-btn-delete">';
                    html += '<a id="' + studentList[i].UserId + '"><img src="../../Content/AppTheme/images/delete.png" alt="" /><p>删除</p></a>';
                    html += '</div>';
                    html += '</div>';
                    html += '</div>';
                }
                $(".studentList").html('');
                $(html).prependTo(".studentList");
                var selectNum = 0;
                $(".line-wrapper .quanxuan img").click(function (e) {
                    if ($(this).attr("src") == "../../Content/AppTheme/images/xuan1.png") {
                        selectNum++;
                        Current.DeleteIDStr.push($(this).attr("id"));
                        $(this).attr("src", "../../Content/AppTheme/images/xuan2.png");
                    } else {
                        selectNum--;
                        var index = Current.DeleteIDStr.indexOf($(this).attr("id"));
                        Current.DeleteIDStr.splice(index, 1);
                        $(this).attr("src", "../../Content/AppTheme/images/xuan1.png");
                    }
                    $("#stuNum").html(selectNum);
                });

                $(".line-btn-delete a").click(function () {
                    Current.DeleteIDStr = [];
                    var id = $(this).attr("id");
                    Current.DeleteIDStr.push(id);
                    $(".box3").css("display", "block");
                    $(".black_overlay1").css("display", "block");
                })
                init();
                Current.CancelEdit();
            } else {
                $("#studentNum").html("班级成员（0人）");
            }
        });
    }

    $("#close").click(function () {
        //调用移动端接口
        var data = {
            ////传递的参数json
            //"data": {
            //    "MessageStr": MessageStr

            //}

        };
        //调用移动端的方法
        window.WebViewJavascriptBridge.callHandler(
            'finish', data, function (responseData) {

            }
        );
    });

    //取消编辑
    this.CancelEdit = function () {
        $(".line-wrapper .quanxuan").css("display", "none");
        $(".chuangjian").css("display", "block");
        $(".chuangjian1").css("display", "none");
        $(".Html6 .yaoqing").css("display", "block");
        $(".Html6 .shanchu").css("display", "none");
        $(".line-btn-delete a").css("display", "block");
        var imgArr = $(".line-wrapper .quanxuan img");
        for (var i = 0; i < imgArr.length; i++) {
            if ($(imgArr[i]).attr("src") == "../../Content/AppTheme/images/xuan2.png") {
                $(imgArr[i]).attr("src", "../../Content/AppTheme/images/xuan1.png");
            }
        }
        $("#stuNum").html('0');
        Current.DeleteIDStr = [];
    }

    //编辑按钮
    $("#edit").click(function () {
        $(".line-wrapper .quanxuan").css("display", "block");
        $(".chuangjian").css("display", "none");
        $(".chuangjian1").css("display", "block");
        $(".Html6 .yaoqing").css("display", "none");
        $(".Html6 .shanchu").css("display", "block");
        $(".line-btn-delete a").css("display", "none");
    })

    //取消编辑
    $("#cancel").click(function (e) {
        Current.CancelEdit();
    });

    //删除学生（多选）
    $("#deleteStudents").click(function () {
        Current.DeleteStudent();
    })

    $("#cancelDeleteStudents").click(function () {
        $(".box2").css("display", "none");
        $(".black_overlay1").css("display", "none");
    })

    //删除学生（单选）
    $("#cancelDeleteStudent").click(function () {
        $(".box3").css("display", "none");
        $(".black_overlay1").css("display", "none");
    })

    $("#deleteStudent").click(function () {
        Current.DeleteStudent();
    })


    $(".shanchu").click(function () {
        if (Current.DeleteIDStr.length == 0) {
            popup("请先选中需要删除的学生");
            return false;
        }
        $(".box2").css("display", "block");
        $(".black_overlay1").css("display", "block");
    });

    //删除学生
    this.DeleteStudent = function () {
        var idStr = '';
        for (var i = 0, length = Current.DeleteIDStr.length; i < length; i++) {
            if (i == Current.DeleteIDStr.length - 1) {
                idStr += Current.DeleteIDStr[i];
            } else {
                idStr += Current.DeleteIDStr[i] + ',';
            }
        }
        var data = {
            IDStr: idStr, ClassID: Current.ClassID ,
            PKey: "", RTime: Common.DateNow()
        };
        var obj = { FunName: "DeleteStudentList", Info: $.toJSON(data), FunWay: "0", Flag: "", Key: "" }
        $.post("http://192.168.3.1:8026/dc/active", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")")
                if (result.Success) {
                   
                    window.location.reload();
                } else {
                    popup("学生删除失败，请重试");
                }
            } else {
                popup("发送请求失败，请重新发送");
            }
        });
    }

    $("#InviteStu").click(function () {
        
        var data = {
            UserID: Current.UserID, APPID: Current.APPID,
            PKey: "", RTime: Common.DateNow()
        };
        var obj = { FunName: "GetUserName", Info: $.toJSON(data), FunWay: "0", Flag: "", Key: "" }
        $.post("http://192.168.3.1:8026/dc/active", $.toJSON(obj), function (data) {
            if (objdata) {
                var result = JSON.parse(objdata);
                if (result.Success) {
                    Current.trueName = result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName;
                    Current.vName = result.VersionName;

                    var url = "183.47.42.221:8001";
                    //调用移动端接口
                    var data = {
                        //传递的参数json
                        "data": {
                            "shareUrl": "http://" + url + "/Class/InviteStudent.html?UserID=" + Current.UserID + "&AppID=" + Current.APPID,
                            "text": "加入班级",
                            "title": "我是英语老师" + Current.trueName + ",请下载金太阳同步学" + Current.vName + ",赶紧加入班级让我们一起学习"
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


    //$("#InviteStu").click(function () {
    //    var obj = { UserID: Current.UserID, APPID: Current.APPID };
    //    $.post("../Handler/WeChatHandler.ashx?queryKey=getusername", obj, function (data) {
    //        if (data) {
    //            var result = JSON.parse(data); //eval("(" + data + ")");
    //            if (result.Success) {
    //                $("#teaName_1").html(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
    //                $("#teaName_2").html(result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName);
    //                Current.UserTrueName = result.UserInfo.TrueName == null ? '' : result.UserInfo.TrueName;
    //                Current.SchoolID = result.UserInfo.SchoolID;
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
}

var classDetailInit;
$(function () {
    classDetailInit = new ClassDetailInit();
    classDetailInit.Init();
});