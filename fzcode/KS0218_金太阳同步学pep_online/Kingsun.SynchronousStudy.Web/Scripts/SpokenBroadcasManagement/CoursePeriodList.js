///////////课时管理//////////// 

var CoursePeriodListInit = function () {
    var Current = this;
    this.Init = function () {
        var CourseID = GetQueryString("CourseID");
        Current.InitCoursePeriodList(CourseID);
    };
    //初始化列表
    this.InitCoursePeriodList = function (CourseID) {
        var queryStr = "a.CourseID =" + CourseID;
        $('#tbdatagrid').datagrid({
            url: "?action=Query&queryStr=" + encodeURI(queryStr),
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 30, 40, 50, 40, 50, 40, 10],
            width: $(window).width() - 30,
            height: $(window).height() - 95,
            columns: [[
                 {
                     field: 'CoursePeriodTimeID', title: '课时时间ID', width: 150, align: 'center', formatter: function (value, row) {
                         var html = '';
                         html += ' <span>' + row.CoursePeriodTimeID + '</a>';
                         return html;
                     }
                 },
                    {
                        field: 'CoursePeriodID', title: '课时ID', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.CoursePeriodID + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'Name', title: '课时名称', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.Name + '</a>';
                             return html;
                         }
                     },
                    {
                        field: 'Price', title: '课时原价', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.Price + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'NewPrice', title: '课时现价', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.NewPrice + '</a>';
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
                         field: 'EndTime', title: '上课结束时间', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             var EndTime = new Date(eval('new ' + (row.EndTime.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
                             html += ' <span>' + EndTime + '</a>';
                             return html;
                         }
                     },
                      {
                          field: "BigImage", title: "课时海报", width: 120, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html = '<a href="javascript:void(0)" onclick="UpdateImg(' + row.CoursePeriodID + ',0)">查看</a>   ';
                              html += '<a href="javascript:void(0)" onclick="UpdateImg(' + row.CoursePeriodID + ',1)">修改</a>   ';
                              return html;
                          }
                      },
                      {
                          field: 'Status', title: '课程状态', width: 80, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + (row.Status == 1 ? "启用" : "禁用") + '</a>';
                              return html;
                          }
                      },
                    {
                        field: "Operate", title: "操作", width: 200, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += '<a href="javascript:void(0)" onclick="AddCoursePeriodTime(' + row.CoursePeriodID + ')" >添加课时时间</a>   ';
                            html += '<a href="javascript:void(0)" onclick="UpdateCoursePeriod(' + row.CoursePeriodTimeID + ',' + row.CoursePeriodID + ')" >修改</a>   ';
                            if (row.Status == 1) {
                                html += '<a href="javascript:void(0)" onclick="UpdateStatus(' + row.CoursePeriodID + ',0)" >禁用</a>   ';
                            } else {
                                html += '<a href="javascript:void(0)" onclick="UpdateStatus(' + row.CoursePeriodID + ',1)" >启用</a>   ';
                            }
                            html += '<a href="javascript:void(0)" onclick="DelCoursePeriodTime(' + row.CoursePeriodTimeID + ',' + row.CoursePeriodID + ')" >删除</a>   ';
                            return html;
                        }
                    }
            ]],
            onDblClickRow: function (rowIndex) {
                $('#tbdatagrid').datagrid('selectRow', rowIndex);
                var currentRow = $("#tbdatagrid").datagrid("getSelected");
                //alert(currentRow.CoursePeriodTimeID);
                UpdateCoursePeriod(currentRow.CoursePeriodTimeID,currentRow.CoursePeriodID);
            }
        });
    }
    //通过关键字搜索
    $("#searchValue").focus(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "请输入课时名称") {
            $("#searchValue").val("");
        }
    })
    $("#searchValue").blur(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "") {
            $("#searchValue").val("请输入课时名称");
        }
    })
    $("#search").click(function () {
        var CourseID = GetQueryString("CourseID");
        var searchValue = $("#searchValue").val();
        var txtSearchStartTime = $("#txtSearchStartTime").val();
        var txtSearchEndTime = $("#txtSearchEndTime").val();
        var queryStr = "";
        if (searchValue == "请输入课时名称" && txtSearchStartTime == "" && txtSearchEndTime == "") {
            queryStr = "a.CourseID =" + CourseID;
            $('#tbdatagrid').datagrid({
                url: "?action=Query&queryStr=" + encodeURI(queryStr),
            });
            return false;
        }
        if (searchValue != "请输入课时名称") {
            queryStr += "a.CourseID=" + CourseID + " and a.Name like '%" + searchValue + "%'";
        } else {
            queryStr += " a.CourseID=" + CourseID;
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
}
var coursePeriodListInit;
$(function () {
    coursePeriodListInit = new CoursePeriodListInit();
    coursePeriodListInit.Init();
});
//删除课时
function DelCoursePeriodTime(CoursePeriodTimeID, CoursePeriodID) {
    //alert(CoursePeriodTimeID);
    if (confirm("您确定要删除吗？")) {
        $.post("?action=DelCoursePeriodTime", { CoursePeriodTimeID: CoursePeriodTimeID, CoursePeriodID: CoursePeriodID }, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.state == 1) {
                    alert("删除成功!");
                    $('#tbdatagrid').datagrid("reload");
                } else {
                    alert(result.msg);
                }
            }
        });
    }
}
//修改课时信息
function UpdateCoursePeriod(CoursePeriodTimeID, CoursePeriodID) {
    $.post("?action=GetCoursePeriod", { CoursePeriodID: CoursePeriodID }, function (data) {
        if (data) {
            var resultCoursePeriod = eval("(" + data + ")");
            $("#txtCoursePeriodID").text(resultCoursePeriod[0].ID);
            $("#txtName").val(resultCoursePeriod[0].Name);
            $("#txtPrice").val(resultCoursePeriod[0].Price);
            $("#txtNewPrice").val(resultCoursePeriod[0].NewPrice);
            $("#txtAheadMinutes").val(resultCoursePeriod[0].AheadMinutes);
            $("#txtLimitNum").val(resultCoursePeriod[0].LimitNum);
            $("#txtSummary").val(resultCoursePeriod[0].Summary);
            //加载课时时间信息
            $.post("?action=GetCoursePeriodTime", { CoursePeriodTimeID: CoursePeriodTimeID }, function (data) {
                if (data) {
                    var result = eval("(" + data + ")");
                    $("#txtCoursePeriodTimeID").text(result[0].ID);
                    $("#txtTimeLimitNum").val(result[0].LimitNum);
                    var StartTime = new Date(eval('new ' + (result[0].StartTime.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
                    var EndTime = new Date(eval('new ' + (result[0].EndTime.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
                    $("#txtStartTime").val(StartTime);
                    $("#txtEndTime").val(EndTime);
                    $("#txtTeacherType").val(result[0].TeacherType);
                }
            });
            $("#divUpdateCoursPeriod").css("display", "block");
            $("#divUpdateCoursPeriod").dialog({
                title: '修改课时',
                width: 750,
                height: 500,
                closed: false,
                cache: false,
                modal: true,
                buttons: [
                    {
                        text: '保存',
                        handler: function () {
                            UpdateCoursePeriodSave(CoursePeriodTimeID, CoursePeriodID);
                        }
                    }, {
                        text: '关闭',
                        handler: function (EditionID) {
                            $("#divUpdateCoursPeriod").dialog('close');
                        }
                    }
                ]
            });
        }
    });
}

//修改课时信息
function UpdateCoursePeriodSave(CoursePeriodTimeID, CoursePeriodID) {
    var txtName = $("#txtName").val();
    var txtPrice = $("#txtPrice").val();
    var txtNewPrice = $("#txtNewPrice").val();
    //var txtAheadMinutes = $("#txtAheadMinutes").val();
    //var txtLimitNum = $("#txtLimitNum").val();
    var txtSummary = $("#txtSummary").val();
    //课时信息
    //var txtTimeLimitNum = $("#txtTimeLimitNum").val();
    var txtStartTime = $("#txtStartTime").val();
    var txtEndTime = $("#txtEndTime").val();
    var txtTeacherType = $("#txtTeacherType").val();
    //验证
    if (txtName == "") {
        alert('课时名称不能为空！');
        return false;
    }
    if (txtPrice == "") {
        alert('课时原价不能为空！');
        return false;
    }
    if (txtNewPrice == "") {
        alert('课时现价不能为空！');
        return false;
    }
    //if (txtAheadMinutes == "") {
    //    alert('提前进入时间不能为空！');
    //    return false;
    //}
    //if (txtLimitNum == "") {
    //    alert('课时人数不能为空！');
    //    return false;
    //}
    //if (txtTimeLimitNum == "") {
    //    alert('课时时间人数不能为空！');
    //    return false;
    //}
    //if (txtStartTime == "") {
    //    alert('课时名称不能为空！');
    //    return false;
    //}
    //if (txtEndTime == "") {
    //    alert('课时名称不能为空！');
    //    return false;
    //}
    if (txtStartTime >= txtEndTime) {
        alert('结束时间必须大于开始时间');
        return false;
    }
    var obj = {
        CoursePeriodTimeID: CoursePeriodTimeID, CoursePeriodID: CoursePeriodID, txtName: txtName, txtSummary: txtSummary, txtPrice: txtPrice, txtNewPrice: txtNewPrice,
        txtStartTime: txtStartTime, txtEndTime: txtEndTime, txtTeacherType: txtTeacherType
    };
    $.post("?action=UpdateCoursePeriod", obj, function (data) {
        if (data) {
            var result = eval("(" + data + ")");
            if (result.state == "1") {
                alert("修改成功!");
                $('#tbdatagrid').datagrid("reload");
                $("#divUpdateCoursPeriod").dialog('close');
            } else {
                alert(result.msg);
            }
        }
    });
}
//修改课程状态
function UpdateStatus(id, state) {
    $.post("?action=UpdateStatus", { ID: id, State: state }, function (data) {
        if (data) {
            var result = eval("(" + data + ")");
            if (result.state == "1") {
                alert("设置成功!");
                $('#tbdatagrid').datagrid("reload");
            } else {
                alert(result.msg);
            }
        }
    });
}

//修改和查看课时图片
function UpdateImg(CoursePeriodID, type) {
    $.post("?action=GetCoursePeriodModel", { ID: CoursePeriodID }, function (data) {
        if (data) {
            var result = eval("(" + data + ")");
            var BigImage = result[0].BigImage;
            $("#ImgShow").attr("src", BigImage);
            $("#divUpdateCoursPeriodImg").attr("style", "display:block");
            if (type == 0) {//查看
                $("#UploadImg").css("display", "none");
                $("#divUpdateCoursPeriodImg").dialog({
                    title: '查看课时海报',
                    width: 750,
                    height: 500,
                    closed: false,
                    cache: false,
                    modal: true,
                    buttons: [
                          {
                              text: '关闭',
                              handler: function (EditionID) {
                                  $("#divUpdateCoursPeriodImg").dialog('close');
                                  $("#ImgShow").attr("src", "");
                              }
                          }
                    ]
                });
            } else {    //修改课时图片
                $("#UploadImg").css("display", "block");
                $("#divUpdateCoursPeriodImg").dialog({
                    title: '修改课时海报',
                    width: 750,
                    height: 500,
                    closed: false,
                    cache: false,
                    modal: true,
                    buttons: [
                        {
                            text: '保存',
                            handler: function () {
                                UpdateCoursePeriodImg(CoursePeriodID);
                            }
                        }, {
                            text: '关闭',
                            handler: function (EditionID) {
                                $("#divUpdateCoursPeriodImg").dialog('close');
                                $("#ImgShow").attr("src", "");
                            }
                        }
                    ]
                });
            }
        }
    });
}
//修改课时图片
function UpdateCoursePeriodImg(id) {
    //上传图片
    var UploadImg = $("#UploadImg").val();
    //alert(UploadImg);
    if (UploadImg != "") {
        $.ajaxFileUpload({
            url: '/Handler/UploadFile.ashx?action=UploadImgToOss&filesName=UploadImg&keyFilePath=SynchronousStudy/SpokenBroadcas/image/CoursePeriod/', //用于文件上传的服务器端请求地址
            secureuri: false, //是否需要安全协议，一般设置为false
            fileElementId: 'UploadImg', //文件上传域的ID
            dataType: 'json', //返回值类型 一般设置为json
            success: function (data, status)  //服务器成功响应处理函数
            {
                if (data.msg == "1") {
                    //alert(data.data); 
                    var ImgUrl = data.data;
                    var obj = { ID: id, ImgUrl: ImgUrl };
                    $.post("?action=UpdateCoursePeriodImg", obj, function (data) {
                        if (data) {
                            var result = eval("(" + data + ")");
                            if (result) {
                                alert("保存成功!");
                                $("#ImgShow").attr("src", ImgUrl);
                                return;
                                $('#tbdatagrid').datagrid("reload");
                                $("#divUpdatePeriodCourse").dialog('close');
                            } else {
                                alert("保存失败!");
                            }
                        }
                    });
                } else {
                    alert('上传图片失败');
                }
            },
            error: function (data, status, e)//服务器响应失败处理函数
            {
                alert('异常');
            }
        });
    } else {
        alert('请选择要修改的图片');
    }
}
//采用正则表达式获取地址栏参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
//新增课时信息
function addCoursePeriod() {
    //清空表单
    $("#txtAddName").val("");
    $("#txtAddNewPrice").val("");
    $("#txtAddPrice").val("");
    $("#AddUploadImg").val("");
    $("#txtAddSummary").val("");
    $("#txtAddStartTime1").val("");
    $("#txtAddEndTime1").val("");
    $("#txtAddTeacherType1").val("");
    $("#txtAddStartTime2").val("");
    $("#txtAddEndTime2").val("");
    $("#txtAddTeacherType2").val("");
    $("#txtAddStartTime3").val("");
    $("#txtAddEndTime3").val("");
    $("#txtAddTeacherType3").val(""); 


    $("#divAddCoursePeriod").css("display", "block");
    $("#divAddCoursePeriod").dialog({
        title: '新增课时',
        width: 750,
        height: 500,
        closed: false,
        cache: false,
        modal: true,
        buttons: [
            {
                text: '保存',
                handler: function () {
                    addCoursePeriodSave();
                }
            }, {
                text: '关闭',
                handler: function () {
                    $("#divAddCoursePeriod").dialog('close');
                }
            }
        ]
    });
}
//提交新增
function addCoursePeriodSave() {
    var hiddenCourseID = $("#hiddenCourseID").val();
    //课时信息
    var txtName = $("#txtAddName").val();
    var selStatus = $("#selStatus").val();
    var txtPrice = $("#txtAddPrice").val();
    var txtNewPrice = $("#txtAddNewPrice").val();
    var AddUploadImg = $("#AddUploadImg").val();
    var txtSummary = $("#txtAddSummary").val();
    //课时时间信息
    var txtAddStartTime1 = $("#txtAddStartTime1").val();
    var txtAddEndTime1 = $("#txtAddEndTime1").val();
    var txtAddTeacherType1 = $("#txtAddTeacherType1").val();
    var txtAddStartTime2 = $("#txtAddStartTime2").val();
    var txtAddEndTime2 = $("#txtAddEndTime2").val();
    var txtAddTeacherType2 = $("#txtAddTeacherType2").val();
    var txtAddStartTime3 = $("#txtAddStartTime3").val();
    var txtAddEndTime3 = $("#txtAddEndTime3").val();
    var txtAddTeacherType3 = $("#txtAddTeacherType3").val();
    //验证
    if (txtName == "") {
        alert('课时名称不能为空！');
        return false;
    }
    if (txtPrice == "") {
        alert('课时原价不能为空！');
        return false;
    }
    if (txtNewPrice == "") {
        alert('课时现价不能为空！');
        return false;
    }
    if (AddUploadImg == "") {
        alert('请选择课时海报！');
        return false;
    }
    //时间
    if (txtAddStartTime1 != "" && txtAddEndTime1 != "") {
        if (txtAddStartTime1 > txtAddEndTime1) {
            alert('上课开始时间1必须小于上课结束时间1');
            return false;
        }
    }
    if (txtAddStartTime2 != "" && txtAddEndTime2 != "") {
        if (txtAddStartTime2 > txtAddEndTime2) {
            alert('上课开始时间2必须小于上课结束时间2');
            return false;
        }
    }
    if (txtAddStartTime3 != "" && txtAddEndTime3 != "") {
        if (txtAddStartTime3 > txtAddEndTime3) {
            alert('上课开始时间3必须小于上课结束时间3');
            return false;
        }
    }
    //上传封面
    $.ajaxFileUpload({
        url: '/Handler/UploadFile.ashx?action=UploadImgToOss&filesName=AddUploadImg&keyFilePath=SynchronousStudy/SpokenBroadcas/image/CoursePeriod/', //用于文件上传的服务器端请求地址
        secureuri: false, //是否需要安全协议，一般设置为false
        fileElementId: 'AddUploadImg', //文件上传域的ID
        dataType: 'json', //返回值类型 一般设置为json
        success: function (data, status)  //服务器成功响应处理函数
        {
            if (data.msg == "1") {
                //alert(data.data); 
                var ImgUrl = data.data;
                //保存课时信息 
                var obj = {
                    CourseID: hiddenCourseID,
                    txtName: txtName, selStatus: selStatus, txtPrice: txtPrice, txtNewPrice: txtNewPrice, txtSummary: txtSummary, ImgUrl: ImgUrl,
                    txtAddStartTime1: txtAddStartTime1, txtAddEndTime1: txtAddEndTime1, txtAddTeacherType1: txtAddTeacherType1,
                    txtAddStartTime2: txtAddStartTime2, txtAddEndTime2: txtAddEndTime2, txtAddTeacherType2: txtAddTeacherType2,
                    txtAddStartTime3: txtAddStartTime3, txtAddEndTime3: txtAddEndTime3, txtAddTeacherType3: txtAddTeacherType3
                };
                $.post("?action=AddCoursePeriod", obj, function (data) {
                    if (data) {
                        var result = eval("(" + data + ")");
                        if (result.state == 1) {
                            alert("保存成功!");
                            $('#tbdatagrid').datagrid("reload");
                            $("#divAddCoursePeriod").dialog('close');
                        } else {
                            alert(result.msg);
                        }
                    }
                });
            } else {
                alert('上传图片失败');
            }
        },
        error: function (data, status, e)//服务器响应失败处理函数
        {
            alert('异常');
        }
    });
}

//新增课时时间信息
function AddCoursePeriodTime(CoursePeriodID) {
    //alert(CoursePeriodID);
    $("#txtAddTimeStartTime1").val("");
    $("#txtAddTimeEndTime1").val("");
    $("#txtAddTimeTeacherType1").val("");
    $("#txtAddTimeStartTime2").val("");
    $("#txtAddTimeEndTime2").val("");
    $("#txtAddTimeTeacherType2").val("");
    $("#txtAddTimeStartTime3").val("");
    $("#txtAddTimeEndTime3").val("");
    $("#txtAddTimeTeacherType3").val("");
    $("#divAddCoursePeriodTime").css("display", "block");
    $("#divAddCoursePeriodTime").dialog({
        title: '新增课时时间',
        width: 750,
        height: 500,
        closed: false,
        cache: false,
        modal: true,
        buttons: [
            {
                text: '保存',
                handler: function () {
                    addCoursePeriodTimeSave(CoursePeriodID);
                }
            }, {
                text: '关闭',
                handler: function () {
                    $("#divAddCoursePeriodTime").dialog('close');
                }
            }
        ]
    });
}
//新增课时时间信息保存
function addCoursePeriodTimeSave(CoursePeriodID) {
    //课时时间信息
    var txtAddStartTime1 = $("#txtAddTimeStartTime1").val();
    var txtAddEndTime1 = $("#txtAddTimeEndTime1").val();
    var txtAddTeacherType1 = $("#txtAddTimeTeacherType1").val();
    var txtAddStartTime2 = $("#txtAddTimeStartTime2").val();
    var txtAddEndTime2 = $("#txtAddTimeEndTime2").val();
    var txtAddTeacherType2 = $("#txtAddTimeTeacherType2").val();
    var txtAddStartTime3 = $("#txtAddTimeStartTime3").val();
    var txtAddEndTime3 = $("#txtAddTimeEndTime3").val();
    var txtAddTeacherType3 = $("#txtAddTimeTeacherType3").val();
    //时间
    if (txtAddStartTime1 != "" && txtAddEndTime1 != "") {
        if (txtAddStartTime1 > txtAddEndTime1) {
            alert('上课开始时间1必须小于上课结束时间1');
            return false;
        }
    }
    if (txtAddStartTime2 != "" && txtAddEndTime2 != "") {
        if (txtAddStartTime2 > txtAddEndTime2) {
            alert('上课开始时间2必须小于上课结束时间2');
            return false;
        }
    }
    if (txtAddStartTime3 != "" && txtAddEndTime3 != "") {
        if (txtAddStartTime3 > txtAddEndTime3) {
            alert('上课开始时间3必须小于上课结束时间3');
            return false;
        }
    }
    //验证
    if ((txtAddStartTime1 != "" && txtAddEndTime1 != "") || (txtAddStartTime2 != "" && txtAddEndTime2 != "") || (txtAddStartTime3 != "" && txtAddEndTime3 != "")) {
        //保存课时时间信息 
        var obj = {
            CoursePeriodID: CoursePeriodID,
            txtAddStartTime1: txtAddStartTime1, txtAddEndTime1: txtAddEndTime1, txtAddTeacherType1: txtAddTeacherType1,
            txtAddStartTime2: txtAddStartTime2, txtAddEndTime2: txtAddEndTime2, txtAddTeacherType2: txtAddTeacherType2,
            txtAddStartTime3: txtAddStartTime3, txtAddEndTime3: txtAddEndTime3, txtAddTeacherType3: txtAddTeacherType3
        };
        $.post("?action=AddCoursePeriodTime", obj, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.state == 1) {
                    alert("保存成功!");
                    $('#tbdatagrid').datagrid("reload");
                    $("#divAddCoursePeriodTime").dialog('close');
                } else {
                    alert(result.msg);
                }
            }
        });

    } else {
        alert('请至少填写一组时间！');
    }
}