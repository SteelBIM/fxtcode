var NowIsUpdateRight = 0;
var houseX = 114.045082;
var houseY = 22.559325;
var userX = 114.045082;
var userY = 22.559325;
var map = null;
var NowProjectId = 0;
var NowAllotId = 0;
$(function () {
    NowIsUpdateRight = $("#hdIsUpdateRight").val() * 1;
    houseX = $("#hdHouseX").val() * 1;
    houseY = $("#hdHouseY").val() * 1;
    userX = $("#hdUserX").val() * 1;
    userY = $("#hdUserY").val() * 1;
    NowProjectId = $("#hdProjectId").val();
    NowAllotId = $("#hdAllotId").val();
    if (houseX > 0 && houseY > 0) {
        $("#dingwei").html("已定位");
        $("#btnlocation").hide();
        $("#btnSavelocation").hide();
    }
    else {
        $("#dingwei").html("未定位");
    }
    //BindMap();/**初始化绑定地图对象***/
    //HousePosition();/***默认绑定物业位置***/
    $("#btnUserPoint").bind("click", function () {
        $("#MapPointBtn").find(".mapBtn").removeClass("btn-white");
        $("#MapPointBtn").find(".mapBtn").addClass("btn-white");
        $(this).removeClass("btn-white");
        UserPosition();
    });
    $("#btnHousePoint").bind("click", function () {
        $("#MapPointBtn").find(".mapBtn").removeClass("btn-white");
        $("#MapPointBtn").find(".mapBtn").addClass("btn-white");
        $(this).removeClass("btn-white");
        HousePosition();
    });
    $("#searchAroundMap").change(function () {
        var searchAround = $("#searchAroundMap").find("option:selected").val();
        SearchAroundPosition(searchAround);
    });
    $("#btnSubmit").bind("click", function () {
        SubmitData();
    });
    BindProjectInfo();
    /**点击小图显示大图**/
    $("a.projPhoto").fancybox({
        'opacity': true,
        'overlayShow': false,
        'transitionIn': 'elastic',
        'transitionOut': 'none'
    });

    //行政区联动片区
    $("#txt_areaid").change(function () {
        select_subarea();
    });
    $("#dropdown_map").click(function () {
        setTimeout(function () {
            BindMap();/**初始化绑定地图对象***/
        }, 200);
    });
    $("#btnlocation").click(function () {
        Projectlocation();
    });

    $('.chosen-select').chosen({ allow_single_deselect: true });

});
var lng;
var lat
/***开始定位****/
function Projectlocation() {
    function myFun(result) {
        lng = result.center.lng;
        lat = result.center.lat;
        var point = new BMap.Point(result.center.lng, result.center.lat);
        //map.centerAndZoom(point, 17);
        marker = new BMap.Marker(point);
        map.addOverlay(marker);
    }
    if (!lng) {
        var myCity = new BMap.LocalCity();
        myCity.get(myFun);
    }

    map.addEventListener("click", function (e) {
        map.setDefaultCursor("pointer"); //设置鼠标手势
        map.clearOverlays(); //清除除标注
        lng = e.point.lng;
        lat = e.point.lat;
        point = new BMap.Point(lng, lat);
        var marker = new BMap.Marker(point); // 创建标注
        map.addOverlay(marker); // 将标注添加到地图中
    });

    //坐标拾取--确定
    $("#btnSavelocation").click(function () {
        if (lng != null && lng > 0) {
            $.post("/AllotFlowInfo/UploadProjectXY", { Id: NowProjectId, X: lng, Y: lat }, function (data) {
                if (data.result) {
                    AlertFancybox2("定位成功", function () {
                        $("#btnlocation").hide();
                        $("#btnSavelocation").hide();
                        $("#dingwei").html("已定位");
                    });
                } else {
                    alert(data.message);
                }
            })
        }
    });
}

