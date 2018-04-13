///////////预约用户统计//////////// 
var CourseListInit = function () { 
    var Current = this;
    this.Init = function () {
        Current.InitCourseList();
    };
    //初始化列表
    this.InitCourseList = function () {
        $('#tbdatagrid').datagrid({
            url: "?action=Query",
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 30, 40, 50, 40, 50],
            width: $(window).width() - 20,
            height: $(window).height() - 95,
            columns: [[
                    {
                        field: 'UserID', title: '用户ID', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.UserID + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'UserName', title: '用户名', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.UserName + '</a>';
                             return html;
                         }
                     },
                    {
                        field: 'TrueName', title: '真实姓名', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.TrueName + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'TelePhone', title: '联系方式', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.TelePhone + '</a>';
                             return html;
                         }
                     },
                     {
                         field: 'CourseID', title: '预约课程ID', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.CourseID + '</a>';
                             return html;
                         }
                     },
                      {
                          field: 'CourseName', title: '预约课程名称', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.CourseName + '</a>';
                              return html;
                          }
                      },
                       {
                           field: 'CoursePeriodID', title: '预约课时ID', width: 150, align: 'center', formatter: function (value, row) {
                               var html = '';
                               html += ' <span>' + row.CoursePeriodID + '</a>';
                               return html;
                           }
                       },
                        {
                            field: 'CoursePeriodName', title: '预约课时名称', width: 150, align: 'center', formatter: function (value, row) {
                                var html = '';
                                html += ' <span>' + row.CoursePeriodName + '</a>';
                                return html;
                            }
                        },
                       {
                           field: 'CreateTime', title: '预约时间', width: 150, align: 'center', formatter: function (value, row) {
                               var html = '';
                               //var CreateTime = new Date(eval('new ' + (row.CreateTime.replace(/\//g, '')))).format('yyyyMMdd hh:mm:ss');
                               html += ' <span>' + row.CreateTime + '</a>';
                               return html;
                           }
                       },
                       {
                           field: 'StartTime', title: '上课时间', width: 300, align: 'center', formatter: function (value, row) {
                               var html = '';
                               //var StartTime = new Date(eval('new ' + (row.StartTime.replace(/\//g, '')))).format('yyyyMMdd hh:mm:ss') + "-" + new Date(eval('new ' + (row.EndTime.replace(/\//g, '')))).format('hh:mm:ss');
                               //var StartTime = new Date(eval('new ' + (row.StartTime.replace(/\//g, '')))).format('yyyyMMdd hh:mm:ss');
                               html += ' <span>' + row.StartTime + '</a>';
                               return html;
                           }
                       },
                      {
                          field: 'NewPrice', title: '预约价格', width: 80, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.NewPrice + '</a>';
                              return html;
                          }
                      }
            ]]
        });
    }
    //通过关键字搜索
    //$("#searchUserInfoValue").focus(function () {
    //    var searchValue = $("#searchUserInfoValue").val();
    //    if (searchValue == "请输入联系方式") {
    //        $("#searchUserInfoValue").val("");
    //    }
    //})
    //$("#searchUserInfoValue").blur(function () {
    //    var searchValue = $("#searchUserInfoValue").val();
    //    if (searchValue == "") {
    //        $("#searchUserInfoValue").val("请输入联系方式");
    //    }
    //})
    $("#search").click(function () {
        var selType = $("#selType").val();
        var searchValue = $("#searchValue").val();
        var txtSearchStartTime = $("#txtSearchStartTime").val();
        var txtSearchEndTime = $("#txtSearchEndTime").val();
        var queryStr = "";
        if (selType == 0) {//联系方式
            if (searchValue == "") {
                queryStr += " and 1=1";
            } else {
                queryStr += " and  b.TelePhone  like '%" + searchValue + "%'";
            }
        } else if (selType == 1) {//课程名称
            if (searchValue == "") {
                queryStr += " and 1=1";
            } else {
                queryStr += " and  a.AppointCourseName like '%" + searchValue + "%'";
            }
        } else {    //课时名称
            if (searchValue == "") {
                queryStr += " and 1=1";
            } else {
                queryStr += " and  a.AppointCoursePeriodName like '%" + searchValue + "%'";
            }
        }
        if (txtSearchStartTime != "" && txtSearchEndTime != "") {
            if (txtSearchStartTime > txtSearchEndTime) {
                alert('结束时间必须大于开始时间');
                return false;
            }
            queryStr += "and StartTime between '" + txtSearchStartTime + "' and '" + txtSearchEndTime + "' ";
        } else {
            if (txtSearchStartTime != "") {
                queryStr += "and StartTime>='" + txtSearchStartTime + "'";
            }
            if (txtSearchEndTime != "") {
                queryStr += "and StartTime<='" + txtSearchEndTime + "'";
            }
        }
        $('#tbdatagrid').datagrid({
            url: '?action=Query&queryStr=' + encodeURI(queryStr)
        });
    })
    //导出
    $("#exportExcel").click(function () {
        var selType = $("#selType").val();
        var searchValue = $("#searchValue").val();
        var txtSearchStartTime = $("#txtSearchStartTime").val();
        var txtSearchEndTime = $("#txtSearchEndTime").val();
        var queryStr = "";
        if (selType == 0) {//联系方式
            if (searchValue == "") {
                queryStr += " and 1=1";
            } else {
                queryStr += " and  b.TelePhone  like '%" + searchValue + "%'";
            }
        } else if (selType == 1) {//课程名称
            if (searchValue == "") {
                queryStr += " and 1=1";
            } else {
                queryStr += " and  a.AppointCourseName like '%" + searchValue + "%'";
            }
        } else {    //课时名称
            if (searchValue == "") {
                queryStr += " and 1=1";
            } else {
                queryStr += " and  a.AppointCoursePeriodName like '%" + searchValue + "%'";
            }
        }
        if (txtSearchStartTime != "" && txtSearchEndTime != "") {
            if (txtSearchStartTime > txtSearchEndTime) {
                alert('结束时间必须大于开始时间');
                return false;
            }
            queryStr += "and StartTime between '" + txtSearchStartTime + "' and '" + txtSearchEndTime + "' ";
        } else {
            if (txtSearchStartTime != "") {
                queryStr += "and StartTime>='" + txtSearchStartTime + "'";
            }
            if (txtSearchEndTime != "") {
                queryStr += "and StartTime<='" + txtSearchEndTime + "'";
            }
        }
        window.open("?action=excel&queryStr=" + encodeURI(queryStr));
    });
    //添加预约用户
    $("#addUserAppoint").click(function () {
        $("#divShowUserInfo").css("display", "block");
        $("#divShowUserInfo").dialog({
            title: '预约课时',
            width: 750,
            height: 500,
            closed: false,
            cache: false,
            modal: true,
            resizable: true,
            fitColumns: true,
            buttons: [
                {
                    text: '添加课时',
                    handler: function () {
                        selectUserInfo();
                    }
                }, {
                    text: '关闭',
                    handler: function () {
                        $("#divShowUserInfo").dialog('close');
                    }
                }
            ]
        });
    });
    //联系方式搜索
    $("#searchUserInfoShow").click(function () {
        var queryStr = '';
        var searchValue = $("#searchUserInfoValue").val();
        if (searchValue == "") {
            queryStr += " 1=1";
        } else {
            queryStr += " TelePhone like '%" + searchValue + "%'";
        }
        $('#tbdatagridUserInfo').datagrid({
            url: '?action=getUserInfoByTelePhone&queryStr=' + encodeURI(queryStr),
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 30, 40],
            width: $("#divShowUserInfo").width(),
            height: $("#divShowUserInfo").height() - 150,
            columns: [[
                    {
                        field: 'UserID', title: '用户ID', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.UserID + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'UserName', title: '用户名', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.UserName + '</a>';
                             return html;
                         }
                     },
                    {
                        field: 'TrueName', title: '真实姓名', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.TrueName + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'TelePhone', title: '联系方式', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.TelePhone + '</a>';
                             return html;
                         }
                     }
            ]]
        });
    });
    //课时搜索
    $("#searchCoursePeriodShow").click(function () {
        var queryStr = '';
        var CoursePeriodNameValue = $("#CoursePeriodNameValue").val();
        var GroupValue = $("#GroupValue").val();
        var txtSelectStartTime = $("#txtSelectStartTime").val();
        var txtSelectEndTime = $("#txtSelectEndTime").val();
        if (CoursePeriodNameValue == "") {
            queryStr += " 1=1";
        } else {
            queryStr += " b.Name like '%" + CoursePeriodNameValue + "%'";
        }
        if (GroupValue == "") {
            queryStr += " and 1=1";
        } else {
            queryStr += " and a.Groups like '%" + GroupValue + "%'";
        }
        if (txtSelectStartTime != "" && txtSelectEndTime != "") {
            if (txtSelectStartTime > txtSelectEndTime) {
                alert('结束时间必须大于开始时间');
                return false;
            }
            queryStr += "and StartTime between '" + txtSelectStartTime + "' and '" + txtSelectEndTime + "' ";
        } else {
            if (txtSelectStartTime != "") {
                queryStr += "and StartTime>='" + txtSelectStartTime + "'";
            }
            if (txtSelectEndTime != "") {
                queryStr += "and StartTime<='" + txtSelectEndTime + "'";
            }
        }
        $('#tbdatagridCoursePeriod').datagrid({
            url: '?action=getCoursePeriodTimeSearch&queryStr=' + encodeURI(queryStr),
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: false,
            pagesize: 10,
            pageList: [5, 10, 20, 30, 40],
            width: $("#divShowCoursePeriod").width(),
            height: $("#divShowCoursePeriod").height() - 300,
            columns: [[
                    { field: 'ck', checkbox: true },
                    {
                        field: 'CoursePeriodName', title: '课时名称', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.CoursePeriodName + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'CourseName', title: '所属课程', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.CourseName + '</a>';
                             return html;
                         }
                     },
                    {
                        field: 'StartTime', title: '上课开始时间', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            var StartTime = new Date(eval('new ' + (row.StartTime.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
                            html += ' <span>' + StartTime + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'Groups', title: '级别', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.Groups + '</a>';
                             return html;
                         }
                     }
            ]]
        });
    });
    //继续添加课时
    $("#btnGoOn").click(function () {
        $("#tabCoursePeriod").css("display", "block");
        $("#divCoursePeriod").css("display", "block");
        $("#divCoursePeriodAppoint").css("display", "none");
        $("#divShowCoursePeriod .dialog-button").css("display", "block");
        $("#divShowCoursePeriod .dialog-button a:eq(0)").css("display", "inline-block");
    });
    //确认添加预约
    $("#btnOK").click(function () {
        var CoursePeriodTimeID = new Array();
        CoursePeriodTimeID = $("#hiddenAppointCoursePeriod").val();
        //alert(CoursePeriodTimeID);
        //var obj = {
        //    UserName: $("#spanUserName").text(), UserID: $("#spanUserID").text(), TrueName: $("#spanTrueName").text(), TelePhone: $("#spanTelePhone").text(),
        //    CoursePeriodTimeID: CoursePeriodTimeID
        //};
        var total = $('#tbdatagridCoursePeriodAppoint').datagrid("getData").total;
        if (total == 0) {
            alert('请选择该用户要预约的课时！');
            return;
        }
        $.post("?action=handadduserappointinfo", { UserName: $("#spanUserName").text(), UserID: $("#spanUserID").text(), TrueName: $("#spanTrueName").text(), TelePhone: $("#spanTelePhone").text(), CoursePeriodTimeID: CoursePeriodTimeID }, function (data) {
            //alert(data);
            data = eval("(" + data + ")");
            if (data.state == 1) { 
                $.messager.alert("温馨提示", data.msg, "info", function () {
                    window.location.reload();
                });
            } else {
                alert(data.msg);
            }
        });
    });
}
//选择用户
function selectUserInfo() {
    $("#hiddenAppointCoursePeriod").val(0);
    var tabUserInfoState = $("#tbdatagridUserInfo").css("display");
    if (tabUserInfoState=="block") {
        alert('请选择用户');
        return;
    }
    var row = $('#tbdatagridUserInfo').datagrid('getSelected');
    if (row) {
        //alert(row.UserID);
        //alert(row.UserName);
        //alert(row.TrueName);
        //alert(row.TelePhone);
        $("#spanUserName").text(row.UserName);
        $("#spanUserID").text(row.UserID);
        $("#spanTrueName").text(row.TrueName);
        $("#spanTelePhone").text(row.TelePhone);

        $("#tabCoursePeriod").css("display", "block");
        $("#divCoursePeriod").css("display", "block");
        $("#divCoursePeriodAppoint").css("display", "none");

        $("#divShowCoursePeriod").css("display", "block");
        $("#divShowCoursePeriod").dialog({
            title: '预约课时',
            width: 750,
            height: 700,
            closed: false,
            cache: false,
            modal: true,
            resizable: true,
            fitColumns: true,
            buttons: [
                {
                    text: '保存',
                    handler: function () {
                        selectCoursePeriod();
                    }
                }, {
                    text: '关闭',
                    handler: function () {
                        $("#divShowCoursePeriod").dialog('close');
                        $("#hiddenAppointCoursePeriod").val(0);
                    }
                }
            ]
        });
    } else {
        alert('请选择用户');
    }
}
//选择课时
function selectCoursePeriod() {
    var tabUserInfoState = $("#tbdatagridCoursePeriod").css("display");
    if (tabUserInfoState == "block") {
        alert('请选择课时！');
        return;
    }
    var checkedItems = $('#tbdatagridCoursePeriod').datagrid('getChecked');
    if (checkedItems) {
        var CoursePeriodTimeID = [];
        var hiddenArrayResult = new Array();
        hiddenArrayResult = $("#hiddenAppointCoursePeriod").val().split(",");
        $.each(checkedItems, function (index, item) {
            if (hiddenArrayResult.contains(item.CoursePeriodTimeID) == false) {
                hiddenArrayResult.push(item.CoursePeriodTimeID);
            }
        });
        $("#hiddenAppointCoursePeriod").val(hiddenArrayResult);
        //alert($("#hiddenAppointCoursePeriod").val());
        //显示选中的课时
        $("#tabCoursePeriod").css("display", "none");
        $("#divCoursePeriod").css("display", "none");
        $("#divCoursePeriodAppoint").css("display", "block");
        showSelectCoursePeriod();
        $('#tbdatagridCoursePeriod').datagrid('clearSelections');
        $("#divShowCoursePeriod .dialog-button a:eq(0)").css("display", "none");
    } else {
        alert('请选择课时！');
    }
}
//显示选中的课时
function showSelectCoursePeriod() {
    var queryStr = '';
    var CoursePeriodTimeID = $("#hiddenAppointCoursePeriod").val().split(",");
    queryStr += "c.ID in(" + CoursePeriodTimeID + ")";
    $('#tbdatagridCoursePeriodAppoint').datagrid({
        url: '?action=getCoursePeriodTimeSearch&queryStr=' + encodeURI(queryStr),
        pagination: true,
        rownumbers: true,
        fitColumns: true,
        striped: true,
        singleSelect: true, 
        pagesize: 10,
        pageList: [5, 10, 20, 30, 40],
        width: $("#divShowCoursePeriod").width(),
        height: $("#divShowCoursePeriod").height() - 300,
        columns: [[
                //{ field: 'ck', checkbox: true },
                {
                    field: 'CoursePeriodName', title: '课时名称', width: 150, align: 'center', formatter: function (value, row) {
                        var html = '';
                        html += ' <span>' + row.CoursePeriodName + '</a>';
                        return html;
                    }
                },
                 {
                     field: 'CourseName', title: '所属课程', width: 150, align: 'center', formatter: function (value, row) {
                         var html = '';
                         html += ' <span>' + row.CourseName + '</a>';
                         return html;
                     }
                 },
                {
                    field: 'StartTime', title: '上课开始时间', width: 150, align: 'center', formatter: function (value, row) {
                        var html = '';
                        var StartTime = new Date(eval('new ' + (row.StartTime.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
                        html += ' <span>' + StartTime + '</a>';
                        return html;
                    }
                },
                 {
                     field: 'Groups', title: '级别', width: 150, align: 'center', formatter: function (value, row) {
                         var html = '';
                         html += ' <span>' + row.Groups + '</a>';
                         return html;
                     }
                 },
                   {
                       field: "Operate", title: "操作", width: 120, align: 'center', formatter: function (value, row) {
                           var html = '';
                           html += '<a href="javascript:void(0)" onclick="DelCoursePeriod(' + row.CoursePeriodTimeID + ')" >删除</a>   ';
                           return html;
                       }
                   }
        ]]
    });
}
//删除选中的课时
function DelCoursePeriod(CoursePeriodTimeID) {
    if (confirm("确定要删除吗？")) {
        var hiddenArrayResult = new Array();
        hiddenArrayResult = $("#hiddenAppointCoursePeriod").val();
        var result = hiddenArrayResult.replace(CoursePeriodTimeID, 0);
        $("#hiddenAppointCoursePeriod").val(result);
        showSelectCoursePeriod();
    }
}
var courseListInit;
$(function () {
    courseListInit = new CourseListInit();
    courseListInit.Init();
    //Array数组扩展方法
    Array.prototype.contains = function (needle) {
        for (i in this) {
            if (this[i] == needle) return true;
        }
        return false;
    }
    Array.prototype.remove = function (val) {
        var index = this.indexOf(val);
        if (index > -1) {
            this.splice(index, 1);
        }
    };
});
