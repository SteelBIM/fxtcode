<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoQuestion.aspx.cs" Inherits="Kingsun.SunnyTask.Web.QuestionModels.DoQuestion" %>

<!DOCTYPE HTML>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>口语评测</title>
    <link rel="Shortcut Icon" href="../favicon.ico" />
    <link href="../App_Themes/css/base.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/css/task.css" rel="stylesheet" type="text/css" />
    <script src="../App_Themes/js/jquery-1.10.2.min.js"></script>
    <!--[if IE 6]>
    <script src="../App_Themes/js/DD_belatedPNG_0.0.8a-min.js"></script>
    <script>
      /* EXAMPLE */
      DD_belatedPNG.fix('.topC');
      DD_belatedPNG.fix('.topC a');
      DD_belatedPNG.fix('.btmC');
      DD_belatedPNG.fix('.btmC a.prevA');
      DD_belatedPNG.fix('.btmC a.nextA');
      DD_belatedPNG.fix('.btmC .titCont em');
      /* string argument can be any CSS selector */
      /* .png_bg example is unnecessary */
      /* change it to what suits you! */
    </script>
    <![endif]-->
</head>
<body class="template">
    <div class="wrap">
        <div class="main">
            <div class="" id="divrecord" style="width: 1px; height: 1px; position: relative; z-index: 999; left: 400px; top: 250px">
            </div>
            <div class="box">
                <div class="leftC" style="z-index: 999">
                    <ul id="menu">
                        <%
                            foreach (var c in this.Catalogs)
                            {
                                string img = "", titile = "", className = "", type = "";
                                int star = 0;
                                var stuCata = StuCatalogs.FirstOrDefault(o => o.CatalogID == c.CatalogID);
                                if (stuCata != null)
                                {
                                    var score=stuCata.BestTotalScore;
                                    star = score < 60 ? 1 : (score < 80 ? 2 : (score < 90 ? 3 : 4));
                                }
                                string starImage="url(/App_Themes/images/star"+star+".png)";
                                if (c.CatalogName.Contains("单词"))
                                {
                                    img = "danci1.png";
                                    titile = "Look and learn";
                                    className = "danci";
                                    type = "word";
                                }
                                else if (c.CatalogName.Contains("句子"))
                                {
                                    img = "juzi1.png";
                                    titile = "Listen and say";
                                    className = "juzi";
                                    type = "sentence";
                                }
                                else if (c.CatalogName.Contains("课文"))
                                {
                                    img = "kewen1.png";
                                    titile = "Enjoy a story";
                                    className = "kewen";
                                    type = "story";
                                }
                                else
                                {
                                    img = "yuyin1.png";
                                    titile = "Learn the sounds";
                                    className = "yuyin";
                                    type = "sound";
                                }                            
                             
                        %>
                        <li <%=Request.QueryString["type"]==type?"class ='on'":"" %>>
                            <a onclick="return CheckSubmit()" class="danci" href="DoQuestion.aspx?CatalogId=<%=c.CatalogID %>&UnitCatalogId=<%=c.ParentID %>&type=<%=type %>">
                                <img src="../App_Themes/images/<%=img %>" alt="加载图片出错" />
                                <h3><%=c.CatalogName %></h3>
                                <p><%=titile %></p>
                            </a>
                            <p class="xing" <%="style=background-image:"+starImage %>></p>
                            <p class="xian"></p>
                        </li>
                        <%
                          }
                        %>
                    </ul>
                </div>
                <div class="topC">
                    <a class="returnA" href="<%="StuTaskList.aspx?userid="+Request.Cookies["UserId"].Value+"&bookid="+Request.Cookies["BookID"].Value %>">&nbsp;</a>
                    <%-- <a class="helpA" id="helpA" style="display:none" href="javascript:openHelpDialog()">&nbsp;</a>--%>
                    <h2 class="topic" id="topicTitle"><%=UnitCatalog.CatalogName %></h2>
                    <span class="accomplishment"></span>
                    <a id="submitA" class="submitA" style="display: none;visibility:hidden" href="javascript:submitTask()">提交</a>
                </div>
                <div class="centerC">
                    <div class="cont">
                        <iframe id="iframe1" name="iframe1" frameborder="0" scrolling="no" allowtransparency="true" style="border: 0; padding: 0; background-color: transparent; width: 71%; margin: 0 auto 0 240px; min-height: 426px;"></iframe>

                    </div>
                </div>
                <div class="btmC">
                    <a id="prevA" class="prevA" style="display: none" href="javascript:prevQuestion()"></a>
                    <a id="nextA" class="nextA" style="display: none" href="javascript:nextQuestion()"></a>

                    <div class="solution" style="display: none">
                    </div>
                    <%-- <div class="titCont" style="display:none"></div>--%>
                    <p class="yu"></p>
                    <div class="titBox" style="display: none"><span></span></div>
                    <ul class="actUl">
                        <li class="li1" style="display: none">
                            <div class="mp3player" id="mp3player">
                            </div>
                        </li>
                        <li class="li2" style="display: none">
                            <a id="aSmall" title="回放"></a>
                            <div id="backplayer"></div>
                        </li>
                    </ul>
                    <div class="ongoing" style="display: none">
                        <%--  <label>录音中</label>--%>
                        <div style="margin-left: 100px" class="progressBar"><span style="width: 90%"></span></div>
                    </div>
                </div>
                <div class="btmC" style="display:none">
                   <a class="again"  href="javascript:submitTask(null,true)">再试一次</a>
                    <a class="continue"   href="javascript:submitTask(true,null)">继续</a>
                    <a class="finish"    href="javascript:submitTask()">提交成绩</a>
                </div>
            </div>
        </div>        
    </div>
     <audio id="audio" src="" autoplay="autoplay" controls="controls" style="display: none"></audio>
    <script src="../Scripts/Common.js" type="text/javascript"></script>
    <script src="../Scripts/Client.js" type="text/javascript"></script>
    <script src="../App_Themes/js/artDialog/artDialog.js?skin=simple" type="text/javascript"></script>
    <script src="../Scripts/Plugins/jplayer/jquery.jplayer.min.js" type="text/javascript"></script>
    <script src="../Scripts/Plugins/KingsunMp3Player.js" type="text/javascript"></script>
    <script src="../Scripts/Plugins/KingRecord/swfobject.js" type="text/javascript"></script>
    <script src="../Scripts/Plugins/KingRecord/KingsunRecord.js" type="text/javascript"></script>
    <link href="../Scripts/Plugins/KingRecord/KingsunRecord.css" rel="stylesheet" />
    <script src="../Scripts/Page/DoQuestion.js" charset="gbk" type="text/javascript"></script>
</body>
</html>
