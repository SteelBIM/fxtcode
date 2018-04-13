<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="M1.aspx.cs" Inherits="Kingsun.SunnyTask.Web.QuestionModels.M1" %>

<!DOCTYPE HTML>
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
    <div class="M M1">
        <ul>
            <li>
                <img id="qImg" style="z-index: 1000; cursor: pointer" src="" alt="" /></li>
        </ul>
        <h3 id="qContent" class="<%=Request.QueryString["type"].Contains("sentence")?"qContent2":"qContent1" %>  style="z-index: 1000; cursor: pointer"></h3>
    </div>
    <script type="text/javascript" src="../Scripts/Common.js"></script>
    <script type="text/javascript">
        var accessType = 0;
        var questionID = '', parentID = '';
        var stuTaskID = '';
        var requireRound = 0, readRound = 0;//要求跟读次数、已读次数
        var startTime, endTime;
        var stuscore = 0, backurl = '', spendtime = 0;//学生得分，录音返回地址，用时
        var qMp3Url = '', qAnswer = '';//题目音频，题目答案
        var catalogID = 0;
        var highRecord;//跟读最高得分记录
        var answerList;//跟读大题下的小题答案记录
        var minQueCount = 0;
        var spendTime = 0;//小题用时
        var rightCount = 0;
        var questionContent = '';
        var qScore = 0;//题目的分数
        $(function () {
            startTime = new Date().getTime();
            accessType = parseInt(Common.QueryString.GetValue("AccessType"));
            stuTaskID = Common.QueryString.GetValue("StuTaskID");
            questionID = Common.QueryString.GetValue("QuestionID");
            parentID = Common.QueryString.GetValue("ParentID");
            catalogID = Common.QueryString.GetValue("CatalogId");

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
            } else if ((accessType == 5) && questionID == "undefined") {
                alert("未获取到题目哦！");
                window.parent.backList();
            } else {
                $.post("?action=GetQuestionInfo", { QuestionID: questionID, AccessType: 3, ParentID: parentID }, function (result) {
                    if (result) {
                        result = eval("(" + result + ")");//JSON.parse
                        if (result.Success) {
                            result = eval("(" + result.Data + ")");
                            var question = result.QuestionInfo;
                            answerList = result.AnswerList;
                            var qAnswerList = [], qMp3List = [];
                          
                            var qcHtml = question.QuestionContent.replace(/\\n/g, "<br/> ").replace(/\/n/g, "<br/> ").replace("[", "<u>").replace("]", "</u>");
                            qcHtml = qcHtml.replace(',', ', ').replace('.', '. ').replace('!', '! ').replace('?', '? ').replace('...', '...').replace('......', '...... ').replace('—', '— ');
                            var words = qcHtml.split(' ');
                            var qHtml = "";
                            for (var i = 0; i < words.length; i++) {
                                if (words[i].lastIndexOf('<br/>') == -1) {//除了换行标签,其他字符用span包起来,后面做各个单词颜色处理
                                    qHtml += '<span>' + words[i] + ' </span>'
                                } else {
                                    qHtml += words[i];
                                }
                            }
                            $("#qContent").html(qHtml);

                            $("#qImg").attr("src", question.ImgUrl);
                            resizeWindow();
                            qMp3Url = question.Mp3Url;
                            //qAnswer = question.BlankAnswer[0].Answer; hlw 
                            qAnswer = question.QuestionContent;
                            questionContent = question.QuestionContent;
                            qScore = question.Score;
                            window.parent.InitRecord(qMp3Url, qAnswer, true);
                            parentID = question.ParentID;
                            minQueCount = question.MinQueCount;
                            readRound = 0;
                            //学生已读,有记录

                            var existID = -1;
                            $(window.parent.stuAnswerList).each(function (index, value) {
                                if (this.QuestionID == questionID) {//本次有做题记录,记录数组下标
                                    existID = index;
                                    return false;
                                }
                            })
                            if (existID != -1) {
                                var answer = window.parent.stuAnswerList[existID];
                                window.parent.ShowResult(answer.BestScore, 1, 1, answer.BestAnswer);
                                //SetWordsColor();单词颜色处理
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

            //   window.parent.Mp3Obj.PlayerObj.jPlayer("play");
        });
        function resizeWindow() {
            window.parent.autoSetPosition(0, 0);
            $('.M').niceScroll({ touchbehavior: false, autohidemode: false, cursorcolor: "#dcdcdc", cursoropacitymax: 1, cursorwidth: 8 });
        }

        function EndRecord(data) {
            var replayPath = window.parent.KSRecord.GetReplayPath();
            console.log(data);
            readRound++;
            //stuscore = parseInt(data.lines[0].score);
            var totalScore = 0;
            $(data.lines).each(function () {
                totalScore += parseInt(Math.sqrt(this.score) * 10);
            });
            stuscore = totalScore/data.lines.length;
            window.parent.ShowScore(stuscore, readRound, requireRound);
            window.parent.Mp3Obj.NextRecord(true);
            backurl = window.parent.KSRecord.GetReplayPath();
            endTime = new Date().getTime();
            spendtime = (endTime - startTime) / 1000;
            spendTime += spendtime;
            startTime = endTime;//重新计时

            var existID = -1;
            $(window.parent.stuAnswerList).each(function (index, value) {
                if (this.QuestionID == questionID) {//本次有做题记录,记录数组下标
                    existID = index;
                    return false;
                }
            })
            if (existID == -1) {//无做题记录,push
                window.parent.stuAnswerList.push({ QuestionID: questionID, ParentID: parentID, CatalogID: catalogID, Answer: replayPath, BestAnswer: replayPath, QScore:qScore,Score: stuscore, BestScore: stuscore,QuestionContent:questionContent });
            } else {//有做题记录,更新
                window.parent.stuAnswerList[existID] = { QuestionID: questionID, ParentID: parentID, CatalogID: catalogID, Answer: replayPath, BestAnswer: replayPath, QScore: qScore, Score: stuscore, BestScore: stuscore, QuestionContent: questionContent };
            }
            window.parent.showNextA();
            console.log(window.parent.stuAnswerList);

            SetWordsColor(data);

            if (accessType != 3) {
                $.post("?action=UploadAudioFile&Rand=" + Math.random(), { BackUrl: backurl }, function (result) {
                    if (result) {
                        result = eval("(" + result + ")");//JSON.parse
                        if (result.Success) {
                            backurl = result.Data;
                            window.parent.ShowBackPlayer(backurl);
                            if (accessType == 4) {//作业报告——错题重做
                                $.post("?action=SaveStuWrongQue&Rand=" + Math.random(), {
                                    StuTaskID: stuTaskID,
                                    QuestionID: questionID, IsRight: stuscore >= 60 ? 1 : 0
                                }, function (result) {
                                    if (result) {
                                        result = eval("(" + result + ")");//JSON.parse
                                        if (result.Success) {
                                            //window.parent.InitRecord(qMp3Url, qAnswer, false);
                                        } else {
                                            alert(result.Message);
                                        }
                                    }
                                    else {
                                        alert(result.Message);
                                    }
                                });
                            }
                        } else {
                            readRound--;
                            alert(result.Message);
                        }
                    } else {
                        readRound--;
                        alert("保存失败，请重试!");
                    }
                });
            }
                //做作业时保存跟读记录
            else {
                var obj = {};
                var parentScore = 0;
                var heighScore = 0;
                rightCount = 0;
                if (highRecord != null && highRecord != undefined) {
                    heighScore = highRecord.StuScore;
                }
                if (answerList != null) {
                    for (var i = 0; i < answerList.length; i++) {
                        if (answerList[i].IsRight == 1) {
                            rightCount = rightCount + 1;
                        }
                        if (answerList[i].QuestionID != questionID) {
                            parentScore += answerList[i].StuScore;
                        }
                    }
                }
                //已有答案，对比答案表中记录
                if (readRound > requireRound) {
                    //本次得分高于答案记录
                    if (stuscore > heighScore) {
                        obj = {
                            QuestionID: questionID, ParentID: parentID, StuTaskID: stuTaskID,
                            StuScore: stuscore, SpendTime: spendTime, BackUrl: backurl,
                            ParentScore: (parentScore + stuscore) / minQueCount, parentAddTime: spendtime,
                            ParentIsRight: (rightCount + (stuscore < 60 ? 0 : 1)) == minQueCount ? 1 : 0
                        };
                    }
                    else {
                        obj = {
                            QuestionID: questionID, ParentID: parentID, StuTaskID: stuTaskID,
                            StuScore: heighScore, SpendTime: spendTime, Answer: highRecord.Answer, BackUrl: highRecord.Remark,
                            ParentScore: (parentScore + heighScore) / minQueCount, parentAddTime: spendtime, newBackUrl: backurl,
                            ParentIsRight: (rightCount + (heighScore < 60 ? 0 : 1)) == minQueCount ? 1 : 0
                        };
                    }

                    $.post("?action=UpdateStuAnswer&Rand=" + Math.random(), obj, function (result) {
                        if (result) {
                            result = eval("(" + result + ")");//JSON.parse
                            if (result.Success) {
                                if (result.Message == "") {
                                    highRecord = result.Data.Answer;
                                    backurl = result.Data.NewBackUrl;
                                } else {
                                    backurl = result.Data;
                                    readRound--;
                                    alert(result.Message);
                                }
                                window.parent.ShowBackPlayer(backurl);
                            } else {
                                readRound--;
                                alert(result.Message);
                            }
                        } else {
                            readRound--;
                            alert("保存失败，请重试!");
                        }
                    });
                }
                    //刚好达到跟读次数，对比跟读记录，取最高分
                else if (readRound == requireRound) {
                    if (stuscore >= heighScore) {
                        obj = {
                            StuScore: stuscore, SpendTime: spendTime, BackUrl: backurl, StuTaskID: stuTaskID,
                            QuestionID: questionID, ParentID: parentID,
                            ParentScore: (parentScore + stuscore) / minQueCount, ParentIsRight: (rightCount + (stuscore < 60 ? 0 : 1)) == minQueCount ? 1 : 0
                        };
                    } else {
                        obj = {
                            StuScore: heighScore, SpendTime: spendTime, BackUrl: highRecord.BackUrl, StuTaskID: stuTaskID,
                            QuestionID: questionID, ParentID: parentID, Answer: highRecord.StuAnswer,
                            ParentScore: (parentScore + heighScore) / minQueCount, NewBackUrl: backurl, ParentIsRight: (rightCount + (heighScore < 60 ? 0 : 1)) == minQueCount ? 1 : 0
                        };
                    }

                    $.post("?action=AddStuAnswer&Rand=" + Math.random(), obj, function (result) {
                        if (result) {
                            result = eval("(" + result + ")");//JSON.parse
                            if (result.Success) {
                                if (result.Message == "") {
                                    backurl = result.Data.NewBackUrl;
                                    highRecord = result.Data.Answer;
                                } else {
                                    backurl = result.Data;
                                    readRound--;
                                    alert(result.Message);
                                }
                                window.parent.ShowBackPlayer(backurl);
                            } else {
                                readRound--;
                                alert(result.Message);
                            }
                        } else {
                            readRound--;
                            alert("保存失败，请重试!");
                        }
                    });
                }
                    //未达到跟读次数，保存记录表
                else {
                    obj = {
                        StuScore: stuscore, SpendTime: spendTime, BackUrl: backurl, StuTaskID: stuTaskID,
                        QuestionID: questionID, ParentID: parentID, Round: readRound
                    };
                    $.post("?action=InsertReadRecord&Rand=" + Math.random(), obj, function (result) {
                        if (result) {
                            result = eval("(" + result + ")");//JSON.parse
                            if (result.Success) {
                                backurl = result.Data.StuAnswer;
                                if (result.Data.StuScore >= heighScore) {
                                    highRecord = result.Data;
                                }
                                window.parent.ShowBackPlayer(backurl);
                            } else {
                                readRound--;
                                alert(result.Message);
                            }
                        } else {
                            readRound--;
                            alert("保存失败，请重试!");
                        }
                    });
                }
            }
        }

        //设置单词颜色
        function SetWordsColor(data) {
            $("#qContent").find("span").removeAttr("style");//先移除span下所有样式
            var words = [];
            for (var i = 0; i < data.lines.length; i++) {//把所有单词对应的分数存到一个新数组中
                for (var j = 0; j < data.lines[i].words.length; j++) {
                    words.push(data.lines[i].words[j]);
                }
            }
            for (var n = 0; n < words.length; n++) {
                $("#qContent").find("span").each(function () {
                    if ($(this).attr("style") == undefined || $(this).attr("style").lastIndexOf("color") == -1) {//赋过颜色的不再处理
                        if (words[n].type != 4 && $(this).text().lastIndexOf(words[n].text)!=-1) {
                            var score=words[n].score * 10;
                            if (score <60) {
                                $(this).css("color", "#ff7800");
                            } else if (score <80 && score >= 60) {
                                $(this).css("color", "#ffae00 ");
                            } else if (score <90  && score >= 80) {
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
    </script>
    <script>
        $(function () {
            $("#qContent").click(function () {
                window.parent.Mp3Obj.Stop();
                window.parent.Mp3Obj.ClickPlay();
            })

            $("#qImg").click(function () {
                window.parent.Mp3Obj.Stop();
                window.parent.Mp3Obj.ClickPlay();
            })
        })


        //function AutoPlay() {
        //    debugger;
        //    if (window.parent.Mp3Obj) {
        //        window.parent.Mp3Obj.ClickPlay();
        //        return;
        //    } else {
        //        setTimeout("AutoPlay()", 1000)
        //    }
        //}       
    </script>

</body>
</html>
