/*!
 * jQuery Cookie Plugin v1.4.1
 * https://github.com/carhartl/jquery-cookie
 *
 * Copyright 2013 Klaus Hartl
 * Released under the MIT license
 */

/*
ʹ�÷���:
1.�����һ���Ự cookie��
$.cookie('the_cookie', 'the_value');
ע����û��ָ�� cookie��Чʱ��ʱ����������cookie��Ч��Ĭ�ϵ��û��ر������Ϊֹ�����Ա���Ϊ���Ựcookie��session cookie������

2.����һ��cookie��������Чʱ��Ϊ 7��:
$.cookie('the_cookie', 'the_value', { expires: 7 });
ע����ָ����cookie��Чʱ��ʱ����������cookie����Ϊ���־� cookie ��persistent  cookie������ 

3.����һ��cookie������ cookie����Ч·����
$.cookie('the_cookie', 'the_value', { expires: 7, path: '/' });
ע����Ĭ������£�ֻ������ cookie����ҳ���ܶ�ȡ�� cookie���������һ��ҳ���ȡ��һ��ҳ�����õ�cookie����������cookie��·����cookie��·�����������ܹ���ȡ cookie�Ķ���Ŀ¼�������·������Ϊ��վ�ĸ�Ŀ¼��������������ҳ���ܻ����ȡ cookie ��һ�㲻Ҫ�������ã���ֹ���ֳ�ͻ�� �� 

4.��ȡcookie��
$.cookie('the_cookie'); // cookie���� => 'the_value'
$.cookie('not_existing'); // cookie������ => null

��ȡcookies���еĲ���ֵ:
$.cookie(); // => { "the_cookie": "the_value", "...remaining": "cookies" }

5.ɾ��cookie��ͨ������null��Ϊcookie��ֵ���ɣ�
$.cookie('the_cookie', null);
//��cookie������ʱ����true, ��cookieδ������ʱ����false... 
$.removeCookie('the_cookie');

----------��ز����Ľ���---------------
1).expires: 365
����cookie����Чʱ�䣬ֵ������һ�����֣��Ӵ���cookieʱ��������Ϊ��λ����һ��Date �������ʡ�ԣ���ô������cookie�ǻỰcookie�������û��˳������ʱ��ɾ����

2).path: '/'
Ĭ�������ֻ������cookie����ҳ���ܶ�ȡ��cookie��
����cookie����Ч·����Ĭ������£��ò�����ֵΪ���� cookie ����ҳ����·������׼���������Ϊ�� ��
���������������վ�з������cookie��Ҫ����������Ч·����path: '/'���������ɾ��һ����������Ч·���� cookie������Ҫ�ڵ��ú���ʱ�������·��:$.cookie('the_cookie', null,{ path: '/' });�� domain: 'example.com'

Ĭ��ֵ������ cookie����ҳ��ӵ�е�������
3).secure: true
Ĭ��ֵ��false�����Ϊtrue��cookie�Ĵ�����Ҫʹ�ð�ȫЭ�飨HTTPS����

4).raw: true
Ĭ��ֵ��false��
Ĭ������£���ȡ��д�� cookie ��ʱ���Զ����б���ͽ��루ʹ��encodeURIComponent ���룬
decodeURIComponent ���룩��Ҫ�ر������������ raw: true ���ɡ�
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
