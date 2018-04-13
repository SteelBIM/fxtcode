/// <reference path="../Common.js" />
var userRole = 0;
$(function () {
    $("#aTJReport").addClass("on");
    userRole = Common.QueryString.GetValue("UserRole");
    if (userRole == "undefined" || userRole == 0) {
        window.parent.window.location.replace('../Default.aspx');
        alert("教师没有权限访问教学统计");
    }
    //初始化筛选框
    SetSelectItem(userRole);

    loadTestData();
});

//筛选框显示
function SetSelectItem(role) {
    var selectItemHtml = '<table><tr>';
    if (role == 1) {
        //筛选时间
        selectItemHtml += '<td>'
                    + '<label>日期：</label>'
                    + '<select><option>2016年1月</option><option>2016年2月</option><option>2016年3月</option><option>2016年4月</option><option>2016年5月</option></select>至'
                    + '<select><option>2016年5月</option><option>2016年4月</option><option>2016年3月</option></select>'
                    + '</td>';
    } else if (role == 2 || role == 3) {
        //筛选学科、年级、教师、时间
        selectItemHtml += '<td>'
                    + '<label>学科：</label>'
                    + '<select><option>英语</option><option>语文</option><option>数学</option></select>'
                    + '</td>';
        selectItemHtml += '<td>'
                    + '<label>年级：</label>'
                    + '<select><option>全部</option><option>一年级</option><option>二年级</option><option>三年级</option></select>'
                    + '</td>';
        selectItemHtml += '<td>'
                    + '<label>教师：</label>'
                    + '<select><option>全部</option><option>张三</option><option>李四</option><option>王二</option></select>'
                    + '</td>';
        selectItemHtml += '<td>'
                    + '<label>日期：</label>'
                    + '<select><option>2016年1月</option><option>2016年2月</option><option>2016年3月</option><option>2016年4月</option><option>2016年5月</option></select>至'
                    + '<select><option>2016年5月</option><option>2016年4月</option><option>2016年3月</option></select>'
                    + '</td>';
    } else if (role == 4) {
        //筛选学科、区、校、年级、日期
        selectItemHtml += '<td>'
                    + '<label>学科：</label>'
                    + '<select><option>英语</option><option>语文</option><option>数学</option></select>'
                    + '</td>';
        selectItemHtml += '<td>'
                    + '<label>区：</label>'
                    + '<select><option>全部</option><option>南山区</option><option>福田区</option><option>罗湖区</option></select>'
                    + '</td>';
        selectItemHtml += '<td>'
                    + '<label>校：</label>'
                    + '<select><option>全部</option><option>金太阳学校</option></select>'
                    + '</td>';
        selectItemHtml += '<td>'
                    + '<label>年级：</label>'
                    + '<select><option>全部</option><option>一年级</option><option>二年级</option><option>三年级</option></select>'
                    + '</td>';
        selectItemHtml += '<td>'
                    + '<label>日期：</label>'
                    + '<select><option>2016年1月</option><option>2016年2月</option><option>2016年3月</option><option>2016年4月</option><option>2016年5月</option></select>至'
                    + '<select><option>2016年5月</option><option>2016年4月</option><option>2016年3月</option></select>'
                    + '</td>';
    }
    selectItemHtml += '<td><input type="button" id="btnSearch" style="margin-left:20px;" value="查询" /></td>';
    selectItemHtml += '<td><div class="topContain"><a onclick="clickA(this)" id="containA" class="">包含补交学生成绩</a></div></td>';
    selectItemHtml += '</tr></table>';
    
    $(".topChange").html(selectItemHtml);
}

//点击“包含补交作业学生成绩”按钮
function clickA(obj) {
    if ($(obj).attr("class") == "on") {
        $(obj).attr("class", "");
    } else {
        $(obj).attr("class", "on");
    }
}

