/*
* jqDnR - Minimalistic Drag'n'Resize for jQuery.
*
* Copyright (c) 2007 Brice Burgess <bhb@iceburg.net>, http://www.iceburg.net/
* Licensed under the MIT License:
* http://www.opensource.org/licenses/mit-license.php
*
* $Version: 2007.08.19 +r2
* 
* last modified by Cross @2012.02.08
* email:mag_lee@126.com
* add features:
* 1. limit the child div in parent div 
*/

(function ($) {
    $.fn.jqDrag = function (handler) {
        return i(this, handler, 'd');
    };

    $.fn.jqDragIn = function (handler) {
        return i(this, handler, 'dd');
    };

    $.fn.jqResize = function (handler) {
        return i(this, handler, 'r');
    };

    $.fn.jqResizeIn = function (handler) {
        return i(this, handler, 'rr');
    };

    $.fn.jqResizeWidthIn = function (handler) {
        return i(this, handler, 'widthrr');
    };
    $.fn.jqResizeHeightIn = function (handler) {
        return i(this, handler, 'heightrr');
    };

    $.jqDnR = {
        dnr: {},
        e: 0,
        drag: function (v) {

            if (M.k.indexOf("dd") >= 0 || M.k.indexOf("rr") >= 0) {
                // 获取父容器的绝对坐标
                var parentX = parseInt(E.parent().offset().left);
                var parentY = parseInt(E.parent().offset().top);
                // 获取父容器的宽度
                var parentW = parseInt(E.parent().css("width").split("px")[0]);
                var parentH = parseInt(E.parent().css("height").split("px")[0]);

                // 获取子容器的相对坐标
                var childX = parseInt(E.position().left);
                var childY = parseInt(E.position().top);
                // 获取子容器的宽度
                var childW = parseInt(E.css("width").split("px")[0]);
                var childH = parseInt(E.css("height").split("px")[0]);
            }
            if (M.k == 'dd') {
                // 限制子容器只能在父容器中拖动
                E.css({
                    left: (M.X + v.pageX - M.pX) < 0 ? 0 : (M.X + v.pageX - M.pX) < parentW - M.W ? (M.X + v.pageX - M.pX) : parentW - M.W,
                    top: (M.Y + v.pageY - M.pY) < 0 ? 0 : (M.Y + v.pageY - M.pY) < parentH - M.H ? (M.Y + v.pageY - M.pY) : parentH - M.H
                });
            }
            else if (M.k == "d") {
                E.css({
                    left: (M.X + v.pageX - M.pX),
                    top: (M.Y + v.pageY - M.pY)
                });
            }
            else if (M.k.indexOf("rr") >= 0) {
                // 限制子容器只能在父容器中缩放
                if (M.k.indexOf("width") >= 0 || M.k.indexOf("height") < 0) {
                    E.css({
                        width: (M.X + v.pageX - M.pX) < parentW - M.W ? Math.max(v.pageX - M.pX + M.W, 15) : parentW - childX                       
                    });
                }
                if (M.k.indexOf("height") >= 0 || M.k.indexOf("width") < 0) {
                    E.css({
                        height: (M.Y + v.pageY - M.pY) < parentH - M.H ? Math.max(v.pageY - M.pY + M.H, 15) : parentH - childY
                    });
                }
                return false;
            }
            else if (M.k == "r") {
                // 限制子容器只能在父容器中缩放
                E.css({
                    width: Math.max(v.pageX - M.pX + M.W, 15),
                    height: Math.max(v.pageY - M.pY + M.H, 15)
                });
                return false;
            }
        },
        stop: function () {
            E.css('opacity', M.o);
            $(document).unbind('mousemove', J.drag).unbind('mouseup', J.stop);
        }
    };

    var J = $.jqDnR,
		M = J.dnr, //{}
		E = J.e, //0
		i = function (e, handler, k) {
		    return e.each(function () {
		        handler = (handler) ? $(handler, e) : e;
		        handler.bind('mousedown', { e: e, k: k }, function (v) {
		            if (k == "dd") {
		                $(".Selected", handler.parent()).removeClass("Selected");
		                handler.addClass("Selected");
		            }
		            var d = v.data,
						p = {};
		            E = d.e;

		            // attempt utilization of dimensions plugin to fix IE issues
		            if (E.css('position') != 'relative') {
		                p = E.position();
		                if (!($.browser.msie && ($.browser.version == "6.0")) && (E.css('position') == 'fixed')) {
		                    p.top -= $(window).scrollTop();
		                    p.left -= $(window).scrollLeft();
		                }
		            }

		            M = {
		                X: p.left || f('left') || 0,
		                Y: p.top || f('top') || 0,
		                W: f('width') || E[0].scrollWidth || 0,
		                H: f('height') || E[0].scrollHeight || 0,
		                pX: v.pageX,
		                pY: v.pageY,
		                k: d.k,
		                o: E.css('opacity')
		            };

		            E.css({ opacity: 0.8 }); $(document).mousemove($.jqDnR.drag).mouseup($.jqDnR.stop);
		            return false;
		        });
		    });
		},

		f = function (k) {
		    return parseInt(E.css(k)) || false;
		};
})(jQuery); 
