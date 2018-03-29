/**
作者:李晓东
摘要:2013.12.27新建 Nav导航
2013.12.30 对Nav() 中弹出窗口进行调整
2014.02.13 修改人:李晓东
           修改 Nav() 对横向菜单的控制
2014.02.18 修改人:李晓东
           新增:在导入中增加对 "押品管理"的操作
           修改:Nav()函数
                在导出中修改对 "押品管理"的操作
2014.02.26 修改人:贺黎亮
           新增:在导出中增加对 "地图查询统计导出"的操作
2014.04.01 修改人:贺黎亮
           新增:在导出中增加对 "押品分类统计导出"的操作
           新增：押品明细导出
2014.04.24 修改人:贺黎亮
           新增 押品资产价值动态监测 导出
           新增 押品资产价值动态监测--分类 导出
2014.05.06 修改人:李晓东
           新增:GetChecked
2014.06.17 修改人:曹青
           新增:增加头部用户导航、导入、导出移动到右边
2014.06.18 修改人:曹青
           修改:用户菜单时间绑定
**/
jQuery(document).ready(function () {
    //App.init(); // initlayout and core plugins
    App.initSidebarMenu();
    //Tasks.initDashboardWidget();
    Nav();
    /*菜单切换颜色*/
    $('ul.page-sidebar-menu>li:not(:first) ul.sub-menu>li').each(function () {
        $(this).click(function () {
            if ($(this).parent().parent().hasClass('open')) {
                SetActiveOrNoActive($(this));
            }
        });

    });
    function SetActiveOrNoActive(obj) {
        $('ul.page-sidebar-menu>li:not(:first) ul.sub-menu>li').each(function () {
            $(this).removeClass('active');
        });
        obj.addClass('active');
    }
});
var mainContent = $("#main_content"), parentDialog = $("#customDialog");
var navtimeseend = 20000; //2was0秒刷新
var navmsgtime = null;
function Nav() {
    var myFile = null;
    $.each($(".page-sidebar-menu").find('a'), function (i, v) {
        $(this).live('click', function () {
            var href = $(this).attr('url'); //地址
            $("#ctitle").text($(this).attr('name')); //面包屑
            switch (parseInt($(this).attr('number'))) {//是否需要导入显示
                case 0:
                    myFile.show();
                    break;
                default:
                    if (myFile != null)
                        myFile.show();
                    break;
            }
            var cparent = $("#navparent");
            if ($(this).attr('data-parent') == 'true') {
                cparent.show();
                var cparenttitle = $(this).parent().parent().parent().find('.title').text();
                cparent.find('a').text(cparenttitle);
            }
            else
                cparent.hide();
            if (href != undefined) {
                mainContent.data('data', { url: href });
                mainContent.extendLoad({ url: href });
            }
        });
    });
    $('.pull-left>li>a').each(function () {
        var li = $(this), attr = li.attr('data-id');
        if (attr != undefined) {
            if (attr == 'myfile') {
                li.click(function () {
                    parentDialog.html('加载中...');
                    parentDialog.extendDialog({
                        title: '文件列表',
                        minWidth: 750,
                        maxHeight: 600,
                        open: function (event, ui) {
                            $(this).extendLoad({ url: "/Upload/Index" });
                            $(this).css({ 'text-align': '' });
                        },
                        close: function (event, ui) {
                        }
                    });
                });
                myFile = li.parent();
                myFile.show();
            }
        }
    });
    //右边导航
    $('.pull-right>li>a').each(function () {
        var li = $(this), attr = li.attr('data-id');
        if (attr != undefined) {
            if (attr == 'down') {//导出
                li.click(function () {
                    var data = mainContent.data('data');
                    if (data == undefined) return false;
                    /*================押品管理,"进行中"的导出=======================*/
                    if (data.url == '/Collateral/') {
                        var sel_li = mainContent.find('.nav-tabs>li.active');
                        if (sel_li.attr('class') != "" && sel_li.attr('class') == "active") {
                            var divselect = $('.tab-content>.in', mainContent);
                            if (divselect.hasClass('filedown')) {//一定是要指定导出项
                                var dataRow = [],
                                    table = $('table', divselect),
                                    downurl = table.attr('data-downurl');
                                if (downurl == undefined) return false;

                                /*=========押品管理,进行中的导出=========*/
                                if (downurl == '/Upload/ExcelDown') {
                                    var tbtr = $('tbody tr', table);

                                    var currentDown = $('<a class="btn green" id="cur">当前页</a>'),
                                        allDown = $('<a class="btn green" id="all">全部</a>'),
                                        callDiv = $('<div></div>');

                                    callDiv.append(currentDown);
                                    callDiv.append('&nbsp;');
                                    callDiv.append(allDown);

                                    $.extendDialog({
                                        title: '提示',
                                        content: callDiv.html(),
                                        minWidth: 200,
                                        buttons: null,
                                        open: function () {
                                            var curDialog = $(this);
                                            $(this).css({ 'text-align': 'center' });
                                            if (cfPage.pageIndex == 0) {
                                                $('a#cur', curDialog).hide();
                                            } else {
                                                $('a#cur', curDialog).show();
                                            }
                                            $('a.btn', curDialog).click(function () {
                                                var id = $.trim($(this).attr('id'));
                                                if (ExcuteColumnData == undefined || ExcuteColumnData == null)
                                                    ExcuteColumnData = GetExcelData();
                                                //console.log(ExcuteColumnData);
                                                /*当前导出*/
                                                if (id == 'cur') {
                                                    tbtr.each(function (i) {/*组装JSON*/
                                                        dataRow = $(this).extendRowData({ nameTohead: true });
                                                        return false;
                                                    });
                                                    if (ExcuteColumnData != null && !jQuery.isEmptyObject(dataRow)) {
                                                        NavDown(curDialog, downurl,
                                                            {
                                                                data: ExcuteColumnData,
                                                                pageSize: cfPage.pageSize,
                                                                pageIndex: cfPage.pageIndex,
                                                                type: 0,
                                                                column: JSON.stringify(dataRow),
                                                                fileId: $.CollateralId
                                                            });
                                                    }
                                                    else {
                                                        curDialog.dialog('close');
                                                        $.extendDialog({
                                                            title: '提示',
                                                            content: '无任何导出信息!'
                                                        });
                                                    }
                                                }/*全部导出*/
                                                else if (id == 'all') {
                                                    tbtr.each(function (i) {/*组装JSON*/
                                                        dataRow = $(this).extendRowData({ nameTohead: true });
                                                        return false;
                                                    });
                                                    /*要进行标准化的列信息是否为空*/
                                                    if (ExcuteColumnData != null && !jQuery.isEmptyObject(dataRow)) {
                                                        NavDown(curDialog, downurl,
                                                        {
                                                            data: ExcuteColumnData,
                                                            pageSize: 0,
                                                            pageIndex: 0,
                                                            type: 0,
                                                            column: JSON.stringify(dataRow),
                                                            fileId: $.CollateralId
                                                        });
                                                    }
                                                }
                                            });
                                        }
                                    });

                                }/*==========押品管理,已标准化的导出========*/
                                else if (downurl == '/Upload/ExcelAllDown') {
                                    NavDown(null, downurl);
                                }
                            }
                        }
                    } else if (data.url == "/CollateralDetect/" ||
                        data.url == "/AssetsCollateralDetect/")/*押品监测 || 押品资产价值动态监测 */ {
                        var sel_li = mainContent.find('.nav-tabs>li.active');
                        if (sel_li.attr('class') != "" && sel_li.attr('class') == "active") {
                            var divselect = $('a', sel_li).attr("href");
                            if (divselect == "#maps") {/*地图查询统计导出*/
                                var _ckbox = $('input[type=checkbox]', _detials), _detials = $('#colldetials'), sendData = GetChecked(_ckbox),
                                    roc = GetRadioOrCheckboxCity('r');
                                if (roc.val == '') return;
                                var _mapseletarr = [], _mapselecttypeArr = [], allmapselearr = [], mapselecttypeArr = [];
                                $("input[name=mapselecttype]").each(function (i, item) {
                                    if ($(item).attr("checked") == "checked") {
                                        _mapseletarr.push($(item).attr("val"));
                                        _mapselecttypeArr.push($(item).val());
                                    }
                                    allmapselearr.push($(item).attr("val"));
                                    mapselecttypeArr.push($(item).val());
                                });
                                if (_mapseletarr.length <= 0) {
                                    _mapseletarr = allmapselearr;
                                    _mapselecttypeArr = mapselecttypeArr;
                                }
                                NavDown(null, "/Upload/MapExcelAllDown", {
                                    cid: roc.val,
                                    daikuantype: sendData.daikuantype,
                                    jianzhutype: sendData.jianzhutype,
                                    mapselecttype: _mapselecttypeArr.join(","),
                                    mianjitype: sendData.mianjitype,
                                    niandaitype: sendData.niandaitype,
                                    nianlingtype: sendData.nianlingtype,
                                    wuyetype: sendData.wuyetype,
                                    type: _mapseletarr.join(",")
                                });
                            }
                            else if (divselect == "#classification") {/*押品统计分类*/

                                var _active = $("ul.nav-pills>li.active", $('#classification')), conturl = "/Upload/CFCountAllDown";
                                var requirement = '{0}&&{1}'.format(_active.attr('data-index'), _active.attr('data-val'));
                                if (data.url == "/AssetsCollateralDetect/") {
                                    conturl = "/Upload/AssetsCFCountAllDown";
                                }
                                NavDown(null, conturl, {
                                    requirement: requirement,
                                    requirename: _active.text()
                                });
                            }
                            else if (divselect == "#colldetials") {/*押品明细查询*/
                                var _ckbox = $('input[type=checkbox]', $('#colldetials')), sendData = GetChecked(_ckbox);
                                $.each($('input[type=text]', $('#colldetials')), function (k, v) {
                                    var _ival = $(this).attr('data-val');
                                    _ival = _ival == undefined ? 0 : _ival;
                                    if (k == 0) {
                                        sendData['start'] = v.value;
                                    } else if (k == 1) {
                                        sendData['end'] = v.value;
                                    } else if (k == 2) {
                                        sendData['companyid'] = _ival;
                                    } else if (k == 3) {
                                        sendData['projectid'] = _ival;
                                    }
                                });
                                NavDown(null, "/Upload/CollDetialsAllDown", sendData);

                            } else if (divselect == '#tab_2') {/*复估风险分析*/
                                var data = {};
                                data['type'] = 1;
                                NavDown(null, "/CollateralReassessment/RiskAnalysis", data);
                            } else if (divselect == "#tab_3") {/*复估明细查询*/
                                var sobj = $("#" + divselect.replace('#', '')),
                                    _ckbox = $('input[type=checkbox]', sobj),
                                    data = GetChecked(_ckbox);
                                $('input[type=text]', sobj).each(function (k, v) {
                                    var id = $(this).attr('data-id');
                                    id = id == undefined ? 0 : id;
                                    if (k == 0)
                                        data['companyid'] = id;
                                    else
                                        data['projectid'] = id;
                                });
                                data['type'] = 1;

                                NavDown(null, "/CollateralReassessment/GetReassessmentDetails", data);
                            }
                        }
                    }
                    else if (data.url == '/CollateralStressTest/Index') {/*压力测试*/
                        var sel_li = mainContent.find('.nav-tabs>li.active');
                        /*获取选择项*/
                        if (sel_li.attr('class') != "" && sel_li.attr('class') == "active") {
                            var divselect = $('a', sel_li).attr("href");
                            if (divselect == "#tab_1") {/*押品价格走势分析*/
                                $("#tab_1>.nav-pills>li").each(function (k, v) {
                                    if ($(this).hasClass('active'))
                                        senddata['type'] = k;
                                });
                                senddata['down'] = true;
                                NavDown(null, "/CollateralStressTest/PriceTrend", senddata);
                            } else if (divselect == "#tab_2") {/*压力测试*/
                                senddata = yaliTest($("#tab_2"), $("#tab_2 button").parent().parent());
                                senddata['type'] = 1;
                                NavDown(null, "/CollateralStressTest/StressTest", senddata);
                            } else if (divselect == "#tab_3") {/*风险预警*/
                                var senddata = {};
                                senddata['typedown'] = 1;
                                senddata['type'] = -1;
                                $("#tab_3").find('.nav-pills li').each(function (k, v) {
                                    if ($(this).hasClass('active'))
                                        senddata['type'] = k;
                                });
                                if (senddata['type'] != -1)
                                    NavDown(null, "/CollateralStressTest/RiskWarning", senddata);
                            }
                        }
                    }
                });
            }
            if (attr == 'up') {//导入
                li.click(function (e) {
                    e.preventDefault();
                    var data = mainContent.data('data');
                    if (data == undefined) return false;
                    /*================押品管理,"进行中"的导入=======================*/
                    if (data.url == '/Collateral/') {
                        var sel_li = mainContent.find('.nav-tabs>li:eq(1)');
                        if (sel_li.attr('class') != "" && sel_li.attr('class') == "active") {
                            //console.log($.PageDisableId);
                            if ($.PageDisableId > 0 || $.PageDisableId == 0) {
                                /*重新绑定*/
                                var target = $('.tab-content>.fileup table', mainContent);
                                $.extendUp(function (eudata) {
                                    var targetid = target.attr('id'),
                                        savemessage = $(".tab-content>.fileup #savemessage", mainContent);
                                    $.ActionTmpl("templete", eudata.data, "{0}>tbody".format(targetid));
                                    if (!isNaN(eudata.pageIndex)) {
                                        cfPage.pageIndex = eudata.pageIndex;
                                        eudata['count'] = eudata.count;

                                        fPage(eudata, { url: '/Upload/fileExcute', data: GetExcelData(eudata.column) });
                                    }
                                    else {
                                        cfPage.pageIndex = 0;
                                        $('.tab-content>.fileup #page_info', mainContent).html('');
                                        $('.tab-content>.fileup .pagination', mainContent).html('');
                                    }
                                    /**自动保存当前的进行中的押品状态*/

                                    savemessage.html('正在保存当前状态....');
                                    setTimeout(function () {
                                        $.extendAjax({
                                            url: '/Collateral/SetFilePageIndex',
                                            type: 'post',
                                            data: { fileId: $.CollateralId, pageIndex: cfPage.pageIndex }
                                        }, function () {
                                            savemessage.html('已保存!');
                                            setTimeout(function () {
                                                savemessage.html('');
                                            }, 1000);
                                        });
                                    }, 1500);
                                    target.extendTable({
                                        edit: true,
                                        cancel: true,
                                        firstNumber: -1
                                    });;
                                });
                            } else {
                                showMessage();
                            }
                        }
                    }

                });
            }
        }
    });
    //用户导航
    var modifyPass = null;
    $('.user-nemu>li>a').each(function () {
        var li = $(this), attr = li.attr('data-id');
        if (attr != undefined) {
            //注销
            if (attr == "logout") {
                li.click(function () {
                    if (confirm("确认退出系统？")) {
                        $.ajax({
                            type: "post",
                            data: {},
                            url: "/Home/LoginOut",
                            dataType: "json",
                            success: function (rdata) {
                                window.location.href = "/Home/Login";
                            }
                        });
                    }
                });
            }
            //修改密码
            if (attr == "modifypass") {
                li.click(function () {
                    parentDialog.html('加载中...');
                    parentDialog.extendDialog({
                        title: '修改密码',
                        minWidth: 750,
                        maxHeight: 600,
                        open: function (event, ui) {
                            $(this).extendLoad({ url: "/Users/ModifyPassword" });
                            $(this).css({ 'text-align': '' });
                        },
                        close: function (event, ui) {
                        }
                    });
                });
                modifyPass = li.parent();
                modifyPass.show();
            }
        }
    });
    mainContent.extendLoad();
    TaskNav();
}

