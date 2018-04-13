/*!
 * artDialog 4.1.7
 * Date: 2013-03-03 08:04
 * http://code.google.com/p/artdialog/
 * (c) 2009-2012 TangBin, http://www.planeArt.cn
 *
 * This is licensed under the GNU LGPL, version 2.1 or later.
 * For details, see: http://creativecommons.org/licenses/LGPL/2.1/
 */
 
;(function (window, undefined) {
//if (window.jQuery) return jQuery;

var $ = window.art = function (selector, context) {
    	return new $.fn.init(selector, context);
	},
    readyBound = false,
    readyList = [],
    DOMContentLoaded,
	isOpacity = 'opacity' in document.documentElement.style,
	quickExpr = /^(?:[^<]*(<[\w\W]+>)[^>]*$|#([\w\-]+)$)/,
	rclass = /[\n\t]/g,
	ralpha = /alpha\([^)]*\)/i,
	ropacity = /opacity=([^)]*)/,
    rfxnum = /^([+-]=)?([\d+-.]+)(.*)$/;

if (window.$ === undefined) window.$ = $;
$.fn = $.prototype = {
	constructor: $,
	
    /**
	 * DOM 就绪
	 * @param	{Function}	回调函数
	 */
    ready: function (callback) {
        $.bindReady();

        if ($.isReady) {
            callback.call(document, $);
        } else if (readyList) {
            readyList.push(callback);
        };

        return this;
    },

    /**
	 * 判断样式类是否存�?
	 * @param	{String}	名称
	 * @return	{Boolean}
	 */
    hasClass: function (name) {		
		var className = ' ' + name + ' ';
		if ((' ' + this[0].className + ' ').replace(rclass, ' ')
		.indexOf(className) > -1) return true;

		return false;
    },

    /**
	 * 添加样式�?
	 * @param	{String}	名称
	 */
    addClass: function (name) {
        if (!this.hasClass(name)) this[0].className += ' ' + name;

        return this;
    },

    /**
	 * 移除样式�?
	 * @param	{String}	名称
	 */
    removeClass: function (name) {
        var elem = this[0];

        if (!name) {
            elem.className = '';
        } else
		if (this.hasClass(name)) {
            elem.className = elem.className.replace(name, ' ');
        };

        return this;
    },

    /**
	 * 读写样式<br />
     * css(name) 访问第一个匹配元素的样式属�?br />
     * css(properties) 把一�?�?值对"对象设置为所有匹配元素的样式属�?br />
     * css(name, value) 在所有匹配的元素中，设置一个样式属性的�?br />
	 */
    css: function (name, value) {
        var i, elem = this[0], obj = arguments[0];

        if (typeof name === 'string') {
            if (value === undefined) {
                return $.css(elem, name);
            } else {
                name === 'opacity' ?
					$.opacity.set(elem, value) :
					elem.style[name] = value;
            };
        } else {
            for (i in obj) {
				i === 'opacity' ?
					$.opacity.set(elem, obj[i]) :
					elem.style[i] = obj[i];
			};
        };

        return this;
    },
	
	/** 显示元素 */
	show: function () {
		return this.css('display', 'block');
	},
	
	/** 隐藏元素 */
	hide: function () {
		return this.css('display', 'none');
	},

    /**
	 * 获取相对文档的坐�?
	 * @return	{Object}	返回left、top的数�?
	 */
    offset: function () {
        var elem = this[0],
            box = elem.getBoundingClientRect(),
            doc = elem.ownerDocument,
            body = doc.body,
            docElem = doc.documentElement,
            clientTop = docElem.clientTop || body.clientTop || 0,
            clientLeft = docElem.clientLeft || body.clientLeft || 0,
            top = box.top + (self.pageYOffset || docElem.scrollTop) - clientTop,
            left = box.left + (self.pageXOffset || docElem.scrollLeft) - clientLeft;

        return {
            left: left,
            top: top
        };
    },
	
	/**
	 * 读写HTML - (不支持文本框)
	 * @param	{String}	内容
	 */
	html: function (content) {
		var elem = this[0];
		
		if (content === undefined) return elem.innerHTML;
		$.cleanData(elem.getElementsByTagName('*'));
		elem.innerHTML = content;
		
		return this;
	},
	
	/**
	 * 移除节点
	 */
	remove: function () {
		var elem = this[0];

		$.cleanData(elem.getElementsByTagName('*'));
		$.cleanData([elem]);
		elem.parentNode.removeChild(elem);
		
		return this;
	},

	/**
	 * 事件绑定
	 * @param	{String}	类型
	 * @param	{Function}	要绑定的函数
	 */
	bind: function (type, callback) {
		$.event.add(this[0], type, callback);
		return this;
	},

	/**
	 * 移除事件
	 * @param	{String}	类型
	 * @param	{Function}	要卸载的函数
	 */
	unbind: function(type, callback) {
		$.event.remove(this[0], type, callback);
		return this;
	}
	
};

$.fn.init = function (selector, context) {
	var match, elem;
	context = context || document;
	
	if (!selector) return this;
	
	if (selector.nodeType) {
		this[0] = selector;
		return this;
	};
	
	if (selector === 'body' && context.body) {
		this[0] = context.body;
		return this;
	};
	
	if (selector === 'head' || selector === 'html') {
		this[0] = context.getElementsByTagName(selector)[0];
		return this;
	};
		
	if (typeof selector === 'string') {
		match = quickExpr.exec(selector);

		if (match && match[2]) {
			elem = context.getElementById(match[2]);
			if (elem && elem.parentNode) this[0] = elem;
			return this;
		};
	};
	
	if (typeof selector === 'function') return $(document).ready(selector);
	
	this[0] = selector;
	
	return this;
};
$.fn.init.prototype = $.fn;

/** 空函�?*/
$.noop = function () {};

/** 检测window */
$.isWindow = function (obj) {
	return obj && typeof obj === 'object' && 'setInterval' in obj;
};

/** 数组判定 */
$.isArray = function (obj) {
    return Object.prototype.toString.call(obj) === '[object Array]';
};

/**
 * 搜索子元�?
 * 注意：只支持nodeName�?className的形式，并且只返回第一个元�?
 * @param	{String}
 */
$.fn.find = function (expr) {
	var value, elem = this[0],
		className = expr.split('.')[1];

	if (className) {
		if (document.getElementsByClassName) {
			value = elem.getElementsByClassName(className);
		} else {
			value = getElementsByClassName(className, elem);
		};
	} else {
		value = elem.getElementsByTagName(expr);
	};
	
	return $(value[0]);
};
function getElementsByClassName (className, node, tag) {
	node = node || document;
	tag = tag || '*';
	var i = 0,
		j = 0,
		classElements = [],
		els = node.getElementsByTagName(tag),
		elsLen = els.length,
		pattern = new RegExp("(^|\\s)" + className + "(\\s|$)");
		
	for (; i < elsLen; i ++) {
		if (pattern.test(els[i].className)) {
			classElements[j] = els[i];
			j ++;
		};
	};
	return classElements;
};

/**
 * 遍历
 * @param {Object}
 * @param {Function}
 */
$.each = function (obj, callback) {
    var name, i = 0,
        length = obj.length,
        isObj = length === undefined;

    if (isObj) {
        for (name in obj) {
            if (callback.call(obj[name], name, obj[name]) === false) break;
        };
    } else {
        for (var value = obj[0];
		i < length && callback.call(value, i, value) !== false;
		value = obj[++i]) {};
    };
	
	return obj;
};

/**
 * 读写缓存
 * @param		{HTMLElement}	元素
 * @param		{String}		缓存名称
 * @param		{Any}			数据
 * @return		{Any}			如果无参数data则返回缓存数�?
 */
$.data = function (elem, name, data) {
	var cache = $.cache,
		id = uuid(elem);
	
	if (name === undefined) return cache[id];
	if (!cache[id]) cache[id] = {};
	if (data !== undefined) cache[id][name] = data;
	
	return cache[id][name];
};

/**
 * 删除缓存
 * @param		{HTMLElement}	元素
 * @param		{String}		缓存名称
 */
$.removeData = function (elem, name) {
	var empty = true,
		expando = $.expando,
		cache = $.cache,
		id = uuid(elem),
		thisCache = id && cache[id];

	if (!thisCache) return;
	if (name) {
		delete thisCache[name];
		for (var n in thisCache) empty = false;
		if (empty) delete $.cache[id];
	} else {
		delete cache[id];
		if (elem.removeAttribute) {
			elem.removeAttribute(expando);
		} else {
			elem[expando] = null;
		};
	};
};

$.uuid = 0;
$.cache = {};
$.expando = '@cache' + + new Date

// 标记元素唯一身份
function uuid (elem) {
	var expando = $.expando,
		id = elem === window ? 0 : elem[expando];
	if (id === undefined) elem[expando] = id = ++ $.uuid;
	return id;
};


/**
 * 事件机制
 * @namespace
 * @requires	[$.data, $.removeData]
 */
$.event = {
	
	/**
	 * 添加事件
	 * @param		{HTMLElement}	元素
	 * @param		{String}		事件类型
	 * @param		{Function}		要添加的函数
	 */
	add: function (elem, type, callback) {
		var cache, listeners,
			that = $.event,
			data = $.data(elem, '@events') || $.data(elem, '@events', {});
		
		cache = data[type] = data[type] || {};
		listeners = cache.listeners = cache.listeners || [];
		listeners.push(callback);
		
		if (!cache.handler) {
			cache.elem = elem;
			cache.handler = that.handler(cache);
			
			elem.addEventListener
			? elem.addEventListener(type, cache.handler, false)
			: elem.attachEvent('on' + type, cache.handler);
		};
	},
	
	/**
	 * 卸载事件
	 * @param		{HTMLElement}	元素
	 * @param		{String}		事件类型
	 * @param		{Function}		要卸载的函数
	 */
	remove: function (elem, type, callback) {
		var i, cache, listeners,
			that = $.event,
			empty = true,
			data = $.data(elem, '@events');
		
		if (!data) return;
		if (!type) {
			for (i in data) that.remove(elem, i);
			return;
		};
		
		cache = data[type];
		if (!cache) return;
		
		listeners = cache.listeners;
		if (callback) {
			for (i = 0; i < listeners.length; i ++) {
				listeners[i] === callback && listeners.splice(i--, 1);
			};
		} else {
			cache.listeners = [];
		};
		
		if (cache.listeners.length === 0) {
			elem.removeEventListener
			? elem.removeEventListener(type, cache.handler, false)
			: elem.detachEvent('on' + type, cache.handler);
			
			delete data[type];
			cache = $.data(elem, '@events');
			for (var n in cache) empty = false;
			if (empty) $.removeData(elem, '@events');
		};
	},
	
	/** @inner 事件句柄 */
	handler: function (cache) {
		return function (event) {
			event = $.event.fix(event || window.event);
			for (var i = 0, list = cache.listeners, fn; fn = list[i++];) {
				if (fn.call(cache.elem, event) === false) {
					event.preventDefault();
					event.stopPropagation();
				};
			};
		};
	},
	
	/** @inner Event对象兼容处理 */
	fix: function (event) {
		if (event.target) return event;
		
		var event2 = {
			target: event.srcElement || document,
			preventDefault: function () {event.returnValue = false},
			stopPropagation: function () {event.cancelBubble = true}
		};
		// IE6/7/8 在原生window.event对象写入数据会导致内存无法回收，应当采用拷贝
		for (var i in event) event2[i] = event[i];
		return event2;
	}
	
};

/**
 * 清理元素集的事件与缓�?
 * @requires	[$.removeData, $.event]
 * @param		{HTMLCollection}	元素�?
 */
$.cleanData = function (elems) {
	var i = 0, elem,
		len = elems.length,
		removeEvent = $.event.remove,
		removeData = $.removeData;
	
	for (; i < len; i ++) {
		elem = elems[i];
		removeEvent(elem);
		removeData(elem);
	};
};

// DOM就绪事件
$.isReady = false;
$.ready = function () {
    if (!$.isReady) {
        if (!document.body) return setTimeout($.ready, 13);
        $.isReady = true;

        if (readyList) {
            var fn, i = 0;
            while ((fn = readyList[i++])) {
                fn.call(document, $);
            };
            readyList = null;
        };
    };
};
$.bindReady = function () {
    if (readyBound) return;

    readyBound = true;

    if (document.readyState === 'complete') {
        return $.ready();
    };

    if (document.addEventListener) {
        document.addEventListener('DOMContentLoaded', DOMContentLoaded, false);
        window.addEventListener('load', $.ready, false);
    } else if (document.attachEvent) {
        document.attachEvent('onreadystatechange', DOMContentLoaded);
        window.attachEvent('onload', $.ready);
        var toplevel = false;
        try {
            toplevel = window.frameElement == null;
        } catch (e) {};

        if (document.documentElement.doScroll && toplevel) {
            doScrollCheck();
        };
    };
};

if (document.addEventListener) {
    DOMContentLoaded = function () {
        document.removeEventListener('DOMContentLoaded', DOMContentLoaded, false);
        $.ready();
    };
} else if (document.attachEvent) {
    DOMContentLoaded = function () {
        if (document.readyState === 'complete') {
            document.detachEvent('onreadystatechange', DOMContentLoaded);
            $.ready();
        };
    };
};

function doScrollCheck () {
    if ($.isReady) return;

    try {
        document.documentElement.doScroll('left');
    } catch (e) {
        setTimeout(doScrollCheck, 1);
        return;
    };
    $.ready();
};

// 获取css
$.css = 'defaultView' in document && 'getComputedStyle' in document.defaultView ?
	function (elem, name) {
		return document.defaultView.getComputedStyle(elem, false)[name];
} :
	function (elem, name) {
		var ret = name === 'opacity' ? $.opacity.get(elem) : elem.currentStyle[name];
		return ret || '';
};

// 跨浏览器处理opacity
$.opacity = {
	get: function (elem) {
		return isOpacity ?
			document.defaultView.getComputedStyle(elem, false).opacity :
			ropacity.test((elem.currentStyle
				? elem.currentStyle.filter
				: elem.style.filter) || '')
				? (parseFloat(RegExp.$1) / 100) + ''
				: 1;
	},
	set: function (elem, value) {
		if (isOpacity) return elem.style.opacity = value;
		var style = elem.style;
		style.zoom = 1;

		var opacity = 'alpha(opacity=' + value * 100 + ')',
			filter = style.filter || '';

		style.filter = ralpha.test(filter) ?
			filter.replace(ralpha, opacity) :
			style.filter + ' ' + opacity;
	}
};

/**
 * 获取滚动条位�?- [不支持写入]
 * $.fn.scrollLeft, $.fn.scrollTop
 * @example		获取文档垂直滚动条：$(document).scrollTop()
 * @return		{Number}	返回滚动条位�?
 */
$.each(['Left', 'Top'], function (i, name) {
    var method = 'scroll' + name;

    $.fn[method] = function () {
        var elem = this[0], win;

		win = getWindow(elem);
		return win ?
			('pageXOffset' in win) ?
				win[i ? 'pageYOffset' : 'pageXOffset'] :
				win.document.documentElement[method] || win.document.body[method] :
			elem[method];
    };
});

function getWindow (elem) {
	return $.isWindow(elem) ?
		elem :
		elem.nodeType === 9 ?
			elem.defaultView || elem.parentWindow :
			false;
};

/**
 * 获取窗口或文档尺�?- [只支持window与document读取]
 * @example 
   获取文档宽度�?(document).width()
   获取可视范围�?(window).width()
 * @return	{Number}
 */
$.each(['Height', 'Width'], function (i, name) {
    var type = name.toLowerCase();

    $.fn[type] = function (size) {
        var elem = this[0];
        if (!elem) {
            return size == null ? null : this;
        };

		return $.isWindow(elem) ?
			elem.document.documentElement['client' + name] || elem.document.body['client' + name] :
			(elem.nodeType === 9) ?
				Math.max(
					elem.documentElement['client' + name],
					elem.body['scroll' + name], elem.documentElement['scroll' + name],
					elem.body['offset' + name], elem.documentElement['offset' + name]
				) : null;
    };

});

/**
 * 简单ajax支持
 * @example
 * $.ajax({
 * 		url: url,
 * 		success: callback,
 * 		cache: cache
 * });
 */
$.ajax = function (config) {
	var ajax = window.XMLHttpRequest ?
			new XMLHttpRequest() :
			new ActiveXObject('Microsoft.XMLHTTP'),
		url = config.url;
	
	if (config.cache === false) {
		var ts = + new Date,
			ret = url.replace(/([?&])_=[^&]*/, "$1_=" + ts );
		url = ret + ((ret === url) ? (/\?/.test(url) ? "&" : "?") + "_=" + ts : "");
	};
	
	ajax.onreadystatechange = function() {
		if (ajax.readyState === 4 && ajax.status === 200) {
			config.success && config.success(ajax.responseText);
			ajax.onreadystatechange = $.noop;
		};
	};
	ajax.open('GET', url, 1);
	ajax.send(null);
};

/** 动画引擎 - [不支持链式列队操作] */
$.fn.animate = function (prop, speed, easing, callback) {
	
	speed = speed || 400;
	if (typeof easing === 'function') callback = easing;
	easing = easing && $.easing[easing] ? easing : 'swing';
	
    var elem = this[0], overflow,
        fx, parts, start, end, unit,
		opt = {
			speed: speed,
			easing: easing,
			callback: function () {
				if (overflow != null) elem.style.overflow = '';
				callback && callback();
			}
		};
	
	opt.curAnim = {};
	$.each(prop, function (name, val) {
		opt.curAnim[name] = val;
	});
	
    $.each(prop, function (name, val) {
        fx = new $.fx(elem, opt, name);
        parts = rfxnum.exec(val);
        start = parseFloat(name === 'opacity'
			|| (elem.style && elem.style[name] != null) ?
			$.css(elem, name) :
			elem[name]);
        end = parseFloat(parts[2]);
        unit = parts[3];
		if (name === 'height' || name === 'width') {
			end = Math.max(0, end);
			overflow = [elem.style.overflow,
			elem.style.overflowX, elem.style.overflowY];
		};
		
        fx.custom(start, end, unit);
    });
	
	if (overflow != null) elem.style.overflow = 'hidden';

    return this;
};

$.timers = [];
$.fx = function (elem, options, prop) {
    this.elem = elem;
    this.options = options;
    this.prop = prop;
};

$.fx.prototype = {
    custom: function (from, to, unit) {
		var that = this;
        that.startTime = $.fx.now();
        that.start = from;
        that.end = to;
        that.unit = unit;
        that.now = that.start;
        that.state = that.pos = 0;

        function t() {
            return that.step();
        };
        t.elem = that.elem;
		t();
        $.timers.push(t);
        if (!$.timerId) $.timerId = setInterval($.fx.tick, 13);
    },
    step: function () {
        var that = this, t = $.fx.now(), done = true;
		
        if (t >= that.options.speed + that.startTime) {
            that.now = that.end;
            that.state = that.pos = 1;
            that.update();
			
			that.options.curAnim[that.prop] = true;
			for (var i in that.options.curAnim) {
				if (that.options.curAnim[i] !== true) {
					done = false;
				};
			};
			
			if (done) that.options.callback.call(that.elem);
			
            return false;
        } else {
            var n = t - that.startTime;
            that.state = n / that.options.speed;
            that.pos = $.easing[that.options.easing](that.state, n, 0, 1, that.options.speed);
            that.now = that.start + ((that.end - that.start) * that.pos);
            that.update();
            return true;
        };
    },
    update: function () {
		var that = this;
		if (that.prop === 'opacity') {
			$.opacity.set(that.elem, that.now);
		} else
		if (that.elem.style && that.elem.style[that.prop] != null) {
			that.elem.style[that.prop] = that.now + that.unit;
		} else {
			that.elem[that.prop] = that.now;
		};
    }
};

$.fx.now = function () {
    return + new Date;
};

$.easing = {
    linear: function (p, n, firstNum, diff) {
        return firstNum + diff * p;
    },
    swing: function (p, n, firstNum, diff) {
        return ((-Math.cos(p * Math.PI) / 2) + 0.5) * diff + firstNum;
    }
};

$.fx.tick = function () {
	var timers = $.timers;
    for (var i = 0; i < timers.length; i++) {
        !timers[i]() && timers.splice(i--, 1);
    };
    !timers.length && $.fx.stop();
};

$.fx.stop = function () {
    clearInterval($.timerId);
    $.timerId = null;
};

$.fn.stop = function () {
	var timers = $.timers;
    for (var i = timers.length - 1; i >= 0; i--) {
    	if (timers[i].elem === this[0]) timers.splice(i, 1);
	};
    return this;
};

//-------------end
return $}(window));




