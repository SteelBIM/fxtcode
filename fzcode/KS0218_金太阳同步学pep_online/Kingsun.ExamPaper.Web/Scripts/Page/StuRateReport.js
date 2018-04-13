/// <reference path="../Common.js" />
var studentName, gradeName;
$(function () {
    $(".navDiv").hide();
    $(".mainTitle").attr("class", "mainTitle T3");
    $(".mainTitle #spHomework").html("评价");
    studentName = unescape(Common.QueryString.GetValue("StudentName"));
    gradeName = unescape(Common.QueryString.GetValue("GradeName"));
    $(".spStuName").html(studentName);
    $(".spGradeName").html(gradeName);
    loadTestData();
});

function loadTestData() {
     $('#container1_1').highcharts({
        chart: {
            polar: true,
            type: 'line'
        },
        title: {
            text: '语言技能'
        },
        pane: {
            size: '80%'
        },
        xAxis: {
            categories: ['听', '说', '读', '写', '唱'],
            tickmarkPlacement: 'on',
            lineWidth: 0
        },
        yAxis: {
            gridLineInterpolation: 'polygon',
            lineWidth: 0,
            min: 0,
            max: 100
        },
        tooltip: {
            shared: true,
            pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.0f}</b><br/>'
        },
        legend: {
            align: 'center',
            verticalAlign: 'bottom'
        },
        series: [{
            name: '个人得分',
            data: [90, 95, 80, 90, 95],
            pointPlacement: 'on'
        }, {
            name: '年级平均分',
            data: [80, 85, 90, 95, 88],
            pointPlacement: 'on'
        }]
    });

    $('#container1_2').highcharts({
        chart: {
            polar: true,
            type: 'line'
        },
        title: {
            text: '语言知识'
        },
        pane: {
            size: '80%'
        },
        xAxis: {
            categories: ['语音', '词汇', '语法', '功能', '话题'],
            tickmarkPlacement: 'on',
            lineWidth: 0
        },
        yAxis: {
            gridLineInterpolation: 'polygon',
            lineWidth: 0,
            min: 0,
            max: 100
        },
        tooltip: {
            shared: true,
            pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.0f}</b><br/>'
        },
        legend: {
            align: 'center',
            verticalAlign: 'bottom'
        },
        series: [{
            name: '个人得分',
            data: [90, 95, 85, 90, 88],
            pointPlacement: 'on'
        }, {
            name: '年级平均分',
            data: [78, 80, 83, 95, 88],
            pointPlacement: 'on'
        }]
    });
}
