/// <reference path="../Common.js" />

$(function () {
    $(".navDiv").hide();
    $(".mainTitle").attr("class", "mainTitle T3");
    $(".mainTitle #spHomework").html("评价");
    loadTestData();
});

function gotoSchool(schoolName) {
    location.href = "SchoolRateReport.aspx?SchoolName=" + escape(schoolName);
}

function loadTestData() {
    $('#container1_1').highcharts({
        chart: {
            type: 'line'
        },
        title: {
            text: '任务完成持续性'
        },
        xAxis: {
            categories: ['0-5次', '5-10次', '10-15次', '15-25次', '30次以上']
        },
        yAxis: {
            title: {
                text: ''
            }
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                },
                enableMouseTracking: false
            }
        },
        series: [{
            name: '任务完成持续性',
            data: [15, 10, 20, 25, 30]
        }]
    });

    $('#container1_2').highcharts({
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
            name: '班级',
            data: [85, 95, 85, 90, 88],
            pointPlacement: 'on'
        }, {
            name: '年级',
            data: [78, 88, 83, 95, 88],
            pointPlacement: 'on'
        }]
    });

    $('#container1_3').highcharts({
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
            name: '班级',
            data: [85, 95, 85, 90, 88],
            pointPlacement: 'on'
        }, {
            name: '年级',
            data: [78, 88, 83, 95, 88],
            pointPlacement: 'on'
        }]
    });
}
