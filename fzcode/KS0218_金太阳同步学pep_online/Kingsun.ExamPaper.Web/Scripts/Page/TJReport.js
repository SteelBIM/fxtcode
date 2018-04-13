/// <reference path="../Common.js" />
var kArray = [], aArray = [];
var kids = '', aids = '';
var monthList = [];
var unitList = [];
var dialog = '';
$(function () {
    $(".navDiv").hide();
    $(".mainTitle").attr("class", "mainTitle T3");
    $(".mainTitle #spHomework").html("教学统计");
    $("#aTJReport").addClass("on");
    GetBaseData();
    $("#selClass").change(function () {
        if (monthList == '') {
            $.post("?action=GetUnits&Rand=" + Math.random(), { GradeID: $("#selClass").val().split("_")[0] }, function (result) {
                if (result) {
                    result = eval("(" + result + ")");
                    LoadUnitList(result.Data);
                    TJUnit_StudentScore();
                }
            });
        } else {
            TJUnit_StudentScore();
        }
    });
    $("#btnSearch").click(function () {
        TJUnit_StudentScore();
    });

});

//点击“包含补交作业学生成绩”按钮
function clickA(obj) {
    if ($(obj).attr("class") == "on") {
        $(obj).attr("class", "");
    } else {
        $(obj).attr("class", "on");
    }
    TJUnit_TaskUnitAvgScore();
    TJUnit_ClassScoreCompare();
    TJUnit_StudentScore();
}

//加载单元列表
function LoadUnitList(unitList) {
    $("#selBeginUnit").html("");
    $("#selEndUnit").html("");
    if (unitList) {
        for (var i = 0; i < unitList.length; i++) {
            if (i == 0) {
                $("#selBeginUnit").append('<option selected="selected" sort="' + i + '" value="' + unitList[i] + '">' + unitList[i] + '</option>');
                $("#selEndUnit").append('<option sort="' + i + '" value="' + unitList[i] + '">' + unitList[i] + '</option>');
            } else if (i == unitList.length - 1) {
                $("#selBeginUnit").append('<option sort="' + i + '" value="' + unitList[i] + '">' + unitList[i] + '</option>');
                $("#selEndUnit").append('<option sort="' + i + '" selected="selected" value="' + unitList[i] + '">' + unitList[i] + '</option>');
            } else {
                $("#selBeginUnit").append('<option sort="' + i + '" value="' + unitList[i] + '">' + unitList[i] + '</option>');
                $("#selEndUnit").append('<option sort="' + i + '" value="' + unitList[i] + '">' + unitList[i] + '</option>');
            }
        }
    }
}

