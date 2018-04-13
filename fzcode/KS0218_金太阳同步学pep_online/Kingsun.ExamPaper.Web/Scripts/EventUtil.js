var EventUtil= {
    //分别使用DOM0级方法、DOM2级方法或IE方法来添加事件
    addHandler: function (element, type, handler) {
        if (element.addEventListener) {//DOM2
            element.addEventListener(type, handler, false);
        } else if (element.attachEvent) {//IE
            element.attachEvent("on" + type, handler);
        } else {//DOM0级
            element["on" + type] = handler;
        }
    },
    //移除之前添加的事件处理程序
    removeHandler: function (element, type, handler) {
        if (element.removeEventListener) {//DOM2
            element.removeEventListener(type, handler, false);
        } else if (element.detachEvent) {//IE
            element.detachEvent("on" + type, handler);
        } else {//DOM0级
            element["on" + type] = null;
        }
    },
    //返回事件的对象
    getEvent: function (event) {
        return event ? event : window.event;
    },
    //返回事件的目标
    getTarget: function (event) {
        return event.target || event.srcElement;
    },
    //取消事件的默认行为
    preventDefault: function (event) {
        if (event.preventDefault) {
            event.preventDefault();
        } else {
            event.returnValue = false;
        }
    },
    //阻止事件冒泡
    stopPropagtion: function (event) {
        if (event.stopPropagtion) {
            event.stopPropagtion();
        } else {
            event.cancelBubble = true;
        }
    },
    //取得相关元素
    getRelatedTarget: function (event) {
        if (event.relatedTarget) {
            return event.relatedTarget;
        } else if (event.toElement) {
            return event.toElement;
        } else if (event.fromElement) {
            return event.fromElement;
        } else {
            return null;
        }
    },
    //获取button属性（将IE模型规范化为DOM方式）
    getButton: function (event) {
        if (document.implementation.hasFeature("MouseEvents", "2.0")) {
            return event.button;
        } else {
            switch (event.button) {
                case 0:
                case 1:
                case 3:
                case 5:
                case 7:
                    return 0;
                case 2:
                case 6:
                    return 2;
                case 4:
                    return 1;
            }
        }
    },
    //鼠标滚轮滚动时显示detail的值
    wheelDelta: function (event) {
        if (event.wheelDelta) {
            return (client.engine.opera && client.engine.opera < 9.5 ? -event.wheelDelta : event.wheelDelta);
        } else {
            return -event.detail * 40;
        }
    },
    getWheelDelta: function (event) {
        if (event.wheelDelta) {
            return event.wheelDelta;
        } else {
            return -event.detail * 40;
        }
    },
    //发生keypress事件时获取按下的键所代表的ASCII编码
    getCharCode: function (event) {
        if (typeof event.charCode == "number") {
            return event.charCode;
        } else {
            return event.keyCode;
        }
    },
    //从剪贴板中取得文本数据
    getClipboardText: function (event) {
        var clipboardData = (event.clipboardData || window.clipboardData);
        return clipboardData.getData("text");
    },
    //将文本数据放到剪贴板中
    setClipboardText: function (event, value) {
        if (event.clipboardData) {
            return event.clipboardData.setData("text/plain", value);
        } else if (window.clipboardData) {
            return window.clipboardData.setData("text", value);
        }
    }
};