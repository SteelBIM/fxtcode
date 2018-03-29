var projectid = 0, buildingid = 0, floorno = 0, houseid = 0,
    projectname="",buildingname="",housename="",buildarea=0,address="";
    //getProejctList();
$(function () {
    //load

    $("#btnsearch").click(function () {
        getProejctList();
    });


    //    $("#projectlist").click(function () {
    //        $("#searchkey").focus();
    //        var ac_results = $(".ac_results");
    //        if (ac_results) {
    //            ac_results.show();
    //        }
    //    });

    $("#searchkey").live("keyup", function (e) {
        alert("0");
        var $this = $(this), len;
        len = $this.val().length;
        if (len == 0) {
            var parent = $this.parent()
            prev = parent.prev();
            $this.val(prev.text());
            if (prev.attr("rel") == "project") {
                $this.attr("rel", "project");
                autoComplete();
            } else if (prev.attr("rel") == "building") {
                $this.attr("rel", "building");
                autobuilding();
            } else if (prev.attr("rel") == "floorno") {
                $this.attr("rel", "floorno");
                autofloor();
            } else if (prev.attr("rel") == "housename") {
                $this.attr("rel", "housename");
                autohouse();
            }
            prev.replaceWith(parent);
        }
        len = $this.val().length;
        if (len > 0) {
            $this.css("width", len + "em").focus();
        } else {
            $this.css("width", len + "em").focus();
        }

    });
    //自动加载
    autoComplete();
    //修改
    $(".divnormal").live("click", function () {
        var divcount = $(this).prevAll().length;
        var searchkey = $("#searchkey");
        var prevalue = searchkey.val();
        var text = $(this).text();
        var next = -1;

        //调用数据
        switch (divcount) {
            case 0:
                searchkey.val(text).unautocomplete().width(text.length + "em");
                autoComplete();
                break;
            case 1:
                searchkey.val(text).unautocomplete().width(text.length + "em");
                autobuilding();
                break;
            case 2:
                searchkey.val(text).unautocomplete().width(text.length + "em");
                autofloor();
                break;
            case 3:
                searchkey.val(text).unautocomplete().width(text.length + "em");
                autohouse();
                break;
            default: break;
        }
        //还原原来的层
        if (searchkey.attr("rel") == "project") {
            searchkey.parent().before('<div class="divnormal" rel="project">' + prevalue + '</div>');
            if (prevalue.split("[")[0] != projectname.split("[")[0]) {
                removeSiblings(searchkey.parent());
            }
        } else if (searchkey.attr("rel") == "building") {
            searchkey.parent().before('<div class="divnormal" rel="building">' + prevalue + '</div>');
            if (prevalue != buildingname) {
                removeSiblings(searchkey.parent());
            }
        } else if (searchkey.attr("rel") == "floorno") {
            searchkey.parent().before('<div class="divnormal" rel="floorno">' + prevalue + '</div>');
            if (prevalue != floorno) {
                removeSiblings(searchkey.parent());
            }
        } else if (searchkey.attr("rel") == "housename") {
            searchkey.parent().before('<div class="divnormal" rel="housename">' + prevalue + '</div>');
        }

        $(this).replaceWith(searchkey.parent());
        searchkey.attr("rel", $(this).attr("rel")).focus();

    });
});
//removeSiblings
function removeSiblings(o) {
    var nextAll = o.nextAll();
    for (var i = 0, len = nextAll.length; i < len; i++) {
        var n = $(nextAll[i]);
        if (n.hasClass("divnormal")) { n.remove(); }
    }
}

