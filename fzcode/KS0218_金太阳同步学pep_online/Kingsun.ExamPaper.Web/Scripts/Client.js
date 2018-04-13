/**
 * Created by Webb on 2015/11/17.
 * 说明：用户代理检测与浏览器User Agent详细分析
 */
var Client = function () {
    //呈现引擎
    var engine = {
        trident: 0,
        gecko: 0,
        webkit: 0,
        khtml: 0,
        presto: 0,
        ver: null
    };
    //浏览器
    var browser = {
        ie: 0,
        firefox: 0,
        safari: 0,
        konq: 0,
        opera: 0,
        chrome: 0,
        ver: null//具体的版本号
    };
    //平台、设备和操作系统
    var system = {
        win: false,
        mac: false,
        x11: false,

        //移动设备
        iphone: false,
        ipod: false,
        ipad: false,
        ios: false,
        android: false,
        nokiaN: false,
        winMobile: false,

        //游戏系统
        wii: false,
        ps: false
    };

    var ua = navigator.userAgent;
    //匹配Webkit内核浏览器（Chrome、Safari、新Opera）
    if (/AppleWebKit\/(\S+)/.test(ua)) {
        engine.ver = RegExp["$1"];
        engine.webkit = parseFloat(engine.ver);
        //确定是不是引用了Webkit内核的Opera
        if (/OPR\/(\S+)/.test(ua)) {
            browser.ver = RegExp["$1"];
            browser.opera = parseFloat(engine.ver);
        }
            //确定是不是Chrome
        else if (/Chrome\/(\S+)/.test(ua)) {
            browser.ver = RegExp["$1"];
            browser.chrome = parseFloat(browser.ver);
        }
            //确定是不是高版本（3+）的Safari
        else if (/Version\/(\S+)/.test(ua)) {
            browser.ver = RegExp["$1"];
            browser.safari = parseFloat(browser.ver);
        }
            //近似地确定低版本Safafi版本号
        else {
            var safariVersion = 1;
            if (engine.webkit < 100) {
                safariVersion = 1;
            } else if (engine.webkit < 312) {
                safariVersion = 1.2;
            } else if (engine.webkit < 412) {
                safariVersion = 1.3;
            } else {
                safariVersion = 2;
            }
            browser.safari = browser.ver = safariVersion;
        }
    }
        //只匹配拥有Presto内核的老版本Opera 5+(12.15-)
    else if (window.opera) {
        engine.ver = browser.ver = window.opera.version();
        engine.presto = browser.opera = parseFloat(engine.ver);
    }
        //匹配不支持window.opera的Opera 5-或伪装的Opera
    else if (/Opera[\/\s](\S+)/.test(ua)) {
        engine.ver = browser.ver = RegExp["$1"];
        engine.presto = browser.opera = parseFloat(engine.ver);
    }
    else if (/KHTML\/(\S+)/.test(ua) || /Konqueror\/([^;]+)/.test(ua)) {
        engine.ver = browser.ver = RegExp["$1"];
        engine.khtml = browser.konq = parseFloat(engine.ver);
    }
        //排除了WebKit和KHTML之后，可以准确检测Gecko内核了
    else if (/rv:([^\)]+)\) Gecko\/\d{8}/.test(ua)) {
        engine.ver = RegExp["$1"];
        engine.gecko = parseFloat(engine.ver);
        //确定是不是FireFox
        if (/Firefox\/(\S+)/.test(ua)) {
            browser.ver = RegExp["$1"];
            browser.firefox = parseFloat(browser.ver);
        }
    }
        //确定是否是Trident内核的浏览器（IE8+）
    else if (/Trident\/([\d\.]+)/.test(ua)) {
        engine.ver = RegExp["$1"];
        engine.trident = parseFloat(engine.ver);
        if (/rv\:([\d\.]+)/.test(ua) || /MSIE ([^;]+)/.test(ua)) {   //匹配IE8-11+
            browser.ver = RegExp["$1"];
            browser.ie = parseFloat(browser.ver);
        }
    }
        //匹配IE6、IE7
    else if (/MSIE ([^;]+)/.test(ua)) {
        browser.ver = RegExp["$1"];
        browser.ie = parseFloat(browser.ver);
        engine.ver = browser.ie - 4.0;  //模拟IE6、IE7中的Trident值
        engine.trident = parseFloat(engine.ver);
    }

    //检测平台
    var p = navigator.platform;
    system.win = p.indexOf("Win") == 0;
    system.mac = p.indexOf("Mac") == 0;
    system.x11 = (p.indexOf("X11") == 0) || (p.indexOf("Linux") == 0);
    //检测Windows操作系统
    if (system.win) {
        if (/Win(?:dows )?([^do]{2})\s?(\d+\.\d+)?/.test(ua)) {
            if (RegExp["$1"] == "NT") {
                switch (RegExp["$2"]) {
                    case "5.0":
                        system.win = "2000";
                        break;
                    case "5.1":
                        system.win = "XP";
                        break;
                    case "6.0":
                        system.win = "Vista";
                        break;
                    case "6.1":
                        system.win = "7";
                        break;
                    case "6.2":
                        system.win = "8";
                        break;
                    case "6.3":
                        system.win = "8.1";
                        break;
                    case "10":
                        system.win = "10";
                        break;
                    default:
                        system.win = "NT";
                        break;
                }
            } else if (RegExp["$1"] == "9x") {
                system.win = "ME";
            } else {
                system.win = RegExp["$1"];
            }
        }
    }

    //移动设备
    system.iphone = ua.indexOf("iPhone") > -1;
    system.ipod = ua.indexOf("iPod") > -1;
    system.ipad = ua.indexOf("iPad") > -1;
    system.nokiaN = ua.indexOf("NokiaN") > -1;

    //检测Windows Mobile（也称为Windows CE）
    system.winMobile = (system.win == "CE");
    if (system.win == "CE") {
        system.winMobile = system.win;
    } else if (system.win == "Ph") {
        if (/Windows Phone OS (\d+\.\d+)/.test(ua)) {
            system.win = "Phone";
            system.winMobile = parseFloat(RegExp["$1"]);
        }
    }

    //检测iOS版本
    if (system.mac && ua.indexOf("Mobile") > -1) {
        if (/CPU(?:iPhone )?OS (\d+_\d+)/.test(ua)) {
            system.ios = parseFloat(RegExp.$1.replace("_", "."));
        } else {
            system.ios = 2;//不能真正检测出来，所以只能猜测
        }
    }

    //检测Android版本
    if (/Android (\d+\.\d+)/.test(ua)) {
        system.android = parseFloat(RegExp.$1);
    }

    //游戏系统
    system.wii = ua.indexOf("Wii") > -1;
    system.ps = /Playstation/i.test(ua);

    //返回这些对象
    return {
        isSupportIE8: (browser.ie > 7 || browser.ie == 0),//表示是否支持IE8+
        ua: ua,  //用户浏览器UA原文
        engine: engine, //包含着用户浏览器引擎（内核）信息
        browser: browser,   //包括用户浏览器品牌与版本信息
        system: system  //用户所用的操作系统及版本信息
    };
}();