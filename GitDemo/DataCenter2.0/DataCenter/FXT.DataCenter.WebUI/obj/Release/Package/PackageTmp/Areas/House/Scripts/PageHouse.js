/*
    CreateName:刘晓博
    CreateTime：2014-11-12
    Description:房号页面交互(批量新增、修改、删除、设置楼层均价、设置楼层差、设置单元(室号)差、计算价格系数、设置价格系数说明)
*/

/*
    tb_arr:选项数组
    add_arr：用于新增记录保存的Ojenct
    update_arr:用于修改记录保存的Ojenct
    delete_arr:用于删除记录保存的Ojenct
*/
var tb_arr = ["HouseName", "BuildArea", "PurposeName", "SubHouse", "SubHouseArea",
    "HouseTypeName", "StructureName", "UnitPrice", "Weight", "FrontName", "SightName",
    "IsEValue", "IsShowBuildingArea", "VDName", "FitmentName", "Cookroom", "Balcony", "Toilet", "NoiseCodeName"],
    add_arr = {},
    update_arr = {},
    delete_arr = {};
//HouseModel
var HouseAttr = {
    HouseId: 0,            //房号ID
    BuildingId: "",         //楼栋ID
    HouseName: "",          //房号名称
    BuildArea: "",          //面积
    PurposeCode: "",        //用途
    SubHouseType: "",       //附属房屋类型
    SubHouseArea: "",       //附属房屋面积
    HouseTypeCode: "",      //户型
    StructureCode: "",      //户型结构
    UnitPrice: "",          //初始单价
    Weight: "",             //价格系数
    FrontCode: "",          //朝向  
    SightCode: "",          //景观
    IsEValue: "",           //是否可估
    IsShowBuildingArea: "", //面积确认
    VDCode: "",             //通风采光
    FitmentCode: "",        //装修
    Cookroom: "",           //有无厨房
    Balcony: "",            //阳台数
    Toilet: "",             //洗手间数
    NoiseCode: "",           //噪音情况
    UnitNo: "",             //单元    
    CityID: "",             //城市ID
    Valid: 1,               //是否有效
    FxtCompanyId: "",       //评估机构Id
    FloorNo: "",            //物理楼层
    NominalFloor: ""        //实际层

};
var HouseObj = {
    CityID: "",               //城市ID
    BuildingId: "",           //楼栋ID
    FxtCompanyId: "",         //评估机构Id
    TotalFloor: 0,            //总楼层
    AveragePrice: "",         //楼栋均价
    PAveragePrice: "",        //楼盘均价
    PriceDetail: "",          //价格系数说明
    ProWeight: "",            //百分比
    FloorPrjceCha: 0,         //楼层价差 
    UnitNoPriceCha: 0,        //单元价差   
    //加载初始化
    InitLoad: function (buildingId, cityId, fxtcompanyId, totalFloor, pAveragePrice, averagePrice, priceDetail, proWeight) {
        this.BuildingId = buildingId;//楼栋Id
        this.CityID = cityId;//城市Id
        this.FxtCompanyId = fxtcompanyId;
        this.TotalFloor = totalFloor;
        this.PAveragePrice = pAveragePrice;
        this.AveragePrice = averagePrice;
        this.PriceDetail = priceDetail;
        this.ProWeight = proWeight;
    },
    /*
     添加楼层
     floorNum:楼层号
    */
    AddFloorNo: function (floorNum) {
        if (ValidFloorNum(floorNum)) {
            var num = parseInt(floorNum);
            if (num < 1)//地下层待实现
            {
                return false;
            }
            if (num <= this.TotalFloor) {
                alert("楼层号已经存在");
                return false;
            }
            FoeachAddFloorNo(floorNum, this.TotalFloor);//添加楼层、面积、用途。。。。。。
            this.TotalFloor = floorNum;//重置总楼层数
            loadFloor(floorNum);//重新绑定楼层下拉

        }
    },
    /*
     删除楼层
     floorNum:楼层号
   */
    DelFloorNo: function (floorNum) {
        if (ValidFloorNum(floorNum)) {
            var num = parseInt(floorNum);
            if (num < 1) {
                return false;//地下层  待实现
            }
            if (num > this.TotalFloor) {
                alert("不存在该楼层号");
                return false;
            } else {
                FoeachDelFloorNo(floorNum, this.TotalFloor);//删除楼层、面积、用途。。。。。。
                this.TotalFloor = floorNum;//重置总楼层数
                loadFloor(floorNum);//重新绑定楼层下拉
            }
        }
    },
    /*
     新增房号;
     unitNo:单元;A
     houseName:室号;01
     房号=单元+楼层+室号(A101,A201,A301)
    */
    AddHouse: function (unitNo, houseName) {
        if (unitNo == "" && houseName == "") {
            alert("单元室号不能为空");
            return false;
        }
        FoeachAddHouse(unitNo, houseName);//添加房号
        loadUnitNo();//加载单元
    },
    /* 
     删除房号;
     unitNo:单元;A
     houseName:室号;01
     */
    DelHouse: function (unitNo, houseName) {
        FoeachDelHouse(unitNo, houseName);//删除房号
        loadUnitNo();//加载单元
    },
    /* 
    删除房号;
    根据房号名称删除房号
    */
    DelHouseName: function (floor, unitNo, houseName) {
        houseName = GetTrim(houseName);//输入的房号
        if (houseName == null || houseName == "") {
            alert("房号不能为空");
            return false;
        }
        var table = $("#tab_HouseName").next("table");
        var hname = GetTrim($(table).find("tbody>tr").eq(GetInt32(floor) - 1).find("td").eq(GetInt32(unitNo)).text());//实际房号
        if (hname.toUpperCase() != houseName.toUpperCase()) {
            alert("房号不存在");
            return false;
        }
        floor = GetInt32(floor);//物理楼层
        var nominalFloor = $(table).find("tbody>tr").eq(floor - 1).find("td").eq(1).text();//名义层（实际层）
        var fxtcompanyId = $(table).find("tbody>tr").eq(floor - 1).find("td").eq(GetInt32(unitNo)).attr("fxtcompanyid");//当前房号的fxtcompanyid
        HouseAttr = {
            FloorNo: floor,
            NominalFloor: GetTrim(nominalFloor),
            HouseName: houseName,                         //房号名称
            BuildingId: HouseObj.BuildingId,            //楼栋ID
            CityID: HouseObj.CityID,                    //城市ID
            FxtCompanyId: GetTrim(fxtcompanyId)         //当前房号的fxtcompanyid
        };
        /*
           创建一个房号
           end
       */
        if (add_arr != null && add_arr[houseName] != undefined) {
            delete add_arr[houseName];//如果是尚未提交的新增房号直接移除
        } else {
            if (delete_arr != null && delete_arr[houseName] != undefined) {
                delete delete_arr[houseName];//如果是尚未提交的新增房号直接移除
            }
            delete_arr[houseName] = HouseAttr;//已经存在的房号
        }
        for (var i = 0; i < tb_arr.length; i++) {
            var tobj = GetCurrTableByTId("#tab_" + tb_arr[i]);//获取table
            $(tobj).find("tbody>tr").eq(floor - 1).find("td").eq(GetInt32(unitNo)).text("");
        }
        $("#btn_delHouseName").val("");
    },
    /*
      设置
      floor:从多少层
      floorTo:到多少层
      unitNo:从多少单元
      unitNoTo:到多少单元
      setValue:值
      tableModule:table模块
    */
    CommonSetValue: function (floor, floorTo, unitNo, unitNoTo, setValue, tableModule, point) {

        //初始单价tab项，
        if (point) {
            if (isNaN(point)) {
                alert("百分比必须为数字类型");
                return false;
            }
            setValue = (setValue * (point / 100)).toFixed(2);
        }

        if (GetTrim(setValue) == null || GetTrim(setValue) == "") {
            alert("不能输入空值");
            return false;
        }
        if (tableModule == "#tab_BuildArea" || tableModule == "#tab_SubHouseArea" || tableModule == "#tab_UnitPrice" || tableModule == "#tab_Weight" || tableModule == "#tab_Balcony" || tableModule == "#tab_Toilet") {
            if (isNaN(setValue)) {
                alert("请输入数字");
                return false;
            }
        }
        //比较floor和floorTo大小、unitNo和unitNoTo并且互换
        var tempfloor, tempunitNo;//中间变量
        if (GetInt32(floor) > GetInt32(floorTo)) {
            tempfloor = GetInt32(floor);
            floor = GetInt32(floorTo);
            floorTo = tempfloor;
        }
        if (GetInt32(unitNo) > GetInt32(unitNoTo)) {
            tempunitNo = GetInt32(unitNo);
            unitNo = GetInt32(unitNoTo);
            unitNoTo = tempunitNo;
        }
        FoeachCommonSetValue(floor, floorTo, unitNo, unitNoTo, setValue, tableModule);
    },
    /*
        设置楼层均价
        avgpriceFloor:要设置均价的楼层层
    */
    SetFloorAvgPrice: function (avgpriceFloor) {
        if (this.AveragePrice == null || this.AveragePrice == "") {
            alert("楼栋均价不正确，请先到“价格系数”页设置项目均价和楼栋均价!");
            return false;
        }
        var tobj = GetCurrTableByTId("#tab_UnitPrice"),//获取table
        thead = GetTableHead(tobj),//获取table的单元列(包含列:物理层和实际层这两列)
        htobj = GetCurrTableByTId("#tab_HouseName");//获取房号table项
        for (var i = 2; i < $(thead).length; i++) {//i=2(物理层和实际层除外)
            var houName = GetTrim($(htobj).find("tbody>tr").eq(avgpriceFloor - 1).find("td").eq(i).text());
            if (houName != null && houName != "") {
                $(tobj).find("tbody>tr").eq(avgpriceFloor - 1).find("td").eq(i).text(this.AveragePrice);
                SaveUpdate(houName, tobj, avgpriceFloor, i, "#tab_UnitPrice", this.AveragePrice);
            }
        }
    },
    /*
       设置楼层差
      floor:从多少层
      floorTo:到多少层
      jiajian:1：递增;0：递减
      val：1：递增;0：递减[的百分比]
   */
    SetFloorCha: function (floor, floorTo, jiajian, val) {
        SetFloorCha(floor, floorTo, jiajian, val);
    },
    /*
       设置单元差
      floor:从多少单元
      floorTo:到多少单元
      jiajian:1：递增;0：递减
      val：1：加;0：减[的百分比]
   */
    SetUnitnoCha: function (unitNo, unitNoTo, jiajian, val) {
        SetUnitNoCha(unitNo, unitNoTo, jiajian, val);
    },
    //设置价格系数
    SetPriceWeight: function () {
        SetPriceWeight();
    },
    Submit: function (thisobj) {
        $(thisobj).attr({ "disabled": "disabled" });
        var dataparam = {};
        /*
        参数模板
        {"addHouse":[{"BuildingId":"123"},{"BuildingId":"A"}],
           "updateHouse":[{"BuildingId":"111"},{"BuildingId":"B"}],
           "deleteHouse":[{"BuildingId":"8"},{"BuildingId":"C"}]
        };
        */
        if ($.isEmptyObject(add_arr) && $.isEmptyObject(update_arr) && $.isEmptyObject(delete_arr)) {
            alert("尚无数据提交");
            $(thisobj).removeAttr("disabled");
            return false;
        }
        if (!$.isEmptyObject(add_arr)) {
            dataparam.addHouse = JsonData(add_arr, 1);
        };
        if (!$.isEmptyObject(update_arr)) {
            dataparam.updateHouse = JsonData(update_arr, 1);
        }
        if (!$.isEmptyObject(delete_arr)) {
            dataparam.deleteHouse = JsonData(delete_arr, 0);
        }
        $.ajax({
            url: "/House/House/EndityHouse",
            data: JSON.stringify(dataparam),
            dataType: "Json",
            contentType: "application/json",
            type: "POST",
            cache: false,
            success: function (jsondata) {
                if (!jsondata.result) {
                    alert(jsondata.msg);
                }
                window.location.href = "/House/House/Index?ProjectId=" + projectId + "&BuildId=" + HouseObj.BuildingId + "&FxtCompanyId=" + HouseObj.FxtCompanyId + "&TotalFloor=" + HouseObj.TotalFloor;
            },
            error: function (jsondata) {
                $(thisobj).removeAttr("disabled");
            }
        });
    },
    ChangeValue: function (floor, unitNoLen, id) {
        var table = $("#tab_HouseName").next("table");
        var houseName = GetTrim($(table).find("tbody>tr").eq(GetInt32(floor) - 1).find("td").eq(GetInt32(unitNoLen)).text());
        if (houseName == null || houseName == "") {
            $(id).val("");
        } else {
            $(id).val(GetTrim(houseName));
        }
    }

}
//json处理
function JsonData(obj, valid) {
    var jsonstr = [];
    for (var key in obj) {
        var json = {
            "BuildingId": obj[key]["BuildingId"], "HouseName": obj[key]["HouseName"],
            "BuildArea": obj[key]["BuildArea"], "PurposeCode": obj[key]["PurposeCode"],
            "SubHouseType": obj[key]["SubHouseType"], "SubHouseArea": obj[key]["SubHouseArea"],
            "HouseTypeCode": obj[key]["HouseTypeCode"], "StructureCode": obj[key]["StructureCode"],
            "UnitPrice": obj[key]["UnitPrice"], "Weight": obj[key]["Weight"],
            "FrontCode": obj[key]["FrontCode"], "SightCode": obj[key]["SightCode"],
            "IsEValue": obj[key]["IsEValue"], "IsShowBuildingArea": obj[key]["IsShowBuildingArea"],
            "VDCode": obj[key]["VDCode"], "FitmentCode": obj[key]["FitmentCode"],
            "Cookroom": obj[key]["Cookroom"], "Balcony": obj[key]["Balcony"],
            "Toilet": obj[key]["Toilet"], "NoiseCode": obj[key]["NoiseCode"], "FloorNo": obj[key]["FloorNo"],
            "UnitNo": obj[key]["UnitNo"], "CityID": obj[key]["CityID"],
            "Valid": valid, "FxtCompanyId": obj[key]["FxtCompanyId"],
            "NominalFloor": obj[key]["NominalFloor"]
        };
        jsonstr.push(json);
    }
    return jsonstr;
}
//设置价格系数
function SetPriceWeight() {

    if (window.confirm("价格系数是由房号的单价与项目均价求得的，请确认房号单价和楼盘均价设置无误，是否继续？")) {
        var projectAvgPrice = GetTrim(HouseObj.PAveragePrice);
        if (projectAvgPrice == null || projectAvgPrice == "") {
            alert("项目均价不能为空");
            return false;
        }
        if (parseInt(projectAvgPrice) == 0) {
            alert("项目均价不能零");
            return false;
        }
        var tobj = GetCurrTableByTId("#tab_UnitPrice"),//获取初始单价tableItem
    hobj = GetCurrTableByTId("#tab_HouseName"),//获取房号tableItem
    weithobj = GetCurrTableByTId("#tab_Weight"),//获取房号tableItem
thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
        for (var i = 0; i < HouseObj.TotalFloor; i++) {
            for (var j = 2; j < $(thead).length; j++) {
                var houName = GetTrim($(hobj).find("tbody>tr").eq(i).find("td").eq(j).text()),//房号
                 price = GetTrim($(tobj).find("tbody>tr").eq(i).find("td").eq(j).text());//单价
                if ((houName != null && houName != "") && (price != null && price != "")) {
                    var weight = GetTrim(parseFloat(parseFloat(price) / parseFloat(projectAvgPrice)).toFixed(4));
                    $(weithobj).find("tbody>tr").eq(i).find("td").eq(j).text(weight);
                    SaveUpdate(houName, tobj, (i + 1), j, "#tab_Weight", weight);
                }
            }
        }

    }
}
//设置单元差
function SetUnitNoCha(unitNo, unitNoTo, jiajian, val) {
    if (val == null || val == "" || val == 0) {
        alert("百分比不能为空");
        return false;
    }
    if (isNaN(val)) {
        alert("百分比只能是数字");
        return false;
    }
    val = GetTrim(val); unitNo = GetInt32(unitNo); unitNoTo = GetInt32(unitNoTo);
    var tobj = GetCurrTableByTId("#tab_UnitPrice"),//获取初始单价tableItem
        hobj = GetCurrTableByTId("#tab_HouseName"),//获取房号tableItem
    thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
    for (var i = 0; i < HouseObj.TotalFloor; i++) {
        var j;
        if (unitNo > unitNoTo) {
            for (j = unitNo; j >= unitNoTo; j--) {
                UnitNoPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i);
            }
        } else {
            for (j = unitNo; j <= unitNoTo; j++) {
                UnitNoPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i);
            }
        }
        HouseObj.UnitNoPriceCha = 0;
    }
}
//单元差增或减
function UnitNoPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i) {


    var houName = GetTrim($(hobj).find("tbody>tr").eq(i).find("td").eq(j).text());
    var $td = $(tobj).find("tbody>tr").eq(i).find("td").eq(j);
    var price = 0;

    //var price = GetTrim($(tobj).find("tbody>tr").eq(i).find("td").eq(j).text());
    //var houName = GetTrim($(hobj).find("tbody>tr").eq(i).find("td").eq(j).text());

    //if (price != null && price != "")//第floor(新增范围的首层)或者房号为空
    //{
    //    if (HouseObj.UnitNoPriceCha <= 0)//第一次赋值
    //    {
    //        HouseObj.UnitNoPriceCha = price;
    //    }
    //}
    //if (HouseObj.UnitNoPriceCha > 0) {
    //    if (jiajian == 0) {
    //        price = (parseFloat(HouseObj.UnitNoPriceCha) - (parseFloat(HouseObj.UnitNoPriceCha) * (val / 100))).toFixed(2);
    //        //price = (parseFloat(HouseObj.UnitNoPriceCha) - 100).toFixed(2);
    //    } else if (jiajian == 1) {
    //        price = (parseFloat(HouseObj.UnitNoPriceCha) + (parseFloat(HouseObj.UnitNoPriceCha) * (val / 100))).toFixed(2);
    //        //price = (parseFloat(HouseObj.UnitNoPriceCha) + 100).toFixed(2);
    //    }
    //    if (houName != null && houName != "") {
    //        $(tobj).find("tbody>tr").eq(i).find("td").eq(j).text(price);
    //        SaveUpdate(houName, tobj, (i + 1), j, "#tab_UnitPrice", price);
    //    }
    //    HouseObj.UnitNoPriceCha = price;
    //}

    if (jiajian == 0) {
        price = (parseFloat($td.text()) - (parseFloat($td.text()) * (val / 100))).toFixed(2);
    } else if (jiajian == 1) {
        price = (parseFloat($td.text()) + (parseFloat($td.text()) * (val / 100))).toFixed(2);
    }

    if (houName) {
        $td.text(price);
        SaveUpdate(houName, tobj, (i + 1), j, "#tab_UnitPrice", price);
    }

}
//设置楼层差
function SetFloorCha(floor, floorTo, jiajian, val) {
    if (val == null || val == "" || val == 0) {
        alert("百分比不能为空");
        return false;
    }
    if (isNaN(val)) {
        alert("百分比只能是数字");
        return false;
    }
    val = GetTrim(val); floor = GetInt32(floor); floorTo = GetInt32(floorTo);
    var tobj = GetCurrTableByTId("#tab_UnitPrice"),//获取初始单价tableItem
        hobj = GetCurrTableByTId("#tab_HouseName"),//获取房号tableItem
    thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
    for (var i = 2; i < $(thead).length; i++) {
        var j;
        if (floor > floorTo) {
            for (j = floor; j >= floorTo; j--) {
                FloorPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i, floor);
            }
        } else {
            for (j = floor; j <= floorTo; j++) {
                FloorPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i, floor);
            }
        }
        HouseObj.FloorPrjceCha = 0;
    }
}
//楼层差递增或者递减
function FloorPriceAddOrSubtract(tobj, hobj, jiajian, val, j, i, startingFloor) {

    var price = GetTrim($(tobj).find("tbody>tr").eq(j - 1).find("td").eq(i).text());
    var houName = GetTrim($(hobj).find("tbody>tr").eq(j - 1).find("td").eq(i).text());

    if (price != null && price != "")//第floor(新增范围的首层)或者房号为空
    {
        if (HouseObj.FloorPrjceCha <= 0)//第一次赋值
        {
            HouseObj.FloorPrjceCha = price;
        }
    }

    if (j == startingFloor) return;//忽略均价楼层

    if (HouseObj.FloorPrjceCha > 0) {
        if (jiajian == 0) {
            price = (parseFloat(HouseObj.FloorPrjceCha) - (parseFloat(HouseObj.FloorPrjceCha) * (val / 100))).toFixed(2);
            //price = (parseFloat(HouseObj.FloorPrjceCha) - 100).toFixed(2);
        } else if (jiajian == 1) {
            price = (parseFloat(HouseObj.FloorPrjceCha) + (parseFloat(HouseObj.FloorPrjceCha) * (val / 100))).toFixed(2);
            //price = (parseFloat(HouseObj.FloorPrjceCha) + 100).toFixed(2);
        }

        if (houName != null && houName != "") {
            $(tobj).find("tbody>tr").eq(j - 1).find("td").eq(i).text(price);
            SaveUpdate(houName, tobj, j, i, "#tab_UnitPrice", price);
        }
        HouseObj.FloorPrjceCha = price;
    }
}
//验证楼层
function ValidFloorNum(floorNum) {
    floorNum = GetTrim(floorNum);
    if (floorNum == null || floorNum == "") {
        alert("楼层号不能为空");
        return false;
    }
    if (isNaN(floorNum)) {
        alert("楼层号只能是数字");
        return false;
    }
    return true;
}
//验证单元号是否存在z
function ValidFloorUnitNo(unitNo) {
    var tobj = GetCurrTableByTId("#tab_" + tb_arr[0]);//获取table
    var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
    var flag = true;
    for (var i = 2; i < $(thead).length; i++) {
        if (unitNo.toUpperCase() == GetTrim($(thead[i]).text()).toUpperCase())//转换小写
        {
            flag = false;
            break;
        }
    }
    if (!flag) {
        return false;
    } else {
        return flag;
    }

}
function ValidFloorUnitNo(unitNo, houseName) {
    var tobj = GetCurrTableByTId("#tab_" + tb_arr[0]);//获取table
    var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
    var flag = true;
    for (var i = 2; i < $(thead).length; i++) {
        if ((unitNo.toUpperCase()) + (houseName.toUpperCase()) == GetTrim($(thead[i]).text()).toUpperCase())//转换小写
        {
            flag = false;
            break;
        }
    }
    if (!flag) {
        return false;
    } else {
        return flag;
    }
}
//根据单元和室号获取th的索引
function GetThIndex(unitNo, houseName) {
    var tobj = GetCurrTableByTId("#tab_" + tb_arr[0]);//获取table
    var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
    var index = 0;
    for (var i = 2; i < $(thead).length; i++) {
        if (unitNo.toUpperCase() + houseName.toUpperCase() == GetTrim($(thead[i]).text()).toUpperCase()) {
            index = i;
            break;
        }
    }
    return index;

}
/*
Operation:循环添加楼层
attr:房号、面积、用途、附属房屋类型、附属房屋面积、户型、
     户型结构、初始单价、价格系数、朝向、景观、可估价、
     面积确认、通风采光、装修、是否有厨房、阳台数、洗手间数
*/
function FoeachAddFloorNo(floorNum, totalFloor) {
    for (var a = 0; a < tb_arr.length; a++) {
        var tobj = GetCurrTableByTId("#tab_" + tb_arr[a]);//获取table
        var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
        for (var i = 1 ; i <= GetInt32(floorNum) - GetInt32(totalFloor) ; i++) {
            var tr = "<tr>";
            for (var j = 0; j < $(thead).length; j++) {
                if (j == 0 || j == 1) {
                    tr += "<td>";
                    tr += GetInt32(totalFloor) + i;
                    tr += "</td>";
                } else {
                    var houName = GetTrim((GetInt32(totalFloor) + i) + GetTrim((thead).eq(j).text()));
                    tr += "<td>";
                    if (a == 0) {//房号项
                        tr += houName;//房号=单元+楼层+室号(A101,A201,A301)
                        /*
                     创建一个房号
                     begin
                 */
                        HouseAttr = {
                            FloorNo: GetTrim(GetInt32(totalFloor) + i),      //物理楼层
                            NominalFloor: GetTrim(GetInt32(totalFloor) + i), //名义层（实际层）
                            HouseName: houName,                     //房号名称
                            BuildingId: HouseObj.BuildingId,        //楼栋ID
                            CityID: HouseObj.CityID,                //城市ID
                            FxtCompanyId: HouseObj.FxtCompanyId,    //评估机构ID
                            BuildArea: "",                          //面积
                            PurposeCode: "",                        //用途
                            SubHouseType: "",                       //附属房屋类型
                            SubHouseArea: "",                       //附属房屋面积
                            HouseTypeCode: "",                      //户型
                            StructureCode: "",                      //户型结构
                            UnitPrice: "",                          //初始单价
                            Weight: "",                             //价格系数
                            FrontCode: "",                          //朝向  
                            SightCode: "",                          //景观
                            IsEValue: "",                           //是否可估
                            IsShowBuildingArea: "",                 //面积确认
                            VDCode: "",                             //通风采光
                            FitmentCode: "",                        //装修
                            Cookroom: "",                           //是否有厨房
                            Balcony: "",                            //阳台数
                            Toilet: "",                             //洗手间数
                            NoiseCode: "",                           //噪音情况
                            UnitNo: GetTrim($(thead).eq(j).text()),  //单元    
                            Valid: 1                                //是否有效
                        };
                        /*
                            创建一个房号
                            end
                        */
                        add_arr[houName] = HouseAttr;//添加到数据字典
                    } else {
                        tr += "";
                    }
                    tr += "</td>";
                }
            }
            tr += "/</tr>";
            $(tobj).find("tbody").append(tr);
        }
    }
}
/*
Operation:循环删除楼层
attr:房号、面积、用途、附属房屋类型、附属房屋面积、户型、
     户型结构、初始单价、价格系数、朝向、景观、可估价、
     面积确认、通风采光、装修、是否有厨房、阳台数、洗手间数
*/
function FoeachDelFloorNo(floorNum, totalFloor) {
    for (var a = 0; a < tb_arr.length; a++) {
        var tobj = GetCurrTableByTId("#tab_" + tb_arr[a]);//获取table
        var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
        for (var j = 2; j < $(thead).length; j++) {
            var tr = $(tobj).find("tbody>tr").eq(GetInt32(floorNum - 1));
            if (a == 0)//房号项
            {
                var houName = GetTrim(tr.find("td").eq(j).text());//房号名称
                if (add_arr != null && add_arr[houName] != undefined) {
                    delete add_arr[houName];//如果是尚未提交的新增房号直接移除
                } else {
                    if (houName != null && houName != "") {//房号不能为空
                        /*
                             创建一个房号
                             begin
                         */

                        HouseAttr = {
                            FloorNo: GetInt32(floorNum),                //物理楼层
                            NominalFloor: GetTrim(tr.find("td").eq(1).text()),   //名义层（实际层）
                            HouseName: houName,                         //房号名称
                            BuildingId: HouseObj.BuildingId,            //楼栋ID
                            CityID: HouseObj.CityID,                    //城市ID
                            FxtCompanyId: GetTrim(tr.find("td").eq(j).attr("fxtcompanyid"))         //当前房号的fxtcompanyid
                        };
                        /*
                           创建一个房号
                           end
                       */
                        if (delete_arr != null && delete_arr[houName] != undefined) {
                            delete delete_arr[houName];//如果是尚未提交的新增房号直接移除
                        }
                        delete_arr[houName] = HouseAttr;//已经存在的房号
                    }
                }
            }
            tr.find("td").eq(j).text("");
        }
    }
}
/*
Operation:循环添加房号
*/
function FoeachAddHouse(unitNo, houseName) {
    if (unitNo == null || unitNo == "") {
        //alert("单元不能为空");
        //return false;
        unitNo = "";
    }
    if (ValidFloorUnitNo(unitNo, houseName)) {
        for (var a = 0; a < tb_arr.length; a++) {
            var tobj = GetCurrTableByTId("#tab_" + tb_arr[a]);//获取table
            $(tobj).find("thead>tr").append("<th fanghao='" + houseName + "'>" + (unitNo + houseName) + "</th>");//添加单元
            tobj = GetCurrTableByTId("#tab_" + tb_arr[a]);//重新获单元信息
            var thead = GetTableHead(tobj);//获取table的单元列(包含列:物理层和实际层这两列)
            for (var i = 0; i < HouseObj.TotalFloor; i++) {
                var tr = "";
                if (a == 0) {//房号项 房号=单元+楼层+室号(A101,A201,A301)
                    var houName = unitNo + (i + 1) + houseName;
                    tr += "<td>";
                    tr += houName;
                    tr += "</td>";
                    /*
                        新增房号
                        begin
                    */
                    HouseAttr = {
                        FloorNo: GetTrim(i + 1),      //物理楼层
                        NominalFloor: GetTrim(i + 1), //名义层（实际层）
                        HouseName: houName,                     //房号名称
                        BuildingId: HouseObj.BuildingId,        //楼栋ID
                        CityID: HouseObj.CityID,                //城市ID
                        FxtCompanyId: HouseObj.FxtCompanyId,    //评估机构ID
                        BuildArea: "",                          //面积
                        PurposeCode: "",                        //用途
                        SubHouseType: "",                       //附属房屋类型
                        SubHouseArea: "",                       //附属房屋面积
                        HouseTypeCode: "",                      //户型
                        StructureCode: "",                      //户型结构
                        UnitPrice: "",                          //初始单价
                        Weight: "",                             //价格系数
                        FrontCode: "",                          //朝向  
                        SightCode: "",                          //景观
                        IsEValue: "",                           //是否可估
                        IsShowBuildingArea: "",                 //面积确认
                        VDCode: "",                             //通风采光
                        FitmentCode: "",                        //装修
                        Cookroom: "",                           //是否有厨房
                        Balcony: "",                            //阳台数
                        Toilet: "",                             //洗手间数
                        NoiseCode: "",                           //噪音情况
                        UnitNo: GetTrim((GetTrim(unitNo) + GetTrim(houseName))),//单元    
                        Valid: 1                                //是否有效
                    };
                    /*
                        新增房号
                        end
                    */
                    add_arr[houName] = HouseAttr;//保存到数据字典
                } else {
                    tr += "<td></td>";
                }
                $(tobj).find("tbody tr").eq(i).append(tr);
            }
        }
    } else {
        alert("已经存在相同的单元");
        return false;
    }
}
/*
Operation:循环删除房号
*/
function FoeachDelHouse(unitNo, houseName) {
    //if (unitNo == null || unitNo == "") {
    //    alert("单元不能为空");
    //    return false;
    //}
    if (ValidFloorUnitNo(unitNo, houseName)) {
        alert("该单元不存在");
        return false;
    } else {
        var thIndex = GetThIndex(unitNo, houseName);
        for (var a = 0; a < tb_arr.length; a++) {
            var tobj = GetCurrTableByTId("#tab_" + tb_arr[a]);//获取table
            for (var i = 0; i < HouseObj.TotalFloor; i++) {
                if (a == 0)//房号项
                {
                    var houName = GetTrim($(tobj).find("tbody>tr").eq(i).find("td").eq(thIndex).text());
                    if (add_arr != null && add_arr[houName] != undefined) {
                        delete add_arr[houName];//如果是尚未提交的新增房号直接移除
                    } else {
                        if (houName != null && houName != "") {//房号不能为空
                            /*
                        创建一个房号
                        begin
                    */
                            HouseAttr = {
                                FloorNo: GetInt32(i + 1),                     //物理楼层
                                NominalFloor: GetTrim(GetTrim(unitNo) + GetTrim(houseName)),                 //名义层（实际层）
                                HouseName: houName,                         //房号名称
                                BuildingId: HouseObj.BuildingId,            //楼栋ID
                                CityID: HouseObj.CityID,                    //城市ID
                                FxtCompanyId: GetTrim($(tobj).find("tbody>tr").eq(i).find("td").eq(thIndex).attr("fxtcompanyid"))         //当前房号的fxtcompanyid
                            };
                            /*
                                创建一个房号
                                end
                            */
                            if (delete_arr != null && delete_arr[houName] != undefined) {
                                delete delete_arr[houName];
                            }
                            delete_arr[houName] = HouseAttr;//已经存在的房号
                        }
                    }
                }
                $(tobj).find("tbody>tr").eq(i).find("td").eq(thIndex).remove();
            }
            $(tobj).find("thead>tr>th").eq(thIndex).remove();

        }
    }
}
/*
Operation:设置
面积、用途、附属房屋类型、附属房屋面积、
户型、户型结构、初始单价、价格系数、朝向、
景观、可估价、面积确认、通风采光、装修、是否有厨房、阳台数、洗手间数
*/
function FoeachCommonSetValue(floor, floorTo, unitNo, unitNoTo, newVal, tableModule) {
    var tobj = GetCurrTableByTId(tableModule);//获取table
    var houtable = GetCurrTableByTId("#tab_HouseName");//获取房号Item
    for (var i = GetInt32(floor) ; i <= GetInt32(floorTo) ; i++) {
        for (var j = GetInt32(unitNo) ; j <= GetInt32(unitNoTo) ; j++) {
            var houseName = GetTrim($(houtable).find("tbody>tr").eq(i - 1).find("td").eq(j).text());//房号名称
            if (houseName != null && houseName != "") {//存在该房号
                $(tobj).find("tbody>tr").eq(i - 1).find("td").eq(j).text(newVal);
                SaveUpdate(houseName, tobj, i, j, tableModule, GetTrim(newVal));
            }
        }
    }
}
//获取table(取得一个包含匹配的元素集合中每一个元素紧邻的后面同辈元素的元素集合)
function GetCurrTableByTId(id) {
    return $(id).next("table");
}
//获取table的单元列(包含列:物理层和实际层这两列)
function GetTableHead(table) {
    return $(table).find("thead>tr>th");
}
//转换成整数
function GetInt32(obj) {
    try {
        return parseInt(obj);
    } catch (e) {
        alert(e.message);
    }
    return 0;
}
//去除空格
function GetTrim(obj) {
    return $.trim(obj);
}
/*
    保存更新
    houseName:房号
    tobj：当前item的tableId
    i：楼层
    j：单元(室号)
    tableModule:当前模块项
    newVal:设置的值
*/
function SaveUpdate(houseName, tobj, i, j, tableModule, newVal) {
    var ba = GetCurrTableByTId("#tab_BuildArea"), pn = GetCurrTableByTId("#tab_PurposeName"),
     sh = GetCurrTableByTId("#tab_SubHouse"), sha = GetCurrTableByTId("#tab_SubHouseArea"),
     htn = GetCurrTableByTId("#tab_HouseTypeName"), stn = GetCurrTableByTId("#tab_StructureName"),
     up = GetCurrTableByTId("#tab_UnitPrice"), w = GetCurrTableByTId("#tab_Weight"),
     fn = GetCurrTableByTId("#tab_FrontName"), sn = GetCurrTableByTId("#tab_SightName"),
     ie = GetCurrTableByTId("#tab_IsEValue"), isba = GetCurrTableByTId("#tab_IsShowBuildingArea"),
     vdn = GetCurrTableByTId("#tab_VDName"), fin = GetCurrTableByTId("#tab_FitmentName"),
     cr = GetCurrTableByTId("#tab_Cookroom"), b = GetCurrTableByTId("#tab_Balcony"),
     t = GetCurrTableByTId("#tab_Toilet"), nn = GetCurrTableByTId("#tab_NoiseCodeName");

    if (add_arr[houseName] != null && add_arr[houseName] != undefined) {
        AddOrUpdate(add_arr[houseName], tableModule, newVal);
        delete update_arr[houseName];
    } else {
        if (update_arr[houseName] == null && update_arr[houseName] == undefined) {
            HouseAttr = {
                FloorNo: i,//物理楼层
                UnitNo: GetTrim($(tobj).find("thead>tr").eq(0).find("th").eq(j).text()),//单元
                HouseName: houseName,                   //房号名称
                BuildingId: HouseObj.BuildingId,        //楼栋ID
                CityID: HouseObj.CityID,                //城市ID
                FxtCompanyId: GetTrim($(ba).find("tbody>tr").eq(i - 1).find("td").eq(j).attr("fxtcompanyid")),     //当前房号的评估机构ID
                BuildArea: GetTrim($(ba).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),          //面积
                PurposeCode: GetTrim($(pn).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),        //用途
                SubHouseType: GetTrim($(sh).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),       //附属房屋类型
                SubHouseArea: GetTrim($(sha).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),       //附属房屋面积
                HouseTypeCode: GetTrim($(htn).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),      //户型
                StructureCode: GetTrim($(stn).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),      //户型结构
                UnitPrice: GetTrim($(up).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),          //初始单价
                Weight: GetTrim($(w).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),             //价格系数
                FrontCode: GetTrim($(fn).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),          //朝向  
                SightCode: GetTrim($(sn).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),          //景观
                IsEValue: GetTrim($(ie).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),           //是否可估
                IsShowBuildingArea: GetTrim($(isba).find("tbody>tr").eq(i - 1).find("td").eq(j).text()), //面积确认
                VDCode: GetTrim($(vdn).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),             //通风采光
                FitmentCode: GetTrim($(fin).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),        //装修
                Cookroom: GetTrim($(cr).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),           //是否有厨房
                Balcony: GetTrim($(b).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),            //阳台数
                Toilet: GetTrim($(t).find("tbody>tr").eq(i - 1).find("td").eq(j).text()),             //洗手间数
                NoiseCode: GetTrim($(nn).find("tbody>tr").eq(i - 1).find("td").eq(j).text())           //噪音情况
            };
            update_arr[houseName] = HouseAttr;
        } else {
            AddOrUpdate(update_arr[houseName], tableModule, newVal);
        }
    }
}

