///////////优惠卷管理////////////


var CouponListInit = function () {
    var Current = this;
    this.Init = function () {

        Current.InitCouponList();

    };

    //初始化列表
    this.InitCouponList = function () {
        $('#tbdatagrid').datagrid({
            url: "?action=Query",
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 30, 40, 50],
            width: $(window).width() - 20,
            height: $(window).height() - 95,
            columns: [[
                {
                    field: 'EditionID', title: '版本ID', width: 150, align: 'center', formatter: function (value, row) {
                        var html = '';
                        html += ' <span>' + row.EditionID + '</a>';
                        return html;
                    }
                },
                 {
                     field: 'TextbookVersion', title: '版本', width: 150, align: 'center', formatter: function (value, row) {
                         var html = '';
                         html += ' <span>' + row.TextbookVersion + '</a>';
                         return html;
                     }
                 },
                {
                    field: 'TicketName', title: '使用券名称', width: 150, align: 'center', formatter: function (value, row) {
                        var html = '';
                        html += ' <span>' + row.TicketName + '</a>';
                        return html;
                    }
                },
                   {
                       field: 'StartDate', title: '开始时间', width: 150, align: 'center', formatter: function (value, row) {
                           var html = '';
                           var StartDate = new Date(eval('new ' + (row.StartDate.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
                           html += ' <span>' + StartDate + '</a>';
                           return html;
                       }
                   },
                      {
                          field: 'EndDate', title: '结束时间', width: 150, align: 'center', formatter: function (value, row) {
                              var html = '';
                              var EndDate = new Date(eval('new ' + (row.EndDate.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
                              html += ' <span>' + EndDate + '</a>';
                              return html;
                          }
                      },
                {
                    field: "Operate", title: "操作", width: 120, align: 'center', formatter: function (value, row) {
                        var html = '';
                        //html = '<a href="javascript:void(0)" onclick="DelCoupon(' + row.EditionID + ')" >删除</a>   ';
                        html += '<a href="javascript:void(0)" onclick="UpdateCoupon(' + row.EditionID + ')" >修改</a>   ';
                        //html += '<a href="../ModuleManagement/ModuleConfig.aspx?bookid=' + row.BookID + '&BookName=' + encodeURI(row.EducationLevel + row.CourseCategory + row.TextbookVersion + row.JuniorGrade + row.TeachingBooks) + '" >配置模块</a>   ';
                        //if (row.State) {
                        //    html += '<a href="javascript:void(0)" onclick="courseManagerInit.ExportExcel(' + row.BookID + ')">下载模板</a>   ';
                        //    html += '<a href="javascript:void(0)" onclick="courseManagerInit.ImportExcel(' + row.BookID + ')">导入资源</a>   ';
                        //} else {
                        //    html += '<span>下载模板</span>   ';
                        //    html += '<span>导入资源</span>   ';
                        //}
                        //html += '<a href="javascript:void(0)" onclick="courseManagerInit.UpdataCatalog(' + row.BookID + ',\'' + row.EducationLevel + row.CourseCategory + row.TextbookVersion + row.JuniorGrade + row.TeachingBooks + '\')">更新目录</a>   ';
                        //html += '<a href="javascript:void(0)" onclick="courseManagerInit.ExportCatalogExcel(' + row.BookID + ')">下载目录页码模板</a>   ';
                        //html += '<a href="javascript:void(0)" onclick="courseManagerInit.ImportCatalogExcel(' + row.BookID + ')">导入目录页码资源</a>   ';
                        //html += '<a href="/Course/CourseVersion.aspx?CourseID=' + row.BookID + '">课程版本管理</a>   ';
                        return html;
                    }
                }
            ]]
        });
    }

    //加载版本  
    this.InitEdition = function () {
        $.post("?action=GetAppVersionList", {}, function (data) {
            $("#selectEdition").html("");
            $("#selectEdition").append("<option value=0 selected>请选择版本</option>");
            var edition = eval("(" + data + ")");
            var versionModel = {};
            for (var i = 0; i < edition.length; i++) {
                $("#selectEdition").append("<option value=" + edition[i].VersionID + ">" + edition[i].VersionName + "</option>");
            }
        });
        return;
        Common.CodeAjax("do.jsonp", "ED", function (data) {
            var edition = data["ED"];
            var edi = [];
            $.each(edition, function (e, obj) {
                edi.push(obj);
            });
            var titleobj = {};
            titleobj.ID = 0;
            titleobj.CodeName = "请选择版本";
            edi.unshift(titleobj);
            $("#selectEdition").KingsunSelect({
                data: edi,
                onchange: function (index, data) {
                    Current.EditionID = data.ID;
                    Current.Edition = data.CodeName;
                }
            });
            $("#selectEdition").data("select").selectValue(Current.EditionID);
        });
    }

    //添加优惠卷
    $("#addCoupon").click(function () {
        Current.InitEdition();
        $("#divAddCoupon").attr("style", "display:block");
        $("#divAddCoupon").dialog({
            title: '添加优惠卷',
            width: 750,
            height: 300,
            closed: false,
            cache: false,
            modal: true,
            buttons: [
                {
                    text: '保存',
                    handler: Current.SaveCoupon
                }, {
                    text: '关闭',
                    handler: function () {
                        $("#divAddCoupon").dialog('close');

                    }
                }
            ]
        });
    });
    //保存
    this.SaveCoupon = function () {
        var selectEdition = $("#selectEdition").val();
        var selectType = $("#selectType").val();//卷类型 
        var txtName = $("#txtName").val();
        var txtPrice = $("#txtPrice").val();
        var txtStartDate = $("#txtStartDate").val();
        var txtEndDate = $("#txtEndDate").val();
        var UploadImg = $("#UploadImg").val();
        var selectStatus = $("#selectStatus").val();//卷状态
        if (selectEdition == 0) {
            alert('请选择版本');
            return false;
        }
        if (txtName == "") {
            alert('请输入优惠卷名字');
            return false;
        }
        if (txtPrice == "") {
            alert('请输入额度');
            return false;
        }
        if (txtStartDate == "") {
            alert('请输入开始时间');
            return false;
        }
        if (txtEndDate == "") {
            alert('请输入结束时间');
            return false;
        }
        if (UploadImg == "") {
            alert('请上传图片');
            return false;
        }
        if (txtStartDate >= txtEndDate) {
            alert('结束时间必须大于开始时间');
            return false;
        }
        $.ajaxFileUpload({
            url: '/Handler/UploadFile.ashx?action=UploadImg', //用于文件上传的服务器端请求地址
            secureuri: false, //是否需要安全协议，一般设置为false
            fileElementId: 'UploadImg', //文件上传域的ID
            dataType: 'json', //返回值类型 一般设置为json
            success: function (data, status)  //服务器成功响应处理函数
            {
                if (data.msg == "1") {
                    var obj = { Edition: selectEdition, selectType: selectType, TicketName: txtName, Price: txtPrice, StartDate: txtStartDate, EndDate: txtEndDate, ImgUrl: data.data, selectStatus: selectStatus };
                    $.post("?action=save", obj, function (data) {
                        var result = eval("(" + data + ")");
                        if (result.state == "1") {
                            alert("保存成功!");
                            $('#tbdatagrid').datagrid("reload");
                            $("#divAddCoupon").dialog('close');
                        } else if (result.state == "2") {
                            if (confirm(result.msg)) {  //当前版本的使用卷正在使用
                                $.post("?action=existsSave", obj, function (data) {
                                    var ExistsResult = eval("(" + data + ")");
                                    if (ExistsResult.state == "1") {
                                        alert(ExistsResult.msg);
                                        $('#tbdatagrid').datagrid("reload");
                                        $("#divAddCoupon").dialog('close');
                                    } else {
                                        alert(ExistsResult.msg);
                                    }
                                });
                            }
                        } else {
                            alert(result.msg);
                        }
                    });
                } else {
                    alert(data.data);
                }
            },
            error: function (data, status, e)//服务器响应失败处理函数
            {
                alert('异常');
            }
        });
    }
    //删除
    //this.DelCoupon = function (EditionID) {
    //    if (confirm("您确定要删除吗？")) {
    //        $.post("?action=del", { EditionID: EditionID }, function (data) {
    //            if (data) {
    //                var result = eval("(" + data + ")");
    //                if (result.state == "1") {
    //                    alert("删除成功!");
    //                    $('#tbdatagrid').datagrid("reload");
    //                } else {
    //                    alert(result.msg);
    //                }
    //            }
    //        });
    //    }
    //}

    //通过关键字搜索
    $("#searchValue").focus(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "查询关键字") {
            $("#searchValue").val("");
        }
    })
    $("#searchValue").blur(function () {
        var searchValue = $("#searchValue").val();
        if (searchValue == "") {
            $("#searchValue").val("查询关键字");
        }
    })
    $("#search").click(function () {
        var searchValue = $("#searchValue").val();
        var queryStr = "";
        if (searchValue == "查询关键字") {
            $('#tbdatagrid').datagrid({
                url: '?action=Query'
            });
            return false;
        }
        queryStr += " and TextbookVersion like '%" + searchValue + "%'";
        $('#tbdatagrid').datagrid({
            url: '?action=Query&queryStr=' + encodeURI(queryStr)
        });
    })
}

var couponListInit;
$(function () {
    couponListInit = new CouponListInit();
    couponListInit.Init();
});
//删除
function DelCoupon(EditionID) {
    if (confirm("您确定要删除吗？")) {
        $.post("?action=del", { EditionID: EditionID }, function (data) {
            if (data) {
                var result = eval("(" + data + ")");
                if (result.state == "1") {
                    alert("删除成功!");
                    $('#tbdatagrid').datagrid("reload");
                } else {
                    alert(result.msg);
                }
            }
        });
    }
}

//修改
function UpdateCoupon(EditionID) {
    //alert(EditionID);
    $.post("?action=getticketmodel", { Edition: EditionID }, function (data) {
        //alert(data);
        var result = eval("(" + data + ")");
        var StartDate = result[0].StartDate;
        var EndDate = result[0].EndDate;
        var Status = result[0].Status;
        StartDate = new Date(eval('new ' + (StartDate.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
        EndDate = new Date(eval('new ' + (EndDate.replace(/\//g, '')))).format('yyyy-MM-dd hh:mm:ss');
        $("#txtUpdateStartDate").val(StartDate);
        $("#txtUpdateEndDate").val(EndDate);
        $("#selectUpdateStatus").val(Status);
        $("#divUpdateCoupon").attr("style", "display:block");
        $("#divUpdateCoupon").dialog({
            title: '修改优惠卷',
            width: 750,
            height: 300,
            closed: false,
            cache: false,
            modal: true,
            buttons: [
                {
                    text: '保存',
                    handler: function () {
                        StartDate = $("#txtUpdateStartDate").val();
                        EndDate = $("#txtUpdateEndDate").val();
                        Status = $("#selectUpdateStatus").val();
                        //alert(EditionID);
                        if (StartDate == "") {
                            alert('请输入开始时间');
                            return false;
                        }
                        if (EndDate == "") {
                            alert('请输入结束时间');
                            return false;
                        }
                        if (StartDate >= EndDate) {
                            alert('结束时间必须大于开始时间');
                            return false;
                        }
                        $.post("?action=update", { Edition: EditionID, StartDate: StartDate, EndDate: EndDate, Status: Status }, function (data) {
                            //alert(data);
                            var result = eval("(" + data + ")");
                            alert(result.msg);
                            $('#tbdatagrid').datagrid("reload");
                            $("#divUpdateCoupon").dialog('close');
                        });
                    }
                }, {
                    text: '关闭',
                    handler: function (EditionID) {
                        $("#divUpdateCoupon").dialog('close');
                    }
                }
            ]
        });
    });
}

Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    fmt = fmt || "yyyy-MM-dd";
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}