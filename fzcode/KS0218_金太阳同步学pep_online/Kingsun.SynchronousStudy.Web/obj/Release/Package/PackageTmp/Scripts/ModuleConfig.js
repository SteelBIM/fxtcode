var ModuleConfigInit = function () {
    var current = this;

    this.UnitArr = [];//单元数据
    this.BookID = 0;//书本ID
    this.Section = "";//单元章节

    this.Init = function () {
        current.BookID = parseInt(getQueryString("bookid"));
        current.InitCatalog();
    };

    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    ///////////////////////////////////////////
    /////////////////加载教材目录//////////////
    ///////////////////////////////////////////
    this.InitCatalog = function (callback) {
        //////////通过教材ID获取教材目录////////////
        Common.GetCatalogByBookId(current.BookID, function (data) {
            if (data) {
                current.UnitArr = [];
                var Part = [];
                for (var i = 0; i < data.length; i++) {
                    var tempobj = {};
                    tempobj.ID = data[i].id;
                    tempobj.CodeName = data[i].title;
                    tempobj.children = data[i].children;
                    current.UnitArr.push(tempobj);
                }
                var titleobj = {};
                titleobj.ID = 0;
                titleobj.CodeName = "请选择教材目录";
                titleobj.children = null;
                current.UnitArr.unshift(titleobj);
                $("#firstTitle").KingsunSelect({
                    data: current.UnitArr,
                    onchange: function (index, data) {
                        current.UnitID = data.ID;
                        current.PageIndex = 0;
                        current.InitPart(data);
                    }
                });
                $("#firstTitle").data("select").selectValue(current.UnitID);
            };
        });
    }

    ///////////////////////////////////////////
    /////////////////加载教材单元块//////////////
    ///////////////////////////////////////////
    this.InitPart = function (data) {
        var Part = [];
        if (data.children == null) return;
        $("#divSecond").attr("style", "visibility:visible");
        for (var j = 0; j < data.children.length; j++) {
            var tempobj = {};
            tempobj.ID = data.children[j].id;
            tempobj.CodeName = data.children[j].title;
            Part.push(tempobj);
        }
        var titleobj = {};
        titleobj.ID = 0;
        titleobj.CodeName = "请选择教材单元块";
        Part.unshift(titleobj);
        $("#secondTitle").KingsunSelect({
            data: Part,
            onchange: function (index, data) {
                current.Section = data.ID;
                current.PageIndex = 0;
            }
        });
        $("#secondTitle").data("select").selectValue(current.Section);
    }
}

var moduleConfigInit;
$(function () {
    moduleConfigInit = new ModuleConfigInit();
    moduleConfigInit.Init();
});