function loadTestData() {
    toggleDefaultPage(0);
    $('#report1').highcharts({
        title: {
            text: '',
            x: -20 //center
        },
        xAxis: {
            categories: ['2016年1月', '2016年2月', '2016年3月', '2016年4月', '2016年5月']
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
            name: '语音',
            data: [75, 80, 75, 85, 83]
        }, {
            name: '词汇',
            data: [70, 70, 65, 82, 80]
        }, {
            name: '语法',
            data: [60, 65, 70, 75, 85]
        }, {
            name: '功能意念',
            data: [68, 72, 70, 60, 77]
        }]
    });

    $('#report2').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: ''
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

    var tableHtml = '';
    var tableHtml = '<table class="tableStu" cellpadding="0" cellspacing="0"><thead><tr><td rowspan="2">排名</td><td rowspan="2">姓名</td><td rowspan="2">综合得分</td>'
        +'<td colspan="5">语言知识得分率</td><td colspan="4">语言技能得分率</td></tr>'
        +'<tr><td>语音</td><td>词汇</td><td>语法</td><td>功能意念</td><td>话题</td><td>听力</td><td>口语</td><td>阅读</td><td>写作</td></tr></thead><tbody>'
        +'<tr><td>1</td><td>小红</td><td>95</td><td>95</td><td>95</td><td>95</td><td>95</td><td>95</td><td>95</td><td>95</td><td>95</td><td>95</td></tr>'
        +'<tr><td>2</td><td>晓明</td><td>89</td><td>90</td><td>98</td><td>90</td><td>87</td><td>88</td><td>90</td><td>90</td><td>87</td><td>87</td></tr>'
        + '<tr><td>3</td><td>小李</td><td>80</td><td>80</td><td>85</td><td>82</td><td>78</td><td>77</td><td>90</td><td>90</td><td>81</td><td>81</td></tr>'
        + '<tr><td>4</td><td>小何</td><td>78</td><td>76</td><td>88</td><td>83</td><td>80</td><td>80</td><td>80</td><td>80</td><td>87</td><td>87</td></tr>'
        +'</tbody></table>';
    $("#report3").html(tableHtml);

    window.parent.autoSetPosition();
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

//选中语言知识
function clickKnowledge()
{
    $(".ulClass li.on").removeClass("on");
    $("#knowledge_li").addClass("on");
    $('#report1').highcharts({
        title: {
            text: '',
            x: -20 //center
        },
        xAxis: {
            categories: ['2016年1月', '2016年2月', '2016年3月', '2016年4月', '2016年5月']
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
            name: '语音',
            data: [75, 80, 75, 85, 83]
        }, {
            name: '词汇',
            data: [70, 70, 65, 82, 80]
        }, {
            name: '语法',
            data: [60, 65, 70, 75, 85]
        }, {
            name: '功能意念',
            data: [68, 72, 70, 60, 77]
        }]
    });
}

//选中语言技能
function clickAbility() {
    $(".ulClass li.on").removeClass("on");
    $("#ability_li").addClass("on");
    $('#report1').highcharts({
        title: {
            text: '',
            x: -20 //center
        },
        xAxis: {
            categories: ['2016年1月', '2016年2月', '2016年3月', '2016年4月', '2016年5月']
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
            data: [82, 70, 75, 85, 83]
        }, {
            name: '说',
            data: [70, 70, 65, 82, 80]
        }, {
            name: '读',
            data: [60, 65, 70, 68, 85]
        }, {
            name: '写',
            data: [60, 72, 68, 60, 77]
        }]
    });
}

//选中语言技能
function clickGeneral() {
    $(".ulClass li.on").removeClass("on");
    $("#general_li").addClass("on");
    $('#report1').highcharts({
        title: {
            text: '',
            x: -20 //center
        },
        xAxis: {
            categories: ['2016年1月', '2016年2月', '2016年3月', '2016年4月', '2016年5月']
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
            name: '综合得分',
            data: [75, 80, 75, 85, 83]
        }]
    });
}