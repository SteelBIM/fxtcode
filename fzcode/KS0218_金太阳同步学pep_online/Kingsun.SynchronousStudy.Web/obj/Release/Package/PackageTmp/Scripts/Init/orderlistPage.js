/// <reference path="../jquery-1.4.4.min.js" />
/// <reference path="../easyuiCommon.js" />
/// <reference path="../../jquery-easyui/jquery.easyui.min.js" />
/// <reference path="../Common.js" />

/// <reference path="../Management/OrderManagement.js" />

var OrderListPage = function () {
    var Current = this;
    this.Init = function () {
        //初始化时间控件
        $('#txtdate').daterangepicker({
            presetRanges: [
            //{text:'最近一周', dateStart:'yesterday-7days', dateEnd:'yesterday' },
            //{text:'最近一月', dateStart:'yesterday-1month', dateEnd:'yesterday' },
            //{text:'最近一年', dateStart:'yesterday-1year', dateEnd:'yesterday' }
            ],
            presets: {
                dateRange: '自定义时间'
            },
            rangeStartTitle: '起始日期',
            rangeEndTitle: '结束日期',
            nextLinkText: '下月',
            prevLinkText: '上月',
            doneButtonText: '确定',
            cancelButtonText: '取消',
            earliestDate: '',
            latestDate: Date.parse('today'),
            constrainDates: true,
            rangeSplitter: '--',
            dateFormat: 'yy-mm-dd',
            closeOnSelect: false,
            onOpen: function () {
                $('.ui-daterangepicker-dateRange').click().parent().hide();
            },
            onClose: function () {
                $("#tbfeesetting").datagrid("reload");
            }
        }
	    );


        //初始化版本列表


        $("#selectEdition").combobox({});


        var datag = {
            url: "?action=querylist&queryStr=" + Current.GetWhere(),
            toolbar: '#tb',
            width: $(window).width() - 20,
            height: $(window).height() - 55,
            nowrap: false,
            striped: true,
            autoRowHeight: false,
            singleSelect: true,
            fitColumns: true,
            pagination: true,
            rownumbers: true,
            pageSize: 20,
            columns: [[
        { title: '订单编号', field: 'OrderID', align: 'center', width: 100, align: 'center' }
        , {
            title: '支付状态', field: 'State', align: 'center', width: 60, align: 'center', formatter: function (value, row, index) {
                var html = '';
                if (value == "0000") {
                    html += '<span style="color:blue">未支付</span>';
                }
                else if (value == "0001") {
                    html += '<span style="color:green">支付成功</span>';
                }
                else if (value == "0002") {
                    html += '<span style="color:red">支付失败</span>';
                }
                else {
                    html += '<span style="color:black">未知</span>';
                }
                return html;
            }
        },
        {
            title: '购买时间', field: 'CreateDate', halign: 'center', width: 120, align: 'center', formatter: function (value, row, index) {
                date = new Date(parseInt(value.substring(6, value.length - 2)))
                return date.format("yyyy-MM-dd hh:mm:ss");
            }
        },
        { title: '手机号', field: 'TelePhone', align: 'center', width: 100, align: 'center' },
        { title: '购买金额（元）', field: 'TotalMoney', align: 'center', width: 40 }
        , { title: '套餐名称', field: 'FeeName', align: 'center', width: 100 }
        , { title: '购买渠道', field: 'PayWay', align: 'center', width: 100 }
        , {
            title: '购买时长（月）', field: 'Month', align: 'center', width: 60, formatter: function (value, row, index) {

                return value;
            }
        }
        , {
            title: '生效时间', field: 'StartDate', align: 'center', width: 120, formatter: function (value, row, index) {
                if (value) {
                    value = value || "";
                    date = new Date(parseInt(value.substring(6, value.length - 2)))
                    return date.format("yyyy-MM-dd hh:mm:ss");
                } else {
                    row.CreateDate = row.CreateDate || "";
                    date = new Date(parseInt(row.CreateDate.substring(6, row.CreateDate.length - 2)))
                    return date.format("yyyy-MM-dd hh:mm:ss");
                }

            }
        }
        , {
            title: '到期时间', field: 'EndDate', align: 'center', width: 120, formatter: function (value, row, index) {
                if (value) {
                    value = value || "";
                    date = new Date(parseInt(value.substring(6, value.length - 2)))
                    return date.format("yyyy-MM-dd hh:mm:ss");
                } else {
                    row.CreateDate = row.CreateDate || "";
                    date = new Date(parseInt(row.CreateDate.substring(6, row.CreateDate.length - 2)))
                    date.setMonth(date.getMonth() + row.Month);
                    return date.format("yyyy-MM-dd hh:mm:ss");
                }
            }
        }
        , { title: '购买版本', field: 'TeachingNaterialName', align: 'center', width: 120 }
        , {
            title: '创建时间', field: 'CreateTime', align: 'center', width: 120, formatter: function (value,row,index)
            {
                if (value) {
                    date = new Date(parseInt(value.substring(6, value.length - 2)))
                    return date.format("yyyy-MM-dd hh:mm:ss");
                }
            }
          }
        , { title: '班级短ID', field: 'ClassShortID', align: 'center', width:80 }

            ]]
        }; 
        $("#tbfeesetting").datagrid(datag);

        $("#searchbtn").click(function () {
            datag.url = "?action=querylist&queryStr=" + Current.GetWhere();
            $('#tbfeesetting').datagrid(datag);
        })


        //搜索按钮
        ;
        //        $("#selectPayway").change(function () {
        //            $('#tbfeesetting').datagrid('reload'); 
        //        });
        //        $("#selectEdition").change(function () {
        //            $('#tbfeesetting').datagrid('reload'); 
        //        });

        $("#exportExcel").click(function () {
            window.open("?action=excel&queryStr=" + Current.GetWhere());
        });
    }


    this.GetWhere = function () {
        var where = "";
        var payway = $('#selectPayway').combobox('getText');
        var edition = $('#selectEdition').combobox('getText');
        var time = $('#txtdate').val();
        var num = $('#searchkey').val();
        var ispay = $('[name="ispay"]:checked').val();
        if (parseInt($('#selectPayway').combobox('getValue'))) {
            where += "PayWay like '%" + payway + "%'";
        }

        if (edition) {
            where += where == "" ? "" : " and ";
            where += "FeeName='" + edition + "'";
        }
        if (time && time != "选择时间段") {
            var times = time.split("--");
            if ($.trim(times[1]) == "") {
                alert("请选择完整的日期区间", "错误", 1000);
                return;
            }
            where += where == "" ? "" : " and ";
            where += "CreateDate>='" + times[0] + "' and CreateDate<='" + times[1] + " 23:59:59'";
        }
        if (num) {
            var s = "%";
            where += where == "" ? "" : " and ";
            where += "(UserName LIKE " + "'%" + num + "%' or Telephone Like '%" + num + "%')";
            where = escape(where);
        }

        if (ispay == "1") {
            where += where == "" ? "" : " and ";
            where += "State='0001'";
        } else if (ispay == "0") {
            where += where == "" ? "" : " and ";
            where += "State='0000'";
        }
        return where;
    }
}


//表格大小随界面大小变化
$(window).resize(function () {
    $('#tbfeesetting').datagrid('resize', {
        width: $(window).width() - 20,
        height: $(window).height() - 55
    }).datagrid('resize', {
        width: $(window).width() - 20,
        height: $(window).height() - 55
    });
});