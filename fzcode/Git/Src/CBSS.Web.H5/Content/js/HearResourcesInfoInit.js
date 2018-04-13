/// <reference path="../../AppTheme/js/jquery-1.11.2.min.js" />

var HearResourcesInfoInit = function () {
    var Current = this;
    var _getOssFilesUrl = "https://tbxcdn.kingsun.cn/SynchronousStudy/UpLoadFile/";
    var _getFilesUrl = "https://file.kingsun.cn/GetFiles.ashx";

    //相关信息初始化
    this.Init = function () {
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.BookID = Common.QueryString.GetValue("BookID");
        Current.FirstTitleID = Common.QueryString.GetValue("FirstTitleID");
        Current.SecondTitleID = Common.QueryString.GetValue("SecondTitleID");
        if (Current.SecondTitleID == "" || Current.SecondTitleID == undefined) {
            Current.SecondTitleID = 0;
        }
        Current.FirstModularID = Common.QueryString.GetValue("FirstModularID");
        Current.SecondModularID = Common.QueryString.GetValue("SecondModularID");
        Current.AppID = Common.QueryString.GetValue("AppID");
        Current.BookAppID = Common.QueryString.GetValue("BookAppID");
        //Current.UserID = "449307214";
        //Current.BookID = "108";
        //Current.FirstTitleID = "272857";
        //Current.SecondTitleID = "272858";
        //Current.FirstModularID = "10";
        //Current.SecondModularID = "1002";
        Current.AudioArr = [];
        Current.GetListenSpeakList();
        Current.Audio = document.getElementById('audio1');
        Current.curr = 0;

        Current.Audio.addEventListener('ended', function () {
            $(".textDesc" + (Current.curr - 1)).attr("style", "color:black");
            if (Current.curr < Current.AudioArr.length) {
                Current.play();
            } else {
                Current.Audio.pause();
                $(".main1 .sen_list").removeClass("on");
                $(".main1 .text_list").removeClass("on");
                $(".main1 .sen_list .img_bo").attr("src", "../../Content/img/shenyin.png");
                $(".main1 .text_list .img_bo").attr("src", "../../Content/img/shenyin.png");

            }
        }, false);
    };

    //加载用户提交资源列表
    this.GetListenSpeakList = function () {
        var data = {
            UserID: Current.UserID,
            BookID: Current.BookID,
            FirstTitleID: Current.FirstTitleID,
            SecondTitleID: Current.SecondTitleID,
            FirstModularID: Current.FirstModularID,
            SecondModularID: Current.SecondModularID,
            PKey: "", RTime: Common.DateNow() 
        };
        var obj = { FunName: "GetListenSpeakAchievementByWeb", Info: $.toJSON(data), FunWay: "0", Flag: "", Key: "" }
        $.post("http://192.168.3.1:8026/dc/active", obj, function (data) {
            if (data) {
                var result = JSON.parse(data);
                if (result.Success) {
                    if (result.UserInfo != null) {
                        $("#userName").html(result.UserInfo.TrueName);
                        if (result.UserInfo.UserImage != '00000000-0000-0000-0000-000000000000') {
                            //$("#userImg").attr("background", Constant.file_Url + 'GetFiles.ashx?FileID=' + result.UserInfo.UserImage);
                            $("#userImg1").attr("src", Constant.file_Url + 'GetFiles.ashx?FileID=' + result.UserInfo.UserImage);

                        }
                    }
                    if (result.ResourcesList != null && result.ResourcesList.length > 0) {
                        var averageScore = result.ResourcesList[0].AverageScore;
                        $("#averageScore").html(parseInt(averageScore * 10) / 10);
                        var time = Common.FormatTime(result.ResourcesList[0].CreateDate, 'yyyy.MM.dd');
                        $(".time").html(time);
                        var html = '';
                        if (Current.SecondModularID == "1001" || Current.SecondModularID == "1002" || Current.SecondModularID == "1004") {
                            for (var i = 0, length = result.ResourcesList.length; i < length; i++) {
                                var totalScore = result.ResourcesList[i].TotalScore;
                                var videofile = result.ResourcesList[i].VideoFileID != 0 ? _getOssFilesUrl + result.ResourcesList[i].VideoFileID : _getFilesUrl + "?FileID=" + result.ResourcesList[i].VideoFileID;

                                html += "<div class='sen_list' data-src='" + videofile + "'><a href='javascript:void(0)'>";
                                html += "<img class='img_bo' src='../../Content/img/shenyin.png' alt=''>";
                                html += "<p>" + formatStr(result.ResourcesList[i].TextDesc) + "</p>";

                                //html += "<img class='img_yu' src='" + this.img_yu + "' alt=''>";

                                //html += '<div class="con_main">';
                                //html += '<a class="main_img1">';
                                //html += '<img src="img/laba3.png" class="hornImg" id="' + result.ResourcesList[i].VideoFileID + '" /></a>';
                                //html += '<span>' + formatStr(result.ResourcesList[i].TextDesc) + '</span>';
                                //html += '<div class="main_img2">';
                                if (totalScore >= 0 && totalScore < 40) {
                                    html += '<img class="img_yu" src="../../Content/img/yu1.png" />';
                                }
                                if (totalScore >= 40 && totalScore < 60) {
                                    html += '<img class="img_yu" src="../../Content/img/yu2.png" />';
                                }
                                if (totalScore >= 60 && totalScore < 80) {
                                    html += '<img class="img_yu" src="../../Content/img/yu3.png" />';
                                }
                                if (totalScore >= 80 && totalScore < 90) {
                                    html += '<img class="img_yu" src="../../Content/img/yu4.png" />';
                                }
                                if (totalScore >= 90 && totalScore < 101) {
                                    html += '<img  class="img_yu" src="../../Content/img/yu5.png" />';
                                }
                                html += "</a></div>";
                                $('.main1 .section1').html("");
                                $(".main1 .section1").append(html);
                                //html += '</div>';
                                //html += '</div>';
                            }
                        }
                        if (Current.SecondModularID == "1003") {
                            //html += '<div class="con_main1">';
                            //html += '<div class="main_img2">';
                            //html += "<div class='text_list' data-src='" + result.ResourcesList[i].VideoFileID + "'>";
                            html += "<div class='text_list'>";
                            html += "<div class='text_title'>";
                            html += "<a href='javascript:void(0)'>";
                            html += "<img class='img_bo' src='../../Content/img/shenyin.png' alt=''>";
                            html += "<h2>" + "LET'S TALK" + "</h2>";

                            var averageScore = result.ResourcesList[0].AverageScore;
                            if (averageScore >= 0 && averageScore < 40) {
                                html += '<img  class="img_yu" src="../../Content/img/yu1.png" alt="" />';
                            }
                            if (averageScore >= 40 && averageScore < 60) {
                                html += '<img  class="img_yu" src="../../Content/img/yu2.png" alt="" />';
                            }
                            if (averageScore >= 60 && averageScore < 80) {
                                html += '<img  class="img_yu" src="../../Content/img/yu3.png" alt="" />';
                            }
                            if (averageScore >= 80 && averageScore < 90) {
                                html += '<img  class="img_yu" src="../../Content/img/yu4.png" alt="" />';
                            }
                            if (averageScore >= 90 && averageScore < 101) {
                                html += '<img  class="img_yu" src="../../Content/img/yu5.png" alt="" />';
                            }
                            html += "</a>";
                            html += "</div>";

                            //html += '</div>';
                            //html += '<a class="main_img1"><img src="img/laba3.png" id="hornImg" alt="" /></a>'
                            for (var i = 0, length = result.ResourcesList.length; i < length; i++) {
                                html += '<p>' + result.ResourcesList[i].TextDesc + '</p>';
                                Current.AudioArr.push(result.ResourcesList[i].VideoFileID);
                            }
                            html += "</div>";
                            html += "</div>";
                            $('.main1 .section2').html("");
                            $(".main1 .section2").append(html);
                        }

                        Current.PlayAudio();
                        
                    }
                }
            }
        });

    }

    //绑定播放用户跟读录音
    this.PlayAudio = function () {
        //$(".main1 .section2 .text_list .img_bo").click(function (e) {
        //    Current.HiddenPic();
        //    var filespec = "http://file.kingsun.cn/GetFiles.ashx?FileID=" + $(this).attr("id") + "&view=true";
        //    $("#audio1").attr("src", filespec);
        //    var my = document.getElementById('audio1');
        //    if (my.paused) {
        //        my.play();
        //        $(".main1 .section2 .text_list .img_bo").attr("src", "img/laba3.png");
        //        $(this).attr("src", "img/laba.gif");
        //    }
        //    else if (my.played) {
        //        $(".main1 .section2 .text_list .img_bo").attr("src", "img/laba3.png");
        //        $(this).attr("src", "img/laba.gif");
        //        my.play();
        //    }
        //    my.addEventListener('ended', function () {
        //        $(".main1 .section2 .text_list .img_bo").attr("src", "img/laba3.png");
        //    }, false);
        //});
        $(".main1 .sen_list").on("click", function () {
            Current.HiddenPic();
            
            var my_audio = document.getElementById('audio1');
            if ($(this).hasClass("on")) {
                if (my_audio.paused) {
                    my_audio.play();
                    $(this).find(".img_bo").attr("src", "../../Content/img/shenyin.gif");
                }
                else if (my_audio.played) {
                    my_audio.pause();
                    $(this).find(".img_bo").attr("src", "../../Content/img/shenyin.png");
                }
            }
            else {
                my_audio.src = $(this).data('src');
                my_audio.load();
                my_audio.play();
                $(".main1 .sen_list .img_bo").attr("src", "../../Content/img/shenyin.png");
                $(this).find(".img_bo").attr("src", "../../Content/img/shenyin.gif");
            }
            $(".main1 .sen_list").removeClass("on");
            $(this).addClass("on");
        });
        $(".main1 .text_list").click(function () {
            Current.HiddenPic();
           
            var my_audio = document.getElementById('audio1');
            if ($(this).hasClass("on")) {
                if (my_audio.paused) {
                    my_audio.play();
                    $(this).find(".img_bo").attr("src", "../../Content/img/shenyin.gif");
                }
                else if (my_audio.played) {
                    my_audio.pause();
                    $(this).find(".img_bo").attr("src", "../../Content/img/shenyin.png");
                }
            }
            else {
                //my_audio.src = $(this).data('src');
                //my_audio.load();
                //my_audio.play();
                Current.play();
                $(".main1 .text_list .img_bo").attr("src", "../../Content/img/shenyin.png");
                $(this).find(".img_bo").attr("src", "../../Content/img/shenyin.gif");
            }
            $(".main1 .text_list").removeClass("on");
            $(this).addClass("on");
        });


        //$("#hornImg").click(function (e) {
        //    Current.Audio.pause();
        //    $(Current.Audio).unbind();
        //    Current.HiddenPic();
        //    for (var i = 0, length = Current.AudioArr.length; i < length; i++) {
        //        $(".textDesc" + i).attr("style", "color:black");
        //    }
        //    if (Current.Audio.paused) {
        //        Current.curr = 0
        //        Current.play();
        //        $("#hornImg").attr("src", "img/laba3.png");
        //        $(this).attr("src", "img/laba.gif");
        //    } else if (my.played) {
        //        $("#hornImg").attr("src", "img/laba3.png");
        //        $(this).attr("src", "img/laba.gif");
        //    }
        //});
    }

    this.play = function () {
        var filespec = "http://file.kingsun.cn/GetFiles.ashx?FileID=" + Current.AudioArr[Current.curr] + "&view=true";
        $(".textDesc" + Current.curr).attr("style", "color:red");
        $("#audio1").attr("src", filespec);
        Current.curr++;
        Current.Audio.play();
    }
    //隐藏动态图片
    this.HiddenPic = function () {
        var display = $("#invincible").css("display");
        if (display == "block") {
            var my = document.getElementById('audio4');
            my.pause();
            $("#invincible").animate({ top: "-2000px" }, 3000);
        }
    }

    $("#linkapp").click(function () {
        if (Current.AppID == "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385") {//广州版
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.gz.syslearning";
        } else if (Current.AppID == "43716a9b-7ade-4137-bdc4-6362c9e1c999") {//上海本地
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.shbd.syslearning";
        }
        else if (Current.AppID == "241ea176-fce7-4bd7-a65f-a7978aac1cd2") {//深圳版
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.sz.syslearning";
        }
        else if (Current.AppID == "9426808e-da8e-488c-9827-b082c19b62a7") {//上海全国
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.shqg.syslearning";
        }
        else if (Current.AppID == "0a94ceaf-8747-4266-bc05-ed8ae2e7e410") {//北京
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.bj.syslearning";
        }
        else if (Current.AppID == "5373bbc9-49d4-47df-b5b5-ae196dc23d6d") {//人教pep
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.rj.syslearning";
        }
        else if (Current.AppID == "37ca795d-42a6-4117-84f3-f4f856e03c62") {//广东
            window.location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=com.gd.syslearning";
        }
        else {
            window.location.href = "http://tbx.kingsun.cn/downloadList.html";
        }
    });
}

var hearResourcesInfoInit;
$(function () {
    hearResourcesInfoInit = new HearResourcesInfoInit();
    hearResourcesInfoInit.Init();
});

function formatStr(str) {
    while (str.indexOf("\\n") > 0) {
        str = str.replace("\\n", "<br/>");
    }
    return str;
}