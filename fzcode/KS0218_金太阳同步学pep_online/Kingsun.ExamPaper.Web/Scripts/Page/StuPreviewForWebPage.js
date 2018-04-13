//学生预习页面

var leadID = '';
var obj = '';
var stuTaskID = '';
var leadInfo = '';
var pageIndex = '';
var leadDisList = '';//记录
var stuID = '';
$(function () {
    stuTaskID = Common.QueryString.GetValue("StuTaskID");
    pageIndex = Common.QueryString.GetValue("PageIndex");
    stuID = $("#stuID").val();
    if (stuTaskID == "undefined" || pageIndex == "undefined") {
        alert("传入参数错误！");
        return;
    }
    $(".topTitle a").attr("href", "../Student/StuTaskList.aspx?PageIndex=" + pageIndex);
    //获取预习内容
    GetResourceInfo();
});

//获取预习内容
function GetResourceInfo() {
    obj = { StuTaskID: stuTaskID };
    //获取资源信息
    $.post("?action=GetResourceInfo&Rand=" + Math.random(), obj, function (result) {
        if (result) {
            result = eval("(" + result + ")");
            if (result.Success) {
                leadInfo = result.Data;
                //加载导学页面信息
                ShowLeadHtml(result.Data.ResourceInfo[0].ResourceID);

                //加载资源信息
                ShowResourceHtml(result.Data.ResourceInfo[0].ResourceID);

                ShowDisInfo();
                showtextarea('');
            } else {
                alert(result.Message);
            }
        }
    });
}

//加载导学页面信息
function ShowLeadHtml(resourceID) {
    //显示左边资源表
    //资源集合
    var resourceList = leadInfo.ResourceInfo;
    var resHtml = '';
    for (var i = 0; i < resourceList.length; i++) {
        resHtml += '<li id=' + resourceList[i].ResourceID + '><a onclick="ChangeRes(\'' + resourceList[i].ResourceID + '\')">';
        resHtml += '<h2>' + GetResourceType(resourceList[i].ResourceType, resourceList[i].ResourceStyle) + '</h2>';
        var img = Constant.file_Url + "GetFileImg.ashx?fileID=" + resourceList[i].ResourceUrl + "&view=true";
        resHtml += '<img src="' + img + '" alt=""/>';
        resHtml += '<p>' + resourceList[i].ResourceName + '</p>';
        resHtml += '</a></li>';
    }
    $(".recList").html(resHtml);
    //显示讨论内容
    $("#target").html(leadInfo.Traget);

    $(".queList").html(ShowLeadDis(leadInfo.LeadDis));
}

//加载讨论
function ShowLeadDis(leadDisList) {
    //显示预习讨论主题
    var discussHtml = '';
    discussHtml += '<ol>';
    for (var i = 0; i < leadDisList.length; i++) {
        //讨论主题
        discussHtml += '<li><p class="disCuS"><label>问题' + (i + 1) + '：</label>'
            + '<a href="javascript:void(0)" class="queInfo">' + leadDisList[i].DiscussContent + '</a>';
        discussHtml += '<i class="discussStu" id="i_' + leadDisList[i].LeadDiscussID + '">参与讨论(' + leadDisList[i].DisCount + ')</i></p>';
        //学生参与情况
        discussHtml += '<div class="commendList"><ul>';
        discussHtml += '<div class="commendTextarea"><h3>评论<label id="lab_' + leadDisList[i].LeadDiscussID + '">(' + leadDisList[i].DisCount + ')</label></h3>' +
                        '<textarea placeholder="我来说几句..."></textarea>' +
                        '<p><input type="button" value="发表评论" onclick="Submit(this,0,\'' + leadDisList[i].LeadDiscussID + '\')"/></p></div>';
        discussHtml += '<ul class="discussList" id="ul_' + leadDisList[i].LeadDiscussID + '">';
        discussHtml += ShowDiscuss(leadDisList[i].LeadDiscussID, leadDisList[i].DiscussList, 0);
        discussHtml += '</ul></div></li>';
    }
    discussHtml += '</ol>';
    return discussHtml;
}

