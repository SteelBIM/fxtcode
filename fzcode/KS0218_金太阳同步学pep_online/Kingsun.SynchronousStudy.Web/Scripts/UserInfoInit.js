var UserInfo = function () {
    var Current = this;
    var AppID = "";
    Current.Phone = "";
    this.Init = function () {
        Current.InitUserInfo();

        $("#divAddVersion").dialog({
            title: "开通会员",
            width: 400,
            height: 200,
            modal: true,
            closed: true
        });



        Common.Ajax("FeeSettingImplement", "QueryED", { PageIndex: 1, PageSize: 999, Where: "" }, function (data) {
            if (data.Success) {
                $("#type").combobox({
                    valueField: 'BookID'
                    , textField: 'TeachingNaterialName'
                    , width: 220
                    , panelHeight: 120
                    , data: data.Data
                    , onSelect: function (row) {

                    }
                })
                $("#type").combobox("select", data.Data[0].BookID);
            }

        })

        $("#kvipsbtn").click(function () {
            $("#divAddVersion").dialog("open");
        });


        $("#btnSearch").click(function () {

        });

        $("#save").click(function () {
            var type = $("#type").combobox("getValue")
            var phone = $("#phone").val();
            if (phone) {
                var info = {};
                info.UserID = phone,
                info.CourseID = type;
                info.Months = $("#months").val();
                Common.Ajax("FeeSettingImplement", "KVip", info, function (data) {
                    if (data.Success) {
                        alert("开通成功");
                        $("#divAddVersion").dialog("close");
                    }

                })

            } else {
                alert("请输入手机号码");
            }
        });

    };

    //加载App信息列表
    this.InitUserInfo = function () {
        $('#tbdatagrid').datagrid({
            pagination: true,
            rownumbers: true,
            //toolbar: "#divtoolbar",
            fitColumns: true,
            striped: true,
            singleSelect: true,
            columns: [[
                { field: 'UserName', title: '用户名', align: 'center', width: 15 },
                 { field: 'UserID', title: '用户ID', align: 'center', width: 15 },
                { field: 'NickName', title: '昵称', align: 'center', width: 15 },
                {
                    field: 'CreateTime', title: '注册时间', align: 'center', width: 15, formatter: function (value, rows) {
                        return rows.CreateTime;
                    }
                },
                 //{
                 //    field: 'GG', title: '操作', align: 'center', width: 30, formatter: function (value, rows) {
                 //        return '<a href="javascript:void(0)" onclick="userInit.Kvip(' + rows.UserID + ')">开通会员</a>&nbsp;';
                 //    }
                 //}
            ]],
            pagination: true,    //分页控件  
        });

        var p = $('#tbdatagrid').datagrid('getPager');
        p.pagination({
            pageSize: 20, //每页显示的记录条数，默认为10  
            pageList: [10, 20, 30, 40, 50], //可以设置每页记录条数的列表  
            beforePageText: '第', //页数文本框前显示的汉字  
            afterPageText: '页    共 {pages} 页',
            displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
            onRefresh: function (pageNumber, pageSize) {
                loadData(getWhere(), pageNumber, pageSize);
            },
            onSelectPage: function (pageNumber, pageSize) {
                loadData(getWhere(), pageNumber, pageSize);
            }
        });
        loadData(getWhere(), 1, 20);
        //搜索框提示
        $("#txtSearchKey").focus(function () {
            var txtbox = $("#txtSearchKey");
            if (txtbox.val() == "请输入要查询的用户名或者手机号") {
                $("#txtSearchKey").val("");
            }
        }).blur(function () {
            var txtbox = $("#txtSearchKey");
            if ($.trim(txtbox.val()) == "") {
                txtbox.val("请输入要查询的用户名或者手机号");
            }
        });

        //搜索按钮事件
        $("#btnSearch").click(function () {
            loadData(getWhere(), 1, 20);
        });
    }


}

//获取分页数据并且绑定到表格
function loadData(WhereCondition, CurrentPageIndex, PageSize) {

    if (WhereCondition == "" || WhereCondition == undefined) {
        WhereCondition = "123456789";
    }
    Feemanager.QueryUserInfo(WhereCondition, PageSize, CurrentPageIndex, function (data) {
        if (data.Data == null || data.Data == undefined) {
            data.Data = { total: 0, rows: [] };
        }
        $("#tbdatagrid").datagrid("loadData", data.Data);
    });
}

