/// <reference path="../../AppTheme/js/jquery-1.11.2.min.js" />

var ShareResultInit = function () {
    var Current = this;

    //this.UserID = '';//用户ID
    //this.BookID = '';//BookID
    //this.FirstTitleID = '';//一级标题ID
    //this.SecondTitleID = '';//二级标题ID
    //this.FirstModularID = '';//一级模块ID
    //this.SecondModularID = '';//二级模块ID
    //this.AudioArr = []; //音频列表
    //this.boolArr = []; // 判断音频文件存在性

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
                $("#hornImg").attr("src", "img/laba3.png");
                $(this).attr("src", "img/laba.gif");
            }
        }, false);
    };

    //加载用户提交资源列表
    this.GetListenSpeakList = function () {
        var obj = {
            UserID: Current.UserID,
            BookID: Current.BookID,
            FirstTitleID: Current.FirstTitleID,
            SecondTitleID: Current.SecondTitleID,
            FirstModularID: Current.FirstModularID,
            SecondModularID: Current.SecondModularID
        };
        $.post("?action=GetListenSpeakAchievement", obj, function (data) {
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
                        var time = Common.FormatTime(result.ResourcesList[0].CreateDate, 'yyyy.MM.dd');
                        $("#creatTime").html(time);
                        if (averageScore >= 1 && averageScore < 60) {
                            $("#headImg").attr("src", "img/xin1.png");
                            $("#headText").html("默默一星，累觉不爱");
                        }
                        if (averageScore >= 60 && averageScore < 80) {
                            $("#headImg").attr("src", "img/xin2.png");
                            $("#headText").html("萌萌二星，再接再厉");
                        }
                        if (averageScore >= 80 && averageScore < 90) {
                            $("#headImg").attr("src", "img/xin3.png");
                            $("#headText").html("超群三星，更进一步");
                        }
                        if (averageScore >= 90 && averageScore < 101) {
                            $("#invincible").attr("style", "display: block");
                            $("#headImg").attr("src", "img/xin4.png");
                            $("#headText").html("无敌四星，所向披靡");
                            var my = document.getElementById('audio4');
                            my.play();
                            my.addEventListener('ended', function () {
                                $("#invincible").animate({ top: "-2000px" }, 3000);
                            }, false);
                        }
                        var html = '';
                        if (Current.SecondModularID == "1001" || Current.SecondModularID == "1002" || Current.SecondModularID == "1004") {
                            for (var i = 0, length = result.ResourcesList.length; i < length; i++) {
                                var totalScore = result.ResourcesList[i].TotalScore;
                                html += '<div class="con_main">';
                                html += '<a class="main_img1">';
                                html += '<img src="img/laba3.png" class="hornImg" id="' + result.ResourcesList[i].VideoFileID + '" /></a>';
                                html += '<span>' + formatStr(result.ResourcesList[i].TextDesc) + '</span>';
                                html += '<div class="main_img2">';
                                if (totalScore >= 0 && totalScore < 40) {
                                    html += '<img src="img/yu1.png" />';
                                }
                                if (totalScore >= 40 && totalScore < 60) {
                                    html += '<img src="img/yu2.png" />';
                                }
                                if (totalScore >= 60 && totalScore < 80) {
                                    html += '<img src="img/yu3.png" />';
                                }
                                if (totalScore >= 80 && totalScore < 90) {
                                    html += '<img src="img/yu4.png" />';
                                }
                                if (totalScore >= 90 && totalScore < 101) {
                                    html += '<img src="img/yu5.png" />';
                                }
                                html += '</div>';
                                html += '</div>';
                            }
                        }
                        if (Current.SecondModularID == "1003") {
                            html += '<div class="con_main1">';
                            html += '<div class="main_img2">';
                            var averageScore = result.ResourcesList[0].AverageScore;
                            if (averageScore >= 0 && averageScore < 40) {
                                html += '<img src="img/yu1.png" alt="" />';
                            }
                            if (averageScore >= 40 && averageScore < 60) {
                                html += '<img src="img/yu2.png" alt="" />';
                            }
                            if (averageScore >= 60 && averageScore < 80) {
                                html += '<img src="img/yu3.png" alt="" />';
                            }
                            if (averageScore >= 80 && averageScore < 90) {
                                html += '<img src="img/yu4.png" alt="" />';
                            }
                            if (averageScore >= 90 && averageScore < 101) {
                                html += '<img src="img/yu5.png" alt="" />';
                            }
                            html += '</div>';
                            html += '<a class="main_img1"><img src="img/laba3.png" id="hornImg" alt="" /></a>'
                            for (var i = 0, length = result.ResourcesList.length; i < length; i++) {
                                html += '<p class="textDesc' + [i] + '">' + result.ResourcesList[i].TextDesc + '</p>';
                                Current.AudioArr.push(result.ResourcesList[i].VideoFileID);
                            }
                            html += '</div>';
                        }
                        $('#detail').html("");
                        $(html).appendTo("#detail");
                        Current.PlayAudio();

                    }
                }
            }
        });
    }

    //绑定播放用户跟读录音
    this.PlayAudio = function () {
        $(".content1 .contentDetail .con_main .main_img1 img").click(function (e) {
            Current.HiddenPic();
            var filespec = "http://file.kingsun.cn/GetFiles.ashx?FileID=" + $(this).attr("id") + "&view=true";
            $("#audio1").attr("src", filespec);
            var my = document.getElementById('audio1');
            if (my.paused) {
                my.play();
                $(".content1 .contentDetail .con_main .main_img1 img").attr("src", "img/laba3.png");
                $(this).attr("src", "img/laba.gif");
            }
            else if (my.played) {
                $(".content1 .contentDetail .con_main .main_img1 img").attr("src", "img/laba3.png");
                $(this).attr("src", "img/laba.gif");
                my.play();
            }
            my.addEventListener('ended', function () {
                $(".content1 .contentDetail .con_main .main_img1 img").attr("src", "img/laba3.png");
            }, false);
        });


        $("#hornImg").click(function (e) {
            Current.Audio.pause();
            $(Current.Audio).unbind();
            Current.HiddenPic();
            for (var i = 0, length = Current.AudioArr.length; i < length; i++) {
                $(".textDesc" + i).attr("style", "color:black");
            }
            if (Current.Audio.paused) {
                Current.curr = 0
                Current.play();
                $("#hornImg").attr("src", "img/laba3.png");
                $(this).attr("src", "img/laba.gif");
            } else if (my.played) {
                $("#hornImg").attr("src", "img/laba3.png");
                $(this).attr("src", "img/laba.gif");
            }
        });
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
        } else {
            window.location.href = "http://tbx.kingsun.cn/downloadList.html";
        }
    });
}

var shareResultInit;
$(function () {
    shareResultInit = new ShareResultInit();
    shareResultInit.Init();
});

function formatStr(str) {
    while (str.indexOf("\\n") > 0) {
        str = str.replace("\\n", "<br/>");
    }
    return str;
}