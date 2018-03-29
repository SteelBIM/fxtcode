(function ($) {
    $.fn.selectCss = function () {
        function hideOptions(speed) {
            if (speed.data) { speed = speed.data }
            if ($(document).data("nowselectoptions")) {
                $($(document).data("nowselectoptions")).hide();
                $($(document).data("nowselectoptions")).prev("div").removeClass("tag_select_open");
                $(document).data("nowselectoptions", null);
                $(document).unbind("click");
                $(document).unbind("keyup");
            }
        }
        function hideOptionsOnEscKey(e) {
            var myEvent = e || window.event;
            var keyCode = myEvent.keyCode;
            if (keyCode == CAS.KeyCode.ESC) hideOptions(e.data);
        }
        function showOptions(speed) {
            $(document).bind("click", speed, hideOptions);
            $(document).bind("keyup", speed, hideOptionsOnEscKey);
            $($(document).data("nowselectoptions")).show();
            $($(document).data("nowselectoptions")).prev("div").addClass("tag_select_open");
        }

        $(this).each(function () {
            var $this = $(this);
            var mul = $this.hasClass("mul");
            var input = $this.hasClass("input");
            var speed = "fast";
            if ($this.data("cssobj")) { //把替换用的层缓存起来 kevin
                $($this.data("cssobj")).remove();
            }
            if (mul) { //多选，将下拉框加上多选属性 kevin
                $this.attr("multiple", "multiple");
            }
            $this.hide();
            var divselect = $("<div></div>").insertAfter(this).addClass("tag_select");
            if (input) { //可以输入
                $("<input type='text' style='margin:0;line-height:" + divselect.height() + "px'/>").appendTo(divselect).width("100%").css({ border: 0 });
            }
            $this.data("cssobj", divselect);
            var width = $this.width();
            divselect.css("width", width); //保持宽度一致 kevin
            width = divselect[0].offsetWidth - 2;
            var divoptions = $("<ul></ul>").insertAfter(divselect).addClass("tag_options").hide();
            divoptions.css("width", width);

            divselect.click(function (e) {
                //多个下拉框同时存在时，相互点击切换隐藏 kevin
                if ($($(document).data("nowselectoptions")).get(0) != $(this).next("ul").get(0)) {
                    hideOptions(speed);
                }
                //显示下拉层 kevin
                if (!$(this).next("ul").is(":visible")) {
                    e.stopPropagation();
                    $(document).data("nowselectoptions", $(this).next("ul"));
                    showOptions(speed);
                    divoptions.css("left", divselect.offset().left);
                    divoptions.css("top", divselect.offset().top + divselect[0].offsetHeight);
                }
            });

            divselect.hoverClass("tag_select_hover");

            function setValue(text) {
                if (input) {
                    $("input", divselect).val(text);
                }
                else
                    divselect.text(text);
            }

            //选项发生变化时赋值 kevin
            $this.change(function () {
                var html = [];
                var ops = $(this).children("option:selected");
                ops.each(function (i) {
                    html.push(ops.eq(i).text());
                });
                setValue(html.join(","));
            });

            $this.children("option").each(function (i) {
                var op = "";
                if (mul) { //多选
                    op += "<input type='checkbox'/>";
                }
                op += $(this).text();
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
                if (mul) { //多选，让checkbox可以控制
                    var chk = $("input[type=checkbox]", lioption);
                    chk.click(function () {
                        $(this).parent().trigger("click");
                    });
                }
                lioption.click(function (e) {
                    lioption.data("option").selected = true;
                    if (mul) {
                        e.stopPropagation();
                        var chk = $("input[type=checkbox]", lioption);
                        if (chk.attr("checked") == "checked") {
                            chk.removeAttr("checked");
                            lioption.removeClass("open_selected");
                            lioption.data("option").selected = false;
                        }
                        else {
                            chk.attr("checked", "checked");
                            lioption.addClass("open_selected");
                        }
                    }
                    else {
                        lioption.addClass("open_selected").siblings().removeClass("open_selected");
                    }
                    $(lioption.data("option")).trigger("change", true);

                });
                lioption.hoverClass("open_hover");
            });

        });

    }

})(jQuery);