//自动追加内容的下拉 kevin
//如邮箱，输入字符，自动列出邮箱后缀，方便输入
; (function ($) { 
    $.fn.casAutoAppend=function(args){
        var list=args.list;
        return this.each(function(){
            var $this=$(this);
            var listdiv=$("<div/>").insertAfter($this);
            listdiv.css({position:"absolute",background:"#fff",border:"1px solid #999"})
            .css("left",$this.position().left + "px")
            .css("top",$this.position().top + $this.height() + "px")
            .width($this.width())
            .hide();
            var listhtml="";
            for (var i = 0; i < list.length; i++) {
                listhtml +="<li style='cursor:default;color:#666;white-space:nowrap;display:block;height:20px;line-height:20px;margin:0;' rel='"+list[i]+"'>" + list[i] + "</li>";
            }
            var listul=$("<ul style='margin:0;padding:0'>" + listhtml + "</ul>").appendTo(listdiv);
            $(document).bind("click", function(){listdiv.hide();});
            $("li",listul).each(function(){
                $(this)
                .bind("click",function(){
                    $this.val($(this).text());
                    listdiv.hide();
                })
                .bind("mouseenter",function(){
                    if($("li.over",listul)!=$(this))
                        $("li.over",listul).removeClass("over");
                    $(this).addClass("over");
                })            
            });
            var hasReturn=false; //是否选中了值返回
            //选择
            $this.bind("keyup",function(){
                if(hasReturn) return;
                if($(this).val()=="") {listdiv.hide();return;}
                if(!listdiv.is(":visible")) listdiv.show();
                $("li",listul).each(function(){
                    var val=$this.val();
                    var index = val.indexOf("@");
                    if(index>0) val=val.substring(0,index);
                    $(this).html("<font color='#000'>" + val + "</font>" + $(this).attr("rel"));
                });
            }).bind("keydown",function(e){ //键控制
                hasReturn=false;
                if(e.keyCode==CAS.KeyCode.HOME || e.keyCode==CAS.KeyCode.SHIFT
                || e.keyCode==CAS.KeyCode.END || e.keyCode==CAS.KeyCode.LEFT
                || e.keyCode==CAS.KeyCode.CTRL || e.keyCode==CAS.KeyCode.ALT
                || e.keyCode==CAS.KeyCode.RIGHT){ 
                    hasReturn=true;
                    return;
                }
                if(!listdiv.is(":visible")) 
                {
                    listdiv.show();
                }
                var cur = $("li.over",listul);
                if(e.keyCode==CAS.KeyCode.DOWN || e.keyCode==CAS.KeyCode.UP) //上下
                {
                    var next=null;
                    var firstorlast=null;
                    if(e.keyCode==CAS.KeyCode.DOWN)
                    {
                        next = cur.size()>0? cur.eq(0).next(): $("li:first",listul);
                    }
                    else{
                        next = cur.size()>0? cur.eq(0).prev(): $("li:last",listul);
                    }
                    next.addClass("over");
                    cur.removeClass("over");                    
                    e.stopPropagation();
                }
                else if(e.keyCode==CAS.KeyCode.ENTER || e.keyCode==CAS.KeyCode.TAB){ //回车或TAB
                    if(cur.size()>0 && $this.val()!="")
                    {
                        $this.val(cur.eq(0).text());                     
                    }
                    listdiv.hide();
                    hasReturn=true;
                    e.stopPropagation();
                }     
            });
        });
    }
})(jQuery)