//显示预习讨论内容
function ShowDiscuss(leadDisID, discussData, parentID) {
    var disList = '';
    if (discussData != null && discussData.length > 0) {
        for (var i = 0; i < discussData.length; i++) {
            var avaUrl = discussData[i].AvatarUrl == null ? "../App_Themes/images/defultHeadStu.jpg" : discussData[i].AvatarUrl;
            if (parentID == 0) {
                disList += '<li> <img src="' + avaUrl + '" width="40" height="40">';
                disList += '<h3>' + discussData[i].TrueName + '</h3>';
                disList += '<p>' + discussData[i].ReplyContent;
                disList += '<span><em>' + Common.FormatTime(discussData[i].DiscussTime,"yyyy-MM-dd hh:mm") + '</em>';
                if (stuID == discussData[i].UserID) {
                    disList += '<a class="answerDelete" onclick="DelDiscuss(' + discussData[i].DiscussID + ',\'' + leadDisID + '\')">删除</a>';
                }
                disList += '<a href="javascript:void(0)" class="answerClick">回复</a></span></p>';
                disList += '<div class="commendTextarea">' +
                            '<div class="textareaShow">' +
                            '<label class="studentName">回复 ' + discussData[i].TrueName + '：</label>' +
                            '<textarea ></textarea>' +
                            '</div>' +
                            '<p><input type="button" value="提交" class="submitBtn" onclick="Submit(this,' + discussData[i].DiscussID + ',\'' + leadDisID + '\')"/></p>' +
                            '</div></li>';
                if (discussData[i].ChildDis.length > 0) {
                    disList += ShowChildDis(leadDisID, discussData[i].ChildDis, discussData[i].DiscussID, discussData[i].TrueName);
                }
            }
        }
    }
    return disList;
}

//加载讨论子集合
function ShowChildDis(leadDisID, discussData, parentID,parentName) {
    var disList = '';
    for (var i = 0; i < discussData.length; i++) {
        var avaUrl = discussData[i].AvatarUrl == null ? "../App_Themes/images/defultHeadStu.jpg" : discussData[i].AvatarUrl;
        disList += '<li class="secLi"> <img src="' + avaUrl + '" width="40" height="40">';
        disList += '<h3>' + discussData[i].TrueName + '</h3>';
        disList += '<p><b>回复 ' + parentName + '：</b>' + discussData[i].ReplyContent;
        disList += '<span><em>' + Common.FormatTime(discussData[i].DiscussTime, "yyyy-MM-dd hh:mm") + '</em>';
        if (stuID == discussData[i].UserID)
        {
            disList += '<a class="answerDelete" onclick="DelDiscuss(' + discussData[i].DiscussID + ',\'' + leadDisID + '\')">删除</a>';
        }
        disList += '<a href="javascript:void(0)" class="answerClick">回复</a></span></p>';
        disList += '<div class="commendTextarea">' +
            '<div class="textareaShow">' +
            '<label class="studentName">回复 ' + discussData[i].TrueName + '：</label>' +
            '<textarea></textarea>' +
            '</div>' +
            '<p><input type="button" value="提交" class="submitBtn" onclick="Submit(this,' + discussData[i].DiscussID + ',\'' + leadDisID + '\')"/></p>' +
            '</div></li>';
        if (discussData[i].ChildDis.length > 0) {
            disList += ShowChildDis(leadDisID, discussData[i].ChildDis, discussData[i].DiscussID, discussData[i].TrueName);
        }
    }
    return disList;
}

