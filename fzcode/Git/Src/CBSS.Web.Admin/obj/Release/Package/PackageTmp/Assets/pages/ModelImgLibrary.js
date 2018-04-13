//初始化数据
var ModelImgLibraryTableInit = function () { 
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function (ModelID) { 
        $('#tb_roletable').bootstrapTable({
            url: '/Tbx/ModelImgLibrary/GetModelImgLibraryPage',         //请求后台的URL（*）
            method: 'post',                     //请求方式（*）
            //toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: true,                    //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: false,                  //是否显示所有的列
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: false,                //是否启用点击选中行
            //height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "ID",                 //每一行的唯一标识，一般为主键列
            showToggle: false,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                checkbox: true
            }, {
                field: 'ModelImgLibraryID',
                visible: false
            }, {
                field: 'ImgName',
                title: '图片名称'
            }, {
                field: 'ImgType',
                title: '图片格式',
                formatter: function (value, row, index) { 
                    if (value == 1) {
                        return "静态图";
                    } else if (value==2) {
                        return "动态图";
                    } else {
                        return "序列图";
                    }
                }
            }, {
                field: 'ImgPath',
                title: '图片',
                formatter: function (value, row, index)
                {
                    return "<a href='" + row.ImgPath + "' target='_blank'><Img src='" + row.ImgPath + "?x-oss-process=image/resize,m_fill,h_100,w_100' style='width: 100px;'/></a>";
                }
            }, {
                field: 'CreateDate',
                title: '发布时间',
                formatter: function (value, row, index) {
                    return formatDate(row.CreateDate,"YYYY-MM-dd HH:mm:ss");
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) { 
                        return "<a class='btn mini green thickbox' title='编辑图片' onclick=\"tb_show('编辑图片','/Tbx/ModelImgLibrary/Edit/" + row.ModelImgLibraryID + "?TB_iframe=true&amp;height=520&amp;width=800',false)\"  ><i class='icon-edit'> </i> 编辑图片  </a >";
                }
            }]
        });
    };
    removecloud();
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset, //页码 
            ModelID: ModelID
        };
        return temp;
    };
    return oTableInit;
};
//删除
$("#deletes").click(function () {
    var selects = $('#tb_roletable').bootstrapTable('getSelections');
    var ids = '';
    for (var i = 0; i < selects.length; i++) {
        if (ids) {
            ids += "," + selects[i].ModelImgLibraryID;
        } else {
            ids += selects[i].ModelImgLibraryID;
        }
    } 
    if (ids != "") {
        bootbox.setDefaults("locale", "zh_CN");
        bootbox.confirm("确定删除~", function (result) {
            if (result) {
                $.post("/Tbx/ModelImgLibrary/Delete", { ids: ids }, function (data) {
                    if (data > 0) {
                        //$('#tb_roletable').bootstrapTable("refresh");
                        $('#tb_roletable').bootstrapTable('selectPage', 1);
                    }
                    else {
                        bootbox.alert("删除失败，请重试~");
                    }
                });
            }
        });
    } else {
        bootbox.alert("请至少选择一项~");
    }
}); 