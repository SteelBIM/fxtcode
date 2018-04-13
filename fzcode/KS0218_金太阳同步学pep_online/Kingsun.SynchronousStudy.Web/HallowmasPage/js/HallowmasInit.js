var HallowmasInit = function () {
    var Current = this;
    this.Index = 1;
    this.flag = true;
    this.file_Url = "http://file.kingsun.cn/";
    this.voteRecord = false;

    this.Init = function () {
        $("#prev").css("background-image", "url('')");
        Current.UserID = Common.QueryString.GetValue("UserID");
        Current.VideoID = Common.QueryString.GetValue("VideoID");
        //Current.UserID = "1847530068";
        //Current.VideoID = "ddc67e38-b4a1-4621-914a-99573191015e";
        Current.GetUserInfo();
        Current.GetRankList(Current.Index);
    };

    //获取用户信息、视频信息
    this.PreventRepeatVote = function () {
        var obj = { VideoID: Current.VideoID };
        $.ajax({
            url: "?action=PreventRepeatVote",
            async: true,
            data: obj,
            type: "POST",
            dataType: "json",
            success: function (data) {
                if (data) {
                    var result = data;
                    if (result.Success) {
                        Current.voteRecord = true;
                    }

                }
            }
        });
    }

    //获取用户信息、视频信息
    this.GetUserInfo = function () {
        var obj = { UserID: Current.UserID, VideoID: Current.VideoID };
        $.ajax({
            url: "?action=GetUserInfo",
            async: false,
            data: obj,
            type: "POST",
            dataType: "json",
            success: function (data) {
                if (data) {
                    var result = data;
                    if (result.Success) {
                        Current.GetNumberOfPrize();
                        Current.PreventRepeatVote();
                        if (result.UserInfo.UserImage != '00000000-0000-0000-0000-000000000000') {
                            $("#userImg").attr("src", Current.file_Url + 'GetFiles.ashx?FileID=' + result.UserInfo.UserImage);
                        }
                        $("#userName").html(result.UserInfo.NickName);
                        var time = Common.FormatTime(result.UserVideoInfo.CreateTime, 'yyyy年MM月dd日 hh时mm分');
                        $("#shareTime").html(time);
                        $("#video").attr("src", result.UserVideoInfo.VideoFileID);
                    }

                }
            }
        });
    }

    this.GetNumberOfPrize = function () {
        var obj = { VideoID: Current.VideoID };
        $.ajax({
            url: "?action=GetNumberOfPrize",
            async: true,
            data: obj,
            type: "POST",
            dataType: "json",
            success: function (data) {
                if (data) {
                    var result = data;
                    if (result.Success) {
                        Current.ClearNumberOfPrize();
                        $("#PrizeOne").html(result.UserActiveVideoInfo.PrizeOne);
                        $("#PrizeTwo").html(result.UserActiveVideoInfo.PrizeTwo);
                        $("#PrizeThree").html(result.UserActiveVideoInfo.PrizeThree);
                        $("#PrizeFour").html(result.UserActiveVideoInfo.PrizeFour);
                        $("#PrizeFive").html(result.UserActiveVideoInfo.PrizeFive);
                    } else {
                        Current.ClearNumberOfPrize();
                    }
                }
            }
        });
    }

    $("#next").click(function () {
        if (Current.Index == 5) {
            return false;
        }
        Current.Index++;
        if (Current.Index == 5) {
            $("#next").css("background-image", "url('')");
        }
        $("#prev").css("background-image", "url('img/changeA.png')");
        Current.GetRankList(Current.Index);
    })

    $("#prev").click(function () {
        if (Current.Index == 1) {
            return false;
        }
        Current.Index--;
        if (Current.Index == 1) {
            $("#prev").css("background-image", "url('')");
        }
        $("#next").css("background-image", "url('img/changeA.png')");
        Current.GetRankList(Current.Index);
    })

    $(".sele ul li a").bind("click", function () {
        return false;
        if (Current.voteRecord) {
            alert("你已经给他送过糖果了，不能再送了哦！")
            return false;
        }
        if (Current.flag) {
            $(this).parent().parent().find("a").attr("class", "")
            $(this).attr("class", "on");
            var prize = $(this).find("b").attr("id");
            Current.InsertPrize(prize);
            Current.GetRankList(Current.Index);
            Current.flag = false;
        }
    });

    this.GetRankList = function (num) {
        var index = num;
        var prize = "";
        $("#content").attr('class', 'b' + index);
        switch (index) {
            case 1:
                prize = "PrizeOne";
                $("#content").html("萌出一脸血奖");
                break;
            case 2:
                prize = "PrizeTwo";
                $("#content").html("淘气牙痒痒奖");
                break;
            case 3:
                prize = "PrizeThree";
                $("#content").html("小小机灵鬼奖");
                break;
            case 4:
                prize = "PrizeFour";
                $("#content").html("我是学霸奖");
                break;
            case 5:
                prize = "PrizeFive";
                $("#content").html("小大人附身奖");
                break;
            default:
                prize = "PrizeOne";
                $("#content").html("萌出一脸血奖");
        }
        var obj = { Prize: prize };
        $.ajax({
            url: "?action=GetRankList",
            async: true,
            data: obj,
            type: "POST",
            dataType: "json",
            success: function (data) {
                if (data) {
                    var result = data;
                    if (result.Success) {
                        $("#tbdatagrid").html('');
                        var html = "";
                        for (var i = 0, length = result.List.length; i < length; i++) {
                            html += '<tr>';
                            html += '<td>' + (i + 1) + '.</td>';
                            if (result.List[i].UserImage != '00000000-0000-0000-0000-000000000000') {
                                html += '<td><img src="' + Current.file_Url + 'GetFiles.ashx?FileID=' + result.List[i].UserImage + '" alt="img"></td>';
                            } else {
                                html += '<td><img src="img/userImg.png" alt="img"></td>';
                            }
                            if (result.List[i].NickName == null) {
                                var userName = result.List[i].UserName;
                                var content = "";
                                for (var j = 0; j < userName.length; j++) {
                                    if (j == 0 || j == (userName.length - 1)) {
                                        content += userName[j];
                                    } else {
                                        content += "*";
                                    }
                                }
                                html += '<td><span class="textSp">' + content + '</span></td>';
                            } else {
                                html += '<td><span class="textSp">' + result.List[i].NickName + '</span></td>';
                            }
                            html += '<td><span class="sp">' + result.List[i].Count + '</span></td>';
                            html += '<tr>';
                        }
                        $(html).appendTo("#tbdatagrid");
                    }
                }
            }
        });
    }

    //清空点赞数量
    this.ClearNumberOfPrize = function () {
        $("#PrizeOne").html(0);
        $("#PrizeTwo").html(0);
        $("#PrizeThree").html(0);
        $("#PrizeFour").html(0);
        $("#PrizeFive").html(0);
    }

    //新增点赞
    this.InsertPrize = function (prize) {
        var obj = { VideoID: Current.VideoID, Prize: prize };
        $.ajax({
            url: "?action=InsertPrize",
            async: true,
            data: obj,
            type: "POST",
            dataType: "json",
            success: function (data) {
                if (data) {
                    var result = data;
                    if (result.Success) {
                        Current.GetNumberOfPrize();
                        Current.GetRankList(Current.Index);
                    }
                }
            }
        });
    }

}


var hallowmasInit;
$(function () {
    hallowmasInit = new HallowmasInit();
    hallowmasInit.Init();
});