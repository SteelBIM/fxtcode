$(function () {
    $.post("/ResourceMgr/GetcurrentAction", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            var dataaction = "";
            if (jsonData.Add)
                dataaction += "<button style=\"background-color:#E16965; color:#fff;border-color:#E16965; margin-right:15px; border-radius:5px;\"  onclick='AddCourse()' id=\"btn_add\" type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>新建 </button>"
            dataaction += "<button style=\"float:left; margin-left:20px; background-color:#515562; color:#fff; border-radius:5px;\"  onclick='BatchDelCourse()'  type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>删除 </button>"
            dataaction += "<button style=\"float:left; margin-left:20px; background-color:#515562; color:#fff; border-radius:5px;\"  onclick='BatchStartCourse()'  type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-plus\" aria-hidden=\"true\"></span>启用 </button>"
            dataaction += "<button style=\"float:left; margin-left:20px; background-color:#515562; color:#fff; border-radius:5px;\"  onclick='BatchStopCourse()'  type=\"button\" class=\"btn btn-default\"><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span>禁用 </button>"
            dataaction += "<button style=\"float:left; margin-left:20px; background-color:#515562; color:#fff; border-radius:5px;\"  onclick='OpenCourseExcel()'  type=\"button\" class=\"btn btn-default\"><span class=\"\" aria-hidden=\"true\"></span>下载目录模板 </button>"
            $("#toolbar").html(dataaction);
            var oTable = new TableInit(jsonData);
            oTable.Init();
        }
    });

    $.post("/ResourceMgr/GetSelectValue", function (data) {
        if (data != null && data != "") {
            var jsonData = eval(data);
            for (var i = 0; i < jsonData.length; i++) {
                if (jsonData[i].Keys == "学段") {
                    var htmlPeriod = '';
                    htmlPeriod += ' <option value="' + jsonData[i].ID + '">' + jsonData[i].Name + '</option>';
                    $(htmlPeriod).appendTo("#Period");
                    $(htmlPeriod).appendTo("#ModalPeriod");
                }
                if (jsonData[i].Keys == "年级") {
                    var htmlGrade = '';
                    htmlGrade += ' <option value="' + jsonData[i].ID + '">' + jsonData[i].Name + '</option>';
                    $(htmlGrade).appendTo("#Grade");
                    $(htmlGrade).appendTo("#ModalGrade");
                }
                if (jsonData[i].Keys == "版本") {
                    var htmlEdition = '';
                    htmlEdition += ' <option value="' + jsonData[i].ID + '">' + jsonData[i].Name + '</option>';
                    $(htmlEdition).appendTo("#Edition");
                    $(htmlEdition).appendTo("#ModalEdition");
                }
                if (jsonData[i].Keys == "学科") {
                    var htmlSubject = '';
                    htmlSubject += ' <option value="' + jsonData[i].ID + '">' + jsonData[i].Name + '</option>';
                    $(htmlSubject).appendTo("#Subject");
                    $(htmlSubject).appendTo("#ModalSubject");
                }
                if (jsonData[i].Keys == "册别") {
                    var htmlReel = '';
                    htmlReel += ' <option value="' + jsonData[i].ID + '">' + jsonData[i].Name + '</option>';
                    $(htmlReel).appendTo("#Reel");
                    $(htmlReel).appendTo("#ModalReel");
                }
                if (jsonData[i].Keys == "出版社") {
                    var htmlPublish = '';
                    htmlPublish += ' <option value="' + jsonData[i].ID + '">' + jsonData[i].Name + '</option>';
                    $(htmlPublish).appendTo("#Publish");
                    $(htmlPublish).appendTo("#ModalPublish");
                }
            }
            var htmlStatus = '';
            htmlStatus += ' <option value="' + 0 + '">' + '未启用' + '</option>';
            htmlStatus += ' <option value="' + 1 + '">' + '启用' + '</option>';
            htmlStatus += ' <option value="' + 2 + '">' + '禁用' + '</option>';
            $(htmlStatus).appendTo("#Status");
        }
    });

    $("#searchbtn").click(function () {
        $('#tb_resource').bootstrapTable('selectPage', 1);
    })

    $("#btn_save").click(function () {
        //JS验证
        if ($("#ModalPeriod").val()==-1) {
            bootbox.alert("请选择学段");
            return;
        }
        if ($("#ModalGrade").val() == -1) {
            bootbox.alert("请选择年级");
            return;
        }
        if ($("#ModalReel").val() == -1) {
            bootbox.alert("请选择册别");
            return;
        }
        if ($("#ModalSubject").val() == -1) {
            bootbox.alert("请选择学科");
            return;
        }
        if ($("#ModalEdition").val() == -1) {
            bootbox.alert("请选择版本");
            return;
        }
        if ($("#ModalPublish").val() == -1) {
            bootbox.alert("请选择出版社");
            return;
        }
        if ($("#BookName").val() == "") {
            bootbox.alert("请填写书名");
            return;
        }
        var formData = new FormData($("#formAdd")[0]);
        formData.append("PeriodName", $("#ModalPeriod").find("option:selected").text());
        formData.append("GradeName", $("#ModalGrade").find("option:selected").text());
        formData.append("ReelName", $("#ModalReel").find("option:selected").text());
        formData.append("SubjectName", $("#ModalSubject").find("option:selected").text());
        formData.append("EditionName", $("#ModalEdition").find("option:selected").text());
        formData.append("PublishidName", $("#ModalPublish").find("option:selected").text());
        formData.append("BookName", $("#BookName").val());
        $.ajax({
            url: '/ResourceMgr/AddBook',
            type: 'POST',
            data: formData,
            async: false,
            cache: false,
            contentType: false,
            enctype: 'multipart/form-data',
            processData: false,
            success: function (data) {
                if (data.Success) {
                    bootbox.alert(data.Data);
                    $("#addModal").modal("hide");
                    $("#tb_resource").bootstrapTable("refresh");
                }
                else {
                    bootbox.alert(data.ErrorMsg);
                }
            }
        });
    });
});

