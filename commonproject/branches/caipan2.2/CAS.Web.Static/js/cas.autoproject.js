//自动完成楼盘 kevin
; (function ($) { 
    $.fn.casAutoProject= function (args) {
        var tmpvalue = "";
        var $this = $(this);
        //处理数据
        function _return(data) {
            if (tmpvalue == $this.val()) return;
            //如果关联楼栋，则处理
            $("#" + args.objprojectid).val("");
            $("#" + args.objusableyear).val("");
            $("#" + args.objaddress).val("");
            $("#" + args.objareaid).val("");
            $("#" + args.objsubareaid).val("");
            if (args.buildings) {
                $("#" + args.buildings.objbuildingname).val("").casunautocomplete();
                $("#" + args.buildings.objbuildingid).val("");
                $("#" + args.buildings.objbuildingdate).val("");
                $("#" + args.buildings.objtotalfloor).val("");
                //调用清除事件
                $("#" + args.buildings.objbuildingname).casautobuilding({ buildings: args.buildings, clear: true });
            }
            if (data && data.projectid) {               
                $("#" + args.objprojectid).val(data.projectid);
                if (data.usableyear) $("#" + args.objusableyear).val(data.usableyear);
                if (data.address) $("#" + args.objaddress).val(data.address);
                if (data.areaid) $("#" + args.objareaid).val(data.areaid);              
                if (data.subareaid) $("#" + args.objsubareaid).val(data.subareaid);
                //楼栋联动
                if (args.buildings) {
                    $("#" + args.buildings.objbuildingname).casautobuilding({ buildings: args.buildings, data: data, callback: args.buildings.callback });
                }
            }
            tmpvalue = $this.val();
        }
        $this.casautocomplete({
            fieldformats: [{ field: "projectname"}], //, { field: "address", begin: "[", end: "]"}], //列表字段及格式
            fieldresult: "projectname", //返回的字段
            data: "project.projectlist", //API地址或者json数据
            options: { extraParams: { type: "dropdown", cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid} }, //扩展选项
            callback: function (event, data, formatitem) {//回调函数               
                _return(data);
                if (args && args.callback) args.callback(data);
            }
        });
        //处理找不到的情况
        $this.bind("change", function () {
            if (args && args.callback) {
                args.callback(null);
            }
            _return(null);
        });

        return this;
    };
    //自动完成楼栋
    $.fn.casautobuilding= function (args) {
        var $this = $(this);
        function clear() {
            $("#" + args.buildings.objbuildingid).val("");
            $("#" + args.buildings.objbuildingdate).val("");
            $("#" + args.buildings.objtotalfloor).val("");
            if (args.buildings.floors) {
                $("#" + args.buildings.floors.objfloornumber).val("").casunautocomplete();
                $("#" + args.buildings.floors.objfloornumber).casautofloor({ floors: args.buildings.floors, clear: true });
            }
        }
        if (args.clear) {
            clear();
            return;
        }
        var tmpvalue = "";
        function _return(data) {
            if (tmpvalue == $this.val()) return;
            clear();
            //字母、数据结尾，加"栋"
            if (/[a-zA-Z0-9]$/.test($this.val())) {
                $this.val($this.val() + "栋");
            }
            if (data && data.buildingid) {
                $("#" + args.buildings.objbuildingid).val(data.buildingid);
                if (data.buildingdate) $("#" + args.buildings.objbuildingdate).val(data.buildingdate);
                $("#" + args.buildings.objtotalfloor).val(data.floortotal);
                if (args.buildings.floors) {
                    $("#" + args.buildings.floors.objfloornumber).casautofloor({ floors: args.buildings.floors, data: data, callback: args.buildings.floors.callback });
                }
            }
            tmpvalue = $this.val();
        }
        var data = args.data;
        $this.val("").casunautocomplete();
        var vdata = { type: "dropdown", projectid: data.projectid, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid };
        if (data.projectid) {
            CAS.API({ type: "post", api: "project.buildinginfo", data: vdata, callback: function (data) {
                var sdata = data.data;
                $this.casautocomplete({
                    fieldformats: [{ field: "buildingname"}], //列表字段及格式
                    fieldresult: "buildingname", //返回的字段
                    fieldmatchs: ["buildingname"],
                    data: sdata, //API地址或者json数据
                    options: { minChars: 0 }, //扩展选项
                    callback: function (event, data, formatitem) {//回调函数                       
                        _return(data);
                        if (args && args.callback) args.callback(data);
                    }
                });
            }
            });
        }
        //处理找不到的情况
        $this.bind("change", function () {
            if (args && args.callback) {
                args.callback(null);
            }
            _return(null);
        });
        return this;
    };
    //自动完成楼层
    $.fn.casautofloor= function (args) {
        var $this = $(this);
        function clear() {
            if (args.floors.houses) {
                $("#" + args.floors.houses.objhousename).val("").casunautocomplete();
                $("#" + args.floors.houses.objhousename).casautoroom({ houses: args.floors.houses, clear: true });
            }
        }
        if (args.clear) {
            clear();
            return;
        }
        var tmpvalue = "";
        function _return(data) {
            if (tmpvalue == $this.val()) return;
            if (args.floors.houses) {
                clear();
            }
            if (data && data.floorno) {
                $this.val(data.floorno);
                if (args.floors.houses) {
                    $("#" + args.floors.houses.objhousename).casautoroom({ houses: args.floors.houses, data: data, callback: args.floors.houses.callback });
                }
            }
            tmpvalue = $this.val();
        }
        var data = args.data;
        $this.val("").casunautocomplete();

        var buildingid;
        if (null != data) {
            buildingid = data.buildingid;
        } else {
            buildingid = 0;
        }
        var vdata = { type: "dropdown", buildingid: buildingid, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid };
        if (buildingid > 0) {
            CAS.API({ type: "post", api: "project.floorlist", data: vdata, callback: function (data) {
                var sdata = data.data;
                $this.casautocomplete({
                    fieldformats: [{ field: "floorno"}], //列表字段及格式
                    fieldresult: "floorno", //返回的字段
                    fieldmatchs: ["floorno"],
                    data: sdata, //API地址或者json数据
                    options: { minChars: 0 }, //扩展选项
                    callback: function (event, data, formatitem) {//回调函数
                        data = $.extend({}, data, { buildingid: buildingid });
                        _return(data);
                        if (args && args.callback) args.callback(data);
                    }
                });
            }
            });
        }
        //处理找不到的情况
        $this.bind("change", function () {
            if (args && args.callback) {
                args.callback(null);
            }
            _return(null);
        });
        return this;
    };
    //自动完成房号
    $.fn.casautoroom= function (args) {
        var $this = $(this);
        function clear() {
            if (args.houses.house) {
                $("#" + args.houses.house.objhouseid).val("");
                $("#" + args.houses.house.objbuildingarea).val("");
                $("#" + args.houses.house.objsubhousearea).val("");
                $("#" + args.houses.house.objsubhousetype).val("");
            }
        }
        if (args.clear) {
            clear();
            return;
        }
        var tmpvalue = "";
        function _return(data) {
            if (tmpvalue == $this.val()) return;
            if (args.houses.house) {
                clear();
            }
            if (data && data.houseid) {
                $("#" + args.houses.house.objhouseid).val(data.houseid);
                if (data.buildarea) $("#" + args.houses.house.objbuildingarea).val(data.buildarea);
                if (data.subhousearea) $("#" + args.houses.house.objsubhousearea).val(data.subhousearea);
                if (data.subhousetype) $("#" + args.houses.house.objsubhousetype).val(data.subhousetype);
                if (args.houses.house.callback) args.houses.house.callback(data);
            }
            tmpvalue = $this.val();
        }
        var data = args.data;
        $this.val("").casunautocomplete();
        var buildingId = 0, floorNo = 0;
        if (null != data) {
            buildingId = data.buildingid;
            floorNo = data.floorno;
        }
        var vdata = { type: "dropdown", buildingid: buildingId, floorno: floorNo, cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid };
        if (floorNo > 0) {
            CAS.API({ type: "post", api: "project.houselist", data: vdata, callback: function (data) {
                var sdata = data.data;
                $this.casautocomplete({
                    fieldformats: [{ field: "housename"}], //列表字段及格式
                    fieldresult: "housename", //返回的字段
                    fieldmatchs: ["housename"],
                    data: sdata, //API地址或者json数据
                    options: { minChars: 0 }, //扩展选项
                    callback: function (event, data, formatitem) {//回调函数                       
                        _return(data);
                        if (args && args.callback) args.callback(data);
                    }
                });
            }
            });
        }
        //处理找不到的情况
        $this.bind("change", function () {
            if (args && args.callback) {
                args.callback(null);
            }
            _return(null);
        });
        return this;
    };
    })(jQuery)