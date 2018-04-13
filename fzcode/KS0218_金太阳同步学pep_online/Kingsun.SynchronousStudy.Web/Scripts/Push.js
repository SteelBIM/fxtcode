///////////推送列表查询//////////// 

var GameUserListInit = function () {
    var Current = this;
    this.Init = function () {
        Current.InitGameList();
    };
    //初始化列表
    this.InitGameList = function () {
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
                        field: 'ID', title: '序号', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.ID + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'VersionName', title: '推送版本', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.VersionName + '</a>';
                             return html;
                         }
                     },
                    {
                        field: 'IdentityType', title: '身份', width: 150, align: 'center', formatter: function (value, row) {
                            var html = '';
                            html += ' <span>' + row.IdentityType + '</a>';
                            return html;
                        }
                    },
                     {
                         field: 'Content', title: '内容', width: 150, align: 'center', formatter: function (value, row) {
                             var html = '';
                             html += ' <span>' + row.Content + '</a>';
                             return html;
                         }
                     },
                      {
                          field: 'Jump', title: '跳转到', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + row.Jump + '</a>';
                              return html;
                          }
                      },
                      {
                          field: 'PushTime', title: '推送时间', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              html += ' <span>' + formatDate(row.PushTime) + '</a>';
                              return html;
                          }
                      },
                       {
                           field: 'VersionNumber', title: '推送版本号', width: 150, align: 'center', formatter: function (value, row) {
                               var html = '';
                               html += ' <span>' + row.VersionNumber + '</a>';
                               return html;
                           }
                       },
                       {
                           field: "Operate", title: "操作", width: 120, align: 'center', formatter: function (value, row) {
                               var html = '';
                               if (row.State == 1) {
                                   html += '<a href="javascript:void(0)" onclick="UpdateState(' + row.ID + ',0)" >禁用</a>   ';
                               } else {
                                   html += '<a href="javascript:void(0)" onclick="UpdateState(' + row.ID + ',1)" >启用</a>   ';
                               }
                               html += '<a href="javascript:void(0)" onclick="Update(' + row.ID + ')" >修改</a>   ';
                               html += '<a href="javascript:void(0)" onclick="Del(' + row.ID + ')" >删除</a>   ';
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
}

var gameListInit;
$(function () {
    gameListInit = new GameUserListInit();
    gameListInit.Init();
    //获取版本信息
    GetTB_APPManagement();
    $("#VersionName").change(function () {
        var VersionID = $("#VersionName").val();
        GetTB_ApplicationVersionByVersionID(VersionID);
    });
});
//获取版本号信息
function GetTB_ApplicationVersionByVersionID(VersionID) {
    $.post("?action=GetTB_ApplicationVersionByVersionID", { "VersionID": VersionID }, function (data) {
        //alert(data);
        if (data) {
            var result = eval("(" + data + ")");
            var opt = '';
            for (var i = 0; i < result.length; i++) {
                opt += "<option value='" + result[i].ID + "'>" + result[i].VersionNumber + "</option>";
            }
            $("#VersionNumber").html(opt);
        } else {
            alert('版本号获取失败');
        }
    });
}
//获取版本信息
function GetTB_APPManagement()
{
    $.post("?action=GetTB_APPManagement", function (data) {
        //alert(data);
        if (data) {
            var result = eval("(" + data + ")");
            var opt = '';
            for (var i = 0; i < result.length; i++) {
                opt+="<option value='" + result[i].VersionID + "'>" + result[i].VersionName + "</option>";
            } 
            $("#VersionName").html(opt);
        } else {
            alert('版本获取失败');
        }
    });
}
//修改状态
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
//修改
function Update(id) { 
    $.post("?action=GetModel", { ID: id }, function (data) {
        if (data) {
            //alert(data);
            var result = eval("(" + data + ")"); 
            $("#VersionName option:contains('" + result[0].VersionName + "')").attr('selected', true);
            $("#IdentityType option:contains('" + result[0].IdentityType + "')").attr('selected', true); 
            $("#VersionNumber option:contains('" + result[0].VersionNumber + "')").attr('selected', true);
            $("#Jump option:contains('" + result[0].Jump + "')").attr('selected', true);  
            var PushTime = new Date(eval('new ' + (result[0].PushTime.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
            $("#PushTime").val(PushTime);
            $("#txtContent").val(result[0].Content);

            $("#divPush").css("display", "block");
            $("#divPush").dialog({
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
                            if (result[0].State==1) {
                                alert("启用状态下不可修改！"); 
                            } else {
                                settingSave(1, id);
                            } 
                        }
                    }, {
                        text: '关闭',
                        handler: function () {
                            $("#divPush").dialog('close');
                        }
                    }
                ]
            });
        }
    });
}
//删除
function Del(ID) {
    if (confirm("您确定要删除吗？")) {
        $.post("?action=Del", { ID: ID }, function (data) {
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
//物理删除
function Del(ID) {
    if (confirm("您确定要删除吗？")) {
        $.post("?action=DelState", { ID: ID }, function (data) {
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
//新增
function AddUpdate(type, id) { 
    $("#divPush").css("display", "block");
    if (type == 0) {  //新增
        $("#hTypeName").text("新增推送");
        $("#divPush").dialog({
            title: '设置推送内容',
            width: 750,
            height: 500,
            closed: false,
            cache: false,
            modal: true,
            buttons: [
                {
                    text: '保存',
                    handler: function () {
                        settingSave(0,0);
                    }
                }, {
                    text: '关闭',
                    handler: function () {
                        $("#divPush").dialog('close');
                    }
                }
            ]
        });
    } else {    //修改
        $("#hTypeName").text("修改推送");
    } 
}
//提交数据
function settingSave(type, id) {
   
    var VersionID = $("#VersionName").val();
    var VersionName = $("#VersionName").find("option:selected").text();
    var IdentityType = $("#IdentityType").find("option:selected").text();
    var VersionNumber = $("#VersionNumber").find("option:selected").text();
    var PushTime = $("#PushTime").val();
    var Jump = $("#Jump").find("option:selected").text();
    var txtContent = $("#txtContent").val();
    if (txtContent=="") {
        alert('推送内容不能为空！');
        return false;
    }
    if (PushTime == "") {
        alert('推送时间不能为空！');
        return false;
    }
    if (type == 0) {  //新增 
        var obj = {
            VersionID: VersionID,
            VersionName: VersionName,
            IdentityType: IdentityType,
            VersionNumber: VersionNumber,
            PushTime: PushTime,
            Jump: Jump,
            Content: txtContent
        };
        $.post("?action=Add", obj, function (data) {
            var data = eval("(" + data + ")");
            if (data.state == "1") {
                alert("保存成功!");
                $('#tbdatagrid').datagrid("reload");
                $("#divPush").dialog('close'); 
            } else {
                alert(data.msg);
            }
        });
    } else {    //修改
        var obj = {
            ID:id,
            VersionID: VersionID,
            VersionName: VersionName,
            IdentityType: IdentityType,
            VersionNumber: VersionNumber,
            PushTime: PushTime,
            Jump: Jump,
            Content: txtContent
        }; 
        $.post("?action=Update", obj, function (data) {
            var data = eval("(" + data + ")");
            if (data.state == "1") {
                alert("修改成功!");
                $('#tbdatagrid').datagrid("reload");
                $("#divPush").dialog('close');

            } else {
                alert(data.msg);
            }
        }); 
    } 
}
function formatDate(now) {
    return new Date(parseInt(now.substr(6, 13))).toLocaleDateString().replace("/", "-").replace("/", "-");
}