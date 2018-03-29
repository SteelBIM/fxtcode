; (function ($, window) {
    var CAS = { //动态加载的js序列
        API: function (args) {//dataType默认为json,type默认为GET,async默认异步，cache默认不缓存
            $.ajax({ dataType: args.dataType ? args.dataType : "json", url: args.url, type: args.type ? args.type : "POST", data: args.data,
                async: args.async ? false : true, cache: args.cache ? args.cache : false,
                success: function (data) {
                    if (args.callback) { args.callback(data); }
                }
            });
        },GetQuery: function (name, url) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = null;
            if (url)
                r = url.substr(1).match(reg);
            else
                r = window.location.search.substr(1).match(reg);
            if (r != null) return decodeURI(r[2]); return null;
        }
    };
    window.CAS=CAS;
    //jQuery扩展方法
    $.fn.extend({
        btnMouseSweep: function (enterClass,leaveClass) {//enterClass：划入class,leaveClass：原class
            var that =this;
            that.mouseenter(function () {
                if (that.hasClass(leaveClass)) {
                     that.attr("class", enterClass)
                }
            }).mouseleave(function () {
                if (that.hasClass(enterClass)) {
                     that.attr("class", leaveClass)
                }
            });
            return that;
        }
    });
})(jQuery,window);

//(function(){
//    //禁止退格键 作用于firefox、opera   
//    document.onkeypress = banBackSpace;
//    //禁止退格键 作用于ie、chrome  
//    document.onkeydown = banBackSpace;

//    //处理键盘事件 禁止后退键（Backspace）密码或单行、多行文本框除外   
//function banBackSpace(e) {
//    var ev = e || window.event; //获取event对象     
//    var obj = ev.target || ev.srcElement; //获取事件源       
//    var t = obj.type || obj.getAttribute('type'); //获取事件源类型       
//    //获取作为判断条件的事件类型   
//    var vReadOnly = obj.readOnly;
//    var vDisabled = obj.disabled;
//    //处理undefined值情况   
//    vReadOnly = (vReadOnly == undefined) ? false : vReadOnly;
//    vDisabled = (vDisabled == undefined) ? true : vDisabled;
//    //当敲Backspace键时，事件源类型为密码或单行、多行文本的，    
//    //并且readOnly属性为true或disabled属性为true的，则退格键失效    
//    var flag1 = ev.keyCode == 8 && (t == "password" || t == "text" || t == "textarea") && (vReadOnly == true || vDisabled == true);
//    //当敲Backspace键时，事件源类型非密码或单行、多行文本的，则退格键失效      
//    var flag2 = ev.keyCode == 8 && t != "password" && t != "text" && t != "textarea";
//    //判断      
//    if (flag2 || flag1)
//        event.returnValue = false;
//}
//})();