var userRole = '';

$(function () {
    userRole = Common.QueryString.GetValue("UserRole");
    if (userRole == "undefined" || userRole == 0)
    {
        window.parent.window.location.replace('../Default.aspx');
        alert("教师没有权限访问教学统计");        
    }
    //加载筛选框
    SetSelectItem(userRole);

    //曲线图
    SetLineGragh();

    window.parent.autoSetPosition();
});

//筛选框显示
function SetSelectItem(role)
{
    var classHtml = '';
    var selectItemHtml = '<table><tr>';
    if (role == 1)
    {
        //筛选时间
        selectItemHtml +='<td>' 
                    +'<label>日期：</label>'
                    + '<select><option>2016年5月</option><option>2016年4月</option><option>2016年3月</option></select>至'
                    + '<select><option>2016年5月</option><option>2016年4月</option><option>2016年3月</option></select>'
                    + '</td>';
        //获取教师所带班级
        $.post("?action=GetClassList&Rand=" + Math.random(), null, function (result) {
            if (result) {
                result = eval("(" + result + ")");
                if (result.Success) {
                    document.getElementById('ulC1').style.display = "block";
                    for (var i = 0; i < result.Data.length; i++)
                    {
                        classHtml += '<li class="' + (i == 0 ? 'on' : '')
                            + '"><a onclick="clickClass(this)" gradeid="' + result.Data[i].GradeID
                            + '" cid="' + result.Data[i].ClassID + '">'
                            + result.Data[i].ClassName + '</a></li>';
                    }
                    classHtml +='<li>（可多选）</li>';
                    $("#ulC1").html(classHtml);
                } else {
                    toggleDefaultPage(1);
                }
            } else {
                toggleDefaultPage(1);
            }
        });
    } else if (role == 2||role==3)
    {
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
                    + '<select><option>2016年5月</option><option>2016年4月</option><option>2016年3月</option></select>至'
                    + '<select><option>2016年5月</option><option>2016年4月</option><option>2016年3月</option></select>'
                    + '</td>';
    } else if (role == 4)
    {
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
                    + '<select><option>2016年5月</option><option>2016年4月</option><option>2016年3月</option></select>至'
                    + '<select><option>2016年5月</option><option>2016年4月</option><option>2016年3月</option></select>'
                    + '</td>';
    }
    selectItemHtml += '<td><input type="button" id="btnSearch" style="margin-left:20px;" value="查询" /></td>';
    selectItemHtml += '</tr></table>';   
    $(".topChange").html(selectItemHtml);
}

//设置曲线图
function SetLineGragh()
{
    $('#report1').highcharts({
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
}

//选择班级
function clickClass(obj)
{
    var classArray = $("#ulC1 li.on a");
    if ($(obj).parent().attr("class") == "on") {
        //取消选中时，至少保证选中一个班级
        if ($(obj).parent().parent().find("li.on").length > 1) {
            $(obj).parent().attr("class", "");
        }
    } else {
        //选中
        if (classArray) {
            if ($(classArray[0]).attr("gradeid") != $(obj).attr("gradeid")) {
                //选择了不同年级的班级，取消勾选其他班级
                $("#ulC1 li.on").attr("class", "");
            }
        }
        $(obj).parent().attr("class", "on");
    }
}

//切换无班级的缺省页
function toggleDefaultPage(flag) {
    if (flag == 1) {
        $(".topChange").hide();
        $(".ulClass").hide();
        $(".content").hide();
        $("#defaultPageClass").show();
        $(".span").html('您还没有班级哦，请联系学校管理员创建班级吧！');
    } else {
        $("#defaultPageClass").hide();
        $(".topChange").show();
        $(".ulClass").show();
        $(".content").show();
    }
}