select c.CatalogName �Ծ���,b.BookName ���,t.��������,t.������� from 
(SELECT CatalogID,SUM(AnswerNum) �������,COUNT(*) ��������
  FROM [FZ_Exampaper].[dbo].[Tb_StuCatalog] group by CatalogID)
  as t 
  inner join [FZ_Exampaper].[dbo].QTb_Catalog c on t.CatalogID=c.CatalogID 
  inner join [FZ_Exampaper].[dbo].QTb_Book b on c.BookID=b.BookID
   order by b.BookID,c.CatalogID