/****初始化绑定地图对象****/
function BindMap() {
    //$("#dropdown14").addClass("active");
    if (map == null) {
        map = new BMap.Map("mapContent");/***创建地图实例  ***/
        map.enableDragging(); //启用地图拖拽事件，默认启用(可不写)
        map.enableScrollWheelZoom(); //启用地图滚轮放大缩小            
        map.disableDoubleClickZoom();//禁用鼠标双击放大
        map.enableKeyboard(); //启用键盘上下左右键移动地图 
        /**向地图中添加缩放控件**/
        var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_LARGE });
        map.addControl(ctrl_nav);
        /**向地图中添加缩略图控件**/
        var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 1 });
        map.addControl(ctrl_ove);
        /**向地图中添加比例尺控件**/
        var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
        map.addControl(ctrl_sca);

        if (!(houseX > 0 && houseY > 0)) {
            function myFun(result) {
                //lng = result.center.lng;
                //lat = result.center.lat;
                var point = new BMap.Point(result.center.lng, result.center.lat);
                map.centerAndZoom(point, 17);
            }
            var myCity = new BMap.LocalCity();
            myCity.get(myFun);
        } else {
            HousePosition();/***默认绑定物业位置***/
        }
    }
    //$("#dropdown14").removeClass("active");
}
/***定位物业位置****/
function HousePosition() {
    if (!(houseX > 0 && houseY > 0)) {
        //map.clearOverlays();
        return;
    }
    /** 创建点坐标  **/
    var point = new BMap.Point(houseX, houseY);
    map.centerAndZoom(point, 17);
    /**"物业位置"图片***/
    var myIcon = new BMap.Icon("http://api.map.baidu.com/images/marker_red_sprite.png", new BMap.Size(150, 80), {
        anchor: new BMap.Size(0, 0)
    });
    marker = new BMap.Marker(point);
    //marker = new BMap.Marker(point, { icon: myIcon });
    map.addOverlay(marker);

    //setTimeout(function () {
    //    map.setCenter(point);
    //}, 200);
}
/****定位周边位置****/
function SearchAroundPosition(searchAround) {
    map.clearOverlays();
    if (!(houseX > 0 && houseY > 0)) {
        map.clearOverlays();
        return;
    }
    /** 创建点坐标  **/
    var point = new BMap.Point(houseX, houseY);
    map.centerAndZoom(point, 17);
    var local = new BMap.LocalSearch(map, {
        renderOptions: { map: map, autoViewport: true }
    });
    local.searchInBounds(searchAround, map.getBounds());
}
/****定位查勘员位置*****/
function UserPosition() {
    if (!(userX > 0 && userY > 0)) {
        map.clearOverlays();
        return;
    }
    /** 创建点坐标  **/
    var point = new BMap.Point(userX, userY);
    map.centerAndZoom(point, 15);
    map.panTo(point);
    /**查勘员范围定位**/
    var circle = new BMap.Circle(point, 300, { fillOpacity: 0.3 });
    map.addOverlay(circle);
}
function BindProjectInfo() {

    /*****绑定企业*********/
    $("#SubmitPanel").find(".txt_pc").each(function () {
        var code = $(this).attr("data-code");
        var codeDom = $("#codeLnkPCList").find(".code_" + code);
        if (codeDom.length > 0) {
            $(this).val(codeDom.html());
        }
    });

    /*****绑定照片类型名称*******/
    for (var i = 0; i < photoTypeCodeListJson.length; i++) {
        var codeObj = photoTypeCodeListJson[i];
        $("#projPhotoList").find(".photoTypeName_" + codeObj.code).html(codeObj.codename);
    }

}
function GetCityNameByProvinceIdAndCityId(provinceId, cityId) {
    var cityName = "";
    for (var i = 0; i < province_json_city.length; i++) {
        var obj = province_json_city[i];
        if (obj.provinceid == provinceId * 1) {
            var citylist = obj.citylist;
            for (var j = 0; j < citylist.length; j++) {
                var obj2 = citylist[j];
                if (cityId == obj2.cityid) {
                    cityName = obj2.cityname;
                    return cityName;
                }
            }
            return cityName;
        }
    }
    return cityName;
}