//autocompany 基options
var baseoptions = {
    scrollHeight: 250,
    max: 15,
    matchContains: true,
    dataType: "json",
    scroll: true,
    divwidth: function () { var $elem = $("#projectlist"); return $elem.get(0).offsetWidth - 2; },
    divtop: function () { var $elem = $("#projectlist"); return $elem.offset().top + $elem.get(0).offsetHeight; },
    divleft: function () { }
}
//加载楼盘
function autoComplete() {
    var args = { type: "autoprojectlist",
        key: function () { return $("#searchkey").val().split("[")[0]; },
        cityid: CAS.Define.cityid,
        fxtcompanyid: CAS.Define.fxtcompanyid,
        wx: wx,
        parse: null,
        pageindex: 1,
        pagerecords: 10
    }

    var projectoptions = {
        extraParams: args,
        divisible: false,
        formatItem: function (row, i, max) {
            return row.projectname;
        },
        formatMatch: function (row, i, max) {
            return row.pinyin + " " + row.pinyinall;
        },
        formatResult: function (row) {
            return row.projectname;
        }
    }


    $("#searchkey").autocomplete(root + "/api/autoprice.ashx", $.extend({}, baseoptions, projectoptions)).result(function (event, data, formatted) {
        if (data.projectid == projectid) {
            //            $(this).parent().before('<div class="divnormal" rel="project">' + data.projectname + '</div>').insertAfter($(".divnormal:last"));
            //            $("#searchkey").val("").unautocomplete();
            return;
        }
        projectid = data.projectid; projectname = data.projectname; address = data.address;
        $(this).parent().before('<div class="divnormal" rel="project">' + data.projectname + '</div>')
        removeSiblings($(this).parent());
        $("#searchkey").val("").unautocomplete();

        autobuilding();

    });
}
//加载楼栋
function autobuilding() {
    var buildingoptions = {
        minChars: 0,
        divisible: true,
        formatItem: function (row, i, max) {
            return row.buildingname;
        },
        formatMatch: function (row, i, max) {
            return row.buildingname;
        },
        formatResult: function (row) {
            return row.buildingname;
        }
    };


    $.ajax({
        type: "POST",
        url: root + "/api/autoprice.ashx",
        data: { type: "autobuildinglist", projectid: projectid, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid, wx: wx },
        dataType: "json",
        success: function (data) {
            var vdata = data.data;
            $("#searchkey").autocomplete(vdata, $.extend({}, baseoptions, buildingoptions)).result(function (event, data, formatted) {
                if (data.buildingid == buildingid) {
//                    $(this).parent().before('<div class="divnormal" rel="building">' + data.buildingname + '</div>').insertAfter($(".divnormal:last"));
//                    $("#searchkey").val("").unautocomplete();
                    return;
                }
                $(this).parent().before('<div class="divnormal" rel="building">' + data.buildingname + '</div>')
                buildingid = data.buildingid; buildingname = data.buildingname;
                removeSiblings($(this).parent());
                $("#searchkey").val("").unautocomplete();
                autofloor();
            });
        },
        error: function () {//报错
            alert('抱歉网络异常，请稍后再重试');
        }
    });
}
//加载楼层
function autofloor() {
    var floornooptions = {
        minChars: 0,
        divisible: true,
        formatItem: function (row, i, max) {

            return row.floorno + "层";
        },
        formatMatch: function (row, i, max) {
            return row.floorno + "层";
        },
        formatResult: function (row) {
            return row.floorno + "层";
        }
    };

    $.ajax({
        type: "POST",
        url: root + "/api/autoprice.ashx",
        data: { type: "autofloorlist", buildingid: buildingid, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid, wx: wx },
        dataType: "json",
        success: function (data) {
            var vdata = data.data;
            $("#searchkey").autocomplete(vdata, $.extend({}, baseoptions, floornooptions)).result(function (event, data, formatted) {
                if (data.floorno + "层" == floorno) {
//                    $(this).parent().before('<div class="divnormal" rel="floorno">' + data.floorno + '层</div>').insertAfter($(".divnormal:last"));
//                    $("#searchkey").val("").unautocomplete();
                    return;
                }
                floorno = data.floorno;
                $(this).parent().before('<div class="divnormal" rel="floorno">' + data.floorno + '层</div>');
                removeSiblings($(this).parent());
                $("#searchkey").val("").unautocomplete();
                autohouse()
            });
        },
        error: function () {//报错
            alert('抱歉网络异常，请稍后再重试');
        }
    });
}
//加载房号
function autohouse() {
    var houseoptions = {
        minChars: 0,
        divisible: true,
        formatItem: function (row, i, max) {

            return row.housename;
        },
        formatMatch: function (row, i, max) {
            return row.housename;
        },
        formatResult: function (row) {
            return row.housename;
        }
    };

    $.ajax({
        type: "POST",
        url: root + "/api/autoprice.ashx",
        data: { type: "autohouselist", buildingid: buildingid, floorno: floorno, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid, wx: wx },
        dataType: "json",
        success: function (data) {
            var vdata = data.data;
            $("#searchkey").autocomplete(vdata, $.extend({}, baseoptions, houseoptions)).result(function (event, data, formatted) {
                if (data.houseid == houseid) {
//                    $(this).parent().before('<div class="divnormal" rel="housename">' + data.housename + '</div>').insertAfter($(".divnormal:last"));
//                    $("#searchkey").val("").unautocomplete();
                    return;
                }
                houseid = data.houseid; housename = data.housename;
                $(this).val("").unautocomplete().parent().before('<div class="divnormal" rel="housename">' + data.housename + '</div>');

            });
        },
        error: function () {//报错
            alert('抱歉网络异常，请稍后再重试');
        }
    });
}

