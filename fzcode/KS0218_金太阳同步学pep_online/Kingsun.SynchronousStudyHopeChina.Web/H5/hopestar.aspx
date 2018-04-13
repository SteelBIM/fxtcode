<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hopestar.aspx.cs" Inherits="Kingsun.SynchronousStudyHopeChina.Web.H5.hopestar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>开始报名</title>
    <script src="js/jquery-1.10.2.min.js"></script>
    <link rel="stylesheet" type="text/css" href="css/reset.css"/>
    <link rel="stylesheet" type="text/css" href="css/tbxSkin.css"/>
    <script src="../AppTheme/js/jquery.json-2.4.js"></script>
    <script src="../AppTheme/js/jquery.cookie.js"></script>
    <script src="../AppTheme/js/CommonDB.js"></script>
    <script src="../Scripts/Management/UserInfoManagement.js"></script>
    <script src="js/popupControl.js"></script>
    <script src="js/JsBridge.js"></script>
    <script type="text/javascript">
        $(function() {
            $("#btnSubmit").on("click", function() {
                var data = localStorage["UserId"];
                var udata = JSON.parse(data);
                var userid = udata.UserId;

                var userinfo = GetUserinfo(userid);
                if (userinfo != null) {
                    var result = UserinfoManage.AddUserinfo(userinfo.Userid, userinfo.Username, userinfo.Parentname, userinfo.Phone, userinfo.Teachername, userinfo.Teacherphone, userinfo.Schoolname, userinfo.Period, userinfo.Grade, userinfo.Isjoin, userinfo.AccountName);
                    if (result.Success) {
                        //alert("恭喜你,报名成功");
                        window.location.href = "stargame.aspx?error=0";
                    } else {
                        ShowMessage("报名失败,请重试");
                        return null;
                    }
                }
            });
        });

        function ShowMessage(MessageStr) {
            data = {
                "data": {
                    "MessageStr": MessageStr

                }
            };
            window.WebViewJavascriptBridge.callHandler(
                'loadFailed', data, function (responseData) {

                }
            );
        }

        function GetUserinfo(userid) {
            var txtName = $("#txtName").val();
            var txtPName = $("#txtPName").val();
            var txtPMobile = $("#txtPMobile").val();
            var txtTName = $("#txtTName").val();
            var txtTMobile = $("#txtTMobile").val();
            var txtSchoolName = $("#txtSchoolName").val();
            var txtGroup = $("#txtGroup").val();
            var isJoin = "";
            if ($('#istrue').hasClass("choosed")) {
                isJoin = "true";
            } else if ($('#isfasle').hasClass("choosed")) {
                isJoin = "false";
            } else {
                isJoin = "";
            }

            var errortext;

            if (txtName == "" || txtName == undefined) {
                ShowMessage("姓名不能为空");
                return null;
            } else {
                errortext = Common.ValidateTxt(txtName);
                if (errortext != '') {
                    ShowMessage(errortext);
                    return null;
                }
            }

            if (txtPName == "" || txtPName == undefined) {
                ShowMessage("父母姓名不能为空");
                return null;
            } else {
                errortext = Common.ValidateTxt(txtPName);
                if (errortext != '') {
                    ShowMessage(errortext);
                    return null;
                }
            }

            if (txtPMobile == "" || txtPMobile == undefined) {
                ShowMessage("父母联系方式不能为空");
                return null;
            } else {
                errortext = Common.ValidateTxt(txtPMobile);
                if (errortext != '') {
                    ShowMessage(errortext);
                    return null;
                } else {
                    if (!Common.Validate.IsMobileNo(txtPMobile)) {
                        ShowMessage("请输入正确的手机号码");
                        return null;
                    }
                }
            }

            if (txtTName == "" || txtTName == undefined) {
                ShowMessage("老师姓名不能为空");
                return null;
            } else {
                errortext = Common.ValidateTxt(txtTName);
                if (errortext != '') {
                    ShowMessage(errortext);
                    return null;
                }
            }


            if (txtTMobile == "" || txtTMobile == undefined) {
                ShowMessage("老师联系方式不能为空");
                return null;
            } else {
                errortext = Common.ValidateTxt(txtTMobile);
                if (errortext != '') {
                    ShowMessage(errortext);
                    return null;
                } else {
                    if (!Common.Validate.IsMobileNo(txtTMobile)) {
                        ShowMessage("请输入正确的手机号码");
                        return null;
                    }
                }
            }

            if (txtSchoolName == "" || txtSchoolName == undefined) {
                ShowMessage("学校不能为空");
                return null;
            } else {
                errortext = Common.ValidateTxt(txtSchoolName);
                if (errortext != '') {
                    ShowMessage(errortext);
                    return null;
                }
            }

            if (txtGroup == "" || txtGroup == undefined) {
                ShowMessage("参赛组别不能为空");
                return null;
            } else {
                errortext = Common.ValidateTxt(txtGroup);
                if (errortext != '') {
                    ShowMessage(errortext);
                    return null;
                }
            }

            if (isJoin == "" || isJoin == undefined) {
                ShowMessage("请选择是否已参加2017年度CCTV“希望之星”英语风采大赛");
                return null;
            }

            var data = {
                Userid:userid,
                Username:txtName,
                Parentname:txtPName,
                Phone:txtPMobile,
                Teachername:txtTName,
                Teacherphone:txtTMobile ,
                Schoolname:txtSchoolName,
                Period:txtGroup,
                Grade:0 ,
                Isjoin:isJoin ,
                AccountName:""
            };

            return data;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="skell">
            <div class="header">
                <a class="return" href="#" onclick="Finish()"></a>
                <b>2017年度希望之星北京赛区报名</b>
            </div>
            <div class="content">
                <div class="lpart">
                    <p>姓名<em>Name</em></p>
                    <input type="text" value="" id="txtName" />
                </div>
                <div class="lpart">
                    <p>家长姓名<em>Parents'Name</em></p>
                    <input type="text" value=""id="txtPName" />
                </div>
                <div class="lpart">
                    <p>联系电话<em>Parents' Mobile</em></p>
                    <input type="text" value=""id="txtPMobile" />
                </div>
                <div class="lpart">
                    <p>老师姓名<em>Tutor's Name</em></p>
                    <input type="text" value=""id="txtTName" />
                </div>
                <div class="lpart">
                    <p>老师电话<em>Tutor's Mobile</em></p>
                    <input type="text" value=""id="txtTMobile" />
                </div>
                <div class="lpart">
                    <p>学校<em>School</em></p>
                    <input type="text" value="" id="txtSchoolName"/>
                </div>
                <div class="lpart">
                    <p>参赛组别<em>Group</em> <input type="hidden" id="txtGroup" /></p>
                    <div class="group" onclick="popupShow();">请选择
                    </div>

                </div>
                <p class="pt">是否已参加2017年度CCTV“希望之星”英语风采大赛</p>
                <div class="btn">
                    <a class="sp1" onclick="btnState(this);" id="istrue">是</a>
                    <a class="sp2" onclick="btnState(this);" id="isfasle" >否</a>
                </div>
                <a class="refer" id="btnSubmit">确认报名</a>
                <div class="placeholder"></div>
            </div>
        </div>
        <div class="popupGroup" style="display: none">
            <div class="shadow"></div>
            <div class="gr">
                <img src="images/popupBack.png" />
                <div>
                    <p><a onclick="selectGroup(this);">请选择</a>
                    </p>
                    <p><a onclick="selectGroup(this);">小学1-2年级（F）</a>
                    </p>
                    <p><a onclick="selectGroup(this);">小学3-4年级（F）</a>
                    </p>
                    <p><a onclick="selectGroup(this);">小学5-6年级（F）</a>
                    </p>
                    <p><a onclick="selectGroup(this);">初中（F）</a>
                    </p>
                    <p><a onclick="selectGroup(this);">高中（F）</a>
                    </p>
                    <a class="close" onclick="popupHide();">关闭</a>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