//获取基础数据：班级、语言知识、语言技能、综合得分
function GetBaseData() {
    $.post("?action=GetMeteData&Rand=" + Math.random(), null, function (result) {
        if (result) {
            result = eval("(" + result + ")");
            if (result.Success) {
                toggleDefaultPage(0);
                var betweenUnit = '';
                if (result.Data.ClassList) {
                    var classHtml = '';
                    for (var i = 0; i < result.Data.ClassList.length; i++) {
                        classHtml += '<li class="' + (i == 0 ? 'on' : '')
                            + '"><a onclick="clickClass(this)" gradeid="' + result.Data.ClassList[i].GradeID
                            + '" cid="' + result.Data.ClassList[i].ClassID + '">'
                            + result.Data.ClassList[i].ClassName + '</a></li>';
                        $("#selClass").append('<option value="' + (result.Data.ClassList[i].GradeID + '_' + result.Data.ClassList[i].ClassID) + '">' + result.Data.ClassList[i].ClassName + '</option>');
                    }
                    classHtml += '<li>（可多选）</li>';
                    $("#ulC1").append(classHtml);
                    $("#ulC2").append(classHtml);
                }
                if (result.Data.KnowledgeList) {
                    var kHtml = '';
                    if ($("#subID").val() == "1") {
                        kHtml = '<li><a onclick="clickKnowledge(this)" kid="0">语文知识：</a></li>';
                    } else if ($("#subID").val() == "2") {
                        kHtml = '<li><a onclick="clickKnowledge(this)" kid="0">知识技能：</a></li>';
                    } else {
                        kHtml = '<li><a onclick="clickKnowledge(this)" kid="0">语言知识：</a></li>';
                    }
                    for (var i = 0; i < result.Data.KnowledgeList.length; i++) {
                        kids += ',' + result.Data.KnowledgeList[i].lpId;
                        kArray.push({ KID: result.Data.KnowledgeList[i].lpId, KName: result.Data.KnowledgeList[i].lpName });
                        kHtml += '<li><a onclick="clickKnowledge(this)" kid="' + result.Data.KnowledgeList[i].lpId + '" >' + result.Data.KnowledgeList[i].lpName + '</a></li>';
                    }

                    $("#ulK1").append(kHtml + '<li>（可多选）</li>');
                    $("#ulK2").append(kHtml + '<li>（单选）</li>');
                }
                if (result.Data.AbilityList) {
                    var aHtml = '';
                    if ($("#subID").val() == "1") {
                        aHtml = '<li><a onclick="clickAbility(this)" aid="0">语文能力：</a></li>';
                    } else if ($("#subID").val() == "2") {
                        aHtml = '<li><a onclick="clickAbility(this)" aid="0">数学素养：</a></li>';
                    } else {
                        aHtml = '<li><a onclick="clickAbility(this)" aid="0">语言技能：</a></li>';
                    }
                    for (var i = 0; i < result.Data.AbilityList.length; i++) {
                        aids += ',' + result.Data.AbilityList[i].id;
                        aArray.push({ AID: result.Data.AbilityList[i].id, AName: result.Data.AbilityList[i].capacityItem });
                        aHtml += '<li><a onclick="clickAbility(this)" aid="' + result.Data.AbilityList[i].id + '" >' + result.Data.AbilityList[i].capacityItem + '</a></li>';
                    }

                    $("#ulA1").append(aHtml + '<li>（可多选）</li>');
                    $("#ulA2").append(aHtml + '<li>（单选）</li>');
                }

                if ($("#subID").val() == "1") {
                    $("#ulG1").append('<li class="on"><a onclick="clickGeneral(this)">综合得分（默认）</a></li><li>（语文知识、语文能力、综合得分三选一）</li>');
                    $("#ulG2").append('<li class="on"><a onclick="clickGeneral(this)">综合得分（默认）</a></li><li>（语文知识、语文能力、综合得分三选一）</li>');
                } else if ($("#subID").val() == "2") {
                    $("#ulG1").append('<li class="on"><a onclick="clickGeneral(this)">综合得分（默认）</a></li><li>（知识技能、数学素养、综合得分三选一）</li>');
                    $("#ulG2").append('<li class="on"><a onclick="clickGeneral(this)">综合得分（默认）</a></li><li>（知识技能、数学素养、综合得分三选一）</li>');
                } else {
                    $("#ulG1").append('<li class="on"><a onclick="clickGeneral(this)">综合得分（默认）</a></li><li>（语言知识、语言技能、综合得分三选一）</li>');
                    $("#ulG2").append('<li class="on"><a onclick="clickGeneral(this)">综合得分（默认）</a></li><li>（语言知识、语言技能、综合得分三选一）</li>');
                }


                if (result.Data.MonthList) {
                    monthList = result.Data.MonthList;
                }
                if (monthList != '') {
                    LoadUnitList(result.Data.MonthList);
                } else {
                    LoadUnitList(result.Data.UnitList);
                }

                if (kids != '') {
                    kids = kids.substring(1);
                }
                if (aids != '') {
                    aids = aids.substring(1);
                }
                InitTJData(result.Data.ClassList[0].ClassID, result.Data.ClassList[0].GradeID, kids, aids, $("#selBeginUnit").val());
            }
            else {
                toggleDefaultPage(1);
            }
        } else {
            toggleDefaultPage(1);
        }
        Common.AutoPosition();
        window.parent.autoSetPosition();
    });
}

//切换无班级的缺省页
function toggleDefaultPage(flag) {
    if (flag == 1) {
        $(".moduleS").hide();
        $(".main").hide();
        $("#defaultPageClass").show();
        $(".span").html('您还没有班级哦，请联系学校管理员创建班级吧！');
    } else {
        $("#defaultPageClass").hide();
        $(".moduleS").show();
        $(".main").show();
    }
}

