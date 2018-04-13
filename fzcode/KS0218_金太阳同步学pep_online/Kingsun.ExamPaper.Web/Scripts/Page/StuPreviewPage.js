//学生预习页面

var stuTaskID = '';
var obj = '';
var leadID = '';
var stuID = '';
$(function () {
    stuTaskID = Common.QueryString.GetValue("StuTaskID");

    if (stuTaskID == "undefined")
    {
        alert("传入参数错误！");
        return;
    }
    //获取预习内容
    GetPrepareInfo();
});

//获取预习内容
function GetPrepareInfo()
{
    obj = { StuTaskID: stuTaskID };
    $.post("?action=GetPrepareInfo&Rand=" + Math.random(), obj, function (result) {
        if(result)
        {
            var resourceHtml = '';
            result = eval("(" + result + ")");
            if (result.Success) {
                leadID = result.Data[0].LeadID;
                stuID = result.Data[0].StudentID;
                var leadInfo = result.Data[0].Lead;
                for (var i = 0; i < leadInfo.Reslist.length; i++)
                {
                    var img = Constant.file_Url + "GetFileImg.ashx?fileID=" + leadInfo.Reslist[i].ResourceUrl + "&view=true";
                    resourceHtml += '<li><a onclick="GoToRescource(\'' + leadInfo.Reslist[i].ResourceID + '\')">'
                        + '<h2>' + GetResourceType(leadInfo.Reslist[i].ResourceType, leadInfo.Reslist[i].ResourceStyle) + '</h2>'
                        + '<img src="' + img + '" alt="" width="571" height="297"/>'
                        + '<p>' + leadInfo.Reslist[i].ResourceName + '</p>'
                        +'</a></li>';
                }
                $("#recList").html(resourceHtml);

            } else {
                alert(result.Message);
            }
        }
    });
}

//保存学生答案记录
function GoToRescource(resourceID)
{
    obj = { ResourceID: resourceID, StuTaskID: stuTaskID, StudentID: stuID };
    $.post("?action=SaveStuAnswer&Rand=" + Math.random(), obj, function (result) {
        if (result) {
            result = eval("(" + result + ")");
            if (result.Success) {
                window.location.href = result.Data +"/SunnyTask/Student/ResourcePreview.aspx?ID=" + resourceID + "&LeadID=" + leadID + "&StuTaskID=" + stuTaskID + "&StuID=" + stuID;
            } else {
                alert(result.Message);
            }
        }
    });
}

//获取资源类型
function GetResourceType(type,style)
{    
    if (type != 0&&type!="")
    {
        //案例、媒体素材、量规集、文献、模版
        if (style!=null&&style!="") {
            return GetTypeName(style);
        }
        else {
            return GetTypeName(type);
        }
    }
}

//获取资源类型名称
function GetTypeName(type)
{
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