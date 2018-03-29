﻿/*!
* lhgcore Dialog Plugin v4.2.0
* Date: 2012-04-19 10:55:11 
* http://code.google.com/p/dialog/
* Copyright 2009-2012 LiHuiGang
*/

; (function ($, window, undefined) {
    var _ie6 = window.ActiveXObject && !window.XMLHttpRequest,
	_fn = function () { },
	_count = 0,
	_rurl = /^url:/,
	_singleton,
	onKeyDown,
	document = window.document,
    topwindow = window.top,
	expando = 'JDG' + Math.random().toString().replace(".",""),

dialogTpl =
'<table class="ui_border">' +
	'<tbody>' +
		'<tr>' +
			'<td class="ui_lt"></td>' +
			'<td class="ui_t"></td>' +
			'<td class="ui_rt"></td>' +
		'</tr>' +
		'<tr>' +
			'<td class="ui_l"></td>' +
			'<td class="ui_c">' +
				'<div class="ui_inner">' +
				'<table class="ui_dialog">' +
					'<tbody>' +
						'<tr>' +
							'<td colspan="2">' +
								'<div class="ui_title_bar">' +
									'<div class="ui_title" unselectable="on"></div>' +
									'<div class="ui_title_buttons">' +
										'<a class="ui_min" href="javascript:void(0);" title="\u6700\u5C0F\u5316"><b class="ui_min_b"></b></a>' +
										'<a class="ui_max" href="javascript:void(0);" title="\u6700\u5927\u5316"><b class="ui_max_b"></b></a>' +
										'<a class="ui_res" href="javascript:void(0);" title="\u8FD8\u539F"><b class="ui_res_b"></b><b class="ui_res_t"></b></a>' +
										'<a class="ui_close" href="javascript:void(0);" title="\u5173\u95ED">\xd7</a>' +
									'</div>' +
								'</div>' +
							'</td>' +
						'</tr>' +
						'<tr>' +
							'<td class="ui_icon"></td>' +
							'<td class="ui_main">' +
								'<div class="ui_content"></div>' +
							'</td>' +
						'</tr>' +
						'<tr>' +
							'<td colspan="2">' +
								'<div class="ui_buttons"></div>' +
							'</td>' +
						'</tr>' +
					'</tbody>' +
				'</table>' +
				'</div>' +
			'</td>' +
			'<td class="ui_r"></td>' +
		'</tr>' +
		'<tr>' +
			'<td class="ui_lb"></td>' +
			'<td class="ui_b"></td>' +
			'<td class="ui_rb"></td>' +
		'</tr>' +
	'</tbody>' +
'</table>',

    /*!
    * _path 获取组件核心文件dialog.js所在的绝对路径
    * _args 获取dialog.js文件后的url参数组，如：dialog.js?self=true&skin=aero中的?后面的内容
    */
_args, _path = (function (script, i, me) {
    var l = script.length;

    for (; i < l; i++) {
        me = !!document.querySelector ?
		    script[i].src : script[i].getAttribute('src', 4);

        if (me.substr(me.lastIndexOf('/')).indexOf('dialog') !== -1)
            break;
    }

    me = me.split('?'); _args = me[1];

    return me[0].substr(0, me[0].lastIndexOf('/') + 1);
})(document.getElementsByTagName('script'), 0),

    /*!
    * 获取url参数值函数
    * @param  {String}
    * @return {String||null}
    * @demo dialog.js?skin=aero | _getArgs('skin') => 'aero'
    */
_getArgs = function (name) {
    if (_args) {
        var p = _args.split('&'), i = 0, l = p.length, a;
        for (; i < l; i++) {
            a = p[i].split('=');
            if (name === a[0]) return a[1];
        }
    }
    return null;
},

    /*! 取皮肤样式名，默认为 default */
_skin = _getArgs('skin') || CAS.Style,

    /*! 获取 dialog 可跨级调用的最高层的 window 对象和 document 对象 */
_doc, _top = (function (w) {
    try {
        _doc = w['top'].document;  // 跨域|无权限
        _doc.getElementsByTagName; // chrome 浏览器本地安全限制
    } catch (e) {
        _doc = w.document; return w;
    };

    // 如果指定参数self为true则不跨框架弹出，或为框架集则无法显示第三方元素
    if (_getArgs('self') === 'true' ||
	    _doc.getElementsByTagName('frameset').length > 0) {
        _doc = w.document; return w;
    }

    return w['top'];
})(window),

_root = _doc.documentElement, _doctype = _doc.compatMode === 'BackCompat';

    _$doc = $(_doc), _$top = $(_top), _$html = $(_doc.getElementsByTagName('html')[0]);

    /*! 在最顶层页面添加样式文件 */
    CAS.Use(["jquery.dialog.css"],null);       

    /*!----------------------------------以下为dialog核心代码部分----------------------------------*/

    var dialog = function (config) {
        config = config || {};
        var api, setting = dialog.setting;    
        
        // 合并默认配置
        for (var i in setting) {
            if (config[i] === undefined) config[i] = setting[i];
        }
        
        config.id = config.id || expando + _count;
        // 如果定义了id参数则返回存在此id的窗口对象
        api = dialog.list[config.id];
        
        /*!
        * 全局快捷键
        * 由于跨框架时事件是绑定到最顶层页面，所以当当前页面卸载时必须要除移此事件
        * 所以必须unbind此事件绑定的函数，所以这里要给绑定的事件定义个函数
        * 这样在当前页面卸载时就可以移此事件绑定的相应函数，不而不影响顶层页面此事件绑定的其它函数
        */
        onKeyDown = function (event) {
            var target = event.target,
            api = dialog.focus,
            keyCode = event.keyCode;
            if (api){
                if(keyCode==CAS.KeyCode.TAB){ 
                    if(api.config.id=="Alert" ||api.config.id=="Confirm"||api.config.id=="Prompt")
                        return false;
                }
                if(keyCode==CAS.KeyCode.ESC){
                    if(api.config.id=="Alert" ||api.config.id=="Confirm"||api.config.id=="Prompt"){
                        api.close();
                    }
                } 
                else if(keyCode==CAS.KeyCode.ENTER){
                    if(api.config.id=="Alert")
                        api.close();
                    else if(api.config.id=="Confirm"||api.config.id=="Prompt"){
                        api.config.ok();
                    }
                    CAS.stopPropagation(event);
                    return false;
                }
            }
        };

        _$doc.bind('keydown', onKeyDown);

        if (api){
            dialog.focus=api;
            return api.zindex().focus();
        }
        // 按钮队列
        config.button = config.button || [];

        config.ok &&
	    config.button.push({
	        id: 'ok',
	        name: config.okVal,
	        callback: config.ok,
	        focus: config.focus
	    });

            config.cancel &&
	    config.button.push({
	        id: 'cancel',
	        name: config.cancelVal,
	        callback: config.cancel
	    });

        // zIndex全局配置 修复 kevin
        if(!topwindow.dialogzIndex)
            topwindow.dialogzIndex = config.zIndex;
        
        _count++;
        return dialog.list[config.id] = _singleton ?
	    _singleton._init(config) : new dialog.fn._init(config);        
    };

    dialog.fn = dialog.prototype =
{
    constructor: dialog,

    _init: function (config) {
        var that = this, DOM,
		    content = config.content,
			isIfr = _rurl.test(content);

        that.opener = window;
        that.config = config;

        that.DOM = DOM = that.DOM || that._getDOM();
        that.closed = false;
        that.data = config.data;

        // 假如提示性图标为真默认不显示最小化和最大化按钮
        if (config.icon && !isIfr) {
            config.min = false;
            config.max = false;

            DOM.icon[0].style.display = '';
            DOM.icon[0].innerHTML = '<img src="' + CAS.StaticUrl + 'css/' + CAS.Style + '/images/dialog/icons/' + config.icon + '" class="ui_icon_bg"/>';
        }
        else
            DOM.icon[0].style.display = 'none';

        DOM.wrap.addClass(config.skin); // 多皮肤共存
        DOM.rb[0].style.cursor = config.resize ? 'se-resize' : 'auto';
        DOM.title[0].style.cursor = config.drag ? 'move' : 'auto';
        DOM.max[0].style.display = config.max ? 'inline-block' : 'none';
        DOM.min[0].style.display = config.min ? 'inline-block' : 'none';
        DOM.close[0].style.display = config.cancel === false ? 'none' : 'inline-block'; //当cancel参数为false时隐藏关闭按钮
        DOM.content[0].style.padding = config.padding;

        that.button.apply(that, config.button);
        that.title(config.title)
		.content(content, true, isIfr)
		.size(config.width, config.height)
		.position(config.left, config.top)
		.time(config.time)
		[config.show ? 'show' : 'hide'](true).zindex();
        config.focus && that.focus();
        config.lock && that.lock();
        that._ie6PngFix()._addEvent();
        
        _singleton = null;

        // 假如加载的是单独页面的内容页config.init函数会在内容页加载完成后执行，这里就不执行了
        if (!isIfr && config.init)
            config.init.call(that, window);
        //直接最大化 kevin
        if(config.initmax){
            setTimeout(function(){
                that.max();},10);
        }   
        dialog.focus=that;             
        return that;
    },

    /**
    * 自定义按钮
    * @example
    button({
    name: 'login',
    callback: function(){},
    disabled: false,
    focus: true
    }, .., ..)
    */
    button: function () {
        var that = this, DOM = that.DOM,
		    buttons = DOM.buttons[0],
			focusButton = 'ui_state_highlight',
			listeners = that._listeners = that._listeners || {},
			ags = [].slice.call(arguments),
			i = 0, item, value, id, isNewButton, button;

        for (; i < ags.length; i++) {
            item = ags[i];

            value = item.name;
            id = item.id || value;
            isNewButton = !listeners[id];
            button = !isNewButton ? listeners[id].elem : _doc.createElement('a');
            
            $(button).addClass("btn");

            if (!listeners[id])
                listeners[id] = {};

            if (value)
                button.innerText = value;

            if (item.callback)
                listeners[id].callback = item.callback;

            if (item.focus) {
                that._focus && that._focus.removeClass(focusButton);
                that._focus = $(button).addClass(focusButton);
                that.focus();
            }

            button[expando + 'callback'] = id;
            button.disabled = !!item.disabled;
            
            if (isNewButton) {
                listeners[id].elem = button;
                buttons.appendChild(button);
            }
            $(button).addClass("ml5");
            $(button).casbutton();
            
            if(id=="ok")
                $(button).addClass("btnenter");        
        }

        buttons.style.display = ags.length ? '' : 'none';
        
        that._ie6SelectFix();

        return that;
    },

    /**
    * 设置标题
    * @param	{String, Boolean}	标题内容. 为false则隐藏标题栏
    * @return	{this}	如果无参数则返回对象本身
    */
    title: function (text) {
        if (text === undefined) return this;

        var DOM = this.DOM,
			border = DOM.border,
			title = DOM.title[0];

        if (text === false) {
            title.style.display = 'none';
            title.innerHTML = '';
            border.addClass('ui_state_tips');
        }
        else {
            title.style.display = '';
            title.innerHTML = text;
            border.removeClass('ui_state_tips');
        };

        return this;
    },

    /*!
    * 设置内容
    * @param	{String}	内容 (如果内容前3个字符为‘url:’就加载单独页面的内容页)
    * @param   {Boolean}   是否为后增加的内容
    * @param   {Boolean}   是否使用iframe方式加载内容页
    * @return	{this}		如果无参数则返回对象本身
    */
    content: function (msg, add, frm) {
        if (msg === undefined) return this;

        var that = this, DOM = that.DOM,
            wrap = DOM.wrap[0],
			width = wrap.offsetWidth,
			height = wrap.offsetHeight,
			left = parseInt(wrap.style.left),
			top = parseInt(wrap.style.top),
			cssWidth = wrap.style.width,
			$content = DOM.content,
			loading = dialog.setting.content;            
        // 假如内容中前3个字符为'url:'就加载相对路径的单独页面的内容页        
        if(CAS.isImage(msg)){ //图片展示 kevin
            setTimeout(function(){
                $content.addClass("w100p h100p").html("<div id='imgshow' class='w100p h100p'/>");
                var url = msg.split('url:')[1];                        
                $("#imgshow").casimageview({ url: url });
                that._resize=function(){
                    setTimeout(function () {
                        $("#imgshow").casimageview({resize:true});
                    }, 0); 
                }
            },200);
        }else if (frm) {
            $content[0].innerHTML = loading;
            var url = msg.split('url:')[1];
            //url=url + (url.indexOf("?")>0?"&":"?") +Math.random();
            that._iframe(url);
        }
        else
            $content.html(msg);
        

        // 新增内容后调整位置
        if (!add) {
            width = wrap.offsetWidth - width;
            height = wrap.offsetHeight - height;
            left = left - width / 2;
            top = top - height / 2;
            wrap.style.left = Math.max(left, 0) + 'px';
            wrap.style.top = Math.max(top, 0) + 'px';

            if (cssWidth && cssWidth !== 'auto')
                wrap.style.width = wrap.offsetWidth + 'px';

            that._autoPositionType();
        }

        that._ie6SelectFix();

        return that;
    },

    /**
    *	尺寸
    *	@param	{Number, String}	宽度
    *	@param	{Number, String}	高度
    */
    size: function (width, height) {
        var that = this, DOM = that.DOM,
			wrap = DOM.wrap[0],
			style = DOM.main[0].style;

        wrap.style.width = 'auto';

        if (typeof width === 'number')
            width = width + 'px';

        if (typeof height === 'number')
            height = height + 'px';

        style.width = width;
        style.height = height;

        if (width !== 'auto')  // 防止未定义宽度的表格遇到浏览器右边边界伸缩
            wrap.style.width = wrap.offsetWidth + 'px';

        that._ie6SelectFix();        
        return that;
    },

    /**
    * 位置(相对于可视区域)
    * @param	{Number, String}
    * @param	{Number, String}
    */
    position: function (left, top) {
        var that = this,
			config = that.config,
			wrap = that.DOM.wrap[0],
			style = wrap.style,
			isFixed = _ie6 ? false : config.fixed,
			ie6Fixed = _ie6 && config.fixed,
			docLeft = _$top.scrollLeft(),
			docTop = _$top.scrollTop(),
			dl = isFixed ? 0 : docLeft,
			dt = isFixed ? 0 : docTop,
			ww = _$top.width(),
			wh = _$top.height(),
			ow = wrap.offsetWidth,
			oh = wrap.offsetHeight;

        if (left || left === 0) {
            that._left = left.toString().indexOf('%') !== -1 ? left : null;
            left = that._toNumber(left, ww - ow);

            if (typeof left === 'number') {
                left = ie6Fixed ? (left += docLeft) : left + dl;
                left = Math.max(left, dl) + 'px';
            }

            style.left = left;
        }

        if (top || top === 0) {
            that._top = top.toString().indexOf('%') !== -1 ? top : null;
            top = that._toNumber(top, wh - oh);

            if (typeof top === 'number') {
                top = ie6Fixed ? (top += docTop) : top + dt;
                top = Math.max(top, dt) + 'px';
            }

            style.top = top;
        }

        if (left !== undefined && top !== undefined)
            that._autoPositionType();

        return that;
    },

    /*!
    * 定时关闭
    * @param	{Number}	单位为秒, 无参数则停止计时器
    * @param   {Function}  关闭窗口前执行的回调函数
    */
    time: function (second, callback) {
        var that = this,
			timer = that._timer;

        timer && clearTimeout(timer);
        callback && callback.call(that);

        if (second) {
            that._timer = setTimeout(function () {
                that._click('cancel');
            }, 1000 * second);
        }

        return that;
    },

    /*! 显示对话框 */
    show: function (args) {
        this.DOM.wrap[0].style.visibility = 'visible';
        this.DOM.border.addClass('ui_state_visible');

        if (!args && this._lock)
            $('#ldg_lockmask', _doc)[0].style.display = '';

        return this;
    },

    /*! 隐藏对话框 */
    hide: function (args) {
        this.DOM.wrap[0].style.visibility = 'hidden';
        this.DOM.border.removeClass('ui_state_visible');

        if (!args && this._lock)
            $('#ldg_lockmask', _doc)[0].style.display = 'none';

        return this;
    },

    /*! 置顶对话框 */
    zindex: function () {
        var that = this, DOM = that.DOM,
		    load = that._load,
			top = dialog.focus,
			index = topwindow.dialogzIndex++;
        // 设置叠加高度
        DOM.wrap[0].style.zIndex = index;

        // 设置最高层的样式
        top && top.DOM.border.removeClass('ui_state_focus');
        dialog.focus = that;
        DOM.border.addClass('ui_state_focus');

        // 扩展窗口置顶功能，只用在iframe方式加载内容
        // 或跨域加载内容页时点窗口内容主体部分置顶窗口
        if (load && load.style.zIndex)
            load.style.display = 'none';
        if (top && top !== that && top.iframe)
            top._load.style.display = '';

        return that;
    },

    /*! 设置焦点 */
    focus: function () {
        try {
            elemFocus = this._focus && this._focus[0];// || this.DOM.close[0];
            elemFocus && elemFocus.focus();
        } catch (e) { };

        return this;
    },

    /*! 锁屏 */
    lock: function () {
        var that = this, frm,
		    index = topwindow.dialogzIndex - 1,
			config = that.config,
			mask = $('#ldg_lockmask', _doc)[0],
			style = mask ? mask.style : '',
			positionType = _ie6 ? 'absolute' : 'fixed';

        if (!mask) {
            frm = '<iframe frameborder=0 src="javascript:\'\'" style="width:100%;height:100%;position:absolute;' +
			    'top:0;left:0;z-index:-1;border:0;"></iframe>';                
            mask = _doc.createElement('div');
            mask.id = 'ldg_lockmask';
            mask.style.cssText = 'position:' + positionType + ';left:0;top:0;width:100%;height:100%;overflow:hidden;background:#eee;filter:alpha(opacity=0);opacity:0;';

            style = mask.style;
            if (_ie6) mask.innerHTML = frm;

            _doc.body.appendChild(mask);
        }

        if (positionType === 'absolute') {
            style.width = _$top.width();
            style.height = _$top.height();
            style.top = _$top.scrollTop();
            style.left = _$top.scrollLeft();

            that._setFixed(mask);
        }

        style.zIndex = index;
        style.display = '';

        that.zindex();
        that.DOM.border.addClass('ui_state_lock');

        that._lock = true;

        return that;
    },

    /*! 解除锁屏 */
    unlock: function () {
        var that = this,
		    config = that.config,
			mask = $('#ldg_lockmask', _doc)[0];
        
        if (mask && that._lock) {
            // 无限级锁屏
            if (config.parent && config.parent._lock) {
                var index = config.parent.DOM.wrap[0].style.zIndex;
                mask.style.zIndex = parseInt(index, 10) - 1;
            }
            else
                mask.style.display = 'none';

            that.DOM.border.removeClass('ui_state_lock');
        }

        that._lock = false;

        return that;
    },

    /*! 关闭对话框 */
    close: function (nocheck) {
        var that = this, DOM = that.DOM,
			wrap = DOM.wrap,
			list = dialog.list,
            //kevin 退出前检查
            fn = nocheck?"":that.config.close;
            
        that.time();
        
        // 当使用iframe方式加载内容页时的处理代码
        if (that.iframe) {
            if (typeof fn === 'function' && fn.call(that, that.iframe.contentWindow, window) === false)
                return that;
                
            // 重要！需要重置iframe地址，否则下次出现的对话框在IE6、7无法聚焦input
            // IE删除iframe后，iframe仍然会留在内存中出现上述问题，置换src是最容易解决的方法
            $(that.iframe).unbind('load', that._fmLoad).attr('src', "javascript:''").remove();            
            DOM.content.removeClass('ui_state_full');
            if (that._frmTimer) clearTimeout(that._frmTimer);
        }
        else {
            if (typeof fn === 'function' && fn.call(that, window) === false)
                return that;
        }

        that.unlock();

        if (that._maxState) {
            _$html.removeClass('ui_lock_scroll');
            DOM.res[0].style.display = 'none';
        }

        if (dialog.focus === that) dialog.focus = null;

        that._removeEvent();
        
        delete list[that.config.id];
        
        // 移除HTMLElement或重用
        if (_singleton)
            wrap.remove();
        else {
            _singleton = that;

            if (that._minState) {
                DOM.main[0].style.display = '';
                DOM.buttons[0].style.display = '';
                DOM.dialog[0].style.width = '';
            }

            DOM.wrap[0].style.cssText = 'left:0;top:0;z-index:-1';
            DOM.wrap[0].className = '';
            DOM.border.removeClass('ui_state_focus');
            DOM.title[0].innerHTML = '';
            DOM.content.html('');
            DOM.icon[0].innerHTML = '';
            DOM.buttons[0].innerHTML = '';

            that.hide(true)._setAbsolute();

            // 清空除this.DOM之外临时对象，恢复到初始状态，以便使用单例模式
            for (var i in that) {
                if (that.hasOwnProperty(i) && i !== 'DOM') delete that[i];
            };
        }

        that.closed = true;
        return that;
    },

    /*! 最大化窗口 */
    max: function () {
        var that = this, maxSize,
		    DOM = that.DOM,
			wrapStyle = DOM.wrap[0].style,
			mainStyle = DOM.main[0].style,
			rbStyle = DOM.rb[0].style,
			titleStyle = DOM.title[0].style,
			config = that.config,
		    top = _$top.scrollTop(),
		    left = _$top.scrollLeft();

        if (!that._maxState) {

            _$html.addClass('ui_lock_scroll');

            if (that._minState)
                that.min();

            // 存储最大化窗口前的状态
            that._or = {
                t: wrapStyle.top,
                l: wrapStyle.left,
                w: mainStyle.width,
                h: mainStyle.height,
                d: config.drag,
                r: config.resize,
                rc: rbStyle.cursor,
                tc: titleStyle.cursor
            };

            wrapStyle.top = top + 'px';
            wrapStyle.left = left + 'px';

            maxSize = that._maxSize();
            that.size(maxSize.w, maxSize.h)._setAbsolute();

            if (_ie6 && _doctype)
                wrapStyle.width = _$top.width() + 'px';

            config.drag = false;
            config.resize = false;
            rbStyle.cursor = 'auto';
            titleStyle.cursor = 'auto';

            DOM.max[0].style.display = 'none';
            DOM.res[0].style.display = 'inline-block';

            that._maxState = true;
        }
        else {
            _$html.removeClass('ui_lock_scroll');

            wrapStyle.top = that._or.t;
            wrapStyle.left = that._or.l;
            that.size(that._or.w, that._or.h)._autoPositionType();
            config.drag = that._or.d;
            config.resize = that._or.r;
            rbStyle.cursor = that._or.rc;
            titleStyle.cursor = that._or.tc;

            DOM.res[0].style.display = 'none';
            DOM.max[0].style.display = 'inline-block';

            delete that._or;

            that._maxState = false;
        }
        //改变大小重新布局 kevin
        if(that._resize){
            that._resize();                    
        }
        return that;
    },

    /*! 最小化窗口 */
    min: function () {
        var that = this,
		    DOM = that.DOM,
			main = DOM.main[0].style,
			buttons = DOM.buttons[0].style,
			dialog = DOM.dialog[0].style,
			rb = DOM.rb[0].style.cursor,
			resize = that.config.resize;

        if (!that._minState) {
            if (that._maxState)
                that.max();

            that._minRz = { rzs: resize, btn: buttons.display };
            main.display = 'none';
            buttons.display = 'none';
            dialog.width = main.width;
            rb.cursor = 'auto';
            resize = false;

            that._minState = true;
        }
        else {
            main.display = '';
            buttons.display = that._minRz.btn;
            dialog.width = '';
            resize = that._minRz;
            rb.cursor = that._minRz.rzs ? 'se-resize' : 'auto';

            delete that._minRz;

            that._minState = false;
        }

        that._ie6SelectFix();

        return that;
    },

    /*!
    * 获取指定id的窗口对象或窗口中iframe加载的内容页的window对象
    * @param {String} 指定的id
    * @param {String} 是否返回的为指定id的窗口对象
    *        用数字1来表示真，如果不写或写其它为false
    * @return {Object|null}
    */
    get: function (id, object) {
        if (dialog.list[id]) {
            if (object === 1)
                return dialog.list[id];
            else
                return dialog.list[id].content || null;
        }

        return null;
    },

    /**
    * 刷新或跳转指定页面
    * @param	{Object, 指定页面的window对象}
    * @param	{String, 要跳转到的页面地址}
    */
    reload: function (win, url, callback) {
        win = win || window;

        try {
            win.location.href = url ? url : win.location.href;
        }
        catch (e) { // 跨域
            url = this.iframe.src;
            $(this.iframe).attr('src', url);
        };

        callback && callback.call(this);

        return this;
    },

    /*!
    * 设置iframe方式加载内容页
    */
    _iframe: function (url) {
        var that = this, iframe, $iframe, iwin, $idoc, $ibody, iWidth, iHeight,
		    $content = that.DOM.content,
			config = that.config,
			loading = that._load = $('.ui_loading', $content[0])[0],
		    initCss = 'position:absolute;left:-9999em;z-index:1;border:none 0;background:transparent',
		    loadCss = 'width:100%;height:100%;border:none 0;';
            
        $(loading).height(that.config.height);
        // 是否允许缓存. 默认true
        if (config.cache === false) {
            var ts = (new Date).getTime(),
				ret = url.replace(/([?&])_=[^&]*/, '$1_=' + ts);
            url = ret + ((ret === url) ? (/\?/.test(url) ? '&' : '?') + '_=' + ts : '');
        }

        iframe = that.iframe = _doc.createElement('iframe');
        iframe.name = config.id;
        //iframe.setAttribute("allowtransparency",true);
        //iframe.setAttribute("hideFocus",true);
        iframe.style.cssText = initCss;
        iframe.setAttribute('frameborder', 0, 0);
        
        $iframe = $(iframe);
        $content[0].appendChild(iframe);
        var dv = _doc.createElement("div");
        dv=$(dv);
        dv.addClass("windowmask");
        dv.css({width:"100%",height:"100%",display:"none",top:0,left:0,position:"absolute",zIndex:9,filter:"alpha(opacity=0)",opacity:"0",background:"#ffffff"})
        $content.prepend(dv);

        // iframe中页面加载完成后执行的函数
        var load = that._fmLoad = function () {
            $content.addClass('ui_state_full');

            // 增强窗口置顶功能，iframe方式加载内容或跨域加载内容页时点窗口内容部分置顶窗口
            // 通过使用重置loading层来优雅的完成此功能，在focus方法中有此功能的相关代码
            var DOM = that.DOM, ltSize,
			    lt = DOM.lt[0].offsetHeight,
				main = DOM.main[0].style;
            
            // 此部分代码结束，在拖动改变大小的_dragEvent.onmove方法中还有此功能的相关代码

            try {
                iwin = that.content = iframe.contentWindow; // 定义窗口对象content属性为内容页的window对象
                $idoc = $(iwin.document);
                $ibody = $(iwin.document.body);
            } catch (e) {// 跨域
                iframe.style.cssText = loadCss;
                return;
            }
            // 获取iframe内部尺寸
            iWidth = config.width === 'auto'
			? $idoc.width() + (_ie6 ? 0 : parseInt($ibody.css('marginLeft')))
			: config.width;

            iHeight = config.height === 'auto'
			? $idoc.height() : config.height;

            // 适应iframe尺寸
            setTimeout(function () {
                iframe.style.cssText = loadCss;
            }, 0); // setTimeout: 防止IE6~7对话框样式渲染异常

            // 窗口最大化时这里不用再计算窗口的尺寸和位置了，如果再计算窗口会出现错位
            if (!that._maxState) {
                that.size(iWidth, iHeight)
			    .position(config.left, config.top);
            }
            if(typeof(iwin.Close_Check)=="function"){config.close=iwin.Close_Check;}
            // 非跨域时还要对loading层重设大小，要不宽和度都为'auto'         
            loading.style.cssText = 'display:none;width:1px;height:1px;';            
            config.init && config.init.call(that, iwin, _top);
            
        };

        that._resize=function(){
            setTimeout(function () {
                iframe.style.cssText = loadCss;
                //这里要控制重新布局 kevin
                $(iframe.contentWindow.document.body).caslayout();
            }, 0); 
        }

        // 绑定iframe元素api属性为窗口自身对象，在内容页中此属性很重要
        that.iframe.api = that;      
        // 延迟加载iframe的src属性，IE6下不延迟加载会出现加载进度条的BUG
        that._frmTimer = setTimeout(function () {
            $iframe.attr('src', url).bind('load', load);
        }, 1);  
        
    },

    /*! 获取窗口元素 */
    _getDOM: function () {
        var wrap = _doc.createElement('div'),
		    body = _doc.body;
        wrap.style.cssText = 'position:absolute;left:0;top:0;visibility:hidden;';
        wrap.innerHTML = dialogTpl;

        var name, i = 0,
			DOM = { wrap: $(wrap) },
			els = wrap.getElementsByTagName('*'),
			len = els.length;

        for (; i < len; i++) {
            name = els[i].className.split('ui_')[1];
            if (name) DOM[name] = $(els[i]);
        };

        body.insertBefore(wrap, body.firstChild);

        return DOM;
    },

    /*!
    * px与%单位转换成数值 (百分比单位按照最大值换算)
    * 其他的单位返回原值
    */
    _toNumber: function (thisValue, maxValue) {
        if (typeof thisValue === 'number')
            return thisValue;

        if (thisValue.indexOf('%') !== -1)
            thisValue = parseInt(maxValue * thisValue.split('%')[0] / 100);

        return thisValue;
    },

    /*! 计算最大化窗口时窗口的尺寸 */
    _maxSize: function () {
        var that = this, DOM = that.DOM,
		    wrap = DOM.wrap[0],
			main = DOM.main[0],
			maxWidth, maxHeight;

        maxWidth = _$top.width() - wrap.offsetWidth + main.offsetWidth;
        maxHeight = _$top.height() - wrap.offsetHeight + main.offsetHeight;

        return { w: maxWidth, h: maxHeight };
    },

    /*! 让IE6 CSS支持PNG背景 */
    _ie6PngFix: function () {
        if (_ie6) {
            var i = 0, elem, png, pngPath, runtimeStyle,
				path = dialog.setting.path + '/skins/',
				list = this.DOM.wrap[0].getElementsByTagName('*');

            for (; i < list.length; i++) {
                elem = list[i];
                png = elem.currentStyle['png'];
                if (png) {
                    pngPath = path + png;
                    runtimeStyle = elem.runtimeStyle;
                    runtimeStyle.backgroundImage = 'none';
                    runtimeStyle.filter = "progid:DXImageTransform.Microsoft." +
						"AlphaImageLoader(src='" + pngPath + "',sizingMethod='scale')";
                };
            }
        }

        return this;
    },

    /*! 强制覆盖IE6下拉控件 */
    _ie6SelectFix: _ie6 ? function () {
        var $wrap = this.DOM.wrap,
			wrap = $wrap[0],
			expando = expando + 'iframeMask',
			iframe = $wrap[expando],
			width = wrap.offsetWidth,
			height = wrap.offsetHeight;

        width = width + 'px';
        height = height + 'px';
        if (iframe) {
            iframe.style.width = width;
            iframe.style.height = height;
        } else {
            iframe = wrap.appendChild(_doc.createElement('iframe'));
            $wrap[expando] = iframe;
            iframe.src = "javascript:''";
            iframe.style.cssText = 'position:absolute;z-index:-1;left:0;top:0;'
			+ 'filter:alpha(opacity=0);width:' + width + ';height:' + height;
        }
    } : _fn,

    /*! 自动切换定位类型 */
    _autoPositionType: function () {
        this[this.config.fixed ? '_setFixed' : '_setAbsolute']();
    },

    /*! 设置静止定位 */
    _setFixed: function (el) {
        var style = el ? el.style : this.DOM.wrap[0].style;

        if (_ie6) {
            var sLeft = _$top.scrollLeft(),
				sTop = _$top.scrollTop(),
				left = parseInt(style.left) - sLeft,
				top = parseInt(style.top) - sTop,
				txt = _doctype ? 'this.ownerDocument.body' :
				    'this.ownerDocument.documentElement';

            this._setAbsolute();

            style.setExpression('left', txt + '.scrollLeft +' + left);
            style.setExpression('top', txt + '.scrollTop +' + top);
        }
        else
            style.position = 'fixed';
    },

    /*! 设置绝对定位 */
    _setAbsolute: function () {
        var style = this.DOM.wrap[0].style;

        if (_ie6) {
            style.removeExpression('left');
            style.removeExpression('top');
        }

        style.position = 'absolute';
    },

    /*! 按钮回调函数触发 */
    _click: function (name) {
        var that = this,
			fn = that._listeners[name] && that._listeners[name].callback;
        return typeof fn !== 'function' || fn.call(that, window) !== false ?
			that.close() : that;
    },

    /*! 重置位置与尺寸 */
    _reset: function () {
        var test = !!window.ActiveXObject,
		    newSize,
			that = this,
			tw = _$top.width(),
			tt = _$top.height(),
			oldSize = that._winSize || tw * tt,
			oldWidth = that._lockDocW || tw,
			left = that._left,
			top = that._top;

        if (test) {
            //IE6下遮罩大小改变
            if (that._lock && _ie6)
                $('#ldg_lockmask', _doc).css({ width: tw + 'px', height: tt + 17 + 'px' });

            newWidth = that._lockDocW = tw;
            //IE6~7 window.onresize bug
            newSize = that._winSize = tw * tt;
            if (oldSize === newSize) return;
        };

        if (that._maxState) {
            var size = that._maxSize();
            that.size(size.w, size.h);
        }

        //IE6~8会出现最大化还原后窗口重新定位，锁定滚动条在IE下就会触发resize事件BUG 
        if (test && Math.abs(oldWidth - newWidth) === 17) return;

        if (left || top)
            that.position(left, top);
    },

    _addEvent: function () {
        var resizeTimer,
			that = this,
			config = that.config,
			DOM = that.DOM;

        // 窗口调节事件
        that._winResize = function () {
            resizeTimer && clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function () {
                that._reset();
            }, 140);
        };
        _$top.bind('resize', that._winResize);

        // 监听点击
        DOM.wrap.bind('click', function (event) {
            var target = event.target, callbackID;
            
            if (target.disabled) return false; // IE BUG
            //按钮点击时触发，我们用的是A，所以要修正，kevin
            if($(target).isTag("em") && $(target).parent().hasClass("btn")){
                target = $(target).parent().get(0);
            }            
            if (target === DOM.close[0]) {
                that._click('cancel');
                return false;
            }
            else if (target === DOM.max[0] || target === DOM.res[0] || target === DOM.max_b[0]
			    || target === DOM.res_b[0] || target === DOM.res_t[0]) {
                that.max();
                return false;
            }
            else if (target === DOM.min[0] || target === DOM.min_b[0]) {
                that.min();
                return false;
            }
            else {
                callbackID = target[expando + 'callback'];
                callbackID && that._click(callbackID);
            }
        }).bind('mousedown', function (event) {
            that.zindex();

            var target = event.target;

            if (config.drag !== false && target === DOM.title[0]
			|| config.resize !== false && target === DOM.rb[0]) {
                _use(event);
                return false;
            }
        });

        // 双击标题栏最大化还窗口事件
        if (config.max)
            DOM.title.bind('dblclick', function () { that.max(); return false; });
    },

    /*!  卸载事件代理 */
    _removeEvent: function () {
        var that = this,
			DOM = that.DOM;

        DOM.wrap.unbind();
        DOM.title.unbind();
        _$top.unbind('resize', that._winResize);
    }
};

    dialog.fn._init.prototype = dialog.fn;

    /*! 此对象用来存储获得焦点的窗口对象实例 */
    dialog.focus = null;

    /*! 存储窗口实例的对象列表 */
    dialog.list = {};

    /*!
    * 框架页面卸载前关闭所有穿越的对话框
    * 同时移除拖动层和遮罩层
    */
    _top != window && $(window).bind('unload', function () {
        var list = dialog.list;
        for (var i in list) {
            if (list[i])
                list[i].close();
        }
        _singleton && _singleton.DOM.wrap.remove();

        _$doc.unbind('keydown', onKeyDown);

        $('#ldg_lockmask', _doc)[0] && $('#ldg_lockmask', _doc).remove();
        $('#ldg_dragmask', _doc)[0] && $('#ldg_dragmask', _doc).remove();
    });

    /*! dialog 的全局默认配置 */
     dialog.setting =
{
    content: '<div class="ui_loading"><span>loading...</span></div>',
    title: '\u89C6\u7A97 ',     // 标题,默认'视窗'
    button: null,      		// 自定义按钮
    ok: null, 				// 确定按钮回调函数
    cancel: null, 			// 取消按钮回调函数
    init: null, 				// 对话框初始化后执行的函数
    close: null, 			// 对话框关闭前执行的函数
    okVal: '\u786E\u5B9A', 	// 确定按钮文本,默认'确定'
    cancelVal: '\u53D6\u6D88', // 取消按钮文本,默认'取消'
    skin: '', 				// 多皮肤共存预留接口
    esc: false, 				// 是否支持Esc键关闭
    show: true, 				// 初始化后是否显示对话框
    width: 'auto', 			// 内容宽度
    height: 'auto', 			// 内容高度
    icon: null, 				// 消息图标名称
    path: _path,                // dialog路径
    lock: false, 			// 是否锁屏
    focus: true,                // 窗口是否自动获取焦点
    parent: null,               // 打开子窗口的父窗口对象，主要用于多层锁屏窗口
    padding: '0px', 	    // 内容与边界填充距离
    fixed: true, 			// 是否静止定位
    left: '50%', 			// X轴坐标
    top: '45%', 			// Y轴坐标
    max: true,                  // 是否显示最大化按钮
    min: true,                  // 是否显示最小化按钮
    zIndex: 100, 			// 对话框叠加高度值(重要：此值不能超过浏览器最大限制)
    resize: true, 			// 是否允许用户调节尺寸
    drag: true, 				// 是否允许用户拖动位置
    cache: true,                // 是否缓存窗口内容页
    data: null,                 // 传递各种数据
    extendDrag: false           // 增加dialog拖拽体验
};

    /*!
    *------------------------------------------------
    * 对话框模块-拖拽支持（可选外置模块）
    *------------------------------------------------
    */
    var _use, _isSetCapture = 'setCapture' in _root,
	_isLosecapture = 'onlosecapture' in _root;

    dialog.dragEvent =
{
    onstart: _fn,
    start: function (event) {
        var that = dialog.dragEvent;

        _$doc
		.bind('mousemove', that.move)
		.bind('mouseup', that.end);

        that._sClientX = event.clientX;
        that._sClientY = event.clientY;
        that.onstart(event.clientX, event.clientY);

        return false;
    },

    onmove: _fn,
    move: function (event) {
        var that = dialog.dragEvent;

        that.onmove(
		    event.clientX - that._sClientX,
			event.clientY - that._sClientY
		);

        return false;
    },

    onend: _fn,
    end: function (event) {
        var that = dialog.dragEvent;

        _$doc
		.unbind('mousemove', that.move)
		.unbind('mouseup', that.end);

        that.onend(event.clientX, event.clientY);
        return false;
    }
};

    _use = function (event) {
        var limit, startWidth, startHeight, startLeft, startTop, isResize,
		api = dialog.focus,
		config = api.config,
		DOM = api.DOM,
		wrap = DOM.wrap[0],
		title = DOM.title,
		main = DOM.main[0],
		_dragEvent = dialog.dragEvent,

        // 清除文本选择
	clsSelect = 'getSelection' in _top ?
	function () {
	    _top.getSelection().removeAllRanges();
	} : function () {
	    try { _doc.selection.empty(); } catch (e) { };
	};

        // 对话框准备拖动
        _dragEvent.onstart = function (x, y) {
            if (isResize) {
                startWidth = main.offsetWidth;
                startHeight = main.offsetHeight;
            }
            else {
                startLeft = wrap.offsetLeft;
                startTop = wrap.offsetTop;
            };
            
            _$doc.bind('dblclick', _dragEvent.end);

            !_ie6 && _isLosecapture
		? title.bind('losecapture', _dragEvent.end)
		: _$top.bind('blur', _dragEvent.end);

            _isSetCapture && title[0].setCapture();

            DOM.border.addClass('ui_state_drag');
            api.focus();
        };

        // 对话框拖动进行中
        _dragEvent.onmove = function (x, y) {
            if (isResize) {
                var wrapStyle = wrap.style,
				style = main.style,
				width = x + startWidth,
				height = y + startHeight;

                wrapStyle.width = 'auto';
                config.width = style.width = Math.max(0, width) + 'px';
                wrapStyle.width = wrap.offsetWidth + 'px';

                config.height = style.height = Math.max(0, height) + 'px';
                //api._ie6SelectFix();
                // 使用loading层置顶窗口时窗口大小改变相应loading层大小也得改变
                //改变大小重新布局 kevin
                if(api._resize){
                    api._resize();                    
                }
                //api._load && $(api._load).css({ width: style.width, height: style.height });
            }
            else {
                var style = wrap.style,
				left = x + startLeft,
				top = y + startTop;

                config.left = Math.max(limit.minX, Math.min(limit.maxX, left));
                config.top = Math.max(limit.minY, Math.min(limit.maxY, top));
                style.left = config.left + 'px';
                style.top = config.top + 'px';
            }
            $("div.windowmask",wrap).show();
            clsSelect();
        };

        // 对话框拖动结束
        _dragEvent.onend = function (x, y) {
            _$doc.unbind('dblclick', _dragEvent.end);
            $("div.windowmask",wrap).hide();
            !_ie6 && _isLosecapture
		? title.unbind('losecapture', _dragEvent.end)
		: _$top.unbind('blur', _dragEvent.end);

            _isSetCapture && title[0].releaseCapture();

            _ie6 && api._autoPositionType();

            DOM.border.removeClass('ui_state_drag');
        };

        isResize = event.target === DOM.rb[0] ? true : false;

        limit = (function (fixed) {
            var ow = wrap.offsetWidth,
            // 向下拖动时不能将标题栏拖出可视区域
			oh =  title.height() || 30,//wrap.offsetHeight 
			ww = _$top.width(),
			wh = _$top.height(),
			dl = fixed ? 0 : _$top.scrollLeft(),
			dt = fixed ? 0 : _$top.scrollTop();
            // 坐标最大值限制(在可视区域内)	
//            maxX = ww - ow + dl;
//            maxY = wh - oh + dt;
            maxX = ww;
            maxY = wh - oh + dt;
            return {
                minX: dl - ww,
                minY: dt,
                maxX: maxX,
                maxY: maxY
            };
        })(wrap.style.position === 'fixed');

        _dragEvent.start(event);
    };

    /*! 
    * 页面DOM加载完成执行的代码
    */
    $(function () {
        // 触发浏览器预先缓存背景图片
        setTimeout(function () {
            if (_count) return;
            dialog({ left: '-9999em', time: 9, fixed: false, lock: false, focus: false });
        }, 150);

        
    });

    /*! 使用jQ方式调用窗口 */
    $.fn.dialog = function () {
        var config = arguments;
        this.bind('click', function () { dialog.apply(this, config); return false; });        
        return this;
    };

    window.dialog  = $.dialog = dialog;    

})(this.jQuery || this.lhgcore, this);

