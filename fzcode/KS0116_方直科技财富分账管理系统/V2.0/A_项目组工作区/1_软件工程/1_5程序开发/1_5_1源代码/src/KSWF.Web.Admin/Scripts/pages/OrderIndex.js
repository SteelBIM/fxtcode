/// <reference path="../../Template/vendor/jquery/jquery.min.js" />
/// <reference path="../CommonMetaData.js" />


//订单数据统计
function orderstatistics() {
    var areacode = '';
    if ($("#selpro").val()) {
        areacode = $("#selpro").val();
    }
    if ($("#selcity").val()) {
        areacode = $("#selcity").val();
    }
    if ($("#selarea").val()) {
        areacode = $("#selarea").val();
    }

    //var deptid = $("#seldepttree").ddTreeGetValue();
    var deptid = $("#selectdeptid").val();
    if (deptid == "0") { deptid = ""; }
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        SearchType: $("#searchtype").val()
        , SearchKey: $("#searchkey").val()
        , startDate: $("#hidstartdate").val()//$('#hidstartdate').val()
        , endDate: addDate($("#hidenddate").val(), 1)//$('#hidenddate').val()
        , AreaCode: areacode
        , SchoolID: $("#selschoolid").val()
        , GradeID: $("#selgradeid").val()
        , ClassID: $("#selclass").val()
        //, PayType: $("#selpayType").val()
        , Qudao: $("#selqudao").val()
        , Dept: deptid
        , MasterName: $("#selmaster").val()
        , Agency: $("#selagency").val()
        , ChannelID: $("#selchannel").val()
         , PayType: $("#PayType").val()
         , Version: $("#Version").val()
    };

    $.post("/Order/GetOrdersTotal", temp, function (data) {
        if (data != null) {
            $("#OrderCount").html(data.ordernumber);
            $("#TotalAmount").html(DataToFixed(data.paycount));
            removecloud();
        }
    });
}


$(function () {
    addcloud();
    LoadTreeDept();
    LoadUser();
    orderstatistics();
    $("#selchannel").change(function () {
        $.post("/RoyaltyPolicyMgr/GetVersion", { productid: $(this).find("option:selected").val(), categoryid: 0 }, function (response) {
            if (response != "") {
                var jsondata = eval(response);
                $("#Version").empty();
                //$("#Version").append("<option value=\"0\">全部</option>");
                $.each(eval(response), function (index, item) {
                    var opt = $("<option >").text(item.version).val(item.versionid);
                    $("#Version").append(opt);
                });
                if (jsondata.length > 0) {
                    GetProportion(jsondata[0].versionid);
                }
            } else {
                $("#Version").empty();
            }
        });
    });
})


//加载版本  
function LoadVersion(productid) {
    $.post("/RoyaltyPolicyMgr/GetVersion", { productid: productid, categoryid: 0 }, function (response) {
        if (response != "") {
            var jsondata = eval(response);
            $("#Version").empty();
            //$("#Version").append("<option value=\"0\">全部</option>");
            $.each(eval(response), function (index, item) {
                var opt = $("<option >").text(item.version).val(item.versionid);
                $("#Version").append(opt);
            });
            if (jsondata.length > 0) {
                GetProportion(jsondata[0].versionid);
            }
        } else {
            $("#Version").empty();
        }
    });
}


//点击事件
function itemOnclick(id) {
    addcloud();
    $("#selectdeptid").val(id);
    $('#tb_orders').bootstrapTable('selectPage', 1);
    LoadUser();
    orderstatistics();

}

//加载树形菜单
function LoadTreeDept() {
    $.ajax({
        type: "Post",
        url: "/AccountInfo/GetDept",
        dataType: "json",
        success: function (result) {
            $('#tree').treeview({
                data: result,
                showIcon: false,
                onNodeExpanded: function (event, node) {
                },
                onNodeSelected: function (event, node) {
                    itemOnclick(node.tag);
                }
            });
        }
    });
}


