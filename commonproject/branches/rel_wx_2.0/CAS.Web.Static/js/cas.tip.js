//提示 kevin
; (function ($) {
    $.fn.casTip=$.casTip=function(args){
        return this.each(function(){    
            var $this=$(this);
            args=args || {};
            var defaults={
                type:"flow",//跟随式
                keep:false,//保持显示
                content:$this.attr("rel") || "内容不能为空"
            };
            args = $.extend({}, defaults, args);
            var tip = $this.data("tip") ;
            if(args.hide)
            {
                if(tip) {
                    tip.hidden();
                    
                }
                return;
            }
            if(!tip){                
                if(args.type=="flow")
                {
                    tip=$('<span class="r wordwarm dn">$content</span>');
                }
                else if(args.type=="pop")
                {
                    tip=$('<span class="bubwarm dn"><em class="bubbox content"></em><em class="bubicon"></em></span>');
                }
                tip.insertAfter($this);
                $this.data("tip",tip);
                tip.hidden=function(){
                    $this.removeClass("ipterr");
                    tip.hide();
                    $this.parent().removeClass("rel");
                }
                $this.bind("focus", function () {       
                    if(args.keep) return;
                    tip.hidden();
                }).bind("blur",function(){
                    if(args.keep) return;
                    tip.hidden()
                });
            }
            $("em.content",tip).html(args.content);
            $this.addClass("ipterr");

            function setpos(scroll,init){
                var left,top;
                tip.parent().addClass("rel");
                if(args.type=="flow")
                {
                    left=$this.position().left + $this.width() + 10;
                    top=$this.position().top + 3;
                }
                else if(args.type=="pop")
                {
                    left=$this.position().left ;
                    top=$this.position().top - 30 ;
                }                
                tip.css({
                    position: "absolute",
                    left: left,
                    top: top
                });
                if(init) tip.show();
            }
            setpos(null,true);
            //$("div").bind("scroll",function(){if(tip.is(":visible")) setpos($(this));});
            //$(window).bind("scroll resize",function(){setTimeout(function(){if(tip.is(":visible")) setpos($(this));},100);});
            return tip;
        });
    }
})(jQuery)