//单元报告
function UnitReport() {
    $("#unitA,#termA").removeClass("on");
    $("#unitA").addClass("on");
}
//学期报告
function TermReport() {
    $("#unitA,#termA").removeClass("on");
    $("#termA").addClass("on");
}
//选择班级
function clickClass(obj) {
    var isUpdate = false;
    var currentNo = $(obj).parent().parent().attr("id").substring(3);//取ID中的整数

    var classArray = $("#ulC" + currentNo + " li.on a");
    if ($(obj).parent().attr("class") == "on") {
        //取消选中时，至少保证选中一个班级
        if ($(obj).parent().parent().find("li.on").length > 1) {
            $(obj).parent().attr("class", "");
            isUpdate = true;
        }
    } else {
        //选中
        if (classArray) {
            if ($(classArray[0]).attr("gradeid") != $(obj).attr("gradeid")) {
                //选择了不同年级的班级，取消勾选其他班级
                $("#ulC" + currentNo + " li.on").attr("class", "");
            }
        }
        $(obj).parent().attr("class", "on");
        isUpdate = true;
    }

    if (isUpdate) {
        if (currentNo == "1") {
            TJUnit_TaskUnitAvgScore();
        } else if (currentNo == "2") {
            TJUnit_ClassScoreCompare();
        }
    }
}
//选择综合能力
function clickGeneral(obj) {
    var currentNo = $(obj).parent().parent().attr("id").substring(3);//取ID中的整数
    if ($(obj).parent().attr("class") == "on") {
        //若处于选中状态，则无变化
    } else {
        //反之，取消选中语言知识和语言技能，选中综合能力
        $("#ulA" + currentNo + " li.on").attr("class", "");
        $("#ulK" + currentNo + " li.on").attr("class", "");

        $(obj).parent().attr("class", "on");
        if (currentNo == "1") {
            TJUnit_TaskUnitAvgScore();
        } else if (currentNo == "2") {
            TJUnit_ClassScoreCompare();
        }
    }
}
//选择语言知识
function clickKnowledge(obj) {
    var isUpdate = false;
    var currentNo = $(obj).parent().parent().attr("id").substring(3);//取ID中的整数
    //currentNo=1 可多选，currentNo=2单选
    if ($(obj).attr("kid") == "0") {//选择父级元素时
        if ($(obj).parent().attr("class") == "on") {
            //若处于选中状态，则无变化
        } else {
            //反之，取消选中语言技能和综合能力，并选中语言知识和其第一个子维度
            $("#ulA" + currentNo + " li.on").attr("class", "");
            $("#ulG" + currentNo + " li.on").attr("class", "");

            $(obj).parent().attr("class", "on");
            $(obj).parent().next().attr("class", "on");
            isUpdate = true;
        }
    } else {//选择子元素时
        if ($(obj).parent().attr("class") == "on") {
            //若处于选中状态，判断是否选了多个子元素
            if ($("#ulK" + currentNo + " li.on").length > 2) {
                $(obj).parent().attr("class", "");
                isUpdate = true;
            }
        } else {
            //反之，取消选中语言技能和综合能力，并选中语言知识和当前子维度           
            $("#ulA" + currentNo + " li.on").attr("class", "");
            $("#ulG" + currentNo + " li.on").attr("class", "");
            if (currentNo == 2) {
                $("#ulK" + currentNo + " li.on").removeClass("on");
            }
            $("#ulK" + currentNo + " li").first().attr("class", "on");
            $(obj).parent().attr("class", "on");
            isUpdate = true;
        }
    }
    if (isUpdate) {
        if (currentNo == "1") {
            TJUnit_TaskUnitAvgScore();
        } else if (currentNo == "2") {
            TJUnit_ClassScoreCompare();
        }
    }
}

//选择语言技能
function clickAbility(obj) {
    var isUpdate = false;
    var currentNo = $(obj).parent().parent().attr("id").substring(3);//取ID中的整数

    if ($(obj).attr("aid") == "0") {//选择父级元素时
        if ($(obj).parent().attr("class") == "on") {
            //若处于选中状态，则无变化
        } else {
            //反之，取消选中语言知识和综合能力，并选中语言技能和其第一个子维度
            $("#ulK" + currentNo + " li.on").attr("class", "");
            $("#ulG" + currentNo + " li.on").attr("class", "");

            $(obj).parent().attr("class", "on");
            $(obj).parent().next().attr("class", "on");
            isUpdate = true;
        }
    } else {//选择子元素时
        if ($(obj).parent().attr("class") == "on") {
            //若处于选中状态，判断是否选了多个子元素
            if ($("#ulA" + currentNo + " li.on").length > 2) {
                $(obj).parent().attr("class", "");
                isUpdate = true;
            }
        } else {
            //反之，取消选中语言知识和综合能力，并选中语言技能和当前子维度
            $("#ulK" + currentNo + " li.on").attr("class", "");
            $("#ulG" + currentNo + " li.on").attr("class", "");
            if (currentNo == 2) {
                $("#ulA" + currentNo + " li.on").removeClass("on");
            }
            $("#ulA" + currentNo + " li").first().attr("class", "on");
            $(obj).parent().attr("class", "on");
            isUpdate = true;
        }
    }
    if (isUpdate) {
        if (currentNo == "1") {
            TJUnit_TaskUnitAvgScore();
        } else if (currentNo == "2") {
            TJUnit_ClassScoreCompare();
        }
    }
}

