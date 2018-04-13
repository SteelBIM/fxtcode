//学生预习页面
var u = navigator.userAgent;
var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/);
var resourceID = '';
var leadID = '';
var obj = '';
var stuTaskID = '';
var stuID = '';
var dialog = '';
//var PreviewUrl = Constant.file_Url + "Preview.ashx";
$(function () {
    setIframeSize();
    resourceID = Common.QueryString.GetValue("ID");
    leadID = Common.QueryString.GetValue("LeadID");
    stuTaskID = Common.QueryString.GetValue("StuTaskID");
    stuID = Common.QueryString.GetValue("StuID");
    if (resourceID == "undefined" || leadID == "undefined" || stuTaskID == "undefined" || stuID == "undefined") {
        alert("传入参数错误！");
        return;
    }
    //获取预习内容
    GetResourceInfo();
});

//获取预习内容
function GetResourceInfo() {
    obj = { LeadID: leadID, ResourceID: resourceID, StuID: stuID };
    //获取资源信息
    $.post("?action=GetResourceInfo&Rand=" + Math.random(), obj, function (result) {
        if (result) {
            
            result = eval("(" + result + ")");
            if (result.Success) {
                var resourceInfo = result.Data.ResourceInfo[0];
                //加载页面
                loadHtml(resourceInfo, result.Data);

                //资源浏览
                GetPreviewUrl(resourceInfo.ResourceUrl, Constant.file_Url + "MobilePreview.ashx", function (res) {
                    var scanHtml = '<div class="nothing">' +
                                    '<div class="unplayTip">' +
                                    '<p class="pt40">手机暂时无法播放此类课件哦</p>' +
                                    '<p>请移步电脑端进行学习吧~</p></div></div>';
                    if (res.CanPreview) {
                        if (resourceInfo.FileType == "swf" || resourceInfo.FileType == "ppt" || resourceInfo.FileType == "pptx"
                            || resourceInfo.FileType == "doc" || resourceInfo.FileType == "docx") {
                            $(".iframeContent").html(scanHtml);
                        } else {
                            $("#previewPage").attr("src", res.URL);
                        }        
                    }
                    else {
                        $(".iframeContent").html('<div class="nothing">' +
                               '<div class="unplayTip">' +
                               '<p class="pt40">暂时未获取到相应的预习资源哦~</p>' +
                               '</div></div>');
                    }
                    
                });                                  
            } else {
                alert(result.Message);
            }
        }
    });
}

function setIframeSize() {
    var pW = $(".iframeContent").width();
    $("#previewPage").css("width", pW + "px");
}

function loadHtml(resourceInfo, leadInfo) {
    var discussHtml = '';
    //判断用户是否点赞
    UserHasGood();

    //加载资源页面
    //资源标题                
    $(".head").html('<h1>' + resourceInfo.ResourceName + '</h1>');
    //$(".fullScreen").attr("href", '../Student/StuPreview.aspx?StuTaskID=' + stuTaskID + '');
    //显示资源浏览数和点赞数
    $(".rightSpan").html('<b>' + resourceInfo.ScanCounts + '</b> <a class="agree" onclick="OperateResource(4)">' + resourceInfo.GoodCounts + '</a>');
    //显示学习目标
    $(".discussQue").html(leadInfo.Traget);
    //显示预习讨论主题
    var leadDisList = leadInfo.LeadDis;
    discussHtml += '<ol>';
    for (var i = 0; i < leadDisList.length; i++) {
        //讨论主题
        discussHtml += '<li><p><b>问题' + (i + 1) + '：</b>'
            + '<a href="javascript:ShowDisInfo(\'' + leadDisList[i].LeadDiscussID + '\')">' + leadDisList[i].DiscussContent + '</a></p>';
        discussHtml += '<p class="discussStu" id="p_' + leadDisList[i].LeadDiscussID + '">';
        discussHtml += '<a class="downCommend" href="javascript:ShowDisInfo(\'' + leadDisList[i].LeadDiscussID + '\')">展开讨论(' + leadDisList[i].DisCount + ')</a>';
        discussHtml += '<a class="addCommend" style="display: none;" onclick="ReplayDis(\'\',0,\'' + leadDisList[i].LeadDiscussID + '\')">发表评论</a></p>';
        //学生参与情况
        discussHtml += '<div class="commendList" id="d_' + leadDisList[i].LeadDiscussID + '" style="display:none;">';
        discussHtml += ShowDiscuss(leadDisList[i].LeadDiscussID, leadDisList[i].DiscussList, 0);
        //discussHtml += '<p class="discussStu upDiscuss"><a class="upCommend" href="javascript:ShowDisInfo(\'' + leadDisList[i].LeadDiscussID + '\')">收起评论</a></p>';
        discussHtml += '</div></li>';
    }
    discussHtml += '</ol>';
    $(".queList").html(discussHtml);

    //浏览次数+1
    OperateResource(3);

    //展开、收缩讨论控件
    //ShowDisInfo();
    setIframeSize();
    CssFit();
}

