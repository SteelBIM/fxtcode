///////////比赛用户查询//////////// 

var GameUserListInit = function () {
    var Current = this;
    this.Init = function () {
        Current.InitGameList();
        //获取版本信息
        GetTB_APPManagement();
    };
    //初始化列表
    this.InitGameList = function () {
        $('#tbdatagrid').datagrid({
            //url: "?action=Query",
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 20, 30, 40, 50, 40, 50],
            width: $(window).width() - 20,
            height: $(window).height() - 95,
            columns: [[
                    {
                        field: 'ContactPhone', title: '手机号码', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.ContactPhone + '</span>';
                            return html;
                        }
                    },
                     {
                         field: 'VersionName', title: '版本', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.VersionName + '</span>';
                             return html;
                         }
                     },
                     {
                         field: 'SignUpTime', title: '报名时间', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + formatDate(row.SignUpTime) + '</span>';
                             return html;
                         }
                     },
                    {
                        field: 'UserName', title: '姓名', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.UserName + '</span>';
                            return html;
                        }
                    },
                     {
                         field: 'SchoolName', title: '学校', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.SchoolName + '</span>';
                             return html;
                         }
                     },
                      {
                          field: 'ClassName', title: '班级', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.ClassName + '</span>';
                              return html;
                          }
                      },
                      {
                          field: 'TeacherName', title: '指导老师', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.TeacherName + '</span>';
                              return html;
                          }
                      },
                       {
                           field: 'Stage', title: '赛段', width: 150, align: 'center', formatter: function (value, row) {
                               var html = '';
                                html += ' <span>' + row.Stage + '</span>';
                               return html;
                           }
                       },
                        {
                            field: 'TotalScore', title: '总分（配音+朗读+投票）', width: 150, align: 'center', formatter: function (value, row) {
                                var html = '';
                                html += ' <span>' + row.TotalScore + '</span>';
                                return html;
                            }
                        },
                         {
                             field: 'BookPlay', title: '配音作品', width: 150, align: 'center', formatter: function (value, row) {
                                 var html = '';
                                 html += ' <a href="' + row.DubbingAddress + '" target="_blank">下载</a>';
                                 return html;
                             }
                         },
                          {
                              field: 'StoryRead', title: '朗读作品', width: 150, align: 'center', formatter: function (value, row) {
                                  var html = '';
                                  html += ' <a href="' + row.DubbingAddress + '" target="_blank">下载</a>';
                                  return html;
                              }
                          }
            ]],
            onDblClickRow: function (rowIndex) {
                //$('#tbdatagrid').datagrid('selectRow', rowIndex);
                //var currentRow = $("#tbdatagrid").datagrid("getSelected");
                //alert(currentRow.ID);
                //UpdateCourse(currentRow.ID);
            }
        });
    }
    //通过关键字搜索
    $("#searchValue").focus(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "请输入手机号码") {
            $("#searchValue").val("");
        }
    })
    $("#searchValue").blur(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "") {
            $("#searchValue").val("请输入手机号码");
        }
    })
    $("#search").click(function () { 
        var selVersionName = $("#selVersionName").val();
        var selStage = $("#selStage").val();
        var searchValue = $("#searchValue").val();
        var queryStr = "";
        queryStr += " VersionID=" + selVersionName + "";
        if (selStage != 0) {
            switch (selStage) {
                case "1":
                    queryStr += " and GradeID<=2 ";
                    break;
                case "2":
                    queryStr += " and GradeID>2 and GradeID<=4";
                    break;
                case "3":
                    queryStr += " and GradeID>4 and GradeID<=6";
                    break;
                default:
                    queryStr += " and GradeID<=2 ";
                    break;
            } 
        } 
        if (searchValue == "请输入手机号码" || searchValue=="") {
            $('#tbdatagrid').datagrid({
                url: '?action=Query&queryStr=' + encodeURI(queryStr)
            });
            return false;
        } 
        queryStr += " and  ContactPhone ='" + searchValue + "'";
        $('#tbdatagrid').datagrid({
            url: '?action=Query&queryStr=' + encodeURI(queryStr)
        });
    })

    //导出excel全部数据
    $("#exportExcel").click(function () {
        window.open("?action=excel");
    });
}