/* 任务导航*/
function TaskNav() {
    $("#taknav").click(function () {
        mainContent.data('data', { url: "/Task/Index" });
        mainContent.extendLoad({ url: "/Task/Index" });
    });
    CreateTask();
}

function showMessage() {
    $.extendDialog({
        title: '提示',
        content: '请执行要标准化的文件,再执行导入!'
    });
}
/*文件下载*/
function NavDown(cd, url, data) {
    //console.log(data);
    if (cd != null)
        cd.dialog('close');
    $.extendAjax({
        url: url,
        type: 'post',
        data: data != undefined ? data : ''
    }, function (rData) {
        if (rData != null && rData != '') {//不能为空
            var downs = $('#iframeDown'),
                url = '/Home/Download/?path={0}&file={1}'.format(rData.path, rData.name);

            if (downs.length == 0) {
                var down = $('<iframe id="iframeDown" style="display:none"></iframe>');
                down.attr('src', url);
                $('body').append(down);
            } else {
                downs.attr('src', url);
            }
        }
    });
}
/* 获得组装后需要拆分的信息 */
function GetExcelData(column) {
    var send = '', cArray = {
        "Number": "押品编号", "Branch": "分行", "PurposeCode": "押品类型",
        "Name": "押品名称", "BuildingArea": "面积", "Address": "押品地址"
    }, cdata;
    if (column != undefined) {
        cdata = $.parseJSON(column)
        for (var ca in cArray) {
            for (var c in cdata) {
                if (cdata[c] == cArray[ca]) {
                    send += '{0}={1}&'.format(ca, c);
                }
            }
        }
        send = "&{0}&type=0&filename={1}".format(send.substring(0, send.length - 1), Global_FileName);
    } else {
        var i = 0;
        for (var ca in cArray) {
            send += '{0}={1}&'.format(ca, i);
            i++;
        }
        send = "&{0}&type=0&filename={1}".format(send.substring(0, send.length - 1), Global_FileName);
    }
    return send;
}