function GetClassCodeSelect(nowClass, nowCode) {
    var selecthtml = "<select id=\"classcode_" + nowCode + "\"><option value=\"0\" class=\"statusInfo statusInfo_0\" >等级</option>";
    for (var i = 0; i < classCodeListJson.length; i++) {
        var obj = classCodeListJson[i];
        var select = "";
        var nowId = nowClass
        if (nowId == obj.code) {
            select = "selected=\"selected\"";
        }
        var ophtml = "<option value=\"" + obj.code + "\" class=\"statusInfo statusInfo_" + obj.code + "\" " + select + ">" + obj.codename + "</option>";
        selecthtml = selecthtml + ophtml;
    }
    selecthtml = selecthtml + "</select>";
    return selecthtml;
}


/*********提交数据*****/
function SubmitData() {
    VerifyErrorStyleRemove();

    /*楼盘名称*/
    var projectname = $("#txt_projectname");
    if (projectname) {
        if (projectname.val() == "" || projectname.val() == null) {
            VerifyErrorStyle($("#txt_projectname"));
            alert("楼盘名不能为空");
            return false;
        }
    }

    /*楼盘地址*/
    var address = $("#txt_address");
    if (address) {
        if (address.val() == "" || address.val() == null) {
            VerifyErrorStyle($("#txt_address"));
            alert("地址不能为空");
            return false;
        }
    }

    /*行政区*/
    var areaid = $("#txt_areaid");
    if (areaid) {
        if (areaid.val() == "" || areaid.val() == "0") {
            VerifyErrorStyle($("#txt_areaid"));
            alert("请选择行政区");
            return false;
        }
    }

    /*总栋数*/
    var buildingnum = $("#txt_buildingnum");
    if (buildingnum) {
        if (buildingnum.val() == "" || buildingnum.val() == null) {
            VerifyErrorStyle($("#txt_buildingnum"));
            alert("总栋数不能为空");
            return false;
        }
        if (!VeriInt(buildingnum.val())) {
            VerifyErrorStyle($("#txt_buildingnum"));
            alert("总栋数必须为整数");
            return false;
        }
    }

    /*主用途*/
    var purposecode = $("#PurposeCode");
    if (purposecode) {
        if (purposecode.val() == 0) {
            VerifyErrorStyle($("#PurposeCode"));
            alert("请选择主用途");
            return false;
        }
    }

    /*产权形式*/
    var rightcode = $("#RightCode");
    if (rightcode) {
        if (rightcode.val() == 0 || rightcode.val() == "") {
            VerifyErrorStyle($("#RightCode"));
            alert("请选择产权形式");
            return false;
        }
    }

    /*占地面积*/
    var LandArea = $("#LandArea");
    if (LandArea) {
        if (!VeriDecimal(LandArea.val())) {
            VerifyErrorStyle(LandArea);
            alert("占地面积格式填写错误");
            return false;
        }
    }

    /*土地使用年限*/
    var UsableYear = $("#UsableYear");
    if (UsableYear) {
        if (!VeriInt(UsableYear.val())) {
            VerifyErrorStyle(UsableYear);
            alert("土地使用年限必须为整数");
            return false;
        }
    }

    /*建筑面积*/
    var BuildingArea = $("#BuildingArea");
    if (BuildingArea) {
        if (!VeriDecimal(BuildingArea.val())) {
            VerifyErrorStyle(BuildingArea);
            alert("建筑面积格式填写错误");
            return false;
        }
    }

    /*可销售面积*/
    var SalableArea = $("#SalableArea");
    if (SalableArea) {
        if (!VeriDecimal(SalableArea.val())) {
            VerifyErrorStyle(SalableArea);
            alert("可销售面积格式填写错误");
            return false;
        }
    }

    /*容积率*/
    var CubageRate = $("#CubageRate");
    if (CubageRate) {
        if (!VeriDecimal(CubageRate.val())) {
            VerifyErrorStyle(CubageRate);
            alert("容积率格式填写错误");
            return false;
        }
    }

    /*绿化率*/
    var GreenRate = $("#GreenRate");
    if (GreenRate) {
        if (!VeriDecimal(GreenRate.val())) {
            VerifyErrorStyle(GreenRate);
            alert("绿化率格式填写错误");
            return false;
        }
    }

    /*车位数*/
    var txt_parkingnumber = $("#txt_parkingnumber");
    if (txt_parkingnumber) {
        if (!VeriInt(txt_parkingnumber.val())) {
            VerifyErrorStyle(txt_parkingnumber);
            alert("车位数必须为整数");
            return false;
        }
    }

    /*项目均价*/
    var AveragePrice = $("#AveragePrice");
    if (AveragePrice) {
        if (!VeriDecimal(AveragePrice.val())) {
            VerifyErrorStyle(AveragePrice);
            alert("项目均价格式填写错误");
            return false;
        }
    }

    /*总套数*/
    var txt_totalnum = $("#txt_totalnum");
    if (txt_totalnum) {
        if (!VeriInt(txt_totalnum.val())) {
            VerifyErrorStyle(txt_totalnum);
            alert("总套数必须为整数");
            return false;
        }
    }

    /*办公面积*/
    var OfficeArea = $("#OfficeArea");
    if (OfficeArea) {
        if (!VeriDecimal(OfficeArea.val())) {
            VerifyErrorStyle(OfficeArea);
            alert("办公面积格式填写错误");
            return false;
        }
    }

    /*其他用途面积*/
    var OtherArea = $("#OtherArea");
    if (OtherArea) {
        if (!VeriDecimal(OtherArea.val())) {
            VerifyErrorStyle(OtherArea);
            alert("其他用途面积格式填写错误");
            return false;
        }
    }

    /*商业面积*/
    var BusinessArea = $("#BusinessArea");
    if (BusinessArea) {
        if (!VeriDecimal(BusinessArea.val())) {
            VerifyErrorStyle(BusinessArea);
            alert("商业面积格式填写错误");
            return false;
        }
    }

    /*工业面积*/
    var IndustryArea = $("#IndustryArea");
    if (IndustryArea) {
        if (!VeriDecimal(IndustryArea.val())) {
            VerifyErrorStyle(IndustryArea);
            alert("工业面积格式填写错误");
            return false;
        }
    }

    /*开盘均价*/
    var SalePrice = $("#SalePrice");
    if (SalePrice) {
        if (!VeriDecimal(SalePrice.val())) {
            VerifyErrorStyle(SalePrice);
            alert("开盘均价格式填写错误");
            return false;
        }
    }

    /**********楼盘信息************/
    //var projectColDom = $("#SubmitPanel").find(".datproject");
    /**楼盘信息**/
    //var projJson = "{";
    //for (var i = 0; i < projectColDom.length; i++) {
    //    var key = $(projectColDom.get(i)).attr("id");
    //    key = key.replace("txt_", "");
    //    var value = $(projectColDom.get(i)).val();
    //    projJson = projJson + "\"" + key + "\":\"" + value + "\","
    //}
    //projJson = projJson.TrimEnd(',') + "}";

    var developersCompany = encodeURIComponent($("#txt_developerscompany").val());
    var managerCompany = encodeURIComponent($("#txt_managercompany").val());

    //var form = $("form");

    //var templet = { templetname: form.attr("templetname"), fieldgroups: [] };

    //var groups = form.find("fieldset");

    //if (groups != null && groups.length > 0) {

    //    for (var i = 0; i < groups.length; i++) {

    //        var groupdow = $(groups[i]);

    //        var group = { title: groupdow.attr("groupname"), fields: [] }

    //        var fieils = groupdow.find(".datproject");

    //        if (fieils != null && fieils.length > 0) {

    //            for (var f = 0; f < fieils.length; f++) {

    //                var fielddow = $(fieils[f]);

    //                //fieldchoise = fielddow.attr("fieldchoise")

    //                //var choises = [];

    //                //if (fieldchoise) {

    //                //    choisesvalue = fieldchoise.split(',');

    //                //    for (var c = 0; c < choisesvalue.length; c++) {
    //                //        choisesvalue2 = choisesvalue[c].split('|');
    //                //        choises.push({ code: choisesvalue2[1], codename: choisesvalue2[0] });
    //                //    }
    //                //}

    //                var field = {
    //                    fieldname: fielddow.attr("name"),
    //                    type: fielddow.attr("fieldtype"),
    //                    fieldtype: fielddow.attr("fieldtype2"),
    //                    title: fielddow.attr("fieldtitle"),
    //                    maxlength: fielddow.attr("fieldmaxlength"),
    //                    isrequire: fielddow.attr("fieldisrequire"),
    //                    editexttype: fielddow.attr("fieldeditexttype"),
    //                    value: fielddow.val(),
    //                    choise: jQuery.parseJSON( fielddow.attr("fieldchoise")),
    //                    //choise: choises,
    //                }

    //                group.fields.push(field);
    //            }
    //        }


    //        templet.fieldgroups.push(group);
    //    }
    //}

    var data = $("form").serializeArray();
    //data.push({ name: "TempletContent", value: JSON.stringify(templet) })

    $.extendAjax(
                {
                    url: "/AllotFlowInfo/EditProject_SubmitData_Api",
                    data: data,
                    type: "post",
                    dataType: "json"
                },
                function (data) {
                    if (!data.result) {
                        alert(decodeURIComponent(data.message));
                    }
                    else {
                        AlertFancybox2("修改成功", function () {
                            parent.$.fancybox.close();
                        });
                        //parent.location.reload(true);
                    }
                },
               { dom: "#SubmitPanel" });
}
function VerifyErrorStyle(dom) {
    $(dom).css("borderColor", "red").removeClass("vererror").addClass("vererror");
}
function VerifyErrorStyleRemove() {
    $("#SubmitPanel").find(".vererror").css("borderColor", "#B5B5B5");
}
/*****验证带小数类型数据格式******/
function VeriDecimal(val) {
    if (val != null && val != "") {
        var patrn = /^-?\d+\.{0,}\d{0,}$/;

        if (!patrn.exec(val)) {
            return false;
        }
    }
    return true;
}
/*****验证整数数据格式******/
function VeriInt(val) {
    if (val != null && val != "") {
        var patrn = /^\d+$/;

        if (!patrn.exec(val)) {
            return false;
        }
    }
    return true;
}



