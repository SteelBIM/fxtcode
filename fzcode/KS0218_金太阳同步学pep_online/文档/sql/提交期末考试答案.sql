declare @StuCatID varchar(50)
declare @IsBest int
set @StuCatID=''
set @IsBest=0
select @StuCatID=StuCatID,@IsBest=(case when BestTotalScore<21 then 1 else 0 end) 
from Tb_StuCatalog 
where StuID='756090643' and CatalogID=3
-----用户这套试卷有做题记录-------
if @StuCatID<>'' 
begin	
	----最佳成绩----
	if @IsBest=1 
	begin
		-------------更新最佳成绩-----------------
		update Tb_StuCatalog with(rowlock) 
		set TotalScore=21,DoDate='2017-05-04 17:15:49',BestTotalScore=(case when @IsBest=1 then 21 else BestTotalScore end) 
		where StuCatID=@StuCatID; 
		
		-----删除之前记录-------
		delete Tb_StuAnswer
		where StuID='756090643' and CatalogID=3
		
		-----插入做题记录-------
		insert into Tb_StuAnswer
		(
			StuAnswerID,StuCatID,StuID,CatalogID,QuestionID,ParentID,Answer,IsRight,Score,BestAnswer,BestIsRight,BestScore
		) 
		values(
			cast(newid() as varchar(50)),@StuCatID,'756090643',3,'d1aba15c-18d1-479e-88f7-b960a46f9911','85dab573-7cbd-435e-8de8-5c92f9e24507'
			,'',0,2,'',0,2
		)
	end
end
else 
begin 
	set @StuCatID=cast(newid() as varchar(50));	
	----------插入成绩------------
	insert into Tb_StuCatalog
	(
		StuCatID,StuID,CatalogID,TotalScore,BestTotalScore,DoDate
	) 
	values(
		@StuCatID,'756090643',3,21,21,'2017-05-04 17:15:49'
	)
	----------插入做题记录------------
	insert into Tb_StuAnswer
	(
		StuAnswerID,StuCatID,StuID,CatalogID,QuestionID,ParentID,Answer,IsRight,Score,BestAnswer,BestIsRight,BestScore
	) 
	values
	(
		cast(newid() as varchar(50)),@StuCatID,'756090643',3,'d1aba15c-18d1-479e-88f7-b960a46f9911',
		'85dab573-7cbd-435e-8de8-5c92f9e24507','',0,2,'',0,2
	)	
	-----试卷做题次数+1-------
	update QTb_Catalog set AnswerNum=(case when AnswerNum is null then 0 else AnswerNum)+1 
	where CatalogID=3;	
end;
-----用户试卷做题次数+1-------
update Tb_StuCatalog set AnswerNum=(case when AnswerNum is null then 0 else AnswerNum)+1 
where StuID='756090643' and CatalogID=3; 



