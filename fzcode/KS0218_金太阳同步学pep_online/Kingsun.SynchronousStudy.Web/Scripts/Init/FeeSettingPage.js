/// <reference path="../../jquery-easyui/jquery.min.js" />
/// <reference path="../../jquery-easyui/jquery.easyui.min.js" />
/// <reference path="../Common.js" />
/// <reference path="../jquery.json-2.4.js" />
/// <reference path="../easyuiCommon.js" />
/// <reference path="../Management/FeeSettingManagement.js" />


var FeeSettingPage = function () {
    var Current = this;
    this.Init = function () {
        //初始化添加/修改应用弹窗
        $("#adddiv").dialog({
            title: "新增套餐"
            , width: 600
            , height: 500
             , modal: true
            , closed: true
            , buttons: [{
                text: '保存',
                iconCls: 'icon-save',
                handler: Current.Save,
            }, {
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#adddiv").dialog("close");
                }
            }]
        });
        //初始化表格
        $("#tbfeesetting").datagrid({
            url: "?action=querylist",
            toolbar: '#tb',
            width: $(window).width() - 20,
            height: $(window).height() - 55,
            nowrap: false,
            striped: true,
            autoRowHeight: false,
            singleSelect: true,
            fitColumns: true,
            pagination: true, rownumbers: true,
            pageSize: 20,
            columns: [[
                { title: '套餐名', field: 'FeeName', halign: 'center', width: 90, align: 'center' },

                 { title: '时间（月）', field: 'Month', align: 'center', width: 60 }
                , { title: '价格（元）', field: 'FeePrice', align: 'center', width: 60 }
                 , {
                     title: '折扣（元）', field: 'Discount ', align: 'center', width: 60, formatter: function (value, row, index) {
                         return row.Discount;
                     }
                 }
                , {
                    title: '套餐状态', field: 'State', width: 80, align: 'center', formatter: function (value, row, index) {
                        var html = '';
                        if (value == "1") {
                            html = "<span style=\"color:red;\">使用中</span>";
                        }
                        else {
                            html = "<span style=\"color:#999;\">已下架</span>";
                        }
                        return html;
                    }
                }
                , { title: '创建人', field: 'CreateUser', align: 'center', width: 80 }
                , {
                    title: '创建时间', field: 'CreateDate', align: 'center', width: 100, formatter: function (value, row, index) {
                        date = new Date(parseInt(value.substring(6, value.length - 2)))
                        return date.format("yyyy.MM.dd hh:mm");
                    }
                }
                , { title: '最后修改人', field: 'ModifyUser', align: 'center', width: 80 }
                , {
                    title: '最后修改时间', field: 'ModifyDate', align: 'center', width: 100, formatter: function (value, row, index) {
                        if (value) {
                            date = new Date(parseInt(value.substring(6, value.length - 2)))
                            return date.format("yyyy.MM.dd hh:mm");
                        }
                    }
                }
                 , {
                     title: '操作', field: 'ID', align: 'center', width: 150, align: 'center', formatter: function (value, row, index) {
                         var html = '';
                         html = "<a fid=\"" + row.ID + "\" href=\"javascript:void(0)\" onclick=\"feepage.ModifyFeeCombo(this)\">修改</a>";
                         if (row.State) {
                             html += "<a style='margin-left:20px' fid=\"" + row.ID + "\" href=\"javascript:void(0)\" onclick=\"feepage.JFeeCombo(this)\">禁用</a>";
                         } else {
                             html += "<a style='margin-left:20px' fid=\"" + row.ID + "\" href=\"javascript:void(0)\" onclick=\"feepage.JFeeCombo(this)\">启用</a>";
                         }


                         return html;
                     }
                 }
            ]]
        });

        $("#addFee").click(function () {
            $("#adddiv").dialog("open");
            id = "";

        });


    }
    $("#combotype").combobox({
        valueField: 'ComboID'
                , textField: 'ComboName'
                , width: 120
                , panelHeight: 120
                , data: [{ ComboID: 0, ComboName: "付费套餐" }, { ComboID: 1, ComboName: "活动套餐" }]
                , onSelect: function (row) {




                }
    })



    $("#select1").combobox({
        valueField: 'ComboID'
                    , textField: 'ComboName'
                    , width: 120
                    , panelHeight: 120
                    , data: [{ ComboID: 1, ComboName: "安卓" }, { ComboID: 2, ComboName: "苹果" }, { ComboID: 3, ComboName: "全部" }]
                    , onSelect: function (row) {

                    }
    })

    Feemanager.QueryAppID(function (data) {
        if (data.Success) {
            data = data.Data;
            $("#selecttype").combobox({
                valueField: 'ID'
                   , textField: 'VersionName'
                   , width: 120
                   , panelHeight: 120
                   , data: data
                   , onSelect: function (row) {
                       var appid = $("#selecttype").combobox("getValue");
                       $("#appid").val(appid);

                       for (var i = 0; i < data.length; i++) {
                           if (data[i].ID == appid) {
                               $("#edid").val(data[i].VersionID);
                           }
                       }

                   }
            })
        }
    });


    this.Save = function () {
        if (id) {
            var info = {};
            info.ID = id;
            info.AppID = $("#selecttype").combobox("getValue");
            info.VersionID = $("#edid").val();
            info.Type = $("#select1").combobox("getValue");
            info.FeePrice = $("#daprice").val();
            info.Discount = $("#discount ").val();
            info.Month = $("#dafeetime ").val();
            info.AppleID = $("#appName ").val();
            info.FeeName = $("#selecttype").combobox("getText");
            info.ComboType = $("#combotype").combobox("getValue");
            info.ImageUrl = $("#acimg ").val();
            info.FeeName = $("#FeeName").val();
            Feemanager.ModifyFeeCombo(info, function (data) {
                if (data.Success) {
                    $("#adddiv").dialog("close");
                    easyuiMessage.ShowTopMid("修改成功", "提示", 2000);
                }
            });

        } else {
            var info = {};
            info.AppID = $("#selecttype").combobox("getValue");
            info.VersionID = $("#edid").val();
            info.Type = $("#select1").combobox("getValue");
            info.FeePrice = $("#daprice").val();
            info.Discount = $("#discount").val();
            info.Month = $("#dafeetime").val();
            info.AppleID = $("#appName").val();
            info.FeeName = $("#selecttype").combobox("getText");
            info.ComboType = $("#combotype").combobox("getValue");
            info.ImageUrl = $("#acimg").val();
            info.FeeName = $("#FeeName").val();
            Feemanager.AddCombo(info, function (data) {
                if (data.Success) {
                    $("#adddiv").dialog("close");
                    easyuiMessage.ShowTopMid("添加成功", "提示", 2000);
                }
            });
        }
    }
    var id = "";

    this.ModifyFeeCombo = function (self) {
        $("#adddiv").dialog("open");
        id = $(self).attr("fid");
        Feemanager.QueryFee({ ID: id }, function (data) {
            if (data.Success) {
                var fee = data.Data;
                $("#selecttype").combobox("select", fee.AppID);
                $("#select1").combobox("select", fee.Type);
                $("#appName").val(fee.AppleID);
                $("#dafeetime").val(fee.Month);
                $("#daprice").val(fee.FeePrice);
                $("#discount").val(fee.Discount);
                $("#combotype").combobox("select", fee.ComboType);
                $("#acimg").val(fee.ImageUrl);
                $("#FeeName").val(fee.FeeName);
            }
        });

    }

    this.JFeeCombo = function (self) {
        id = $(self).attr("fid");
        Feemanager.JFeeCombo({ ID: id }, function (data) {
            if (data.Success) {
                $("#tbfeesetting").datagrid("reload");
                if (data.Data == "1") {
                    alert("启用成功");
                } else {
                    alert("禁用成功");
                }
            }
        });
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