//作业单元平均成绩走势
function TJUnit_TaskUnitAvgScore() {
    var classArray = $("#ulC1 li.on a"), knowledgeArray, abilityArray;
    var classIDs = '', knowledgeIDs = '', abilityIDs = '', gradeID = '';
    for (var i = 0; i < classArray.length; i++) {
        if (gradeID == '' || gradeID == '0') {
            gradeID = $(classArray[i]).attr("gradeid");
        }
        classIDs += "," + $(classArray[i]).attr("cid");
    }
    if ($("#ulG1 li.on").length > 0) {//综合能力

    } else if ($("#ulK1 li.on a").length > 0) {//语言知识
        knowledgeArray = $("#ulK1 li.on a");
        for (var i = 0; i < knowledgeArray.length; i++) {
            knowledgeIDs += "," + $(knowledgeArray[i]).attr("kid");
        }
        knowledgeIDs = knowledgeIDs.substring(1);
    } else {//语言技能
        abilityArray = $("#ulA1 li.on a");
        for (var i = 0; i < abilityArray.length; i++) {
            abilityIDs += "," + $(abilityArray[i]).attr("aid");
        }
        abilityIDs = abilityIDs.substring(1);
    }
    $.post("?action=TJUnit_TaskUnitAvgScore&Rand=" + Math.random(), {
        GradeID: gradeID, ClassIDs: classIDs.substring(1),
        KnowledgeIDs: knowledgeIDs, AbilityIDs: abilityIDs,
        IsContain: ($("#containA").attr("class") == "on" ? 1 : 0)
    }, function (result) {
        if (result) {
            result = eval("(" + result + ")");
            if (result.Success) {
                if (result.Data) {
                    loadOne(result.Data);
                }
            } else {
                alert(result.Message);
                loadOne("");
            }
        }
    });
}
//班级成绩对比
function TJUnit_ClassScoreCompare() {
    var classArray = $("#ulC2 li.on a"), knowledgeArray, abilityArray;
    var classIDs = '', knowledgeIDs = '', abilityIDs = '', gradeID = '';
    for (var i = 0; i < classArray.length; i++) {
        if (gradeID == '' || gradeID == '0') {
            gradeID = $(classArray[i]).attr("gradeid");
        }
        classIDs += "," + $(classArray[i]).attr("cid");
    }
    if ($("#ulG2 li.on").length > 0) {//综合能力

    } else if ($("#ulK2 li.on a").length > 0) {//语言知识
        knowledgeArray = $("#ulK2 li.on a");
        for (var i = 0; i < knowledgeArray.length; i++) {
            knowledgeIDs += "," + $(knowledgeArray[i]).attr("kid");
        }
        knowledgeIDs = knowledgeIDs.substring(1);
    } else {//语言技能
        abilityArray = $("#ulA2 li.on a");
        for (var i = 0; i < abilityArray.length; i++) {
            abilityIDs += "," + $(abilityArray[i]).attr("aid");
        }
        abilityIDs = abilityIDs.substring(1);
    }
    $.post("?action=TJUnit_ClassScoreCompare&Rand=" + Math.random(), {
        GradeID: gradeID, ClassIDs: classIDs.substring(1),
        KnowledgeIDs: knowledgeIDs, AbilityIDs: abilityIDs,
        IsContain: ($("#containA").attr("class") == "on" ? 1 : 0)
    }, function (result) {
        if (result) {
            result = eval("(" + result + ")");
            if (result.Success) {
                if (result.Data) {
                    loadTwo(result.Data);
                }
            } else {
                alert(result.Message);
                loadTwo("");
            }
        }
    });
}
//学生成绩详情
function TJUnit_StudentScore() {
    var selClassID = $("#selClass").val().split("_");
    //判断单元选择大小
    var begin = parseInt($("#selBeginUnit option:selected").attr("sort"));
    var end = parseInt($("#selEndUnit option:selected").attr("sort"));
    if (begin > end) {
        alert("请选择正确的筛选范围~");
        return;
    }
    Tips("正在统计，请等待...");
    $.post("?action=TJUnit_StudentScore&Rand=" + Math.random(), {
        GradeID: selClassID[0], ClassID: selClassID[1],
        KnowledgeIDs: kids, AbilityIDs: aids,
        BeginUnit: escape($("#selBeginUnit").val()),
        EndUnit: escape($("#selEndUnit").val()),
        IsContain: ($("#containA").attr("class") == "on" ? 1 : 0)
    }, function (result) {
        if (result) {
            dialog.close();
            result = eval("(" + result + ")");
            if (result.Success) {
                if (result.Data) {
                    loadThree(result.Data);
                }
            } else {
                alert(result.Message);
                loadThree("");
            }
        }
    });
}
//首次加载页面时按默认条件（综合能力）统计数据
function InitTJData(classID, gradeID, knowledgeIDs, abilityIDs, betweenUnit) {
    var obj = { ClassIDs: classID, GradeID: gradeID, KnowledgeIDs: knowledgeIDs, AbilityIDs: abilityIDs, BeginUnit: escape($("#selBeginUnit").val()), EndUnit: escape($("#selEndUnit").val()) };
    $.post("?action=InitTJData&Rand=" + Math.random(), obj, function (result) {
        if (result) {
            result = eval("(" + result + ")");
            if (result.Success) {
                if (result.Data.TJUnit1) {
                    loadOne(result.Data.TJUnit1);
                } else {
                    loadOne("");
                }
                if (result.Data.TJUnit2) {
                    loadTwo(result.Data.TJUnit2);
                } else {
                    loadTwo("");
                }
                if (result.Data.TJUnit3) {
                    loadThree(result.Data.TJUnit3);
                } else {
                    loadThree("");
                }
            }
        }
    });
}
//显示作业单元平均成绩走势
function loadOne(tjData) {
    var nameList = [];//名称
    var dataArray = [];//数据列表   
    var unitArray = [];//单元列表

    if ($("#ulK1 li.on a").length == 0 && $("#ulA1 li.on a").length == 0) {
        nameList.push("综合得分");
    } else {
        if ($("#ulK1 li.on a").length > 0) {
            knowledgeArray = $("#ulK1 li.on a");
            for (var i = 0; i < knowledgeArray.length; i++) {
                if ($(knowledgeArray[i]).attr("kid") != "0") {
                    nameList.push($(knowledgeArray[i]).html());
                }
            }
        }
        if ($("#ulA1 li.on a").length > 0) {
            abilityArray = $("#ulA1 li.on a");
            for (var i = 0; i < abilityArray.length; i++) {
                if ($(abilityArray[i]).attr("aid") != "0") {
                    nameList.push($(abilityArray[i]).html());
                }
            }
        }
    }

    //获取横坐标值
    if (monthList != null && monthList != '') {
        unitArray = monthList;
    } else {
        if (tjData == "") {
            $('#report1').empty();
            return;
        }
        for (var i = 0; i < tjData[0].length; i++) {
            unitArray.push(tjData[0][i].Unit);
        }
    }
    //获取坐标对应数据
    if (monthList != '') {
        for (var i = 0; i < tjData.length; i++) {
            var seriesArray = [];//单元数据列表
            for (var m = 0; m < monthList.length; m++) {
                var score = 0;
                if (tjData[i] != null && tjData[i].length > 0) {
                    for (var j = 0; j < tjData[i].length; j++) {
                        if (monthList[m] == tjData[i][j].Unit) {
                            score = tjData[i][j].Score
                        }
                    }
                }
                seriesArray.push(score);
            }
            dataArray.push({ name: nameList[i], data: seriesArray });
        }
    } else {
        for (var i = 0; i < tjData.length; i++) {
            var seriesArray = [];//单元数据列表
            for (var j = 0; j < tjData[i].length; j++) {
                seriesArray.push(tjData[i][j].Score);
            }
            dataArray.push({ name: nameList[i], data: seriesArray });
        }
    }


    $('#report1').highcharts({
        chart: {
            type: 'line'
        },
        title: {
            text: '',
            x: -20 //center
        },
        xAxis: {
            categories: unitArray
        },
        yAxis: {
            min: 0,
            max: 100,
            title: {
                text: ''
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                },
                enableMouseTracking: false
            }
        },
        tooltip: {
            valueSuffix: ''
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: dataArray
    });
    $(".highcharts-button").hide();
}
//显示班级成绩对比
function loadTwo(tjData) {
    if (monthList == '' && tjData == "") {
        $('#report2').empty();
        return;
    }
    var seriesArray = [];
    var unitArray = [];
    var currentClassName = '';
    var currentClassData = [];
    var fistClass = 1;
    var classList = [];
    var nameList = [];
    if ($("#ulC2 li.on a").length > 0) {
        classList = $("#ulC2 li.on a");
        for (var i = 0; i < classList.length; i++) {
            if ($(classList[i]).attr("cid") != "0") {
                nameList.push($(classList[i]).html());
            }
        }
    }
    //语数班级对比
    if (monthList != '') {
        unitArray = monthList;//横坐标        
        for (var i = 0; i < nameList.length; i++) {
            //初始化横坐标对应的值
            currentClassData = new Array(unitArray.length);
            for (var a = 0; a < unitArray.length; a++) {
                currentClassData[a] = 0;
            }
            if (tjData != null && tjData.length > 0) {
                for (var j = 0; j < tjData.length; j++) {
                    if (tjData[j].ClassName == nameList[i]) {
                        for (var m = 0; m < monthList.length; m++) {
                            if (monthList[m] == tjData[j].Unit) {
                                currentClassData[m] = tjData[i].Score;
                            }
                        }
                    }
                }
            }
            seriesArray.push({ name: nameList[i], data: currentClassData });
        }
    } else {
        for (var i = 0; i < tjData.length; i++) {
            if (currentClassName != tjData[i].ClassName) {
                if (currentClassName != '') {
                    seriesArray.push({ name: currentClassName, data: currentClassData });
                }
                currentClassName = tjData[i].ClassName;
                currentClassData = [];
            }
            if (currentClassName == tjData[0].ClassName) {//遍历第一个班级的所有单元的成绩时，添加单元列表
                unitArray.push(tjData[i].Unit);
            }
            currentClassData.push(tjData[i].Score);
            if (i == tjData.length - 1) {
                seriesArray.push({ name: currentClassName, data: currentClassData });
            }
        }
    }
    $('#report2').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: ''
        },
        xAxis: {
            categories: unitArray,
            crosshair: true
        },
        yAxis: {
            min: 0,
            max: 100,
            title: {
                text: ''
            }
        },
        tooltip: {
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: seriesArray
    });
    $(".highcharts-button").hide();
}

