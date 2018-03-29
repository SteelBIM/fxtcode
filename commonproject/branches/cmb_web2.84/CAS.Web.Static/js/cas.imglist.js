//图片列表 kevin
function fixsize(obj, width, height) {
    if ($(obj).width() > width)
        $(obj).width(width);
    else if ($(obj).height() > height)
        $(obj).height(height);
}
function showimg(url) {
    window.open(unescape(url));
}
; (function ($) {
    $.fn.casImgList=$.casImgList=function(args){
        args=args || {};
        var defaults={
            imgwidth:120,
            imgheight:120,
            op:9, //每页显示数
            mode:"data", //或api
            data:{}
        };
        
        args = $.extend({}, defaults, args);        
        return this.each(function(){            
            var $this=$(this);
            var html="<ul class='cas_imglist'>";
            var curpage=1;
            var list=null;
            if(args.mode=="data"){
                list = args.data.data;
            }
            function loadlist()
            {
                var li="";
                for (var i = (curpage-1)*args.op; i < curpage * args.op; i++) {
                    if(i>=args.data.total) break;
                    li+='<li style="margin:4px;border:1px solid #ccc; padding:2px;float:left;width:' + args.imgwidth + 'px;height:' + (args.imgheight + 20) + 'px;">';
                    li+='<div style="width:100%;text-align:center;height:' + args.imgheight + 'px;"><img style="border:1px solid #ccc;" onload="fixsize(this,'+args.imgwidth+','+args.imgheight+')" onclick="showimg(\'' + escape(list[i].big) + '\')" src="' + list[i].thum + '" title="'+list[i].title+'"/></div>';
                    li+='<div style="width:100%;text-align:center;height:20px;line-height:20px;">'+list[i].title+'</div></li>';
                }                
                return li;
            }
            html+=loadlist();            
            html+="</ul><div style='clear:both'></div>"            
            $this.html(html);   
            
            var pagecount = Math.ceil(args.data.total / args.op);
            if(!$.fn.casPager){
                CAS.Use(["cas.pager.js"],function(){
                    $this.casPager({op:args.op,page:curpage,total:args.data.total,pagecount:pagecount,callback:loadpage});// args.op,curpage,args.data.total,pagecount,loadpage);
                });                
            }else loadpage(curpage);

            //加载页数据
            function loadpage(page){
                $this.casPager({op:args.op,page:page,total:args.data.total,pagecount:pagecount,callback:loadpage});
                curpage = page;
                var html="";
                html+=loadlist();
                $("ul.cas_imglist").html(html);
            }

        });

    }
    
})(jQuery)