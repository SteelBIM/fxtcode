/*
    作者:贺黎亮
    摘要:新建 押品资产价值动态监测 2014.03.24
*/
// 百度地图API功能
var mpTypeText = '', _zoom = 11;
function CDMap(text, complete) {
    var mp = $("#map").extendMap({
        name: text,
        pname: text,
        zoom: _zoom,
        complete: function () {
            if (mp.getZoom() > _zoom) {
                mp.setCenter(text);
                mp.setZoom(_zoom);
            }
            if ($.isFunction(complete))
                complete(mp);
        }
    });
}
/* 设置坐标显示信息 */
function SetMapPoint(name, pname, data, count, mp) {
    new BMap.Geocoder().getPoint(name, function (point) {
        if (point) {
            function ComplexCustomOverlay(point, name, data, count) {
                this._point = point;
                this._text = name;
                this._data = data;
                this._count = count;
            }
            ComplexCustomOverlay.prototype = new BMap.Overlay();
            ComplexCustomOverlay.prototype.initialize = function (map) {
                this._map = map;
                var div = this._div = document.createElement("div");
                div.style.position = "absolute";
                div.style.zIndex = BMap.Overlay.getZIndex(this._point.lat);
                //div.style.backgroundColor = "#EE5D5B";
                div.style.borderBottom = "1px solid #086ceb";
                div.style.color = "white";
                div.style.height = "18px";
                div.style.padding = "0px";
                div.style.lineHeight = "14px";
                div.style.whiteSpace = "nowrap";
                div.style.MozUserSelect = "none";
                div.style.fontSize = "12px"

                /*主体*/
                var divBody = document.createElement("div");
                divBody.style.width = "100px";
                divBody.style.float = "left";
                divBody.style.height = "auto";
                divBody.style.marginTop = "-20px";
                divBody.innerHTML = '<ul id="q-graph">' +
                                '<li id="q1">' +
                                    '<div style="color:#060606;">' + this._text + '</div>' +
                                    '<div style="color:#f11445;">' + mpTypeText + ':' + this._data.Count + '</div>' +
                                    '<div id="dialog">' +
                                    '<div style="border: 1px solid #ddd; background-color: white;color:black; ">' +
                                    '    <table>' +
                                    '    <tr>' +
                                    '        <td>押品总量:</td>' +
                                    '        <td>' + this._data.CollNumberCount + '</td>' +
                                    '        <td>笔</td>' +
                                    '    </tr>' +
                                    '        <tr>' +
                                    '            <td>贷款总额:</td>' +
                                    '            <td>' + this._data.CollTotal + '</td>' +
                                    '            <td>万元</td>' +
                                    '        </tr>' +
                                    '        <tr>' +
                                    '            <td>贷款余额:</td>' +
                                    '            <td>' + this._data.CollOver + '</td>' +
                                    '            <td>万元</td>' +
                                    '        </tr>' +
                                    '        <tr>' +
                                    '            <td>押品面积:</td>' +
                                    '            <td>' + this._data.CollateralArea + '</td>' +
                                    '            <td></td>' +
                                    '        </tr>' +
                                    '        <tr>' +
                                    '            <td>原估价值:</td>' +
                                    '            <td>' + this._data.OriginalValue + '</td>' +
                                    '            <td>万元</td>' +
                                    '        </tr>' +
                                    '        <tr>' +
                                    '            <td>现估价值:</td>' +
                                    '            <td>' + this._data.AssessedValue + '</td>' +
                                    '            <td>万元</td>' +
                                    '        </tr>' +
                                    '        <tr>' +
                                    '            <td>担保金额:</td>' +
                                    '            <td>' + this._data.AmountValue + '</td>' +
                                    '            <td>万元</td>' +
                                    '        </tr>' +
                                    '        <tr>' +
                                    '            <td>原抵押率:</td>' +
                                    '            <td>' + this._data.OriginalRate + '</td>' +
                                    '            <td></td>' +
                                    '        </tr>' +
                                    '        <tr>' +
                                    '            <td>现抵押率:</td>' +
                                    '            <td>' + this._data.MortgageRate + '</td>' +
                                    '            <td></td>' +
                                    '        </tr>' +
                                    '        <tr>' +
                                    '            <td></td>' +
                                    '            <td></td>' +
                                    '            <td>更多...</td>' +
                                    '        </tr>' +
                                    '    </table>' +
                                    '</div>' +
                                    '<div style="margin-left: 50px; background: url(http://map.baidu.com/fwmap/upload/r/map/fwmap/static/house/images/label.png) no-repeat; width: 11px; height:10px;"></div>' +
                '</div>' +
                '</li>' +
                '<li id="q2">' +
                '    <ul>' +
                '        <li class="south bar" style="height:' + Math.ceil((this._data.Count / this._count) * 100) + 'px;"></li>' +
                '    </ul>' +
                '</li>' +
                '</ul>';
                $('ul#q-graph>#q1', divBody).mousemove(function () {
                    $('div:eq(2)', this).show();
                }).mouseout(function () {
                    $('div:eq(2)', this).hide();
                });
                div.appendChild(divBody);
                var that = this;

                var arrow = this._arrow = document.createElement("div");
                arrow.style.background = "url(http://map.baidu.com/fwmap/upload/r/map/fwmap/static/house/images/label.png) no-repeat";
                arrow.style.position = "absolute";
                arrow.style.width = "11px";
                arrow.style.height = "10px";
                arrow.style.top = "18px";
                arrow.style.left = "10px";
                arrow.style.overflow = "hidden";
                div.appendChild(arrow);
                mp.getPanes().labelPane.appendChild(div);

                return div;
            }
            ComplexCustomOverlay.prototype.draw = function () {
                var map = this._map;
                var pixel = map.pointToOverlayPixel(this._point);
                this._div.style.left = (pixel.x - parseInt(this._arrow.style.left)) + "px";
                this._div.style.top = pixel.y - 30 + "px";
            }
            var myCompOverlay = new ComplexCustomOverlay(point, name, data, count);

            mp.addOverlay(myCompOverlay);
        }
    }, pname);
}

