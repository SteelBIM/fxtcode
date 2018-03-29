/***
*作者:李晓东
*摘要:新建 2014.01.20
      2014.01.27 修改人:李晓东
                 对$.ActionList的调整,新增fExcute和fPage
      2014.01.28 修改人:李晓东
                 新增ElementSelect,fColumns 对列的控制
      2014.02.07 修改人:李晓东
                 修改fColumns函数中的取消操作
      2014.02.08 修改人:李晓东
                 修改fColumns,editRow,新增chSplit
      2014.02.10 修改人:李晓东
                 修改editRow,fColumns
      2014.02.11 修改人:李晓东
                 修改fExcute,dialogClose,ActionList
      2014.02.20 修改人:李晓东
                 把 fExcute fPage fColumns 三个函数移到FxtCollateral.js文件中
      2014.03.10 修改人:李晓东
                 修改:fload中新增押品全部获取
      2014.06.11 修改人:贺黎亮
                 添加:银行，项目下拉选择，上传文件验证
      2014.07.08 修改人:李晓东
                 修改:弹出我的文件中搜索按钮及条件无效的问题及与其他页面冲突,修正fload无法正常使用
**/
var oldDialog = null,
    isClose = false,
    Global_FileName = '', lisetUploadPage;
function fload() {
    var nation = $('.dialog_pagination'), paginfo = $('#dialog_page_info');
    if (lisetUploadPage == undefined) {
        lisetUploadPage = $.ActionPage
        lisetUploadPage.pageSize = 10;
        lisetUploadPage.pageIndex = 1;
    }
    lisetUploadPage.bankid = $("#upload_banksel").val();
    lisetUploadPage.proid = $("#projectsel").val();
    lisetUploadPage.key = $("#dialog_projectname").val();
    $.extendAjax({
        url: '/Upload/GetFiles',
        type: 'post',
        data: lisetUploadPage
    }, function (frdata) {        

        $.ActionTmpl("upload_template", frdata.data, "listupload>tbody");
        $.ProcessPage({
            pagination: nation,
            paginfo: paginfo,
            count: frdata.count,
            page: lisetUploadPage,
            complete: function (page) {
                lisetUploadPage.pageIndex = lisetUploadPage.pageIndex;
                fload()
            }
        });
    });
}

/*-添加任务 */
function AddTask(bankid, uploadfileid, bankproid,  tasktype) {
    $.extendAjax({
        url: '/CollateralReassessment/ReassTask',
        type: 'post',
        data: { bankid: bankid, tasktype: tasktype, uploadfileid: uploadfileid, bankproid: bankproid}
    }, function (data) {
        if (data.type == 1) {
            CreateTask();
            $.extendDialog({
                title: '提示',
                content: "添加成功"
            });
        }
    });
}

function dialogClose(dial) {
    dial.dialog('close');
}
/*上传*/
function fuplod() {
    if ($("#projectsel").val() == "0") {
        $.extendDialog({
            title: '提示',
            content: "请选择项目"
        });
        return;
    }
    $.extendAjaxFile({
        formid: '#fupload',
        success: function () {
            $("#file").val("");
            fload();
        }
    });
}

/*上传 */
$("#file").change(function () {
    fuplod();
});

/* 银行项目联动*/
function BankProject() {
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
        $("#upload_banksel").append(bankmes.join(""));
    });
    $("#upload_banksel").change(function () {
        GetBankPro($(this).val());
    });
}

/* 获取指定银行项目*/
function GetBankPro(bankid) {
    $.extendAjax({
        url: '/Projects/GetSysBankProjectList',
        type: 'post',
        data: { bankid: bankid }
    }, function (data) {
        bankmes = [];
        bankmes.push("<option value=\"0\" >-请选择-</option>");
        $(data.data).each(function (i, o) {
            bankmes.push("<option value=" + o.Id + " >" + o.ProjectName + "</option>");
        });
        $("#projectsel").empty().append(bankmes.join(""));
        $("#projectsel").change(function () {
            if ($(this).val() != "0") {
                $("#uploadtip>span").parent().hide(1000);
            }
        })
    });
}

/*查询 */
function Searchprofile() {
    $("#dialog_searchbankpro").click(function () {
        $("#uploadtip>span").parent().hide();
        fload();
    });
}

$(document).ready(function () {
    fload();
    BankProject();
    Searchprofile();
    $("#addproject").click(function () {
        AddProject();
    });
});

/* 添加项目*/
function AddProject() {
    var field = $("#template-project");
    isClose = false;
    oldDialog = field;
    field.extendDialog({
        title: '添加项目',
        open: function () {
            /*设置form提交目标*/
            field.find('select').html($("#upload_banksel").html());
            $("#addprojetsub").click(function () {
                if ($(this).attr("iclose") == "1") {
                    dialogClose(oldDialog);//关闭父级弹出框
                    return;
                }
                if ($("#bankprojectname").val() == "") {
                    $("#subtip").show();
                    return;
                }
                if ($("#addbankpro").val() == "0") {
                    $("#subtip").show();
                    return;
                }
                $.extendAjax({
                    url: '/Projects/AddEditProjects',
                    type: 'post',
                    data: { bankid: $("#addbankpro").val(), projectname: $("#bankprojectname").val() }
                }, function (data) {
                    dialogClose(oldDialog);//关闭父级弹出框
                    if ($("#upload_banksel").val() != "0") {
                        GetBankPro($("#upload_banksel").val());
                    }
                });
            });
            $("#exitprojetsub").click(function () {
                dialogClose(field);//关闭父级弹出框
            });
        },
        close: function () {
            if (isClose)
                dialogClose(parentDialog);//关闭父级弹出框
        }
    });
}