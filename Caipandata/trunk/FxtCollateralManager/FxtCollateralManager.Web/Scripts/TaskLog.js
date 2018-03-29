/**
作者:贺黎亮
时间:2014.06.06
摘要:新建
    2014.6.11 任务日志列表，
*/

SearchBankProjectLog();

function SearchBankProjectLog() {
    $.ActionPage.pageSize = 100;
    $.ActionPage.pageIndex = 1;
    $.ActionPage.taskid = curid;
    $.ActionList({
        tourl: "/Task/GetTaskLogList",
        toid: "taskloglist>tbody",
        tmplid: "tasklog_template",
        sdata: $.ActionPage,
        complete: function (d) {
        }
    });
}