//显示学生成绩详情
function loadThree(tjData) {
    if (tjData == "") {
        $('#report3').empty();
        return;
    }
    var tableHtml = '<table class="Stable"><thead><tr><td rowspan="2" class="darkBlue" width="40">排名</td>'
        + '<td rowspan="2" class="darkBlue" width="40">姓名</td>'
        + '<td rowspan="2" class="darkBlue" width="40">综合得分</td>';
    if ($("#subID").val() == "1") {
        tableHtml += '<td colspan="10" class="noneColor short"><div class="tableTab"><a class="a1 selected">语文知识得分率</a><a class="a2">语文能力得分率</a></div></td></tr>';
    }
    else if ($("#subID").val() == "2") {
        tableHtml += '<td colspan="10" class="noneColor short"><div class="tableTab"><a class="a1 selected">知识技能得分率</a><a class="a2">数学素养得分率</a></div></td></tr>';
    } else {
        tableHtml += '<td colspan="10" class="noneColor short"><div class="tableTab"><a class="a1 selected">语言知识得分率</a><a class="a2">语言技能得分率</a></div></td></tr>';
    }
    tableHtml += '<tr id="arrayName">';
    for (var i = 0; i < kArray.length; i++) {
        tableHtml += '<td>' + kArray[i].KName + '</td>';
    }
    tableHtml += '</tr></thead><tbody id="scoreTbody">';
    for (var i = 0; i < tjData.length; i++) {
        tableHtml += '<tr><td>' + (i + 1) + '</td><td>' + tjData[i].TrueName + '</td><td>' + tjData[i].GScore + '</td>';
        for (var j = 0; j < kArray.length; j++) {
            tableHtml += '<td>' + tjData[i]["KScore" + kArray[j].KID] + '</td>';
        }
        tableHtml += '</tr>';
    }
    tableHtml += '</tbody></table>';
    $("#report3").html(tableHtml);
    changeTaskCss(tjData);
    window.parent.autoSetPosition();
}

