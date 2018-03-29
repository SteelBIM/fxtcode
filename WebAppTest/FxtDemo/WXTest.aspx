download<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXTest.aspx.cs" Inherits="WebAppTest.WXTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        
        [{ "title": "公司概况", "src": "", "details": "山东金庆房地产土地评估测绘有限公司前身隶属于潍坊市房产管理局，于二○○三年改制成立了以注册房地产估价师出资为主的有限公司，注册资本壹仟万元。公司拥有房地产评估贰级资质、土地评估A级资质（全国范围执业）、司法鉴定评估资质、工程造价咨询甲级资质。公司是集房地产评估、土地评估、资产评估、工程造价咨询、房地产经纪、测绘于一体的综合咨询机构。 ~       公司被山东省房地产业协会评为2010年度房地产估价&quot;诚信企业&quot;；在2010年度、2011年、2012年度连续三个年度在潍坊市住房与城乡建设局组织的全市房地产估价机构检查中评为第一名。" }, { "title": "组织与文化", "src": "", "details": "组织机构~   公司下设九部一室：房产评估部、土地评估部、资产评估部、工程造价咨询部、测绘部、审核部、行政管理部、财务部、人力资源部、客户拓展部；总经理室。" }, { "title": "企业文化", "src": "", "details": "金庆愿景：做评估行业的领跑者~    金庆价值观： 做专、做精、做强 ~    金庆宗旨：聚专为台 、赢得共赢 ~    金庆精神：严谨、诚信、专业、服务、创新 ~    金庆服务承诺：服务、服务、专业服务；金庆评估，与您同步。" }, { "title": "资质与荣誉", "src": "", "details": "公司资质~   中国土地估价师团体会员~   房地产评估二级资质~   土地评估A级资质（全国范围执业）~   司法鉴定评估资质、~   工程造价咨询甲级资质~   公司现拥有十七名专职房地产估价师~   公司现拥有九名专职土地估价师" }, { "title": "合作伙伴", "src": "", "details": "政府~   潍坊市住房和城乡建设局~   昌乐住房和城乡建设局 ~   临朐住房和城乡建设局~   安丘住房和城乡建设局 ~   昌邑住房和城乡建设局 ~   高新区财政局~   坊子区财政局 ~   寒亭区财政局 ~   潍坊市财政投资评审中心~~银行~   中国银行~   中国工商银行~   潍坊银行~   中国建设银行~   中国民生银行~   华夏银行~   中国农业银行~   中国邮政储蓄银行~   招商银行~   恒丰银行~   中信银行~   浦发银行~~司法~   潍坊市中级人民法院~~企业~   潍坊市国有资产经营投资公司"}]
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        &nbsp;<asp:Label ID="Label1" runat="server" Text="操作："  Height="20px"></asp:Label>
&nbsp;<asp:DropDownList ID="DropDownList1" runat="server" Height="20px" Width="100px">
            <asp:ListItem Value="text">发文本消息</asp:ListItem>
            <asp:ListItem Value="image">发图片消息</asp:ListItem>
            <asp:ListItem Value="CLICK">点击</asp:ListItem>
            <asp:ListItem Value="subscribe">关注</asp:ListItem>
            <asp:ListItem Value="unsubscribe">取消关注</asp:ListItem>
            <asp:ListItem Value="location">地理位置消息</asp:ListItem>
        </asp:DropDownList>
&nbsp;&nbsp;
        <asp:Label ID="Label2" runat="server" Text="事件类型："  Height="20px" ></asp:Label>
        <asp:DropDownList ID="DropDownList2" runat="server" Height="20px" Width="100px">
            <asp:ListItem Value="gjb_company_introduce">公司介绍</asp:ListItem>
            <asp:ListItem Value="gjb_company_business_area">业务范围</asp:ListItem>
            <asp:ListItem Value="gjb_company_classic_case">经典案例</asp:ListItem>
            <asp:ListItem Value="gjb_company_contact">联系方式</asp:ListItem>
            <asp:ListItem Value="gjb_company_feedback">意见反馈</asp:ListItem>
            <asp:ListItem Value="gjb_house_enquiry">住宅询价</asp:ListItem>
            <asp:ListItem Value="gjb_enquiry">询价</asp:ListItem>
            <asp:ListItem Value="gjb_other_enquiry">其它询价</asp:ListItem>
            <asp:ListItem Value="gjb_business_progress">业务进度</asp:ListItem>
            <asp:ListItem Value="gjb_online_entrust">委托业务</asp:ListItem>
            <asp:ListItem Value="gjb_oa_pend">OA</asp:ListItem>
            <asp:ListItem Value="gjb_oa_query">查价</asp:ListItem>
        </asp:DropDownList>
        &nbsp;
       微信OpenID: 
        <asp:DropDownList ID="DropDownList3" runat="server">
            <asp:ListItem Value="o6Uk8t3v7vn2jtDlg7kMDSBG9A-k">瞿客户经理</asp:ListItem>
            <asp:ListItem Value="ocjayjhOyCFo3D5hN-Elr0jsXp3o">银行客户经理</asp:ListItem>
            <asp:ListItem Value="ocjayjk8DEduAhjbOCeOvvNuzOxc">业务员</asp:ListItem>
            <asp:ListItem Value="ocjayjk8DEduAhjbOCeOvvNuzOtyt">账号测试</asp:ListItem>
            <asp:ListItem Value="oM1WKuAI3ABLw9ejPpw42pTNofDw">估价师</asp:ListItem>
        </asp:DropDownList>
&nbsp;
评估机构: 
        <asp:DropDownList ID="DropDownList4" runat="server">
            <asp:ListItem Value="gh_8e42e02815ef">房讯通</asp:ListItem>
            <asp:ListItem Value="gh_8e42e140224wx">微信测试</asp:ListItem>
            <asp:ListItem Value="gh_8e42e02815ad">测试机构</asp:ListItem>
        </asp:DropDownList>
&nbsp;
        <br />
        <asp:TextBox ID="sendTb" runat="server" Height="247px" TextMode="MultiLine" 
            Width="429px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="acceptTb" runat="server" Height="248px" TextMode="MultiLine" 
            Width="313px"></asp:TextBox>
        <br />
        
        <asp:Button ID="Button1" runat="server" Text="提交" onclick="Button1_Click" />
    &nbsp;&nbsp;
        <asp:Button ID="btnSend" runat="server" onclick="btnSend_Click" Text="发消息" />
    </div>
    </form>
</body>
</html>
