var FeedBackInit = function () {
    var Current = this;

    this.Init = function () {
        Current.InitFeedBackList();
    };

    //加载用户信息列表
    this.InitFeedBackList = function () {
        $('#tbdatagrid').datagrid({
            url: "?action=queryFeedBackList",
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pageSize: 20,
            pageList: [10, 20, 30, 40, 50],
            width: $(window).width() - 20,
            height: $(window).height() - 120,
            columns: [[
                { field: 'UserName', title: '电话', align: 'center', width: 30 },
                {
                    field: 'ProblemDescription', title: '问题描述', align: 'center', width: 45, formatter: function (value, rows) {
                        var html = '';
                        var content = rows.ProblemDescription.substring(0, 15) + '...';
                        html += '<a title="' + rows.ProblemDescription + '" style="text-decoration:none;" href="javascript:void(0)" onclick="feedBackInit.FeedbackDetails(' + rows.ID + ',\'' + rows.ProblemDescription + '\')">' + (rows.ProblemDescription.length > 15 ? content : rows.ProblemDescription) + '</a>';
                        return html;
                    }
                },
                {
                    field: 'CreateDate', title: '提交时间', align: 'center', width: 30, formatter: function (value, rows) {
                        var html = '';
                        html += '<span>' + Common.FormatTime(rows.CreateDate, "yyyy-MM-dd hh:mm:ss") + '</span>';
                        return html;
                    }
                },
                {
                    field: 'FeedbackLevel', title: '反馈有用等级', align: 'center', width: 30, formatter: function (value, rows) {
                        var html = '';
                        var content = Current.ConfirmFeedbackLevel(rows.FeedbackLevel);
                        if (content == "很好") {
                            html += '<span style="color:red" title="看了一下">' + content + '</span>';
                        } else if (content == "还行") {
                            html += '<span title="看了一下">' + content + '</span>';
                        } else {
                            html += '<span>' + content + '</span>';
                        }
                        return html;
                    }
                }
            ]]
        });
    }

    //确认反馈等级
    this.ConfirmFeedbackLevel = function (levelNum) {
        var num = levelNum;
        var msg = "";
        switch (num) {
            case 0:
                msg = "没用，删了吧";
                break;
            case 1:
                msg = "还行";
                break;
            case 2:
                msg = "很好";
                break;
            default:
                var msg = "还未查看";
                break;
        }
        return msg;
    }

    //更改反馈等级
    this.FeedbackDetails = function (ID, Content) {
        var feedBackID = ID;
        var content = Content;
        $("#feedBackInfo").attr("style", "display:block");
        $("#feedBackDetail").val(content);
        $('#feedBackInfo').dialog({
            title: '详情描述',
            width: 450,
            height: 250,
            closed: false,
            cache: false,
            modal: true,
            buttons: [{
                text: '没用，删了吧',
                handler: function () {
                    Current.UpdateFeedback(feedBackID, 0);
                }
            }, {
                text: '还行',
                handler: function () {
                    Current.UpdateFeedback(feedBackID, 1);
                }
            }, {
                text: '很好',
                handler: function () {
                    Current.UpdateFeedback(feedBackID, 2);
                }
            }]
        });
    }

    //更新用户反馈信息
    this.UpdateFeedback = function (ID, Level) {
        var feedBackID = ID;
        var level = Level;
        var content = $("#feedBackDetail").val();
        if (content == "") {
            alert('反馈信息不能为空！');
            return false;
        }
        var obj = { FeedBackID: feedBackID, FeedbackLevel: level, ProblemDescription: content };
        $.post("?action=update", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result) {
                    $('#tbdatagrid').datagrid("reload");
                    $("#feedBackInfo").dialog('close');
                } else {
                    alert("保存失败!");
                }
            }
        });
    }

}

var feedBackInit;
$(function () {
    feedBackInit = new FeedBackInit();
    feedBackInit.Init();
});