function CssFit()
{
    if (isiOS) {
        $("#commendTextarea").css({ "position": "absolute" });
        $("body").on("touchend", function (event) {
            var e = event || window.event || e; // 兼容IE7
            var obj = $(e.srcElement || e.target);
            if ($(obj).attr("class") == "answerText" || $(obj).attr("class") == "addCommend" || $(obj).attr("class") == "answerClick" || $(obj).attr("class") == "sumbmitBtn") {
            }
            else {

                $("#commendTextarea .answerText").blur();
                $("#commendTextarea").hide();
            }
        });
    } else {
        $("body").on("click", function (event) {
            var e = event || window.event || e; // 兼容IE7
            var obj = $(e.srcElement || e.target);
            if ($(obj).attr("class") == "answerText" || $(obj).attr("class") == "addCommend" || $(obj).attr("class") == "answerClick") {
            }
            else {
                $("#commendTextarea .answerText").blur();
                $("#commendTextarea").hide();
            }
        });
    }
}

function UserHasGood()
{
    //判断该用户是否对此资源点赞
    obj={ResourceID: resourceID,StuID:stuID}
    $.post("?action=HasGood&Rand=" + Math.random(), obj, function (data) {
        data = eval("(" + data + ")");
        if (data.Success) {
            if (data.Data == "1") {
                $(".agree").addClass(" agreed");
                $(".agree").attr('title', "取消点赞");
            } else {
                $(".agree").attr('title', "点赞");
            }
        }
        else {
            alert(data.Message);
        }
    });
}

//加载讨论集合
function ShowDiscuss(leadDiscussID,discussData, parentID)
{
    var disList = '<ul>';
    if (discussData != null && discussData.length > 0) {
        for (var i = 0; i < discussData.length; i++) {
            if (parentID == 0) {
                var avaUrl = discussData[i].AvatarUrl == null ? "../App_Themes/images/defultHeadStu.jpg" : discussData[i].AvatarUrl;
                disList += '<li id="li_' + discussData[i].DiscussID + '"> <img src="' + avaUrl + '" width="40" height="40">';
                disList += '<span>';
                disList += '<h3>' + discussData[i].TrueName + '</h3>';
                disList += '<a href="javascript:ReplayDis(\'' + discussData[i].TrueName + '\',' + discussData[i].DiscussID + ',\'' + leadDiscussID + '\')" class="answerClick">回复</a>';
                if (stuID == discussData[i].UserID) {
                    disList += '<a class="answerDelete" onclick="DelDiscuss(' + discussData[i].DiscussID + ',\'' + leadDiscussID + '\')">删除</a>';
                }
                disList += '</span>';
                disList += '<p>' + discussData[i].ReplyContent + '</p>';
                disList += '</li>';
                if (discussData[i].ChildDis.length > 0) {
                    disList += ShowChildDis(leadDiscussID, discussData[i].ChildDis, discussData[i].DiscussID, discussData[i].TrueName);
                }
            }
        }
    }
    disList += '</ul>';
    disList += '<p class="discussStu upDiscuss">';
    disList += '<a class="upCommend" href="javascript:ShowDisInfo(\'' + leadDiscussID + '\')">收起评论</a></p>';
    return disList;
}

