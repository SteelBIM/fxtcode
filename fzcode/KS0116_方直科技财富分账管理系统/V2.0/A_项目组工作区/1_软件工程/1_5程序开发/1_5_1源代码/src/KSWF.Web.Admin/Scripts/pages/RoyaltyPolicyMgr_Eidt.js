
var ptype = getQueryString("t");
var RoyaltyId = getQueryString("Id");
var typename = "直销";
$(function () {
    $("#ProductName").attr("disabled", true);
    if (ptype == 1) {
        $(".page-header").html("编辑代理商商务策略");
        $("#tc").html("*销售折扣");
        typename = "代理";
        $("#ProportionComment").html("说明：销售折扣为代理商相关联订单的提成金额，班级奖励是相关订单有班级信息的额外提成比例");
    } else {
        ptype = 0;
    }
    if (RoyaltyId > 0) {
        $("#royaltypolicyid").val(RoyaltyId);
        $.post("/RoyaltyPolicyMgr/GetRoyaltyPolickById", { Id: RoyaltyId }, function (data) {
            if (data != "") {
                var jsondata = eval(data);
                $("#ProductName").val(jsondata.pid);
                $("#ProductRemark").val(jsondata.remark);
                var Prefix = $("#ProductName").find("option:selected").text() + "-" + typename + "-"
                $("#Prefix").html(Prefix);
                $("#StrategyName").val(jsondata.pllicyname.replace(Prefix,""));
                LoadVersionCategory(jsondata.pid);
                Loadable();
            }
        });
    }
    $table = $('#tb_proportions');
   

    var oTable = new TableInit();
    oTable.Init();
    if (ptype == 1)
        $("#policytypename").html("销售折扣");
    //根据产品加载分类、版本  1
    $("#ProductName").change(function () {
        $("#Version").empty();
        $("#Category").empty();
        $("#channelid").val("");
        var productid = $(this).find("option:selected").val();//产品id
        if (productid != "") {
            $("#StrategyName").val($(this).find("option:selected").text() + "-" + typename + "-");//策略名称
            $("#channelid").val(productid);//产品来源id
            if (productid > 0) {
                LoadVersionCategory(productid);//加载分类和版本 2
            }
        }
    });

    //加载分类和版本 2.1
    function LoadVersionCategory(productid) {
        $.post("/RoyaltyPolicyMgr/GetCategoryid", { productid: productid, versionid: 0 }, function (Categorydata) {
            if (Categorydata != "") {
                if (Categorydata != "") {
                    var jsondata = eval(Categorydata);
                    $("#Category").empty();
                    for (var a = 0; a < jsondata.length; a++) {
                        var opt2 = $("<option>").text(jsondata[a].category).val(jsondata[a].categorykey);
                        $("#Category").append(opt2);
                    }
                    if (jsondata.length > 0)
                    {
                        LoadVersion(productid, "");
                    }
                }
            } else {
                $("#Category").empty();
            }
        });
    }

    //加载版本 2.2
    function LoadVersion(productid, Category) {
        $.post("/RoyaltyPolicyMgr/GetVersion", { productid: productid, categoryid: Category }, function (response) {
            if (response != "") {
                var jsondata = eval(response);
                $("#Version").empty();
                $.each(eval(response), function (index, item) {
                    var opt = $("<option >").text(item.version).val(item.versionid);
                    $("#Version").append(opt);
                });
                if (jsondata.length > 0) {
                    GetProportion(jsondata[0].versionid);
                }
            } else {
                $("#Version").empty();
            }
        });
    }

    //点击分类 改变版本 3
    $("#Category").change(function () {
        LoadVersion($("#ProductName").val(), $(this).find("option:selected").val());
    });

    function GetProportion(VersionId) {
        $.post("/RoyaltyPolicyMgr/GetProportion", { Categoryid: $("#Category").val(), VersionId: VersionId, pid: $("#ProductName").val() }, function (response) {
            if (response != "") {
                var jsondata = eval(response);
                $("#ProportionHidden").val(jsondata.Proportion);
                $("#classProportionHidden").val(jsondata.classProportion);
            } else {
                $("#ProportionHidden").val("");
                $("#classProportionHidden").val("");
            }
        });
    }
    $("#Version").change(function () {
        GetProportion($(this).find("option:selected").val());
    });

    var data = [];
    var rowId = 0;
    //添加策略（改动，先添加比例）4
    $("#addroyalty").click(function () {
         
        var versionId = $("#Version").val();//版本id
        var versionName = $("#Version").find("option:selected").text();
        var categoryId = $("#Category").val();//分类id
        var categoryName = $("#Category").find("option:selected").text();

        var divided = $.trim($("#Proportion").val());//基础提成比例
        var class_divided = $.trim($("#classProportion").val());//班级奖励
        var ProportionHidden = $.trim($("#ProportionHidden").val());//基础提成比例最大值
        var classProportionHidden = $.trim($("#classProportionHidden").val());//班级奖励最大值
        if (divided == "" || divided == "0") {
            bootbox.alert("请填写基础提成比例！");
            return false;
        }

       
            if (!checkproportion(divided)) {
                bootbox.alert(comment + "取两位整数！");
                return false;
            }
        if (class_divided != "" && class_divided != "0") {
            if (!checkproportion(class_divided)) {
                bootbox.alert( "班级奖励取两位整数！");
                return false;
            }
        } else {
            class_divided = "0";
        }
        if (parseInt(divided) > parseInt(ProportionHidden)) {
            bootbox.alert(comment+"超过了" + ProportionHidden + "%的最大值！");
            return false;
        }
        if (parseInt(class_divided) > parseInt(classProportionHidden)) {
            bootbox.alert( "班级奖励超过了" + classProportionHidden + "%的最大值！");
            return false;
        }
        //获取产品ID 和 产品名称
        var productName = $("#ProductName").find("option:selected").text();
        var productId = $("#ProductName").val();

        if (productName == " ") {
            bootbox.alert("请选中产品！");
            return false;
        }
        var bf = categoryId + "fb" + versionId;
        var bf1 = "0fb" + versionId;
        var bf2 = categoryId + "fb0";
        var commission = $("#commission").val();
        if (commission != "") {
            if ((categoryId == 0 && versionId == 0) || commission.indexOf("0fb0") != -1) {//包含
                bootbox.alert("请勿重复选择分类及版本！");
                return false;
            }
            if (commission.indexOf(bf) != -1 || commission.indexOf(bf1) != -1 || commission.indexOf(bf2) != -1) {//包含
                bootbox.alert("请勿重复选择分类或版本！");
                return false;
            }
            if (categoryId == 0) {
                if (commission.indexOf("fb0") != -1) {
                    bootbox.alert("请勿重复选择分类！");
                    return false;
                }
            }
            if (versionId == 0) {
                if (commission.indexOf("0fb") != -1) {
                    bootbox.alert("请勿重复选择版本！");
                    return false;
                }
            }
        }
        commission += bf;
        $("#commission").val(commission);
       
        rowId += 1;
        //加载到表格
        data.push({
            rowId: rowId, productName: productName, productId: productId, categoryId: categoryId, categoryName: categoryName,
            versionId: versionId, versionName: versionName, divided: divided, class_divided: class_divided
        });
        $("#tb_proportions").bootstrapTable('load', data);
    });

    function Loadable() {
        //获取产品ID 和 产品名称
        var productName = $("#ProductName").find("option:selected").text();
        var productId = $("#ProductName").val();

        $.ajax({
            url: '/RoyaltyPolicyMgr/bpolicyproduct_View',
            traditional: true,
            data: { policyid: RoyaltyId, pagesize: 10000, pageindex: 0 },
            dataType: "json",
            type: "POST",
            success: function (jsondata) {
                if (jsondata != null && jsondata != "") {
                    var jd = eval(jsondata);
                    var commission = "";
                    $(eval(jd.rows)).each(function (i, item) {
                        commission += item.categorykey + "fb" + item.versionid;
                        rowId += 1;
                        data.push({
                            rowId: rowId, productName: productName, productId: productId, categoryId: item.categorykey, categoryName: item.category,
                            versionId: item.versionid, versionName: item.version, divided: (item.divided * 100).toFixed(2), class_divided: (item.class_divided * 100).toFixed(2)
                        });
                    });
                    $("#commission").val(commission);
                    $("#tb_proportions").bootstrapTable('load', data);
                }
            }
        });
    }

    //验证提成比例(两位正整数) 4.1
    function checkproportion(proportion) {
        var regu = /^[1-9][0-9]?$/;
        var re = new RegExp(regu);
        if (re.test(proportion)) {
            return true;
        } else {
            return false;
        }
    }

    //保存策略按钮点击事件 6
    $("#submitpolicty").click(function () {
        var bootstrapValidator = $("#RoyaltyForm").data('bootstrapValidator');
        bootstrapValidator.validate();
        if (!bootstrapValidator.isValid()) {
            return;
        }

        //6.1.2 验证是否有添加比例
        var tableData = $table.bootstrapTable('getData');
        if (tableData.length == 0) {
            bootbox.alert("请添加产品分成比例！");
            return false;
        }
        //bug 验证全部与子集的互斥提示（待完成）

        //indData的格式
        var cfg_bpolicyproducts = [];

        for (x in tableData) {
            var cfg_bpolicyproduct = {
                bid: RoyaltyId,
                categorykey: tableData[x].categoryId,
                versionid: tableData[x].versionId,
                divided: tableData[x].divided / 100,
                class_divided: tableData[x].class_divided / 100,
            };
            cfg_bpolicyproducts.push(cfg_bpolicyproduct);
        }

        var cfg_bpolicy = {
            bid: RoyaltyId, ptype: ptype, pllicyname: $("#Prefix").html()+$("#StrategyName").val(), pid: $("#ProductName").val(),
            remark: $.trim($("#ProductRemark").val())
        };

        var data = { cfg_bpolicy: cfg_bpolicy, cfg_bpolicyproducts: cfg_bpolicyproducts };
        //6.2 传数据到后台
        $.ajax({
            url: '/RoyaltyPolicyMgr/BpolicyEdit',
            traditional: true,
            data: { jsonData: JSON.stringify(data) },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Success) {
                    bootbox.alert(response.Data + "~", function (result) {
                        location.href = "/RoyaltyPolicyMgr/Index?t=" + ptype;
                    });
                } else {
                    bootbox.alert(response.ErrorMsg);
                }
            }
        });
    });

    //form 相关参数验证 6.1
    $("#RoyaltyForm").bootstrapValidator({
        message: 'This value is not valid',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            StrategyName: {
                message: '请输入1-30个文字的产品策略名称',//考虑是否用正则固定前缀（待完成）
                validators: {
                    notEmpty: { message: '请输入1-30个文字的产品策略名称' },
                    stringLength: {
                        min: 1,
                        max: 30,
                        message: '策略名称长度必须大于1且小于30个文字'
                    }
                }
            }, ProductName: {
                message: '请选择产品名称',
                validators: {
                    notEmpty: {
                        message: '请选择产品名称'
                    }
                }
            }
        }
    });
});