function changeTaskCss(tjData) {
    $(".Stable .tableTab a.a1").click(function () {
        $(this).parent().removeClass("status2");
        $(".Stable .tableTab a.a2").removeClass("selected");
        $(this).addClass("selected");
        ShowKnowledge(tjData);
    });
    $(".Stable .tableTab a.a2").click(function () {
        $(this).parent().addClass("status2");
        $(".Stable .tableTab a.a1").removeClass("selected");
        $(this).addClass("selected");
        ShowAbility(tjData);
    });
}

function ShowKnowledge(tjData) {
    var nameHtml = '';
    for (var i = 0; i < kArray.length; i++) {
        nameHtml += '<td>' + kArray[i].KName + '</td>';
    }
    $("#arrayName").html(nameHtml);

    var scoreHtml = '';
    for (var i = 0; i < tjData.length; i++) {
        scoreHtml += '<tr><td>' + (i + 1) + '</td><td>' + tjData[i].TrueName + '</td><td>' + tjData[i].GScore + '</td>';
        for (var j = 0; j < kArray.length; j++) {
            scoreHtml += '<td>' + tjData[i]["KScore" + kArray[j].KID] + '</td>';
        }
        scoreHtml += '</tr>';
    }
    $("#scoreTbody").html(scoreHtml);
}

