//简易tab kevin
; (function ($) {
    $.fn.casSimpleTab=$.casSimpleTab=function(args){
        var $this=$(this);
        var defaults={};
        args=$.extend({},defaults,args);
        var showclass = "on";
        var cur=null;//$("#" + $("li.on",$this).attr("rel"));
        return $("li",$this).each(function(){
            var rel=$(this).attr("rel");
            var href = $(this).attr("href");
            if($(this).hasClass(showclass)){
                var o=$("#"+rel);
                if(!o[0]) appendO(rel,href);
            }
            $(this).bind("click",function(){
                if($(this).hasClass(showclass)){return;}                
                var o=$("#"+rel);
                $("." + showclass).removeClass(showclass);
                $(this).addClass(showclass);
                if(o[0]){
                    if(cur) cur.hide();
                    o.show();                        
                }
                else{
                    o = appendO(rel,href);
                }
                cur=o;
                if(href && !o.data("loaded")){
                    o.attr("src",href).data("loaded",1);
                }
            });
        });
        function appendO(rel,href){
            var index=href.indexOf("api:");
            if (index >= 0)
                href = CAS.APIPage({api:href.substring(index + 4)});
            else{
                if(href.indexOf("http://")<0)
                    href = CAS.RootUrl + href;
            }
            if($("#webcontent")[0]){
                var o = $('<iframe id="' + rel + '" width="100%" height="100%" scrolling="no" src="' + href +'" frameborder="0"></iframe>');
                o.appendTo($("#webcontent")).siblings().hide();
                o.data("loaded",1);
                return o;
            }
        }
    }
})(jQuery)