var TableInit = function () {
    var oTableInit = new Object();
    oTableInit.Init = function () {
        $('#tb_proportions').bootstrapTable({
            //url: '/RoyaltyPolicyMgr/bpolicyproduct_View',         //请求后台的URL（*）
            //method: 'post',                     //请求方式（*）
            toolbar: '#toolbar',                  //工具按钮用哪个容器
            striped: true,                        //是否显示行间隔色
            cache: false,                         //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: false,                     //是否显示分页（*）
            sortable: true,                       //是否启用排序
            sortOrder: "asc",                     //排序方式
            queryParams: oTableInit.queryParams,  //传递参数（*）
            sidePagination: "client",             //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                        //初始化加载第一页，默认第一页
            pageSize: 10,                         //每页的记录行数（*）
            pageList: [10, 25, 50, 100],          //可供选择的每页的行数（*）
            search: false,                        //是否显示表格搜索，此搜索是客户端搜索，不会进服务端
            strictSearch: true,
            showColumns: false,                   //是否显示所有的列
            showRefresh: false,                   //是否显示刷新按钮
            minimumCountColumns: 2,               //最少允许的列数
            clickToSelect: false,                 //是否启用点击选中行
            uniqueId: "rowId",                       //每一行的唯一标识，一般为主键列
            showToggle: false,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                      //是否显示详细视图
            detailView: false,                    //是否显示父子表
            columns: [{
                field: 'rowId',
                visible: false
            }, {
                field: 'productName',
                title: '产品'
            }, {
                field: 'productId',
                visible: false
            },
            {
                field: 'categoryName',
                title: '分类',
                formatter: function (value, row, index) {
                    if (value == 0)
                        return "全部";
                    return value;
                }
            }, {
                field: 'categoryId',
                visible: false
            }, {
                field: 'versionName',
                title: '版本',
                formatter: function (value, row, index) {
                    if (value == 0)
                        return "全部";
                    return value;
                }
            }, {
                title: "<span id=\"policytypename\">基础提成比例<span>",
                formatter: function (value, row, index) {
                    return row.divided  + "%";
                }
            }, {
                title: '班级奖励',
                formatter: function (value, row, index) {
                    return row.class_divided + "%";
                }
            }, {
                title: '操作',
                formatter: function (value, row, index) {
                    return "<a href=\"javascript:;\" onclick='Delete(" + row.rowId + "," + row.categoryId + "," + row.versionId + ")'>删除</a>";
                }
            }]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
            pagesize: params.limit,   //页面大小
            pageindex: params.offset, //页码
            //policyid: $("#royaltypolicyid").val()
        };
        return temp;
    };
    return oTableInit;
};
//删除 5
function Delete(rowId, categoryId, versionId) {
    bootbox.setDefaults("locale", "zh_CN");
    bootbox.confirm("确定删除~", function (result) {
        if (result) {
            $("#tb_proportions").bootstrapTable('remove', { field: 'rowId', values: [rowId] });
            $("#commission").val($("#commission").val().replace(categoryId + "fb" + versionId, ""));
        }
    });
}