/*获得指定值*/
function GetData(data, text) {
    for (var item in data) {
        if (data[item].ProvinceName != undefined &&
            ($.trim(data[item].ProvinceName) == $.trim(text) || $.trim(data[item].Alias) == $.trim(text))) {
            return data[item];
            break;
        } else if (data[item].CityName != undefined &&
            ($.trim(data[item].CityName) == $.trim(text) || $.trim(data[item].Alias) == $.trim(text))) {
            return data[item];
            break;
        }
        else if (data[item].AreaName != undefined && $.trim(data[item].AreaName) == $.trim(text)) {
            return data[item];
            break;
        }
    }
}

/*获得Checkbox中所选值*/
function GetChecked(_objdata) {
    var _data = {};
    $.each(_objdata, function (k, v) {
        if (_data[v.name] == undefined) {
            _data[v.name] = v.checked ? v.value : '';
        }
        else {
            if (v.checked) {
                if (_data[v.name] == '')
                    _data[v.name] = v.value;
                else
                    _data[v.name] = '{0},{1}'.format(_data[v.name], v.value);
            }
        }
    });
    return _data;
}

/*任务列表*/
function CreateTask() {
    clearTimeout(navmsgtime);
    $.extendAjax({
        url: '/Task/GetTaskList',
        type: 'post',
        data: { status: 0 },
        isBlockUI: false
    }, function (data) {
        if (data.type == 1) {
            var successcount = 0, failurecount = 0, _count = 0, tasktype = 0, bfb = 0, fid = 0;
            $("#tasknum").html((data.data.length > 0) ? data.data.length : "");
            $.ActionTmpl("task_tmpl", data.data, ".slimScrollDiv>ul");
            navmsgtime = setTimeout(CreateTask, navtimeseend);
        }
    });
}