//加载资源信息
function ShowResourceHtml(resourceID) {
    //去除选中
    $.each($(".leftMenu .recList li"), function (index) {
        $("#" + (this.id)).removeClass();
    });
    for (var i = 0; i < leadInfo.ResourceInfo.length; i++) {
        if (resourceID == leadInfo.ResourceInfo[i].ResourceID) {
            //点击资源的资源选中
            $("#" + (resourceID)).addClass("on");
            var resource = leadInfo.ResourceInfo[i];
            var index = i;
            //显示资源标题
            $(".topTitle h1").html("预习——" + resource.ResourceName);
            //首先加载资源预览
            GetPreviewUrl(resource.ResourceUrl, Constant.file_Url + "Preview.ashx", function (res) {
                if (res.CanPreview) {
                    $("#previewPage").attr("src", res.URL);
                    ////显示点赞数和浏览数
                    //$("#scanNum").html(resource.ScanCounts);
                    //$("#goodNum").html(resource.GoodCounts);
                    ////初始化点赞
                    //if ($(".agree").hasClass("agreed")) {
                    //    $(".agree").removeClass("agreed");
                    //    $(".agree").attr('title', "点赞");
                    //}
                    ////判断用户是否点赞
                    //UserHasGood(resource.ResourceID);
                    ////浏览次数+1
                    //OperateResource(index, resource.ResourceID, 3);

                    ////添加点赞操作
                    //$("#goodNum").unbind().bind("click", function () {
                    //    OperateResource(index, resourceID, 4);
                    //});

                    ////学生完成该预习
                    //DoTask(resourceID);
                }
            });
            //显示点赞数和浏览数
            $("#scanNum").html(resource.ScanCounts);
            $("#goodNum").html(resource.GoodCounts);
            //初始化点赞
            if ($(".agree").hasClass("agreed")) {
                $(".agree").removeClass("agreed");
                $(".agree").attr('title', "点赞");
            }
            //判断用户是否点赞
            UserHasGood(resource.ResourceID);
            //浏览次数+1
            OperateResource(index, resource.ResourceID, 3);

            //添加点赞操作
            $("#goodNum").unbind().bind("click", function () {
                OperateResource(index, resourceID, 4);
            });
            //学生完成该预习
            DoTask(resourceID);
        }
    }
}

//切换资源
function ChangeRes(resourceID) {
    //选中资源
    //显示资源
    ShowResourceHtml(resourceID);
}

//提交作业
function DoTask(resourceID) {
    obj = { ResourceID: resourceID, StuTaskID: stuTaskID };
    $.post("?action=SaveStuAnswer&Rand=" + Math.random(), obj, function (data) {
        data = eval("(" + data + ")");
        if (data.Success) {
        }
        else {
            alert(data.Message);
        }
    });
}

