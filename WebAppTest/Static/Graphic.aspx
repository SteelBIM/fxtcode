<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Graphic.aspx.cs" Inherits="Static.Graphic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style/graphic.css" rel="stylesheet" type="text/css" />
</head>
<body>
<form id="form1" runat="server">

<div class="media_preview_area">
    <div class="appmsg editing" style="margin-top:50px;margin-left:30px;">
        <div id="js_appmsg_preview" class="appmsg_content" >
            <div id="appmsgItem1" data-fileid="" data-id="1" class="js_appmsg_item  has_thumb" >
                <h4 class="appmsg_title"><a onclick="return false;" href="javascript:void(0);" target="_blank">asdfsaf</a></h4>
                <div class="appmsg_info">
                    <em class="appmsg_date"></em>
                </div>
                <div class="appmsg_thumb_wrp">
                    <img class="js_appmsg_thumb appmsg_thumb" src="img/getimgdata">
                    <i class="appmsg_thumb default">封面图片</i>
                </div>
                <p class="appmsg_desc">asfasfasfasfasfasfasfafafafasfafsafaf</p>
            </div>
        </div>
    </div>
</div>


<div id="js_appmsg_editor" class="media_edit_area">
<div class="appmsg_editor" style="margin-top: 50px;width:500px;">
	<div class="inner">
		<div class="appmsg_edit_item">
			<label for="" class="frm_label">标题</label>
			<span class="frm_input_box"><input type="text" class="frm_input js_title"></span>
		</div>
		<div class="appmsg_edit_item">
			<label for="" class="frm_label">
				<strong class="title">作者</strong>
				<p class="tips l">（选填）</p>
			</label>
			<span class="frm_input_box"><input type="text" class="frm_input js_author "></span>
		</div>
		<div class="appmsg_edit_item">
			<label for="" class="frm_label">
				<strong class="title">封面</strong>
				<p class="js_cover_tip tips r">大图片建议尺寸：360像素 * 200像素</p>
			</label>
			<div class="frm_input_box">
                <div class="upload_box">
                    <div class="upload_area">
                        <a id="js_appmsg_upload_cover" href="javascript:void(0);" onclick="return false;" class="upload_access" width="50" height="22">
                            <i class="icon18_common upload_gray"></i>
                            上传                        </a>
					</div>
                </div>
                <p class="js_cover upload_preview" style="display: block;"><img src="img/getimgdata" width="360px;" height="200px;">
					<a class="js_removeCover" href="javascript:void(0);" onclick="return false;">删除</a>
                </p>
			</div>
            <p class="frm_tips">
                <label for="" class="frm_checkbox_label js_show_cover_pic">
                    <i class="icon_checkbox"></i>
                    <input type="checkbox" class="frm_checkbox" checked="">
                   封面图片显示在正文中 
                </label>
            </p>
		</div>
		<p><a class="js_addDesc" href="javascript:void(0);" onclick="return false;" style="display: none;">添加摘要</a></p>
		<div class="js_desc_area dn appmsg_edit_item" style="display: block;">
			<label for="" class="frm_label">摘要</label>
			<span class="frm_textarea_box"><textarea class="js_desc frm_textarea"></textarea></span>
		</div>
		<div id="js_ueditor" class="appmsg_edit_item content_edit">
            <label for="" class="frm_label">
                <strong class="title">正文</strong>
                <p class="tips r">
                    <em id="js_auto_tips">自动保存：2014/02/22 11:31:17</em> 
                	<a id="js_cancle" style="display: none;" href="javascript:void(0);" onclick="return false;">取消</a>
                </p>
            </label>
			<div id="js_editor" class="edui_editor_wrp edui-default">
	</div>
	<i class="arrow arrow_out" style="margin-top: 0px;"></i>
	<i class="arrow arrow_in" style="margin-top: 0px;"></i>
	</div>
	</div>
	</div>
</div>

</form>
</body>
</html>
