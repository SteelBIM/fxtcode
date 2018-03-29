<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="gjbmobile.aspx.cs" Inherits="WebAppTest.gjbmobile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#args").blur(function () {
                var args = $("#args").val();
                var url = args.indexOf("?");
                var argsArray = args.split("&");
                var html = "";
                for (var i = 0, len = argsArray.length; i < len; i++) {
                    var inputArray = argsArray[i].split("=");
                    html += '<input style="width:100px;" id="' + inputArray[0] + '" name="' + inputArray[0] + '" value="' + inputArray[1] + '" /> <br/>';
                }
                $("#divadd").html(html);
            });
        });
    </script>
</head>
<body>
    <form id="form1" action="http://localhost:17873/handler/getquerylist.ashx?token1=<%=db %>&strcode=<%=strcode %>&strdate=<%=strdate %>"  method="post" enctype="multipart/form-data">
    <div>
        <textarea style="width:420px; height:200px;" id="args"></textarea>
        <br />
        <div id="divadd">
         
        </div>
        <input type="file" name="file2" value="" />
        <input type="submit" onclick="return check()" value="提交"/>
    </div>
    </form>
</body>
</html>