//判断用户是否对该资源进行过点赞操作
function UserHasGood(resourceID) {
    //判断该用户是否对此资源点赞
    obj = { ResourceID: resourceID }
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

//发表评论或回复
function Submit(cur, pDiscussID, leadDisID) {
    var disContent = $(cur).parent().parent().find("textarea").val();
    var content = '';
    if (disContent == "") {
        Tips('请填写评论内容！');
        return;
    }
    //加载中
    var dialog = art.dialog({
        id: 'loading',
        opacity: .1,
        padding: '0',
        lock: true,
        content: '<img style="width:300px;height:300px;align:center" src="../App_Themes/images/Loading.gif" />'
    });
    $(".aui_close").hide();
    //发送评论
    obj = { LeadDisID: leadDisID, DiscussContent: disContent, ParentDisID: pDiscussID }
    $.post("?action=SendDiscuss&Rand=" + Math.random(), obj, function (data) {
        data = eval("(" + data + ")");
        Tips(data.Success ? '发表成功！' : data.Message);
        
        dialog.close();
        GetSingleDis(cur, leadDisID);
    });
}

function DelDiscuss(discussID, leadDisID) {
    if (confirm("确定要删除此评论？")) {
        //加载中
        var dialog = art.dialog({
            id: 'loading',
            opacity: .1,
            padding: '0',
            lock: true,
            content: '<img style="width:300px;height:300px;align:center" src="../App_Themes/images/Loading.gif" />'
        });
        $(".aui_close").hide();
        //删除评论
        obj = { DiscussID: discussID }
        $.post("?action=DelDiscuss&Rand=" + Math.random(), obj, function (data) {
            data = eval("(" + data + ")");
            Tips(data.Success ? '删除成功！' : data.Message);
            dialog.close();
            GetSingleDis(this,leadDisID);
        });
    }
}

//弹窗
function Tips(content)
{
    var dialog = art.dialog({
        time: 2,
        opacity: .1,
        lock: true,
        content: '<div class="tipMsg"><span><h4>提示：</h4><p>' + content + '</p></span>'
    });
    $(".aui_close").hide();
}

//刷新讨论
function GetSingleDis(cur, leadDisID) {
    obj = { LeadDisID: leadDisID }
    $.post("?action=GetDiscussList&Rand=" + Math.random(), obj, function (data) {
        data = eval("(" + data + ")");
        if (data.Success) {
            //文本置空
            $(cur).parent().parent().find("textarea").val("");
            //讨论总数
            var str = "参与讨论(" + data.Data.DisCount + ")";
            $("#lab_" + leadDisID).html(data.Data.DisCount);
            $("#i_" + leadDisID).html(str);
            $("#ul_" + leadDisID).html(ShowDiscuss(leadDisID, data.Data.DiscussList, 0));
            showtextarea(leadDisID);
        }
        else {
            alert(data.Message);
        }
    });
}

//更新用户操作资源记录（4：点赞；3：浏览）
function OperateResource(index,resourceID, type) {
    obj = { ResourceID: resourceID, Type: type }
    $.post("?action=OperateResource&Rand=" + Math.random(), obj, function (data) {
        data = eval("(" + data + ")");
        if (data.Success) {
            var result = eval("(" + data.Data + ")");
            if (result.State) {
                if (type == 4) {
                    $(".agree").html(result.Count);
                    if ($(".agree").hasClass("agreed")) {
                        $(".agree").removeClass("agreed");
                        $(".agree").attr('title', "点赞");
                        leadInfo.ResourceInfo[index].GoodCounts-=1;
                    }
                    else {
                        $(".agree").addClass("agreed");
                        $(".agree").attr('title', "取消点赞");
                        leadInfo.ResourceInfo[index].GoodCounts+=1;
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

//资源浏览
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

/*展开内容简介*/
function ShowDisInfo() {
    $(".queList ol li p.disCuS").click(function () {
        $(".queList ol li .commendList").not($(this).parent().find(".commendList")).slideUp();
        $(this).parent().find(".commendList").slideToggle();
    });
}
/*回复文本框的显隐以及文本框text-indent的动态改变*/
function showtextarea(id) {
    if (id == '') {
        $(".discussList li p a.answerClick").click(function () {
            var textareaBox = $(this).parent().parent().next(".commendTextarea");
            $(".discussList li .commendTextarea").not(textareaBox).slideUp();
            textareaBox.slideToggle();
            var bl = textareaBox.css("display");
            if (bl = "block") {
                var strlength = textareaBox.find(".studentName").width();
                textareaBox.find("textarea").css("text-indent", strlength + 'px');
            }
        });
    } else {
        $("#ul_"+id+" li p a.answerClick").click(function () {
            var textareaBox = $(this).parent().parent().next(".commendTextarea");
            $(".discussList li .commendTextarea").not(textareaBox).slideUp();
            textareaBox.slideToggle();
            var bl = textareaBox.css("display");
            if (bl = "block") {
                var strlength = textareaBox.find(".studentName").width();
                textareaBox.find("textarea").css("text-indent", strlength + 'px');
            }
        });
    }
}

//获取资源类型
function GetResourceType(type, style) {
    if (type != 0 && type != "") {
        //案例、媒体素材、量规集、文献、模版
        if (style != null && style != "") {
            return GetTypeName(style);
        }
        else {
            return GetTypeName(type);
        }
    }
}

//获取资源类型名称
function GetTypeName(type) {
    var typename = '';
    switch (type) {
        case 1:
            typename = "媒体素材";
            break;
        case 2:
            typename = "文本";
            break;
        case 3:
            typename = "图形";
            break;
        case 4:
            typename = "视频";
            break;
        case 5:
            typename = "音频";
            break;
        case 6:
            typename = "动画";
            break;
        case 7:
            typename = "量规集";
            break;
        case 8:
            typename = "试题";
            break;
        case 9:
            typename = "试卷";
            break;
        case 10:
            typename = "课件";
            break;
        case 11:
            typename = "案例";
            break;
        case 12:
            typename = "教案";
            break;
        case 13:
            typename = "学案";
            break;
        case 14:
            typename = "学生作品";
            break;
        case 15:
            typename = "教学设计";
            break;
        case 16:
            typename = "文献资料";
            break;
        case 17:
            typename = "论文";
            break;
        case 18:
            typename = "报告";
            break;
        case 19:
            typename = "教学大纲";
            break;
        case 20:
            typename = "工具书";
            break;
        case 21:
            typename = "其他";
            break;
        case 22:
            typename = "课程";
            break;
        case 23:
            typename = "教与学工具和模板";
            break;
        case 24:
            typename = "课程设计软件";
            break;
        case 25:
            typename = "学习工具软件";
            break;
        case 26:
            typename = "教学方法模板";
            break;
        case 27:
            typename = "互动课件";
            break;
        case 28:
            typename = "图片";
            break;
        default:
            break;
    }
    return typename;
}