$(document).ready(function () {
    $(".page-sidebar>ul>li>a[href='/House/ProjectCase/']").append("<span class='selected'></span>").parent().addClass("active");
    //日期控件渲染
    $("#casedateStart, #casedateEnd,#savedatetime").datepicker({ format: 'yyyy-mm-dd' });


    $("#areaid").change(function () {
        var selectedText = $("#areaid option:selected").text();
        $("#areaname").val(selectedText);
    });

    $("#btnRetrive").click(function () {

        var caseDateFrom = $.trim($("#casedateStart").val());
        var caseDateTo = $.trim($("#casedateEnd").val());

        if (caseDateFrom == "" || caseDateTo == "") {
            alert("案例时间不能为空");
            return false;
        }
        var arrayDate = caseDateFrom.split('-');
        var maxCaseDate = Number(arrayDate[0]) + 1 + arrayDate[1] + arrayDate[2];
        if (caseDateTo > maxCaseDate) {
            alert("案例时间跨度不能大于一年！");
            return false;
        }
    });

    //删除
    $("#delete").unbind();
    $("#delete").click(function () {
        var ids = [];
        var checks = $("input[name='ids']:checked");
        if (checks.length == 0) {
            alert("请选择要删除的数据");
            return;
        }
        if (confirm("是否确定删除？")) {
            checks.each(function () {
                ids.push($(this).val());
            });
            $.ajax({
                type: "POST",
                url: UrlDelete,
                data: { ids: ids },
                traditional: true,
                success: function (data) {
                    if (data.result) {
                        if (data.msg != "") {
                            alert(data.msg);
                        }
                        location.reload();
                    } else {
                        alert(data.msg);
                    }
                }
            });
        }

    });

    //导出
    $("#btnExport").click(function () {
        var caseDateFrom = $.trim($("#casedateStart").val());
        var caseDateTo = $.trim($("#casedateEnd").val());

        if (caseDateFrom == "" || caseDateTo == "") {
            alert("案例时间不能为空");
            return false;
        }
        var arrayDate = caseDateFrom.split('-');
        var maxCaseDate = Number(arrayDate[0]) + 1 + arrayDate[1] + arrayDate[2];
        if (caseDateTo > maxCaseDate) {
            alert("案例时间跨度不能大于一年！");
            return false;
        }

        var areaid = $("#areaid").val();
        var casedateStart = $("#casedateStart").val();
        var casedateEnd = $("#casedateEnd").val();
        var caseTypeCode = $("#caseTypeCode").val();
        var buildingAreaFrom = $.trim($("#buildingAreaFrom").val());
        var buildingAreaTo = $.trim($("#buildingAreaTo").val());
        var purposeCode = $("#purposeCode").val();
        var unitPriceFrom = $.trim($("#unitPriceFrom").val());
        var unitPriceTo = $.trim($("#unitPriceTo").val());
        var key = $("#key").val();
        var buildingTypeCode = $("#buildingTypeCode").val();

        var request = {
            areaid: areaid,
            casedateStart: casedateStart,
            casedateEnd: casedateEnd,
            caseTypeCode: caseTypeCode,
            buildingAreaFrom: buildingAreaFrom,
            buildingAreaTo: buildingAreaTo,
            purposeCode: purposeCode,
            unitPriceFrom: unitPriceFrom,
            unitPriceTo: unitPriceTo,
            key: key,
            buildingTypeCode: buildingTypeCode
        };

        var requestJson = JSON.stringify(request);
        location.href = UrlExport + "?request=" + requestJson;

    });

    //删除重复案例
    $("#btnDeleteSameCase").click(function () {
        var result = [];

        $(this).tb_windowAddFooter({
            sender1: "sender1", //第一个按钮的ID
            name1: "提 交", //第一个按钮的名称
            sender2: "sender2", //第二个按钮的ID
            name2: "取 消", //第二个按钮的名称
            sen1func: function () { //第一个按钮的功能函数
                if (confirm("是否确定删除重复案例？")) {
                    $("#sameCaseForm").submit();
                }
                return false;
            }
        });
    });
});