/*项目列表*/
function CreateItemList(id, stype) {
    $.extendAjax({
        url: '/Projects/GetSysBankProjectList',
        type: 'post',
        data: {}
    }, function (data) {
        if (data.type == 1) {
            $.ActionTmpl("item_tmpl", data.data, id);
            App.init();
        }
        CityProcVal(id, "itemarrid");
        CreateCityList(id + "city", stype);
    });
}

/*任务状态 */
function ProceStatus(status, val) {
    switch (parseInt(status)) {
        case 0:
            return (val == 1) ? 2 : "暂停";
            break;
        case 2:
        case 3:
            return (val == 1) ? 0 : "启动";
            break;
        case 1:
            return (val == 1) ? 1 : "查看详情";
            break;
    }
}

/*城市列表*/
function CreateCityList(id, stype) {
    $.extendAjax({
        url: '/Projects/GetAppointCity',
        type: 'post',
        data: { status: 0 }
    }, function (data) {
        if (data.type == 1) {
            $.ActionTmpl("city_tmpl", data.data, id);
            if (stype == "singlecity") {
                $.ActionTmpl("singlecity_tmpl", data.data, ".singlecity");
                $(data.data).each(function (i, o) {
                    var dvspan = $(id + ">div[val='" + o.cityid + "']");
                    dvspan.data("dt", o);
                });
                CityProcVal(".singlecity", "mapcityarrid");
            }
            App.init();
            CityProcVal(id, "cityarrid");
            CityProcVal(".singlecity", "mapcityarrid");
        }
    });
}

