var root = root, api = root + "/api/autoprice.ashx", prow = 1, brow = 1, frow = 1, hrow = 1, pagerecords =5
    , dedata = {};
//getProejctList();
$(function () {



    $("#btnsearch").click(function () {
        var projectname = encodeURI($("#projectname").val()),
            floorno = $("#floorno").val(),
            buildingname = encodeURI($("#buildingname").val()),
            housename = encodeURI($("#housename").val());
        if (projectname.length == 0) {
            alert("请输入楼盘"); $("#projectname").focus();
        } else if (buildingname.length == 0) {
            alert("请输入楼栋"); $("#buildingname").focus();
        } else if (floorno.length == 0) {
            alert("请输入楼层"); $("#floorno").focus();
        } else if (housename.length == 0) {
            alert("请输入房号"); $("#housename").focus();
        } else {
            var hurl = location.href.substring(0, location.href.indexOf("?"));
            if (houseid > 0 && casecnt > 0 && $("#projectavg").is(':visible')) {

                root = root + "/auto/price.aspx?type=autoprice&cityid=" + CAS.Define.cityid + "&fxtcompanyid=" + CAS.Define.fxtcompanyid + "&projectid=" + projectid + "&projectname=" + init.urlargshandle(projectname)
                    + "&buildingid=" + buildingid + "&buildingname=" + init.urlargshandle(buildingname)
+ "&address=" + address + "&wx=" + wx + "&floorno=" + floorno + "&areaid=" + areaid + "&totalfloor=" + totalfloor + "&totalfloor=" + totalfloor
                    + "&houseid=" + houseid + "&housename=" + init.urlargshandle(housename) + "&buildarea=" + buildarea + "&casecnt=" + casecnt + "&isinside=" + isinside
                    + "&wxopenid=" + wxopenid + "&telephone=" + telephone + "&pid=" + pid;
                location.href = root;
            } else {
                root = root + "/auto/project.aspx?type=autohouselist&cityid=" + init.GetQuery("cityid") + "&fxtcompanyid=" + CAS.Define.fxtcompanyid + "&projectid=" + projectid + "&projectname=" + init.urlargshandle(projectname)
                    + "&address=" + address + "&wx=" + wx + "&totalfloor=" + totalfloor
                    + "&buildingid=" + buildingid + "&buildingname=" + init.urlargshandle(buildingname) + "&floorno=" + floorno + "&areaid=" + areaid
                    + "&houseid=" + houseid + "&housename=" + init.urlargshandle(housename) + "&buildarea=" + buildarea + "&isinside=" + isinside
                    + "&wxopenid=" + wxopenid + "&casecnt=" + casecnt + "&telephone=" + telephone + "&pid=" + pid;
                location.href = root;
            }
        }

    });



    //LI 翻页
    $("#pageul span").live("mousedown", function (e) {
        var $this = $(this),
            $p = $("#projectname"),
            pv = $("#projectname").val();
        if ($this.attr("r") == "up" && !$this.hasClass("colgray2")) {
            if ($this.attr("rel") == "projectname") {
                var $p = $("#projectname");
                $p.blur();
                prow--;
                projectload($p, $p.val());
            } else if ($this.attr("rel") == "buildingname") {
                brow--;
                buildingload($("#buildingname"));
            } else if ($this.attr("rel") == "floorno") {
                frow--;
                floornoload($("#floorno"));
            } else if ($this.attr("rel") == "housename") {
                hrow--;
                houseload($("#housename"));
            }
        } else if ($this.attr("r") == "down" && !$this.hasClass("colgray2")) {
            if ($this.attr("rel") == "projectname") {
                var $p = $("#projectname");
                $p.blur();
                prow++;
                projectload($p, $p.val());
            } else if ($this.attr("rel") == "buildingname") {
                brow++;
                buildingload($("#buildingname"));
            } else if ($this.attr("rel") == "floorno") {
                frow++;
                floornoload($("#floorno"));
            } else if ($this.attr("rel") == "housename") {
                hrow++;
                houseload($("#housename"));
            }
        }
        //如果提供了事件对象，则这是一个非IE浏览器
        if (e && e.stopPropagation) {
            //阻止默认浏览器动作(W3C)
            e.stopPropagation();
        } else if (e) {
            //IE中阻止函数器默认动作的方式
            e.cancelBubble = false;
        }
        //如果提供了事件对象，则这是一个非IE浏览器
        if (e && e.preventDefault) {
            //阻止默认浏览器动作(W3C)
            e.preventDefault();
        } else if (e) {
            //IE中阻止函数器默认动作的方式
            e.returnValue = false;
        }
        return false;
    })

    //楼盘
    $("#projectname").bind("keyup", function (e) {
        var tar = e.target,
        $this = $(this),
         v = $this.val()
        defValue = tar.historyVal,
        projectavg=$("#projectavg");
        prow = 1;
        tar.historyVal = v;

        if (v != "" && v != defValue) {
            if (projectavg.is(':visible')) { projectavg.hide(); }
            if (tar.deplayFn) { clearTimeout(tar.deplayFn) }
            tar.deplayFn = setTimeout(function () { projectload($this, v); }, 800);
        }

        if (v != projectname) {
            clearproject();
        }

    }).bind("click", function () {
        var $this = $(this),
            v = $this.val(),
            _name = $this.attr("id"),
            ac_results = $(".ac_results");
        if (ac_results && $("#p_" + _name).next() != ac_results) { ac_results.remove(); }

        if (v.length > 0) {
            projectload($this, v);
        }
        return false;
    });
    //楼栋
    $("#buildingname").bind("keyup", function (e) {
        var $this = $(this),
            v = $this.val();
        brow = 1;
        if (projectid > 0) {
            buildingload($this);
        }
        if (v != buildingname || v.length == 0) {
            clearbuilding();
        }
    }).bind("click", function () {
        var $this = $(this),
            _name = $this.attr("id");
        ac_results = $(".ac_results");
        if (ac_results && $("#p_" + _name).next() != ac_results) { ac_results.remove(); }
        if (projectid > 0) {
            buildingload($this);

        }
        return false;
    });
    //楼层
    $("#floorno").bind("keyup", function (e) {
        var $this = $(this);
        frow = 1;
        if (buildingid > 0) {
            floornoload($this);
        }
        if (v != floorno || v.length == 0) {
            clearfloorno();
        }
    }).bind("click", function () {
        var $this = $(this),
            _name = $this.attr("id"),
            ac_results = $(".ac_results");
        if (ac_results && $("#p_" + _name).next() != ac_results) { ac_results.remove(); }
        if (buildingid > 0) {
            floornoload($this);

        }
        return false;
    });
    //房号
    $("#housename").bind("keyup", function (e) {
        var $this = $(this),
            v = $this.val();
        hrow = 1;
        if (floorno > 0) {
            houseload($this);
        }
        if (v != housename || v.length == 0) {
            clearhouse();
        }
    }).bind("click", function () {
        var $this = $(this),
            _name = $this.attr("id"),
            ac_results = $(".ac_results");
        if (ac_results && $("#p_" + _name).next() != ac_results) { ac_results.remove(); }
        if (floorno > 0) {
            houseload($this);
        }
        return false;
    });

    $("#li_result li").live("click", function (e) {//将选中LI项赋值给文本框
        var _this = $(this),
            _rel = _this.attr("rel").split("_"),
            _name = _rel[0],
            _id = _rel[1],
            _text = _this.text()
        line = _this.attr("line"),
            _o = $("#" + _name);
        if (_name == "projectname" && _id > 0) {
            if (projectid != _id) {
                _o.val(_text);
                projectid = _id;
                loadProjectdetails(projectid);
                loadprojectavg(projectid);
                buildingid = 0, floorno = 0, houseid = 0;
                projectname = _text;
                $("#buildingname").val(""); $("#floorno").val(""); $("#housename").val("");
                buildingload($("#buildingname"));
            } else {
                _o.val(_text); $(".ac_results").remove();
            }

        } else if (_name == "buildingname" && _id > 0) {
            if (buildingid != _id) {

                _o.val(_text), buildingid = _id; totalfloor = dedata[totalfloor].totalfloor;
                floorno = 0, houseid = 0;
                buildingname = _text;
                $("#floorno").val(""); $("#housename").val("");
                floornoload($("#floorno"));
            } else {
                $(".ac_results").remove();
            }


        } else if (_name == "floorno" && _id > 0) {
            if (floorno != _id) {
                _o.val(_text), floorno = _id;
                houseid = 0; $("#housename").val("");
                houseload($("#housename"));
            } else {
                $(".ac_results").remove();
            }


        } else if (_name == "housename" && _id > 0) {
            if (houseid != _id) {
                _o.val(_text), houseid = _id; buildarea = dedata[line].buildarea;
                housename = _text;
                $(".ac_results").remove();
            } else {
                $(".ac_results").remove();
            }

        }
        // setTimeout(function () { _this.parent().parent().remove() }, 100);
        e.stopPropagation();
    }).live("touchstart", function () {//mobile 触摸添加样式
        var _this = $(this),
            _rel = _this.attr("rel").split("_"),
            _name = _rel[0],
            _id = _rel[1];
        if (_id > 0) {
            _this.addClass("ac_hover");
            setTimeout(function () { _this.removeClass("ac_hover"); }, 300);
        }

    });
    //
    $("body").bind("click", function (e) {

        var tar = e.target,
            r = $(tar).attr("r");
        if (!(r == "down" || r == "up")) {
            $(".ac_results").hide();
        }

    });
    //装载数据
    //args 请求参数，url数据地址，acobject操作对象
    var loaddata = function (args, url, acobject, sdata, pageindex) {

        if (args.key && acobject.attr("id") == "projectname") {
            args.key = args.key();
            args.key = args.key.trimEnd();
            if (args.key == "") { $(".ac_results").remove(); acobject.removeClass("loading"); return; }
        } else {
            args.key = args.key();
            if (args.key.length > 0) {
                args.key = args.key.trimEnd();
            }
        }
        acobject.addClass("loading");
        $.ajax({
            type: "POST",
            url: url, // root + "/api/autoprice.ashx",
            data: args,
            dataType: "json",
            cache: false,
            success: function (data) {
                var vdata = data.data;
                if (vdata.length > 0) {
                    dedata = vdata;
                    var html = "<div class='ac_results'><ul id='li_result'>";
                    for (var i = 0, len = vdata.length; i < len; i++) {
                        var vd = vdata[i],
                            vt = sdata[1] == "floorno" ? vd[sdata[1]] : vd[sdata[1]], c = 0;

                        html += "<li line='" + i + "' class='' rel='" + acobject.attr("id") + "_" + vd[sdata[0]] + "'>" + vt + "</li>"
                    }
                    html += "</ul>";

                    if (pagerecords < vdata[0].recordcount) {
                        var d = "", u = "";
                        if (pageindex * pagerecords >= vdata[0].recordcount) {
                            d = "colgray2";
                        }
                        if (pageindex * pagerecords <= pagerecords) {
                            u = "colgray2";
                        }
                        html += "<ul id='pageul'><li><span r='up'  class='" + u + "' id='btnup' rel='" + acobject.attr("id") + "' style='display:inline-block;text-align:left;margin-right:5em;width:30%;'>上一页</span><span class='di " + d + "' rel='" + acobject.attr("id") + "' r='down' style='text-align:right;display:inline-block;width:30%;'>下一页</span></li></ul>";
                    }



                    html += "</div>";
                    var ac_results = $(".ac_results");
                    ac_results.remove();
                    acobject.parent().parent().parent().append(html);
                } else {

                    $(".ac_results").remove();
                }
                acobject.removeClass("loading");
                // $("#projectname").parent().parent().appendTo(html);
            },
            error: function () {//报错   
                acobject.removeClass("loading");
                alert('抱歉网络异常，请稍后再重试');
            }
        });
    },
    projectload = function ($this, v) {//读楼盘
        // if (e.keyCode == 0) {
        var args = { type: "autoprojectlist",
            key: function () { var v = $this.val().substring("["); return $this.val().substring("["); },
            cityid: CAS.Define.cityid,
            fxtcompanyid: CAS.Define.fxtcompanyid,
            wx: wx,
            pageindex: prow,
            pagerecords: pagerecords
        };
        loaddata(args, api, $this, ["projectid", "projectname"], prow);

        //  }
    },
    buildingload = function ($this, v) { //读楼栋
        // if (e.keyCode == 0) {
        var args = { type: "autobuildinglist", projectid: projectid, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid, wx: wx,
            pageindex: brow,
            key: function () { return $this.val(); },
            pagerecords: pagerecords
        };
        loaddata(args, api, $("#buildingname"), ["buildingid", "buildingname"], brow);
        //  }
    },
    floornoload = function ($this, v) { //读楼层
        // if (e.keyCode == 0) {
        var args = { type: "autofloorlist", buildingid: buildingid, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid, wx: wx,
            pageindex: frow,
            key: function () { return $this.val().replace(/\D/ig, ""); },
            pagerecords: pagerecords
        };
        loaddata(args, api, $("#floorno"), ["floorno", "floorno"], frow);
        //  }
    },
    houseload = function ($this, v) { //读房号
        // if (e.keyCode == 0) {

        var args = { type: "autohouselist", buildingid: buildingid, floorno: floorno, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid, wx: wx,
            pageindex: hrow,
            key: function () { return $this.val(); },
            pagerecords: pagerecords
        };
        loaddata(args, api, $("#housename"), ["houseid", "housename"], hrow);
        //  }
    }, clearproject = function () {
        projectid = 0, areaid = 0, address = "", casecnt = 0;
        $("#buildingname").val("");
        clearbuilding();
    }, clearbuilding = function () {
        buildingid = 0, totalfloor = 0,
        $("#floorno").val("");
        clearfloorno();
    }, clearfloorno = function () {
        floorno = 0,
        $("#housename").val("");
        clearhouse();
    }, clearhouse = function () {
        buildarea = 0, houseid = 0;
    }, loadProjectdetails = function (projectid) {

        $.ajax({
            type: "POST",
            url: root + "/api/autoprice.ashx",
            data: { type: "autoprojectdetails", projectid: projectid, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid, wx: wx},
            dataType: "json",
            cache: false,
            success: function (data) {

                var vdata = data.data;
                if (vdata) {
                    address = vdata.address, casecnt = vdata.casecnt, areaid = vdata.areaid;

                }
            },
            error: function () {//报错   
                alert('抱歉网络异常，请稍后再重试');
            }
        });
    }, loadprojectavg = function (projectid) {
        $.ajax({
            type: "POST",
            url: root + "/api/autoprice.ashx",
            data: { type: "autoprice", projectid: projectid, buildingid: 0, houseid: 0, floorno: 0, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid, wx: wx, wxopenid: wxopenid },
            dataType: "json",
            success: function (data) {
                var vdata = data.data;
                if (vdata && vdata.avgPrice > 0) {
                    $($("#projectavg").show().find("span")[1]).text(init.Commafy(vdata.avgPrice) + "元").addClass("red");
                } else {
                    $("#projectavg").hide()
                }
            },
            error: function () {//报错
                alert('抱歉网络异常，请稍后再重试');
            }
        });
    }


});


//removeSiblings

