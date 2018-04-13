// JavaScript Document

$(function () {
    /*口语课程切换*/
    $(".langcourses .main ul li").click(function () {
        var state = $(this).text();
        if (state == "课程列表") {
            $("#hidenCourseType").val(0);
        } else {
            $("#hidenCourseType").val(1);
            var CoursePeriodUrl = GetQueryString("CourseType");
            if (CoursePeriodUrl == "" || CoursePeriodUrl == null) {
                window.location.href = window.location.href + "&CourseType=1";//.replace("CourseType=0", "CourseType=1");
            } else {
                window.location.href = window.location.href.replace("CourseType=0", "CourseType=1");
            }
        }
        //alert( $("#hidenCourseType").val());
        $(".langcourses .main ul li").removeClass("on");
        $(this).addClass("on");
        var num = $(this).index();
        $(".langcourses .main .move").css("display", "none");
        $(".langcourses .main .chose" + (num + 1)).css("display", "block");
    });
    /*预约弹框111111111111111111*/
    /*预约弹框*/
    $(".room .order1").click(function () {
        var CoursePeriodUrl = GetQueryString("local");
        if (CoursePeriodUrl != null && CoursePeriodUrl.toString().length > 0 && CoursePeriodUrl.toString() == "weixin") {
            location.href = "http://tbx.kingsun.cn/downloadlist.html";
            return;
        }
        //$(".movieclass .box ul").html("");
        $(".movieclass .box").css("display", "block");
        $(".movieclass .shadow1").css("display", "block");
        var CourseID = $(this).attr("courseid");//课程ID
        var CoursePeriodID = $(this).attr("courseperiodid");//课时ID
        var UserID = $(this).attr("userid");//UserID
        //alert(UserID);
        $("#CoursePeriodID").val(CoursePeriodID);
        $("#CourseID").val(CourseID);
        //alert(CoursePeriodID);
        $.post("/CoursePeriod/GetCoursePeriodTime", { CoursePeriodID: CoursePeriodID, UserID: UserID }, function (data) {
            data = eval(data);
            if (data.Success) {
                var courseList = eval(data.Data);
                var liHtml = '';
                var count = 0;
                for (var i = 0; i < courseList.length; i++) {
                    if (courseList[i].CourseState == "已结束") {
                        liHtml += ' <li class="over" CourseType=' + $("#CourseType").val() + ' CourseID=' + CourseID + ' CoursePeriodID=' + CoursePeriodID + ' CoursePeriodTimeID=' + courseList[i].ID + '><em>' + courseList[i].TeacherType + '</em>' + new Date(courseList[i].StartTime.replace(/T/g, ' ').replace(/-/g, "/")).format("hh:mm") + '&mdash;' + new Date(courseList[i].EndTime.replace(/T/g, ' ').replace(/-/g, "/")).format("hh:mm") + '<span>' + courseList[i].CourseState + '</span></li>';
                    } else if (courseList[i].CourseState == "已满") {
                        liHtml += ' <li class="full" CourseType=' + $("#CourseType").val() + '  CourseID=' + CourseID + ' CoursePeriodID=' + CoursePeriodID + ' CoursePeriodTimeID=' + courseList[i].ID + '><em>' + courseList[i].TeacherType + '</em>' + new Date(courseList[i].StartTime.replace(/T/g, ' ').replace(/-/g, "/")).format("hh:mm") + '&mdash;' + new Date(courseList[i].EndTime.replace(/T/g, ' ').replace(/-/g, "/")).format("hh:mm") + '<span>' + courseList[i].CourseState + '</span></li>';
                    } else if (courseList[i].CourseState == "已预约") {
                        liHtml += ' <li class="bespoke" CourseType=' + $("#CourseType").val() + '  CourseID=' + CourseID + ' CoursePeriodID=' + CoursePeriodID + ' CoursePeriodTimeID=' + courseList[i].ID + '><em>' + courseList[i].TeacherType + '</em>' + new Date(courseList[i].StartTime.replace(/T/g, ' ').replace(/-/g, "/")).format("hh:mm") + '&mdash;' + new Date(courseList[i].EndTime.replace(/T/g, ' ').replace(/-/g, "/")).format("hh:mm") + '<span>' + courseList[i].CourseState + '</span></li>';
                    }
                    else { //可预约
                        if (count == 0) {
                            liHtml += ' <li class="on" CourseType=' + $("#CourseType").val() + '  CourseID=' + CourseID + ' CoursePeriodID=' + CoursePeriodID + ' CoursePeriodTimeID=' + courseList[i].ID + '><em>' + courseList[i].TeacherType + '</em>' + new Date(courseList[i].StartTime.replace(/T/g, ' ').replace(/-/g, "/")).format("hh:mm") + '&mdash;' + new Date(courseList[i].EndTime.replace(/T/g, ' ').replace(/-/g, "/")).format("hh:mm") + '</li>';
                        } else {
                            liHtml += ' <li CourseType=' + $("#CourseType").val() + '  CourseID=' + CourseID + ' CoursePeriodID=' + CoursePeriodID + ' CoursePeriodTimeID=' + courseList[i].ID + '><em>' + courseList[i].TeacherType + '</em>' + new Date(courseList[i].StartTime.replace(/T/g, ' ').replace(/-/g, "/")).format("hh:mm") + '&mdash;' + new Date(courseList[i].EndTime.replace(/T/g, ' ').replace(/-/g, "/")).format("hh:mm") + '</li>';
                        }
                        count++;
                    }
                }
                $(".movieclass .box ul").html(liHtml);

                if ($(".movieclass .box ul li").hasClass("on")) {
                    $(".movieclass .box a").css("background-color", "#FFB200");
                }
                else {
                    $(".movieclass .box a").css("background-color", "#B5B4B4");
                }
            }
        });
    });
    /*弹出框课程时段选择*/
    $(document).on("click", ".movieclass .box ul li", function () {
        if ($(this).hasClass("over")) {
            return;
        }
        if ($(this).hasClass("full")) {
            return;
        }
        if ($(this).hasClass("bespoke")) {
            return;
        }
        else {
            $(".movieclass .box ul li").removeClass("on");
            $(this).addClass("on");
        }
    });

    /*选择预定时间下一步*/
    $(".movieclass .box a").click(function () {
        if (!$(".movieclass .box ul li").hasClass("on")) {
            return false;
        }
        //window.location.href = "/ConfirmInfo/Index?TrueName=科比&TelePhone=18888888888";
        //return;
        //1.判断数据库Tb_UserInfo表是否记录当前用户姓名和电话 
        $.post("/CoursePeriod/GetUserInfo", { UserID: $("#UserID").val() }, function (data) {
            //alert(data);
            data = eval(data);
            if (data.Success) {
                var courseList = eval(data.Data);
                var IsFirstLog = courseList[0].isFirstLog;
                if (IsFirstLog == 0 || IsFirstLog == 1) {  //第一次,显示确认信息
                    var CoursePeriodTimeID = ''; var CoursePeriodID = ''; var CourseID = ''; var CourseType = '';
                    var len = $(".movieclass .box ul li");
                    for (var i = 0; i < len.length; i++) {
                        if (len[i].className == "on") {
                            CoursePeriodTimeID = len[i].getAttribute("CoursePeriodTimeID");
                            CoursePeriodID = len[i].getAttribute("CoursePeriodID");
                            CourseID = len[i].getAttribute("CourseID");
                            CourseType = len[i].getAttribute("CourseType");
                        }
                    }
                    window.location.href = "/ConfirmInfo/Index?CourseType=" + CourseType + "&CourseID=" + CourseID + "&CoursePeriodID=" + CoursePeriodID + "&CoursePeriodTimeID=" + CoursePeriodTimeID + "&UserID=" + courseList[0].UserID + "&TrueName=" + courseList[0].TrueName + "&TelePhone=" + courseList[0].TelePhone + "";
                    //$(".movieclass .box").css("display", "none");
                    //$(".movieclass .box1").css("display", "block");
                }
                //else {    //进入支付页面（未用）
                //    //一验证.(1.该课时的这个时间段是否结束；2.判断这个时间段是否已预约满 3.判断当前用户是否已预约) 
                //    var CoursePeriodTimeID = ''; var CoursePeriodID = '';
                //    var len = $(".movieclass .box ul li");
                //    for (var i = 0; i < len.length; i++) {
                //        if (len[i].className == "on") {
                //            CoursePeriodTimeID = len[i].getAttribute("CoursePeriodTimeID");
                //            CoursePeriodID = len[i].getAttribute("CoursePeriodID");
                //        }
                //    }
                //    $.post("/CoursePeriod/GetCoursePeriodTimeState", { UserID: courseList[0].UserID, CoursePeriodTimeID: CoursePeriodTimeID }, function (data) {
                //        data = eval(data);
                //        if (data.Success) {
                //            if (data.Data == "可预约") {
                //                //判断是否支付成功

                //                //如果成功提交预约
                //                $.post("/CoursePeriod/CommitUserAppoint", { UserID: courseList[0].UserID, CoursePeriodID: CoursePeriodID, CoursePeriodTimeID: CoursePeriodTimeID }, function (data) {
                //                    data = eval(data);
                //                    if (data.Success) { //预约成功

                //                        $(".movieclass .box").css("display", "none");
                //                        $(".movieclass .box1").css("display", "block");
                //                    } else {
                //                        alert(data.Msg);
                //                    }
                //                });

                //            } else {
                //                alert(data.Msg);
                //            }
                //        } else {
                //            alert(data.Msg);
                //        }
                //    });
                //    alert('修改成功，进入支付页面');
                //}
            } else {
                alert(data.Msg);
            }
        });
    });
    /*点击遮罩弹窗消失*/
    $(".movieclass .shadow1").click(function () {
        $(".movieclass .box").css("display", "none");
        $(".movieclass .box1").css("display", "none");
        $(".movieclass .box2").css("display", "none");
        $(".movieclass .shadow1").css("display", "none");
    });
});
//采用正则表达式获取地址栏参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

