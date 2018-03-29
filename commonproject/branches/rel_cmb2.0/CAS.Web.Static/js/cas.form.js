//表单提交 kevin
//表单提交按钮默认是class为btnenter的按钮，也可以用args.ctl绑定触发提交的按钮。
//表单所有需要提交的数据控件以field_前缀，要注意checkbox,radio的使用
; (function ($) {
    $.fn.casForm=$.casForm=function(args){
        var $this=$(this);
        var reqs = $("input.must,textarea.must",$this);
        var defaults={debug:false};
        args=$.extend({},defaults,args);
        reqs.live("blur",function(){
            if($(this).val()!="") $(this).castip({hide:true});
        });
        var ctl = $(".btnenter", $this);
        if(args.ctl) ctl = args.ctl;
        //文本框回车提交
        $("input[type=text],input[type=password]", $this).live("keydown", function (e) {
            if (e.keyCode == CAS.KeyCode.ENTER) {
                ctl.trigger("click");
                e.stopPropagation();
            }
        });
        ctl.click(function () {
            var btn=$(this);
            var pass = true;
            reqs.castip({ hide: true }) ;
            for (var i = 0; i < reqs.size(); i++) {
                var item = reqs.eq(i);
                if(item.hasClass("ignore")) continue;
                if (item.val() == "" || item.val() == item.attr("rel")) {
                    item.focus().castip({ type: "pop", keep: false, content: item.attr("rel") });                    
                    pass = false;
                    break;
                }
            }
            var objs = $("*",$this);
            function submitdata(){
                btn.casbtnloading();                
                var progress = CAS.Progress("正在处理，请稍候...");
                var postdata={};
                objs.each(function(){
                    var o=$(this);
                    if(o.attr("id") && o.attr("id").indexOf("field_")>=0 && !o.hasClass("ignore")){
                        var field = o.attr("id").replace("field_","");
                        if(o.isTag("input")){
                            if (o[0].type == "text" || o[0].type == "hidden" || o[0].type=="password") {
                                if(o.hasClass("iptval") && o.attr("rel")==o.val())
                                    postdata[field]="";    
                                else
                                    postdata[field] = o.val();
                            }
                        }
                        else if(o.isTag("textarea")){
                            postdata[field] = o.val();
                        }
                        else if(o.isTag("select")){
                            postdata[field] = o.val();
                        }
                        //选择框，这里要注意是使用class来选取的 kevin
                        else if(o.hasClass("check_radio") || o.hasClass("check_checkbox")){
                            postdata[field] = o.casCheckValue();
                        }
                    }
                });
                //除默认field控件外，还可接受另外赋的值，替代默认
                postdata=$.extend({},postdata,args.getdata());
                CAS.API({ type: "post", api: args.api, data: postdata
                    , callback: function (data) {
                        //objs.casenable();
                        btn.casbtnloaded();
                        progress.close();
                        var rtn = data;
                        if(args.debug)
                            rtn.postdata = postdata;
                        args.callback(rtn);
                    }
                });   
            }
            if (pass) {
                //这里判断另外的检查处理
                if(args.check){
                    if(!args.check()) return ;
                }
                //objs.casdisable();
                if(args.confirm){
                    CAS.Confirm({ content: args.confirm, callback: submitdata });
                }
                else submitdata();             
            }
        });
    };
})(jQuery)