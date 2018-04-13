<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Result.aspx.cs" Inherits="Kingsun.ExamPaper.Web.QuestionModels.Result" %>

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
    <div class="M M2">
        <h2 id="h2">你击败了全国<span>99%</span>的对手</h2>
        <img class="img1" src="../App_Themes/images/star3.png" alt="" />
        <p class="p1">PERFECT!!</p>
        <div class="nr">
            <p class="p2">详细得分（本次共<span>4</span>个单词）</p>
            <ul id="words">               

                    <li><em>
                        <img src="../App_Themes/images/yu.png" alt="" /></em><img src="../App_Themes/images/laba.png" alt="" /><div>
                            <p>Monkey mmmm mmm</p>
                        </div>
                    </li>
                    <li><em></em>
                        <img src="../App_Themes/images/laba.png" alt="" /><div>
                            <p>Frog</p>
                        </div>
                    </li>
                    <li><em></em>
                        <img src="../App_Themes/images/laba.png" alt="" /><div>
                            <p>Live</p>
                        </div>
                    </li>
                    <li><em></em>
                        <img src="../App_Themes/images/laba.png" alt="" /><div>
                            <p>Wolf</p>
                        </div>
                    </li>
            </ul>
        </div>
    </div>
    <audio id="audio" src="" autoplay="autoplay" controls="controls" style="display: none"></audio>
    <script type="text/javascript" src="../Scripts/Common.js"></script>
    <script src="../Scripts/jquery.json-2.4.js"></script>
    <script>
        function Init() {
           
            var stuAnswers = parent.stuAnswerList;
            var count = parent.TaskQuestionList.length;
            $(".p2").html('详细得分（本次共<span>' + count + '</span>个题目)' );
          
            $("#words").html("");
            for (var i = 0; i < stuAnswers.length; i++) {
                var yuCount = parent.YuCount(stuAnswers[i].BestScore);
                var yuHtml = "";
                for (j = 0; j < yuCount; j++) {
                    yuHtml += '<img src="/App_Themes/images/yu.png">';
                }
                //$("#words").append(' <li mp3="' + stuAnswers[i].Answer + '" onclick="play(this)"><img  src="../App_Themes/images/laba.png" alt="" /><span>' + stuAnswers[i].QuestionContent + '</span><i style="float:right">' + yuHtml + '</i>');
                $("#words").append(' <li mp3="' + stuAnswers[i].Answer + '" onclick="play(this)"><em>' + yuHtml + '</em><img src="../App_Themes/images/laba.png" alt="" /><div><p>' + stuAnswers[i].QuestionContent + '</p></div></li>');
            }
            Stars(stuAnswers);
        }

        function play(obj) {
            $("#audio").attr("src", $(obj).attr("mp3"));
        }

        function Stars(answers) {
            var realScore = 0;//实际得分
            var qScore = 0;//题目总分
            var divideScore = 0;
            for (var i = 0; i < answers.length; i++) {
                realScore += answers[i].QScore * answers[i].BestScore / 100;//该题实际得分
                qScore += answers[i].QScore;//该题实际分数
            }
            divideScore = realScore / qScore * 100;//折算成100分制
            var star = 0;
            if (divideScore < 60) {
                star = 1;
                $(".p1").html("good!");
            } else if (divideScore >= 60 && divideScore < 80) {
                $(".p1").html("great!");
                star = 2;
            } else if (divideScore >= 80 && divideScore < 90) {
                star = 3;
                $(".p1").html("excellent!");
            } else {
                star = 4;
                $(".p1").html("perfect!");
            }
            $(".img1").attr("src", "../App_Themes/images/star" + star.toString() + ".png");
          //  parent.SetStars(star);暂不更新左边星星数
            //$($(".xing")[3]).css("background-image","url(/app_themes/images/star1.png)")
            //击败全国??%的用户
            if (divideScore < 60) {
                $("#h2").html("你击败了全国<span>37%</span>的对手");
            } else if (divideScore >= 60 && divideScore < 70) {
                $("#h2").html("你击败了全国<span>60%</span>的对手");
            } else if (divideScore >= 70 && divideScore < 85) {
                $("#h2").html("你击败了全国<span>85%</span>的对手");
            }
            else if (divideScore >= 85 && divideScore < 95) {
                $("#h2").html("你击败了全国<span>93%</span>的对手");
            } else {
                $("#h2").html("你击败了全国<span>99%</span>的对手");
            }
        }

        $(function () {
            Init();
        })
    </script>
</body>
</html>