var OrderPage = function () {
    var Current = this;
    this.DefaultCatogory;
    this.DefaultChannel;
    this.DefaultPayType;
    this.GradeClass;
    this.Init = function () {
        Current.TabInit();
        var totalcount = $(".pagination-info").text();

        //Current.DatePickerInit();
        Current.InitAreaSelect();
        $("#searchbtn").click(function () {
            $('#tb_orders').bootstrapTable('selectPage', 1);
            orderstatistics();
        })

        //清空条件
        $("#BtnclearCondition").click(function () {
            $("#selectdeptid").val(0);
            LoadTreeDept();
            LoadUser();
            $("#searchtype").prop("selectedIndex", 0);
            $("#searchkey").val('');
            //$('#datetimepicker1 .input-group-addon:has(span.glyphicon-remove)').click();
            //$('#datetimepicker2 .input-group-addon:has(span.glyphicon-remove)').click();
            //$('#datetimepicker1').datetimepicker("hide");
            //$('#datetimepicker2').datetimepicker("hide");
            $('#hidstartdate').val("");
            $('#hidenddate').val("");
            $("#divcondition select").prop("selectedIndex", 0);
            $("#selpro").change();
            $('#selschoolid').selectpicker("refresh");
            $('#selagency').selectpicker("refresh");
            $('#selmaster').selectpicker("refresh");

            //$("#seldepttree").ddTreeClearValue();
            $("#searchbtn").click();

        })

        $("#selcolumn").change(function () {
            // Current.TabInit();
            $('#tb_orders').bootstrapTable("refreshOptions", { columns: Current.GetColumn() });
        })

        $("#selchannel").change(function () {
            var k = $(this).val();
            $("#selcategory").html('<option value="">全部</option>');
            for (var i = 0; i < Current.DefaultCatogory.length; i++) {
                var v = Current.DefaultCatogory[i];
                if (v.ParentKey == k) {
                    $(' <option value="' + v.Key + '">' + v.Value + '</option>').appendTo("#selcategory");
                }
            }
        })
        //初始化部门下拉树列表
        Current.InitDeptDropdownTree();
    }



    //导出表格
    this.exportbtn = function () {
        var params = { limit: 0, offset: 0 };
        var obj = Current.GetParams(params);
        var $form = $('<form target="down-file-iframe" method="post" />');
        $form.attr('action', "/Order/ExportOrderXls");
        for (var key in obj) {
            $form.append('<input type="hidden" name="' + key + '" value="' + obj[key] + '" />');
        }
        $(document.body).append($form);
        $form.submit();
        //$.post("/Order/ExportOrderXls", obj);
        $form.remove();
    }

    this.InitAreaSelect = function () {
        $("#selpro").change(function () {
            $("#selcity").html('<option value="">全部</option>');
            $("#selcity").change();
            var v = $(this).val();
            if (v == "") {
                return;
            }
            var obj = { parentid: v };
            $.post("/Order/GetAreaList", obj, function (data) {
                if (data.Success) {
                    if (data.Data) {
                        for (var i = 0; i < data.Data.length; i++) {
                            var result = data.Data[i];
                            var html = '';
                            html += ' <option value="' + result.Seq + '">' + result.CodeName + '</option>';
                            $(html).appendTo("#selcity");
                        }
                    }
                }
            });
        })
        $("#selcity").change(function () {
            $("#selarea").html('<option value="">全部</option>');
            $("#selarea").change();
            var v = $(this).val();
            if (v == "") {
                return;
            }
            var obj = { parentid: v };
            $.post("/Order/GetAreaList", obj, function (data) {
                if (data.Success) {
                    if (data.Data) {
                        for (var i = 0; i < data.Data.length; i++) {
                            var result = data.Data[i];
                            var html = '';
                            html += ' <option value="' + result.Seq + '">' + result.CodeName + '</option>';
                            $(html).appendTo("#selarea");
                        }
                    }
                }
            });
        })
        $("#selarea").change(function () {
            $("#selschoolid").html('<option value="">全部</option>');
            $('#selschoolid').selectpicker("refresh");
            var v = $(this).val();
            if (v == "") {
                return;
            }
            var obj = { areaID: v };
            $.post("/Order/GetSchoolInfo", obj, function (data) {
                if (data.Success) {
                    if (data.Data) {
                        for (var i = 0; i < data.Data.length; i++) {
                            var result = data.Data[i];
                            var html = '';
                            html += ' <option value="' + result.ID + '">' + result.SchoolName + '</option>';
                            $(html).appendTo("#selschoolid");
                        }
                        $('#selschoolid').selectpicker("refresh");
                    }
                }
            });
        })
        $('#selschoolid').change(function () {
            $("#selgradeid").html('<option value="">全部</option>');
            $("#selclass").html('<option value="">全部</option>');
            var v = $(this).val();
            if (v == "") {
                return;
            }
            var obj = { schoolID: v };
            $.post("/Order/GetSchollGradeClass", obj, function (data) {
                if (data.Success) {
                    if (data.Data) {
                        Current.GradeClass = data.Data;
                        for (var i = 0; i < Current.GradeClass.length; i++) {
                            var result = Current.GradeClass[i];
                            var html = '';
                            html += ' <option value="' + result.GradeID + '">' + result.GradeName + '</option>';
                            $(html).appendTo("#selgradeid");
                        }
                    }
                }
            });
        })
        $("#selgradeid").change(function () {
            $("#selclass").html('<option value="">全部</option>');
            var gid = $(this).val();
            if (gid == "") {
                return;
            }
            for (var i = 0; i < Current.GradeClass.length; i++) {

                var result = Current.GradeClass[i];
                if (result.GradeID == gid) {
                    for (var j = 0; j < result.ClassList.length; j++) {
                        var html = '';
                        html += ' <option value="' + result.ClassList[j].ID + '">' + result.ClassList[j].ClassName + '</option>';
                        $(html).appendTo("#selclass");
                    }
                }
            }
        })

        var obj = { parentid: 0 };

        //根据数据查看权限，动态加载部门负责区域或个人区域
        //var obj = { }
        //$.post("/Order/GetAreaId", "", function (data) {
        //    if (data.Success) {
        //        if (data.Data) {
        //            obj = { parentid: data.Data.areaID
        //        };
        //    }
        //}
        //})

        $.post("/Order/GetAreaList", obj, function (data) {
            if (data.Success) {
                if (data.Data) {
                    for (var i = 0; i < data.Data.length; i++) {
                        var result = data.Data[i];
                        var html = '';
                        html += ' <option value="' + result.Seq + '">' + result.CodeName + '</option>';
                        $(html).appendTo("#selpro");

                    }
                }
            }
        })

    }

    this.DatePickerInit = function () {
        var picker1 = $('#datetimepicker1').datetimepicker({
            format: 'yyyy-mm-dd hh:ii:ss',
            language: 'zh-CN',
            weekStart: 1,
            todayBtn: true,
            keyboardNavigation: true,
            pickerPosition: 'bottom-left',
            showMeridian: true,
            autoclose: true,
            todayHighlight: 1,
            startView: 2,
            minView: 0,
            forceParse: 0,
            weekStart: 1,
            linkField: "hidstartdate"

        });
        var picker2 = $('#datetimepicker2').datetimepicker({
            format: 'yyyy-mm-dd hh:ii:ss',
            language: 'zh-CN',
            weekStart: 1,
            todayBtn: true,
            keyboardNavigation: true,
            pickerPosition: 'bottom-left',
            showMeridian: true,
            autoclose: true,
            todayHighlight: 1,
            startView: 2,
            minView: 0,
            forceParse: 0,
            weekStart: 1
            , linkField: "hidenddate"
        });
        //动态设置最小值
        picker1.on('changeDate', function (e) {
            var d = new Date();
            picker2.datetimepicker('setStartDate', e.date);
        });
        //动态设置最大值
        picker2.on('changeDate', function (e) {
            picker1.datetimepicker('setEndDate', e.date);
        });
    }

    this.TabInit = function () {
        $('#tb_orders').bootstrapTable({
            url: '/Order/GetOrderPageList',     //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: Current.GetParams,     //传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            //search: true,                     //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            //strictSearch: true,
            showColumns: false,                  //是否显示所有的列
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            //height: 500,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度 设置之后表头内容无法对齐。
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            //showToggle: true,                 //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: Current.GetColumn()
        });
    }

    this.GetColumn = function () {
        var column1 = [{ field: 'o_id', title: '订单编号' }, {
            field: 'o_datetime', title: '订单日期', formatter: function (value, row, index) { return Current.FormatTime(value, "yyyy-MM-dd hh:mm:ss"); }
        }, {
            field: 'path',
            title: '省',
            formatter: function (value, row, index) {
                if (value != undefined) {
                    var v = $.trim(value).split(' ');
                    return v[0];
                }
            }
        }, {
            field: 'path',
            title: '市',
            formatter: function (value, row, index) {
                if (value != undefined) {
                    var v = $.trim(value).split(' ');
                    if (v[0].indexOf('市')!=-1) {
                        return v[0];
                    } else {
                        if (v.length > 1)
                            return v[1];
                    }
                }
            }
        }, {
            field: 'path',
            title: '区/县',
            formatter: function (value, row, index) {
                if (value != undefined) {
                    var v = $.trim(value).split(' ');
                    if (v[0].indexOf('市') != -1) {
                        if (v.length == 2) {
                            return v[1];
                        }
                    }
                    if (v.length > 2) {
                        return v[2];
                    }
                }
            }
        }, {
            field: 'schoolname',
            title: '学校'
        }, {
            field: 'gradename',
            title: '年级'
        }
            , {
                field: 'classname',
                title: '班级'
            }, {
                field: 'u_teachername',
                title: '老师姓名'
            }, {
                field: 'm_mastertype',
                title: '渠道'
            }, {
                field: 'm_deptname',
                title: '部门'
            }, {
                field: 'm_a_name',
                title: '员工/代理商'
            }, {
                field: 'channel',
                title: '产品名称',
                formatter: function (value, row, index) {
                    var v = Current.GetChannelByKey(value);
                    return v ? v.Value : value;
                }
            } , {
                field: 'p_version',
                title: '版本' 
            }, {
                field: 'o_feetype',
                title: '支付方式',
                formatter: function (value, row, index) {
                    return paytype(value);
                }
            }, {
                field: 'o_payamount',
                title: '支付金额',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_feeamount',
                title: '手续费',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }, {
                field: 'o_actamount',
                title: '实际到账',
                formatter: function (value, row, index) {
                    return DataToFixed(value);
                }
            }
        ];
        return column1;
    }

    this.GetParams = function (params) {
        var areacode = '';
        if ($("#selpro").val()) {
            areacode = $("#selpro").val();
        }
        if ($("#selcity").val()) {
            areacode = $("#selcity").val();
        }
        if ($("#selarea").val()) {
            areacode = $("#selarea").val();
        }
        //var deptid = $("#seldepttree").ddTreeGetValue();
        var deptid = $("#selectdeptid").val();
        if (deptid == "0") { deptid = ""; }
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset  //页码
            , SearchType: $("#searchtype").val()
            , SearchKey: $("#searchkey").val()
            , startDate: $("#hidstartdate").val()//$('#hidstartdate').val()
            , endDate: addDate($("#hidenddate").val(), 1)//$('#hidenddate').val()
            , AreaCode: areacode
            , SchoolID: $("#selschoolid").val()
            , GradeID: $("#selgradeid").val()
            , ClassID: $("#selclass").val()
            //, PayType: $("#selpayType").val()
            , Qudao: $("#selqudao").val()
            , Dept: deptid
            , MasterName: $("#selmaster").val()
            , Agency: $("#selagency").val()
            , ChannelID: $("#selchannel").val()
             , PayType: $("#PayType").val()
              , Version: $("#Version").val()
        };
        return temp;
    }

    this.FormatTime = function (time, format) {
        if (!time) {
            return "";
        }
        if (format == undefined || format == "") {
            format = "yyyy年MM月dd日 hh:mm:ss";
        }
        var date = new Date(parseInt(time.substring(6, time.length - 2)))
        return date.format(format);
    }

    this.InitDeptDropdownTree = function () {
        //$.post("/Order/GetCurrentDeptList", null, function (result) {
        //$("#seldepttree").dropdownTree(result);
        //$('#depttree .divtree').treeview({
        //    data: result,
        //    showIcon: false,
        //    onNodeExpanded: function (event, node) {
        //        var tree = $('#divtree');
        //        if (node.state.expanded) {//展开  
        //            tree.treeview('expandNode', node.nodeId);
        //        } else { //折叠  
        //            tree.treeview('collapseNode', node.nodeId);
        //        }
        //    },
        //    onNodeSelected: function (event, data) {
        //        $("#btndeptddntree").find("span").first().text(data.text);
        //        $("#seldept").val(data.Id);
        //        $("#depttree").hide();
        //    }
        //});
        //$("#btndeptddntree").click(function () {
        //    if ($("#depttree").is(':visible')) {
        //        $("#depttree").hide();
        //    } else {
        //        $("#depttree").show();
        //    }
        //});
        //$(document).click(function (e) {
        //    e = window.event || e; // 兼容IE7
        //    obj = $(e.srcElement || e.target);
        //    if (!$(obj).hasClass("expand-icon") && $(obj).closest("#depttree").length == 0 && !$(obj).is("#depttree,#btndeptddntree") && $(obj).closest("#btndeptddntree").length == 0) {
        //        //要执行的函数b
        //        $("#depttree").hide();
        //    }
        //});
        //})
    }

    ///获取来源信息
    this.GetChannelByKey = function (key) {
        if (Current.DefaultChannel) {
            for (var i = 0; i < Current.DefaultChannel.length; i++) {
                if (Current.DefaultChannel[i].Key == key) {
                    return Current.DefaultChannel[i];
                }
            }
        }
    }
    //获取分类信息
    this.GetCatogoryByKey = function (key) {
        if (Current.DefaultCatogory) {
            for (var i = 0; i < Current.DefaultCatogory.length; i++) {
                if (Current.DefaultCatogory[i].Key == key) {
                    return Current.DefaultCatogory[i];
                }
            }
        }
    }
    //获取支付方式信息
    this.GetPayTypeByKey = function (key) {
        if (Current.DefaultPayType) {
            for (var i = 0; i < Current.DefaultPayType.length; i++) {
                if (Current.DefaultPayType[i].Key == key) {
                    return Current.DefaultPayType[i];
                }
            }
        }
    }
}

Date.prototype.format = function (format) {
    var o =
    {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}


function LoadUser() {
    $.post("/Order/GetEmployee", { deptid: $("#selectdeptid").val() }, function (data) {
        $("#selmaster").empty();
        $("#selmaster").append($("<option>").text("全部").val(""));
        if (data != null && data != "") {
            $.each(data, function (index, item) {
                var opt = $("<option>").text(item.truename).val(item.mastername);
                $("#selmaster").append(opt);
            });
        }
        $('#selmaster').selectpicker("refresh");
    });
    $.post("/Order/GetAgent", { deptid: $("#selectdeptid").val() }, function (data) {
        $("#selagency").empty();
        $("#selagency").append($("<option>").text("全部").val(""));
        if (data != null && data != "") {
            $.each(data, function (index, item) {
                var opt = $("<option>").text(item.agentname).val(item.mastername);
                $("#selagency").append(opt);
            });
        }
        $('#selagency').selectpicker("refresh");
    });
}


function paytype(type) {
    if (type == 0)
        return "微信";
    else if (type == 1)
        return "支付宝";
    else if (type == 2)
        return "苹果";
    return type;
}

function Area() {

}