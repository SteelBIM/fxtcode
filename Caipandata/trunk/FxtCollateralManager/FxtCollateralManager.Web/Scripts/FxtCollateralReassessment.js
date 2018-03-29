/**
作者:李晓东
时间:2014-04-09
摘要:新建
*/
Date.prototype.addMonths = function (m) {
    var d = this.getDate();
    this.setMonth(this.getMonth() + m);

    if (this.getDate() < d)
        this.setDate(0);
};
var monthNumber = 0, pid = 0, cid = 0, aid = 0;
$(document).ready(function () {
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
                var _append = '<input value="{0}" type="checkbox" name="{2}">{1}'
                    .format(v[data.key], v[data.text], data.id);
                if (data.id != 'wuyetype' && data.id != 'jianzhutype') {
                    var reg = /[\u4E00-\u9FA5]/g,
                        _val = v[data.text].replace(reg, ''),
                        _valSplit;
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
            }
        });
    }
    //押品复估
    $('.creassessments').click(function () {
        $.extendAjax({
            type: 'post',
            url: '/CollateralReassessment/ReassTask',
            data: null
        }, function (crdata) {
            if (crdata.type == 1) {
                alert("新增任务成功!");
            } else {
                alert("新增任务失败!");
            }
        });
    });

    //押品复估管理
    var loadReaManager = function (page) {
        var _sdata = GetPCAId();

        var cpage = $.ActionPage,
        tabfade = $("#tab_1"),
        findtable = $('table', tabfade),
        findbutton = $('button', tabfade),
        nation = $('.pagination', tabfade),
        paginfo = $('#page_info', tabfade);
        findbutton.click(function () {
            monthNumber = $(this).parent().find('input').val();
        });
        cpage.pageSize = $.extendBug ? 1 : 10;
        if (page == undefined) {
            //cpage.pageSize = 1;
            //cpage.pageIndex = 1;
            _sdata['pageIndex'] = 1;
        } else {
            _sdata['pageIndex'] = page.pageIndex;
        }

        _sdata['pageSize'] = cpage.pageSize;

        $.extendAjax({
            type: 'post',
            url: '/CollateralReassessment/GetReassessment',
            data: _sdata
        }, function (ReaData) {
            if (ReaData.type == 1) {
                $.ActionTmpl("tab1_Tmpl", ReaData.data, "tab1_table>tbody");
                $.ProcessPage({
                    pagination: nation,
                    paginfo: paginfo,
                    count: ReaData.count,
                    page: cpage,
                    complete: function (page) {
                        loadReaManager(page);
                    }
                });

                var tr = $('tbody tr', findtable),
                th = $('thead tr', findtable),
                eMonth = new Date().dateformat("yyyyMM"), sMonth = new Date(),
                row = 0, monthData = [];
                sMonth.addMonths(-2);

                for (var i = parseInt(sMonth.dateformat("yyyyMM")) ; i <= parseInt(eMonth) ; i++) {
                    if (findtable.find('th[name="{0}"]'.format(i)).length == 0) {
                        th.append('<th name="{0}">{0}</th>'.format(i));
                    }
                    monthData.push({ month: i });
                }

                function GetMonth(list, month) {
                    var result;
                    for (var item in list) {
                        if (list[item].Months == parseInt(month)) {
                            result = list[item];
                            break;
                        }
                    }
                    return result;
                }
                //console.log(tr.length);
                function loadRowReassessment(_tr) {//逐行加载                    
                    var trid = $(_tr).attr('data-id');
                    if (row < tr.length && trid != undefined) {
                        $.extendAjax({
                            type: 'post',
                            url: '/CollateralReassessment/GetReassessmentMonthNumber',
                            data: { month: new Date().getMonth() + 1, number: trid }
                        }, function (RMNData) {

                            $.each(monthData, function (k1, v1) {
                                var newtd, v = GetMonth(RMNData.data, v1.month);
                                //显示相应标题的值
                                if (v != undefined) {
                                    newtd = $('<td data-id="{1}" data-rid="{2}">{0}</td>'.format(v.Price, trid + v.ID, v.ID));
                                    newtd.click(function () {
                                        var _ft = $('td.tdedit', findtable);
                                        if (_ft.length > 0 && _ft.attr('data-id') != $(this).attr('data-id')) {
                                            var _tdinput = $('input', _ft);
                                            _ft.html(_tdinput.val());
                                            _ft.removeClass('tdedit');
                                        }
                                        var _input = $('input', this), _button = $('<button class="btn green">确定</button>');
                                        if (_input.length == 0) {
                                            $(this).addClass('tdedit');
                                            $(this).html('<div class="col-md-4" style="width:90px"><input value="' + $(this).text()
                                                + '" type="text" class="form-control"/></div>');
                                            //人工复估
                                            _button.click(function () {
                                                var _id = $(this).parent().attr('data-rid'),
                                                    _price = $('input', $(this).parent()).val();

                                                $.extendAjax({
                                                    type: 'post',
                                                    url: '/CollateralReassessment/ChangeReass',
                                                    data: { id: _id, price: _price }
                                                }, function (cdata) {
                                                    console.log(cdata);
                                                    if (cdata.type == 1)
                                                        alert(cdata.message);
                                                });
                                            });
                                            $(this).append(_button);
                                        }
                                    })
                                } else {
                                    newtd = $('<td data-id="{0}" data-rid="{1}"></td>'.format(trid, 0));
                                }
                                $(_tr).append(newtd);
                            });
                            row++;
                            loadRowReassessment(tr[row]);
                        });
                    }
                }
                loadRowReassessment(tr[row]);
            }
        });
    },
    //风险分析
    loadRiskAnalysis = function () {
        var data = GetPCAId();
        data['type'] = 0;
        $.extendAjax({
            type: 'post',
            url: '/CollateralReassessment/RiskAnalysis',
            data: data
        }, function (kaData) {
            $.ActionTmpl("tab2_Tmpl", kaData.data, "tab2_table>tbody");
        });
    },
    //明细
    loadDetails = function () {
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

        cpage.pageSize = $.extendBug ? 1 : 1;

        $(".detailbutton").click(function () {
            if (($.cookie("cityarrid") == "null" || $.cookie("cityarrid") == "")
                       && ($.cookie("itemarrid") == "null" || $.cookie("itemarrid") == "")
                       ) {
                return;
            }
            var _ckbox = $('input[type=checkbox]', _detail),
                sendData = GetChecked(_ckbox), PCA = GetPCAId();
            sendData['companyid'] = company;
            sendData['projectid'] = project;
            sendData['pid'] = PCA['pid'];
            sendData['cid'] = PCA['cid'];
            sendData['aid'] = PCA['aid'];
            sendData['type'] = 0;
            sendData['pageSize'] = cpage.pageSize;
            sendData['pageIndex'] = cpage.pageIndex;
            $.extendAjax({
                url: '/CollateralReassessment/GetReassessmentDetails',
                type: 'post',
                data: sendData
            }, function (rdata) {
                $.ActionTmpl("tab3_Tmpl", rdata.data, "tb_colldetials>tbody");

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
            });

        });
    }
    
    $('.nav-tabs>li').each(function () {
        $('a', this).click(function () {
            if ($(this).attr('href') == '#tab_1') {
                loadReaManager();
            } else if ($(this).attr('href') == '#tab_2') {
                loadRiskAnalysis();
            }
            else if ($(this).attr('href') == '#tab_3') {
                loadReaManager();
            }
        });
    });
    /*加载目城市*/
    CreateItemList(".procheckbox-list");
    SetCheckBox(_checkdata[_checki]);
    loadDetails();
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
        _data['cname'] = _cData.CityName;
        _data['aid'] = 0;
    }
    if (_aData != undefined) {
        _data['aid'] = _aData.AreaId;
    }
    return _data;
}
