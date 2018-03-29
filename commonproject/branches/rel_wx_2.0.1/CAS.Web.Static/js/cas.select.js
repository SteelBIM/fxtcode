//下拉框 kevin
; (function ($) {
    $.fn.casSelect = function () {
        function hideOptions(speed) {
            if (speed.data) { speed = speed.data }
            var sel = $(document).data("nowselectoptions");
            if (sel) {
                $(sel).hide();                
                $(document).unbind("click");
                $(document).unbind("keyup");
                $(sel).parent().parent().removeClass("rel");
            }
        }
        function hideOptionsOnEscKey(e) {
            var myEvent = e || window.event;
            var keyCode = myEvent.keyCode;
            if (keyCode == CAS.KeyCode.ESC) hideOptions(e.data);
        }
        function showOptions(speed) {
            var sel = $(document).data("nowselectoptions");
            var div = $(sel).prev();
            $(document).bind("click", speed, hideOptions);
            $(document).bind("keyup", speed, hideOptionsOnEscKey);
            if (navigator.userAgent.indexOf('Firefox') >= 0 && $(sel).parent().parent().isTag("td"))
            {
                $(sel).parent().parent().parent().parent().parent().addClass("rel");
            }
            else if($(sel).parent().parent().isTag("td")){
                $(sel).parent().parent().addClass("rel");
            }
            $(sel).show();              
        }

        $(this).each(function (args) {
            var $this = $(this);
            args=$.extend({},{debug:false},args);
            if(!$this.hasClass("select")) $this.addClass("select");
            var mul = $this.hasClass("mul");
            var input = $this.hasClass("input");
            var disable= $this.hasClass("disabled") || $this.attr("disabled");
            var speed = "fast";
            var width = $this.width() - 9;
            var div = $this.data("cssobj");
            var divselect=null;
            var divoptions=null;
            function setValue(text) {
                divselect.val(text);
            }
            if (div) { //把替换用的层缓存起来 kevin
                divselect=$("input[type='text']",div);
                divoptions=$("ul",div);
                divoptions.empty();
                div.width(width);
                divselect.width(width-4); 
            }
            else{
                div=$("<span class='tag_select'/>").insertAfter(this);
                divselect = $("<input type='text' readonly/>").appendTo(div).addClass("input");
                if (input) { //可以输入
                    divselect.removeAttr("readonly");
                }
                if( disable){
                    div.casdisable();
                }
                if (mul) { //多选，将下拉框加上多选属性 kevin
                    $this.attr("multiple", "multiple");
                }
                divoptions = $("<ul></ul>").insertAfter(divselect).addClass("tag_options").hide();
                $this.data("cssobj", div);
                div.width(width);
                divselect.width(width-4); 
                div.click(function (e) {
                    if($(this).hasClass("disabled")) return;                
                    //多个下拉框同时存在时，相互点击切换隐藏 kevin
                    if ($($(document).data("nowselectoptions")).get(0) != $(this).find("ul").get(0)) {
                        hideOptions(speed);
                    }
                    //显示下拉层 kevin
                    if (!$(this).find("ul").is(":visible")) {                    
                        $(document).data("nowselectoptions", $(this).find("ul"));
                        showOptions(speed);
                        setPos();
                    }
                    else{
                        hideOptions(speed);
                    }
                    CAS.stopDefault(e);
                    CAS.stopPropagation(e);
                });

                function setPos(){                        
                    
                    width = div.outerWidth() - 2;                      
                    divoptions.css("left", div.position().left);
                    var top=div.position().top + div.outerHeight();
                    if(div.offset().top + divoptions.outerHeight() > $(document.body).height() ){
                        top = div.position().top - divoptions.outerHeight() +1;
                    }
                    divoptions.css("top", top);
                    divoptions.css("width", width);
                }

                //$("div").bind("scroll",setPos);
            
                div.hoverClass("tag_select_hover");
                
            
                //选项发生变化时赋值 kevin
                $this.change(function () {
                    var html = [];
                    var ops = $(this).children("option:selected");
                    ops.each(function (i) {
                        html.push(ops.eq(i).text());
                    });
                    setValue(html.join(","));
                });
            }
            
            if(!args.debug) $this.hide();  
            var options=$this.find("option");            
            if(options.size()>0){
                options.each(function (i) {
                    var op = "";
                    if (mul) { //多选
                        op += "<input type='checkbox'/>";
                    }
                    op += "<label>" + $(this).html() + "</label>";
                    var lioption = $("<li style='margin:0;'></li>").html(op).appendTo(divoptions);
                    //修复IE下高度 kevin
                    if ($.browser.msie && divoptions.height() > 200) divoptions.css("height", "200");
                    if ($(this).attr("selected")) {
                        lioption.addClass("open_selected");
                        setValue($(this).text());
                        if (mul) {
                            $("input[type=checkbox]", lioption).attr("checked", "checked");
                        }
                    }
                    lioption.data("option", this);
                
                    lioption.click(function (e) {
                        if(e.srcElement && $(e.srcElement).isTag("li")) {
                            $("label",$(this)).eq(0).trigger("mousedown");                        
                        }
                        if(!mul)
                        {
                            lioption.data("option").selected = true;
                            lioption.addClass("open_selected").siblings().removeClass("open_selected");
                            hideOptions(speed);
                        }
                        else{
                            lioption.data("option").selected = !lioption.data("option").selected ;
                            lioption.toggleClass("open_selected");                        
                        }
                        e.stopPropagation();
                        $(lioption.data("option")).trigger("change", true);

                    });
                    lioption.hoverClass("open_hover");                
                });
                divoptions.cascheckbox({debug:false});
            }
        });

    }

})(jQuery);  