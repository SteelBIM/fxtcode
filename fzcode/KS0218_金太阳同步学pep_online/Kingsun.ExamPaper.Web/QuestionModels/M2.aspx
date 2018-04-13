<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="M2.aspx.cs" Inherits="Kingsun.SunnyTask.Web.QuestionModels.M2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../App_Themes/css/base.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/css/task.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../App_Themes/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../App_Themes/js/jquery.nicescroll.js"></script>
</head>
<body>
    <!--M2——T002-->
    <div class="M M3">
        <div class="nr">
            <div class="kewen" id="scroll-1">
                <ul id="qContent">
                </ul>
            </div>
        </div>

    </div>
    <script type="text/javascript" src="../Scripts/Common.js"></script>
    <script src="../Scripts/jquery.json-2.4.js"></script>
    <script type="text/javascript">
        var accessType = 0;
        var questionID = '';
        var stuTaskID = '';
        var focusIndex = -1, qLength = 0;//当前是第几小题、共多少小题
        var readRecordList = [];//保存每句的跟读分数、地址及用时
        var requireRound = 0, readRound = 0;//要求跟读次数、已读次数
        var startTime, endTime;
        var stuscore = 0, backurl = '', allbackurl = '', spendtime = 0;//学生得分，录音返回地址，拼接起来的录音地址，用时
        var catalogID = Common.QueryString.GetValue("CatalogId")
        var qAnswerList = '', qMp3List = '';
        var strAlphbet = "abcdefghijklmnopqrstuvwxyz";

        var recordList = [];//记录跟读
        var spendTime = 0;
        var highScore = 0;//记录最高分
        var questionContent =[];
        $(function () {     
            startTime = new Date().getTime();
            accessType = parseInt(Common.QueryString.GetValue("AccessType"));
            stuTaskID = Common.QueryString.GetValue("StuTaskID");
            questionID = Common.QueryString.GetValue("QuestionID");
            requireRound = parseInt(Common.QueryString.GetValue("Round"));
            if (accessType == 3 && (stuTaskID == "undefined" || questionID == "undefined")) {
                alert("未获取到作业哦！");
                window.parent.backList();
            } else if ((accessType <= 2) && questionID == "undefined") {
                alert("未获取到题目哦！");
                window.parent.backList();
            } else if (accessType == 4 && (stuTaskID == "undefined" || questionID == "undefined")) {
                alert("未获取到题目哦！");
                window.parent.backList();
            } else {
                $.post("?action=GetQuestionInfo", { catalogId: Common.QueryString.GetValue("CatalogId"), StuTaskID: stuTaskID == "undefined" ? "" : stuTaskID, QuestionID: questionID, AccessType: 3 }, function (result) {//获取题目
                    if (result) {
                        result = eval("(" + result + ")");//JSON.parse
                        if (result.Success) {
                            Common.CheckIndexOf();
                            var ulHtml = '';
                            //requireRound = result.Data[0].Round;
                            readRound = 0;
                            qLength = result.Data.length - 1;
                            var highRound = 0;//记录最高得分的round
                            $.each(result.Data, function (index, value) {
                                if (index == 0) {

                                } else {
                                    var minReadRecord = [];
                                    var hasName = 1;//0-无人名；1-有冒号:的人名；2-有中括号【】的人名
                                    var qContent = value.QuestionContent.replace(/\\n/g, "<br/>").replace(/\/n/g, "<br/> ").replace("[", "<u>").replace("]", "</u>");
                                    questionContent.push(qContent);
                                    qContent = qContent.replace(',', ', ').replace('.', '. ').replace('!', '! ').replace('?', '? ').replace('...', '...').replace('......', '...... ').replace('—', '— ');
                                    var words = qContent.split(' ');
                                    qContent = "";
                                    for (var i = 0; i < words.length; i++) {
                                        if (words[i].lastIndexOf('<br/>') == -1) {//除了换行标签,其他字符用span包起来,后面做各个单词颜色处理
                                            qContent += '<span>' + words[i] + ' </span>'
                                        } else {
                                            qContent += words[i];
                                        }
                                    }
                                    var qName;
                                    if (qContent.indexOf(":") > 0) {
                                        qName = qContent.split(":")[0].split(" ");
                                        for (var j = 0; j < qName.length; j++) {
                                            //判断人名：第一个冒号前的所有单词的首字母是否大写
                                            if (strAlphbet.indexOf(qName[j].charAt(0)) >= 0) {
                                                hasName = 0;
                                                break;
                                            }
                                        }
                                    } else if (qContent.indexOf("】") > 0) {
                                        hasName = 2;
                                    } else {
                                        hasName = 0;
                                    }
                                    if (hasName == 1) {
                                        ulHtml += '<li><p qid="' + value.QuestionID + '"><b>【' + qContent.split(":")[0] + '】</b>' + qContent.substring(qContent.split(":")[0].length + 1);
                                    } else if (hasName == 2) {
                                        ulHtml += '<li><p qid="' + value.QuestionID + '"><b>' + qContent.split("】")[0] + '】</b>' + qContent.substring(qContent.split("】")[0].length + 1);
                                    } else {
                                        ulHtml += '<li><p qid="' + value.QuestionID + '">' + qContent;
                                    }
                                    ulHtml += '</p></li>';
                                    qAnswerList += ';' + value.QuestionContent.replace(";", " ");
                                    qMp3List += ';' + value.Mp3Url.replace(";", " ");
                                }
                            });
                            $("#qContent").append(ulHtml);
                            resizeWindow();
                            if (qMp3List.length > 0) {
                                qMp3List = qMp3List.substring(1);
                            }
                            if (qAnswerList.length > 0) {
                                qAnswerList = qAnswerList.substring(1);
                            }
                            if (allbackurl.length > 0) {
                                allbackurl = allbackurl.substring(1);
                            }
                            window.parent.InitRecord(qMp3List, qAnswerList, true);
                            if (allbackurl.length > 0) {
                                window.parent.ShowResult(stuscore, readRound, requireRound, allbackurl);
                            }
                        } else {
                            alert(result.Message);
                            window.parent.backList();
                        }
                    } else {
                        alert("未获取到作业哦！");
                        window.parent.backList();
                    }
                });
            }
        });
        function resizeWindow() {
            window.parent.autoSetPosition(0, 0);
            $('.M').niceScroll({ touchbehavior: false, autohidemode: false, cursorcolor: "#dcdcdc", cursoropacitymax: 1, cursorwidth: 10 });
        }
        //返回分数，处理并显示
        function EndRecord(data) {
            //stuscore = parseInt(data.lines[0].score);
            stuscore = parseInt(Math.sqrt(data.lines[0].score) * 10);
            backurl = window.parent.KSRecord.GetReplayPath();

            if (focusIndex == -1) {
                allbackurl = backurl;
            } else {
                allbackurl += ";" + backurl;
            }
            endTime = new Date().getTime();
            spendtime = (endTime - startTime) / 1000;
            startTime = endTime;//重新计时
            focusIndex++;
            if ($("#qContent li:eq(" + focusIndex + ") p .s")) {
                $("#qContent li:eq(" + focusIndex + ") p .s").remove();
            }
           // $("#qContent li:eq(" + focusIndex + ") p").append('<em class="s">' + stuscore + '</em>');不显示分数(分数对小学生不友好)

            showOn();
            window.parent.Mp3Obj.NextRecord(true);
            SetWordsColor(data, $("#qContent li:eq(" + focusIndex + ") p").attr("qid"));

            if (focusIndex > readRecordList.length - 1) {
                readRecordList.push({
                    QuestionID: $("#qContent li:eq(" + focusIndex + ") p").attr("qid"),
                    StuScore: stuscore, BackUrl: backurl, SpendTime: spendtime
                });
                //$("#qContent li:eq(" + focusIndex + ") p").append('<em class="s">' + stuscore + '</em>');
            } else {
                readRecordList[focusIndex] = {
                    QuestionID: $("#qContent li:eq(" + focusIndex + ") p").attr("qid"),
                    StuScore: stuscore, BackUrl: backurl, SpendTime: spendtime
                };
                //$("#qContent li:eq(" + focusIndex + ") p").append('<em class="s">' + stuscore + '</em>');
            }

            //读完完整的一遍后
            if (focusIndex >= qLength - 1) {
                focusIndex = -1;
                readRound++;
                //获取大题分数及用时
                stuscore = 0;
                spendtime = 0;
                var newBackUrl = '';//录音路径集合
                window.parent.stuAnswerList = [];//先清空上次读的成绩
                for (var i = 0; i < readRecordList.length; i++) {
                    stuscore += readRecordList[i].StuScore;
                    spendtime += readRecordList[i].SpendTime;
                    newBackUrl += ";" + readRecordList[i].BackUrl;
                  
                    window.parent.stuAnswerList.push({ QuestionID: readRecordList[i].QuestionID, CatalogID: catalogID, Answer: readRecordList[i].BackUrl, BestAnswer: readRecordList[i].BackUrl, Score: stuscore, BestScore: stuscore, QuestionContent: questionContent[i] });
                }
                stuscore = parseInt(Math.floor(stuscore / qLength));
                if (newBackUrl != "") {
                    newBackUrl = newBackUrl.substring(1);
                }
                window.parent.ShowScore(stuscore, 1, 1);
                window.parent.ShowBackPlayer(allbackurl);
                window.parent.showNextA();
                window.parent.showSubmit();
            }
        }
        //设置单词样式
        function SetWordsColor(data, qid) {
            var spans = $("#qContent li:eq(" + focusIndex + ") p").find("span");
            spans.removeAttr("style");//先移除span下所有样式
            var words = [];
            for (var i = 0; i < data.lines.length; i++) {//把所有单词对应的分数存到一个新数组中
                for (var j = 0; j < data.lines[i].words.length; j++) {
                    words.push(data.lines[i].words[j]);
                }
            }
            for (var n = 0; n < words.length; n++) {
                spans.each(function () {
                    if ($(this).attr("style") == undefined || $(this).attr("style").lastIndexOf("color") == -1) {//赋过颜色的不再处理
                        if (words[n].type != 4 && $(this).text().lastIndexOf(words[n].text) != -1) {
                            var score = words[n].score * 10;
                            if (score < 60) {
                                $(this).css("color", "#ff7800");
                            } else if (score < 80 && score >= 60) {
                                $(this).css("color", "#ffae00 ");
                            } else if (score < 90 && score >= 80) {
                                $(this).css("color", "#46cb85");
                            }
                            else if (score <= 100 && score >= 90) {
                                $(this).css("color", "#4680c8");
                            }
                            return false;
                        }
                    }
                })
            }


        }
        //隐藏每句分数
        function hideScore() {
            if (arguments[0]) {
                focusIndex = arguments[0] - 1;
            }
            if (focusIndex == -1) {
                $("#qContent li p .s").remove();
            } else {
                $("#qContent li p:gt(" + focusIndex + ") .s").remove();
            }
        }

        function showOn() {
            $("#qContent li").removeClass("on");
            $("#qContent li:eq(" + (focusIndex + 1) + ")").addClass("on");
            if (focusIndex > 1 && focusIndex < qLength - 1) {//课文比较长时,滚动条自动滚动.(从第三句开始滚动)
                var h = $("#qContent li:eq(" + (focusIndex + 1) + ")").position().top - $("#qContent li:eq(" + (focusIndex) + ")").position().top;
                var currentH = parseInt($(".kewen").prop("scrollTop"));
                $(".kewen").scrollTop(currentH + h);
            }
            if (focusIndex >= 4 && focusIndex < qLength - 1) {
                var v = $("#qContent li.on").offset().top + $("#qContent li.on").height() + 20 - parseInt(window.parent.autoScrollHeight);
                $('.M').animate({ scrollTop: v }, 'slow');
            }
        }

    </script>
</body>
</html>
