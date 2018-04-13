/// <reference path="../Common.js" />
var className;
$(function () {
    $(".navDiv").hide();
    $(".mainTitle").attr("class", "mainTitle T3");
    $(".mainTitle #spHomework").html("评价");
    className = unescape(Common.QueryString.GetValue("ClassName"));
    if (className == "undefined" || className == "") {
        className = "一（1）班";
    }
    $(".spClassName").html(className);
    $(".spClassNum").html(className.substr(0, 1));
    loadTestData();
});

function gotoStudent(studentName) {
    location.href = "StuRateReport.aspx?StudentName=" + escape(studentName) + "&GradeName=" + escape(className.substr(0, 1) + "年级");
}

function loadTestData() {
    $('#container1_1').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: '各分数段人数分布'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                },
                showInLegend: true
            }
        },
        series: [{
            type: 'pie',
            name: '占比',
            data: [
                ['90-100分', 40.3],
                ['80-90分', 25.4],
                ['60-80分', 25.2],
                ['低于60分', 9.1]
            ]
        }]
    });

    $('#container1_2').highcharts({
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

    $('#container1_3').highcharts({
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

    $('#container1_4').highcharts({
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