var TableInit = function (jsonData) {
    var oTableInit = new Object();
    var i = 0;
    oTableInit.Init = function () {
        $('#tb_resource').bootstrapTable({
            url: '/ResourceMgr/Detail',
            method: 'post',
            toolbar: '#toolbar',
            striped: true,
            cache: false,
            pagination: true,
            sortable: true,
            sortName: "UpdateTime",
            sortOrder: "desc",
            queryParams: oTableInit.queryParams,
            sidePagination: "server",
            pageNumber: 1,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            search: false,
            strictSearch: true,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,
            clickToSelect: false,
            uniqueId: "BookID",
            showToggle: false,
            cardView: false,
            columns: [{
                checkbox: true
            }, {
                title: '序号',
                formatter: function (value, row, index) {
                    return index+1;
                }
            }, {
                field: 'BookName',
                title: '书本名称'
            }, {
                field: 'BookID',
                title: '书本ID'
            }, {
                field: 'CreateTime',
                sortable: true,
                title: '新建时间',
                formatter: function (value, row, index) {
                    return formatDate(row.CreateTime, 'YYYY-MM-dd');
                }
            }, {
                field: 'UpdateTime',
                title: '更新时间',
                sortable: true,
                formatter: function (value, row, index) {
                    return formatDate(row.UpdateTime, 'YYYY-MM-dd');
                }
            }, {
                field: 'Status',
                title: '状态',
                formatter: function (value, row, index) {
                    if (row.Status == 0) {
                        return "未启用";
                    }
                    if (row.Status == 1) {
                        return "启用";
                    }
                    if (row.Status == 2) {
                        return "禁用";
                    }
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    var action = "";
                    if (jsonData.Edit) {
                        action += " <a href=\"javascript:;\"  onclick='EditCourse(" + row.BookID + ")'>修改</a> ";
                    }
                    action += " <a href=\"javascript:;\"  onclick='AddResource(" + row.BookID + ")'>添加资源</a> ";
                    if (row.Status == 1) {
                        action += " <a href=\"javascript:;\"  onclick='StopCourse(" + row.BookID + ")'>禁用</a> ";
                    } else {
                        action += " <a href=\"javascript:;\"  onclick='StartCourse(" + row.BookID + ")'>启用</a> ";
                    }
                    if (jsonData.Del) {
                        action += " <a href=\"javascript:;\"  onclick='DelCourse(" + row.BookID + ")'>删除</a> ";
                    }
                    return action;
                }
            }]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {
            pagesize: params.limit,  //页面大小
            pageindex: params.offset, //页码
            sort: params.sort,
            order: params.order,
            Period: $("#Period").val(),
            Grade: $("#Grade").val(),
            Edition: $("#Edition").val(),
            Subject: $("#Subject").val(),
            Reel: $("#Reel").val(),
            Status: $("#Status").val(),
            Publish: $("#Publish").val()
        };
        return temp;
    };

    return oTableInit;
};

function AddResource(bookid) {
    window.location.href = "/ResourceMgrFiles/Index?bookid=" + bookid;
}

function AddCourse() {
    $("#ModalPeriod").val(-1);
    $("#ModalGrade").val(-1);
    $("#ModalReel").val(-1);
    $("#ModalSubject").val(-1);
    $("#ModalEdition").val(-1);
    $("#ModalPublish").val(-1);
    $("#ModalEPlate").val("");
    $("#ModalBookCatalog").val("");
    $("#ModalBookCover").val("");
    $("#myModalLabel").text("新建");
    $("#BookID").val("");
    $("#ModalStatus").val("");
    $("#BookName").val("");
    $('#addModal').modal('show');
}

function EditCourse(bookID) {
    $.post("/ResourceMgr/GetCourseByID",{bookID:bookID}, function (data) {
        if (data.Success) {
            var book = data.Data;
            $("#ModalPeriod").val(book.PeriodID);
            $("#ModalGrade").val(book.GradeID);
            $("#ModalReel").val(book.ReelID);
            $("#ModalSubject").val(book.SubjectID);
            $("#ModalEdition").val(book.EditionID);
            $("#ModalPublish").val(book.Publishid);
            $("#ModalEPlate").val(book.EPlate);
            $("#ModalBookCatalog").val("");
            $("#ModalBookCover").val("");
            $("#ModalStatus").val(book.Status);
            $("#myModalLabel").text("修改");
            $("#BookID").val(book.BookID);
            $("#BookName").val(book.BookName);
            $("#addModal").modal("show");
        }
        else {
            bootbox.alert(data.ErrorMsg);
        }
    });
}

function StartCourse(bookID) {
    $.post("/ResourceMgr/StartCourseByID", { bookID: bookID }, function (data) {
        if (data.Success) {
            bootbox.alert(data.Data);
            $("#tb_resource").bootstrapTable("refresh");
        }
        else {
            bootbox.alert(data.ErrorMsg);
        }
    });
}

function StopCourse(bookID) {
    $.post("/ResourceMgr/StopCourseByID", { bookID: bookID }, function (data) {
        if (data.Success) {
            bootbox.alert(data.Data);
            $("#tb_resource").bootstrapTable("refresh");
        }
        else {
            bootbox.alert(data.ErrorMsg);
        }
    });
}

function DelCourse(bookID) {
    bootbox.confirm("你确认要删除?", function (result) {
        if (result) {
            $.post("/ResourceMgr/DelCourseByID", { bookID: bookID }, function (data) {
                if (data.Success) {
                    bootbox.alert(data.Data);
                    $("#tb_resource").bootstrapTable("refresh");
                }
                else {
                    bootbox.alert(data.ErrorMsg);
                }
            });
        }
    }); 
}

function GetSelectBookIDs() {
    var selects = $('#tb_resource').bootstrapTable('getSelections');
    var bookIDs = '';
    for (var i = 0; i < selects.length; i++) {
        if (bookIDs) {
            bookIDs += "," + selects[i].BookID;
        } else {
            bookIDs += selects[i].BookID;
        }
    }
    return bookIDs;
}

function BatchStartCourse() {
    var bookIDs = GetSelectBookIDs();
    $.post("/ResourceMgr/BatchStartCourse", { bookIDs: bookIDs }, function (data) {
        if (data.Success) {
            bootbox.alert(data.Data);
            $("#tb_resource").bootstrapTable("refresh");
        }
        else {
            bootbox.alert(data.ErrorMsg);
        }
    });
}

function BatchStopCourse() {
    var bookIDs = GetSelectBookIDs();
    $.post("/ResourceMgr/BatchStopCourse", { bookIDs: bookIDs }, function (data) {
        if (data.Success) {
            bootbox.alert(data.Data);
            $("#tb_resource").bootstrapTable("refresh");
        }
        else {
            bootbox.alert(data.ErrorMsg);
        }
    });
}

function OpenCourseExcel() {
    window.open("/Upload/课程目录模板.xlsx", "_blank");
}

function BatchDelCourse() {
    bootbox.confirm("你确认要删除?", function (result) {
        if (result) {
            var bookIDs = GetSelectBookIDs();
            $.post("/ResourceMgr/BatchDelCourse", { bookIDs: bookIDs }, function (data) {
                if (data.Success) {
                    bootbox.alert(data.Data);
                    $("#tb_resource").bootstrapTable("refresh");
                }
                else {
                    bootbox.alert(data.ErrorMsg);
                }
            });
        }
    });
}

