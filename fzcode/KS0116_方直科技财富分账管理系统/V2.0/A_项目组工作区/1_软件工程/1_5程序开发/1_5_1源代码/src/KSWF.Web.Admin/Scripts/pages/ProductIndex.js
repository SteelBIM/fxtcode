/// <reference path="../../Template/vendor/jquery/jquery.min.js" />
/// <reference path="../CommonMetaData.js" />

var ProductPage = function () {
    var Current = this;
    this.DefaultCatogory;
    this.DefaultChannel;
    this.Init = function () {
        Current.TabInit();//初始化表格
        Current.InitSelectValue();
        $("#selProductChannel").change();
        $("#btn_add").click(function () {
            $("#myModal .modal-title").text("添加产品");
            Current.ClearInput();
            $("#hidproduct").val("");
            $("#myModal").modal("show");
            $("#formedit select").change();
        });

        $("#btn_edit").click(function () {
            Current.ClearInput();
            var selects = $('#tb_products').bootstrapTable('getSelections')[0];
            if (selects) {
                $("#hidproduct").val(selects.id);
                $("#myModal .modal-title").text("修改产品");
                $("#selProductChannel").val(selects.channel).change();
                $("#selSUB").val(selects.subjectid).change();
                $("#selED").val(selects.versionid).change();
                $("#selGRADE").val(selects.gradeid).change();
                $("#selCategory").val(selects.category).change();
                $("#txtProductName").val(selects.productname);
                $("#txtProductNO").val(selects.productno);
                $("#myModal").modal("show");
            }
            else {
                alert("请选择一个修改的对象");
            }
        })

        //提交信息保存
        $("#editBtn").click(function () {
            var obj = $("#formedit").serialize();
            $.post("/Product/SubmitData", obj, function (data) {
                if (data.Success) {
                    $("#myModal").modal("hide");
                    $('#tb_products').bootstrapTable("refresh");
                }
                else {
                    alert(data.ErrorMsg);
                }
            })
        })

        $("#btn_search").click(function () {
            //$('#tb_products').bootstrapTable("refresh");
            $('#tb_products').bootstrapTable('selectPage', 1);
        })


    }

    this.DeleteProduct = function () {
        var selects = $('#tb_products').bootstrapTable('getSelections');
        var ids = '';
        for (var i = 0; i < selects.length; i++) {
            if (ids) {
                ids += "," + selects[i].id;
            } else {
                ids += selects[i].id;
            }
        }
        if (ids.length > 0) {
            if (confirm("确认删除这些记录吗？")) {
                var obj = { IDS: ids };
                $.post("/Product/DeleteProduct", obj, function (data) {
                    if (data.Success) {
                        $('#tb_products').bootstrapTable("refresh");
                    }
                    else {
                        alert(data.ErrorMsg);
                    }
                })
            }
        }
    }

    this.ClearInput = function () {
        $("#formedit input").val('');
        $("#formedit select").prop("selectedIndex", 0);
        // $('select').prop('selectedIndex', 0);

    }

    this.InitSelectValue = function () {
        var gbcode = 'SUB,ED,GRADE';
        var sendValues = gbcode.split(",");
        Common.GetMetaData(gbcode, function (data) {
            $.each(data, function (index) {
                var valueKey = index;
                if (data[valueKey]) {
                    var eleID = "#sel" + valueKey;
                    for (var i = 0; i < data[valueKey].length; i++) {
                        var html = '<option value="' + data[valueKey][i].ID + '">' + data[valueKey][i].CodeName + '</option>';
                        $(html).appendTo(eleID);
                    }
                    $(eleID).change();
                }
            });
        })

        ///初始化选择来源之后改变后面的选择项
        $("#selproductchannel").change(function () {
            var id = $(this).val()
            var cid = 0;
            var obj = { Channel: id, CategoryKey: cid };
            $.post("/Product/GetSearchValue", obj, function (data) {
                if (data.Data.CatogoryList) {
                    var TData = data.Data.CatogoryList;
                    $("#selcategory").html('<option value="">类别</option>');
                    for (var i = 0; i < TData.length; i++) {
                        var html = '<option value="' + TData[i].Key + '">' + TData[i].Value + '</option>';
                        $(html).appendTo("#selcategory");
                    }
                }
                if (data.Data.SubjectList) {
                    var TData = data.Data.SubjectList;
                    $("#selectSUB").html('<option value="">科目</option>');
                    for (var i = 0; i < TData.length; i++) {
                        var html = '<option value="' + TData[i].Key + '">' + TData[i].Value + '</option>';
                        $(html).appendTo("#selectSUB");
                    }
                }
                if (data.Data.GradeList) {
                    var TData = data.Data.GradeList;
                    $("#selectGRADE").html('<option value="">年级</option>');
                    for (var i = 0; i < TData.length; i++) {
                        var html = '<option value="' + TData[i].Key + '">' + TData[i].Value + '</option>';
                        $(html).appendTo("#selectGRADE");
                    }
                }
                if (data.Data.VersionList) {
                    var TData = data.Data.VersionList;
                    $("#selectED").html('<option value="">版本</option>');
                    for (var i = 0; i < TData.length; i++) {
                        var html = '<option value="' + TData[i].Key + '">' + TData[i].Value + '</option>';
                        $(html).appendTo("#selectED");
                    }
                }
            })
        })

        $("#selcategory").change(function () {
            var chanelid = $("#selproductchannel").val();
            var categoryid = $(this).val();
            var obj = { Channel: chanelid, CategoryKey: categoryid };
            $.post("/Product/GetSearchValue", obj, function (data) {
                if (data.Data.VersionList) {
                    var TData = data.Data.VersionList;
                    $("#selectED").html('<option value="">版本</option>');
                    for (var i = 0; i < TData.length; i++) {
                        var html = '<option value="' + TData[i].Key + '">' + TData[i].Value + '</option>';
                        $(html).appendTo("#selectED");
                    }
                }
            });
        })


        ///初始化编辑框内渠道选择框
        $("#selProductChannel").change(function () {
            var id = $(this).val()
            var obj = { Channel: id, CategoryKey: 0 };
            $.post("/Product/GetSearchValue", obj, function (data) {
                if (data.Data.CatogoryList) {
                    var TData = data.Data.CatogoryList;
                    $("#selCategory").html('');
                    for (var i = 0; i < TData.length; i++) {
                        var html = '<option value="' + TData[i].Key + '">' + TData[i].Value + '</option>';
                        $(html).appendTo("#selCategory");
                    }
                }
            });
        })

        ///编辑弹窗选择控件内容上传
        $("#formedit select").change(function () {
            var v = $(this).find("option:selected").text();
            var ele = $(this).next(".hidname");
            if (ele.length > 0) {
                $(ele).val(v);
            }
        })
    }

    this.TabInit = function () {
        $('#tb_products').bootstrapTable({
            url: '/Product/CompDepart_ProductPageList',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: Current.GetParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            // search: true,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: false,                  //是否显示所有的列
            showRefresh:false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
           // height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度    设置之后表头内容无法对齐。
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [ {
                field: 'id',
                title: '序号',
                formatter: function (value, row, index) {
                    return index + 1;
                }
            }, {
                field: 'channel',
                title: '产品来源',
                formatter: function (value, row, index) {
                    if (value != 0) {
                        if (value > 0) {
                            var v = Current.GetChannelByKey(value);
                            return v.Value;
                        }
                    }
                }
            }, {
                field: 'subject',
                title: '科目'
            }, {
                field: 'version',
                title: '版本'
            }, {
                field: 'grade',
                title: '学段'
            }, {
                field: 'categorykey',
                title: '类别',
                formatter: function (value, row, index) {
                    if (value != 0) {
                        var v = Current.GetCatogoryByKey(value);
                        return v.Value;
                    }
                }
            }, {
                field: 'productname',
                title: '商品名称'
            }
            , {
                field: 'productno',
                title: '业务系统商品编号'
            }
            ]
        });
    }

    this.GetParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset  //页码
            , SubjectID: $("#selectSUB").val()
            , VersionID: $("#selectED").val()
            , GradeID: $("#selectGRADE").val()
            , CategoryKey: $("#selcategory").val()
            , Channel: $("#selproductchannel").val()
        };
        return temp;
    }

    this.GetChannelByKey = function (key) {
        for (var i = 0; i < Current.DefaultChannel.length; i++) {
            if (Current.DefaultChannel[i].Key == key) {
                return Current.DefaultChannel[i];
            }
        }
    }

    this.GetCatogoryByKey = function (key) {
        for (var i = 0; i < Current.DefaultCatogory.length; i++) {
            if (Current.DefaultCatogory[i].Key == key) {
                return Current.DefaultCatogory[i];
            }
        }
    }
}