function ShowAbility(tjData) {
    var nameHtml = '';
    for (var i = 0; i < aArray.length; i++) {
        nameHtml += '<td>' + aArray[i].AName + '</td>';
    }
    $("#arrayName").html(nameHtml);

    var scoreHtml = '';
    for (var i = 0; i < tjData.length; i++) {
        scoreHtml += '<tr><td>' + (i + 1) + '</td><td>' + tjData[i].TrueName + '</td><td>' + tjData[i].GScore + '</td>';
        for (var j = 0; j < aArray.length; j++) {
            scoreHtml += '<td>' + tjData[i]["AScore" + aArray[j].AID] + '</td>';
        }
        scoreHtml += '</tr>';
    }
    $("#scoreTbody").html(scoreHtml);
}

//弹窗
function Tips(content) {
    dialog = art.dialog({
        opacity: .1,
        lock: true,
        content: '<div class="tipMsg"><span><h4>提示：</h4><p>' + content + '</p></span>'
    });
    $(".aui_close").hide();
}

function loadTestData() {
    toggleDefaultPage(0);
    $('#report1').highcharts({
        title: {
            text: '',
            x: -20 //center
        },
        xAxis: {
            categories: ['Unit 1', '', 'Unit 3', 'Unit 4', 'Unit 5', 'Unit 6', 'Unit 7', 'Unit 8']
        },
        yAxis: {
            min: 0,
            max: 100,
            title: {
                text: ''
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: ''
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: [{
            name: '听',
            data: [75, 80, 75, 85, 83, 88, 92, 98]
        }, {
            name: '说',
            data: [70, 70, 65, 82, 80, 88, 85, 93]
        }, {
            name: '读',
            data: [60, 65, 70, 75, 85, 80, 88, 90]
        }]
    });

    $('#report2').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: ''
        },
        xAxis: {
            categories: ['Unit 1', 'Unit 2', 'Unit 3', 'Unit 4', 'Unit 5', 'Unit 6', 'Unit 7', 'Unit 8'],
            crosshair: true
        },
        yAxis: {
            min: 0,
            max: 100,
            title: {
                text: ''
            }
        },
        tooltip: {
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [{
            name: '一年级1班',
            data: [75, 80, 78, 85, 90, 88, 88, 95]
        }, {
            name: '一年级2班',
            data: [70, 75, 73, 82, 85, 88, 85, 93]
        }, {
            name: '一年级3班',
            data: [65, 75, 78, 80, 85, 83, 88, 92]
        }]
    });
}
