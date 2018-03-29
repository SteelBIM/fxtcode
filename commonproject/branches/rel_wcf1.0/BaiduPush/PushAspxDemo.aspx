﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PushAspxDemo.aspx.cs" Inherits="BaiduPush.PushAspxDemo" %>
<!-- 
    Copyright (c) 2011 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
-->

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
    ApiKey:&nbsp&nbsp&nbsp<asp:TextBox ID="TBAK" Text="jmfhQD6RLLi16DrZaE2wkluv" runat="server" Width="400px" ></asp:TextBox>
    <br />
    SecretKey:<asp:TextBox ID="TBSK" Text="XQnl5CK9fTGk94zLttugl9i8nGi4ymiU" runat="server" Width="400px"></asp:TextBox>
    <br />
    
    <br />
        Send Type:
    <br />
    &nbsp<asp:RadioButton ID="RBUnicast" runat="server" Text="Unicast" GroupName="rbCastType"/>
    
    &nbsp UserId:<asp:TextBox ID="TBUserId" Text="688520328761376249" runat="server"></asp:TextBox>
    
    ChannelId:<asp:TextBox ID="TBChannelID" Text="4498473584697613830" runat="server"></asp:TextBox>
    
    <br />
    &nbsp<asp:RadioButton ID="RBMulticast" runat="server" Text="Multicast" GroupName="rbCastType"/>

    Tag:&nbsp&nbsp&nbsp<asp:TextBox ID="TBTag" runat="server"></asp:TextBox>
    
    <br />
    &nbsp<asp:RadioButton ID="RBBroadcast" runat="server" Text="Broadcast" GroupName="rbCastType" Checked="True"/>

    <br />

    <br />
        Device Type(IOS Only Support Notification):
    <br />
    &nbsp<asp:RadioButton ID="RBAndroid" runat="server" Text="Android" GroupName="rbDeviceType" Checked=true/>
    
    &nbsp<asp:RadioButton ID="RBIOSPRO" runat="server" Text="IOS(Production)" GroupName="rbDeviceType"/>

    &nbsp<asp:RadioButton ID="RBIOSDEV" runat="server" Text="IOS(Developer)" GroupName="rbDeviceType"/>

    <br />
     <br />
        Message Type:
    <br />
    &nbsp<asp:RadioButton ID="RbNotify" runat="server" Text="Notification" GroupName="rbMessage"  Checked="True"/>
    <br />
    &nbsp&nbsp&nbsp Title:<br />
    &nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="TBTitle" Text="标题标题标题" runat="server" Width="400px"></asp:TextBox>
    <br />
    &nbsp&nbsp&nbsp Description:<br />
    &nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="TBDescription" Text="内容内容" runat="server" Width="400px"></asp:TextBox>
    <!--
    Builder Id:
    <asp:TextBox ID="TBBuilderId" runat="server" Width="40px" Text="0"></asp:TextBox>
    Basic Id:
    <asp:TextBox ID="TBBasicId" runat="server" Width="40px" Text="0"></asp:TextBox>
    OpenType:
    <asp:TextBox ID="TBOpenType" runat="server" Width="40px" Text="0"></asp:TextBox>
    user_confirm:
    <asp:TextBox ID="TextBox1" runat="server" Width="40px" Text="0"></asp:TextBox>
    OpenType:
    <asp:TextBox ID="TextBox2" runat="server" Width="40px" Text="0"></asp:TextBox>
    url:
    <asp:TextBox ID="TextBox3" runat="server" Width="40px" Text="0"></asp:TextBox>
    pkg_content:
    <asp:TextBox ID="TextBox4" runat="server" Width="40px" Text="0"></asp:TextBox>
    custom_content:
    <asp:TextBox ID="TextBox5" runat="server" Width="40px" Text="0"></asp:TextBox>
    
    -->

    <br />
    &nbsp<asp:RadioButton ID="RbMessage" runat="server" Text="Message" GroupName="rbMessage" />
    <br />
    &nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="TBMessage" runat="server" Width="400px"></asp:TextBox>
    <br />
    
    <br />
    <br />
    <asp:Button ID="BTNSend" runat="server" onclick="BtnSend_Click" 
        Text="Send Notification/Message" />
    <br />
    <br />
    Response:<br />
    <asp:TextBox ID="TextBoxResponse" runat="server" Height="200px" Width="400px" TextMode="MultiLine"></asp:TextBox>
    </form>
</body>
</html>