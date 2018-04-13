//检测系统
function checkPlatform() {
    console.log("checkPlatform");
    var system =
    {
        win: false,
        mac: false,
        xll: false
    };
    //检测平台    
    var p = navigator.platform;

    system.win = p.indexOf("Win") == 0;
    system.mac = p.indexOf("Mac") == 0;
    system.x11 = (p == "X11") || (p.indexOf("Linux") == 0);
    //跳转语句    
    if (system.win || system.mac || system.xll) {
        console.log("PC端");
        return 0;
    }
    else {
        if (navigator.userAgent.indexOf("Mac") < 0) {
            console.log("安卓端");
            return 1;
        }
        else {
            console.log("苹果端");
            return 2;
        }

    }
}

/*与OC交互的所有JS方法都要放在此处注册，才能调用通过JS调用OC或者让OC调用这里的JS*/
//添加 callback
function setupWebViewJavascriptBridge(callback) {
    if (window.WebViewJavascriptBridge) { return callback(WebViewJavascriptBridge); }

    if (checkPlatform() == 1) {
        document.addEventListener('WebViewJavascriptBridgeReady', function () {
            callback(WebViewJavascriptBridge);
        }, false);
        return;
    }

    if (window.WVJBCallbacks) { return window.WVJBCallbacks.push(callback); }
    window.WVJBCallbacks = [callback];
    var WVJBIframe = document.createElement('iframe');
    WVJBIframe.style.display = 'none';
    WVJBIframe.src = 'wvjbscheme://__BRIDGE_LOADED__';
    document.documentElement.appendChild(WVJBIframe);
    setTimeout(function () { document.documentElement.removeChild(WVJBIframe) }, 0);
}
setupWebViewJavascriptBridge(function (bridge) {

    if (checkPlatform() == 1) {
        bridge.init(function (message, responseCallback) {
            log('JS got a message', message);
            var data = { 'Javascript Responds': 'Wee!' };
            log('JS responding with', data);
            responseCallback(data);
        });
    }
    //退出班级
    bridge.registerHandler('selectClassInfo', function (data) {
        //alert(data);
        data = JSON.parse(data);
        var UserID = data.UserID;
        var TelePhone = data.Telephone;
        var AppID = data.AppID;
        if (data) {
            window.location.href = "../Information/Index?returnPage=Information&AppID=" + AppID + "&UserID=" + UserID + "&TelePhone=" + TelePhone + "";
        }
    });
    //页面返回
    bridge.registerHandler('returnClick', function (data) { 
        //alert(window.location.search); 
        //return;
        var returnPage = GetQueryString("returnPage");
        var UserID = GetQueryString("UserID");
        var AppID = GetQueryString("AppID");
        var TelePhone = GetQueryString("TelePhone");
        if (returnPage==null) {
            finish();
        } else if (returnPage == "Details") {
            window.location.href = "../Apply/Index?&UserID=" + UserID + "&AppID=" + AppID + "";
        } else if (returnPage == "Information") {
            window.location.href = "../Apply/Index?&UserID=" + UserID + "&AppID=" + AppID + "";
        } else if (returnPage == "AmendPhone") {
            window.location.href = "../Information/Index?&UserID=" + UserID + "&AppID=" + AppID + "&TelePhone=" + TelePhone + "";
        } else if (returnPage == "Agreement") {
            window.location.href = "../Information/Index?&UserID=" + UserID + "&AppID=" + AppID + "&TelePhone=" + TelePhone + "";
        } else if (returnPage == "Details2") {
            history.go(-1);
        } else {
            finish();
        } 
    });   
});


//采用正则表达式获取地址栏参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}