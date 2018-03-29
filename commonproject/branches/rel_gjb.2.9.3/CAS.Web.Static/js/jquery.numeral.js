/** 
* 限制输入框只能输入数字(JQuery插件) 
* 
* @example $("#amount").numeral(1) //1有千分位,0无
* 
* @example $("#amount").numeral(1,4) or $("#amount").numeral({'afy':1,'scale': 4}) 有千位，有小数点
* 
* @example $(".x-amount").numeral(1) 
**/
$.fn.numeral = function () {
    var args = arguments;
    var json = typeof (args[0]) == "object";
    var scale = json ? args[0].scale : args[1];
    scale = scale || 0;

    var afy = json ? args[0].afy : args[0];
    afy = afy || 0;

    $(this).css("ime-mode", "disabled");
    var keys = new Array(8, 9, 35, 36, 37, 38, 39, 40, 46, 13);
    this.bind("keydown", function (e) {
        e = window.event || e;
        var code = e.which || e.keyCode;
        var idx = Array.indexOf(keys, code);
        if (idx != -1) {
            return true;
        }
        var value = this.value;
        if (code == 190 || code == 110) {
            if (scale == 0 || value.indexOf(".") != -1) {
                return false;
            }
            return true;

        } else {
            if ((code >= 48 && code <= 57) || (code >= 96 && code <= 105)) {
                if (scale > 0 && value.indexOf(".") != -1) {
                    var reg = new RegExp("^[0-9]+(\.[0-9]{0," + (scale - 1) + "})?|[1-9]+(\.[0-9]{0," + (scale - 1) + "})$");
                    if (!reg.test(value)) {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    });
    this.bind("blur", function () {
        if ($(this).hasClass("readonly") || $(this).hasClass("disabled")) return;
        //修复与默认值控件的冲突 kevin
        var rel = $(this).attr("rel");
        if (rel) {
            if (this.value == rel) return;
        }
        if (this.value.lastIndexOf(".") == (this.value.length - 1)) {
            this.value = this.value.substr(0, this.value.length - 1);
        } else if (isNaN(this.value)) {
            this.value = "";
        } else {
            var value = this.value;
            if (scale > 0 && value.indexOf(".") != -1) {
                var reg = new RegExp("^[^0*][0-9]+(\.[0-9]{0," + scale + "})?$");
                if (!reg.test(value)) {
                    this.value = CAS.Scale(value, scale);
                }
            }
            else {
                this.value = this.value.replace(/^0*/, '');
            }
            if (afy) this.value = CAS.Commafy(this.value);
            if (parseFloat(this.value) == 0 || this.value == "") this.value = "0";
        }
    });
    this.bind("paste", function () {
        var s = window.clipboardData.getData('text');
        if (!/\D/.test(s));
        value = s.replace(/^0*/, '');
        return false;
    });
    this.bind("focus", function () {
        if ($(this).hasClass("readonly") || $(this).hasClass("disabled")) return;
        if (this.value.indexOf(",") >= 0) {
            this.value = this.value.replace(/,/g, '')
            if (this.createTextRange) {
                var txt = this.createTextRange();
                txt.moveStart('character', this.value.length);
                txt.collapse(true);
                txt.select();
            }
        }
    });
    this.bind("dragenter", function () {
        return false;
    });


    Array.indexOf = function (array, value) {
        for (var i = 0; i < array.length; i++) {
            if (value == array[i]) {
                return i;
            }
        }
        return -1;
    }
};