$(document).ready(function () {
    $(".nav-tabs>li>a").click(function () {
        if ($(this).attr("href") == "#maps") {
            $("#singlecity").show();
            $("#mulllecity").hide();
        } else {
            $("#mulllecity").show();
            $("#singlecity").hide();
        }
    });
    var _maps = $('#maps'),
        _class = $('#classification'),
        _detials = $('#colldetials'),
        _name = null,
        _counttype = 0,
        _checkdata = [
            { id: 'wuyetype', val: 8007, key: 'Code', text: 'CodeName' },
            { id: 'jianzhutype', val: 2003, key: 'Code', text: 'CodeName' },
            { id: 'niandaitype', val: 8004, key: 'Code', text: 'CodeName' },
            { id: 'daikuantype', val: 8008, key: 'Code', text: 'CodeName' },
            { id: 'mianjitype', val: 8003, key: 'Code', text: 'CodeName' },
            { id: 'nianlingtype', val: 8009, key: 'Code', text: 'CodeName' },
            { id: 'fangdaidate', val: 8004, key: 'Code', text: 'CodeName' }
        ], _checki = 0;
    /*加载目城市*/
    CreateItemList(".procheckbox-list", "singlecity");
    SetCheckBox(_checkdata[_checki])
    /* 一步一步加载条件 */
    function SetCheckBox(data) {
        $.extendAjax({ type: 'post', url: '/AssetsCollateralDetect/GetCode/?id={0}'.format(data.val) }, function (cdata) {
            $.each(cdata, function (k, v) {
                var _append = '<input value="{0}" type="checkbox" name="{2}">{1}'
                    .format(v[data.key], v[data.text], data.id);
                if (data.id != 'wuyetype' && data.id != 'jianzhutype') {
                    var reg = /[\u4E00-\u9FA5]/g,
                        _val = v[data.text].replace(reg, ''),
                        _valSplit;
                    if (data.id == 'daikuantype') {
                        if (_val.indexOf('~') > -1) {
                            var _vs = _val.split('~');
                            _val = '{0}~{1}'.format(_vs[0] * 10000, _vs[1] * 10000);
                        } else {
                            _val = _val * 10000;
                        }
                    }
                    if (k == 0) {
                        _val = "{0}&&<".format(_val);
                    } else if (k == cdata.length - 2) {
                        _val = "{0}&&==".format(_val);
                    } else if (k == cdata.length - 1) {
                        _val = "{0}&&>".format(_val);
                    } else {
                        _val = "{0}&&=".format(_val);
                    }
                    _append = '<input value="{0}" type="checkbox" name="{2}">{1}'.format(_val, v[data.text], data.id);
                }
                $('.{0}'.format(data.id)).append(_append);
            });
            if (_checki != _checkdata.length - 1) {
                _checki++;
                SetCheckBox(_checkdata[_checki]);
            } else {
                App.init();
                CityProcVal(".singlecity", "mapcityarrid");
            }
        });
    }
    /*地图*/
    var loadMap = function () {
        /*设置地图显示类型*/
        $('li:not(.dropdown)', _maps).each(function (i) {
            if (i == 0) {
                SetMpTypeText(i);
            }
            $(this).click(function () {
                SetMpTypeText(i);
            });
        });
        $('div.tab-content>div.form-inline>button', _maps).click(function () {
            var sendData = GetChecked($("input[type=checkbox]", _maps)),
                roc = GetRadioOrCheckboxCity('r');
            sendData['cid'] = roc.val;
            sendData['type'] = _counttype;
            /*设置坐标*/
            CDMap(roc.text, function (mp) {
                //console.log(sendData);
                $.extendAjax({
                    url: '/AssetsCollateralDetect/GetPCACount',
                    type: 'post',
                    data: sendData
                }, function (rdata) {
                    var _rdata = rdata.data, sum = 0;
                    if (_rdata != undefined) {
                        for (var item in _rdata) {
                            sum += _rdata[item].Count;
                        }
                        for (var item in _rdata) {
                            if (_rdata[item].Area != undefined) {
                                var _Area = $.parseJSON(_rdata[item].Area);
                                SetMapPoint(_Area.AreaName, roc.text, _rdata[item], sum, mp);
                            } else if (_rdata[item].City != undefined) {
                                var _Area = $.parseJSON(_rdata[item].City);
                                SetMapPoint(_Area.CityName, roc.text, _rdata[item], sum, mp);
                            }
                        }
                    }
                });
            });
        });
        function SetMpTypeText(i) {
            _counttype = i;
            switch (i) {
                case 0:
                    mpTypeText = '总量';
                    break;
                case 1:
                    mpTypeText = '总额';
                    break;
                case 2:
                    mpTypeText = '余额';
                    break;
                case 3:
                    mpTypeText = '押品面积';
                    break;
                case 4:
                    mpTypeText = '原估价值';
                    break;
                case 5:
                    mpTypeText = '现估价值';
                    break;
                case 6:
                    mpTypeText = '担保金额';
                    break;
                case 7:
                    mpTypeText = '原抵押率';
                    break;
                case 8:
                    mpTypeText = '现抵押率';
                    break;
            }
        }
    },
        /*分类统计*/
        loadClass = function () {
            $('ul.nav-pills li:not(.dropdown)', _class).each(function (k, v) {
                var _tmpdata = _checkdata[k];
                $(this).attr('data-index', k);
                $(this).attr('data-val', _tmpdata.val);
            });
            $("ul.nav-pills>li", _class).click(function () {
                var  sendData = {}; 
                sendData['requirement'] = '{0}&&{1}'.format($(this).attr('data-index'), $(this).attr('data-val'));                
                $.extendAjax({
                    url: '/AssetsCollateralDetect/GetCFCount',
                    type: 'post',
                    data: sendData
                }, function (rdata) {
                    $("#tb_chart{0}>tbody".format(1)).empty();
                    for (var i = 1; i <= 7; i++) {
                        $("div.form-inline>div #pie_chart{0}".format(i), _class).parent().hide();
                    }
                    if (rdata.type == 1) {
                        rdata = rdata.data;
                        /*表格数据显示*/
                        $("#tb_chart{0}>thead>tr>th".format(1)).eq(0).html($("#classification>ul>li.active").text());
                        var isnear = 0;
                        $("#tb_chart{0}>tbody".format(1)).html('');
                        for (var i = 1; i <= 7; i++) {
                            var _data = [];
                            for (var item in rdata) {
                                _data[item] = {
                                    label: rdata[item].name,
                                    data: rdata[item].count,
                                    buildingarea: rdata[item].buildingarea,
                                    loanamount: rdata[item].loanamount,
                                    oldrate: rdata[item].oldrate,
                                    rate: rdata[item].rate,
                                    guaranteeprice: rdata[item].guaranteeprice,
                                    oldavergerates: rdata[item].oldavergerates,
                                    avergerates: rdata[item].avergerates
                                }
                            }
                            if (i == 1) {
                                $.tmpl("<tr><td>${label}</td><td>${data}</td><td>${buildingarea}</td><td>${oldrate}</td><td>${rate}</td>" +
                            "<td>${guaranteeprice}</td><td>${oldavergerates}</td><td>${avergerates}</td></tr>", _data)
                                    .appendTo("#tb_chart{0}>tbody".format(i));
                            }
                            isnear = 0;
                            $(rdata).each(function (j, o) {
                                switch (i) {
                                    case 1:
                                        isnear += parseFloat(o.count);
                                        break;
                                    case 2:
                                        isnear += parseFloat(o.buildingarea);
                                        break;
                                    case 3:
                                        isnear += parseFloat(o.loanamount);
                                        break;
                                    case 4:
                                        isnear += parseFloat(o.oldrate);
                                        break;
                                    case 5:
                                        isnear += parseFloat(o.guaranteeprice);
                                        break;
                                    case 6:
                                        isnear = 0;
                                        break;
                                }
                            });
                            if (!isNaN(isnear) && isnear != "0" && isnear != "") {
                                /*显示7个图表*/
                                $("div.form-inline>div #pie_chart{0}".format(i), _class).parent().show();
                                $.plot($("div.form-inline>div #pie_chart{0}".format(i), _class), _data, {
                                    series: {
                                        pie: {
                                            show: true
                                        }
                                    },
                                    legend: {
                                        show: false
                                    }
                                });
                            } else {
                                $("div.form-inline>div #pie_chart{0}".format(i), _class).parent().hide();
                            }
                        }
                    }
                });
        });
},
        /*明细查询*/
        loadDetials = function () {
            var _inputtext = $('input[type=text]', _detials);

            _inputtext.eq(2).extendAutomplete({
                url: '/AssetsCollateralDetect/CompanySearch',
                column: [{ v1: 'cityid', v2: $('select[name=horizontalcity]'), v3: 'data-val' }],
                formatItem: function (row) {
                    return row.ChineseName;
                },
                result: function (event, data, formatted) {
                    $(this).val(data.ChineseName);
                    $(this).attr('data-val', data.CompanyId);
                }
            });
            _inputtext.eq(3).extendAutomplete({
                url: '/AssetsCollateralDetect/Search',
                column: [{ v1: 'cityid', v2: $('select[name=horizontalcity]'), v3: 'data-val' }],
                formatItem: function (row) {
                    return row.ProjectName;
                },
                result: function (event, data, formatted) {
                    $(this).val(data.ProjectName);
                    $(this).attr('data-val', data.ProjectId);
                }
            });

            var cpage = $.ActionPage,
                nation = $('.pagination', _detials),
                paginfo = $('#page_info', _detials);

            cpage.pageSize = $.extendBug ? 1 : 50;

            $("button", _detials).click(function () {

                var _ckbox = $('input[type=checkbox]', _detials),
                    sendData = GetChecked(_ckbox);

                $.each(_inputtext, function (k, v) {
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
                sendData['pageSize'] = cpage.pageSize;
                sendData['pageIndex'] = cpage.pageIndex;
                //console.log(sendData);
                $.extendAjax({
                    url: '/AssetsCollateralDetect/GetCollDetials',
                    type: 'post',
                    data: sendData
                }, function (rdata) {
                    $("#tb_colldetials>tbody").empty();
                    $("#colldetials_ok").hide();
                    if (rdata.type == 1) {
                        $("#colldetials_ok").show();
                        $.ActionTmpl("tmp_colldetials", rdata.data, "tb_colldetials>tbody");
                        $.ProcessPage({
                            pagination: nation,
                            paginfo: paginfo,
                            count: rdata.count,
                            page: cpage,
                            complete: function (page) {
                                cpage.pageIndex = page.pageIndex;
                                $("button", _detials).click();
                            }
                        });
                    }
                });
            });
        },
        //风险分析
        loadRiskAnalysis = function () {
            var data = {};
            data['type'] = 0;
            $.extendAjax({
                type: 'post',
                url: '/CollateralReassessment/RiskAnalysis',
                data: data
            }, function (kaData) {
                $("#tab2_table>tbody").empty();
                if (kaData.type == 1) {
                    $.ActionTmpl("tab2_Tmpl", kaData.data, "tab2_table>tbody");
                }
            });
        },
        //复估明细查询
    loadReassDetails = function () {
        var _detail = $('#tab_3'),
            _inputtext = $('input[type=text]', _detail),
            company = 0, project = 0;

        _inputtext.eq(0).extendAutomplete({
            url: '/CollateralDetect/CompanySearch',
            column: [{ v1: 'cityid', v2: $('select[name=horizontalcity]'), v3: 'data-val' }],
            formatItem: function (row) {
                return row.ChineseName;
            },
            result: function (event, data, formatted) {
                $(this).val(data.ChineseName);
                company = data.CompanyId;
                $(this).attr('data-id', company);
            }
        });
        _inputtext.eq(1).extendAutomplete({
            url: '/CollateralDetect/Search',
            column: [{ v1: 'cityid', v2: $('select[name=horizontalcity]'), v3: 'data-val' }],
            formatItem: function (row) {
                return row.ProjectName;
            },
            result: function (event, data, formatted) {
                $(this).val(data.ProjectName);
                project = data.ProjectId;
                $(this).attr('data-id', project);
            }
        });

        var cpage = $.ActionPage,
                nation = $('.pagination', _detail),
                paginfo = $('#page_info', _detail);

        cpage.pageSize = $.extendBug ? 1 : 150;

        $(".detailbutton").click(function () {
            var _ckbox = $('input[type=checkbox]', _detail),
                sendData = GetChecked(_ckbox);
            sendData['companyid'] = company;
            sendData['projectid'] = project;
            sendData['type'] = 0;
            sendData['pageSize'] = cpage.pageSize;
            sendData['pageIndex'] = cpage.pageIndex;
            $.extendAjax({
                url: '/CollateralReassessment/GetReassessmentDetails',
                type: 'post',
                data: sendData
            }, function (rdata) {
                $("#tb_colldetials>tbody").empty();
                $("#tab_3>#colldetials_ok").hide();
                if (rdata.type == 1) {
                    $("#tab_3>#colldetials_ok").show();
                    $.ActionTmpl("tab3_Tmpl", rdata.data, "tab3_colldetials>tbody");
                    $.ProcessPage({
                        pagination: nation,
                        paginfo: paginfo,
                        count: rdata.count,
                        page: cpage,
                        complete: function (page) {
                            cpage.pageIndex = page.pageIndex;
                            $(".detailbutton").click();
                        }
                    });
                }
            });

        });
    }
    loadMap();
loadClass();
loadDetials();
$("#tab_2>.btn").click(function () {
    loadRiskAnalysis();
});
$(".detailbutton").click(function () {
    loadReassDetails();
});
if (jQuery().datepicker) {
    $('.date-picker').datepicker({
        rtl: App.isRTL(),
        autoclose: true
    });
}

});

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

function GetPCA() {
    var _pSelect = $('select[name=horizontalprovince]'),
            _pData = GetData(_pSelect.data('data'), $('option:selected', _pSelect).text()),
            _cSelect = $('select[name=horizontalcity]'),
            _cData = GetData(_cSelect.data('data'), $('option:selected', _cSelect).text()),
            _aSelect = $('select[name=horizontalarea]'),
            _aData = GetData(_aSelect.data('data'), $('option:selected', _aSelect).text());
    /*获取省市县区*/
    if (_pData != undefined) {
        _name = _pData.ProvinceName.replace("特区", "").replace("直辖", "");
        _pname = _name;
        _cname = '';
        _aname = '';
        pid = _pData.ProvinceId;
        cid = 0;
        aid = 0;
        _zoom = 8;
    }
    if (_cData != undefined) {
        _name = _cData.CityName.replace("直辖市", "");
        _cname = _name;
        _aname = '';
        cid = _cData.CityId;
        aid = 0;
        _zoom = 12;
    }
    if (_aData != undefined) {
        _name = _aData.AreaName;
        _aname = _name;
        aid = _aData.AreaId;
        _zoom = 16;
    }
}