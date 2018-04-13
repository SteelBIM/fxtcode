/// <reference path="../../jquery-easyui/jquery.min.js" />
/// <reference path="../../jquery-easyui/jquery.easyui.min.js" />
/// <reference path="../Common.js" />
/// <reference path="../jquery.json-2.4.js" />
/// <reference path="../easyuiCommon.js" />
/// <reference path="../Management/ModuleConfigManagement.js" />


var ModuleConfigListPage = function () {
    var Current = this;
    var bookid = getQueryString("bookid");
    var bookname = getQueryString("bookname");
    var queryStr;
    if (bookid != null)
        queryStr = " BookID='" + bookid + "' and State != '1'";
    else
        queryStr = " BookID=149 and State != '1'";
    this.Init = function () {

        //初始化添加/修改应用弹窗
        $("#adddiv").dialog({
            title: "修改目录"
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
        $("#tbConfigList").datagrid({
            url: "?action=querylist&queryStr=" + queryStr,
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
                { title: '课程名', field: 'TeachingNaterialName', halign: 'center', width: 90, align: 'center' },

                 { title: '一级目录', field: 'FirstTitle', align: 'center', width: 60 }
                , {
                    title: '二级目录', field: 'SecondTitle', align: 'center', width: 60, formatter: function (value, row, index) {
                        return row.SecondTitle;
                    }
                }
                 , {
                     title: '操作', field: 'ID', align: 'center', width: 150, align: 'center', formatter: function (value, row, index) {
                         var html = '';
                         html = "<a sid=\"" + row.ID + "\" href=\"javascript:void(0)\" onclick=\"Modulepage.ModifyModule(this)\">修改</a>";
                         html += "<a style='margin-left:20px' fid=\"" + row.ID + "\" href=\"javascript:void(0)\" onclick=\"Modulepage.DeleteModule(this)\">删除</a>";
                         return html;
                     }
                 }
            ]]
        });

    }
    //$("#tb").html = "<a  href=\"javascript:void(0)\" onclick=\"Modulepage.UpdataCatalog(this) \ class=\"easyui-linkbutton\" data-options=\"iconCls:\'icon-add\'\">更新目录</a>";
    
    var id = "";

    this.DeleteModule = function (self) {
        id = $(self).attr("fid");
        var obj = { ID: id };
        ModuleConfig.DeleteModule({ ID: id }, function (data) {
            if (data.Success) {
                alert("删除成功");
                $('#tbConfigList').datagrid("reload");
            }
            else {
                alert("删除失败");
            }
        });
        //$.post("?action=DeleteModule", obj, function (data) {
        //    if (data) {
        //        var result = eval("(" + data + ")");
        //        if (result) {
        //            alert("更新成功!");
        //        } else {
        //            alert("更新失败!");
        //        }
        //    }
        //});

    }

    this.UpdataCatalog = function (bookid, bookname) {
        var obj = { BookID: bookid, BookName: bookname };
        $.post("?action=UpdataCatalog", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result) {
                    alert("更新成功!");
                    $('#tbConfigList').datagrid("reload");
                } else {
                    alert("更新失败!");
                }
            }
        });
    }

    this.Save = function () {
        if (id) {
            var info = {};
            info.ID = id;
            info.FirstTitle = $("#FirstTitle").val();
            info.SecondTitle = $("#SecondTitle").val();

            ModuleConfig.ModifyModule(info, function (data) {
                if (data.Success) {
                    $("#adddiv").dialog("close");
                    easyuiMessage.ShowTopMid("修改成功", "提示", 2000);
                    $('#tbConfigList').datagrid("reload");
                }
            });

        }
    }

    this.ModifyModule = function (self) {
        $("#adddiv").dialog("open");
        id = $(self).attr("sid");
        ModuleConfig.QueryModule({ ID: id }, function (data) {
            if (data.Success) {
                var mm = data.Data;
                $("#FirstTitle").val(mm.FirstTitle);
                $("#SecondTitle").val(mm.SecondTitle);
            }
        });
    }

}

//表格大小随界面大小变化
$(window).resize(function () {
    $('#tbConfigList').datagrid('resize', {
        width: $(window).width() - 20,
        height: $(window).height() - 55
    }).datagrid('resize', {
        width: $(window).width() - 20,
        height: $(window).height() - 55
    });
});