/*!
*------------------------------------------------
* 对话框其它功能扩展模块（可选外置模块）
*------------------------------------------------
*/
; (function ($, dialog, undefined) {

    var _zIndex = function () {
        return top.dialogzIndex;
    };

    /**
    * 警告
    * @param	{String}	消息内容
    */
    dialog.alert = function (content, callback, parent,title) {
        return dialog({
            title: title || '警告',
            id: 'Alert',
            zIndex: _zIndex(),
            icon: 'alert.gif',
            fixed: true,
            lock: true,
            content: content,
            ok: true,
            resize: false,
            padding:"10px",
            close: function (here) {
                if(callback)
                return callback.call(this, here);
            },
            parent: parent || null,
            init: function () {
                var that=this;
                setTimeout(function(){
                    $(that.DOM.wrap).focus();
                },10);
            }
        });
    };

    /**
    * 确认
    * @param	{String}	消息内容
    * @param	{Function}	确定按钮回调函数
    * @param	{Function}	取消按钮回调函数
    */
    dialog.confirm = function (content, yes, no, parent) {
        return dialog({
            title: '确认',
            id: 'Confirm',
            zIndex: _zIndex(),
            icon: 'confirm.gif',
            fixed: true,
            lock: true,
            content: content,
            resize: false,
            padding:"10px",
            parent: parent || null,
            ok: function (here) {
                return yes.call(this, here);
            },
            cancel: function (here) {
                return no && no.call(this, here);
            },
            init: function () {
                var that=this;
                setTimeout(function(){
                    $(that.DOM.wrap).focus();
                },10);
            }
        });
    };

    /**
    * 提问
    * @param	{String}	提问内容
    * @param	{Function}	回调函数. 接收参数：输入值
    * @param	{String}	默认值
    */
    dialog.prompt = function (content, yes, value, parent) {
        value = value || '';
        var input;

        return dialog({
            title: '提示',
            id: 'Prompt',
            zIndex: _zIndex(),
            icon: 'alert.gif',
            fixed: true,
            padding:"10px",
            lock: true,
            parent: parent || null,
            content: [
			'<div style="margin-bottom:5px;font-size:12px">',
				content,
			'</div>',
			'<div>',
				'<input value="',
					value,
				'" style="width:18em;padding:6px 4px" />',
			'</div>'
			].join(''),
            init: function () {
                input = this.DOM.content[0].getElementsByTagName('input')[0];
                input.select();
                input.focus();
            },
            ok: function (here) {
                if($.trim(input.value).length==0){input.focus(); return false;}
                return yes && yes.call(this, $.trim(input.value), here);
            },
            cancel: true
        });
    };

    dialog.progress = function (content,parent) {
        return dialog({
            id: 'progress',
            zIndex: _zIndex(),
            title: false,
            cancel: false,
            fixed: true,
            lock: true,
            padding:"10px",
            resize: false,
            parent: parent || null,
            content: [
			'<div class="pt20 pb20 pl30 pr30"><img src="'+CAS.StaticUrl + 'images/loading.gif' +'" class="tm"> ',
				content,
			'</div>'	
			].join('')
        })
	    
    };

    /**
    * 短暂提示
    * @param	{String}	提示内容
    * @param   {Number}    显示时间 (默认1.5秒)
    * @param	{String}	提示图标 (注意要加扩展名)
    * @param   {Function}  提示关闭时执行的回调函数
    */
    dialog.tips = function (content, time, icon, callback) {
        var reIcon = icon ? function () {
            this.DOM.icon[0].innerHTML = '<img src="' + this.config.path + 'skins/icons/' + icon + '" class="ui_icon_bg"/>';
            this.DOM.icon[0].style.display = '';
            if (callback) this.config.close = callback;
        } : function () {
            this.DOM.icon[0].style.display = 'none';
            if (callback) this.config.close = callback;
        };

        return dialog({
            id: 'Tips',
            zIndex: _zIndex(),
            title: false,
            cancel: false,
            fixed: true,
            lock: false,
            resize: false
        })
	.content(content)
	.time(time || 1.5, reIcon);
    };

})(this.jQuery || this.lhgcore, this.dialog);