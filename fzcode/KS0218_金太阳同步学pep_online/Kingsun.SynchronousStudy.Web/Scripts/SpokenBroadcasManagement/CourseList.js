///////////课程管理//////////// 

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
                        field: 'ID', title: '课程ID', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.ID + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'Name', title: '课程名称', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.Name + '</a>';
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
                         field: 'Num', title: '课时数量', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.Num + '</a>';
                             return html;
                         }
                     },
                      {
                          field: "BigImage", title: "课程海报", width: 120, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html = '<a href="javascript:void(0)" onclick="UpdateImg(' + row.ID + ',0)">查看</a>   ';
                              html += '<a href="javascript:void(0)" onclick="UpdateImg(' + row.ID + ',1)">修改</a>   ';
                              return html;
                          }
                      },
                      {
                          field: 'Status', title: '课程状态', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + (row.Status == 1 ? "启用" : "禁用") + '</a>';
                              return html;
                          }
                      },
                    {
                        field: "Operate", title: "操作", width: 120, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html = '<a href="javascript:void(0)" onclick="UpdateCourse(' + row.ID + ')" >修改</a>   ';
                            if (row.Status == 1) {
                                html += '<a href="javascript:void(0)" onclick="UpdateState(' + row.ID + ',0)" >禁用</a>   ';
                            } else {
                                html += '<a href="javascript:void(0)" onclick="UpdateState(' + row.ID + ',1)" >启用</a>   ';
                            }
                            html += '<a href="CoursePeriodList.aspx?CourseID=' + row.ID + '"  >查看课时</a>   ';
                            html += '<a href="javascript:void(0)" onclick="DelCourse(' + row.ID + ')" >删除</a>   ';
                            return html;
                        }
                    }
            ]],
            onDblClickRow: function (rowIndex) {
                $('#tbdatagrid').datagrid('selectRow', rowIndex);
                var currentRow = $("#tbdatagrid").datagrid("getSelected");
                //alert(currentRow.ID);
                UpdateCourse(currentRow.ID);
            }
        });
    }
    //通过关键字搜索
    $("#searchValue").focus(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "请输入课程名称") {
            $("#searchValue").val("");
        }
    })
    $("#searchValue").blur(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "") {
            $("#searchValue").val("请输入课程名称");
        }
    })
    $("#search").click(function () {
        var searchValue = $("#searchValue").val();
        var queryStr = "";
        if (searchValue == "请输入课程名称") {
            $('#tbdatagrid').datagrid({
                url: '?action=Query'
            });
            return false;
        }
        queryStr += " Name like '%" + searchValue + "%'";
        $('#tbdatagrid').datagrid({
            url: '?action=Query&queryStr=' + encodeURI(queryStr)
        });
    })
}

