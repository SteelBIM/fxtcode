<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HallowmasPage.aspx.cs"
    Inherits="Kingsun.SynchronousStudy.Web.HallowmasPage.HallowmasPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>万圣节分享页面</title>
    <link href="css/reset.css" rel="stylesheet" />
    <link href="css/hallowmas.css" rel="stylesheet" />
</head>
<body>
    <div>
        <video id="video" src="#" controls="controls">
            Your browser does not support the video tag.
        </video>
    </div>
    <div class="wrap">
        <div class="module m1">
            <div class="mid">
                <img src="img/userImg.png" id="userImg" alt="img">
                <h4 id="userName"></h4>
                <span id="shareTime"></span>
            </div>
            <div class="sele">
                <h4></h4>
                <span class="spanSt">
						<p>请榜单上的小伙伴添加微信号领取奖品</p>
						<p>微信号：15889670211（长按复制）</p>
					</span>
                <ul>
                    <li class="li1"><a href="javascript:void(0)"><b id="PrizeOne">0</b></a></li>
                    <li class="li2"><a href="javascript:void(0)"><b id="PrizeTwo">0</b></a></li>
                    <li class="li3"><a href="javascript:void(0)"><b id="PrizeThree">0</b></a></li>
                    <li class="li4"><a href="javascript:void(0)"><b id="PrizeFour">0</b></a></li>
                    <li class="li5"><a href="javascript:void(0)"><b id="PrizeFive">0</b></a></li>
                </ul>
            </div>
        </div>
        <div class="module m2">
            <div class="cont">
                <span class="tit"><b id="content" class="b1">萌出一脸血奖</b></span>
                <table id="tbdatagrid">
                </table>
                <a class="prev" id="prev" href="javascript:void(0)"></a>
                <a class="next" id="next" href="javascript:void(0)"></a>
            </div>
        </div>
        <div class="module m3">
            <div class="rule">
                <h4>活动规则</h4>
                <dl>
                    <dt>一、参与方式</dt>
                    <dd>将万圣节主题配音内容分享到朋友圈，邀请好友来赠送糖果。一人只有一颗糖果可赠送哦。</dd>
                    <dt>二、评比方式</dt>
                    <dd>活动结束时，取宝贝每种奖项排名最优配音进行评比（每个用户只可获得一个奖项）。</dd>
                    <dt>三、获奖细则</dt>
                    <dd>参与者当前所获糖果数可在分享的页面查看。</dd>
                    <dt>四、奖品</dt>
                    <dd>深圳版获奖者欢乐谷万圣节夜场门票两张，广州版获奖者长隆万圣节夜场门票一张。</dd>
                    <dt>五、活动时间</dt>
                    <dd>10月28日-11月3日</dd>
                    <dt>六、结果公布</dt>
                    <dd>11月4日查看排行榜糖果数量，每项奖项前十名赢取万圣节大礼。</dd>
                </dl>
                <ul class="ulStyle">
                    <li>若有疑问，添加微信号：tongbuxue01。</li>
                    <li>此活动最终解释权归深圳市方直科技股份有限公司所有。</li>
                </ul>
            </div>
        </div>
        <div class="aDiv">
            <a href="http://tbx.kingsun.cn/downloadList1.html"></a>
        </div>
    </div>
    <script type="text/javascript">var cnzz_protocol = (("https:" == document.location.protocol) ? " https://" : " http://"); document.write(unescape("%3Cspan style='display:none;' id='cnzz_stat_icon_1260656053'%3E%3C/span%3E%3Cscript src='" + cnzz_protocol + "s11.cnzz.com/z_stat.php%3Fid%3D1260656053' type='text/javascript'%3E%3C/script%3E"));</script>

    <script src="../AppTheme/js/jquery-1.11.2.min.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="js/HallowmasInit.js"></script>
</body>
</html>
