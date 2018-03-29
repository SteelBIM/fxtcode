var baseoptions = {
    scrollHeight: 250,
    max: 15,
    matchContains: true,
    dataType: "json",
    scroll: true,
    divwidth: function () { var $elem = $("#projectlist"); return $elem.get(0).offsetWidth - 2; },
    divtop: function () { var $elem = $("#projectlist"); return $elem.offset().top + $elem.get(0).offsetHeight; },
    divleft: function () { }
}

var projectoptions = {
    extraParams: args,
    formatItem: function (row, i, max) {
        return row.projectname;
    },
    formatMatch: function (row, i, max) {
        return row.pinyin + " " + row.pinyinall;
    },
    formatResult: function (row) {
        return row.projectname;
    }
}

var options = {
    scrollHeight: 250,
    max: 15,
    matchContains: true,
    dataType: "json",
    extraParams: args,
    divwidth: function () { var $elem = $("#projectlist"); return $elem.get(0).offsetWidth - 2; },
    divtop: function () { var $elem = $("#projectlist"); return $elem.offset().top + $elem.get(0).offsetHeight; },
    divleft: function () { },
    formatItem: function (row, i, max) {
        return row.projectname;
    },
    formatMatch: function (row, i, max) {
        return row.pinyin + " " + row.pinyinall;
    },
    formatResult: function (row) {
        return row.projectname;
    }
};

var dataoptions = {
    minChars: 0,
    scrollHeight: 250,
    max: 15,
    matchContains: true,
    dataType: "json",
    scroll: true,
    divisible: true,
    divwidth: function () { var $elem = $("#projectlist"); return $elem.get(0).offsetWidth - 2; },
    divtop: function () { var $elem = $("#projectlist"); return $elem.offset().top + $elem.get(0).offsetHeight; },
    divleft: function () { },
    formatItem: function (row, i, max) {
        return row.buildingname;
    },
    formatMatch: function (row, i, max) {
        return row.buildingname;
    },
    formatResult: function (row) {
        return row.buildingname;
    }
};
var databoptions = {
    minChars: 0,
    scrollHeight: 250,
    max: 15,
    matchContains: true,
    dataType: "json",
    scroll: true,
    divisible: true,
    divwidth: function () { var $elem = $("#projectlist"); return $elem.get(0).offsetWidth - 2; },
    divtop: function () { var $elem = $("#projectlist"); return $elem.offset().top + $elem.get(0).offsetHeight; },
    divleft: function () { },
    formatItem: function (row, i, max) {
        return row.floorno;
    },
    formatMatch: function (row, i, max) {
        return row.floorno;
    },
    formatResult: function (row) {
        return row.floorno;
    }
};