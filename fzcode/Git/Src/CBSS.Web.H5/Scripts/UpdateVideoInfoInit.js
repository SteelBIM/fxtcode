var UpdateVideoInfoInit = function () {
    var Current = this;
    var VideoID = "";
    var VideoDetails = '';
    var VideoDialogueArr = [];

    this.Init = function () {
        Current.VideoID = Common.QueryString.GetValue("VideoID");
        Current.GetVideoInfo();
    };

    //获取视频信息
    this.GetVideoInfo = function () {
        var obj = { VideoID: Current.VideoID };
        var html = '';
        $.post("?action=getVideoInfo", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                $("#bookName").val(result.obj1.BookName);
                $("#firstTitle").val(result.obj1.FirstTitle);
                $("#secondTitle").val(result.obj1.SecondTitle);
                $("#firstModule").val(result.obj1.FirstModular);
                $("#secondModule").val(result.obj1.SecondModular);
                $("#videoTitle").val(result.obj1.VideoTitle);
                $("#videoNumber").val(result.obj1.VideoNumber);
                $("#muteVideo").val(result.obj1.MuteVideo);
                $("#completeVideo").val(result.obj1.CompleteVideo);
                $("#videoTime").val(result.obj1.VideoTime);
                $("#backgroundAudio").val(result.obj1.BackgroundAudio);
                $("#videoCover").val(result.obj1.VideoCover);
                $("#videoDesc").val(result.obj1.VideoDesc);
                $("#videoDifficulty").val(result.obj1.VideoDifficulty);
                Current.VideoDetails = result.obj1;
                Current.VideoDialogueArr = result.obj2;
                if (result.obj2 != null && result.obj2.length > 0) {
                    html = '';
                    $('#tbody').html("");
                    $.each(result.obj2, function (index, value) {
                        html += '<tr id="' + value.ID + '">';
                        html += '<td>' + (index + 1) + '</td>';
                        html += '<td><input type="text" id="dialogueNumber' + (index + 1) + '" /></td>';
                        html += '<td><input type="text" id="dialogueText' + (index + 1) + '" /></td>';
                        html += '<td><input type="text" id="startTime' + (index + 1) + '" /></td>';
                        html += '<td><input type="text" id="endTime' + (index + 1) + '" /></td>';
                        html += '</tr>';
                        $(html).appendTo("#tbody");
                        html = '';
                        $("#dialogueNumber" + (index + 1)).val(value.DialogueNumber);
                        $("#dialogueText" + (index + 1)).val(value.DialogueText);
                        $("#startTime" + (index + 1)).val(value.StartTime);
                        $("#endTime" + (index + 1)).val(value.EndTime);
                    })
                } else {
                    html = '';
                    $('#tbody').html("");
                    html += '<tr>';
                    html += '<td width="80" style="list-style-type:none;"><li>暂无信息</li></td>';
                    html += '</tr>';
                    $(html).appendTo("#tbody");
                }
            }
        });
    }

    //修改视频信息
    this.UpdateVideoDetails = function () {
        var videoTitle = $.trim($("#videoTitle").val());
        var videoNumber = $.trim($("#videoNumber").val());
        var muteVideo = $.trim($("#muteVideo").val());
        var completeVideo = $.trim($("#completeVideo").val());
        var videoTime = $.trim($("#videoTime").val());
        var backgroundAudio = $.trim($("#backgroundAudio").val());
        var videoCover = $.trim($("#videoCover").val());
        var videoDesc = $.trim($("#videoDesc").val());
        var videoDifficulty = $.trim($("#videoDifficulty").val());
        Current.VideoDetails.VideoTitle = videoTitle;
        Current.VideoDetails.VideoNumber = videoNumber;
        Current.VideoDetails.MuteVideo = muteVideo;
        Current.VideoDetails.CompleteVideo = completeVideo;
        Current.VideoDetails.VideoTime = videoTime;
        Current.VideoDetails.BackgroundAudio = backgroundAudio;
        Current.VideoDetails.VideoCover = videoCover;
        Current.VideoDetails.VideoDesc = videoDesc;
        Current.VideoDetails.VideoDifficulty = videoDifficulty;
        var queryStr = $.toJSON(Current.VideoDetails);
        $.post("?action=updateVideoInfo&queryStr=" + queryStr, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result) {
                    if (Current.VideoDialogueArr != null && Current.VideoDialogueArr.length > 0) {
                        Current.UpdateVideoDialogue();
                    } else {
                        alert("更新成功");
                        window.parent.CloseDialog();
                    }
                } else {
                    alert("更新失败，请重试");
                }
            }
        });
    }


    //修改视频对话信息
    this.UpdateVideoDialogue = function () {
        for (var i = 0; i < Current.VideoDialogueArr.length; i++) {
            Current.VideoDialogueArr[i].DialogueNumber = $("#dialogueNumber" + (i + 1)).val();
            Current.VideoDialogueArr[i].DialogueText = $("#dialogueText" + (i + 1)).val();
            Current.VideoDialogueArr[i].StartTime = $("#startTime" + (i + 1)).val();
            Current.VideoDialogueArr[i].EndTime = $("#endTime" + (i + 1)).val();
        }
        var queryStr = $.toJSON(Current.VideoDialogueArr);
        $.post("?action=updateVideoDialogue&queryStr=" + queryStr, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result) {
                    alert("更新成功");
                    window.parent.CloseDialog();
                } else {
                    alert("更新失败，请重试");
                }
            }
        });
    }

    //关闭弹框
    this.Cancel = function () {
        window.parent.CloseDialog();
    }

}


var updateVideoInfoInit;
$(function () {
    updateVideoInfoInit = new UpdateVideoInfoInit();
    updateVideoInfoInit.Init();
});