//------------------------------------------------
// 对话框模�?
//------------------------------------------------
;(function ($, window, undefined) {

$.noop = $.noop || function () {}; // jQuery 1.3.2
var _box, _thisScript, _skin, _path,
	_count = 0,
	_$window = $(window),
	_$document = $(document),
	_$html = $('html'),
	_elem = document.documentElement,
	_isIE6 = window.VBArray && !window.XMLHttpRequest,
	_isMobile = 'createTouch' in document && !('onmousemove' in _elem)
		|| /(iPhone|iPad|iPod)/i.test(navigator.userAgent),
	_expando = 'artDialog' + + new Date;

var artDialog = function (config, ok, cancel) {
	config = config || {};
	
	if (typeof config === 'string' || config.nodeType === 1) {
		config = {content: config, fixed: !_isMobile};
	};
	
	var api,
		defaults = artDialog.defaults,
		elem = config.follow = this.nodeType === 1 && this || config.follow;
		
	// 合并默认配置
	for (var i in defaults) {
		if (config[i] === undefined) config[i] = defaults[i];		
	};
	
	// 兼容v4.1.0之前的参数，未来版本将删除此
	$.each({ok:"yesFn",cancel:"noFn",close:"closeFn",init:"initFn",okVal:"yesText",cancelVal:"noText"},
	function(i,o){config[i]=config[i]!==undefined?config[i]:config[o]});
	
	// 返回跟随模式或重复定义的ID
	if (typeof elem === 'string') elem = $(elem)[0];
	config.id = elem && elem[_expando + 'follow'] || config.id || _expando + _count;
	api = artDialog.list[config.id];
	if (elem && api) return api.follow(elem).zIndex().focus();
	if (api) return api.zIndex().focus();
	
	// 目前主流移动设备对fixed支持不好
	if (_isMobile) config.fixed = false;
	
	// 按钮队列
	if (!$.isArray(config.button)) {
		config.button = config.button ? [config.button] : [];
	};
	if (ok !== undefined) config.ok = ok;
	if (cancel !== undefined) config.cancel = cancel;
	config.ok && config.button.push({
		name: config.okVal,
		callback: config.ok,
		focus: true
	});
	config.cancel && config.button.push({
		name: config.cancelVal,
		callback: config.cancel
	});
	
	// zIndex全局配置
	artDialog.defaults.zIndex = config.zIndex;
	
	_count ++;
	
	return artDialog.list[config.id] = _box ?
		_box._init(config) : new artDialog.fn._init(config);
};

artDialog.fn = artDialog.prototype = {

	version: '4.1.7',
	
	closed: true,
	
	_init: function (config) {
		var that = this, DOM,
			icon = config.icon,
			iconBg = icon && (_isIE6 ? {png: 'icons/' + icon + '.png'}
			: {backgroundImage: 'url(\'' + config.path + '/skins/icons/' + icon + '.png\')'});
		
        that.closed = false;
		that.config = config;
		that.DOM = DOM = that.DOM || that._getDOM();
		
		DOM.wrap.addClass(config.skin);
		DOM.close[config.cancel === false ? 'hide' : 'show']();
		DOM.icon[0].style.display = icon ? '' : 'none';
		DOM.iconBg.css(iconBg || {background: 'none'});
		DOM.se.css('cursor', config.resize ? 'se-resize' : 'auto');
		DOM.title.css('cursor', config.drag ? 'move' : 'auto');
		DOM.content.css('padding', config.padding);
		
		that[config.show ? 'show' : 'hide'](true)
		that.button(config.button)
		.title(config.title)
		.content(config.content, true)
		.size(config.width, config.height)
		.time(config.time);
		
		config.follow
		? that.follow(config.follow)
		: that.position(config.left, config.top);
		
		that.zIndex().focus();
		config.lock && that.lock();
		
		that._addEvent();
		that._ie6PngFix();
		_box = null;
		
		config.init && config.init.call(that, window);
		return that;
	},
	
	/**
	 * 设置内容
	 * @param	{String, HTMLElement}	内容 (可�?
	 * @return	{this, HTMLElement}		如果无参数则返回内容容器DOM对象
	 */
	content: function (msg) {
		var prev, next, parent, display,
			that = this,
			DOM = that.DOM,
			wrap = DOM.wrap[0],
			width = wrap.offsetWidth,
			height = wrap.offsetHeight,
			left = parseInt(wrap.style.left),
			top = parseInt(wrap.style.top),
			cssWidth = wrap.style.width,
			$content = DOM.content,
			content = $content[0];
		
		that._elemBack && that._elemBack();
		wrap.style.width = 'auto';
		
		if (msg === undefined) return content;
		if (typeof msg === 'string') {
			$content.html(msg);
		} else if (msg && msg.nodeType === 1) {
		
			// 让传入的元素在对话框关闭后可以返回到原来的地�?
			display = msg.style.display;
			prev = msg.previousSibling;
			next = msg.nextSibling;
			parent = msg.parentNode;
			that._elemBack = function () {
				if (prev && prev.parentNode) {
					prev.parentNode.insertBefore(msg, prev.nextSibling);
				} else if (next && next.parentNode) {
					next.parentNode.insertBefore(msg, next);
				} else if (parent) {
					parent.appendChild(msg);
				};
				msg.style.display = display;
				that._elemBack = null;
			};
			
			$content.html('');
			content.appendChild(msg);
			msg.style.display = 'block';
			
		};
		
		// 新增内容后调整位�?
		if (!arguments[1]) {
			if (that.config.follow) {
				that.follow(that.config.follow);
			} else {
				width = wrap.offsetWidth - width;
				height = wrap.offsetHeight - height;
				left = left - width / 2;
				top = top - height / 2;
				wrap.style.left = Math.max(left, 0) + 'px';
				wrap.style.top = Math.max(top, 0) + 'px';
			};
			if (cssWidth && cssWidth !== 'auto') {
				wrap.style.width = wrap.offsetWidth + 'px';
			};
			that._autoPositionType();
		};
		
		that._ie6SelectFix();
		that._runScript(content);
		
		return that;
	},
	
	/**
	 * 设置标题
	 * @param	{String, Boolean}	标题内容. 为false则隐藏标题栏
	 * @return	{this, HTMLElement}	如果无参数则返回内容器DOM对象
	 */
	title: function (text) {
		var DOM = this.DOM,
			wrap = DOM.wrap,
			title = DOM.title,
			className = 'aui_state_noTitle';
			
		if (text === undefined) return title[0];
		if (text === false) {
			title.hide().html('');
			wrap.addClass(className);
		} else {
			title.show().html(text || '');
			wrap.removeClass(className);
		};
		
		return this;
	},
	
	/**
	 * 位置(相对于可视区�?
	 * @param	{Number, String}
	 * @param	{Number, String}
	 */
	position: function (left, top) {
		var that = this,
			config = that.config,
			wrap = that.DOM.wrap[0],
			isFixed = _isIE6 ? false : config.fixed,
			ie6Fixed = _isIE6 && that.config.fixed,
			docLeft = _$document.scrollLeft(),
			docTop = _$document.scrollTop(),
			dl = isFixed ? 0 : docLeft,
			dt = isFixed ? 0 : docTop,
			ww = _$window.width(),
			wh = _$window.height(),
			ow = wrap.offsetWidth,
			oh = wrap.offsetHeight,
			style = wrap.style;
		
		if (left || left === 0) {
			that._left = left.toString().indexOf('%') !== -1 ? left : null;
			left = that._toNumber(left, ww - ow);
			
			if (typeof left === 'number') {
				left = ie6Fixed ? (left += docLeft) : left + dl;
				style.left = Math.max(left, dl) + 'px';
			} else if (typeof left === 'string') {
				style.left = left;
			};
		};
		
		if (top || top === 0) {
			that._top = top.toString().indexOf('%') !== -1 ? top : null;
			top = that._toNumber(top, wh - oh);
			
			if (typeof top === 'number') {
				top = ie6Fixed ? (top += docTop) : top + dt;
				style.top = Math.max(top, dt) + 'px';
			} else if (typeof top === 'string') {
				style.top = top;
			};
		};
		
		if (left !== undefined && top !== undefined) {
			that._follow = null;
			that._autoPositionType();
		};
		
		return that;
	},

	/**
	 *	尺寸
	 *	@param	{Number, String}	宽度
	 *	@param	{Number, String}	高度
	 */
	size: function (width, height) {
		var maxWidth, maxHeight, scaleWidth, scaleHeight,
			that = this,
			config = that.config,
			DOM = that.DOM,
			wrap = DOM.wrap,
			main = DOM.main,
			wrapStyle = wrap[0].style,
			style = main[0].style;
			
		if (width) {
			that._width = width.toString().indexOf('%') !== -1 ? width : null;
			maxWidth = _$window.width() - wrap[0].offsetWidth + main[0].offsetWidth;
			scaleWidth = that._toNumber(width, maxWidth);
			width = scaleWidth;
			
			if (typeof width === 'number') {
				wrapStyle.width = 'auto';
				style.width = Math.max(that.config.minWidth, width) + 'px';
				wrapStyle.width = wrap[0].offsetWidth + 'px'; // 防止未定义宽度的表格遇到浏览器右边边界伸�?
			} else if (typeof width === 'string') {
				style.width = width;
				width === 'auto' && wrap.css('width', 'auto');
			};
		};
		
		if (height) {
			that._height = height.toString().indexOf('%') !== -1 ? height : null;
			maxHeight = _$window.height() - wrap[0].offsetHeight + main[0].offsetHeight;
			scaleHeight = that._toNumber(height, maxHeight);
			height = scaleHeight;
			
			if (typeof height === 'number') {
				style.height = Math.max(that.config.minHeight, height) + 'px';
			} else if (typeof height === 'string') {
				style.height = height;
			};
		};
		
		that._ie6SelectFix();
		
		return that;
	},
	
	/**
	 * 跟随元素
	 * @param	{HTMLElement, String}
	 */
	follow: function (elem) {
		var $elem, that = this, config = that.config;
		
		if (typeof elem === 'string' || elem && elem.nodeType === 1) {
			$elem = $(elem);
			elem = $elem[0];
		};
		
		// 隐藏元素不可�?
		if (!elem || !elem.offsetWidth && !elem.offsetHeight) {
			return that.position(that._left, that._top);
		};
		
		var expando = _expando + 'follow',
			winWidth = _$window.width(),
			winHeight = _$window.height(),
			docLeft =  _$document.scrollLeft(),
			docTop = _$document.scrollTop(),
			offset = $elem.offset(),
			width = elem.offsetWidth,
			height = elem.offsetHeight,
			isFixed = _isIE6 ? false : config.fixed,
			left = isFixed ? offset.left - docLeft : offset.left,
			top = isFixed ? offset.top - docTop : offset.top,
			wrap = that.DOM.wrap[0],
			style = wrap.style,
			wrapWidth = wrap.offsetWidth,
			wrapHeight = wrap.offsetHeight,
			setLeft = left - (wrapWidth - width) / 2,
			setTop = top + height,
			dl = isFixed ? 0 : docLeft,
			dt = isFixed ? 0 : docTop;
		
		setLeft = setLeft < dl ? left :
		(setLeft + wrapWidth > winWidth) && (left - wrapWidth > dl)
		? left - wrapWidth + width
		: setLeft;

		setTop = (setTop + wrapHeight > winHeight + dt)
		&& (top - wrapHeight > dt)
		? top - wrapHeight
		: setTop;
		
		style.left = setLeft + 'px';
		style.top = setTop + 'px';
		
		that._follow && that._follow.removeAttribute(expando);
		that._follow = elem;
		elem[expando] = config.id;
		that._autoPositionType();
		return that;
	},
	
	/**
	 * 自定义按�?
	 * @example
		button({
			name: 'login',
			callback: function () {},
			disabled: false,
			focus: true
		}, .., ..)
	 */
	button: function () {
		var that = this,
			ags = arguments,
			DOM = that.DOM,
			buttons = DOM.buttons,
			elem = buttons[0],
			strongButton = 'aui_state_highlight',
			listeners = that._listeners = that._listeners || {},
			list = $.isArray(ags[0]) ? ags[0] : [].slice.call(ags);
		
		if (ags[0] === undefined) return elem;
		$.each(list, function (i, val) {
			var name = val.name,
				isNewButton = !listeners[name],
				button = !isNewButton ?
					listeners[name].elem :
					document.createElement('button');
					
			if (!listeners[name]) listeners[name] = {};
			if (val.callback) listeners[name].callback = val.callback;
			if (val.className) button.className = val.className;
			if (val.focus) {
				that._focus && that._focus.removeClass(strongButton);
				that._focus = $(button).addClass(strongButton);
				that.focus();
			};
			
			// Internet Explorer 的默认类型是 "button"�?
			// 而其他浏览器中（包括 W3C 规范）的默认值是 "submit"
			// @see http://www.w3school.com.cn/tags/att_button_type.asp
			button.setAttribute('type', 'button');
			
			button[_expando + 'callback'] = name;
			button.disabled = !!val.disabled;

			if (isNewButton) {
				button.innerHTML = name;
				listeners[name].elem = button;
				elem.appendChild(button);
			};
		});
		
		buttons[0].style.display = list.length ? '' : 'none';
		
		that._ie6SelectFix();
		return that;
	},
	
	/** 显示对话�?*/
	show: function () {
		this.DOM.wrap.show();
		!arguments[0] && this._lockMaskWrap && this._lockMaskWrap.show();
		return this;
	},
	
	/** 隐藏对话�?*/
	hide: function () {
		this.DOM.wrap.hide();
		!arguments[0] && this._lockMaskWrap && this._lockMaskWrap.hide();
		return this;
	},
	
	/** 关闭对话�?*/
	close: function () {
		if (this.closed) return this;
		
		var that = this,
			DOM = that.DOM,
			wrap = DOM.wrap,
			list = artDialog.list,
			fn = that.config.close,
			follow = that.config.follow;
		
		that.time();
		if (typeof fn === 'function' && fn.call(that, window) === false) {
			return that;
		};
		
		that.unlock();
		
		// 置空内容
		that._elemBack && that._elemBack();
		wrap[0].className = wrap[0].style.cssText = '';
		DOM.title.html('');
		DOM.content.html('');
		DOM.buttons.html('');
		
		if (artDialog.focus === that) artDialog.focus = null;
		if (follow) follow.removeAttribute(_expando + 'follow');
		delete list[that.config.id];
		that._removeEvent();
		that.hide(true)._setAbsolute();
		
		// 清空除this.DOM之外临时对象，恢复到初始状态，以便使用单例模式
		for (var i in that) {
			if (that.hasOwnProperty(i) && i !== 'DOM') delete that[i];
		};
		
		// 移除HTMLElement或重�?
		_box ? wrap.remove() : _box = that;
		
		return that;
	},
	
	/**
	 * 定时关闭
	 * @param	{Number}	单位为秒, 无参数则停止计时�?
	 */
	time: function (second) {
		var that = this,
			cancel = that.config.cancelVal,
			timer = that._timer;
			
		timer && clearTimeout(timer);
		
		if (second) {
			that._timer = setTimeout(function(){
				that._click(cancel);
			}, 1000 * second);
		};
		
		return that;
	},
	
	/** 设置焦点 */
	focus: function () {
		try {
			if (this.config.focus) {
				var elem = this._focus && this._focus[0] || this.DOM.close[0];
				elem && elem.focus();
			}
		} catch (e) {}; // IE对不可见元素设置焦点会报�?
		return this;
	},
	
	/** 置顶对话�?*/
	zIndex: function () {
		var that = this,
			DOM = that.DOM,
			wrap = DOM.wrap,
			top = artDialog.focus,
			index = artDialog.defaults.zIndex ++;
		
		// 设置叠加高度
		wrap.css('zIndex', index);
		that._lockMask && that._lockMask.css('zIndex', index - 1);
		
		// 设置最高层的样�?
		top && top.DOM.wrap.removeClass('aui_state_focus');
		artDialog.focus = that;
		wrap.addClass('aui_state_focus');
		
		return that;
	},
	
	/** 设置屏锁 */
	lock: function () {
		if (this._lock) return this;
		
		var that = this,
			index = artDialog.defaults.zIndex - 1,
			wrap = that.DOM.wrap,
			config = that.config,
			docWidth = _$document.width(),
			docHeight = _$document.height(),
			lockMaskWrap = that._lockMaskWrap || $(document.body.appendChild(document.createElement('div'))),
			lockMask = that._lockMask || $(lockMaskWrap[0].appendChild(document.createElement('div'))),
			domTxt = '(document).documentElement',
			sizeCss = _isMobile ? 'width:' + docWidth + 'px;height:' + docHeight
				+ 'px' : 'width:100%;height:100%',
			ie6Css = _isIE6 ?
				'position:absolute;left:expression(' + domTxt + '.scrollLeft);top:expression('
				+ domTxt + '.scrollTop);width:expression(' + domTxt
				+ '.clientWidth);height:expression(' + domTxt + '.clientHeight)'
			: '';
		
		that.zIndex();
		wrap.addClass('aui_state_lock');
		
		lockMaskWrap[0].style.cssText = sizeCss + ';position:fixed;z-index:'
			+ index + ';top:0;left:0;overflow:hidden;' + ie6Css;
		lockMask[0].style.cssText = 'height:100%;background:' + config.background
			+ ';filter:alpha(opacity=0);opacity:0';
		
		// 让IE6锁屏遮罩能够盖住下拉控件
		if (_isIE6) lockMask.html(
			'<iframe src="about:blank" style="width:100%;height:100%;position:absolute;' +
			'top:0;left:0;z-index:-1;filter:alpha(opacity=0)"></iframe>');
			
		lockMask.stop();
		lockMask.bind('click', function () {
			that._reset();
		}).bind('dblclick', function () {
			//that._click(that.config.cancelVal);
		});
		
		if (config.duration === 0) {
			lockMask.css({opacity: config.opacity});
		} else {
			lockMask.animate({opacity: config.opacity}, config.duration);
		};
		
		that._lockMaskWrap = lockMaskWrap;
		that._lockMask = lockMask;
		
		that._lock = true;
		return that;
	},
	
	/** 解开屏锁 */
	unlock: function () {
		var that = this,
			lockMaskWrap = that._lockMaskWrap,
			lockMask = that._lockMask;
		
		if (!that._lock) return that;
		var style = lockMaskWrap[0].style;
		var un = function () {
			if (_isIE6) {
				style.removeExpression('width');
				style.removeExpression('height');
				style.removeExpression('left');
				style.removeExpression('top');
			};
			style.cssText = 'display:none';
			
			_box && lockMaskWrap.remove();
		};
		
		lockMask.stop().unbind();
		that.DOM.wrap.removeClass('aui_state_lock');
		if (!that.config.duration) {// 取消动画，快速关�?
			un();
		} else {
			lockMask.animate({opacity: 0}, that.config.duration, un);
		};
		
		that._lock = false;
		return that;
	},
	
	// 获取元素
	_getDOM: function () {	
		var wrap = document.createElement('div'),
			body = document.body;
		wrap.style.cssText = 'position:absolute;left:0;top:0';
		wrap.innerHTML = artDialog._templates;
		body.insertBefore(wrap, body.firstChild);
		
		var name, i = 0,
			DOM = {wrap: $(wrap)},
			els = wrap.getElementsByTagName('*'),
			elsLen = els.length;
			
		for (; i < elsLen; i ++) {
			name = els[i].className.split('aui_')[1];
			if (name) DOM[name] = $(els[i]);
		};
		
		return DOM;
	},
	
	// px�?单位转换成数�?(百分比单位按照最大值换�?
	// 其他的单位返回原�?
	_toNumber: function (thisValue, maxValue) {
		if (!thisValue && thisValue !== 0 || typeof thisValue === 'number') {
			return thisValue;
		};
		
		var last = thisValue.length - 1;
		if (thisValue.lastIndexOf('px') === last) {
			thisValue = parseInt(thisValue);
		} else if (thisValue.lastIndexOf('%') === last) {
			thisValue = parseInt(maxValue * thisValue.split('%')[0] / 100);
		};
		
		return thisValue;
	},
	
	// 让IE6 CSS支持PNG背景
	_ie6PngFix: _isIE6 ? function () {
		var i = 0, elem, png, pngPath, runtimeStyle,
			path = artDialog.defaults.path + '/skins/',
			list = this.DOM.wrap[0].getElementsByTagName('*');
		
		for (; i < list.length; i ++) {
			elem = list[i];
			png = elem.currentStyle['png'];
			if (png) {
				pngPath = path + png;
				runtimeStyle = elem.runtimeStyle;
				runtimeStyle.backgroundImage = 'none';
				runtimeStyle.filter = "progid:DXImageTransform.Microsoft." +
					"AlphaImageLoader(src='" + pngPath + "',sizingMethod='crop')";
			};
		};
	} : $.noop,
	
	// 强制覆盖IE6下拉控件
	_ie6SelectFix: _isIE6 ? function () {
		var $wrap = this.DOM.wrap,
			wrap = $wrap[0],
			expando = _expando + 'iframeMask',
			iframe = $wrap[expando],
			width = wrap.offsetWidth,
			height = wrap.offsetHeight;

		width = width + 'px';
		height = height + 'px';
		if (iframe) {
			iframe.style.width = width;
			iframe.style.height = height;
		} else {
			iframe = wrap.appendChild(document.createElement('iframe'));
			$wrap[expando] = iframe;
			iframe.src = 'about:blank';
			iframe.style.cssText = 'position:absolute;z-index:-1;left:0;top:0;'
			+ 'filter:alpha(opacity=0);width:' + width + ';height:' + height;
		};
	} : $.noop,
	
	// 解析HTML片段中自定义类型脚本，其this指向artDialog内部
	// <script type="text/dialog">/* [code] */</script>
	_runScript: function (elem) {
		var fun, i = 0, n = 0,
			tags = elem.getElementsByTagName('script'),
			length = tags.length,
			script = [];
			
		for (; i < length; i ++) {
			if (tags[i].type === 'text/dialog') {
				script[n] = tags[i].innerHTML;
				n ++;
			};
		};
		
		if (script.length) {
			script = script.join('');
			fun = new Function(script);
			fun.call(this);
		};
	},
	
	// 自动切换定位类型
	_autoPositionType: function () {
		this[this.config.fixed ? '_setFixed' : '_setAbsolute']();/////////////
	},
	
	
	// 设置静止定位
	// IE6 Fixed @see: http://www.planeart.cn/?p=877
	_setFixed: (function () {
		_isIE6 && $(function () {
			var bg = 'backgroundAttachment';
			if (_$html.css(bg) !== 'fixed' && $('body').css(bg) !== 'fixed') {
				_$html.css({
					zoom: 1,// 避免偶尔出现body背景图片异常的情�?
					backgroundImage: 'url(about:blank)',
					backgroundAttachment: 'fixed'
				});
			};
		});
		
		return function () {
			var $elem = this.DOM.wrap,
				style = $elem[0].style;
			
			if (_isIE6) {
				var left = parseInt($elem.css('left')),
					top = parseInt($elem.css('top')),
					sLeft = _$document.scrollLeft(),
					sTop = _$document.scrollTop(),
					txt = '(document.documentElement)';
				
				this._setAbsolute();
				style.setExpression('left', 'eval(' + txt + '.scrollLeft + '
					+ (left - sLeft) + ') + "px"');
				style.setExpression('top', 'eval(' + txt + '.scrollTop + '
					+ (top - sTop) + ') + "px"');
			} else {
				style.position = 'fixed';
			};
		};
	}()),
	
	// 设置绝对定位
	_setAbsolute: function () {
		var style = this.DOM.wrap[0].style;
			
		if (_isIE6) {
			style.removeExpression('left');
			style.removeExpression('top');
		};

		style.position = 'absolute';
	},
	
	// 按钮回调函数触发
	_click: function (name) {
		var that = this,
			fn = that._listeners[name] && that._listeners[name].callback;
		return typeof fn !== 'function' || fn.call(that, window) !== false ?
			that.close() : that;
	},
	
	// 重置位置与尺�?
	_reset: function (test) {
		var newSize,
			that = this,
			oldSize = that._winSize || _$window.width() * _$window.height(),
			elem = that._follow,
			width = that._width,
			height = that._height,
			left = that._left,
			top = that._top;
		
		if (test) {
			// IE6~7 window.onresize bug
			newSize = that._winSize =  _$window.width() * _$window.height();
			if (oldSize === newSize) return;
		};
		
		if (width || height) that.size(width, height);
		
		if (elem) {
			that.follow(elem);
		} else if (left || top) {
			that.position(left, top);
		};
	},
	
	// 事件代理
	_addEvent: function () {
		var resizeTimer,
			that = this,
			config = that.config,
			isIE = 'CollectGarbage' in window,
			DOM = that.DOM;
		
		// 窗口调节事件
		that._winResize = function () {
			resizeTimer && clearTimeout(resizeTimer);
			resizeTimer = setTimeout(function () {
				that._reset(isIE);
			}, 40);
		};
		_$window.bind('resize', that._winResize);
		
		// 监听点击
		DOM.wrap
		.bind('click', function (event) {
			var target = event.target, callbackID;
			
			if (target.disabled) return false; // IE BUG
			
			if (target === DOM.close[0]) {
				that._click(config.cancelVal);
				return false;
			} else {
				callbackID = target[_expando + 'callback'];
				callbackID && that._click(callbackID);
			};
			
			that._ie6SelectFix();
		})
		.bind('mousedown', function () {
			that.zIndex();
		});
	},
	
	// 卸载事件代理
	_removeEvent: function () {
		var that = this,
			DOM = that.DOM;
		
		DOM.wrap.unbind();
		_$window.unbind('resize', that._winResize);
	}
	
};

artDialog.fn._init.prototype = artDialog.fn;
$.fn.dialog = $.fn.artDialog = function () {
	var config = arguments;
	this[this.live ? 'live' : 'bind']('click', function () {
		artDialog.apply(this, config);
		return false;
	});
	return this;
};



/** 最顶层的对话框API */
artDialog.focus = null;


/** 获取某对话框API */
artDialog.get = function (id) {
	return id === undefined
	? artDialog.list
	: artDialog.list[id];
};

artDialog.list = {};



// 全局快捷�?
_$document.bind('keydown', function (event) {
	var target = event.target,
		nodeName = target.nodeName,
		rinput = /^INPUT|TEXTAREA$/,
		api = artDialog.focus,
		keyCode = event.keyCode;

	if (!api || !api.config.esc || rinput.test(nodeName)) return;
		
	keyCode === 27 && api._click(api.config.cancelVal);
});



// 获取artDialog路径
_path = window['_artDialog_path'] || (function (script, i, me) {
	for (i in script) {
		// 如果通过第三方脚本加载器加载本文件，请保证文件名含有"artDialog"字符
		if (script[i].src && script[i].src.indexOf('artDialog') !== -1) me = script[i];
	};
	
	_thisScript = me || script[script.length - 1];
	me = _thisScript.src.replace(/\\/g, '/');
	return me.lastIndexOf('/') < 0 ? '.' : me.substring(0, me.lastIndexOf('/'));
}(document.getElementsByTagName('script')));



// 无阻塞载入CSS (�?artDialog.js?skin=aero")
_skin = _thisScript.src.split('skin=')[1];
if (_skin) {
	var link = document.createElement('link');
	link.rel = 'stylesheet';
	link.href = _path + '/skins/' + _skin + '.css?' + artDialog.fn.version;
	_thisScript.parentNode.insertBefore(link, _thisScript);
};



// 触发浏览器预先缓存背景图�?
_$window.bind('load', function () {
	setTimeout(function () {
		if (_count) return;
		artDialog({left: '-9999em',time: 9,fixed: false,lock: false,focus: false});
	}, 150);
});



// 开启IE6 CSS背景图片缓存
try {
	document.execCommand('BackgroundImageCache', false, true);
} catch (e) {};




// 使用uglifyjs压缩能够预先处理"+"号合并字符串
// uglifyjs: http://marijnhaverbeke.nl/uglifyjs
artDialog._templates =
'<div class="aui_outer">'
+	'<table class="aui_border">'
+		'<tbody>'
+			'<tr>'
+				'<td class="aui_nw"></td>'
+				'<td class="aui_n"></td>'
+				'<td class="aui_ne"></td>'
+			'</tr>'
+			'<tr>'
+				'<td class="aui_w"></td>'
+				'<td class="aui_c">'
+					'<div class="aui_inner">'
+					'<table class="aui_dialog">'
+						'<tbody>'
+							'<tr>'
+								'<td colspan="2" class="aui_header">'
+									'<div class="aui_titleBar">'
+										'<div class="aui_title"></div>'
+										'<a class="aui_close" href="javascript:/*artDialog*/;">'
+											'\xd7'
+										'</a>'
+									'</div>'
+								'</td>'
+							'</tr>'
+							'<tr>'
+								'<td class="aui_icon">'
+									'<div class="aui_iconBg"></div>'
+								'</td>'
+								'<td class="aui_main">'
+									'<div class="aui_content"></div>'
+								'</td>'
+							'</tr>'
+							'<tr>'
+								'<td colspan="2" class="aui_footer">'
+									'<div class="aui_buttons"></div>'
+								'</td>'
+							'</tr>'
+						'</tbody>'
+					'</table>'
+					'</div>'
+				'</td>'
+				'<td class="aui_e"></td>'
+			'</tr>'
+			'<tr>'
+				'<td class="aui_sw"></td>'
+				'<td class="aui_s"></td>'
+				'<td class="aui_se"></td>'
+			'</tr>'
+		'</tbody>'
+	'</table>'
+'</div>';



/**
 * 默认配置
 */
artDialog.defaults = {
								// 消息内容
	content: '<div class="aui_loading"><span>loading..</span></div>',
	title: '\u6d88\u606f',		// 标题. 默认'消息'
	button: null,				// 自定义按�?
	ok: null,					// 确定按钮回调函数
	cancel: null,				// 取消按钮回调函数
	init: null,					// 对话框初始化后执行的函数
	close: null,				// 对话框关闭前执行的函�?
	okVal: '\u786E\u5B9A',		// 确定按钮文本. 默认'确定'
	cancelVal: '\u53D6\u6D88',	// 取消按钮文本. 默认'取消'
	width: 'auto',				// 内容宽度
	height: 'auto',				// 内容高度
	minWidth: 96,				// 最小宽度限�?
	minHeight: 32,				// 最小高度限�?
	padding: '20px 25px',		// 内容与边界填充距�?
	skin: '',					// 皮肤�?预留接口,尚未实现)
	icon: null,					// 消息图标名称
	time: null,					// 自动关闭时间
	esc: true,					// 是否支持Esc键关�?
	focus: true,				// 是否支持对话框按钮自动聚�?
	show: true,					// 初始化后是否显示对话�?
	follow: null,				// 跟随某元�?即让对话框在元素附近弹出)
	path: _path,				// artDialog路径
	lock: false,				// 是否锁屏
	background: '#000',			// 遮罩颜色
	opacity: .7,				// 遮罩透明�?
	duration: 300,				// 遮罩透明度渐变动画速度
	fixed: false,				// 是否静止定位
	left: '50%',				// X轴坐�?
	top: '38.2%',				// Y轴坐�?
	zIndex: 1987,				// 对话框叠加高度�?重要：此值不能超过浏览器最大限�?
	resize: true,				// 是否允许用户调节尺寸
	drag: true					// 是否允许用户拖动位置
	
};

window.artDialog = $.dialog = $.artDialog = artDialog;
}(this.art || this.jQuery && (this.art = jQuery), this));