//片区下拉列表
function select_subarea() {
    var areaid = $("#txt_areaid").val();
    $.get('/AllotFlowInfo/GetSubAreaSelect', { areaid: areaid }, function (data) {
        $("#txt_subareaid").val(null).trigger("change");
        $("#txt_subareaid").html("");
        $.each(data, function (i, item) {
            if (item.Selected) {
                $("#txt_subareaid").append($("<option selected=\"selected\"></option>").val(item.Value).html(item.Text));
            } else {
                $("#txt_subareaid").append($("<option></option>").val(item.Value).html(item.Text));
            }
        });
    });
}

/***上传楼盘图片****/
function UploadProjectImage(projectid) {
    var url = "/AllotFlowInfo/UploadProjectImage?projectId=" + projectid;
    $.fancybox({
        'href': url,
        'width': 750,
        'height': 450,
        'padding': 0,
        'overlayShow': true,
        'autoScale': false,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'type': 'iframe',
        'onClosed': function () {
            window.location.href = "/AllotFlowInfo/EditProject?id=" + NowAllotId +  "&type=1";
        }
    });
}
/***删除楼盘图片****/
function DeleteProjectImage(projectid) {
    var ids = [];
    var checks = $("input[name='ids']:checked");
    if (checks.length == 0) {
        alert("请选择要删除的图片！");
        return;
    }
    checks.each(function () {
        ids.push($(this).val());
    });

    var url = "/AllotFlowInfo/DeleteProjectImage";
    DA_Confirm("提示", "确定要删除选中的图片吗？", function (ok) {
        if (ok) {
            $.post(url, { ids: ids }, function (data) {
                if (data.result) {
                    alert(data.message);
                    window.location.href = "/AllotFlowInfo/EditProject?id=" + NowAllotId + "&type=1";
                } else {

                }
            });
        }
    });
}