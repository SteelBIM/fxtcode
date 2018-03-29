/**
作者：李晓东
时间:2014.05.08
摘要:新建

*/
var senddata, _pname = '', _cname = '', _aname = '',
    pid = 0, cid = 0, aid = 0, _zoom = 11, mapcircledata;
$(document).ready(function () {
    $(".nav-tabs>li>a").click(function () {
        if ($(this).attr("href") == "#tab_1" || $(this).attr("href") == "#tab_3") {
            $("#singlecity").show();
            $("#mulllecity").hide();
        } else {
            $("#mulllecity").show();
            $("#singlecity").hide();
        }
    });
    /*加载目城市*/
    CreateItemList(".procheckbox-list", "singlecity");
    $(".nav-tabs>li>a")[0].click();
    /*押品价格走势分析*/
    var loadOne = function () {
        var chartdata = [], _data, d3 = '', yTitle = '', charTitle = '总量对比分析';
        $('.nav-pills>li').each(function (k, v) {
            $(this).find('a').click(function () {
                roc = GetRadioOrCheckboxCity('r');

                $("#tab_1_mj").hide();
                charTitle = $(this).text();
                senddata = {};
                senddata['type'] = k;
                senddata['down'] = false;
                if (k == 0) {
                    senddata['cid'] = roc.val;
                    getData();
                }
                else if (k == 1) {
                    getData();
                } else if (k == 2) {
                    $("#tab_1_mj").show();
                    /*加载面积条件 */
                    function SetCheckBox() {
                        var data = { id: 'mianjitype', val: 8003, key: 'Code', text: 'CodeName' };
                        $.extendAjax({ type: 'post', url: '/CollateralDetect/GetCode/?id={0}'.format(data.val) }, function (cdata) {
                            $.each(cdata, function (k, v) {
                                var active = k == 0 ? "active" : "";;
                                var _append = '<li class="{3}" data-val="{0}" data-id="{2}"><a href="#" data-toggle="tab">{1}</a></li>'
                                    .format(v[data.key], v[data.text], data.id, active);
                                $('#tab_1_mj').append(_append);
                            });
                            $("#tab_1_mj li").each(function (k, v) {
                                $(this).click(function () {
                                    senddata['wh'] = $(this).attr('data-val');
                                    getData();
                                });
                            });
                        });
                    }
                    SetCheckBox();
                    $("#tab_1_mj li").click(function () {
                        if ($(this).hasClass('active')) {
                            senddata['wh'] = $(this).attr('data-val');
                            getData();
                        }
                    });
                }
            });
        });
        function getData() {
            $.extendAjax({
                type: 'post',
                url: '/CollateralStressTest/PriceTrend',
                data: senddata
            }, function (data) {
                console.log(data);
                if (data.type == 1) {
                    _data = data.data;
                    $('#tab_1>button')[0].click();
                }
            });
        }
        /*组装结果*/
        function setDataToJson(field1, field2) {
            var d1 = '', d2 = '';
            chartdata = []; d3 = '';
            for (var item in _data) {
                d3 += '"{0}",'.format(new Date(_data[item].title).dateformat('yyyyMM'));
                d1 += '{0},'.format(_data[item][field1]);
                d2 += '{0},'.format(_data[item][field2]);
            }
            d3 = eval('[' + d3.substring(0, d3.length - 1) + ']');
            chartdata.push({ name: '', data: eval('[' + d1.substring(0, d1.length - 1) + ']') });
            chartdata.push({ name: '', data: eval('[' + d2.substring(0, d2.length - 1) + ']') });
        }
        /*切换视图*/
        $('#tab_1>button').each(function (k, v) {
            $(this).click(function () {
                if (k == 0) {
                    setDataToJson('marketavg', 'collavg');
                    chartdata[0].name = "市场均价";
                    chartdata[1].name = "押品均价";
                    yTitle = '均价';
                } else {
                    setDataToJson('collpricechange', 'marketpricechange');
                    chartdata[0].name = "市场均价涨跌幅";
                    chartdata[1].name = "押品均价涨跌幅";
                    yTitle = '涨跌幅';
                }
                $('#tab_1_1').highcharts({
                    chart: {
                        type: 'spline'
                    },
                    title: {
                        text: charTitle
                    },
                    xAxis: {
                        categories: d3
                    },
                    yAxis: {
                        title: {
                            text: yTitle
                        },
                        labels: {
                            formatter: function () {
                                return this.value
                            }
                        }
                    },
                    tooltip: {
                        crosshairs: true,
                        shared: true
                    },
                    plotOptions: {
                        spline: {
                            marker: {
                                radius: 4,
                                lineColor: '#666666',
                                lineWidth: 1
                            }
                        }
                    },
                    series: chartdata
                });
            });
        });
    },
    /*压力测试*/
        loadTwo = function () {
            var _checkdata = [
            { id: 'wuyetype', val: 8007, key: 'Code', text: 'CodeName' },
            { id: 'jianzhutype', val: 2003, key: 'Code', text: 'CodeName' },
            { id: 'niandaitype', val: 8004, key: 'Code', text: 'CodeName' },
            { id: 'daikuantype', val: 8008, key: 'Code', text: 'CodeName' },
            { id: 'mianjitype', val: 8003, key: 'Code', text: 'CodeName' },
            { id: 'nianlingtype', val: 8009, key: 'Code', text: 'CodeName' },
            { id: 'fangdaidate', val: 8004, key: 'Code', text: 'CodeName' }
            ], _checki = 0;

            /* 一步一步加载条件 */
            function SetCheckBox(data) {
                $.extendAjax({ type: 'post', url: '/CollateralDetect/GetCode/?id={0}'.format(data.val) }, function (cdata) {
                    $.each(cdata, function (k, v) {
                        var _val = v[data.key], _append = '<input value="{0}" type="checkbox" name="{2}">{1}'
                            .format(v[data.key], v[data.text], data.id);
                        if (data.id != 'wuyetype' && data.id != 'jianzhutype') {
                            var reg = /[\u4E00-\u9FA5]/g,
                                _valSplit;
                            _val = v[data.text].replace(reg, '');
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
                        if (v[data.text] != "30岁以下" && $('input[value="{0}"]'.format(_val)).length == 0)
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

            SetCheckBox(_checkdata[_checki]);
            $("#tab2_table>tbody").empty();
            $('#tab_2 button').click(function () {
                senddata = null;
                senddata = yaliTest($(this).parent().parent().parent(), $(this).parent().parent());
                $.extendAjax({
                    type: 'post',
                    url: '/CollateralStressTest/StressTest',
                    data: senddata
                }, function (data) {
                    $("#tab2_table>tbody").empty();
                    if (data.type == 1) {
                        $.ActionTmpl("tab2_tmpl", data.data, "tab2_table>tbody");
                    }
                });
            });
        },
    /*风险预警*/
        loadThree = function () {
            var tab3 = $("#tab_3"), map, roc;
            //senddata = GetPCAId();
            tab3.find('.nav-pills li').unbind('click');
            tab3.find('.nav-pills li').each(function (k, v) {
                $(this).click(function () {
                    senddata = {};
                    roc = GetRadioOrCheckboxCity('r');
                    map = $("#tab_3_map").extendMap({
                        name: roc.text,
                        pname: roc.text,
                        zoom: _zoom,
                        boundary: false,
                        complete: function () {
                            senddata['type'] = k;
                            senddata['typedown'] = 0;
                            $.extendAjax({
                                type: 'post',
                                url: '/CollateralStressTest/RiskWarning',
                                data: senddata
                            }, function (rdata) {
                                if (rdata.type == 1) {
                                    var data1 = rdata.data.v1,
                                        data2 = rdata.data.v2,
                                        i = 0;
                                    tab3.find('button.btn').unbind('click');
                                    tab3.find('button.btn').each(function () {
                                        $(this).bind('click', function () {
                                            i = 0;
                                            mapcircledata = [];
                                            if ($(this).hasClass('red')) {
                                                if (data1.length > 0) {
                                                    GetMapPointData(data1[i], data1, i, function () {
                                                        for (var item in mapcircledata) {
                                                            SetMapPoint(mapcircledata[item].point, mapcircledata[item], map, 'red');
                                                        }
                                                    });
                                                }
                                            }
                                            else {
                                                if (data2.length > 0) {
                                                    GetMapPointData(data2[i], data2, i, function () {
                                                        for (var item in mapcircledata) {
                                                            SetMapPoint(mapcircledata[item].point, mapcircledata[item], map, 'yellow');
                                                        }
                                                    });
                                                }
                                            }
                                        });
                                    });
                                    tab3.find('button.btn.red').trigger('click');
                                }
                            });
                        }
                    });
                });
                //tab3.find('.nav-pills li.active').trigger('click');
                var gcircle, gpoint, gCircleCustom;
                function circleInfo(e) {//点击圈选区域
                    gpoint = e.point;
                    if (gcircle != undefined)
                        mp.removeOverlay(gcircle);
                    var circle = new BMap.Circle(e.point, 1000,
                        { fillColor: "blue", strokeWeight: 1, fillOpacity: 0.3, strokeOpacity: 0.3 });
                    gcircle = circle;
                    mp.addOverlay(circle);

                    new BMap.Geocoder().getLocation(circle.getCenter(), function mCallback(rs) {
                        var allPois = rs.surroundingPois;       //获取全部POI（该点半径为100米内有6个POI点）                        
                        for (i = 0; i < allPois.length; ++i) {
                            console.log((i + 1) + "、" + allPois[i].title + ",地址:" + allPois[i].address);
                        }
                    }, { poiRadius: 1000, numPois: 500 });

                    var ngpoint = gpoint;
                    ngpoint.lng += 0.01;
                    circle.addEventListener('mouseover', function (e) {//区域上显示详情
                        if (gCircleCustom != undefined)
                            mp.removeOverlay(gCircleCustom);
                        function CircleCustomOverlay(point, data) {
                            this._point = point;
                            this._data = data;
                        }
                        CircleCustomOverlay.prototype = new BMap.Overlay();
                        CircleCustomOverlay.prototype.initialize = function (map) {
                            this._map = map;
                            var div = this._div = document.createElement("div");
                            div.style.position = "absolute";
                            div.style.zIndex = BMap.Overlay.getZIndex(this._point.lat);
                            div.style.backgroundColor = "#000";
                            div.style.borderBottom = "1px solid #086ceb";
                            div.style.color = "white";
                            div.style.height = "18px";
                            div.style.padding = "0px";
                            div.style.lineHeight = "14px";
                            div.style.whiteSpace = "nowrap";
                            div.style.MozUserSelect = "none";
                            div.style.fontSize = "12px"



                            var arrow = this._arrow = document.createElement("span");
                            arrow.style.position = "absolute";
                            arrow.style.background = "url(http://map.baidu.com/fwmap/upload/r/map/fwmap/static/house/images/label.png) no-repeat";
                            arrow.style.width = "22px";
                            arrow.style.height = "30px";
                            arrow.style.top = "18px";
                            arrow.style.left = "10px";
                            arrow.style.overflow = "hidden";
                            //div.appendChild(arrow);
                            mp.getPanes().labelPane.appendChild(div);

                            return div;
                        }
                        CircleCustomOverlay.prototype.draw = function () {
                            var map = this._map;
                            var pixel = map.pointToOverlayPixel(this._point);
                            this._div.style.left = (pixel.x - parseInt(this._arrow.style.left)) - 5 + "px";
                            this._div.style.top = pixel.y - 50 + "px";
                        }

                        var circleOverlay = new CircleCustomOverlay(ngpoint, null);
                        gCircleCustom = circleOverlay;
                        mp.addOverlay(circleOverlay);
                    });
                    circle.addEventListener('mouseout', function (e) {
                        if (gCircleCustom != undefined)
                            mp.removeOverlay(gCircleCustom);
                    });
                }
                //mp.addEventListener("click", circleInfo);
            });
        };
    loadOne();
    $('.nav-tabs>li>a').click(function () {
        if ($(this).attr('href') == '#tab_1') {
            loadOne();
        } else if ($(this).attr('href') == '#tab_2') {
            loadTwo();
        }
        else if ($(this).attr('href') == '#tab_3') {
            loadThree();
        }
    });
    if (jQuery().datepicker) {
        $('.date-picker').datepicker({
            rtl: App.isRTL(),
            autoclose: true
        });
    }
});


/* 获取省市县区ID */
function GetPCAId() {
    var _pSelect = $('select[name=horizontalprovince]'),
        _pData = GetData(_pSelect.data('data'), $('option:selected', _pSelect).text()),
        _cSelect = $('select[name=horizontalcity]'),
        _cData = GetData(_cSelect.data('data'), $('option:selected', _cSelect).text()),
        _aSelect = $('select[name=horizontalarea]'),
        _aData = GetData(_aSelect.data('data'), $('option:selected', _aSelect).text()),
        _data = {};
    /*获取省市县区*/
    if (_pData != undefined) {
        _data['pid'] = _pData.ProvinceId;
        _data['cid'] = 0;
        _data['aid'] = 0;
    }
    if (_cData != undefined) {
        _data['cid'] = _cData.CityId;
        _data['aid'] = 0;
    }
    if (_aData != undefined) {
        _data['aid'] = _aData.AreaId;
    }
    return _data;
}

function yaliTest(obj1, obj2) {
    var _ckbox = $('input[type=checkbox]:not(input[name="ylwhere"])', obj1), twhere = '';
    senddata = GetChecked(_ckbox);
    senddata['type'] = 0;

    $.each($('input[type=text]', obj1), function (k, v) {
        if (k == 0) {
            senddata['start'] = v.value;
        } else if (k == 1) {
            senddata['end'] = v.value;
        }
    });

    $('input[type="checkbox"]:checked', obj2).each(function () {
        twhere += '{0},'.format($(this).val());
    });
    twhere = twhere.substring(0, twhere.length - 1);

    senddata['twhere'] = twhere;
    return senddata;
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

/* 设置坐标显示信息 */
function SetMapPoint(point, data, mp, imgt) {
    function ComplexCustomOverlay(point, data, imgt) {
        this._point = point;
        this._data = data;
        this._imgt = imgt;
    }
    ComplexCustomOverlay.prototype = new BMap.Overlay();
    ComplexCustomOverlay.prototype.initialize = function (map) {
        this._map = map;
        var div = this._div = document.createElement("div");
        div.style.position = "absolute";
        div.style.zIndex = BMap.Overlay.getZIndex(this._point.lat);
        //div.style.backgroundColor = "#000";
        div.style.borderBottom = "0px solid #086ceb";
        div.style.color = "white";
        div.style.height = "18px";
        div.style.padding = "0px";
        div.style.lineHeight = "14px";
        div.style.whiteSpace = "nowrap";
        div.style.MozUserSelect = "none";
        div.style.fontSize = "12px"

        var dcount = document.createElement("div");
        dcount.innerText = data.count;
        dcount.style.marginLeft = "10px";
        dcount.style.marginTop = "6px";
        dcount.style.color = '#000';
        div.appendChild(dcount);

        var arrow = this._arrow = document.createElement("img");
        arrow.src = "/Content/img/circle_" + this._imgt + ".png";
        arrow.style.position = "absolute";
        arrow.style.width = (data.count <= 8 ? 8 : data.count + 2) + "px";
        arrow.style.height = (data.count <= 8 ? 8 : data.count + 2) + "px";
        arrow.style.top = "18px";
        arrow.style.left = "10px";
        arrow.style.overflow = "hidden";
        div.appendChild(arrow);
        this._map.getPanes().labelPane.appendChild(div);
        return div;
    }
    ComplexCustomOverlay.prototype.draw = function () {
        var map = this._map;
        var pixel = map.pointToOverlayPixel(this._point);
        this._div.style.left = (pixel.x - parseInt(this._arrow.style.left)) + "px";
        this._div.style.top = pixel.y - 30 + "px";
    }
    var myCompOverlay = new ComplexCustomOverlay(point, data, imgt);
    mp.addOverlay(myCompOverlay);
}

/*把相应的坐标放在一起*/
function GetMapPointData(cdata, data, i, complete) {
    if (mapcircledata.length == 0) {
        mapcircledata.push({ point: { lat: parseFloat(XYIsNull(cdata.Y)), lng: parseFloat(XYIsNull(cdata.X)) }, count: 1, data: data });
    } else {
        for (var item in mapcircledata) {
            var _p = mapcircledata[item].point;
            if (_p.lat != parseFloat(XYIsNull(cdata.Y)) && _p.lng != parseFloat(XYIsNull(cdata.X))) {
                mapcircledata.push({ point: { lat: parseFloat(XYIsNull(cdata.Y)), lng: parseFloat(XYIsNull(cdata.X)) }, count: 1, data: data });
            } else {
                mapcircledata[item].count += 1;
            }
        }
    }
    if (i != data.length - 1) {
        i++;
        GetMapPointData(data[i], data, i, complete);
    } else {
        complete.call(this);
    }
}
function XYIsNull(v) {
    return v == '' ? 0 : v;
}
