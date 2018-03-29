/**
作者:贺黎亮
时间:2014.06.06
摘要:新建
    2014.6.11 任务列表，添加修改删除
*/
var msgtime = null;
var curid = 0;
var curfileid = 0;

$.ActionOperta({
    toid: "#tab1>.actions",
    ActionFormRules: $.projectrules,
    ReloadUrl: '/Projects/Index?status=-1'
});

SearchBankProject();

$("#searchbankpro").click(function () {
    SearchBankProject();
});

$.ActionPage.pageSize = 10;
$.ActionPage.pageIndex = 1;
/*查询*/
function SearchBankProject() {
    $('.row #page_info').empty();
    $('.row .pagination').empty();
    clearTimeout(msgtime);
    $.ActionPage.projectname = $("#projectname").val();
    $.ActionPage.status = $("#taskstatus").val();
    $.ActionList({
        tourl: "/Task/GetTaskList",
        toid: "tasklist>tbody",
        sdata: $.ActionPage,
        isBlockUI:false,
        complete: function (d) {
            var trs = $('tbody tr', $('#tasklist'));
            var successcount = 0, failurecount = 0, _count=0;
            trs.each(function () {
                var tds = $('>td', this);
                tds[2].innerHTML = $(tds[2]).text();
                $(tds[3]).find("a").click(function () {
                    var fid = $(this).attr("data-file");
                    if ($(this).attr("type") == "export") {
                        window.location = '/Task/TaskExport?uploadfileid=' + fid;
                    }
                    else {
                        var status = $(this).attr("data-status");
                        if ($(this).attr("type") == "log") {
                            curid = $(this).attr("data-val");
                            parentDialog.html('加载中...');
                            parentDialog.extendDialog({
                                title: '日志详情',
                                minWidth: 750,
                                maxHeight: 600,
                                open: function (event, ui) {
                                    $(this).extendLoad({ url: "/Task/TaskLog" });
                                    $(this).css({ 'text-align': '' });
                                },
                                close: function (event, ui) {

                                }
                            });
                            return;
                        }
                        if (status == 1) {
                            RedireUrl("/Collateral/Index", { uploadfileid: fid });
                            return;
                        }
                        var status = ProceStatus(status, 1);
                        $.extendAjax({
                            url: '/Task/EditTaskStatus',
                            type: 'post',
                            data: { id: $(this).attr("data-val"), status: status }
                        }, function (data) {
                            if (data.type) {
                                $(tds[3]).find("a").eq(0).attr("data-status", status).text(ProceStatus(status));
                            } else {
                                $.extendDialog({
                                    title: '提示',
                                    content: "操作失败"
                                });
                            }
                        });
                    }
                });
            });
            msgtime = setTimeout(SearchBankProject, navtimeseend);
        }
    });

}

/* 导入 */
function TaskExport(id) {
    $.extendAjaxFile({
        formid: "#frmtask_" + id,
        success: function (data) {
            $.BlockUI(false);
            data = $.parseJSON(data);
            $.extendDialog({
                title: '提示',
                content:data.message
            });
        }
    });
}
