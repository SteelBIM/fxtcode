<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="WebSpiderRule.Old.test" %>





<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<title>小区列表</title>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" />
<script type="text/javascript" src="../js/lib/jquery-2.1.1.min.js"></script>
<script type="text/javascript" src="../js/lib/layer/layer.js"></script>
<script type="text/javascript" src="../js/app/common.js?v=20160720"></script>
<link href="../css/pure-min.css?v=20160511" rel="stylesheet" type="text/css"/>
<link href="../css/base.css?t=20160511" rel="stylesheet" type="text/css"/>
<script type="text/javascript">
    function performCommunity(obj) {
        layer.open({ type: 2 });
        var communityCode = $(obj).find("input[type='hidden']").val();
        var communityName = $(obj).find("div:first>p.community-name").text();
        var propertyType = $("#propertyType").val();
        var cityCode = $("#cityCode").val();
        window.location.href = "index.do?communityCode=" + communityCode + "&communityName=" + communityName + "&propertyId=" + propertyType + "&cityCode=" + cityCode;
    }
</script>
</head>

<body class="bg-grey2">
    <input type="hidden" id="cityCode" name="cityCode" value="10990">
    <input type="hidden" id="propertyType" name="propertyType" value="92">

	<div class="pure-g ">
		<p class="pure-u-1 bg-grey page-title  font-yh">您想找的小区是</p>
	</div>

	<ul class="pure-g normal-list bg-white" style="margin-top: 0" id="communitys">
		
			
			
				
					<li class="normal-list-item pure-g has-right" onclick="performCommunity(this);"><input type="hidden" value="714659882640372607" />
						<div class="pure-u-15-24 pure-g">
							<p class="pure-u-1 normal-list-item-name community-name over-note">嘉华绿洲</p>
							<p class="pure-u-1 normal-list-item-address over-note">南宁市西乡塘区西乡塘区科园大道52-1号</p>
						</div>
						<div class="pure-g pure-u-7-24" style="text-align: left">
							<div  class="normal-list-item-price icon_duiqi" style=display:none;>
								<i class="icon_gongyu"></i><font  class="normal-list-item-dollor" >¥</font> 0
							</div>
							<div  class="normal-list-item-price icon_duiqi" style=display:none;>
								<i class=icon_bsu></i><font  class="normal-list-item-dollor">¥</font> 0
							</div>
							
							<div class="normal-list-item-MoM">环比上月
							 <font class=text-red>0.0 %</font>
							</div>
							
						</div></li>
				
			

		
	</ul>

</body>
</html>
