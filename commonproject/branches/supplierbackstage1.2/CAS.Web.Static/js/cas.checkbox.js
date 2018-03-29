//选择框，包括单选和复选框 kevin
//可以更改disable状态 
; (function ($) {
    $.fn.casCheckCss={
        disabled:"disabled",
        checked:"checked",
        over:"over"
    };
    //取值
    $.fn.casCheckValue = $.casCheckValue =function(){
        var val=[];
        var chks = $(this).find("input[type=checkbox],input[type=radio]").not("[ctl=all]");
        chks.each(function(i,item){
            item=$(item);
            if(item.attr("checked"))
                val.push(item.attr("value"));
        });
        return val.join(",");
    }
    //设定选中
    $.fn.casChecked = $.casChecked = function(v){
        if(typeof(v)=="string") v=[v];
        var val=[];
        var chks = $(this).find("input[type=checkbox],input[type=radio]").not("[ctl=all]");        
        chks.each(function(i,item){
            item=$(item);
            var type = item.attr("type").toLowerCase();
            var event = type=="checkbox"?"change":"click";   
            //value or text          
            if(v==item.attr("value") || v==item.next().text())
            {                
                item.attr("checked","checked").trigger(event);
                if(type=="radio") return false;
            }
            else
                item.removeAttr("checked").trigger(event);
        });
        return true;
    }
    //禁用/启用
    $.fn.casCheckDisable = $.casCheckDisable=function(disabled){
        var cssName= $.fn.casCheckCss;
        var chks = $(this).find("*");
        //设置是否可用        
        if(disabled){
            chks.filter(".checkbox" + cssName.checked).addClass("checkboxchecked" + cssName.disabled);
            chks.filter(".checkbox").not(".checkbox" + cssName.checked).addClass("checkbox" + cssName.disabled);
            chks.filter(".radio" + cssName.checked).addClass("radiochecked" + cssName.disabled);
            chks.filter(".radio").not(".radio" + cssName.checked).addClass("radio" + cssName.disabled);                
            $(this).addClass("checkdisabled");
        }
        else{
            chks.filter(".checkboxchecked" + cssName.disabled).removeClass("checkboxchecked" + cssName.disabled);
            chks.filter(".checkbox" + cssName.disabled).removeClass("checkbox" + cssName.disabled);
            chks.filter(".radiochecked" + cssName.disabled).removeClass("radiochecked" + cssName.disabled);
            chks.filter(".radio" + cssName.disabled).removeClass("radio" + cssName.disabled);
            $(this).removeClass("checkdisabled");
        }        
    }
    $.fn.casCheckBox =$.casCheckBox=function(args){
            var $this=$(this);
            var cssName= $.fn.casCheckCss;
            var defaults={
                type:"checkbox", //默认为checkbox
                debug:false //显示隐藏文本框，方便调试
            };
            args = $.extend({},defaults,args);            
            var itemwidth = args.width;                 
            if(!itemwidth &&$this.attr("wid")){
                itemwidth =$this.attr("wid");
            }
            var chks = $this.find("input[type=checkbox]");
            var rads = $this.find("input[type=radio]"); 
            //默认值
            if(args.chks){
                function initchks(i,item){
                    if(args.chks.contains($(item).attr("value"))){
                        $(item).attr("checked","checked");                        
                    }
                }     
                chks.each(initchks);
                rads.each(initchks);
                          
            }
            chks.each(function(i,item){
                item = $(item);
                if(item.attr("cas")) return;
                item.attr("cas","cas");
                var html="";  
                var checked=item.attr("checked")=="checked";
                html+='<div class="checkitem'+(checked?" checkitem"+cssName.checked:"")+'"'+(itemwidth?' style="width:' + itemwidth + 'px"' : '')+'><span class="checkbox checkboxdefault' + (checked?" "+args.type+cssName.checked:"") + '"></span></div>';
                var ctl = item.next();
                ctl.addClass("checkctl");
                var items = $(html).insertAfter(item);
                ctl.appendTo(items);
                var obj = $("span.checkbox",items);
                item.bind("change",function(){
                    if(item.attr("checked")){
                        obj.addClass(args.type + cssName.checked);
                        items.addClass("checkitem"+ cssName.checked);
                    }
                    else{
                        obj.removeClass(args.type + cssName.checked);
                        items.removeClass("checkitem"+ cssName.checked);
                    }
                    if(args.callback){args.callback(item.attr("checked"))}
                });
                bindEvents(obj);
                bindCtlEvents(ctl);
                if(!args.debug) item.hide();
            });     
            
            var ctlall = $this.find("input[ctl=all]");
            if(ctlall){
                ctlall.bind("change",function(){
                    chks.not($(this)).each(function(){
                        if(ctlall.attr("checked")){
                            $(this).attr('checked','checked');
                        }
                        else
                            $(this).removeAttr('checked');
                        $(this).trigger("change");
                    });
                    
                });
            } 

            var tmpname = CAS.RndVar();
            rads.each(function(i,item){
                item = $(item);
                if(typeof(item.attr("name"))=="undefined")
                    item.attr("name",tmpname);
                if(item.attr("cas")) return;
                item.attr("cas","cas");
                var ctl = item.next();
                var html="";                  
                var checked=item.attr("checked");
                //这里其他浏览器竟然都不会选中radio，真TM奇怪，有时又行
                //alert(item.is(":checked"));
                html+='<div class="checkitem'+(checked?" checkitem"+cssName.checked:"")+'"'+(itemwidth?' style="width:' + itemwidth + 'px"' : '')+'><span class="radio radiodefault' + (checked?" " + args.type + cssName.checked:"") + '"></span></div>';
                ctl.addClass("checkctl");
                var items = $(html).insertAfter(item);
                ctl.appendTo(items);
                var obj = $("span.radio",items);
                obj.attr("name",item.attr("name"));
                item.bind("click",function(){
                    $this.find("." + args.type + cssName.checked).not(obj).removeClass(args.type + cssName.checked);
                    if(!obj.hasClass(args.type + cssName.checked))
                    {                  
                        obj.addClass(args.type + cssName.checked);
                        items.addClass("checkitem"+ cssName.checked);                        
                    }
                    if(typeof( args.callback)=="function"){                            
                            args.callback($this);
                        }
                });
                bindEvents(obj);
                bindCtlEvents(ctl);
                if(!args.debug) item.hide();
            });

            if($this.attr("disabled")){
                $this.casCheckDisable(true);
            }

            function getcheck(obj){
                var checked = obj.parent().prev().attr("checked");
                var check = "";
                if(checked) check="checked";
                return check;
            }
            function bindEvents(obj){
                obj.bind("mouseenter",function(){
                    $(this).addClass(args.type + getcheck($(this)) + cssName.over);
                })
                .bind("mouseleave",function(){
                    var check=getcheck($(this));
                    $(this).removeClass(args.type + cssName.over + " " + args.type + check + cssName.over);
                })
                .bind("mousedown",function(){
                    var o=$(this);
                    var check=getcheck($(this));
                    if(o.hasClass(args.type + cssName.disabled) || o.hasClass(args.type + check + cssName.disabled)) return;
                    if(o.hasClass(args.type + cssName.checked) && args.type!="radio"){
                        o.removeClass(args.type + cssName.checked + " " + args.type + check + cssName.over);
                        o.addClass(args.type + cssName.over);
                    }
                    else{
                        if(args.type=="radio")
                        {
                            var oth =$this.find(":[name=" + o.attr("name") + "]").filter("." + args.type + cssName.checked).not(obj);
                            rads.each(function(){$(this).attr("checked",false);});
                            oth.parent().removeClass("checkitem"+ cssName.checked);
                            oth.removeClass(args.type + cssName.checked);
                        }
                        o.addClass(args.type + cssName.checked);
                        o.addClass(args.type + "checked" + cssName.over);
                        o.parent().addClass("checkitem"+ cssName.checked);
                    }
                    setValue(o);
                })
            }
            //设置值
            function setValue(o){
                if(o.hasClass(args.type + cssName.checked)){
                    o.parent().prev().attr('checked','checked');
                }
                else
                    o.parent().prev().removeAttr('checked');
                var type = o.parent().prev().attr("type").toLowerCase();
                var event = type=="checkbox"?"change":"click";
                o.parent().prev().trigger(event);
            }            
        
            function bindCtlEvents(obj){
                obj.bind("mousedown",function(){
                    var check=getcheck($(this));
                    var o = $(this).prev();
                    if(o.hasClass(args.type + cssName.disabled) || o.hasClass(args.type + check + cssName.disabled)) return;
                    if(o.hasClass(args.type + cssName.checked) && args.type!="radio"){
                        o.removeClass(args.type + cssName.checked);                        
                    }
                    else{
                        if(args.type=="radio"){
                            var oth =$this.find(":[name=" + o.attr("name") + "]").filter("." + args.type + cssName.checked).not(obj);
                            rads.each(function(){$(this).attr("checked",false);});
                            oth.parent().removeClass("checkitem"+ cssName.checked);
                            oth.removeClass(args.type + cssName.checked);                            
                        }
                        o.addClass(args.type + cssName.checked);
                        o.parent().addClass("checkitem"+ cssName.checked);
                    }
                    setValue(o);
                });
            }
    }
})(jQuery)