var gameListInit;
$(function () {
    gameListInit = new GameUserListInit();
    gameListInit.Init();
});
//获取版本信息
function GetTB_APPManagement() {
    $.post("?action=GetTB_APPManagement", function (data) {
        //alert(data);
        if (data) {
            var result = eval("(" + data + ")");
            var opt = '';
            for (var i = 0; i < result.length; i++) {
                opt += "<option value='" + result[i].VersionID + "'>" + result[i].VersionName + "</option>";
            }
            $("#selVersionName").html(opt);
        } else {
            alert('版本获取失败');
        }
    });
}
//新增课程
function setting() {
    $.post(
        "?action=setting",
        "",
        function (data) {
            if (data.result) {
                $.each(data.mod, function (idx, item) {
                    switch (idx) {
                        case "SignUpStartTime":
                            $("#SignUpStartTime").val(formatDate(item));
                            break;
                        case "SignUpEndTime":
                            $("#SignUpEndTime").val(formatDate(item));
                            break;
                        case "FirstGameStartTime":
                            $("#FirstGameStartTime").val(formatDate(item));
                            break;
                        case "FirstGameEndTime":
                            $("#FirstGameEndTime").val(formatDate(item));
                            break;
                        case "SecondGameStartTime":
                            $("#SecondGameStartTime").val(formatDate(item));
                            break;
                        case "SecondGameEndTime":
                            $("#SecondGameEndTime").val(formatDate(item));
                            break;
                        case "FinalsStartTime":
                            $("#FinalsStartTime").val(formatDate(item));
                            break;
                        case "FinalsEndTime":
                            $("#FinalsEndTime").val(formatDate(item));
                            break;
                        case "ID":
                            $("#Times").val(item);
                            break;

                    }
                })
            }
        },
        "JSON"
        );

    $("#divsetting").css("display", "block");
    $("#divsetting").dialog({
        title: '设置比赛时间',
        width: 750,
        height: 300,
        closed: false,
        cache: false,
        modal: true,
        buttons: [
            {
                text: '保存',
                handler: function () {
                    settingSave();
                }
            }, {
                text: '关闭',
                handler: function () {
                    $("#divsetting").dialog('close');
                }
            }
        ]
    });
}
//提交数据
function settingSave() {
    var id = $("#Times").val();
    var SignUpStartTime = $("#SignUpStartTime").val();
    var SignUpEndTime = $("#SignUpEndTime").val();
    var FirstGameStartTime = $("#FirstGameStartTime").val();
    var FirstGameEndTime = $("#FirstGameEndTime").val();
    var SecondGameStartTime = $("#SecondGameStartTime").val();
    var SecondGameEndTime = $("#SecondGameEndTime").val();
    var FinalsStartTime = $("#FinalsStartTime").val();
    var FinalsEndTime = $("#FinalsEndTime").val();
    //验证
    if (SignUpStartTime == "") {
        alert('报名开始时间不能为空！');
        return false;
    }
    if (SignUpEndTime == "") {
        alert('报名结束时间不能为空！');
        return false;
    }
    if (FirstGameStartTime == "") {
        alert('初赛开始时间不能为空！');
        return false;
    }
    if (FirstGameEndTime == "") {
        alert('初赛结束时间不能为空！');
        return false;
    }
    if (SecondGameStartTime == "") {
        alert('复赛开始时间不能为空！');
        return false;
    }
    if (SecondGameEndTime == "") {
        alert('复赛结束时间不能为空！');
        return false;
    }
    if (FinalsStartTime == "") {
        alert('决赛开始时间不能为空！');
        return false;
    }
    if (FinalsEndTime == "") {
        alert('决赛结束时间不能为空！');
        return false;
    }
    var obj = {
        id: id,
        SignUpStartTime: SignUpStartTime,
        SignUpEndTime: SignUpEndTime,
        FirstGameStartTime: FirstGameStartTime,
        FirstGameEndTime: FirstGameEndTime,
        SecondGameStartTime: SecondGameStartTime,
        SecondGameEndTime: SecondGameEndTime,
        FinalsStartTime: FinalsStartTime,
        FinalsEndTime: FinalsEndTime
    };
    $.post("?action=operationData", obj, function (data) {
        if (data.state == "1") {
            alert("保存成功!");
            $('#tbdatagrid').datagrid("reload");
            $("#divsetting").dialog('close');

        } else {
            alert(data.msg);
        }
    }, "JSON");
}
function formatDate(now) {
    return new Date(parseInt(now.substr(6, 13))).toLocaleDateString().replace("/", "-").replace("/", "-");
}