//加载讨论子集合
function ShowChildDis(leadDisID, discussData, parentID,parentName) {
    var disList = '';
    for (var i = 0; i < discussData.length; i++) {
        var avaUrl = discussData[i].AvatarUrl == null ? "../App_Themes/images/defultHeadStu.jpg" : discussData[i].AvatarUrl;
        disList += '<li class="secLi" id="li_' + discussData[i].DiscussID + '"> <img src="' + avaUrl + '" width="40" height="40">';
        disList += '<span>';
        disList += '<h3>' + discussData[i].TrueName + '</h3>';
        disList += '<a href="javascript:ReplayDis(\'' + discussData[i].TrueName + '\',' + discussData[i].DiscussID + ',\'' + leadDisID + '\')" class="answerClick">回复</a>';
        if (stuID == discussData[i].UserID) {
            disList += '<a class="answerDelete" onclick="DelDiscuss(' + discussData[i].DiscussID + ',\'' + leadDisID + '\')">删除</a>';
        }
        disList += '</span>';
        disList += '<p><b>回复' + parentName + '：</b>' + discussData[i].ReplyContent + '</p>';
        disList += '</li>';
        if (discussData[i].ChildDis.length > 0) {
            disList += ShowChildDis(leadDisID, discussData[i].ChildDis, discussData[i].DiscussID, discussData[i].TrueName);
        }
    }
    return disList;
}

function ReplayDis(trueName, pDiscussID, leadDisID) {
    var textareaBox, userW;
    textareaBox = $("#commendTextarea");
    textareaBox.show();    
    textareaBox.find(".sumbmitBtn").attr('onclick', 'Submit(' + pDiscussID + ', \'' + leadDisID + '\')');
    textareaBox.find(".answerText").focus();
    if (trueName == '') {
        textareaBox.find(".answerText").attr("placeholder", "我来说几句~");
    } else {       
        textareaBox.find(".answerText").attr("placeholder", "回复" + trueName + ":");
    }
    
}

/*展开内容简介*/
function ShowDisInfo(id) {
    var block = $("#d_" + id).css("display");
    $(".queList ol>li .commendList").hide();
    $(".queList ol>li .commendList").prev(".discussStu").find(".addCommend").hide();
    $(".queList ol>li .commendList").prev(".discussStu").find(".downCommend").show();
    if (block == "block") {
        $("#d_" + id).hide();
        $("#p_" + id).find(".addCommend").hide();
        $("#p_" + id).find(".downCommend").show();        
    } else if (block == "none") {
        $("#d_" + id).show();
        $("#p_" + id).find(".downCommend").hide();
        $("#p_" + id).find(".addCommend").show();
    }
}

//更新操作资源记录
function OperateResource(type)
{
    obj = { ResourceID: resourceID, Type: type, StuID: stuID }
    $.post("?action=OperateResource&Rand=" + Math.random(), obj, function (data) {
        data = eval("(" + data + ")");
        if (data.Success) {
            var result = eval("(" + data.Data + ")");
            if (result.State) {
                if (type == 4)
                {
                    $(".agree").html(result.Count);
                    if ($(".agree").hasClass("agreed")) {
                        $(".agree").removeClass("agreed");
                        $(".agree").attr('title', "点赞");
                    }
                    else {
                        $(".agree").addClass("agreed");
                        $(".agree").attr('title', "取消点赞");
                    }
                }
            } else {
                alert("更新用户操作失败！");
            }
        }
        else {
            alert(data.Message);
        }
    });
}

