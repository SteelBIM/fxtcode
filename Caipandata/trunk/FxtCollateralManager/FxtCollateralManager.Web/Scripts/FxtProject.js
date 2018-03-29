/**
作者:李晓东
时间:2014.06.06
摘要:新建
    2014.6.11 文件项目列表，添加修改删除
*/
$.ActionSelectList = [
    {
        name: 'bankid',
        url: '/Users/GetAllCustomer/?customerType=2001013',
        val: 'CustomerName',
        key: 'CustomerId'
    }];
$.ActionOperta({
    toid: ".actions",
    ActionFormRules: $.projectrules,
    ReloadUrl: '/Projects/Index'
});


SearchBankProject();

$("#searchbankpro").click(function () {
    SearchBankProject();
});

function SearchBankProject() {
    $.ActionPage.pageSize = 10;
    $.ActionPage.pageIndex = 1;
    $.ActionPage.projectname = $("#projectname").val();
    $.ActionPage.bankid = $("#banksel").val();
    $.ActionList({
        tourl: "/Projects/GetSysBankProjectList",
        toid: "listbankproject>tbody",
        sdata: $.ActionPage
    });
    if ($("#banksel>option").length <= 0) {
        var bankmes = [];
        $.extendAjax({
            url: '/Users/GetAllCustomer',
            type: 'post',
            data: { customerType: 2001013 }
        }, function (data) {
            bankmes.push("<option value=\"0\" >-请选择-</option>");
            $(data.data).each(function (i, o) {
                bankmes.push("<option value=" + o.CustomerId + " >" + o.CustomerName + "</option>");
            });
            $("#banksel").empty().append(bankmes.join(""));
        });
    }
}


