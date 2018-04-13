var Loading = {
    text: '操作正在执行中，请稍候。。。。。。', //Loading默认显示的文字
    //获取滚动条距离上边顶部的距离
    getScrollTop: function () {
        var scrollTop = 0;
        if (document.documentElement && document.documentElement.scrollTop) {
            scrollTop = document.documentElement.scrollTop;
        } else if (document.body) {
            scrollTop = document.body.scrollTop;
        }
        return scrollTop;
    },
    //获取内部内容的总高度,
    getScrollHeight: function () {
        return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
    },
    //是获取可见内容的高度
    getHeight: function () {
        if (window.innerHeight != undefined) {
            return window.innerHeight;
        } else {
            var B = document.body, D = document.documentElement;
            return Math.min(D.clientHeight, B.clientHeight)
        }
    },
    //显示阴影
    showShadow: function () {
        var maskHeight = this.getScrollHeight() + "px";
        var shadowDiv = document.createElement("div");
        shadowDiv.innerHTML = "";
        shadowDiv.setAttribute('id', 'shadowDiv_MASK');
        shadowDiv.setAttribute('style', 'position:fixed; position: absolute; z-index: 999;left:0;top:0;display:block;width:100%;height:' + maskHeight + '; opacity:0.6;filter: alpha(opacity=60);-moz-opacity: 0.6; background:#000;');
        var body = document.getElementsByTagName("body")[0];
        body.appendChild(shadowDiv);
    },
    //关闭阴影
    hideShadow: function () {
        var body = document.getElementsByTagName("body")[0];
        var shadowDiv_MASK = document.getElementById('shadowDiv_MASK');
        if (body && shadowDiv_MASK) {
            body.removeChild(shadowDiv_MASK);
        }
    },
    //显示Loading
    show: function (txt) {
        var top = this.getScrollTop() + (this.getHeight() / 2) + "px";
       Loading.showShadow();
        var me = this;
        if (txt) {
            me.text = txt;
        }
        var loadingDiv = document.createElement("div");
        loadingDiv.innerHTML = me.text;
        loadingDiv.setAttribute('id', 'loadingDiv');
        loadingDiv.setAttribute('style', 'top:' + top + ';left:40%;z-index: 9999;position:absolute;background: #fff;width: 200px;height: 50px;line-height: 50px;text-align: center;');
        var body = document.getElementsByTagName("body")[0];
        body.appendChild(loadingDiv);
    },
    //显示Loading
    showload:function(){
        var html = '';
        html += '<div id="loadingDiv"></div>'
        $("body").append(html);
    },
    hideload:function(){
        $("#loadingDiv").remove();
    },
    //关闭Loading
    hide: function () {
        var body = document.getElementsByTagName("body")[0];
        var loadingDiv = document.getElementById('loadingDiv');
        if (body && loadingDiv) {
            body.removeChild(loadingDiv);
        }
        Loading.hideShadow();
    }
}