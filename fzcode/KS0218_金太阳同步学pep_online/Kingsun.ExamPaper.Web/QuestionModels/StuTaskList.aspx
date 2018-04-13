<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StuTaskList.aspx.cs" Inherits="Kingsun.SunnyTask.Web.Student.StuTaskList" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>口语测评</title>
    <link href="css/catalogue.css" rel="stylesheet" type="text/css">
    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
</head>
<body>

    <div class="content">
        <div class="header">
            <div class="header_nr">
                <h2><%=Catalogs.FirstOrDefault()!=null?Catalogs.FirstOrDefault().BookName:"" %></h2>
            </div>
        </div>
        <div class="choose">
            <div class="main">
                <ul class="ul1" id="scroll-1">
                    <% 
                        foreach (var module in Catalogs.Where(o => o.ParentID == 0))
                        {
                            if (!Catalogs.Any(o => o.ParentID == module.CatalogID))//该模块下没有单元,不显示
                            {
                                continue;
                            }
                    %>
                    <li>
                        <a><%=module.CatalogName %></a>
                        <ul>
                            <%foreach (var unit in Catalogs.Where(o => o.ParentID == module.CatalogID))
                              {
                                  Session["FreeCata"] = Convert.ToInt32(Session["FreeCata"]) >0? Convert.ToInt32(Session["FreeCata"]):unit.CatalogID;//记录第一个单元用于免费使用
                                  var catas = Catalogs.Where(o => o.ParentID == unit.CatalogID).OrderBy(o => o.Sort);
                            %>
                            <li onclick="window.location.href='DoQuestion.aspx?CatalogId=<%=catas.Any()?catas.First().CatalogID:0 %>&UnitCatalogId=<%=unit.CatalogID %>&type=word&unitName=<%=unit.CatalogName.Replace("'",string.Empty) %>'"><%=unit.CatalogName %> </li>
                            <%                               
                              } 
                            %>
                        </ul>
                    </li>
                    <%
                        }
                    %>


                    <%-- <li>
                        <a>Module 1 Using my five senses</a>
                        <ul>
                            <li>Unit 1 Colours</li>
                            <li>Unit 2 Tastes</li>
                            <li>Unit 3 Sounds</li>
                        </ul>
                    </li>
                    <li>
                        <a>Module 1 Using my five senses</a>
                        <ul>
                            <li>Unit 1 Colours</li>
                            <li>Unit 2 Tastes</li>
                            <li>Unit 3 Sounds</li>
                        </ul>
                    </li>
                    <li>
                        <a>Module 1 Using my five senses</a>
                        <ul>
                            <li>Unit 1 Colours</li>
                            <li>Unit 2 Tastes</li>
                            <li>Unit 3 Sounds</li>
                        </ul>
                    </li>--%>
                </ul>
            </div>
        </div>
        <div class="footer">
        </div>
</body>
</html>

