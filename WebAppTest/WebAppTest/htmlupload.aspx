<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="htmlupload.aspx.cs" Inherits="WebAppTest.htmlupload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(function () {
             $("#Upload").click(function () {
                 alert(0);
                 var data = new FormData(document.forms[0]);
                 
                 $.ajax({
                     type: 'POST',
                     url: '/upload.ashx',
                     data: data,
                     /** 
                     *必须false才会自动加上正确的Content-Type 
                     */
                     contentType: false,
                     /** 
                     * 必须false才会避开jQuery对 formdata 的默认处理 
                     * XMLHttpRequest会对 formdata 进行正确的处理 
                     */
                     processData: false
                 })
             });
         });
     </script>
</head>
<body>
    <form id="form1" enctype="multipart/form-data" method="post" runat="server">
    <div class="row">
      <label for="fileToUpload">Select a File to Upload</label><br />
      <input type="file" name="fileToUpload" id="fileToUpload"/>
    </div>
    <div id="fileName"></div>
    <div id="fileSize"></div>
    <div id="fileType"></div>
    <div class="row">
      <input type="button"  id="Upload" value="提交"/>
    </div>
    <div id="progressNumber"></div>
    </form>
</body>
</html>
