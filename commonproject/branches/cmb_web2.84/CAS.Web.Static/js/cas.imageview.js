//图片展示  kevin
; (function ($) {
    $.fn.casImageView=$.casImageView=function(args){
        var $this=$(this);
        //窗体改变时也改
        var resetsize=function(){
            $(".imgshow_content",$this).height($this.height() - $(".imgshow_button",$this).height());
        }
        if(args.resize){
            resetsize();            
            return;
        }
        var html = [];              
        html.push('<div class="imgshow w100p h100p"><div class="imgshow_button h30 lh30 rel">');
        html.push('<a class="btn ml5 mr5 mt5" id="btnzoomup">放大</a><a class="btn mr5 mt5" id="btnzoomdown">缩小</a>');
        html.push('<a class="btn mr5 mt5" id="btnscreensize">适合屏幕</a><a class="btn mt5" id="btnoldsize">原始大小</a> <div class="abs t0 r5">(支持鼠标滚轮缩放)</div>');
        html.push('</div><div class="imgshow_content" style="overflow:hidden;position:relative;background:url('+CAS.StaticUrl + 'images/loading.gif) no-repeat 50% 50%;border:1px solid #ccc;"><img class="abs dn move" src="" onload="$(\'.imgshow_content\').css(\'background\',\'none\');resetpos(this);" /></div></div>');
        
        //重新定位
        window.resetpos = function(o) {
            var pos = getimg(o);
            var l = (pos.w - pos.ow )/2;
            var t = (pos.h - pos.oh )/2;
            if($(o).is(":visible"))
                $(o).stop().animate({ top: t ,left: l,zoom: "100%"},100);
            else
                $(o).css({zoom:"100%", top: t ,left: l}).show();
        }
        //取大小
        function getimg(o){
            var w, h, oh, ow;
            w = $(o).parent().width();
            h = $(o).parent().height();
            oh = $(o).height();
            ow = $(o).width();
            return {w:w,h:h,oh:oh,ow:ow};
        }

        $(window).bind("resize",function(){setTimeout(resetsize,200);});
        //初始化
        var init = function(){            
            $this.html(html.join("")).initUI();
            resetsize();
            var img;
            var keep;
            //适合屏幕 计算并使用zoom ，不改变大小 。
            function screensize(){
                var pos = getimg(img[0]);
                var nh = pos.h;
                var nw = pos.ow * pos.h / pos.oh;
                var l = (pos.w - nw )/2;
                var t = 0;                
                if(nw>pos.w){
                    nw = pos.w;
                    nh = pos.oh * pos.w / pos.ow;
                    l = 0;
                    t = (pos.h - nh) /2;
                }
                var zoom = nw / pos.ow * 100;
                img.stop().animate({top: t,left: l, zoom: zoom + "%" },100);
            }
            //缩放 使用zoom
            function zoomImg(o, flag) {
                o.stop();                               
                var zoom =parseFloat(o[0].style.zoom) || 100;
                if(zoom<5) return;
                var val = (event && event.wheelDelta) ? event.wheelDelta / 10 : 5;
                var l = o.position().left;
                var t = o.position().top;
                var pos = getimg(img[0]);
                if (flag){
                    zoom += val;
                    l = l - (pos.ow * val /100)/2;
                    t = t - (pos.oh * val /100)/2;
                }
                else
                {
                    zoom -= val;
                    l = l + (pos.ow * val /100)/2;
                    t = t + (pos.oh * val /100)/2;
                }
                if (zoom > 5) {
                    o.css({left:l,top:t, zoom: zoom + "%" });
                    if (keep) { setTimeout(function(){ zoomImg(o, flag); },10); }  
                }
            }
            img = $(".imgshow_content img",$this);
            img.attr("src", args.url);
            //鼠标滚轮
            $(".imgshow_content",$this).bind("mousewheel", function () {
                zoomImg(img, true);
                return false;
            });
            //放大
            $("#btnzoomup",$this).bind("mousedown", function () {
                keep=true;
                zoomImg(img, true);
            }).bind("mouseup", function () { keep=false; });
            //缩小
            $("#btnzoomdown",$this).bind("mousedown", function () {
                keep=true;
                zoomImg(img, false);
            }).bind("mouseup", function () { keep=false; });
            //适合屏幕
            $("#btnscreensize",$this).bind("click", screensize);
            //原始大小
            $("#btnoldsize",$this).bind("click", function () {
                resetpos(img[0]);
            });
            //拖动
            CAS.Use(["jquery.ui.js"], function () {
                img.draggable();
            });
        }
        setTimeout(init,100);
    };
})(jQuery)