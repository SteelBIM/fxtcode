/***
*作者:李晓东
*摘要:新增2013.12.19
      修改人:李晓东 2014.01.20 新增1.extendAjaxFile 2.String.prototype.format 3.修改extendDialog
      修改人:李晓东 2014.01.21 jquery.fn 下新增 extendDialog
      修改人:李晓东 2014.01.24 新增formValidate
      修改人:李晓东 2014.01.27 新增ProcessPage
      修改人:李晓东 2014.02.07 新增BlockUI 达到公用
      修改人:李晓东 2014.02.11 新增jQuery.browser浏览器类型
                                   extendTable 对表格的控制、操作
      修改人:李晓东 2014.02.14 修改extendTable 对列的控制做到公共处理
                               新增:BindingSelect,ActionSelectForm,对extendTable进行优化处理
      修改人:李晓东 2014.02.17 修改:extendTable对控制出现的Bug
                                    extendDialog中的Bug修改
      修改人:李晓东 2014.02.18 新增:Global_fdata,Global_data全局变量,extendUp导入
                               修改:extendTable完善Bug
      修改人:李晓东 2014.02.19 修改:extendUp存在的Bug
      修改人:李晓东 2014.02.20 修改:editRow中表单的提交
                               新增:ActionTmpl 模版控制
      修改人:李晓东 2014.02.21 新增:extendRowData 获取table中tbody tr td 的所有值
                                    JSONSerializeUrl 对json转换成url形式
                                    extendTableSave 扩展table行保存
      修改人:李晓东 2014.02.25 新增:ActionPage,extendBug(是否调试)
                               修改:ProcessPage存在的不足
      修改人:李晓东 2014.02.26 修改:extendTable,BlockUI,extendTableSave中的不足
      修改人:李晓东 2014.02.27 修改:extendTable中的不足
                               新增:ActionSetForm
      修改人:李晓东 2014.02.28 修改:extendTable中存在的不足
                               新增:extendTableSaveRow完成某项后保存行信息
      修改人:李晓东 2014.03.03 修改:extendTable,extendDialog中存在的不足
                               新增:extendAutomplete
      修改人:李晓东 2014.03.05 新增:extendMap地图,ActionList列表,ActionOperta操作
      修改人:李晓东 2014.03.07 新增:Opertaings
      修改人:李晓东 2014.03.14 修改:extendUp上传文件问题
      修改人:李晓东 2014.03.28 修改:extendAutomplete 中的数据处理问题
                                    extendTable 存在的不足
                               新增:extendRemoveAttr 删除指定属性
      修改人:李晓东 2014.03.21 修改:extendTable中新增可以自定义列
      修改人:李晓东 2014.04.02 修改:extendAutomplete中存在的Bug
                                    extendTable新增处理相同楼盘的值同时更改
      修改人:李晓东 2014.05.15  修改:extendMap地图新增属性boundary(是否需要覆盖物)
      修改人:李晓东 2014.06.06  新增:LoadDialogClose,ActionFrom
      修改人:曹青   2014.06.18  新增：删除列a标签增加data-content属性（提示文字）  
                                新增：点击修改 显示、隐藏项层、必填项验证修改、只读字段修改      
      修改人:李晓东 2014.06.25  修改:ActionPage添加属性uploadfileid
      修改人:李晓东 2014.06.26  修改:extendTableSave、extendTable存在的不足
      修改人:李晓东 2014.06.30  修改:ActionList加isBlockUI属性,方便部分地方不需要Loading
**/
(function ($) {
    $.extend({
        extendBug: false,
        extendAjax: function (data, fun) {
            data = jQuery.extend({
                data: "",
                url: "",
                type: "get",
                dataType: "json",
                remote: true,
                cache: true,
                beforeSend: null,
                isBlockUI: true
            }, data || {});
            if (!$.isFunction(data.beforeSend)) {/*发送前处理*/
                data.beforeSend = function (XHR) {
                };
            }
            if (data.isBlockUI)
                $.BlockUI(true);
            $.ajax({
                type: data.type,
                data: data.data,
                url: data.url,
                dataType: data.dataType,
                cache: false,
                context: { data: data },
                beforeSend: data.beforeSend,
                success: function (rdata) {
                    if (data.isBlockUI)
                        $.BlockUI(false);
                    var isstring = typeof (rdata) == "string";
                    if (isstring) {
                        try {
                            rdata = $.parseJSON(rdata);
                        } catch (e) {
                            rdata = rdata;
                        }
                    }
                    if (!isstring && rdata != null && rdata.type == 'error') {
                        $.extendDialog({
                            title: '错误提示',
                            content: rdata.message
                        });
                        //$("#listmessage").html(rdata.message);
                        //$("#listmessage").fadeIn('slow').fadeOut(2000);
                    } /*else if (!isstring && rdata.type == 0) {
                        $.extendDialog({
                            title: '错误提示',
                            content: rdata.message
                        });
                    }*/
                    else {
                        if (!isstring && rdata != null && typeof (rdata.data) == "string") {
                            rdata.data = $.parseJSON(rdata.data);
                        }
                        fun(rdata);
                    }
                },
                error: function (XmlHttpRequest, textStatus, errorThrown) {/*发生错误时处理*/
                    console.log(XmlHttpRequest);
                    console.log(textStatus + ',' + errorThrown);
                    if (data.isBlockUI)
                        $.BlockUI(false);
                }
            });
        },
        extendAjaxFile: function (options) {/*异步上传文件*/
            options = $.extend({
                formid: '#fupload',
                success: null
            }, options || {});
            $.BlockUI(true);
            $(options.formid).ajaxSubmit({
                complete: function () {
                    $.BlockUI(true);
                },
                success: function (data) {
                    if ($.isFunction(options.success)) {
                        options.success(data);
                    }
                    $.BlockUI(false);
                },
                error: function (txt) {
                    $.BlockUI(false);
                }
            })
        },
        customDialogId: $('#customDialog'),
        extendDialog: function (options) {
            options = $.extend({
                id: $.customDialogId,
                modal: true,
                title: '提示',
                content: null,
                minWidth: 600,
                width: 600,
                maxHeight: 550,
                position: { my: "center top", at: "center top", of: window },
                show: {
                    effect: "blind",
                    duration: 1000
                },
                hide: {
                    effect: "explode",
                    duration: 1000
                },
                open: null,
                close: null,
                buttons: null,
                traget: null,
            }, options || {});
            var isOpen = false;
            try {
                isOpen = options.id.dialog("isOpen");
            } catch (e) {

            }
            var url = null,
                buttons = {
                    '确定': function () {
                        $(this).dialog('close');
                    }
                };
            if (isOpen == false) {
                if (options.buttons == null) options.buttons = buttons;
                options.id.html('');
                options.id.html(options.content);
                options.id.dialog({
                    modal: options.modal,
                    title: options.title,
                    minWidth: options.minWidth,
                    maxHeight: options.maxHeight,
                    position: options.position,
                    show: options.show,
                    hide: options.hide,
                    open: options.open,
                    buttons: options.buttons
                });
            } else {
                var newDialog = $("<div></div>");
                newDialog.html('');
                newDialog.html(options.content);
                try {/*是否确定后做其他事件*/
                    url = options.traget.attr('url');
                    if (url != undefined && url != null) {
                        buttons = {
                            '确定': function () {
                                $.extendAjax({
                                    url: options.traget.attr('url'),
                                    type: 'POST'
                                }, function (rdata) {
                                    $.ActionList({
                                        toid: options.traget.attr('data-toid'),
                                        tourl: options.traget.attr('data-rloadurl'),
                                        tmplid: options.traget.attr('data-totempl')
                                    });
                                    newDialog.dialog('close');
                                });
                            },
                            '取消': function () {
                                newDialog.dialog('close');
                            }
                        };
                    }
                } catch (e) {

                }
                newDialog.dialog({
                    modal: options.modal,
                    title: options.title,
                    minWidth: options.minWidth,
                    maxHeight: options.maxHeight,
                    position: options.position,
                    show: options.show,
                    hide: options.hide,
                    open: function () {
                        if ($.isFunction(options.open)) {
                            options.open.call(this);
                        }
                    },
                    buttons: buttons
                });
            }
        },
        extendChecked: function (traget) {/*处理全选*/
            var _t = $(traget).parent().parent().parent().parent().parent().find('.checkbox-list');
            $.each(_t.find('input[type=checkbox],input[type=radio]'),
            function (i, field) {
                var _this = $(this).attr("checked", traget.checked);
                $.uniform.update(_this);
            });
        },
        extendParentChecked: function (traget) {/*如果某个父级下的所有子集都选择了,父级本身也要选中*/
            var _t = $(traget).parent().parent().parent().parent().parent().find('.checkbox-list'),
                _tcount = _t.find('input[type=checkbox],input[type=radio]').size(), _tpcount = 0;
            $.each(_t.find('input[type=checkbox],input[type=radio]'),
            function (i, field) {
                if ($(this).attr("checked") == 'checked')
                    _tpcount++;
            });
            if (_tcount == _tpcount)
                $.uniform.update($(traget).attr("checked", true));
        },
        ProcessPage: function (options) {
            options = $.extend({
                pagination: $(".pagination"),
                paginfo: $("#page_info"),
                count: 0,
                page: null,
                complete: null
            }, options || {});
            /**总页数**/
            var pageIndex = options.page.pageIndex,
                pageCount = Math.ceil(options.count / options.page.pageSize);

            var startCount = parseInt(pageIndex > 9 ? parseInt(pageIndex - 4) : 1),
                endCount = parseInt(startCount + 9) > pageCount ? pageCount : parseInt(startCount + 9);

            var firstTmp = '<li class="prev {0}"><a href="javascript:void(0);" title="首页"><i class="icon-backward"></i></a></li>',
                lastTmp = '<li class="next {0}"><a href="javascript:void(0);" title="尾页"><i class="icon-forward"></i></a></li>',
                prevTmp = '<li class="prev {0}"><a href="javascript:void(0);" title="上一页"><i class="icon-caret-left"></i></a></li>',
                nextTmp = '<li class="next {0}"><a href="javascript:void(0);" title="下一页"><i class="icon-caret-right"></i></a></li>',
                pageTmp = '<li {0}><a href="javascript:void(0);">{1}</a></li>';

            if (pageCount > 1) {
                var pageInfo = firstTmp.format('') + prevTmp.format('');
                /*首页*/
                if (options.page.pageIndex == 1) {
                    pageInfo = firstTmp.format('disabled') + prevTmp.format('disabled');
                }

                for (var i = startCount; i <= endCount; i++) {
                    if (options.page.pageIndex == i) {
                        pageInfo = pageInfo + pageTmp.format('class="active"', i);
                    }
                    else {
                        pageInfo = pageInfo + pageTmp.format('', i);
                    }
                }
                /**尾页**/
                if (options.page.pageIndex == pageCount) {
                    pageInfo = pageInfo + nextTmp.format('disabled') + lastTmp.format('disabled');
                }
                else {
                    pageInfo = pageInfo + nextTmp.format('') + lastTmp.format('');
                }
                options.pagination.html(pageInfo);
                options.paginfo.html("当前第{0}页,共{1}页".format(options.page.pageIndex, pageCount));
                var lilist = options.pagination.find('li');
                lilist.click(function () {
                    if ($(this).find('.icon-backward').length > 0) {/*首页*/
                        options.page.pageIndex = 1;
                    }
                    else if ($(this).find('.icon-forward').length > 0) {/*尾页*/
                        options.page.pageIndex = pageCount;
                    } else if ($(this).find('.icon-caret-left').length > 0) {/*上一页*/
                        var currentpage = parseInt($('.active', $(this).parent().parent().parent()).text());
                        options.page.pageIndex = currentpage - 1;
                    } else if ($(this).find('.icon-caret-right').length > 0) {/*下一页*/
                        var currentpage = parseInt($('.active', $(this).parent().parent().parent()).text());
                        options.page.pageIndex = currentpage + 1;
                    } else {
                        options.page.pageIndex = $(this).text();
                    }
                    if (($(this).attr('class') == undefined ||
                   $(this).attr('class').indexOf('disabled') == -1) &&
                   ($(this).attr('class') == undefined ||
                   $(this).attr('class').indexOf('active') == -1)) {
                        if ($.isFunction(options.complete)) {
                            options.complete(options.page);
                        }
                    }
                });
            }
        },
        formValidate: function (options) {
            options = $.extend({
                form: null,
                torules: null,
                ralodurl: null,
                csStyle: true,
                complete: null,
                Page: null,
                message: '验证成功!正在提交...',
                time: { Enable: false, Millisecond: 5 },
                colse: true,
                isRalod:true
            }, options || {});
            if (options.csStyle)
                App.init();
            var aerror = $('.alert-error', options.form);
            var asuccess = $('.alert-success', options.form);
            options.form.validate({
                errorElement: 'span',
                errorClass: 'help-block',
                focusInvalid: false,
                rules: options.torules,
                ignore: '',
                invalidHandler: function (event, validator) {
                    asuccess.hide();
                    aerror.show();
                    App.scrollTo(aerror, -200);
                },
                highlight: function (element) {
                    //alert(element.name);
                    $(element).closest('.form-group').addClass('has-error');
                },
                unhighlight: function (element) {
                    $(element).closest('.form-group').removeClass('has-error');
                },
                success: function (label) {
                    label.closest('.form-group').removeClass('has-error');
                },
                submitHandler: function (form) {

                    var butSubmit = $("button[type=submit]", form);
                    butSubmit.attr({ "disabled": "disabled" });
                    //return false;

                    var span = asuccess.find('span'), formType = $(form).attr('data-type');
                    span.text(options.message);
                    asuccess.show();
                    aerror.hide();
                    var butText = $.trim($(form).find('.green').text());
                    var submitData = $(form).find('input[type=text],input[type=hidden],input[type=password]').serialize();
                    var select = $(form).find('select');/*下拉框*/

                    $.each(select, function (i, field) {
                        var svalue = field.value == '' || field.value == null ? '0' : field.value;
                        if (submitData == '')
                            submitData += "{0}={1}".format(field.name, svalue);
                        else
                            submitData += "&{0}={1}".format(field.name, svalue);
                    });

                    var checkbox = $(form).find('input:checked'), checkedval = '';/*所有的checkbox*/
                    if (checkbox.length > 0) {
                        var chname = '';
                        checkbox.each(function (i, field) {
                            if ($(this).val() != 'on') {
                                chname = field.name;
                                checkedval += $(this).val() + ',';
                            }
                        });
                        checkedval = checkedval.substr(0, checkedval.length - 1);
                        if (submitData == '')
                            submitData += "{0}={1}".format(chname, checkedval);
                        else
                            submitData += "&{0}={1}".format(chname, checkedval);
                    }
                    if (formType != undefined)
                        submitData += '&type={0}'.format(formType);
                    var page = '';
                    if (options.Page != null)
                        page = "&pageSize={0}&pageIndex={1}"
                            .format(options.Page.pageSize, options.Page.pageIndex);
                    //console.log(submitData);
                    $.extendAjax({
                        url: $(form).attr('action'),
                        type: 'POST',
                        data: submitData + page
                    }, function (rdata) {
                        butSubmit.removeAttr("disabled");
                        if ($.isFunction(options.complete)) {
                            options.complete(rdata, {
                                url: $(form).attr('action'),
                                data: submitData
                            }, this);
                        }
                        if (rdata.type == 1) {
                            span.text('操作成功');
                            setTimeout(function () {
                                if (options.colse)
                                    $.customDialogId.dialog('close');
                                //是否需要刷新父级内容
                                if (options.isRalod) {
                                    if (options.ralodurl != null || options.ralodurl != '') {
                                        $.mainContent.extendLoad({ url: options.ralodurl });
                                    }
                                }
                                asuccess.hide();
                            }, 1500);
                        }
                        else {
                            if (rdata.type == 0) {
                                asuccess.hide();
                                aerror.show();
                                var errorspan = aerror.find('span');
                                if (rdata.message == null || rdata.message == '')
                                    errorspan.text("操作失败");
                                else
                                    errorspan.text(rdata.message);
                            }
                        }
                    });
                }
            });
        },
        BlockUI: function (block) {/**加载提示...**/
            var isOpen = false,
                diaBlockUI = $.customDialogId.find('.portlet-body'),
                ActionBlockUI = $(".portlet-body");//$('.tab-content').length != 0 ? $('.tab-content') :
            try {
                isOpen = $.customDialogId.dialog("isOpen");
            } catch (e) {

            }
            if (block) {
                if (isOpen) {
                    App.blockUI(diaBlockUI);
                } else {
                    App.blockUI(ActionBlockUI);
                }
            } else {
                if (isOpen) {
                    App.unblockUI(diaBlockUI);
                } else {
                    App.unblockUI(ActionBlockUI);
                }
            }
        },
        ActionSetForm: function (data) {/*表单赋值*/
            for (var item in data) {
                var name = item.toLowerCase();
                if (name == 'purviewproductid')/*如果是用户权限*/ {
                    $.each($('.nav-tabs').find('li'), function () {
                        if (parseInt($(this).attr('Id')) == data[item] && $(this).find('img').length == 0) {
                            var ok_img = '<img src="/Content/img/icon_ok.png" style="margin-top:-5px;padding-left:8px;"/>';
                            $(this).find('a').append(ok_img);
                        }
                    });
                }
                /*input*/
                var $input = $('input[name=' + name + ']'),
                    inputtype = $input.attr('type');
                //console.log(name + ',' + data[item] + ',' + inputtype + ',' + $('input[name=' + name + ']').size());
                if (inputtype == 'checkbox' || inputtype == 'radio') {
                    var _setsplit = data[item].split(',');
                    $.TempCookie(name, data[item]);
                    $.each($input, function (i, field) {
                        if (IsExists(_setsplit, $(this).val())) {
                            var _this = $(this).attr("checked", true);
                            $.uniform.update(_this);
                        }
                    });
                }
                else {
                    $input.val(data[item]);
                }
                /*select*/
                var selects = $('select[name=' + $.trim(name) + ']');
                if (selects.length > 0) {
                    /*if (name == 'parentid') {}else {}等待执行相关操作*/
                    $.TempCookie(name, data[item]);
                    $("option[value='" + data[item] + "']", selects).attr("selected", true);
                    selects.change(); /*手动触发*/
                }
            }
            function IsExists(data, val) {
                var _flag = false;
                for (var item in data) {
                    if (data[item] == val) {
                        _flag = true;
                        break;
                    }
                }
                return _flag;
            }
        },
        ActionSelectForm: function (options) {
            options = $.extend({
                formid: '#form',
                name: null,
                url: null,
                key: 'Id',
                val: null,
                data: null,
                type: '',
                target: '',
                operats: null,
                complete: function () { }
            }, options || {});
            var _selects = $('select[name=' + options.name + ']');
            if (options.operats != undefined && options.operats != '' && options.operats != null) {
                var set = $(options.formid)
                if (options.operats == 'set') {
                    set.find('input[name=' + options.name + ']').val(options.val);
                }
                if ($.isFunction(options.complete)) {
                    options.complete.call(this);
                }
            }
            else if (_selects.length > 0 && options.val != null) {
                $.extendAjax({
                    url: options.url,
                    type: 'POST'
                }, function (rdata) {
                    if (rdata != null) {
                        rdata = rdata.data;
                        _selects.data('data', rdata);
                        //console.log(JSON.stringify(rdata));
                        _selects.empty();
                        var newoption = '<option value="{0}" {2}>{1}</option>';
                        _selects.append(newoption.format("", "-请选择-", ""));
                        for (var item in rdata) {
                            var datastr = "";
                            if (options.data != null && options.data != "") {
                                console.log(options.data + ":" + rdata[item][options.data]);
                                datastr = options.data + "='" + rdata[item][options.data] + "'";
                            }
                            _selects.append(newoption.format(rdata[item][options.key], decodeURIComponent(rdata[item][options.val]), datastr));
                        }
                        if ($.cookie(options.name) != undefined) {
                            _selects.find("option[value='" + $.cookie(options.name) + "']").attr("selected", true);
                        }
                        if ($.isFunction(options.complete)) {
                            options.complete.call(this);
                        }
                    }
                });
            } else if (options.type != '') {
                $.extendAjax({
                    url: options.url,
                    type: 'POST'
                }, function (rdata) {
                    rdata = rdata.data;
                    /*console.log(JSON.stringify(rdata));*/

                    var _ot = $('#' + options.target);
                    _ot = _ot.length == 0 ? $('.' + options.target) : _ot;

                    if (_ot.length > 0) {
                        _ot.empty();
                        var plist = '<ul>', list = '',
                            pcheckbox = '<input type="{0}" name="chall"/>{1}',
                            ccheckbox = '<input type="{0}" name="{1}" value="{2}" group="{1}" {4}/>{3}';
                        $.each(rdata, function (i, n) {
                            if (rdata.length == 1 && n.Text == '') {
                                list += '<li><div><label class="checkbox-inline">{0}</label></div>'
                                .format(pcheckbox.format(options.type, "全选"));
                            }
                            else {
                                list += '<li><div><label class="checkbox-inline">{0}</label></div>'
                                .format(pcheckbox.format(options.type, n.Text));
                            }

                            if (n.Children.length > 0) {
                                list += '<div class="checkbox-list"><ul>';
                                var clist = '';
                                $.each(n.Children, function (i, cn) {
                                    var checked = CreateChecked(options.name, cn.Id);
                                    clist += '<li>{0}</li>'
                                        .format(ccheckbox
                                        .format(options.type, options.name, cn.Id, cn.Text, checked));
                                });
                                list += clist + "</ul><div>";
                            }
                            list += "</li>";
                        });
                        plist += list + '</ul>';
                        _ot.append(plist);
                        App.init();
                    }
                    if ($.isFunction(options.complete)) {
                        options.complete.call(this);
                    }
                });
            }
            else {
                if ($.isFunction(options.complete)) {
                    options.complete.call(this);
                }
            }
            function CreateChecked(n, v) {
                var list = '';
                if ($.cookie(n) != undefined) {
                    $.each($.cookie(n).split(','), function (i, v1) {
                        if (v1 == v)
                            list = 'checked="checked"';
                    });
                }
                return list;
            }
        },
        TempCookie: function (n, v) {/*处理临时cookie*/
            $.cookie(n, v, { path: '/', expires: 60 });
        },
        ClearCookie: function () {/*清空所有Cookie*/
            $.each(document.cookie.split('; '), function (i, v) {
                var vsplit = v.split('=');
                $.cookie(vsplit[0], null, { path: '/' });
            });
        },
        BindingSelectI: 0,
        BindingCall: null,
        CollateralId: null,
        PageDisableId: null,
        /*Global_fdata: null,备用于上传的时候使用*/
        /*Global_data: null,备用于上传的时候使用*/
        Global_SetNumber: 0,/*设置页数次数*/
        BingdingChange: 0,
        BindingSelect: function (bsComplete) {
            if ($.BindingCall == null)
                $.BindingCall = bsComplete;
            if ($.ActionSelectList.length > 0) {
                var data = $.ActionSelectList[$.BindingSelectI];

                $.ActionSelectForm({
                    name: data.name,
                    url: data.url,
                    val: data.val,
                    data: data.data,
                    type: data.type,
                    target: data.target,
                    operats: data.operats,
                    formid: data.formid,
                    key: data.key,
                    complete: function () {

                        if (data.children != undefined) {
                            var select = $('select[name=' + data.name.toLowerCase() + ']'),
                                    _childen = data.children;
                            if (select.length > 0) {
                                //select.unbind('change');
                                select.change(function () {
                                    $.ActionSelectForm({
                                        name: _childen.name,
                                        url: _childen.url.format(($(this).val() == '' ? 0 : $(this).val())),
                                        val: _childen.val,
                                        type: _childen.type,
                                        target: _childen.target,
                                        operats: _childen.operats,
                                        formid: _childen.formid,
                                        key: _childen.key,
                                        complete: function () {
                                            if (data.children.children != undefined) {
                                                var selectch = $('select[name=' + data.children.name.toLowerCase() + ']'),
                                                        _childen = data.children.children;
                                                if (selectch.length > 0) {
                                                    //console.log(data.children.name.toLowerCase());
                                                    selectch.unbind('change');
                                                    selectch.bind('change', function () {
                                                        $.ActionSelectForm({
                                                            name: _childen.name,
                                                            url: _childen.url.format(($(this).val() == '' ? 0 : $(this).val())),
                                                            val: _childen.val,
                                                            type: _childen.type,
                                                            target: _childen.target,
                                                            operats: _childen.operats,
                                                            formid: _childen.formid,
                                                            key: _childen.key
                                                        });
                                                    });
                                                    /*只触发一次*/
                                                    if ($.BingdingChange == 0) {
                                                        selectch.change();
                                                        $.BingdingChange = 1;
                                                    }
                                                }
                                            }
                                        }
                                    });
                                });
                            }
                        }
                        if ($.BindingSelectI == $.ActionSelectList.length - 1) {
                            $.BindingSelectI = 0;
                            if ($.isFunction(bsComplete)) {
                                bsComplete.call(this);
                                $.BindingCall = null;
                            }
                        }
                        else {
                            $.BindingSelectI++;
                            $.BindingSelect($.BindingCall);
                        }
                    }
                });
            }
            else
                bsComplete.call(this);
        },
        ActionTmpl: function (templid, data, toid) {/*模版遍历加载*/
            var sharp = "#";
            if (toid.substring(0, 1) == '.') {
                sharp = '';
            }
            $(sharp + toid).html('');
            var tmpls = $('#' + templid).tmpl(data, {
                dataFromat: function (format) {
                    format = format == null || format == '' || format == undefined ?
                        'yyyy-MM-dd hh:mm:ss' : format;
                    var df;
                    if (this.data.CreateDate != undefined) {
                        df = new Date(this.data.CreateDate).dateformat(format);
                    } else if (this.data.createdate != undefined) {
                        df = new Date(this.data.createdate).dateformat(format);
                        alert(df)
                    }
                    return df;
                },
                dataFileSize: function () {
                    if (this.data.FileSize != undefined) {
                        if (this.data.FileSize >= 1024000000) {
                            return (this.data.FileSize / 1024000000).toFixed(2) + ' GB';
                        }
                        if (this.data.FileSize >= 1024000) {
                            return (this.data.FileSize / 1024000).toFixed(2) + ' MB';
                        }
                        return (this.data.FileSize / 1024).toFixed(2) + ' KB';
                    }
                    return '';
                },
                dataProcess: function (stype) {
                    var tds = "0%", pnum = 0;
                    this.data.failurecount = (this.data.failurecount == null) ? 0 : this.data.failurecount;
                    var endcount = parseInt(this.data.successcount) +
                        parseInt(this.data.failurecount)
                    if (this.data.count != '0' && this.data.count != '' && endcount > 0) {
                        var matval = Math.ceil((endcount / this.data.count * 100));
                        var colormes = "background-color:#009900;";
                        if (matval < 100) {
                            colormes = "background-color:#FFCC33;";
                        }
                        matval = (matval > 100) ? 100 : matval;
                        bfbnum = matval;
                        var mathcer = "{0}%".format(matval);
                        mathcer = ("<p class=\"procescustom\" style=\"" + colormes + "width:" + (matval * 2) + "px \">") + ((matval < 20) ? ("</p>" + mathcer) : (mathcer + "</p>"));
                        tds = mathcer;
                        pnum = matval;
                    }
                    return (stype == "num") ? pnum : tds;
                },
                dataPercent: function (item) {;
                    if (item.FileSuccessCount > 0) {
                        return "{0}%".format(Math.ceil((item.FileSuccessCount / item.Count) * 100));
                    } else {
                        return "0%";
                    }
                }
            });
            if (typeof (toid) == "string") {
                if (tmpls.length != 0) {
                    tmpls.appendTo(sharp + toid);
                }
            } else {
                tmpls.appendTo(sharp + toid);
            }
        },
        ActionList: function (options) {
            //(options.tmplid != undefined) ? options.tmplid :
            options = $.extend({
                tourl: '',
                toid: 'list>tbody',
                tmplid: 'template',
                sdata: $.ActionPage,
                complete: null,
                isBlockUI:true
            }, options || {});

            $(".portlet > .portlet-title > .tools > a.reload").click(function () {
                $.ActionList(options);
            });
            $.extendAjax(
            {
                url: options.tourl,
                type: 'POST',
                data: options.sdata,
                isBlockUI:options.isBlockUI
            }, function (rdata) {
                var count = rdata.count;
                $.ProcessPage({
                    count: count,
                    page: options.sdata,
                    complete: function () {
                        $.ActionList(options);
                    }
                });
                /** 数据展示 **/
                //绑定模版
                //console.log(options.tmplid);
                $.ActionTmpl(options.tmplid, rdata.data, options.toid);
                $.ActionOperta({ toid: "#" + options.toid, options: options });
                if ($.isFunction(options.complete)) {
                    options.complete(rdata.data);
                }
            });
        },
        ActionOperta: function (options) {
            options = $.extend({
                toid: '',
                ActionFormId: '#form',
                ActionFormRules: null,
                ReloadUrl: null,
                Url: null,
                TmplId: '#template-form',
                options: null,
                ActionUrl: null
            }, options || {});
            $(options.toid).find('a:not(.notunbinding)').click(function () {
                options.ActionUrl = $(this).attr('url');
                var ActionType = $(this).attr('data-type');
                options.ReloadUrl = $(this).attr('data-rloadurl') != undefined ? $(this).attr('data-rloadurl') : options.ReloadUrl;
                if (ActionType != undefined) {
                    switch (ActionType.toUpperCase()) {
                        case 'D':
                            $.Opertaings({
                                traget: $(this),
                                ActionType: ActionType,
                                title: $(this).attr('data-title'),
                                content: $(this).attr('data-content'),
                                AOoptions: options
                            });
                            break;
                        case 'U':
                            options.TmplId = $(this).attr('data-tmplform') != undefined ? $(this).attr('data-tmplform') : options.TmplId;
                            options.ActionFormId = $(this).attr('data-formid') != undefined ? $(this).attr('data-formid') : options.ActionFormId;
                            options.ActionFormRules = eval($(this).attr('data-rules'));
                            options.Url = $(this).attr('data-url');
                            $.Opertaings({
                                ActionType: ActionType,
                                title: $(this).attr('data-title'),
                                AOoptions: options
                            });

                            //显示、隐藏项
                            $("div[key=modify_hide],span[key=modify_hide]").hide();
                            $("div[key=modify_show],span[key=modify_show]").show();
                            //禁用不可修改项目
                            $("input[key=modifydisabled]").attr("disabled", "disabled");
                            $("select[key=modifydisabled]").attr("disabled", "disabled");
                            //去掉非必填项目                           
                            $("input[key=addrequired]").each(function (i, item) {
                                options.ActionFormRules[$(item).attr("name")].required = false;
                            });
                            break;
                        case 'C':
                            options.Url = $(this).attr('data-url');
                            $.Opertaings({
                                ActionType: ActionType,
                                title: $(this).attr('data-title'),
                                AOoptions: options
                            });
                            //还原必填项
                            $("input[key=addrequired]").each(function (i, item) {
                                options.ActionFormRules[$(item).attr("name")].required = true;
                            });
                            break;
                    }
                }
            });
        },
        Opertaings: function (options) {
            $.ClearCookie();
            options = $.extend({
                ActionType: '',
                title: '',
                content: '确定删除吗?',
                traget: null,
                AOoptions: {
                    toid: '',
                    ActionFormId: '#form',
                    ActionFormRules: null,
                    ReloadUrl: null,
                    Url: null,
                    TmplId: '#template-form',
                    options: null,
                    ActionUrl: null
                },
                complete: null,
                callSetForm: true
            }, options || {});
            switch (options.ActionType.toUpperCase()) {
                case 'D':
                    $.extendDialog({
                        traget: options.traget,
                        content: options.content,
                        buttons: {
                            '确定': function () {
                                $.extendAjax({
                                    url: options.AOoptions.ActionUrl,
                                    type: 'POST'
                                }, function (rdata) {
                                    $.customDialogId.dialog('close');
                                    if (rdata.type == 1) {
                                        $.mainContent.extendLoad({ url: options.AOoptions.ReloadUrl });
                                    } else {
                                        alert(rdata.message);
                                    }
                                });
                            },
                            '取消': function () {
                                $.customDialogId.dialog('close');
                            }
                        }
                    });
                    break;
                case 'U':
                    $.extendDialog({
                        title: options.title,
                        content: $(options.AOoptions.TmplId).html(),
                        buttons: [],
                        open: function () {

                            var form = $(options.AOoptions.ActionFormId);

                            if (form.length == 1) {
                                form.attr('action', options.AOoptions.Url);
                            } else {
                                alert('Form表单ID有重复,请检查!');
                                return false;
                            }

                            $.BindingSelect(function () {
                                $.extendAjax({
                                    url: options.AOoptions.ActionUrl,
                                    type: 'POST'
                                },
                                function (rdata) {
                                    if (options.callSetForm)
                                        $.ActionSetForm(rdata.data); /*设置修改值*/
                                    setTimeout(function () {
                                        $.ActionFrom({
                                            toid: options.AOoptions.ActionFormId,
                                            torules: options.AOoptions.ActionFormRules,
                                            ralodurl: options.AOoptions.ReloadUrl
                                        });
                                        $.LoadDialogClose();
                                    }, 800);
                                    if ($.isFunction(options.complete)) {
                                        options.complete(rdata.data);
                                    }
                                });
                            });
                        }
                    });
                    break;
                case 'C':
                    $.extendDialog({
                        title: options.title,
                        content: $(options.AOoptions.TmplId).html(),
                        buttons: [],
                        open: function () {
                            var form = $(options.AOoptions.ActionFormId);
                            $(options.AOoptions.ActionFormId).attr('action', options.AOoptions.Url);

                            $.BindingSelect(function () {

                                $.ActionFrom({
                                    toid: options.AOoptions.ActionFormId,
                                    torules: options.AOoptions.ActionFormRules,
                                    ralodurl: options.AOoptions.ReloadUrl
                                });
                                $.LoadDialogClose();

                            });
                        }
                    });
                    break;
            }
        },
        ActionPage: { pageSize: 10, pageIndex: 1, uploadfileid: -1 },
        ActionFrom: function (options) {
            options = $.extend({
                toid: '#form',
                torules: '',
                ralodurl: null,
                complete: null,
                isRalod: true
            }, options || {});
            $.formValidate({
                form: $(options.toid),
                torules: options.torules,
                ralodurl: options.ralodurl,
                csStyle: true,
                complete: options.complete,
                isRalod: options.isRalod
            });
        },
        LoadDialogClose: function () {
            $(".form-actions").find('.default').click(function () {
                $.customDialogId.dialog('close');
            });
        },
        extendUp: function (complete) {/*导入*/
            var inputfiles = $("#allupupload input[type=file]"), inputfile;
            if (inputfiles.length == 0) {/*只保留一个,其余剔除*/
                var inputfile = $('<input type="file" name="file" id="allupupload_file" multiple="multiple" />');
                $("#allupupload").append(inputfile);
            } else {
                inputfile = inputfiles;
            }
            inputfile.click();
            inputfile.unbind('change');
            inputfile.bind('change', function () {
                $.extendAjaxFile({//文件上传
                    formid: '#allupupload',
                    success: function (filedata) {

                        if (filedata != null && filedata != "") {
                            setTimeout(function () {
                                $.extendAjax({//文件解析
                                    url: '/Upload/ExcelUp',
                                    type: 'post',
                                    data: {
                                        filepath: filedata.filepath,
                                        filename: filedata.filename
                                    }
                                }, function (data) {
                                    //删除自己,然后在绑定一个新的file,防止change不触发
                                    if ($.isFunction(complete)) {
                                        complete(data);
                                    }
                                });
                            }, 600);
                        }
                    }
                });/**/
            });
        },
        JSONSerializeUrl: function (obj) {/*JSON转换成Url*/
            var t = typeof (obj);
            if (t != "object" || obj === null) {
                return "";
            } else {
                var url = "", serializeformat = "{0}={1}&";

                $.each(obj, function (k, v) {
                    url += serializeformat.format(k, v);
                });

                return url.substring(0, url.length - 1);
            }
        },
        extendTableSaveRow: function (options) {
            options = $.extend({
                nRow: null,
                edit: false,
                del: false
            }, options || {});
            var input = null, select = null, jqTds = $('>td', options.nRow);
            $.each(jqTds, function (i, v) {
                if (i > 0) {
                    input = $('input', this), select = $('select option:selected', this);
                    if (input.length > 0) {
                        jqTds[i].innerHTML = input.val();
                    }
                    if (select.length > 0) {
                        var value = select.val();
                        var text = select.text(); // 选中文本
                        jqTds[i].innerHTML = value != null && value != '' ? text : '';
                    }
                    if (i == jqTds.length - 1) {
                        var html = '';
                        if (options.edit)
                            html = html + '<a class="btn edit" href="javascript:;">编辑</a>';
                        if (options.del)
                            html = html + '<a class="btn delete" href="javascript:;">删除</a>';
                        jqTds[i].innerHTML = html;
                    }
                    $(jqTds[i]).css({ height: 36 });/*此处可以修改到自动修改高度*/
                }
            });
        },
        extendTableSave: function (options) {/*table行操作*/
            options = $.extend({
                nRow: null,
                complete: null,
                firstNumber: 0,
                edit: false,
                cancel: false,
                remove: false
            }, options || {});
            var saveCurrent = $(options.nRow).extendRowData({ firstNumber: options.firstNumber }),
                saveUrl = $(options.nRow).parents('table').attr('data-saveurl'),
                saveLastTd = $('>td:last', options.nRow),
                lastTdHtml = saveLastTd.html(),
                customTd = $(">td[data-type='custom']", options.nRow),
                customData = '';
            $.each(customTd, function (_cusi) {
                var _t = $(this).attr('data-ctype');
                if (_t == 'number') {
                    customData += "{type:'" + _t + "',column:'" + $(this).attr('name') + "',value:" + $.getTdVal(this) + "},";
                } else {
                    customData += "{type:'" + _t + "',column:'" + $(this).attr('name') + "',value:'" + $.getTdVal(this) + "'},";
                }
            });
            customData = '[' + customData + '{column:"ProjectId",value:' + saveCurrent['projectid'] + '}]';
            //console.log(saveCurrent);
            //return;
            if (saveUrl != undefined && saveUrl != null && saveUrl != '') {
                saveLastTd.html('处理中...');
                //console.log(saveCurrent);
                $.extendAjax({//保存押品
                    url: saveUrl,
                    type: 'post',
                    data: $.JSONSerializeUrl(saveCurrent)
                }, function (saveData) {
                    if (saveData.type == 1) {
                        saveLastTd.html('处理成功!');
                        $('>td:eq(12)', options.nRow).css({ backgroundColor: '' });
                    } else {
                        saveLastTd.html('处理失败!');
                    }
                    $.extendAjax({//保存自定义列
                        url: '/Collateral/SetCutsomColumnVal',
                        type: 'post',
                        data: {
                            cid: saveCurrent['CityId'],
                            type: $(options.nRow).attr('data-matchstatus'),
                            data: customData
                        }
                    }, function (savePData) {
                    });
                    setTimeout(function () {
                        saveLastTd.html(lastTdHtml);
                        $.extendTableSaveRow({
                            nRow: options.nRow,
                            edit: options.edit,
                            cancel: options.cancel,
                        });
                        if ($.isFunction(options.complete)) {
                            options.complete(saveData, options.nRow);
                            if (options.remove)
                                $(options.nRow).remove();
                        }
                    }, 1500);
                });/**/
            } else {
                $.extendDialog({
                    title: '提示',
                    content: '保存的URL地址不能为空!'
                });
            }
        },
        extendRemoveAttr: function (options) {
            options = $.extend(
            {
                target: null,
                dataColumn: null
            }
            , options || {});
            $.each(options.dataColumn, function (k, v) {
                options.target.removeAttr(v);
            });
        },
        getTdVal: function (td) {/*获得指定列的值*/
            if ($('input', td).length > 0) {
                return $('input', td).val();
            } else {
                return $(td).text();
            }
        }
    });
    $.extend($.fn, {
        extendLoad: function (options) {
            options = $.extend(
            {
                url: '/Home/Content',
                data: null,
                complete: null
            }
            , options || {});
            $(this).html('加载中...');
            this.load(options.url, options.data, function () {
                if ($.isFunction(options.complete)) {
                    options.complete.call(this);
                }
            });
        },
        extendDialog: function (options) {

            options = $.extend({
                id: $.customDialogId,
                modal: true,
                title: '提示',
                content: null,
                minWidth: 500,
                maxHeight: 550,
                position: { my: "center top", at: "center top", of: window },
                show: {
                    effect: "blind",
                    duration: 1000
                },
                hide: {
                    effect: "explode",
                    duration: 1000
                },
                open: null,
                close: null,
                buttons: null,
                traget: null,
            }, options || {});
            if (options.content != null)
                $(this).html(options.content);
            this.dialog(options);

        },
        extendRowData: function (options) {/*获取table中tbody tr td 的所有值*/
            options = $.extend({
                headChinese: false,
                nameTohead: false,
                firstNumber: 0
            }, options || {});
            //console.log(options.firstNumber);
            var tds = $('>td', this), arr = {}, tbth = $('thead tr th', $(this).parent().parent());
            tds.each(function (j) {
                if (j > options.firstNumber && j != tds.length - 1) {
                    var dataVal = $(this).text(),
                        input = $('input', this),
                        select = $('select option:selected', tds[j]),
                        text = $.trim($(tbth[j]).text()),
                        tdname = $(this).attr('name'),
                        tdnamevalue = $(this).attr('data-val');

                    if (options.nameTohead == false) {/*json 如:{Number:"押品编号"}*/
                        /*td列上是否有name属性,且需求不是要的中文标题*/
                        if (tdname != undefined && options.headChinese == false) {
                            text = tdname;
                        }
                        arr[text] = '';
                        if (input.length > 0) {/*是input元素*/
                            arr[text] = $.trim(input.val());
                        }
                        if (select.length > 0 && select.val() != '') {/*是select元素*/
                            arr[text] = $.trim(select.text());
                        }
                        if (input.length == 0 && select.length == 0) {
                            if (dataVal != undefined) {
                                arr[text] = $.trim(dataVal);
                            }
                            else {
                                arr[text] = $.trim($(this).html());
                            }
                        }
                        /*当td中有data-val时,且需求不是要的中文标题*/
                        if (tdnamevalue != undefined && options.headChinese == false) {
                            if (tdname != undefined && tdname.toLowerCase() == 'projectname') {
                                arr['projectid'] = tdnamevalue;
                            } else if (tdname != undefined && tdname.toLowerCase() == 'buildingnumber') {
                                arr['buildingid'] = tdnamevalue;
                            } else if (tdname != undefined && tdname.toLowerCase() == 'roomnumber') {
                                arr['roomid'] = tdnamevalue;
                            } else {
                                arr[text] = tdnamevalue;
                            }
                        }
                    } else if (tdname != undefined && options.nameTohead) {
                        arr[tdname] = text;
                    }
                }
            });
            if (options.nameTohead == false) {
                var trMatchStatus = $(this).attr('data-matchstatus');
                if (trMatchStatus != undefined && trMatchStatus != null && trMatchStatus != '') {
                    arr['matchstatus'] = trMatchStatus;
                }
            }
            return arr;
        },
        extendTable: function (options) {
            options = $.extend({
                firstNumber: 0,
                save: false,
                edit: false,
                cancel: false,
                del: false,
                remove: false,
                sortColumn: null,
                sortComplete: null
            }, options || {});
            var thlist = $(this).find("thead tr th"), table = $(this),
                coll_columns = [];//{ name: '绿化率', field: 'GreenRate', ctype: 'number' }

            /*===============是否自动添加尾部操作=======================*/
            var tr = $('tbody tr', table),
                th = $('thead tr', table),
                lastTh = $('th', th).last();
            /*设置自定义列 此处之后要改成数据库读取*/
            $.each(coll_columns, function (_ci) {
                var _column = coll_columns[_ci];
                if (table.find('th[customname="' + _column.field + '"]').length == 0) {
                    th.append('<th customname="{0}">{1}</th>'.format(_column.field, _column.name));
                }
                tr.append('<td data-val="" data-type="{0}" name="{1}" data-ctype="{2}"></td>'
                    .format('custom', _column.field, _column.ctype));
                if (_ci == coll_columns.length - 1) {
                    SetCustomColumnsVal();
                }
            });
            if (table.attr('data-options') == 'true') {
                var newTd = document.createElement("td"),
                    newTh = document.createElement("th");
                /*=================头部信息=================*/
                if ($.trim(lastTh.text()) != '操作') {
                    newTh.innerHTML = "操作";
                    th.append(newTh);
                }
                /*=================添加到指定项中===========*/
                var html = '';
                if (options.edit)
                    html = html + '<a class="btn edit" href="javascript:;">编辑</a>';
                if (options.del)
                    html = html + '<a class="btn delete" href="javascript:;">删除</a>';
                newTd.innerHTML = html;
                tr.append(newTd);
            }

            /*=============== 头部固定列及滚动的操作 ===================*/
            if (thlist.length > 0) {
                thlist.each(function (i) {
                    /*清除已固定的列*/
                    $(this).removeClass('tdabsolute');
                    var espan = $(this).find('span');
                    if (espan.length > 0 && espan.is(":hidden")) {
                        espan.show();
                    }
                    if (i == 0) {/*全选*/
                        var ch = $(this).find('input[type=checkbox]'),
                            tdlist = table.find("tr").find('td:first');
                        if (ch.length > 0) {
                            ch.live('click', function () {
                                tdlist.each(function (j) {
                                    var tdch = $(this).find('input[type=checkbox]');
                                    $.uniform.update(tdch.attr('checked', ch[0].checked));
                                });
                            });
                        }
                    }
                    var img = $(this).find('i'), th = $(this);
                    if (i >= 3 && i <= 6) {/*只允许指定列有<img src="/Content/img/c_th_gd.jpg"/>*/
                        if (img.length == 0) {
                            var agd = $('<span style="display:inline-block;cursor:pointer;"><i class="icon-unlock"></i></span>'),
                                agd_img = agd.find('i');
                            th_img(th, agd_img, thlist, i);
                            $(this).append(agd);
                        } else {
                            th_img(th, img, thlist, i);
                        }
                    }
                    if (rowSort($(this).text())) {
                        var asc = $('.icon-sort-by-attributes', this),
                            desc = $('.icon-sort-by-attributes-alt', this),
                            span = $('<span style="display:inline-block;"></span>'),
                            orderParent = null, orderName = 'tbody>tr:eq(0)>td:nth-child({0})'.format(i + 1);
                        if (asc.length == 0 && desc.length == 0) {
                            asc = $('<i class="icon-sort-by-attributes" style="cursor:pointer;" title="顺序" data-sort="Asc"></i>');
                            desc = $('<i class="icon-sort-by-attributes-alt" style="cursor:pointer;" title="倒序" data-sort="Desc"></i>');
                            span.append(asc);
                            span.append("&nbsp;");
                            span.append(desc);
                            $(this).append(span);
                        }
                        asc.click(function () {
                            orderParent = $(this).parent().parent().parent().parent().parent().parent().parent();
                            orderName = $(orderName, orderParent).attr('name');

                            if ($.isFunction(options.sortComplete)) {
                                options.sortComplete(orderName, "Asc");
                            }
                        });
                        desc.click(function () {
                            orderParent = $(this).parent().parent().parent().parent().parent().parent().parent();
                            orderName = $(orderName, orderParent).attr('name');
                            if ($.isFunction(options.sortComplete)) {
                                options.sortComplete(orderName, "Desc");
                            }
                        });
                    }
                });
            }
            var scrollable = $(this).parent(),
                tdwidth = 0, tdheight = 0;
            scrollable.scroll(function (e) {
                var sleft = $(this).scrollLeft(), stop = $(this).scrollTop();

                /*某些列需要固定*/
                thlist.each(function (i) {
                    var _d = $(this).data('gd'), td, th, thspan;
                    if (_d != undefined && _d.v0) {
                        td = table.find("tr td:nth-child(" + _d.v1 + ")");
                        th = table.find("tr th:nth-child(" + _d.v1 + ")").first();
                        thspan = th.find('span');

                        var tdoffleft = th.offset().left,
                            tds = table.find("tr td"),
                            ths = table.find("tr th");
                        /*把相关样式删除*/
                        tds.each(function () {
                            $(this).removeClass('tdabsolute');
                        });
                        ths.each(function (i) {
                            if (i >= 1 && i <= 6) {
                                $(this).removeClass('tdabsolute');
                                var espan = $(this).find('span');
                                if (espan.length > 0 && espan.is(":hidden")) {
                                    espan.show();
                                }
                            }
                        });
                        /*把符合的条件的添加样式*/
                        if (_d.width == undefined || _d.height == undefined) {
                            $(this).data('gd', {
                                v0: _d.v0,
                                v1: _d.v1,
                                width: 300,
                                height: th.height()
                            });
                            tdwidth = _d.width;
                            tdheight = _d.height;
                        } else {
                            tdwidth = _d.width;
                            tdheight = _d.height;
                        }
                        if (sleft > (tdoffleft - 5)) {
                            td.each(function () {
                                $(this).addClass('tdabsolute');
                                $(this).css({ left: sleft, width: tdwidth, height: tdheight + 32 });
                            });
                            th.addClass('tdabsolute');
                            th.css({ left: sleft, width: tdwidth, height: tdheight + 16 });
                            thspan.hide();
                        }
                    }
                });
            });
            /*============= 行及列操作、事件 =====================*/
            $.each(table.find("tbody>tr"), function () {
                /*行事件*/
                $(this).live('dblclick', function (e) {
                    e.preventDefault();
                    var element = $(this).find('td:last>a:first');
                    if (element.html() == '编辑') {
                        element.click();
                    }
                });
                /*列事件*/
                var tdlast = $(this).find('td:last');
                tdlast.find('a.edit').live('click', function (e) {
                    e.preventDefault();
                    var nRow = $(this).parents('tr')[0];

                    if (this.innerHTML == '保存') {
                        if (options.save) {
                            $.extendTableSave({
                                nRow: nRow,
                                firstNumber: options.firstNumber,
                                edit: options.edit,
                                cancel: options.cancel,
                                remove: options.remove,
                                complete: function (data, crow) {
                                    //$(crow).remove();删除自己
                                }
                            });
                        }
                    } else if (this.innerHTML == '编辑') {
                        editRow(nRow);
                    } else {
                        $.extendTableSaveRow({
                            nRow: nRow,
                            edit: options.edit,
                            cancel: options.cancel,
                            firstNumber: options.firstNumber
                        });
                        //saveRow(nRow);
                    }
                });
                /*tdlast.find('a.delete').live('click', function (e) {
                    e.preventDefault();
                    var nRow = $(this).parents('tr')[0];
                });*/
                tdlast.find('a.cancel').live('click', function (e) {//信息复原
                    e.preventDefault();
                    var nRow = $(this).parents('tr')[0];
                    cancelRow(nRow);
                });
            });

            //编辑行中的列
            function editRow(nRow) {
                var jqTds = $('>td', nRow), rowData = [];
                $.each(jqTds, function (i, v) {
                    if (i > 6 && i != jqTds.length - 1) {
                        var dataType = $(this).attr('data-type'),
                            dataUrl = $(this).attr('data-url'),
                            dataId = $(this).attr('id'),
                            dataVal = $(this).attr('data-val'),
                            dataColumn = $(this).attr('data-column'),
                            dataCType = $(this).attr('data-ctype'),
                            dataName = $(this).attr('name'),
                            addButton = $('<a class="green" href="#"><i class="icon-plus"></i></a>'),//
                            tables = $('<table></table>'),
                            trstr = $('<tr></tr>'),
                            tbstd = $('<td style="border:0px;"></td>'),
                            tbstdbut = $('<td style="border:0px;vertical-align: middle;"></td>'),
                            pTdText = $.trim(jqTds[i].innerHTML);

                        tables.css({ width: 145, bordercollapse: "collapse", border: "0px" });
                        addButton.click(function () {
                            $.BingdingChange = 0;
                            $.ClearCookie();
                            var newAddButton = $('<div></div>');
                            $.customDialogId = newAddButton;
                            $.customDialogId.extendDialog({
                                title: '新增楼盘、楼栋、房号',
                                minWidth: 780,
                                open: function () {
                                    var templete = $("#templeteAdd"),
                                        tempdialog = $(this),
                                        pcaData = {
                                            provinceid: $(jqTds[7]).attr('data-val'),
                                            cityid: $(jqTds[8]).attr('data-val'),
                                            areaid: $(jqTds[9]).attr('data-val'),
                                            projectname: $('input', jqTds[12]).val(),
                                            address: "{0}{1}".format($('input', jqTds[10]).val(), $('input', jqTds[11]).val()),
                                            buildingname: $('input', jqTds[14]).val(),
                                            floorno: $('input', jqTds[15]).val(),
                                            housename: $('input', jqTds[16]).val()
                                        };

                                    $(this).html(templete.html());
                                    $('input[name=projectid]:hidden', tempdialog).val($(jqTds[12]).attr('data-val'))
                                    $('input[name=buildingid]:hidden', tempdialog).val($(jqTds[14]).attr('data-val'))
                                    $('.nav li', tempdialog).each(function () {
                                        $(this).click(function () {
                                            var liId = $.trim($(this).attr('id'));
                                            $('.nav li', tempdialog).each(function () {
                                                $(this).removeClass('active');
                                                if ($.trim($(this).attr('id')) == liId) {
                                                    $(this).addClass('active');
                                                }
                                            });

                                            $('.tab-content .tab-pane', tempdialog).each(function () {
                                                $(this).removeClass('active').removeClass('in');
                                                if ($.trim($(this).attr('id')) == liId) {
                                                    $(this).addClass('active').addClass('in');
                                                }
                                            });
                                        });
                                    });

                                    $.ActionSelectList = [{//省
                                        name: 'provinceid',
                                        url: '/Collateral/Province',
                                        val: 'ProvinceName',
                                        key: 'ProvinceId',
                                        children: {//市
                                            name: 'cityid',
                                            url: '/Collateral/City/?id={0}',
                                            val: 'CityName',
                                            key: 'CityId',
                                            children: {//县区
                                                name: 'areaid',
                                                url: '/Collateral/Area/?id={0}',
                                                val: 'AreaName',
                                                key: 'AreaId'
                                            }
                                        }
                                    }, {//用途
                                        name: 'purposecode',
                                        url: '/Collateral/ProjectPurposeCode',
                                        val: 'CodeName',
                                        key: 'Code',
                                    }];
                                    $.BindingSelect(function () {
                                        /*设置弹出层省市县区值*/
                                        $.ActionSetForm(pcaData);
                                        var tempform = tempdialog.find('form:not(#province_city_area)');
                                        var pcaform = $("#province_city_area", tempdialog);
                                        /*省市县区验证*/
                                        pcaform.validate({
                                            errorClass: 'help-block',
                                            rules: {
                                                provinceid: {
                                                    required: true
                                                },
                                                cityid: {
                                                    required: true
                                                },
                                                areaid: {
                                                    required: true
                                                }
                                            },
                                            highlight: function (element) {
                                                //alert(element.name);
                                                $(element).closest('.form-group').addClass('has-error');
                                            },
                                            unhighlight: function (element) {
                                                $(element).closest('.form-group').removeClass('has-error');
                                            }
                                        });
                                        /*其他表单验证*/
                                        tempform.each(function (i) {
                                            $.formValidate({
                                                csStyle: false,
                                                form: $(this),
                                                torules: $.collateralloupanrules,
                                                colse: false,
                                                isRalod:false,
                                                complete: function (formData) {
                                                    if (formData.data != null) {
                                                        var finput;
                                                        if (i == 0) {/*当添加楼盘*/
                                                            finput = $('input[name=projectid]', tempform[i + 1]);
                                                            finput.val(formData.data.ProjectId);
                                                        }
                                                        if (i == 1) {/*当添加楼栋*/
                                                            finput = $('input[name=buildingid]', tempform[i + 1]);
                                                            finput.val(formData.data.BuildingId);
                                                        }
                                                    }
                                                }
                                            });
                                            $(this).submit(function () {
                                                var selects = $('select option:selected', pcaform),
                                                    current = $(this), scount = 0;
                                                selects.each(function () {
                                                    var input = $('input[name={0}]'.format($(this).parent().attr('name')), current);
                                                    scount = 0;
                                                    val = $(this).val();
                                                    if (val == '') {
                                                        scount = 1;
                                                    } else {
                                                        input.val(val);
                                                    }
                                                });
                                                if (scount == 1) {
                                                    pcaform.submit();
                                                    return false;
                                                }
                                            });
                                        });
                                    });
                                }
                            });
                        });
                        if (dataType != undefined && dataType != 'custom') {
                            if (dataType == 'select') {/*table中的td列中是否需要添加select标签*/
                                var id = parseInt(dataVal);
                                if (dataId == 'Province')
                                    ElementSelect(dataUrl, dataId, id, i, jqTds[i], jqTds, 'ProvinceId', 'ProvinceName', rowData);
                            }
                            else if (dataType == 'autocomplete') {/*table中的td列中是否需要添加搜索标签*/
                                var textautocomplete = $('<input type="text" class="form-control input-small" value="' + pTdText + '" autocomplete="off">');
                                var autoFormatItem = null,
                                    autoResult = null,
                                    autoColumn = null,
                                    autoWidth = '350',
                                    autoMatchName = null,
                                    autoTableFormat = '<table><tr><td style="width:53px;border-right: 1px solid #dddddd;color:red;">{0}</td><td>{1}</td></tr></table>';
                                /*楼盘*/
                                if (dataName == "ProjectName") {
                                    autoColumn = eval('(' + dataColumn + ')');
                                    autoFormatItem = function (row) {
                                        var dataChildren = $.parseJSON(row.Children);
                                        autoTableFormat = '<table><tr><td style="width:53px;border-right: 1px solid #dddddd;color:red;">{0}</td><td style="width:150px;border-right: 1px solid #dddddd;">{1}</td><td>{2}</td></tr></table>';
                                        autoMatchName = row.MatchStatus == 0 ? '正式库' : '临时库'
                                        return autoTableFormat.format(autoMatchName, row.Project.ProjectName, chSplit(dataChildren));
                                    };
                                    autoResult = function (event, data, formatted) {
                                        var tenTdInput = $('input', jqTds[10]),
                                            nineTdVal = $(jqTds[9]).attr('data-val');
                                        if ($.trim(tenTdInput.val()) == '') {
                                            tenTdInput.val(data.Project.Address);
                                        }
                                        if (nineTdVal != undefined && ($.trim(nineTdVal) == '' || parseInt($.trim(nineTdVal)) <= 0)) {
                                            $(jqTds[9]).attr('data-val', data.Project.AreaID);
                                            $("option[value='" + data.Project.AreaID + "']", $('select', jqTds[9])).attr("selected", true);
                                        }

                                        $(this).val(data.Project.ProjectName);
                                        var pTd = $(this).parent().parent().parent();

                                        var _adrs = $.trim($.getTdVal(jqTds[10]) + $.getTdVal(jqTds[11]));;
                                        ChangeEqualColumnsValue(pTd.attr('name'),
                                            data.Project.ProjectId,
                                            data.Project.ProjectName, _adrs);


                                        /*得到自己的父级的td,并设置值*/
                                        pTd.attr('data-val', data.Project.ProjectId);
                                        pTd.css({ backgroundColor: '' });
                                        rowMatchStatus(nRow, data.MatchStatus, jqTds);

                                        SetCustomColumnsVal();
                                    };
                                }/*楼栋*/
                                else if (dataName == "BuildingNumber") {
                                    autoColumn = eval('(' + dataColumn + ')');
                                    autoWidth = 200;
                                    autoFormatItem = function (row) {
                                        autoMatchName = row.MatchStatus == 0 ? '正式库' : '临时库'
                                        return autoTableFormat.format(autoMatchName, row.Building.BuildingName);
                                    };
                                    autoResult = function (event, data, formatted) {
                                        $(this).val(data.Building.BuildingName);
                                        var pTd = $(this).parent().parent().parent();
                                        /*得到自己的父级的td,并设置值*/
                                        pTd.attr('data-val', data.Building.BuildingId);
                                        pTd.css({ backgroundColor: '' });
                                        rowMatchStatus(nRow, data.MatchStatus, jqTds);
                                    };
                                } /*房号*/
                                else if (dataName == "RoomNumber") {
                                    autoColumn = eval('(' + dataColumn + ')');
                                    autoWidth = 200;
                                    autoFormatItem = function (row) {
                                        autoMatchName = row.MatchStatus == 0 ? '正式库' : '临时库'
                                        return autoTableFormat.format(autoMatchName, row.House.HouseName);
                                    };
                                    autoResult = function (event, data, formatted) {
                                        $(this).val(data.House.HouseName);
                                        var pTd = $(this).parent().parent().parent();
                                        /*得到自己的父级的td,并设置值*/
                                        pTd.attr('data-val', data.House.HouseId);
                                        pTd.css({ backgroundColor: '' });
                                        rowMatchStatus(nRow, data.MatchStatus, jqTds);
                                    };
                                }
                                textautocomplete.extendAutomplete({
                                    url: dataUrl,
                                    column: autoColumn,
                                    width: autoWidth,
                                    formatItem: autoFormatItem,
                                    result: autoResult
                                });

                                tbstd.append(textautocomplete)
                                tbstdbut.append(addButton)
                                tables.append(tbstd);
                                tables.append(tbstdbut);
                                $(jqTds[i]).html(tables);
                                rowData.push({ index: i, tval: dataVal, text: pTdText });
                            }
                        }
                        else {/*table中的td列中是否需要添加text文本标签*/
                            var newinput = $('<input type="text" class="form-control input-small" value="' + $.trim(jqTds[i].innerHTML) + '"/>');
                            if (dataCType == 'number') {/*数字类型*/
                                newinput.keyup(function () {
                                    if (this.value.match(new RegExp(/^\d+(\.\d+)?$/)) == null) { this.value = ""; }
                                });
                            }
                            tbstd.append(newinput);
                            tables.append(tbstd);
                            if (i != 10 && i != 11) {
                                tbstdbut.append(addButton);
                                tables.append(tbstdbut);
                            }
                            $(jqTds[i]).html(tables);
                            rowData.push({ index: i, tval: dataVal, text: pTdText });
                        }

                    } else if (jqTds.length - 1 == i) {
                        var html = '';
                        if (options.save)
                            html = html + '<a class="btn edit" href="javascript:;">保存</a>';
                        if (options.cancel)
                            html = html + '<a class="btn cancel" href="javascript:;">取消</a>';
                        jqTds[i].innerHTML = html;
                        $(jqTds[i]).height($(jqTds[i]).height());
                        $(nRow).data('data', rowData);
                    }
                });

            }
            //取消行中的列编辑
            function cancelRow(nRow) {
                var input = null, select = null, jqTds = $('>td', nRow), rowData = $(nRow).data('data');

                $.each(jqTds, function (i, v) {
                    if (i > 0) {
                        input = $('input', this), select = $('select option:selected', this);
                        var data = cancelData(rowData, i);
                        if (input.length > 0) {
                            if (data != null) {
                                jqTds[i].innerHTML = data.text;
                                if (data.tval != undefined)
                                    $(jqTds[i]).attr('data-val', data.tval);
                            } else {
                                jqTds[i].innerHTML = input.val();
                            }
                        }
                        if (select.length > 0) {
                            if (data != null) {
                                jqTds[i].innerHTML = data.text;
                                if (data.tval != undefined)
                                    $(jqTds[i]).attr('data-val', data.tval);
                            } else {
                                var value = select.val();
                                var text = select.text(); // 选中文本
                                jqTds[i].innerHTML = value != null && value != '' ? text : '';
                            }
                        }
                        if (i == jqTds.length - 1) {
                            var html = '';
                            if (options.edit)
                                html = html + '<a class="btn edit" href="javascript:;">编辑</a>';
                            if (options.del)
                                html = html + '<a class="btn delete" href="javascript:;">删除</a>';
                            jqTds[i].innerHTML = html;
                        }
                        $(jqTds[i]).css({ height: 36 });/*此处可以修改到自动修改高度*/
                    }
                });
            }
            //行中列特殊处理
            function ElementSelect(dataUrl, dataId, id, i, jqTd, jqTds, k1, k2, rowData) {
                $.extendAjax({
                    url: dataUrl,
                    type: 'post'
                }, function (sData) {
                    var select = $('<select id="' + dataId + '" dvalue="' + id + '" index="' + i + '" style=""></select>'),
                        current = $(jqTd),
                        rdata = sData.data;

                    rowData.push({ index: i, tval: current.attr('data-val'), text: $.trim(current.html()) });

                    var newoption = '<option value="{0}">{1}</option>';
                    select.append(newoption.format("", "-请选择-"));
                    for (var item in rdata) {
                        select.append(newoption
                            .format(rdata[item][k1], rdata[item][k2]));
                    }
                    select.change(function () {
                        var sval = $(this).val();
                        if (sval != '') {
                            $(this).attr('dvalue', sval);
                            //设置最上一级的值
                            $(this).parents("table").parent().attr('data-val', sval);
                            id = sval;
                        }
                        var index = parseInt($(this).attr('index')),
                            td = $(jqTds[index + 1]),
                            tdk1 = '', tdk2 = '',
                            url = td.attr('data-url');
                        if (index == 7) {
                            tdk1 = 'CityId';
                            tdk2 = 'CityName';
                        }
                        if (index == 8) {
                            tdk1 = 'AreaId';
                            tdk2 = 'AreaName';
                        }
                        if (url != undefined) {
                            url = url.format(id);
                            ElementSelect(url, td.attr('id'), td.attr('data-val'), index + 1, jqTds[index + 1], jqTds, tdk1, tdk2, rowData);
                        }
                    });
                    select.find("option[value='" + id + "']").attr("selected", true);
                    select.change();
                    var tables = $('<table></table>');
                    tables.append($('<td style="border:0px;"></td>').html(select));
                    tables.css({ bordercollapse: "collapse", border: "0px" });
                    current.html(tables);
                });
            }
            /*处理列模糊搜索出来的子集别名(网络名)*/
            function chSplit(data) {
                var rvalue = '';
                if (data != null && data != '' && data.length > 0) {
                    $.each(data, function () {
                        rvalue += this.NetName + ',';
                    });
                }
                return rvalue.substring(0, rvalue.length - 1);
            }
            /*头部固定列图片事件*/
            function th_img(th, img, list, i) {
                $(th).find('i').removeClass('icon-lock').addClass('icon-unlock');
                th.data('gd', { v0: false, v1: (i + 1) });
                img.attr('title', '未锁定');
                img.click(function () {
                    //img.removeClass('icon-unlock').addClass('icon-lock');

                    if ($(this).is('.icon-unlock')) {
                        $(this).removeClass('icon-unlock').addClass('icon-lock')
                        img.attr('title', '已锁定');
                    } else {
                        $(this).removeClass('icon-lock').addClass('icon-unlock')
                        img.attr('title', '未锁定');
                    }
                    var data = th.data('gd');
                    data.v0 = data.v0 == false ? true : false;
                    th.data('gd', { v0: data.v0, v1: data.v1 });
                    /*只能有一个固定*/
                    list.each(function (_i) {
                        var c_d = $(this).data('gd');
                        if (c_d != undefined && data.v1 != c_d.v1) {
                            $(this).find('i').removeClass('icon-lock').addClass('icon-unlock');
                            $(this).data('gd', { v0: false, v1: c_d.v1 });
                        }
                    });
                });
            }
            /*得到取消中某列的值*/
            function cancelData(rowData, i) {
                var _data = null;
                for (var data in rowData) {
                    if (rowData[data].index == i) {
                        _data = rowData[data];
                        break;
                    }
                }
                return _data;
            }
            /*匹配状态情况*/
            function rowMatchStatus(nRow, val, jqTds) {
                var td1 = $(jqTds[12]).attr('data-val'),
                    td2 = $(jqTds[14]).attr('data-val'),
                    td3 = $(jqTds[16]).attr('data-val');
                if ((td1 != undefined && td1 != '' && parseInt(td1) > 0) &&
                    (td2 != undefined && td2 != '' && parseInt(td2) > 0) &&
                    (td3 != undefined && td3 != '' && parseInt(td3) > 0)) {
                    $(nRow).attr('data-matchstatus', val);
                }
            }
            /*排序*/
            function rowSort(text) {
                var column = options.sortColumn;
                if (column != null)
                    for (var val in column) {
                        if ($.trim(column[val]) == $.trim(text)) {
                            return true;
                        }
                    }
                return false;
            }
            /*设置自定义列值*/
            function SetCustomColumnsVal() {
                $("tbody tr td[data-type='custom']").each(function (_cusi) {
                    var _tdpval = $("td[name='ProjectName']", $(this).parent()).attr('data-val'),
                        _tdcval = $("td[name='CityId']", $(this).parent()).attr('data-val'),
                        _tdcusval = $(this);
                    if (_tdpval != null && _tdpval != '' && _tdcval != null && _tdcval != '') {
                        $.extendAjax({
                            url: '/Collateral/GetCutsomColumnVal',
                            type: 'post',
                            data: { cid: _tdcval, pid: _tdpval, cname: $(this).attr('name') }
                        }, function (rdata) {
                            _tdcusval.html(rdata.data);
                        });

                    }
                });
            }
            /*查找已匹配同列的信息并改变颜色和赋值*/
            function ChangeEqualColumnsValue(name, valId, val, address) {
                address = $.trim(address);
                $('tr', table).each(function (_ri) {
                    var tds = $('>td', this);
                    tds.each(function (_ci) {
                        var _adrs = $.trim($.getTdVal(tds[10]) + $.getTdVal(tds[11]));

                        if ($.trim($(this).attr('name')) == name && _adrs == address) {
                            $(this).attr('data-val', valId);
                            //是否处于编辑状态
                            if ($('input', this).length > 0) {
                                $('input', this).val(val);
                            } else {
                                $(this).html(val);
                            }
                            $(this).css({ backgroundColor: '' });
                        }
                    });
                });
            }

            App.init();
        },
        extendAutomplete: function (options) {
            $(this).unautocomplete();
            options = $.extend({
                url: '/Upload/Search',
                width: '350',
                max: 200,
                column: null,
                formatItem: null,
                result: null
            }, options || {});
            $(this).autocomplete(options.url, {
                width: options.width,
                autoFill: false,
                max: options.max,
                matchContains: true,
                column: options.column,
                cacheLength: 1,
                parse: function (data) {
                    data = $.parseJSON(data);
                    if (typeof (data) == 'object') {
                        if (data.type == 0) {
                            return [];
                        }
                    }
                    var _sortData = data.data == undefined ? data : data.data;
                    return $.map(_sortData, function (row) {
                        if (row.Project != null && typeof (row.Project) == "string") {
                            row.Project = $.parseJSON(row.Project);
                        }
                        else if (row.Building != null && typeof (row.Building) == "string") {
                            row.Building = $.parseJSON(row.Building);
                        }
                        else if (row.House != null && typeof (row.House) == "string") {
                            row.House = $.parseJSON(row.House);
                        } else if (typeof (row.data) == "string") {
                            row.data = $.parseJSON(row.data);
                        }
                        return {
                            data: row,
                            value: row,
                            result: row
                        }
                    });
                },
                formatItem: options.formatItem
            }).result(options.result);
        },
        extendMap: function (options) {/*地图*/
            options = $.extend({
                name: null,
                pname: null,
                zoom: null,
                boundary: true,
                complete: null
            }, options || {});
            var mp = new BMap.Map($(this).attr('id'));
            if (options.pname != null && options.zoom != null) {
                new BMap.Geocoder().getPoint(options.name, function (point) {
                    if (point) {
                        mp.centerAndZoom(point, options.zoom);
                    }
                }, options.pname);
            }
            mp.enableScrollWheelZoom();
            if (options.pname != null && options.boundary) {
                var bdary = new BMap.Boundary();
                bdary.get(options.pname, function (rs) {//获取区域
                    mp.clearOverlays();        //清除地图覆盖物       
                    var count = rs.boundaries.length; //行政区域的点有多少个
                    for (var i = 0; i < count; i++) {
                        var ply = new BMap.Polygon(rs.boundaries[i], { strokeWeight: 2, strokeColor: "#ff0000" }); //建立多边形覆盖物
                        mp.addOverlay(ply);  //添加覆盖物
                        mp.setViewport(ply.getPath());//调整视野
                        if ($.isFunction(options.complete) && i == count - 1) {
                            options.complete.call(this);
                        }
                    }
                });
            } else {
                if ($.isFunction(options.complete)) {
                    setTimeout(function () {
                        options.complete.call(this);
                    }, 1500);
                }
            }
            return mp;
        }
    });
    $.extend({
        customerrules: {/*客户验证*/
            customername: {
                required: true,
                remote: {
                    url: "/Users/CheckCustomerName", type: "post", dataType: "json", data: {
                        customerId: function () {
                            return ($("input[name=customerid]").val() == "" ? 0 : $("input[name=customerid]").val());
                        }, customerName: function () {
                            return $("input[name=customername]").val();
                        }, fxtCompanyId: function () {
                            return ($("select[name=fxtcompanyid]").val() == "" ? 0 : $("select[name=fxtcompanyid]").val());
                        }
                    }
                }
            },
            customertype: {
                required: true
            },
            fxtcompanyid: {
                required: true
            }
        },
        userrules: {/*用户验证*/
            customerid: {
                required: true
            },
            username: {
                required: true,
                remote: {
                    url: "/Users/CheckUserName", type: "post", dataType: "json", data: {
                        id: function () {
                            return ($("input[name=id]").val() == "" ? 0 : $("input[name=id]").val());
                        }, userName: function () {
                            return $("input[name=username]").val();
                        }
                    }
                }
            },
            truename: {
                required: true,
            },
            emailstr: {
                email: true
            },
            userpwd: {
                required: true
            },
            sureuserpwd: {
                required: true,
                equalTo: "input[name=userpwd]",
            }
        },
        modifypassrules: {/*用户修改密码验证*/
            olduserpwd: {
                required: true
            },
            userpwd: {
                required: true
            },
            sureuserpwd: {
                required: true,
                equalTo: "input[name=userpwd]",
            }
        },
        productrules: {/*产品验证*/
            productname: {
                required: true
            }
        },
        menurules: {/*菜单验证*/
            productid: {
                required: true
            },
            menuname: {
                required: true
            },
            url: {
                required: true,
                url: true
            }
        },
        purviewrules: {/*权限验证*/
            purviewname: {
                required: true
            }
        },
        collateralrules: {/*权限验证*/
            Number: {
                required: true
            }, Branch: {
                required: true
            },
            PurposeCode: {
                required: true
            },
            Name: {
                required: true
            },
            BuildingArea: {
                required: true
            },
            Address: {
                required: true
            }
        },
        collateralloupanrules: {/*新增楼盘、楼栋、房号验证*/
            provinceid: {
                required: true
            },
            cityid: {
                required: true
            },
            areaid: {
                required: true
            },
            projectname: {/*楼盘名称*/
                required: true
            },
            purposecode: {/*楼盘用途*/
                required: true
            },
            buildingname: {/*楼栋*/
                required: true
            },
            totalfloor: {/*楼栋总的楼层总数*/
                required: true
            },
            floorno: {/*楼层*/
                required: true
            },
            housename: {/*房号*/
                required: true
            }
        },
        projectrules: {/*新增项目*/
            bankid: {
                required: true
            },
            projectname: {
                required: true
            }
        }
    });
})(jQuery);
var userAgent = navigator.userAgent.toLowerCase();
// Figure out what browser is being used
jQuery.browser = {
    version: (userAgent.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [])[1],
    safari: /webkit/.test(userAgent),
    opera: /opera/.test(userAgent),
    msie: /msie/.test(userAgent) && !/opera/.test(userAgent),
    mozilla: /mozilla/.test(userAgent) && !/(compatible|webkit)/.test(userAgent),
    chrome: userAgent.match(/chrome/) != null
};
Date.prototype.dateformat = function (format) {
    /* 
    * eg:format="yyyy-MM-dd hh:mm:ss"; 
    */
    var o = {
        "M+": this.getMonth() + 1, // month  
        "d+": this.getDate(), // day  
        "h+": this.getHours(), // hour  
        "m+": this.getMinutes(), // minute  
        "s+": this.getSeconds(), // second  
        "q+": Math.floor((this.getMonth() + 3) / 3), // quarter  
        "S": this.getMilliseconds()
        // millisecond  
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4
                        - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
                            ? o[k]
                            : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}
String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    //var reg = new RegExp("({[" + i + "]})", "g");//这个在索引大于9时会有问题，谢谢何以笙箫的指出
                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}