//------------------------------------------------
// 对话框模�?拖拽支持（可选外置模块）
//------------------------------------------------
;(function ($) {

var _dragEvent, _use,
	_$window = $(window),
	_$document = $(document),
	_elem = document.documentElement,
	_isIE6 = !('minWidth' in _elem.style),
	_isLosecapture = 'onlosecapture' in _elem,
	_isSetCapture = 'setCapture' in _elem;

// 拖拽事件
artDialog.dragEvent = function () {
	var that = this,
		proxy = function (name) {
			var fn = that[name];
			that[name] = function () {
				return fn.apply(that, arguments);
			};
		};
		
	proxy('start');
	proxy('move');
	proxy('end');
};

artDialog.dragEvent.prototype = {

	// 开始拖�?
	onstart: $.noop,
	start: function (event) {
		_$document
		.bind('mousemove', this.move)
		.bind('mouseup', this.end);
			
		this._sClientX = event.clientX;
		this._sClientY = event.clientY;
		this.onstart(event.clientX, event.clientY);

		return false;
	},
	
	// 正在拖拽
	onmove: $.noop,
	move: function (event) {		
		this._mClientX = event.clientX;
		this._mClientY = event.clientY;
		this.onmove(
			event.clientX - this._sClientX,
			event.clientY - this._sClientY
		);
		
		return false;
	},
	
	// 结束拖拽
	onend: $.noop,
	end: function (event) {
		_$document
		.unbind('mousemove', this.move)
		.unbind('mouseup', this.end);
		
		this.onend(event.clientX, event.clientY);
		return false;
	}
	
};

_use = function (event) {
	var limit, startWidth, startHeight, startLeft, startTop, isResize,
		api = artDialog.focus,
		//config = api.config,
		DOM = api.DOM,
		wrap = DOM.wrap,
		title = DOM.title,
		main = DOM.main;

	// 清除文本选择
	var clsSelect = 'getSelection' in window ? function () {
		window.getSelection().removeAllRanges();
	} : function () {
		try {
			document.selection.empty();
		} catch (e) {};
	};
	
	// 对话框准备拖�?
	_dragEvent.onstart = function (x, y) {
		if (isResize) {
			startWidth = main[0].offsetWidth;
			startHeight = main[0].offsetHeight;
		} else {
			startLeft = wrap[0].offsetLeft;
			startTop = wrap[0].offsetTop;
		};
		
		_$document.bind('dblclick', _dragEvent.end);
		!_isIE6 && _isLosecapture ?
			title.bind('losecapture', _dragEvent.end) :
			_$window.bind('blur', _dragEvent.end);
		_isSetCapture && title[0].setCapture();
		
		wrap.addClass('aui_state_drag');
		api.focus();
	};
	
	// 对话框拖动进行中
	_dragEvent.onmove = function (x, y) {
		if (isResize) {
			var wrapStyle = wrap[0].style,
				style = main[0].style,
				width = x + startWidth,
				height = y + startHeight;
			
			wrapStyle.width = 'auto';
			style.width = Math.max(0, width) + 'px';
			wrapStyle.width = wrap[0].offsetWidth + 'px';
			
			style.height = Math.max(0, height) + 'px';
			
		} else {
			var style = wrap[0].style,
				left = Math.max(limit.minX, Math.min(limit.maxX, x + startLeft)),
				top = Math.max(limit.minY, Math.min(limit.maxY, y + startTop));

			style.left = left  + 'px';
			style.top = top + 'px';
		};
			
		clsSelect();
		api._ie6SelectFix();
	};
	
	// 对话框拖动结�?
	_dragEvent.onend = function (x, y) {
		_$document.unbind('dblclick', _dragEvent.end);
		!_isIE6 && _isLosecapture ?
			title.unbind('losecapture', _dragEvent.end) :
			_$window.unbind('blur', _dragEvent.end);
		_isSetCapture && title[0].releaseCapture();
		
		_isIE6 && !api.closed && api._autoPositionType();
		
		wrap.removeClass('aui_state_drag');
	};
	
	isResize = event.target === DOM.se[0] ? true : false;
	limit = (function () {
		var maxX, maxY,
			wrap = api.DOM.wrap[0],
			fixed = wrap.style.position === 'fixed',
			ow = wrap.offsetWidth,
			oh = wrap.offsetHeight,
			ww = _$window.width(),
			wh = _$window.height(),
			dl = fixed ? 0 : _$document.scrollLeft(),
			dt = fixed ? 0 : _$document.scrollTop(),
			
		// 坐标最大值限�?
		maxX = ww - ow + dl;
		maxY = wh - oh + dt;
		
		return {
			minX: dl,
			minY: dt,
			maxX: maxX,
			maxY: maxY
		};
	})();
	
	_dragEvent.start(event);
};

// 代理 mousedown 事件触发对话框拖�?
_$document.bind('mousedown', function (event) {
	var api = artDialog.focus;
	if (!api) return;

	var target = event.target,
		config = api.config,
		DOM = api.DOM;
	
	if (config.drag !== false && target === DOM.title[0]
	|| config.resize !== false && target === DOM.se[0]) {
		_dragEvent = _dragEvent || new artDialog.dragEvent();
		_use(event);
		return false;// 防止firefox与chrome滚屏
	};
});

})(this.art || this.jQuery && (this.art = jQuery));

