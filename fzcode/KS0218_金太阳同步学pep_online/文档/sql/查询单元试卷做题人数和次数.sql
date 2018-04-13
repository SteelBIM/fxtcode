select c.CatalogName 试卷名,b.BookName 册别,t.做题人数,t.做题次数 from 
(SELECT CatalogID,SUM(AnswerNum) 做题次数,COUNT(*) 做题人数
  FROM [FZ_Exampaper].[dbo].[Tb_StuCatalog] group by CatalogID)
  as t 
  inner join [FZ_Exampaper].[dbo].QTb_Catalog c on t.CatalogID=c.CatalogID 
  inner join [FZ_Exampaper].[dbo].QTb_Book b on c.BookID=b.BookID
   order by b.BookID,c.CatalogID