var courseListInit;
$(function () {
    courseListInit = new CourseListInit();
    courseListInit.Init();
});
//删除课程
function DelCourse(ID) {
    if (confirm("您确定要删除吗？")) {
        $.post("?action=DelCourse", { ID: ID }, function (data) {
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
//修改课程信息
function UpdateCourse(id) {
    $.post("?action=GetCourseModel", { ID: id }, function (data) {
        if (data) {
            var result = eval("(" + data + ")");
            $("#txtCourseID").text(result[0].ID);
            $("#txtName").val(result[0].Name);
            $("#selType").val(result[0].Type);
            $("#txtGroups").val(result[0].Groups);
            //$("#txtAheadMinutes").val(result[0].AheadMinutes);
            $("#txtNum").val(result[0].Num);
            var txtOpenDate = new Date(eval('new ' + (result[0].OpenDate.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
            $("#txtOpenDate").val(txtOpenDate);
            $("#txtSummary").val(result[0].Summary);
            $("#txtStudioUrl").val(result[0].StudioUrl);
            $("#txtStudioCommand").val(result[0].StudioCommand);
            $("#divUpdateCours").css("display", "block");
            $("#divUpdateCours").dialog({
                title: '修改课程',
                width: 750,
                height: 500,
                closed: false,
                cache: false,
                modal: true,
                buttons: [
                    {
                        text: '保存',
                        handler: function () {
                            UpdateCourseSave(id);
                        }
                    }, {
                        text: '关闭',
                        handler: function () {
                            $("#divUpdateCours").dialog('close');
                        }
                    }
                ]
            });
        }
    });
}
//修改课程信息提交
function UpdateCourseSave(ID) {
    var txtName = $("#txtName").val();
    var selType = $("#selType").val();
    var txtGroups = $("#txtGroups").val();
    //var txtAheadMinutes = $("#txtAheadMinutes").val();
    var txtNum = $("#txtNum").val();
    var txtOpenDate = $("#txtOpenDate").val();
    var txtSummary = $("#txtSummary").val();
    var txtStudioUrl = $("#txtStudioUrl").val().replace(/(^\s+)|(\s+$)/g, "");
    var txtStudioCommand = $("#txtStudioCommand").val().replace(/(^\s+)|(\s+$)/g, "");
    //验证
    if (txtName == "") {
        alert('课程名称不能为空！');
        return false;
    }
    if (txtSummary == "") {
        alert('课程描述不能为空！');
        return false;
    }
    if (txtGroups == "") {
        alert('课程级别不能为空！');
        return false;
    }
    //if (txtAheadMinutes == "") {
    //    alert('提前进入时间不能为空！');
    //    return false;
    //}
    if (txtNum == "") {
        alert('课时数量不能为空！');
        return false;
    }
    //if (txtOpenDate == "") {
    //    alert('开课时间不能为空！');
    //    return false;
    //}
    var obj = {
        CourseID: ID, txtName: txtName, selType: selType, txtGroups: txtGroups, txtNum: txtNum, txtOpenDate: txtOpenDate, txtSummary: txtSummary,
        txtStudioUrl: txtStudioUrl, txtStudioCommand: txtStudioCommand
    };
    $.post("?action=UpdateCourse", obj, function (data) {
        if (data) {
            var result = eval("(" + data + ")");
            if (result.state == "1") {
                alert("修改成功!");
                $('#tbdatagrid').datagrid("reload");
                $("#divUpdateCours").dialog('close');
            } else {
                alert(result.msg);
            }
        }
    });
}
//修改课程状态
function UpdateState(id, state) {
    $.post("?action=UpdateState", { ID: id, State: state }, function (data) {
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
//修改和查看课程图片
function UpdateImg(id, type) {
    $.post("?action=GetCourseModel", { ID: id }, function (data) {
        if (data) {
            var result = eval("(" + data + ")");
            var BigImage = result[0].BigImage;
            $("#ImgShow").attr("src", BigImage);
            $("#divUpdateCourse").attr("style", "display:block");
            if (type == 0) {//查看
                $("#UploadImg").css("display", "none");
                $("#divUpdateCourse").dialog({
                    title: '查看课程海报',
                    width: 750,
                    height: 500,
                    closed: false,
                    cache: false,
                    modal: true,
                    buttons: [
                          {
                              text: '关闭',
                              handler: function (EditionID) {
                                  $("#divUpdateCourse").dialog('close');
                                  $("#ImgShow").attr("src", "");
                              }
                          }
                    ]
                });
            } else {    //修改课程图片
                $("#UploadImg").css("display", "block");
                $("#divUpdateCourse").dialog({
                    title: '修改课程海报',
                    width: 750,
                    height: 500,
                    closed: false,
                    cache: false,
                    modal: true,
                    buttons: [
                        {
                            text: '保存',
                            handler: function () {
                                UpdateCourseImg(id);
                            }
                        }, {
                            text: '关闭',
                            handler: function (EditionID) {
                                $("#divUpdateCourse").dialog('close');
                                $("#ImgShow").attr("src", "");
                            }
                        }
                    ]
                });
            }
        }
    });
}

//修改课程图片
function UpdateCourseImg(id) {
    //上传图片
    var UploadImg = $("#UploadImg").val();
    //alert(UploadImg);
    if (UploadImg != "") {
        $.ajaxFileUpload({
            url: '/Handler/UploadFile.ashx?action=UploadImgToOss&filesName=UploadImg&keyFilePath=SynchronousStudy/SpokenBroadcas/image/Course/', //用于文件上传的服务器端请求地址
            secureuri: false, //是否需要安全协议，一般设置为false
            fileElementId: 'UploadImg', //文件上传域的ID
            dataType: 'json', //返回值类型 一般设置为json
            success: function (data, status)  //服务器成功响应处理函数
            {
                if (data.msg == "1") {
                    //alert(data.data); 
                    var ImgUrl = data.data;
                    var obj = { ID: id, ImgUrl: ImgUrl };
                    $.post("?action=UpdateCourseImg", obj, function (data) {
                        if (data) {
                            var result = eval("(" + data + ")");
                            if (result) {
                                alert("保存成功!");
                                $('#tbdatagrid').datagrid("reload");
                                $("#divUpdateCourse").dialog('close');
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

//新增课程
function addCourse() {
    //清空表单
    $("#txtAddName").val("");
    $("#txtAddGroups").val("");
    $("#txtAddNum").val("");
    $("#txtAddOpenDate").val("");
    $("#txtAddSummary").val("");
    $("#AddUploadImg").val("");
    $("#txtAddStudioUrl").val("");
    $("#txtAddStudioCommand").val("");

    $("#divAddCourse").css("display", "block");
    $("#divAddCourse").dialog({
        title: '修改课程',
        width: 750,
        height: 500,
        closed: false,
        cache: false,
        modal: true,
        buttons: [
            {
                text: '保存',
                handler: function () {
                    addCourseSave();
                }
            }, {
                text: '关闭',
                handler: function () {
                    $("#divAddCourse").dialog('close');
                }
            }
        ]
    });
}
//提交新增
function addCourseSave() {
    var txtName = $("#txtAddName").val();
    var selStatus = $("#selStatus").val();//课程状态
    var selType = $("#selAddType").val();
    var txtGroups = $("#txtAddGroups").val();
    //var txtAheadMinutes = $("#txtAddAheadMinutes").val();
    var txtNum = $("#txtAddNum").val();
    var txtOpenDate = $("#txtAddOpenDate").val();
    var txtSummary = $("#txtAddSummary").val();
    var AddUploadImg = $("#AddUploadImg").val();
    var txtStudioUrl = $("#txtAddStudioUrl").val().replace(/(^\s+)|(\s+$)/g, "");
    var txtStudioCommand = $("#txtAddStudioCommand").val().replace(/(^\s+)|(\s+$)/g, "");
    //验证
    if (txtName == "") {
        alert('课程名称不能为空！');
        return false;
    }
    if (txtSummary == "") {
        alert('课程描述不能为空！');
        return false;
    }
    if (txtGroups == "") {
        alert('课程级别不能为空！');
        return false;
    }
    if (AddUploadImg == "") {
        alert('请选择课程海报！');
        return false;
    }
    if (txtNum == "") {
        alert('课时数量不能为空！');
        return false;
    }
    //上传封面
    $.ajaxFileUpload({
        url: '/Handler/UploadFile.ashx?action=UploadImgToOss&filesName=AddUploadImg&keyFilePath=SynchronousStudy/SpokenBroadcas/image/Course/', //用于文件上传的服务器端请求地址
        secureuri: false, //是否需要安全协议，一般设置为false
        fileElementId: 'AddUploadImg', //文件上传域的ID
        dataType: 'json', //返回值类型 一般设置为json
        success: function (data, status)  //服务器成功响应处理函数
        {
            if (data.msg == "1") {
                //alert(data.data); 
                var ImgUrl = data.data;
                //保存课程信息 
                var obj = {
                    txtName: txtName, selStatus: selStatus, selType: selType, txtGroups: txtGroups, txtNum: txtNum, txtOpenDate: txtOpenDate, txtSummary: txtSummary, ImgUrl: ImgUrl
                    , txtStudioUrl: txtStudioUrl, txtStudioCommand: txtStudioCommand
                };
                $.post("?action=AddCourse", obj, function (data) {
                    if (data) {
                        var result = eval("(" + data + ")");
                        if (result) {
                            alert("保存成功!");
                            $('#tbdatagrid').datagrid("reload");
                            $("#divAddCourse").dialog('close');
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