/*城市，项目选择*/
function CityProcVal(id, cookie) {
    if ($.cookie(cookie) != null || id == ".singlecity") {
        if (id == ".singlecity") {
            if ($.cookie(cookie) == null || $.cookie(cookie) == undefined) {
                $(id + ">div").eq(0).find("span").addClass("checked");
                if ($(id + ">div").eq(0) != null) {
                    $.cookie("mapcityarrid", $(id + ">div").eq(0).attr("val"), { expires: 365, path: '/' });
                    $.cookie("mapcityname", $(id + ">div").eq(0).text().trim());
                }
            } else {
                var dvspan = $(id + ">div[val='" + $.cookie(cookie) + "']");
                if (dvspan.find("span").attr("class") != "checked") {
                    dvspan.find("span").addClass("checked");
                    $.cookie("mapcityname", $(id + ">div").eq(0).text().trim());
                }
            }
        }
        else {
            $($.cookie(cookie).split(",")).each(function (i, o) {
                var dvspan = $(id + ">div[val='" + o + "']");
                dvspan.find("span").addClass("checked");
            });
        }
    }
    $(id + ">div").click(function (e) {
        if ($(e.target).val() == "") {
            if ($(this).find("span").attr("class") == "checked") {
                $(this).find("span").removeClass("checked");
            } else {
                $(this).find("span").addClass('checked');
            }
        }
        if (id == ".singlecity") {
            $.cookie(cookie, $(this).attr("val"), { expires: 365, path: '/' });
        } else {
            var selcarr = [];
            $(id + ">div").each(function () {
                if ($(this).find("span").attr("class") == "checked") {
                    selcarr.push($(this).attr("val"));
                }
            });
            $.cookie(cookie, (selcarr.length > 0) ? selcarr.join(",") : "", { expires: 365, path: '/' });
        }

    });
}

/*获取Radio或者Checkbox的城市 此处请不要修改了!*/
function GetRadioOrCheckboxCity(type) {
    var cp = $("#cityorproject");
    if ($.trim(type) == 'r') {
        var radioObject = cp.find(".singlecity span.checked>input[type='radio']");
        return {
            val: radioObject.val() != undefined ? parseInt(radioObject.val()) : '',
            text: $.trim(radioObject.parent().parent().parent().text())
        };
    }
}

/*跳转*/
function RedireUrl(Url, data) {
    mainContent.data('data', { url: Url, data: data });
    mainContent.extendLoad({ url: Url, data: data });
}