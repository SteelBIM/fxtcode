//按钮 kevin
; (function ($) { 
    $.fn.casButton = $.casButton=function(args){
        args = args || {};
        return this.each(function(){
            var $this=$(this);
            if($this.isTag("a")) //button标签
            {
                var txt=$this.html();
                if(args.text) txt = args.text;
                $this.html("<em><span></span>" + txt + "</em>");
                if(args.text) return;
            }
            if($this.attr("disabled"))
                $this.addClass("noclick");   
                         
            if($this.hasClass("noclick"))
            {
                $this.casdisable();
            }
            else if($this.hasClass("loadbtn"))
            {
                $this.casdisable();
            }
            else if($this.hasClass("close"))
            {
                $this.hoverClass("closeon")
                .bind("click",function(){
                    $("#" + $this.attr("rel")).hide();
                });
            }
            
            $this.hoverClass("btnhover")
            .bind("mousedown",function(){
                if(!$this.hasClass("noclick"))
                    $this.addClass("btnactive");
            })
            .bind("mouseup mouseleave",function(){
                $this.removeClass("btnactive");
            })
            .bind("focus",function(){
                if(!$this.hasClass("noclick"))
                    $this.addClass("btnhover");
            })
            .bind("blur",function(){
                $this.removeClass("btnhover");
            })
            
            return $this;
        });
    }
})(jQuery)