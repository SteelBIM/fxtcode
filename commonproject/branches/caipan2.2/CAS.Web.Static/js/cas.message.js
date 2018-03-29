//右下角消息处理 kevin
var msgid = CAS.GetQuery("msgid");
var commondata = { cityid: CAS.Define.cityid, fxtcompanyid: CAS.Define.fxtcompanyid, userid: CAS.Define.userid };
var muid = 0;
$(function () {
    var data = { type: "topnew", msgid: msgid };
    //关闭
    $("#msg_close").click(function () {
        if (CAS.isC()) {
            external.CloseCurrentWindow();
        } else {
            window.parent.top.HideMsgBox();
        }
    });
    CAS.API({ type: "post", api: "messages.datmessagefileslist",
        data: $.extend({}, commondata, data), callback: function (data) {
            if (data.returntype == 1) {
                data = data.data;
                if (data == null) return;
                muid = data.muid;
                var title = data.title;
                var content = data.message;
                var url = CAS.RootUrlFull;
                var width = 500, height = 400;
                switch (data.messagetype) {
                    case 7004001: //业务 
                    case 7004002: //催办
                        switch (data.businesstypecode) {
                            case 1026001:  //银行委托消息
                                title = "受理委托";
                                width = 800; height = 650;
                                url += "project/acceptentrust.aspx?id=" + data.qid;
                                break;
                            case 1027001: //客户委托
                                if (CAS.Define.systypecode == 1003013) {
                                    url = CAS.APIPage({ api: "page.report.reportlist", data: { id: data.qid, type: "entrust"} });
                                    width = 900;
                                    height = 500;
                                } else {
                                    width = 800; height = 650;
                                    url += "project/acceptentrust.aspx?id=" + data.qid;
                                }
                                break;
                            case 1027002: //客户询价
                                width = 800;
                                height = 450;
                                content = "<label>询价人：</label>" + data.sendusername + "<br>";
                                content += "<label>物&nbsp;&nbsp;业：</label>" + data.message;
                                if (CAS.Define.systypecode == 1003013) {
                                    url += "pages/query/querysituation.aspx?casid=" + data.casid;
                                } else {
                                    url += "query/acceptqueryinfo.aspx?casid=" + data.casid;
                                }
                                break;
                            case 1027003: //查勘
                                url += "query/projectofquery.aspx";
                                break;
                            case 1027004: //报告
                                if (CAS.Define.systypecode == 1003013) {//这里处理报告的名字
                                    url = CAS.APIPage({ api: "page.report.reportlist", data: { id: data.qid, projectname: data.message.slice(3, -3)} });
                                    width = 800;
                                    height = 500;
                                } else {
                                    width = 1000;
                                    height = 650;
                                    url += "project/manegereport.aspx?id=" + data.qid + "&projectname=" + data.title + "&reporttypecode=";
                                }

                                break;
                        }
                        break;
                    case 7004003: //系统消息
                    case 7004005: //咨询
                    case 7004006: //站内信
                    case 7004007: //通知
                        url = CAS.APIPage({ root: CAS.RootUrlFull, api: "page.message.lookznmes", data: { cityid: data.cityid, fxtcompanyid: data.fxtcompanyid,
                            userid: CAS.Define.userid, messagetype: data.messagetype, muid: data.muid, fk_msgid: data.id
                        }
                        });
                        break;
                }
                title = (title.length > 30) ? title.substring(0, 30) + ".."  : title;
                $("#msg_title").html(title);
                $("#msg_content").html(content);

                //传递给c++，窗口自动消失后点击闪闪图标可以直接弹出
                if (CAS.isC()) {
                    external.GetDlgInforFromWeb(url, width, height, 1, 0, 1, 1, title);
                }

                $("#btndetails").click(function () {

                    CAS.API({ type: "post", api: "messages.datmessaguser",
                        data: $.extend(commondata, { type: "updatereadstatus", value: 1, muid: muid }), callback: function (data) {
                            if (data.returntype == 1) {
                                CAS.Dialog({ title: title, min: false, content: "url:" + url, width: width, height: height });
                                //调用C++关闭
                                if (CAS.isC()) {
                                    external.CloseCurrentWindow();
                                } else { //网页这里关闭的应该是层
                                    window.parent.top.HideMsgBox();
                                }
                            }
                        }
                    });
                });
            }
        }
    });
});