<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebAppTest.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(function () {
             $("#buildarea").keyup(function (e) {
                 var tar = e.target;
                 setTimeout(function () {
                     var v = $(tar).val();
                     load(tar, v);
                 }, 300);
             });

             function load(tar, v) {
                 if (v != tar.historyValue) {
                     console.log(v);
                     tar.historyValue = v;
                 }

             }
         });
     </script>
</head>
<body>
    <div>
        <input id="buildarea"/>
    </div>
</body>
</html>
