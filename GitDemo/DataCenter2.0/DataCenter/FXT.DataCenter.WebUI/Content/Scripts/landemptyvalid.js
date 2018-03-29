$(function () {
    var cid_option = 0, areaid_option = 0;
    //规划用途下拉多选
    //$("#planpurpose").multiselect({
    //    header: false,
    //    height: "auto",
    //    minWidth: "140",
    //    selectedList: 4

    //});
    //$(".ui-multiselect").children("span:eq(1)").css({ "color": "black", "font-weight": "100" });
    //$("#planpurpose").multiselect("uncheckAll");
    //$("#submit").click(function () {
    //    var text = "";
    //    var checks = $("#planpurpose").multiselect("getChecked");
    //    for (var i = 0; i < checks.length; i++) {
    //        text += checks[i].value + ",";
    //    }
    //    text = text.substring(0, text.length - 1);
    //    //var text = $(".ui-multiselect").children("span:eq(1)").text();
    //    $("#opValue").val(text);
    //});
    $("#Description").select2();
    $(".select2-container-multi").width(134);

    $("#cityid").change(function () {
        cid_option = $('#cityid option:selected').val();
    });
    $("#areaid").change(function () {
        areaid_option = $('#areaid option:selected').val();
    });
    function checkTime(time) {
        var reg = /^(\d{4})\-(\d{2})\-(\d{2})$/
        if (!reg.test(time)) {
            return false
        }
        else
            return true
    }
    function IsNumber(num) {
        if (!isNaN(num)) return true;
        else return false;
    }
});