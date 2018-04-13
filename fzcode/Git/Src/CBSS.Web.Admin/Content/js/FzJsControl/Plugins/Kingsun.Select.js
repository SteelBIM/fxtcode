(function ($) {
    $.fn.selectValue = function (value) {
        return $(this).each(function () {
            var select = $(this).data("select");
            if (select) {
                select.selectValue(value);
            }
        });
    };
    $.fn.selectIndex = function (index) {
        return $(this).each(function () {
            var select = $(this).data("select");
            if (select) {
                select.selectIndex(index);
            }
        });
    };
    $.fn.refreshData = function (data) {
        var select = $(this).data("select");
        if (select) {
            select.initOptions(data);
        }
    };
    $.fn.KingsunSelect = function (settings) {
        var currentSelect;
        var defaults = {
            data: [],
            needFirst: true,
            needSkin: true,
           // firstOption: '<option value="0" label="请选择...">请选择...</option>',
            valuefiled: "ID",
            textfield: "CodeName",
            onchange: function (index, data) { }
        };
        defaults = $.extend(defaults, settings);

        this.selectValue = function (value) {
            if ($(currentSelect).val() != value) {
                $(currentSelect).val(value);
            }
            var li_List = $(currentSelect).parent().find("li");
            if (li_List && li_List.length > 0) {
                var oldIndex = currentSelect.selectedIndex;
                li_List.eq(oldIndex).trigger("click");
            }
        };
        this.selectValueNoEvent = function (value) {
            if ($(currentSelect).val() != value) {
                $(currentSelect).val(value);
            }
            var li_List = $(currentSelect).parent().find("li");
            if (li_List && li_List.length > 0) {
                var oldIndex = currentSelect.selectedIndex;

                $(currentSelect).parent().find("li").removeClass("selected");
                $(currentSelect).parent().find("li").eq(oldIndex).addClass("selected"); //添加选中样式
                $(currentSelect).parent().find(".select-tit").find("span").text($(currentSelect).parent().find("li").eq(oldIndex).text()); //赋值选中值
            }
        };

        this.selectIndex = function (index) {
            var oldIndex = currentSelect.selectedIndex;
            if (oldIndex != index) {
                $(currentSelect)[0].selectedIndex = index;
            }
        };

        var obj = this.each(function () {
            currentSelect = this;
            $(currentSelect).empty();
            if (defaults.needFirst) {
                $(currentSelect).append(defaults.firstOption);
            }
            $(currentSelect).unbind("change").bind("change", function (e) {
                var index = e.target.selectedIndex;
                var currentData = $("option:selected", currentSelect).data("itemdata");
                defaults.onchange.call(e.target, index, currentData);
            });
            $.each(defaults.data, function (i, item) {
                var optionItem = $('<option value="' + eval("item." + defaults.valuefiled) + '" label="' + eval("item." + defaults.textfield) + '">' + eval("item." + defaults.textfield) + '</option>').data("itemdata", item);
                $(currentSelect).append(optionItem);
            });
        });
        $(currentSelect).data("select", obj);
        $(currentSelect).parent().ruleSingleSelect(); //下拉单选
        return obj;
    };
})(jQuery);