function getWhere() {
    var key = $("#txtSearchKey").val();
    var StartDate = $("#txtStartDate").val();
    var EndDate = $("#txtEndDate").val();
    var ddlVersion = $("#ddlVersion").val();
    var where = "1=1";
    //if (key == "请输入要查询的用户名或者手机号" || key == "" || key == undefined) {
    //    where += ",";
    //}
    //else {
    //    var errMsg = Common.ValidateTxt(key);
    //    if (errMsg != "" && errMsg != undefined) {
    //        alert("输入的搜索条件有误，提示：" + errMsg);
    //        return;
    //    }
    //    else {
    if (key != "请输入要查询的用户名或者手机号" && key != "" && key != undefined) {
        where += "," + " AND UserName like '%" + key + "%' or TelePhone like '%" + key + "%'";
    }
    //  }
    //}
    if (ddlVersion == "1") {
        where += ", AND AppId = '0a94ceaf-8747-4266-bc05-ed8ae2e7e410'";
        where += ", AND AppId = '1548d0a3-ca8e-4702-9c2c-f0ba0cacd385'";
        where += ", AND AppId = '241ea176-fce7-4bd7-a65f-a7978aac1cd2'";
        where += ", AND AppId = '37ca795d-42a6-4117-84f3-f4f856e03c62'";
        where += ", AND AppId = '41efcd18-ad8c-4585-8b6c-e7b61f49914c'";
        where += ", AND AppId = '43716a9b-7ade-4137-bdc4-6362c9e1c999'";
        where += ", AND AppId = '5373bbc9-49d4-47df-b5b5-ae196dc23d6d'";
        where += ", AND AppId = '64a8de22-cea0-4026-ab36-5a70f94dd6e4'";
        where += ", AND AppId = '333d7cfc-cb4f-49d2-8ded-025e7d0fe766'";
        where += ", AND AppId = '8170b2bf-82a8-4c2d-9458-ae9d43cac5e3'";
        where += ", AND AppId = '9426808e-da8e-488c-9827-b082c19b62a7'";
        where += ", AND AppId = 'f0a9e1a7-b4cf-4a37-8fd1-932a66070afa'";
    }
    //北京版
    if (ddlVersion == "0a94ceaf-8747-4266-bc05-ed8ae2e7e410") {
        where += ", AND AppId = '0a94ceaf-8747-4266-bc05-ed8ae2e7e410'";
    }
    //广州版
    if (ddlVersion == "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385") {
        where += ", AND AppId = '1548d0a3-ca8e-4702-9c2c-f0ba0cacd385'";
    }
    //牛津深圳版
    if (ddlVersion == "241ea176-fce7-4bd7-a65f-a7978aac1cd2") {
        where += ", AND AppId = '241ea176-fce7-4bd7-a65f-a7978aac1cd2'";
    }
    //广东版
    if (ddlVersion == "37ca795d-42a6-4117-84f3-f4f856e03c62") {
        where += ", AND AppId = '37ca795d-42a6-4117-84f3-f4f856e03c62'";
    }
    //新课标标准实验版
    if (ddlVersion == "41efcd18-ad8c-4585-8b6c-e7b61f49914c") {
        where += ", AND AppId = '41efcd18-ad8c-4585-8b6c-e7b61f49914c'";
    }
    //牛津上海本地版
    if (ddlVersion == "43716a9b-7ade-4137-bdc4-6362c9e1c999") {
        where += ", AND AppId = '43716a9b-7ade-4137-bdc4-6362c9e1c999'";
    }
    //人教PEP版
    if (ddlVersion == "5373bbc9-49d4-47df-b5b5-ae196dc23d6d") {
        where += ", AND AppId = '5373bbc9-49d4-47df-b5b5-ae196dc23d6d'";
    }
    //人教版新起点
    if (ddlVersion == "64a8de22-cea0-4026-ab36-5a70f94dd6e4") {
        where += ", AND AppId = '64a8de22-cea0-4026-ab36-5a70f94dd6e4'";
    }
    //江苏译林
    if (ddlVersion == "333d7cfc-cb4f-49d2-8ded-025e7d0fe766") {
        where += ", AND AppId = '333d7cfc-cb4f-49d2-8ded-025e7d0fe766'";
    }
    //人教版
    if (ddlVersion == "8170b2bf-82a8-4c2d-9458-ae9d43cac5e3") {
        where += ", AND AppId = '8170b2bf-82a8-4c2d-9458-ae9d43cac5e3'";
    }
    //牛津上海全国版
    if (ddlVersion == "9426808e-da8e-488c-9827-b082c19b62a7") {
        where += ", AND AppId = '9426808e-da8e-488c-9827-b082c19b62a7'";
    }
    //山东版
    if (ddlVersion == "f0a9e1a7-b4cf-4a37-8fd1-932a66070afa") {
        where += ", AND AppId = 'f0a9e1a7-b4cf-4a37-8fd1-932a66070afa'";
    }
    if (StartDate != "" && EndDate != "") {
        where += ",  AND CreateTime >= '" + StartDate + "' AND CreateTime<='" + EndDate + "' ";
    } else if (StartDate != "") {
        where += ",   AND CreateTime >= '" + StartDate + "' ";
    } else if (EndDate != "") {
        where += ",  AND CreateTime<='" + EndDate.Value + "' ";
    }
    return where;
}


var userInit;
$(function () {
    userInit = new UserInfo();
    userInit.Init();
});