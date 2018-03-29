/*
作者:李晓东
摘要:2014.02.20 新建 押品操作
     2014.02.28 修改:getCollateral 中的不足
     2014.03.14 修改人:李晓东
                修改:fPage中的$.Global_fdata,$.Global_data 注释掉
     2014.06.25 修改人:李晓东
                清理一些不必要的函数
*/

var oName = null, oType = null;
/*获得标准化已确认列表*/
function getCollateral(page) {
    var cpage = $.ActionPage,
        tabfade = $('.tab-content>.active'),
        findtable = $('table', tabfade),
        nation = $('.pagination', tabfade),
        paginfo = $('#page_info', tabfade);
    cpage.pageSize =  150;
    if (page == undefined) {
        cpage.pageIndex = 1;
    } else {
        cpage.pageIndex = page.pageIndex;
    }
    if (oName != null && oType != null) {
        cpage["orderName"] = oName;
        cpage["orderType"] = oType;
    }
    cpage.uploadfileid = $("#uploadfileid").val();
    $.extendAjax({
        url: '/Collateral/GetCollateral',
        type: 'post',
        data: cpage,
    }, function (data) {
        $("#{0}>tbody".format(findtable.attr('id'))).empty();
        if (data.type == 1 && data.data.length > 0) {
            $.ActionTmpl("templete_ok", data.data, "{0}>tbody".format(findtable.attr('id')));
            $.ProcessPage({
                pagination: nation,
                paginfo: paginfo,
                count: data.count,
                page: cpage,
                complete: function (page) {
                    getCollateral(page);
                }
            });
            findtable.extendTable({
                firstNumber: -1,
                save: true,
                edit: true,
                cancel: true,
                remove: parseInt($.trim($("#uploadfileid").val())) != 0,
                sortColumn: ["省份", "城市", "行政区", "楼盘名", "楼栋"],
                sortComplete: function (orderName, orderType) {
                    oName = orderName;
                    oType = orderType;
                    getCollateral(cpage);
                }
            });
        }
    });
}

$(document).ready(function () {
    /*加载目城市*/
    CreateItemList(".procheckbox-list");
    
    /*标准化 进行中的入库按钮*/
    $('.excesave').live('click', function () {
        var table = $(this).parent().parent().find('table'),
            tbodytr = $('tbody tr', table),
            checkboxs = $('tbody input[type=checkbox]:checked', table),
            savemessage = $("#savemessage"), message = "共有{0}条记录需要保存,已保存{1}条,失败{2}条!请勿操作...",
            i = 0, rows = [], errri = 0, successi = 0;
        if (tbodytr.length > 0) {
            if (checkboxs.length != 0) {/*如果有被选中的*/
                $.each(checkboxs, function () {
                    var tr = $(this).parents('tr'),
                        firstTd = $('td:first', tr);
                    if (firstTd != undefined)
                        rows.push(tr);
                });
                excSave();
            } else {/*默认全部*/
                $.each(tbodytr, function () {
                    //rows = tbodytr;
                    firstTd = $('td:first', this);
                    if (firstTd != undefined)
                        rows.push($(this));
                });
                var saveDialog = $('<div></div>');
                saveDialog.extendDialog({
                    content: '确定保存全部?',
                    buttons: {
                        '确定': function () {
                            $(this).dialog('close');
                            excSave();
                        },
                        '取消': function () {
                            $(this).dialog('close');
                        }
                    }
                });
            }
        }
        function excSave() {
            if (rows != null && rows.length > 0) {
                savemessage.html('执行中...');
                rowSave(rows[i]);
                i++;
            }
        }
        function rowSave(row) {/*一条一条的保存*/
            $.extendTableSave({
                nRow: row,
                edit: true,
                cancel: true,
                remove:true,
                firstNumber: -1,
                complete: function (saveData,crow) {
                    if (saveData.type == 1) {
                        successi++;
                        var firstTd = $('td:first', rows[i - 1]);
                        firstTd.attr('data-val', saveData.data.Id);
                    }
                    if (i != rows.length) {/* && i == 1 测试*/
                        rowSave(rows[i]);
                        i++;
                    } else {
                        savemessage.html('正在保存当前操作记录,请稍后...');
                        setTimeout(function () {
                            savemessage.html('');
                        }, 2500);

                    }
                    if (saveData.type == 0) {
                        errri++;
                    }
                    savemessage.html(message.format(rows.length, successi, errri));
                }
            });
        }
    });
    if (parseInt($.trim($("#uploadfileid").val())) != 0) {
        $('.nav li:last>a').trigger('click');
        setTimeout(function () {
            getCollateral();
        }, 1000);
    }
});

