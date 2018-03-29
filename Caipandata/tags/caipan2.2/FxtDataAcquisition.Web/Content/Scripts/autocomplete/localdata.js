var data = [];
$(function () {
    $("#cityid").change(function () {
        var cityId = $(this).find("option:selected").val();
        BindCity(cityId);
    });
    $("#areaid").change(function () {
        var areaId = $(this).find("option:selected").val();
        BindSubArea(areaId);
    });

    $("tr[name='zhan']").hide();
    $("#click_ok").click(function () {
        if ($(this).val() == "") {
            $("tr[name='zhan']").show();
            $(this).text("收起");
            $(this).val("ok");
        } else {
            $("tr[name='zhan']").hide();
            $(this).val("");
            $(this).text("点击更多");

        }
    });
    //日期控件渲染
    $("#startdate, #enddate").datepicker({ format: 'yyyy-mm-dd' });
    $.ajax({
        type: "POST",
        datatype: "Json",
        cache: false,
        url: "/land/land/BindCompan",
        data: {companypecode:""},
        success: function (json) {
            if (json.list != null && json.list.length > 0) {
                for (var i = 0; i < json.list.length; i++) {
                    data.push(json.list[i].ChineseName);
                }
            }
            $("#LandOwnerName").focus().autocomplete(data, {
                max: 10, //列表里的条目数 
                minChars: 0, //自动完成激活之前填入的最小字符 
                width: 132,
                matchContains: true//包含匹配，就是data参数里的数据，是否只要包含文本框里的数据就显示 

            });
            $("#LandUseName").focus().autocomplete(data, {
                max: 10, //列表里的条目数 
                minChars: 0, //自动完成激活之前填入的最小字符 
                width: 132,
                matchContains: true//包含匹配，就是data参数里的数据，是否只要包含文本框里的数据就显示
            });
        }
    });
});