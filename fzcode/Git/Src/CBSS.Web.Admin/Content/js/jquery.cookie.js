/*!
 * jQuery Cookie Plugin v1.4.1
 * https://github.com/carhartl/jquery-cookie
 *
 * Copyright 2013 Klaus Hartl
 * Released under the MIT license
 */

/*
使用方法:
1.新添加一个会话 cookie：
$.cookie('the_cookie', 'the_value');
注：当没有指明 cookie有效时间时，所创建的cookie有效期默认到用户关闭浏览器为止，所以被称为“会话cookie（session cookie）”。

2.创建一个cookie并设置有效时间为 7天:
$.cookie('the_cookie', 'the_value', { expires: 7 });
注：当指明了cookie有效时间时，所创建的cookie被称为“持久 cookie （persistent  cookie）”。 

3.创建一个cookie并设置 cookie的有效路径：
$.cookie('the_cookie', 'the_value', { expires: 7, path: '/' });
注：在默认情况下，只有设置 cookie的网页才能读取该 cookie。如果想让一个页面读取另一个页面设置的cookie，必须设置cookie的路径。cookie的路径用于设置能够读取 cookie的顶级目录。将这个路径设置为网站的根目录，可以让所有网页都能互相读取 cookie （一般不要这样设置，防止出现冲突） 。 

4.读取cookie：
$.cookie('the_cookie'); // cookie存在 => 'the_value'
$.cookie('not_existing'); // cookie不存在 => null

读取cookies所有的参数值:
$.cookie(); // => { "the_cookie": "the_value", "...remaining": "cookies" }

5.删除cookie，通过传递null作为cookie的值即可：
$.cookie('the_cookie', null);
//当cookie被发现时返回true, 当cookie未被发现时返回false... 
$.removeCookie('the_cookie');

----------相关参数的解释---------------
1).expires: 365
定义cookie的有效时间，值可以是一个数字（从创建cookie时算起，以天为单位）或一个Date 对象。如果省略，那么创建的cookie是会话cookie，将在用户退出浏览器时被删除。

2).path: '/'
默认情况：只有设置cookie的网页才能读取该cookie。
定义cookie的有效路径。默认情况下，该参数的值为创建 cookie 的网页所在路径（标准浏览器的行为） 。
如果你想在整个网站中访问这个cookie需要这样设置有效路径：path: '/'。如果你想删除一个定义了有效路径的 cookie，你需要在调用函数时包含这个路径:$.cookie('the_cookie', null,{ path: '/' });。 domain: 'example.com'

默认值：创建 cookie的网页所拥有的域名。
3).secure: true
默认值：false。如果为true，cookie的传输需要使用安全协议（HTTPS）。

4).raw: true
默认值：false。
默认情况下，读取和写入 cookie 的时候自动进行编码和解码（使用encodeURIComponent 编码，
decodeURIComponent 解码）。要关闭这个功能设置 raw: true 即可。
*/

// Same path as when the cookie was written... 
(function (factory) {
	if (typeof define === 'function' && define.amd) {
		// AMD
		define(['jquery'], factory);
	} else if (typeof exports === 'object') {
		// CommonJS
		factory(require('jquery'));
	} else {
		// Browser globals
		factory(jQuery);
	}
}(function ($) {
    var pluses = /\+/g;

	function encode(s) {
		return config.raw ? s : encodeURIComponent(s);
	}

	function decode(s) {
		return config.raw ? s : decodeURIComponent(s);
	}

	function stringifyCookieValue(value) {
		return encode(config.json ? JSON.stringify(value) : String(value));
	}

	function parseCookieValue(s) {
		if (s.indexOf('"') === 0) {
			// This is a quoted cookie as according to RFC2068, unescape...
			s = s.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, '\\');
		}

		try {
			// Replace server-side written pluses with spaces.
			// If we can't decode the cookie, ignore it, it's unusable.
			// If we can't parse the cookie, ignore it, it's unusable.
			s = decodeURIComponent(s.replace(pluses, ' '));
			return config.json ? JSON.parse(s) : s;
		} catch(e) {}
	}

	function read(s, converter) {
		var value = config.raw ? s : parseCookieValue(s);
		return $.isFunction(converter) ? converter(value) : value;
	}

	var config = $.cookie = function (key, value, options) {
		// Write
		if (value !== undefined && !$.isFunction(value)) {
			options = $.extend({}, config.defaults, options);

			if (typeof options.expires === 'number') {
				var days = options.expires, t = options.expires = new Date();
				t.setTime(+t + days * 864e+5);
			}

			return (document.cookie = [
				encode(key), '=', stringifyCookieValue(value),
				options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
				options.path    ? '; path=' + options.path : '',
				options.domain  ? '; domain=' + options.domain : '',
				options.secure  ? '; secure' : ''
			].join(''));
		}

		// Read
		var result = key ? undefined : {};

		// To prevent the for loop in the first place assign an empty array
		// in case there are no cookies at all. Also prevents odd result when
		// calling $.cookie().
		var cookies = document.cookie ? document.cookie.split('; ') : [];

		for (var i = 0, l = cookies.length; i < l; i++) {
			var parts = cookies[i].split('=');
			var name = decode(parts.shift());
			var cookie = parts.join('=');

			if (key && key === name) {
				// If second argument (value) is a function it's a converter...
				result = read(cookie, value);
				break;
			}

			// Prevent storing a cookie that we couldn't decode.
			if (!key && (cookie = read(cookie)) !== undefined) {
				result[name] = cookie;
			}
		}

		return result;
	};

	config.defaults = {};

	$.removeCookie = function (key, options) {
		if ($.cookie(key) === undefined) {
			return false;
		}

		// Must not alter options, thus extending a fresh object...
		$.cookie(key, '', $.extend({}, options, { expires: -1 }));
		return !$.cookie(key);
	};

}));
