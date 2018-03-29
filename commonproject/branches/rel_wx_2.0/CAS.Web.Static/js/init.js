//全局JS对象 kevin
var CAS = {
    Domain: "", //域名
    RootUrl: "", //根目录
    RootUrlFull: "", //带域名的根目录
    StaticUrl: "", //静态文件的地址
    StaticVersion: "", //静态文件的版本
    Style: "", //默认样式，用于动态加载文件
    TemplatePath: "", //模板目录
    TemplateStylePath: "", //样式目录
    //都要用到的变量,在masterpagebase中赋值
    Define: {},
    StaticFileLoads: [], //动态加载的js序列
    Debug: false, //是否启用js跟踪调试，由后台debug指令控制，发布不用更改。    
    CookieParams: function () { //cookie domain设置,因localhost不能设置
        if (CAS.RootUrlFull.indexOf("localhost") >= 0) return {};
        return { domain: CAS.Domain };
    },
    KeyCode: {//各常用键值
        ENTER: 13, ESC: 27, END: 35, HOME: 36,
        SHIFT: 16, TAB: 9, CTRL: 17, ALT: 18,
        LEFT: 37, RIGHT: 39, UP: 38, DOWN: 40,
        DELETE: 46, BACKSPACE: 8, F5: 116
    },
    isIE: function () { return $.browser.msie },
    isIE6: function () { return $.browser.msie && $.browser.version == 6; },
    //文件是否为图片
    isImage: function (file) { return /.jpg$/i.test(file) || /.jpeg$/i.test(file) || /.png$/i.test(file) || /.bmp$/i.test(file) || /.gif$/i.test(file) },
    //取缩略图
    Thum: function (file) { return file.replace(".", "_t."); },
    //获取url参数
    GetQuery: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return decodeURI(r[2]); return null;
    },
    stopDefault: function (e) {
        //如果提供了事件对象，则这是一个非IE浏览器
        if (e && e.preventDefault) {
            //阻止默认浏览器动作(W3C)
            e.preventDefault();
        } else {
            //IE中阻止函数器默认动作的方式
            e.returnValue = false;
        }
        return false;
    },
    stopPropagation: function (e) {
        //如果提供了事件对象，则这是一个非IE浏览器
        if (e && e.stopPropagation) {
            //阻止默认浏览器动作(W3C)
            e.stopPropagation();
        } else {
            //IE中阻止函数器默认动作的方式
            e.cancelBubble = false;
        }
        return false;
    },
    //随机变量名
    RndVar: function () { return "CAS_" + Math.random().toString().replace(".", ""); },
    JsonToDate: function (t, format) { //格式日期 如：jsonToDate(date,"yyyy-MM-dd"):2012-08-14
        if (t == "") return t;
        try {
            var obj = {};
            if ("object" == typeof (t)) {
                obj = t;
            }
            else {
                obj = eval('new ' + (t.replace(/\//g, '')));
            }
            return obj.format(format);
        } catch (e) { return t; }
    },
    //千分位
    Commafy: function (n) {
        if (n == null) return "";
        if (isNaN(n)) return n;
        n = n.toString();
        var re = /\d{1,3}(?=(\d{3})+$)/g;
        var n1 = n.replace(/^(\d+)((\.\d+)?)$/, function (s, s1, s2) { return s1.replace(re, "$&,") + s2; });
        return n1;
    },
    //去除千分位
    CancelCommafy: function (n) {
        if (n == null) return "";
        n = n.toString();
        return n.replace(/,+/g, '');
    },
    //最多保留小数
    Scale: function (value, scale) {
        return Math.round(value * Math.pow(10, scale)) / Math.pow(10, scale);
    },
    Include: function (files) { //同步加载js或css，注意不能动态使用在如ajax异步代码中(因为有document.write)。
        for (var i = 0; i < files.length; i++) {
            var name = files[i].replace(/^\s|\s$/g, "");
            var att = name.split('.');
            var ext = att[att.length - 1].toLowerCase();
            if (ext.indexOf("?") > 0) {
                ext = ext.split("?")[0];
            }
            var isCSS = ext == "css";
            var tag = isCSS ? "link" : "script";
            var attr = isCSS ? " type='text/css' rel='stylesheet' " : " language='javascript' type='text/javascript' ";
            var link = (isCSS ? "href" : "src") + "='" + CAS.StaticUrl + ext + "/" + (isCSS ? CAS.Style + "/" : "") + name + (name.indexOf("?") > 0 ? "&" : "?") + CAS.StaticVersion + "'";
            if ($(tag + "[" + link + "]").length == 0) document.write("<" + tag + attr + link + "></" + tag + ">");
        }
    },
    Use: function (files, callback) {//代码内动态同步加载js 
        var load = 0, js = [];
        var _doc = document.getElementsByTagName('head')[0];
        for (var i = 0; i < files.length; i++) {
            var name = files[i].replace(/^\s|\s$/g, "");
            var att = name.split('.');
            var ext = att[att.length - 1].toLowerCase();
            if (ext.indexOf("?") > 0) {
                ext = ext.split("?")[0];
            }
            var isCSS = ext == "css";
            var link = CAS.StaticUrl + ext + "/" + (isCSS ? CAS.Style + "/" : "") + name + (name.indexOf("?") > 0 ? "&" : "?") + CAS.StaticVersion;
            var id = link.substring(link.lastIndexOf("/") + 1);
            id = id.substring(0, id.lastIndexOf(".") + 1) + ext;
            //没有加载过就加载
            if (!CAS.StaticFileLoads.contains(id)) {
                if (isCSS) {
                    var css = document.createElement("link");
                    css.id = id;
                    css.type = "text/css";
                    css.rel = "Stylesheet";
                    css.href = link;
                    _doc.appendChild(css);
                }
                else {
                    //js要保证加载完毕，单独处理
                    js.push({ id: id, link: link });
                    CAS.StaticFileLoads.push(id);
                }
            }
            else if (!isCSS) {
                //判断是否加载完成，完成才触发回调函数
                function status() {
                    if (document.getElementById(id)) {
                        if (document.getElementById(id).status == "loaded") {
                            clearTimeout(time);
                            callback();
                        }
                    }
                }
                var time = setInterval(status, 50);
                return;
            }
        }

        for (var i = 0; i < js.length; i++) {
            loadjs(js[i].id, js[i].link);
        }
        //加载JS，用onload或onreadystatechange实现
        function loadjs(id, link) {
            var script = document.createElement("script");
            script.type = 'text/javascript';
            script.src = link;
            script.id = id;
            var loaded = false;
            _doc.appendChild(script);
            script.onload = script.onreadystatechange = function () {
                if (!loaded && (!script.readyState || script.readyState == 'loaded' || script.readyState == 'complete')) {
                    load++;
                    script.status = "loaded";
                    loaded = true;
                    go();
                    script.onload = script.onreadystatechange = null;
                }
            }
        }
        function go() {
            //全部加载完成后触发回调函数
            if (load == js.length) {
                callback();
            }
        }
    },
    DialogAPI: function () {
        var api = frameElement ? (frameElement.api ? frameElement.api : (parent.frameElement ? parent.frameElement.api : null)) : null;
        return api;
    },
    Dialog: function (args) { //打开新窗口
        //使用C++窗口，判断接口的类型为unknown即为在C++窗口中。 kevin
        //OpenNewDlg(CString szURL,int nWeith,int nHigh,BOOL bIsResize,BOOL bIsMinBtn,BOOL bIsMaxBtn,BOOL bIsCloseBtn,CString szDlgTitle)
        //只有url方式才使用C++窗口 ，这里考虑是否由网页自己决定，取决于C++窗口打开网页是否要重新检查登录等，如果用COOKIE令牌是不是可以不用重新检查呢？
        if (!args.win) args.win = window;
        if (args.content.indexOf("url:") >= 0) {
            var url = args.content.substring(args.content.indexOf(":") + 1);
            //这里不能使用相对路径，需要完整的路径             
            if (url.indexOf("http:") < 0) url = CAS.RootUrlFull + (url.substring(0, 1) == "/" ? url.substring(1) : url);
            if (!args.height) args.height = 300;
        }
        args.parent = CAS.DialogAPI();
        return $.fn.casdialog(args);
    },
    DialogClose: function (args) { //关闭
        var confirm = CAS.GetQuery("confirm");
        var confirmtext = CAS.GetQuery("confirmtext");
        //是否需要确认        
        if (confirm)
            CAS.Confirm({ content: confirmtext, callback: returnVal });
        else returnVal();
        function returnVal() {
            var api = CAS.DialogAPI();
            if (args && args.callback && args.callback != "") {
                var W = api.config.win;
                eval("W." + args.callback + "(args.data)");
            }
            api.close(1);
        }
    },
    Alert: function (args, title) {
        if (typeof (args) == "string")
            args = { content: args };
        var api = CAS.DialogAPI();
        args = $.extend({}, args, { parent: api }, { title: title });
        return $.fn.casalert(args);
    },
    Confirm: function (args) {
        var api = CAS.DialogAPI();
        args.parent = api;
        return $.fn.casconfirm(args);
    },
    Prompt: function (args) {
        var api = CAS.DialogAPI();
        args.parent = api;
        return $.fn.casprompt(args);
    },
    Progress: function (args) {
        var api = CAS.DialogAPI();
        if (typeof (args) == "string")
            args = { content: args };
        args = $.extend({}, args, { parent: api });
        return $.fn.casprogress(args);
    },
    HtmlEncode: function (val) {
        return val.replace(/</ig, "&lt;").replace(/>/ig, "&gt;");
    },
    HtmlDecode: function (val) {
        return val.replace(/&lt;/ig, "<").replace(/&gt;/ig, ">");
    }
}

//扩展方法
Array.prototype.contains = function (element) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == element) {
            return true;
        }
    }
    return false;
}
//字符串转html
String.prototype.str2html = function () {
    return this.replace(/&lt;/ig, "<").replace(/&gt;/ig, ">");
}
//html转字符串
String.prototype.html2str = function () {
    return this.replace(/</ig, "&lt;").replace(/>/ig, "&gt;");
}
//html转字符串
String.prototype.trimEnd = function (args) {
    if (this.length == 0) return this;
    var i = this.lastIndexOf(args);
    var last = this.length - 1;
    if (i != -1 && i == last) {
        return this.substring(0, i * 1);
    } else {
        return this;
    }

}
$.fn.extend({
    val: function (v) { //改写val，主要是自定义select，多选的还要改
        var $this = $(this);
        if (arguments.length > 0) {
            if ($this.isTag("select")) {
                if ($("option[value='" + v + "']", $this).size() > 0) {
                    $("option[value='" + v + "']", $this).attr("selected", "selected");
                }
                else if ($("option:contains('" + v + "')", $this).size() > 0) {
                    $("option:contains('" + v + "')", $this).eq(0).attr("selected", "selected");
                } else {
                    if ($this.hasClass("input")) {//可输入的下拉框，添加输入文本值的下拉选项
                        $("<option value='" + v + "'>" + v + "</option>").prependTo($this);
                    }
                    $this[0].selectedIndex = 0;
                }
                $this.casselect();
            } else if ($this.hasClass("check_radio") || $this.hasClass("check_checkbox")) {
                if ($.fn.casChecked)
                    return $this.casChecked(v.toString());
                else CAS.Use(["cas.checkbox.js"], function () { $this.casChecked(v.toString()); });
            } else if ($this.size() > 0) {
                //$this[0].value = v; //这里chrome动态生成的控件会不能赋值，原因未知，改为attr，kevin
                $this.attr("value", v);
                if ($this.hasClass("iptrequired") && v != "" && !$this.hasClass("iptval")
                || ($this.hasClass("iptval") && v != $this.attr("rel"))) {
                    $this.removeClass("iptrequired").removeClass("iptval");
                    if ($this.hasClass("number")) $this.swapClass("tr");
                }
                if ($this[0].value == "" && $this.hasClass("must") && !$this.hasClass("disabled")) {
                    $this.swapClass("iptrequired");
                }
            }
            return $this;
        }
        else {
            if ($this.isTag("select")) {
                if ($this.hasClass("input")) { //可输入的下拉框，取输入的文本值
                    return $this.next().find("input.input")[0].value;
                }
                else {
                    var v = [];
                    $this.children().each(function (i) {
                        if ($(this).attr("selected")) {
                            v.push($(this)[0].value);
                        }
                    })
                    return v.join(",");
                }
            } else if ($this.hasClass("check_radio") || $this.hasClass("check_checkbox")) {
                return $this.casCheckValue();
            } else {
                var v = null;
                if (null != $this[0]) {
                    if ($this.hasClass("number")) v = CAS.CancelCommafy($this[0].value);
                    else v = $.trim($this[0].value);
                }
                return v;
            }
        }
    },
    //下拉框所有值
    selectval: function (args) {
        var ary = [];
        $("option", this).each(function () {
            var id = $(this)[0].value;
            if (id > 0 && !args) {
                ary.push(id);
            }
            else if (args) { ary.push(id); }
        })
        return ary;
    },
    //忽略表单提交项
    casignore: function (flag) {
        $("*", this).each(function () {
            if ($(this).attr("id") && $(this).attr("id").indexOf("field_") >= 0) {
                if (flag) //ignore
                    $(this).swapClass("ignore");
                else
                    $(this).removeClass("ignore");
            }
        });
    },
    //loading通用
    casloading: function (flag) {
        return $(this).each(function () {
            var $this = $(this);
            if (flag) {
                $this.data("html", $this.html());
                $this.html("&nbsp;").css({ background: "url(" + CAS.StaticUrl + "images/loading.gif) no-repeat 50% 50%" }).show();
            } else {
                $this.html($this.data("html")).css({ background: "none" });
            }
            return $this;
        });
    }
    ,
    selectOnly: function (css) { //只选中当前项
        var classname = css ? css : "selected";
        return $(this).addClass(classname).siblings().removeClass(classname);
    },
    bindSelect: function (nulltext, data, def, v, t, callback, rel) {//绑定下拉框
        return $(this).clearSelect().each(function () {
            if ($(this).isTag("select")) {
                if (null != data) {
                    var o = [];
                    if (nulltext) {
                        o.push('<option value="">' + nulltext + '</option>');
                    }
                    $.each(data, function (index, op) {
                        o.push('<option ' + (rel ? 'rel="' + op["rel"] + '"' : '') + ' value="' + op[v] + '"' + (def == op[v] ? " selected" : "") + '>' + op[t] + '</option>');
                    });
                    $(this).html(o.join("")).casselect();
                }
                if ('function' == typeof (callback)) {
                    callback();
                }
            }
        });
    },
    //修改by kevin 2013-1-31
    bindTreeSelect: function (nulltext, data, def, idname, valuename, textname, parentidname, isadditional, callback) {//绑定树下拉框
        //nulltext:初始项，data:数组, def:默认值, idname:唯一值, valuename:绑定的值, textname:显示的值, 
        //parentidname:父ID指向的idname，无则为null或0, isadditional:子项的隐藏TEXT是否追加父项的名称（REL属性）
        var nodes = [];      //下拉框目标数组
        var tmpdata = data.concat(); //用于递归的临时数组，从DATA拷贝来。
        var fullname = []; //用于追加父项的名称（REL属性）
        //递归方法
        function whileData(level, parentid) {
            for (var i = 0; i < data.length; i++) {
                for (var j = 0; j < tmpdata.length; j++) {
                    var obj = tmpdata[j];
                    if (parentid == 0) level = 0; //顶级树
                    if (obj[parentidname] == null) obj[parentidname] = 0; //兼容0和NULL的父ID
                    //找到子项
                    if (obj[parentidname] == parentid) {
                        fullname[level] = obj[textname]; //保存父项的名称
                        level++; //层级增加
                        var space = ""; //空格控制
                        if (isadditional) obj["rel"] = ""; //子项的fullname名称
                        for (var x = 0; x < level; x++) {
                            if (x > 0) space += "&nbsp;&nbsp;";
                            if (isadditional) obj["rel"] += fullname[x]; //子项的fullname名称
                        }
                        obj[textname] = space + obj[textname]; //text
                        nodes.push(obj);//目标数组
                        tmpdata.splice(tmpdata.indexOf(obj), 1); //从临时数组中REMOVE掉已经找到的项。
                        whileData(level--, obj[idname]); //递归
                    }
                }
            }
        }
        whileData(0, 0); //从顶级开始
        return $(this).bindSelect(nulltext, nodes, def, valuename, textname, callback, isadditional);
    },
    clearSelect: function () {//清除下拉框数据
        return this.each(function () {
            if ($(this).isTag("select"))
                $(this).empty();
        });
    },
    //联动下拉框
    caschange: function (args) {
        var $this = $(this);
        var key = "{\"" + args.key + "\":" + $this.val() + "}";
        key = JSON2.parse(key);
        args.data = $.extend({}, args.data, key);
        CAS.API({ type: "post", api: args.api, data: args.data, callback: function (d) {
            if (d.returntype == 1) {
                $("#" + args.subobj).bindSelect(args.nulltext, d.data, args.defval, args.fieldvalue, args.fieldtext, args.callback)
                .casselect();
            }
            else { $("#" + args.subobj).empty().casselect(); }
            $this.casselect();
            $this.unbind("change").bind("change", function () {
                args.defval = null;
                $(this).caschange(args);
            });
        }
        });
    },
    isTag: function (tn) { //根据tag判断HTML元素
        if (!tn || !$(this)[0]) return false;
        if (!$(this)[0].tagName) return false;
        return $(this)[0].tagName.toLowerCase() == tn ? true : false;
    },
    initUI: function () {
        return this.each(function () {
            if ($.isFunction(initUI)) initUI(this);
        });
    },
    hoverClass: function (className) { //鼠标hover样式
        var _className = className || "hover";
        return this.each(function () {
            $(this).hover(function () {
                $(this).addClass(_className);
            }, function () {
                $(this).removeClass(_className);
            });
        });
    },
    swapClass: function (className) { //如果没有则加上
        var _className = className;
        return this.each(function () {
            if (!$(this).hasClass(_className))
                $(this).addClass(_className);
        });
    },
    focusClass: function (className) { //焦点获得样式
        var _className = className || "focus";
        return this.each(function () {
            $(this).focus(function () {
                $(this).addClass(_className);
            }).blur(function () {
                $(this).removeClass(_className);
            });
        });
    },
    //转向
    cashref: function (url) {
        var $this = $(this)[0].location;
        url = url ? url : $this.href;
        if (url.indexOf("#") > 0) {
            url = url.substring(0, url.indexOf("#"));
        }
        if (CAS.isIE6)
            setTimeout(function () { $this.href = url; }, 10);
        else
            $this.href = url;
    },
    //滑动菜单
    casaccordion: function () {
        var objs = $(this);
        CAS.Use(["jquery.accordion.css", "jquery.accordion.js"], function () {
            return objs.accordion();
        });
    },
    //按钮loading
    casbtnloading: function () {
        var w = $(this).width();
        $(this).data("text", $(this).text());
        $(this).html("<em><span></span></em>");
        $(this).width(w);
        $(this).addClass("loadbtn").casdisable();
    },
    //按钮loaded
    casbtnloaded: function () {
        $(this).removeClass("loadbtn").casenable();
        $(this).html("<em><span></span>" + $(this).data("text") + "</em>");
    },
    //图片列表
    casimglist: function (args) {
        var objs = $(this);
        CAS.Use(["cas.imglist.js"], function () {
            return objs.casImgList(args);
        });
    },
    //按钮
    casbutton: function (args) {
        var objs = $(this);
        if (!$.fn.casButton)
            CAS.Use(["cas.button.js"], function () {
                return objs.casButton(args);
            });
        else
            return objs.casButton(args);
    },
    //必填
    casrequired: function () {
        return this.each(function () {
            var $this = $(this);
            function change() {
                $this.val($.trim($this.val()));
                var defvalue = "";
                if ($this.hasClass("iptval")) {
                    defvalue = $this.attr("rel");
                }
                if (!$this.hasClass("disabled") && ($this.val() == "" || $this.val() == defvalue)) {
                    $this.addClass("iptrequired");
                }
                else {
                    $this.removeClass("iptrequired");
                }
            }
            $this.addClass("must"); //用于控制此元素，否则remove了class后再找不到它了 kevin
            $this.focus(function () {
                $this.removeClass("iptrequired");
            })
            .blur(change)
            .change(change);
            change();
        });
    },
    casdefault: function () { //输入时默认值处理
        return this.each(function () {
            var $this = $(this);
            var defValue = $this.attr("rel");
            function def() {
                if ($this.val() == "" || $this.val() == defValue) {
                    $this.addClass("iptval");
                    $this.val(defValue);
                }
                else {
                    $this.removeClass("iptval");
                }
            }
            def();
            $this.focus(function () {
                if ($this.val() == "" || $this.val() == defValue)
                    $this.val("");
                else if ($this.val() != "" && $this.hasClass("iptval"))
                    $this.removeClass("iptval");
            }).bind("blur", function () {
                def();
            }).keydown(function () {
                $this.removeClass("iptval");
            }).change(def);
        });
    },
    //日期
    casdate: function () {
        var dates = $(this);
        dates.addClass("w80").casreadonly();
        CAS.Use(["jquery.dynDateTime.css", "jquery.dynDateTime.js", "cas.month.js"], function () {
            $.each(dates, function (i, item) {
                item = $(item);
                var format = item.attr("format");
                if (!format) {
                    return item.dynDateTime();
                } else if (format == "year") {

                } else if (format == "month") {
                    item.casDate({ format: "month" });
                } else {
                    return item.dynDateTime({ ifFormat: item.attr("format") });
                }
            });
        });
    },
    //数字
    casnumber: function () {
        var numbers = $(this);
        CAS.Use(["cas.input.js"], function () {
            return numbers.casNumber();
        });
    },
    //密码强度
    caspassword: function () {
        var pwds = $(this);
        CAS.Use(["cas.input.js"], function () {
            return pwds.casPassword();
        });
    },
    //只读
    casreadonly: function () {
        return this.each(function () {
            return $(this).attr("readonly", "readonly");
        });
    },
    //禁用
    casdisable: function () {
        return this.each(function () {
            var $this = $(this);
            if ($this.hasClass("iptrequired")) {
                $this.addClass("mustinput").removeClass("iptrequired"); //author:stickgod。target:去除必填项 //kevin:只去除如何还原？要加中转。
            }
            if ($this.hasClass("btn")) {
                $this.addClass("noclick");
                //因为用的是a标签，所以只设置disabled在有些浏览器是不会禁用点击事件的，所以这里把按钮点击事件缓存起来，可用时再重新绑定。 kevin
                var events = $this.data('events');
                if (events && events.click) {
                    $.each(events.click, function (key, handlerObject) {
                        var func = handlerObject.handler;
                        $this.data("click", func);
                    });
                }
                $this.unbind("click");
                $this.attr("disabled", "disabled");
            } else if ($this.hasClass("select")) {
                $this.attr("disabled", "disabled");
                $this.next().swapClass("disabled").find("input").swapClass("disabled");
            } else if ($this.hasClass("check_checkbox") || $this.hasClass("check_radio")) {
                if ($.fn.casCheckDisable)
                    $this.casCheckDisable(true);
                else {
                    CAS.Use(["cas.checkbox.js"], function () { $this.casCheckDisable(true); })
                }
            }
            else if ($this.hasClass("date")) {
                $this.attr("disabled", "disabled");
                $this.swapClass("disabled");
            } else if ($this.hasClass("swfupload")) {
                $this.hide();
            }
            else {
                $this.swapClass("disabled");
                $this.attr("readonly", "readonly");
                $("input, textarea,select,.check_checkbox,.check_radio,a.btn,.swfupload ", $this).casdisable();
            }
            return $this;
        });
    },
    //可用
    casenable: function () {
        return this.each(function () {
            var $this = $(this);
            $this.removeAttr("disabled").removeAttr("readonly");
            if ($this.hasClass("mustinput")) {
                $this.addClass("iptrequired").removeClass("mustinput");
            }
            if ($this.hasClass("btn")) {
                $this.removeClass("noclick");
                if ($this.data("click")) {
                    $this.bind("click", $this.data("click"));
                }
            } else if ($this.hasClass("select")) {
                $this.next().removeClass("disabled").find("input").removeClass("disabled");
            } else if ($this.hasClass("check_checkbox") || $this.hasClass("check_radio")) {
                if ($.fn.casCheckDisable)
                    $this.casCheckDisable(false);
                else {
                    CAS.Use(["cas.checkbox.js"], function () { $this.casCheckDisable(false); })
                }
            } else {
                $this.removeClass("disabled");
                $("input, textarea,select,.check_checkbox,.check_radio,a.btn ", $this).casenable();
            }
            return $this;
        });
    },
    //自带后缀
    casautoappend: function (args) {
        var appends = $(this);
        CAS.Use(["cas.autoappend.js"], function () {
            appends.each(function () {
                return $(this).casAutoAppend(args);
            });
        });
    },
    //下拉
    casselect: function () {
        var selects = $(this);
        if (!$.fn.casSelect) {
            CAS.Use(["cas.select.js"], function () {
                return selects.casSelect();
            });
        }
        else {
            return selects.casSelect();
        }
    },
    //提示
    castip: function (args) {
        args = args || {};
        var objs = $(this);
        CAS.Use(["cas.tip.js"], function () {
            return objs.casTip(args);
        });
    },
    //系统alert
    casalert: function (args) {
        return top.dialog.alert(args.content, args.callback || null, args.parent || null, args.title || null);
    },
    //进度条
    casprogress: function (args) {
        return top.dialog.progress(args.content, args.parent || null);
    },
    //系统confirm
    casconfirm: function (args) {
        return top.dialog.confirm(args.content, args.callback, args.cancel || null, args.parent || null);
    },
    //系统prompt
    casprompt: function (args) {
        args = args || {};
        var defaults = { value: "" };
        args = $.extend({}, defaults, args);
        return top.dialog.prompt(args.content, args.callback, args.value, args.parent || null);
    },
    //弹出窗口
    casdialog: function (args) {
        var $this = $(this);
        args = args || {};
        var defaults = { title: "CAS", lock: true, width: 500 };
        args = $.extend({}, defaults, args);
        return top.dialog(args);
    },
    //布局
    caslayout: function () {
        return this.each(function () {
            var $this = $(this);
            var iContentW = $this.parent().width();
            var iContentH = $this.parent().height();
            if ($this.parent().isTag("body")) {
                var iContentW = $(window).width();
                var iContentH = $(window).height();
            }
            $this.parent().css("overflow", "hidden");
            $this.width(iContentW);
            $this.height(iContentH);
            var topLayout = $("div.layout_top:visible", $this).eq(0);
            var leftLayout = $("div.layout_left", $this).eq(0);
            var centerLayout = $("div.layout_center", $this).eq(0);
            var rightLayout = $("div.layout_right", $this).eq(0);
            var bottomLayout = $("div.layout_bottom", $this).eq(0);

            if (!topLayout.parent().is($this)) topLayout = null;
            if (!leftLayout.parent().is($this)) leftLayout = null;
            if (!centerLayout.parent().is($this)) centerLayout = null;
            if (!rightLayout.parent().is($this)) rightLayout = null;
            if (!bottomLayout.parent().is($this)) bottomLayout = null;

            var topH = topLayout ? topLayout.height() : 0;
            var bottomH = bottomLayout ? bottomLayout.height() : 0;
            var leftW = leftLayout ? leftLayout.width() : 0;
            var rightW = rightLayout ? rightLayout.width() : 0;
            var spec_v = $("div.layout_spec_v", $this);
            var spec_h = $("div.layout_spec_h", $this);
            var specw = 0, spech = 0;
            spec_v.each(function () { if ($(this).parent().is($this)) specw += $(this).outerWidth(); });
            spec_h.each(function () { if ($(this).parent().is($this)) spech += $(this).outerHeight(); });
            var cH = iContentH - topH - bottomH - spech;
            var cW = iContentW - leftW - rightW - specw;

            if (leftLayout) leftLayout.height(cH);
            if (rightLayout) rightLayout.height(cH);
            if (centerLayout) centerLayout.height(cH).width(cW);
            var layouts = $("div.layout", centerLayout);
            if (centerLayout && layouts.size() > 0) {
                layouts.eq(0).caslayout();
            }
            layouts = $("div.layout", leftLayout);
            if (leftLayout && layouts.size() > 0) {
                layouts.eq(0).caslayout();
            }
            layouts = $("div.layout", rightLayout);
            if (rightLayout && layouts.size() > 0) {
                layouts.eq(0).caslayout();
            }
        });
    },
    //树 ，传参设置 ，不传参获取
    castree: function (args) {
        var $this = $(this);
        if (args) {
            CAS.Use(["jquery.zTree.css", "jquery.zTree.js"], function () {
                function beforeClick(treeId, treeNode, clickFlag) {
                    return (treeNode.click != false);
                }
                function dblClickExpand(treeId, treeNode) {
                    return (!treeNode.locked);
                }
                var setting = {
                    view: {
                        dblClickExpand: dblClickExpand
                    },
                    data: {
                        keep: {
                            parent: true,
                            leaf: false
                        },
                        simpleData: {
                            enable: true
                        }
                    },
                    callback: { beforeClick: beforeClick }
                };
                setting = $.extend(true, {}, setting, args);
                var t = $this.addClass("ztree");
                t = $.fn.zTree.init(t, setting, setting.nodes);
                return t;
            });
        } else {
            return $.fn.zTree.getZTreeObj($this.attr("id"));
        }
    },
    //右键
    cascontextmenu: function (args) {
        var $this = $(this);
        var menus = args.menus;
        var str = [];
        var id = "menu_" + CAS.RndVar();
        str.push('<ul id="' + id + '" class="cm_default">');
        function loadmenu(menu) {
            if (menu.text == "-") {
                str.push('<li class="separator"></li>');
            } else {
                str.push('<li');
                if (menu.callback) {
                    var call = "callmenu_" + CAS.RndVar();
                    eval("window." + call + " = menu.callback");
                    str.push(' onclick="' + call + '()"');
                }
                str.push('><span class="icon ' + (menu.icon ? menu.icon : "") + '"></span>' + menu.text);
                if (menu.submenu) {
                    str.push('<ul>');
                    $(menu.submenu).each(function (i, m) { loadmenu(m); });
                    str.push('</ul>');
                }
                str.push("</li>");
            }
        }
        $(menus).each(function (i, m) { loadmenu(m); });
        str.push("</ul>");
        var menustr = str.join("");

        $(menustr).hide().appendTo($("body"));

        CAS.Use(["jquery.menu.css", "jquery.jeegoocontext.js"], function () {
            $this.jeegoocontext(id, {
                widthOverflowOffset: 0,
                heightOverflowOffset: 3,
                submenuLeftOffset: -4,
                submenuTopOffset: -5
            });
        });
    },
    //tab标签 ，传参设置 ，不传参获取
    castabs: function (args) {
        var $this = $(this);
        if (args) {
            args = args || {};
            var defaults = {
                renderTo: $this.attr("id"),
                autoResizable: true,
                border: 'none',
                active: 0
            };
            args = $.extend({}, defaults, args);
            CAS.Use(["jquery.contextmenu.css", "jquery.contextmenu.js", "jquery.tabs.css", "jquery.tabs.js"], function () {
                tabpanel = new TabPanel(args);
                $this.data("tabpanel", tabpanel);
            });
        } else {
            return $this.data("tabpanel");
        }
    },
    //选择框
    cascheckbox: function (args) {
        args = args || {};
        $(this).each(function () {
            var $this = $(this);
            if ($.fn.casCheckBox)
                $this.casCheckBox(args)
            else
                CAS.Use(["cas.checkbox.js"], function () {
                    $this.casCheckBox(args);
                });
        });
    },
    //表格
    casgrid: function (args) {
        var $this = $(this);
        CAS.Use(["jquery.flexigrid.css", "jquery.flexigrid.js"], function () {
            $this.flexigrid(args);
        });
    },
    //输入框初始化
    casinput: function () {
        var $p = $(this);
        return this.each(function () {
            //所有输入加样式
            $("input[type=text], input[type=password], textarea", $p).addClass("input").focusClass("iptfocus");
            //输入框高度样式
            $("input[type=text], input[type=password]", $p);
        });
    },
    //星级评分
    casstar: function (args) {
        var $this = $(this);
        CAS.Use(["jquery.raty.js"], function () {
            $this.raty(args);
        });
    },
    //表单提交
    casform: function (args) {
        var $this = $(this);
        CAS.Use(["cas.form.js"], function () {
            $this.casForm(args);
        });
    },
    //自动完成 
    casautocomplete: function (args) {

        var $this = $(this);
        var options = {
            scrollHeight: 250,
            max: 200,
            matchContains: true,
            dataType: "json",
            //keycallback: function () { keycallback() },
            formatItem: function (row, i, max) {
                var formats = [];
                if (args.fieldformats) {
                    var len = args.fieldformats.length;
                    for (var i = 0; i < len; i++) {
                        var begin = args.fieldformats[i].begin;
                        if (!begin) begin = "";
                        var end = args.fieldformats[i].end;
                        if (!end) end = "";
                        formats.push(begin + row[args.fieldformats[i].field] + end);
                    }
                    return formats.join("");
                }
            },
            formatMatch: function (row, i, max) {
                if (args.fieldmatchs) {
                    var s = "";
                    var len = args.fieldmatchs.length;
                    for (var i = 0; i < len; i++) {
                        s += row[args.fieldmatchs[i]] + " ";
                    }
                    return s;
                }
            },
            formatResult: function (row) {
                return row[args.fieldresult];
            }
        }
        options = $.extend({}, options, args.options);
        if ($.fn.autocomplete) {
            $this.autocomplete(args.data, options).result(
                function (event, data, formatted) {
                    args.callback(event, data, formatted);
                });
        }
        else {
            CAS.Use(["jquery.autocomplete.css", "jquery.autocomplete.js"], function () {
                $this.autocomplete(args.data, options).result(
                function (event, data, formatted) {
                    args.callback(event, data, formatted);
                });
            });
        }
    },
    //取消自动完成
    casunautocomplete: function () {
        var $this = $(this);
        if ($.fn.unautocomplete) {
            $this.unautocomplete();
        }
        else {
            CAS.Use(["jquery.autocomplete.css", "jquery.autocomplete.js"], function () {
                $this.unautocomplete();
            });
        }
    },
    //自动完成楼盘
    //这里改成了全自动化调用 kevin
    casautoproject: function (args) {
        var $this = $(this);
        CAS.Use(["cas.autoproject.js"], function () {
            $this.casAutoProject(args);
        });
        return this;
    },
    //自动完成公司
    casautocompany: function (args) {
        var tmpcvalue = "";
        var $this = $(this);
        //处理数据
        function _return(data) {
            if (tmpcvalue == $this.val()) return;
            $("#" + args.objcompanyid).val("");
            if (data && data.companyid) {
                $("#" + args.objcompanyid).val(data.companyid);
            }
            tmpcvalue = $this.val();
        }
        $(this).casautocomplete({
            fieldformats: [{ field: "chinesename"}],
            fieldresult: "chinesename", //返回的字段
            data: "company.companylist", //API地址或者json数据
            options: { extraParams: { type: "list", cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid} }, //扩展选项
            callback: function (event, data, formatitem) {//回调函数
                _return(data);
                if (args && args.callback) args.callback(data);
            }
        });
        //处理找不到的情况
        $this.bind("change", function () {
            if (args && args.callback) {
                args.callback(null);
            }
            _return(null);
        });
        return this;
    },
    casvideo: function (args) {
        var $this = $(this);
        var width = args.width || 500;
        var height = args.height || 300;
        CAS.Use(["videoobject.js"], function () {
            if (args.filepath != null) {//加载视频,视频转化成flash后 去掉 1==0
                var file = CAS.RootUrl + "flash/flvplayer.swf";
                var s1 = new SWFObject(file, "single", width, height, "7");
                s1.addParam("allowfullscreen", "true");
                s1.addVariable("file", args.filepath);
                s1.addVariable("image", "preview.jpg");
                s1.addVariable("width", width);
                s1.addVariable("height", height);
                s1.write($this.attr("id"));
            }
        })
    },
    //图片展示
    casimageview: function (args) {
        var $this = $(this);
        CAS.Use(["cas.imageview.js"], function () {
            $this.casImageView(args);
        });
    },
    //图表
    caschart: function (args) {
        var $this = $(this);
        var ofc = CAS.RootUrl + "flash/open-flash-chart.swf";
        var width = args.width || 500;
        var height = args.height || 300;
        var version = "9.0.0";
        var flashvar = {};
        var parms = { wmode: "transparent" }; //透明，修复其他浏览器置于顶层的问题
        CAS.Use(["swfobject.js"], function () {
            if (!$("#caschart_" + $this.attr("id")).attr("id")) {
                var div = document.createElement("div");
                div.id = "caschart_" + $this.attr("id");
                $(div).insertAfter($this).hide().html("<table class='w100p h100p'><tr><td class='tc tm fw f20 h100p'>很抱歉，没有找到满足条件的数据...</td></tr></table>");
            }
            $("#caschart_" + $this.attr("id")).hide();
            if (args.localdata) {
                //重要：ofc是异步加载的，所以open_flash_chart_data方法需要是全局函数。此函数名也不能修改，官方文档是错的 kevin
                //修改,args.localdata参数:从API传过来的data必须json2.parse转下才能使用 stickgod
                window.open_flash_chart_data = function () {
                    return JSON2.stringify(args.localdata);
                }
                flashvar = { "data-get": open_flash_chart_data };
                loadchart();
            } else if (args.api) {
                CAS.API({ type: args.type, api: args.api, data: args.data, callback: function (data) {
                    if (data.returntype == 1) {
                        window.open_flash_chart_data = function () {
                            return data.data;
                        }
                        var fdata = JSON2.parse(data.data);
                        if (fdata && fdata.elements.length > 0 && fdata.elements[0].values.length > 0) {
                            flashvar = { "data-get": open_flash_chart_data };
                            loadchart();
                        } else {
                            //CAS.Alert("找不到数据");
                            $this.hide();
                            $("#caschart_" + $this.attr("id")).show().width(width).height(height);
                        }
                    }
                }
                });
            } else if (args.url) {
                flashvar = { "data-file": args.url };
                loadchart();
            }
            function loadchart() {
                var o = swfobject.embedSWF(ofc, $this.attr("id"), width, height, version, null, flashvar, parms);
            }
        });
    },
    //编辑器
    caseditor: function (args) {
        var $this = $(this);
        //参考http://hi.baidu.com/vfoxer/blog/item/b36cf1f297fbcc1fb17ec542.html
        function getLocalUrl(url, urlType, urlBase)//绝对地址：abs,根地址：root,相对地址：rel
        {
            if ((url.match(/^(\w+):\/\//i) && !url.match(/^https?:/i)) || /^#/i.test(url) || /^data:/i.test(url)) return url; //非http和https协议，或者页面锚点不转换，或者base64编码的图片等
            var baseUrl = urlBase ? $('<a href="' + urlBase + '" />')[0] : location, protocol = baseUrl.protocol, host = baseUrl.host, hostname = baseUrl.hostname, port = baseUrl.port, path = baseUrl.pathname.replace(/\\/g, '/').replace(/[^\/]+$/i, '');
            if (port === '') port = '80';
            if (path === '') path = '/';
            else if (path.charAt(0) !== '/') path = '/' + path; //修正IE path
            url = $.trim(url);
            //删除域路径
            if (urlType !== 'abs') url = url.replace(new RegExp(protocol + '\\/\\/' + hostname.replace(/\./g, '\\.') + '(?::' + port + ')' + (port === '80' ? '?' : '') + '(\/|$)', 'i'), '/');
            //删除根路径
            if (urlType === 'rel') url = url.replace(new RegExp('^' + path.replace(/([\/\.\+\[\]\(\)])/g, '\\$1'), 'i'), '');
            //加上根路径
            if (urlType !== 'rel') {
                if (!url.match(/^(https?:\/\/|\/)/i)) url = path + url;
                if (url.charAt(0) === '/')//处理根路径中的..
                {
                    var arrPath = [], arrFolder = url.split('/'), folder, i, l = arrFolder.length;
                    for (i = 0; i < l; i++) {
                        folder = arrFolder[i];
                        if (folder === '..') arrPath.pop();
                        else if (folder !== '' && folder !== '.') arrPath.push(folder);
                    }
                    if (arrFolder[l - 1] === '') arrPath.push('');
                    url = '/' + arrPath.join('/');
                }
            }
            //加上域路径
            if (urlType === 'abs' && !url.match(/^https?:\/\//i)) url = protocol + '//' + host + url;
            url = url.replace(/(https?:\/\/[^:\/?#]+):80(\/|$)/i, '$1$2'); //省略80端口
            return url;
        }
        CAS.Use(["xheditor.js"], function () {
            $this.each(function () {
                var op = { html5Upload: false, skin: 'o2007silver', tools: args.tools || 'simeple' || $(this).attr("tools")
                    , urlBase: CAS.StaticUrl
                };
                var upAttrs = [
				    ["upLinkUrl", "upLinkExt", "zip,rar,txt"],
				    ["upImgUrl", "upImgExt", "jpg,jpeg,gif,png"],
				    ["upFlashUrl", "upFlashExt", "swf"],
				    ["upMediaUrl", "upMediaExt", "avi"]
			    ];
                $(upAttrs).each(function (i) {
                    var urlAttr = upAttrs[i][0];
                    var extAttr = upAttrs[i][1];
                    op[urlAttr] = CAS.APIUrl + "file/upload.ashx?type=editor";
                    op[extAttr] = $(this).attr(extAttr) || upAttrs[i][2];
                });
                $(this).xheditor(op);
            });
        });
    },
    //上传文件
    casupload: function (args) {
        var $this = $(this);
        var id = $this.attr("id") || "upload_" + Math.random().toString().replace(".", "");
        if (args.progress) {
            var obj = $('<span id="' + id + 'divStatus"></span><button id="' + id + 'btnCancel" class="dn">取消上传</button><div class="flash" id="' + id + 'fsUploadProgress"></div>');
            obj.insertAfter($this);
        }
        //列表和删除 kevin
        if (args.list) {
            if (!args.listid) {
                var list = $('<div class="files"><ul id="' + id + 'files" class="picFile"></ul></div>');
                list.insertBefore($this);
            } else {
                $("#" + args.listid).swapClass("files").html('<ul id="' + id + 'files" class="picFile"></ul>');
            }
        }
        if (args.list && args.files.length > 0) {
            $.each(files, function (i, item) {
                var txt = "<li><p><a target='_blank' href=\"" + CAS.APIUrl + item.filepath + "\">" + item.filename + "</a></p>";
                if (args.remove) txt += "<a href='#' class='close_pic'></a>";
                txt += "</li>";
                var link = $(txt).appendTo($("#" + id + "files"));
                $("a", link).click(function () {
                    if ($(this).hasClass("close_pic")) {
                        var index = $(this).parent().parent().find("li").index($(this).parent()[0]);
                        args.files.splice(index);
                        $(this).parent().remove();
                    }
                    else {
                        var file = $(this).attr("href");
                        if (CAS.isImage(file)) {
                            CAS.Dialog({ title: "查看附件", min: false, content: "url:" + file });
                        }
                    }
                    return false;
                });
            });
        }
        //参考：http://demo.swfupload.org/v220/index.htm
        var url = "";
        if (args.url) url = args.url;
        else url = CAS.APIUrl + "file/upload.ashx";
        if (args.params) {
            url += "?" + $.param(args.params);
        }
        CAS.Use(["swfupload.css", "swfupload.js"], function () {
            var defaults = {
                moviename: "SWFUpload_" + id,
                flash_url: CAS.RootUrl + "flash/swfupload.swf",
                upload_url: url,
                file_size_limit: "30000000", //最大30M，与web.config要保持一致
                file_types: "*.*",
                file_types_description: "所有文件",
                file_upload_limit: 100,
                file_queue_limit: 1,
                custom_settings: {
                    progressTarget: id + "fsUploadProgress",
                    cancelButtonId: id + "btnCancel",
                    statusId: id + "divStatus",
                    progress: args.progress,
                    list: args.list, //是否列表
                    listId: id + "files", //列表ID
                    remove: args.remove, //是否可删除
                    files: args.files, //文件列表数组变量
                    //上传完成回调
                    callback: args.callback || null
                },
                debug: false,
                // Button Settings
                button_placeholder_id: $this.attr("id"),
                button_image_url: CAS.StaticUrl + "images/swfupload.png",
                button_width: 61,
                button_height: 22,
                //button_text:"点击上传",
                button_window_mode: SWFUpload.WINDOW_MODE.TRANSPARENT,
                //button_cursor: SWFUpload.CURSOR.HAND,
                // The event handler functions are defined in handlers.js
                swfupload_loaded_handler: swfUploadLoaded,
                file_queued_handler: fileQueued,
                file_queue_error_handler: fileQueueError,
                file_dialog_complete_handler: fileDialogComplete,
                upload_start_handler: uploadStart,
                upload_progress_handler: uploadProgress,
                upload_error_handler: uploadError,
                upload_success_handler: uploadSuccess,
                upload_complete_handler: uploadComplete,
                queue_complete_handler: queueComplete, // Queue plugin event
                // SWFObject settings
                minimum_flash_version: "9.0.28",
                swfupload_pre_load_handler: swfUploadPreLoad,
                swfupload_load_failed_handler: swfUploadLoadFailed
            };
            if (args.limit) {
                //defaults.file_upload_limit = args.limit;
                defaults.file_queue_limit = args.limit;
            }
            if (args.size) {
                defaults.file_size_limit = args.size;
            }
            if (args.type) {
                switch (args.type) {
                    case "image":
                        defaults.file_types = "*.jpg;*.png;*.bmp";
                        defaults.file_types_description = "图片";
                        break;
                    case "office":
                        defaults.file_types = "*.doc;*.docx;*.xls;*.xlsx;*.ppt;*.pptx";
                        defaults.file_types_description = "文档";
                        break;
                    case "excel":
                        defaults.file_types = "*.xls;*.xlsx;";
                        defaults.file_types_description = "Excel文档";
                        break;
                    case "word":
                        defaults.file_types = "*.doc;*.docx;";
                        defaults.file_types_description = "Word文档";
                        break;
                    case "video":
                        defaults.file_types = "*.3gp;*.flv";
                        defaults.file_types_description = "视频";
                        break;
                }
            }
            args = $.extend({}, defaults, args);
            swfu = new SWFUpload(args);
        });
    },
    //带收缩展开的表格
    //只有一个表格时，不收缩展开，避免不必要的交互
    castable: function (args) {
        args = args || {};
        $(this).attr("cellpadding", "0").attr("cellspacing", "1")
        .each(function () {
            var $this = $(this);
            var cnt = $("table.table").size();
            var div = $('<div class="tablecontainer"><div class="tableheader"><span class="tabletitle">' + $this.attr("rel") + '</span>' + (cnt > 1 ? '<a class="slidebtn">▲</a>' : '') + '</div><div class="tablecontent"></div></div>');
            div.insertBefore($this);
            function setpos() {
                setTimeout(function () {
                    var p = div.parent()[0];
                    div.css({ width: p.clientWidth - 22 });
                }, 50);
            }
            $this.appendTo($(".tablecontent", div)).addClass("w100p");
            setTimeout(setpos, 20);
            if (cnt <= 1 || args.noslide)
                $("a.slidebtn", div).hide();
            $(window).bind("resize", function () {
                setpos();
            });
            $("div.tableheader", div).each(function () {
                var $this = $(this);
                var table = $this.next();
                if (cnt > 1 && !args.noslide)
                    $("a.slidebtn", this).click(function () {
                        var btn = $(this);
                        table.slideToggle(function () {
                            if (btn.text() == "▼") btn.html("▲");
                            else btn.html("▼");
                        });
                    });
            });
        });
    },
    //修复IE6png透明
    casfixpng: function () {
        var $this = $(this);
        CAS.Use(["DD_belatedPNG.js"], function () {
            $this.each(function (i) {
                DD_belatedPNG.fixPng($this.eq(i)[0]);
            });
        });
    },
    //圆角
    cascorner: function () {
        var $this = $(this);
        CAS.Use(["jquery.corner.js"], function () {
            $this.each(function (i) {
                $(this).corner("round");
            });
        });
    },
    //触发C++的窗口托动
    casmovec: function () {
        if (CAS.isC()) $(this).bind("mousedown", function (e) {
            if (e.srcElement && e.srcElement == $(this)[0] || e.srcElement.className == 'title') {
                external.MoveWindows();
            }
        });
    },
    //简单tab
    cassimpletab: function (args) {
        var $this = $(this);
        CAS.Use(["cas.simpletab.js"], function () {
            $this.each(function (i) {
                $(this).casSimpleTab(args);
            });
        });
    },
    //交叉表
    cascrosstable: function (args) {
        var $this = $(this);
        CAS.Use(["cas.crosstable.js"], function () {
            $this.each(function (i) {
                $(this).casCrossTable(args);
            });
        });
    },
    //退出系统
    casexit: function () {
        var $this = $(this);
        $this.bind("click", function () {
            CAS.Confirm({ content: "确定要退出系统吗？", callback: function () {
                CAS.API({ type: "post", api: "user.logout", data: { exit: "1", userid: CAS.Define.userid }, callback: function (data) {
                    if (data.returntype == 1) {
                        if (CAS.isC()) { //在C窗口中，调用C的方法退出系统。
                            external.CloseCAS();
                            return;
                        }
                        else
                            $(window.top).cashref(CAS.RootUrl);
                    }
                }
                })
            }
            });
        });
    },
    //用户设置
    casusersetup: function () {
        var $this = $(this);
        $this.bind("click", function () {
            var url = "page.structure.user_info";
            var title = "用户设置";
            CAS.Dialog({ win: window, title: title, initmax: false, min: false, content: "url:" + CAS.APIPage({ api: url, data: { isself: 1} }), width: 700, height: 550 });
        });
    },
    //只选年的下拉框
    casselectyear: function (args) {
        var $this = $(this);
        args = args || {};
        var end = args.end ? args.end : new Date().getFullYear();
        var begin = args.begin ? args.begin : 2010;
        var select = args.select ? args.select : end;
        var ops = [];
        for (var i = begin; i <= end; i++) {
            ops.push("<option value='" + i + "' " + (i == select ? "selected" : "") + ">" + i + "</option>");
        }
        $this.html(ops.join(""));
    },
    //拷贝
    cascopy: function (args) {
        var $this = $(this);
        CAS.Use(["ZeroClipboard.js"], function () {
            clip = new ZeroClipboard.Client();
            clip.addEventListener('complete', function (client, text) {
                alert('以下内容已成功复制到剪贴板：' + text);
            });
            clip.addEventListener('mousedown', function (client, text) {
                clip.setText(args.gettext());
            });
            clip.glue($this);
        });
    }
});


function FormatJSON(oData, sIndent) {
    if (arguments.length < 2) {
        var sIndent = "";
    }
    var sIndentStyle = "    ";
    var sDataType = RealTypeOf(oData);

    // open object
    if (sDataType == "array") {
        if (oData.length == 0) {
            return "[]";
        }
        var sHTML = "[";
    } else {
        var iCount = 0;
        $.each(oData, function () {
            iCount++;
            return;
        });
        if (iCount == 0) { // object is empty
            return "{}";
        }
        var sHTML = "{";
    }

    // loop through items
    var iCount = 0;
    $.each(oData, function (sKey, vValue) {
        if (iCount > 0) {
            sHTML += ",";
        }
        if (sDataType == "array") {
            sHTML += ("\n" + sIndent + sIndentStyle);
        } else {
            sHTML += ("\n" + sIndent + sIndentStyle + "\"" + sKey + "\"" + ": ");
        }

        // display relevant data type
        switch (RealTypeOf(vValue)) {
            case "array":
            case "object":
                sHTML += FormatJSON(vValue, (sIndent + sIndentStyle));
                break;
            case "boolean":
            case "number":
                sHTML += vValue.toString();
                break;
            case "null":
                sHTML += "null";
                break;
            case "string":
                sHTML += ("\"" + vValue + "\"");
                break;
            default:
                sHTML += ("TYPEOF: " + typeof (vValue));
        }

        // loop
        iCount++;
    });

    // close object
    if (sDataType == "array") {
        sHTML += ("\n" + sIndent + "]");
    } else {
        sHTML += ("\n" + sIndent + "}");
    }

    // return
    return sHTML;
}
function RealTypeOf(v) {
    if (typeof (v) == "object") {
        if (v === null) return "null";
        if (v.constructor == (new Array).constructor) return "array";
        if (v.constructor == (new Date).constructor) return "date";
        if (v.constructor == (new RegExp).constructor) return "regex";
        return "object";
    }
    return typeof (v);
}

//跟踪调试
var loger;
function log(msg, type) {
    if (CAS.Debug) { //开启调试
        if (window.console && window.console.log) {
            window.console.log(msg);
            return;
        }
        if ((typeof msg) == "object") {
            try {
                msg = "\n" + FormatJSON(msg);
            } catch (e) { }
        }
        if (!loger) {
            loger = log4javascript.getDefaultLogger();
        }
        if (!type)
            loger.debug(msg);
        else {
            eval("loger." + type + "('" + msg + "')");
        }
    }
}

//日期格式
var weeks = ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'];
//var ddd = new Date();
//var week = weeks[ddd.format('w')];
//document.write(ddd.format('yyyy年MM月dd日 ' + week + ' hh:mm:ss'));
//format=chs 用几分钟前，几天前等表示 kevin
Date.prototype.format = function (format) {
    var o =
    {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds(), //millisecond
        "w": this.getDay()
    }
    if (format == "chs") {
        var date2 = new Date();
        var date3 = date2.getTime() - this.getTime()
        var days = Math.floor(date3 / (24 * 3600 * 1000))
        var leave1 = date3 % (24 * 3600 * 1000)
        var hours = Math.floor(leave1 / (3600 * 1000))
        var leave2 = leave1 % (3600 * 1000)
        var minutes = Math.floor(leave2 / (60 * 1000))
        var leave3 = leave2 % (60 * 1000)
        var seconds = Math.round(leave3 / 1000)
        format = (days > 365) ? Math.round(days / 365) + "年前" :
        (
            (days > 30 && days < 365) ? Math.round(days / 30) + "月前" :
            (
                (days > 0 && days < 31) ? days + "天前" :
                (
                    (hours > 0) ? hours + "小时前" :
                    (
                        (minutes > 0) ? minutes + "分钟前" : "1分钟前"
                    )
                )
            )
        );
    }
    else {
        if (/(y+)/.test(format))
            format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(format))
                format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    }
    return format;
}

String.format = function () {
    if (arguments.length == 0)
        return null;

    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}

//初始化布局
function initLayout(_box) {
    var $p;
    if ($(_box).isTag("iframe")) {
        $p = $($(_box)[0].contentWindow.document);
    }
    else $p = $(document);
    var layouts = $("div.layout", $p)
    if (layouts.size() > 0)
        setTimeout(function () {
            layouts.eq(0).caslayout();
        }, 10);
}

//初始化组件
function initUI(_box) {
    var $p = $(_box || document);
    $p.casinput();
    //只读
    $("input.readonly, textarea.readonly", $p).casreadonly();
    //禁用
    $("input.disabled, textarea.disabled", $p).casdisable();
    //默认值
    $("input.iptval , textarea.iptval", $p).casdefault();
    //必填 
    $("input.iptrequired , textarea.iptrequired", $p).casrequired();
    //日期控件
    $("input.date", $p).casdate();
    //数字控件
    $("input.number", $p).casnumber();
    //下拉框
    $("select", $p).casselect();
    //自动带后缀，如邮箱
    $("input.autoappend", $p).each(function () {
        var $this = $(this);
        var list;
        if ($this.hasClass("email")) list = CAS.Mails;
        //else if ($this.hasClass("company")) list = ["@fxt", "@zzgy", "@tsgx"];
        return $this.casautoappend({ list: list });
    });
    //密码强度
    $("input.pwdstrong", $p).caspassword();
    //按钮
    $("a.btn", $p).casbutton();
    //IE去除链接的虚线框
    if (CAS.isIE()) $("a").attr("hidefocus", true);
    //光标
    $(".input", $p).not(".date").css("cursor", "text");
}

$(function () {

    //清理浏览器内存,只对IE起效
    if (CAS.isIE()) {
        window.setInterval(CollectGarbage, 10000);
    }

    //修复IE背景图片使用缓存
    if (CAS.isIE6()) {
        try {
            $('.png').casfixpng();
            document.execCommand("BackgroundImageCache", false, true);
        } catch (e) { }
    }    
    //resize时重新布局
    $(window).bind("resize", function () {
        initLayout(this);
    }).bind("unload", function () {
        if (CAS.isIE()) CollectGarbage();
    });
    //$("iframe").bind("resize", function () { initLayout(this); });
    //初始化布局
    initLayout(this);
    //初始化控件
    initUI();
});

$.ajaxSetup({ cache: false });

//重新登录 kevin
function ReLogin() {
    CAS.Alert({ content: "很抱歉，您可能已退出，请重新登录。", callback: function () {
        CAS.API({ type: "post", api: "user.logout", data: { exit: "1",
            cityid: CAS.Define.cityid,
            fxtcompanyid: CAS.Define.fxtcompanyid,
            systypecode: CAS.Define.systypecode,
            username: CAS.Define.username,
            userid: CAS.Define.userid
        }, callback: function (data) {
                $(window.top).cashref(CAS.RootUrl);
        }
        });
    }
    });
}