//发表评论或回复
function Submit(pDiscussID, leadDisID) {
    $("#commendTextarea .answerText").blur();
    $("#commendTextarea").hide();
    var disContent = $(".answerText").val();
    if (disContent == "") {
        Tips('请填写评论内容！');
        return;
    }
    //发送评论
    obj = { LeadDisID: leadDisID, DiscussContent: disContent, ParentDisID: pDiscussID, StuID: stuID }
    $.post("?action=SendDiscuss&Rand=" + Math.random(), obj, function (data) {
        data = eval("(" + data + ")");
        Tips(data.Success ? '发表成功！' : data.Message);
        dialog.close();
        GetSingleDis(leadDisID);
    });
}

//确认是否删除
//function DelDiscuss(discussID, leadDisID){
//    var newdialog = art.dialog({
//        content: '确定要删除此评论？',
//        follow: document.getElementById("li_"+discussID),
//        ok: function () {
//            //$("body").css({ "overflow": "auto" });
//            newdialog.close();
//            //删除评论
//            obj = { DiscussID: discussID }
//            $.post("?action=DelDiscuss&Rand=" + Math.random(), obj, function (data) {
//                data = eval("(" + data + ")");
//                Tips(data.Success ? '删除成功！' : data.Message);
//                dialog.close();
//                GetSingleDis(leadDisID);
//            });
//        },
//        cancelVal: '关闭',
//        cancel: function(){
//            //$("body").css({ "overflow": "auto" });
//            newdialog.close();
//        }, //为true等价于function(){}
//        lock: true
//        //fixed: true
       
//    });
//    //$("body").css({ "overflow": "hidden" });
//    $(".aui_close").hide();
//}

//确认是否删除
function DelDiscuss(discussID, leadDisID) {
    $(".shadeDiv,.deleteDialog").show();
    $(".shadeDiv,.deleteDialog").find(".sure").attr('onclick', 'Del(' + discussID + ', \'' + leadDisID + '\')');
    $(".shadeDiv,.deleteDialog").find(".cancel").attr('onclick', 'closeDialog()');

}

function closeDialog() {
    //document.getElementById("dialog").style.display = "none";
    //document.getElementById("shade").style.display = "none";

        $("#dialog").hide();
        $("#shade").hide();   
}


function Del(discussID, leadDisID) {
    closeDialog();
    obj = { DiscussID: discussID }
    $.post("?action=DelDiscuss&Rand=" + Math.random(), obj, function (data) {
        data = eval("(" + data + ")");
        Tips(data.Success ? '删除成功！' : data.Message);
        dialog.close();
        GetSingleDis(leadDisID);
    });
}

//弹窗
function Tips(content) {
    dialog = art.dialog({
        time: 2,
        opacity: .1,
        lock: true,
        content: '<div class="tipMsg"><span><h4>提示：</h4><p>' + content + '</p></span>'
    });
    $(".aui_close").hide();
}

//刷新讨论
function GetSingleDis(leadDisID) {
    obj = { LeadDisID: leadDisID, StuID: stuID }
    $.post("?action=GetDiscussList&Rand=" + Math.random(), obj, function (data) {
        data = eval("(" + data + ")");
        if (data.Success) {
            //文本置空
            $(".answerText").val("");
            //讨论总数            
            $("#p_" + leadDisID).find(".downCommend").html("展开讨论(" + data.Data.DisCount + ")");
            $("#d_" + leadDisID).html(ShowDiscuss(leadDisID, data.Data.DiscussList, 0));
        }
        else {
            alert(data.Message);
        }
    });
}

function GetPreviewUrl(id, url, callback) {
    var sendValues = { FileID: id };
    $.ajax({
        type: "POST",
        url: url,
        data: sendValues,
        dataType: "jsonp",
        async: true,
        success: function (response) {
            callback(response);
        },
        error: function (request, status, error) {
        }
    });
}