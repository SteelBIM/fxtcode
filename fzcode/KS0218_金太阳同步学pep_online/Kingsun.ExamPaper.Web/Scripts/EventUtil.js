var EventUtil= {
    //�ֱ�ʹ��DOM0��������DOM2��������IE����������¼�
    addHandler: function (element, type, handler) {
        if (element.addEventListener) {//DOM2
            element.addEventListener(type, handler, false);
        } else if (element.attachEvent) {//IE
            element.attachEvent("on" + type, handler);
        } else {//DOM0��
            element["on" + type] = handler;
        }
    },
    //�Ƴ�֮ǰ��ӵ��¼��������
    removeHandler: function (element, type, handler) {
        if (element.removeEventListener) {//DOM2
            element.removeEventListener(type, handler, false);
        } else if (element.detachEvent) {//IE
            element.detachEvent("on" + type, handler);
        } else {//DOM0��
            element["on" + type] = null;
        }
    },
    //�����¼��Ķ���
    getEvent: function (event) {
        return event ? event : window.event;
    },
    //�����¼���Ŀ��
    getTarget: function (event) {
        return event.target || event.srcElement;
    },
    //ȡ���¼���Ĭ����Ϊ
    preventDefault: function (event) {
        if (event.preventDefault) {
            event.preventDefault();
        } else {
            event.returnValue = false;
        }
    },
    //��ֹ�¼�ð��
    stopPropagtion: function (event) {
        if (event.stopPropagtion) {
            event.stopPropagtion();
        } else {
            event.cancelBubble = true;
        }
    },
    //ȡ�����Ԫ��
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
    //��ȡbutton���ԣ���IEģ�͹淶��ΪDOM��ʽ��
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
    //�����ֹ���ʱ��ʾdetail��ֵ
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
    //����keypress�¼�ʱ��ȡ���µļ��������ASCII����
    getCharCode: function (event) {
        if (typeof event.charCode == "number") {
            return event.charCode;
        } else {
            return event.keyCode;
        }
    },
    //�Ӽ�������ȡ���ı�����
    getClipboardText: function (event) {
        var clipboardData = (event.clipboardData || window.clipboardData);
        return clipboardData.getData("text");
    },
    //���ı����ݷŵ���������
    setClipboardText: function (event, value) {
        if (event.clipboardData) {
            return event.clipboardData.setData("text/plain", value);
        } else if (window.clipboardData) {
            return window.clipboardData.setData("text", value);
        }
    }
};