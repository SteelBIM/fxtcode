<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fileupload.aspx.cs" Inherits="WebAppTest.fileupload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#key").keyup(function (e) {
                var tar = e.target
                tar.defValue = "",
                $this = $(tar),
                v = $this.val(),
                re = /^$[0-9]/ig;

                if (re.test(v)) {
                    tar.defvalue = v;
                    console.log("true");
                } else {
                    $this.val(tar.defValue)
                    console.log("false");
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server" method="post" enctype="multipart/form-data">
    <input type="text" id="key"/>
    </form>
</body>
</html>