/*
    更新
*/
function AddOrUpdate(obj, tableModule, newVal) {
    if (tableModule == "#tab_BuildArea") {
        obj.BuildArea = GetTrim(newVal);                 //面积
    }
    if (tableModule == "#tab_PurposeName") {
        obj.PurposeCode = GetTrim(newVal);               //用途
    }
    if (tableModule == "#tab_SubHouse") {
        obj.SubHouseType = GetTrim(newVal);              //附属房屋类型
    }
    if (tableModule == "#tab_SubHouseArea") {
        obj.SubHouseArea = GetTrim(newVal);              //附属房屋面积
    }
    if (tableModule == "#tab_HouseTypeName") {
        obj.HouseTypeCode = GetTrim(newVal);             //户型
    }
    if (tableModule == "#tab_StructureName") {
        obj.StructureCode = GetTrim(newVal);             //户型结构
    }
    if (tableModule == "#tab_UnitPrice") {
        obj.UnitPrice = GetTrim(newVal);                 //初始单价
    }
    if (tableModule == "#tab_Weight") {
        obj.Weight = GetTrim(newVal);                    //价格系数
    }
    if (tableModule == "#tab_FrontName") {
        obj.FrontCode = GetTrim(newVal);                 //朝向
    }
    if (tableModule == "#tab_SightName") {
        obj.SightCode = GetTrim(newVal);                 //景观
    }
    if (tableModule == "#tab_IsEValue") {
        obj.IsEValue = GetTrim(newVal);                  //是否可估
    }
    if (tableModule == "#tab_IsShowBuildingArea") {
        obj.IsShowBuildingArea = GetTrim(newVal);        //面积确认
    }
    if (tableModule == "#tab_VDName") {
        obj.VDCode = GetTrim(newVal);                    //通风采光
    }
    if (tableModule == "#tab_FitmentName") {
        obj.FitmentCode = GetTrim(newVal);               //装修
    }
    if (tableModule == "#tab_Cookroom") {
        obj.Cookroom = GetTrim(newVal);                  //是否有厨房
    }
    if (tableModule == "#tab_Balcony") {
        obj.Balcony = GetTrim(newVal);                   //阳台数
    }
    if (tableModule == "#tab_Toilet") {
        obj.Toilet = GetTrim(newVal);                    //洗手间数
    }
    if (tableModule == "#tab_NoiseCodeName") {
        obj.NoiseCode = GetTrim(newVal);                    //噪音情况
    }
}

//楼层下拉
function loadFloor(floorNum) {
    floorNum = GetInt32(floorNum);
    var PriceFloorNo_str = "", louceng_str = "";
    for (var i = 1; i <= floorNum; i++) {
        louceng_str += "<option value='" + i + "'>" + i + "</option>";
        if (i == parseInt(floorNum / 2) + 1) {
            PriceFloorNo_str += "<option value='" + i + "' selected='selected'>" + i + "</option>";
        } else {
            PriceFloorNo_str += "<option value='" + i + "'>" + i + "</option>";
        }
    }
    $("#PriceFloorNo").empty().append(PriceFloorNo_str);
    $("select[name='FloorNoSet']").each(function () {
        $(this).empty().append(louceng_str);
    });
}
//单元下拉
function loadUnitNo() {
    danyuan_str = "";
    $("#tab_HouseName").siblings("table").find("thead tr th").each(function (t) {
        if (t > 1) {
            danyuan_str += "<option value='" + t + "'>" + $.trim($(this).text()) + "</option>";
        }
    });

    $("select[name='UnitNoSet']").each(function () {
        $(this).empty().append(danyuan_str);
    });
}