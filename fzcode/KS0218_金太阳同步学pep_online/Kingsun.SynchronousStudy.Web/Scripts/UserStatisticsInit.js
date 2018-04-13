var UserStatisticsInit = function () {
    var Current = this;

    this.Init = function () {
        //$('#registerStartTime').datetimebox({
        //    //editable: false,
        //    showSeconds: true,
        //});
        //$('#registerEndTime').datetimebox({
        //    showSeconds: true
        //});
        //$('#lastLoginStartTime').datetimebox({
        //    showSeconds: true
        //});
        //$('#lastLoginEndTime').datetimebox({
        //    showSeconds: true
        //});
        Current.InitUserList();
    };

    //加载App信息列表
    this.InitUserList = function () {
        $('#tbdatagrid').datagrid({
            url: "/Handler/ModuleData.ashx?methodData=GetUserStatistics",
            queryParams: { sortname: "UserName", sortvalue: "asc" },
            pagination: true,
            rownumbers: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            pagesize: 10,
            pageList: [10, 20, 30, 40, 50],
            width: $(window).width() - 20,
            height: $(window).height() - 55,
            onSortColumn: function (sort, order) {
                loadlistgrid(sort, order);
            },
        });
    }

    //TODO:访问两次接口。
    function loadlistgrid(sortname, sortvalue) {
        var url = "/Handler/ModuleData.ashx?methodData=GetUserStatistics&sortname=" + sortname + "&sortvalue=" + sortvalue;
        $.ajax({
            cache: false,
            async: false,
            url: url,
            success: function (data) {
                data.total = data.rows.length;
                $("#dg").datagrid({
                    data: data
                });
            }
        });
    }

    //条件查询
    //this.Search = function () {
    //    var queryStr = "1=1";
    //    var userName = $.trim($("#userName").val());
    //    var email = $.trim($("#email").val());
    //    var telephone = $.trim($("#telephone").val());
    //    var loginTimes = $.trim($("#loginTimes").val());
    //    var registerStartTime = $('#registerStartTime').datetimebox('getValue');
    //    var registerEndTime = $('#registerEndTime').datetimebox('getValue');
    //    var lastLoginStartTime = $('#lastLoginStartTime').datetimebox('getValue');
    //    var lastLoginEndTime = $('#lastLoginEndTime').datetimebox('getValue');
    //    if (userName != "") {
    //        queryStr += " and UserName like '%" + userName + "%'";
    //    }
    //    if (email != "") {
    //        queryStr += " and Email like '%" + email + "%'";
    //    }
    //    if (telephone != "") {
    //        queryStr += " and Telephone like '%" + telephone + "%'";
    //    }
    //    if (loginTimes != "") {
    //        queryStr += " and StartTimes = " + loginTimes;
    //    }
    //    if (registerStartTime != '' && registerEndTime != '') {
    //        queryStr += " and RegistrationTime >= " + registerStartTime + " and RegistrationTime <= " + registerEndTime;
    //    }
    //    if (registerStartTime != '' && registerEndTime == '') {
    //        queryStr += " and RegistrationTime >= " + registerStartTime;
    //    }
    //    if (registerStartTime == '' && registerEndTime != '') {
    //        queryStr += " and RegistrationTime <= " + registerEndTime;
    //    }
    //    if (lastLoginStartTime != '' && lastLoginEndTime != '') {
    //        queryStr += " and LastLoginTime >= " + registerStartTime + " and LastLoginTime <= " + registerEndTime;
    //    }
    //    if (lastLoginStartTime != '' && lastLoginEndTime == '') {
    //        queryStr += " and LastLoginTime >= " + registerStartTime;
    //    }
    //    if (lastLoginStartTime == '' && lastLoginEndTime != '') {
    //        queryStr += " and LastLoginTime <= " + registerEndTime;
    //    }
    //    $('#tbdatagrid').datagrid({
    //        url: '?action=queryuserlist&queryStr=' + encodeURI(queryStr)
    //    });
    //}

}


var userStatisticsInit;
$(function () {
    userStatisticsInit = new UserStatisticsInit();
    userStatisticsInit.Init();
});