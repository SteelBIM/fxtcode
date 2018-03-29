//输入框 kevin
; (function ($) { 
    $.fn.casNumber=$.casNumber=function(){
        var o = this;
        if(!$.fn.numeral){
            CAS.Use(["jquery.numeral.js"], function () {
                return _number();
            });
        }
        else{
            return _number();
        }
        function _number(){
            return o.each(function () {
                var $this = $(this);
                var args = { afy: 0, scale: 0 };
                if ($this.hasClass("afy")) args.afy = 1;
                if ($this.attr("dot")) args.scale = $this.attr("dot");
                if($this.hasClass("iptval") && $this.val()==$this.attr("rel")){
                    $this.bind("focus",function(){$this.addClass("tr");})
                    .bind("blur",function(){
                        if($this.hasClass("iptval") && $this.val()==$this.attr("rel"))
                            $this.removeClass("tr");
                    });
                }
                else{$this.addClass("tr");}
                $this.numeral(args);
            });
        }
    }
    //密码强度输入
    $.fn.casPassword=$.casPassword=function(){
        return this.each(function(){
            var $this=$(this);
            var strongdiv = $("<div class='passwordstrong dn'/>").insertAfter($this);
            $('<div class="password_title">密码强度：</div><div class="password_first"></div><div class="password_second"></div><div class="password_third"></div>').appendTo(strongdiv);
            $this.bind("keyup",function(){
                var password=$this.val();
                if(password=="") {strongdiv.hide();return;}
                if(strongdiv.is(":hidden"))
                {
                    //strongdiv.css({left:$this.position().left+$this.width()+5,top:$this.position().top});
                    strongdiv.show();
                }
                var score = 0; 
                score += password.length * 4; 
                score += ( $.fn.checkRepetition(1,password).length - password.length ) * 1; 
                score += ( $.fn.checkRepetition(2,password).length - password.length ) * 1; 
                score += ( $.fn.checkRepetition(3,password).length - password.length ) * 1; 
                score += ( $.fn.checkRepetition(4,password).length - password.length ) * 1; 
                //含数字
                if (password.match(/(.*[0-9].*[0-9].*[0-9])/)){ score += 5;} 
                //含符号
                if (password.match(/(.*[!,@,#,$,%,^,&,*,?,_,~].*[!,@,#,$,%,^,&,*,?,_,~])/)){ score += 5 ;} 
                //含大小写
                if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)){ score += 10;} 
                //含数字和字符组合
                if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)){ score += 15;} 
                //含符号和数字组合
                if (password.match(/([!,@,#,$,%,^,&,*,?,_,~])/) && password.match(/([0-9])/)){ score += 15;} 
                //含符号和字符组合
                if (password.match(/([!,@,#,$,%,^,&,*,?,_,~])/) && password.match(/([a-zA-Z])/)){score += 15;} 
                //只含字符或数字
                if (password.match(/^\w+$/) || password.match(/^\d+$/) ){ score -= 10;} 
                var strong=2; //0为弱,1为中,2为强
                if ( score < 0 ){score = 0;} 
                if ( score > 100 ){ score = 100;} 
                if (score < 34 ){ strong =0;} 
                else if (score < 68 ){ strong=1;} 
                //$(".password_first",strongdiv).text(score);
                $("div",strongdiv).not(":first").removeClass("password_low").removeClass("password_middle").removeClass("password_strong").text("");
                switch(strong){
                    case 0:
                        $(".password_first",strongdiv).addClass("password_low").text("弱");
                        break;
                    case 1:
                        $(".password_second",strongdiv).addClass("password_middle").text("中");
                        break;
                    case 2:
                        $(".password_third",strongdiv).addClass("password_strong").text("强");
                        break;
                }
                strongdiv.show();
            });
        });
    };
    $.fn.checkRepetition = function(pLen,str) { 
        var res = ""; 
        for (var i=0; i<str.length ; i++ ) 
        { 
            var repeated=true; 
            for (var j=0;j < pLen && (j+i+pLen) < str.length;j++){ 
                repeated=repeated && (str.charAt(j+i)==str.charAt(j+i+pLen)); 
            } 
            if (j<pLen){repeated=false;} 
            if (repeated) { 
                i+=pLen-1; 
                repeated=false; 
            } 
            else { 
                res+=str.charAt(i); 
            } 
        } 
        return res; 
    }; 
})(jQuery)

