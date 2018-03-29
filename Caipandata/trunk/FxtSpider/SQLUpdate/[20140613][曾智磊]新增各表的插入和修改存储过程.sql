CREATE PROC SysData_Company_Insert--
(
    @CompanyName varchar(900),
    @NowID bigint output
)
as
begin
    Insert into SysData_Company with(rowlock)(CompanyName) 
    values (@CompanyName)
    set @NowID=scope_identity()
end
GO
CREATE PROC SysData_CompanyArea_Insert--
(
    @CompanyAreaName varchar(900),
    @NowID bigint output
)
as
begin
    Insert into SysData_CompanyArea with(rowlock)(CompanyAreaName) 
    values (@CompanyAreaName)
    set @NowID=scope_identity()
end
go
CREATE PROC SysData_Project_Insert--
(
    @ProjectName nvarchar(500),
    @CityId int,
    @WebId int,
    @NowID bigint output
)
as
begin
    Insert into SysData_Project with(rowlock)(ProjectName,CityId,WebId) 
    values (@ProjectName,@CityId,@WebId)
    set @NowID=scope_identity()
end
go
CREATE PROC SysData_Area_Insert--
(
    @AreaName varchar(800),
    @CityId int,
    @WebId int,
    @NowID bigint output
)
as
begin
    Insert into SysData_Area with(rowlock)(AreaName,CityId,WebId) 
    values (@AreaName,@CityId,@WebId)
    set @NowID=scope_identity()
end
go
CREATE PROC SysData_SubArea_Insert--
(
    @SubAreaName varchar(800),
    @CityId int,
    @WebId int,
    @NowID bigint output
)
as
begin
    Insert into SysData_SubArea with(rowlock)(SubAreaName,CityId,WebId) 
    values (@SubAreaName,@CityId,@WebId)
    set @NowID=scope_identity()
end
go

CREATE PROC SysData_装修_Insert--
(
    @zxName varchar(800),
    @NowID int output
)
as
begin
    Insert into SysData_装修 with(rowlock)(装修) 
    values (@zxName)
    set @NowID=scope_identity()
end
go

create PROC Dat_SpiderRepetitionLog_Insert--
(
    @WebId int,
    @CityId int,
    @RepetitionCount bigint,
    @Date varchar(50),
    @UpdateTime datetime,
    @NowID bigint output
)
as
begin
    Insert into Dat_SpiderRepetitionLog with(rowlock)(WebId,CityId,RepetitionCount,[Date],UpdateTime) 
    values (@WebId,@CityId,@RepetitionCount,@Date,@UpdateTime)
    set @NowID=scope_identity()
end
go

create PROC Dat_SpiderRepetitionLog_Update--
(
    @WebId int,
    @CityId int,
    @RepetitionCount bigint,
    @Date varchar(50),
    @UpdateTime datetime,
    @NowID bigint
)
as
begin
    Update  Dat_SpiderRepetitionLog with(rowlock) 
    set WebId=@WebId,CityId=@CityId,RepetitionCount=@RepetitionCount,[Date]=@Date,UpdateTime=@UpdateTime
    where ID=@NowID
end
go
create PROC Dat_KeyValueConfig_Insert--
(
    @KeyName nvarchar(400),
    @KeyValue nvarchar(50),
    @CityId int,
    @WebId int,
    @NowID bigint output
)
as
begin
    Insert into Dat_KeyValueConfig with(rowlock)(KeyName,KeyValue,CityId,WebId) 
    values (@KeyName,@KeyValue,@CityId,@WebId)
    set @NowID=scope_identity()
end
go
create PROC Dat_KeyValueConfig_Update--
(
    @KeyName nvarchar(400),
    @KeyValue nvarchar(50),
    @CityId int,
    @WebId int,
    @NowID bigint
)
as
begin
    update Dat_KeyValueConfig with(rowlock) 
    set KeyName=@KeyName,KeyValue=@KeyValue,CityId=@CityId,WebId=@WebId
    where  ID=@NowID
end
go

create PROC Dat_SpiderErrorLog_Insert--
(
    @CityId int,
    @WebId int,
    @Url varchar(900),
    @ErrorTypeCode int,
    @CreateTime datetime,
    @Remark varchar(900),
    @NowID bigint output
)
as
begin
    Insert into Dat_SpiderErrorLog with(rowlock)(CityId,WebId,Url,ErrorTypeCode,CreateTime,Remark) 
    values (@CityId,@WebId,@Url,@ErrorTypeCode,@CreateTime,@Remark)
    set @NowID=scope_identity()
end
go

create PROC Dat_SpiderErrorLog_Update--
(
    @CityId int,
    @WebId int,
    @Url varchar(900),
    @ErrorTypeCode int,
    @CreateTime datetime,
    @Remark varchar(900),
    @NowID bigint 
)
as
begin
    update Dat_SpiderErrorLog with(rowlock) 
    set CityId=@CityId,WebId=@WebId,Url=@Url,ErrorTypeCode=@ErrorTypeCode,CreateTime=@CreateTime,Remark=@Remark 
    where  ID=@NowID
end
go
Create proc 案例信息_Insert
(
   @楼盘名 varchar(900),@案例时间 datetime,@行政区 varchar(900),@片区 varchar(900),@楼栋 nvarchar(500),
   @房号 nvarchar(500),@用途 nvarchar(500),@面积 nvarchar(500),@单价 nvarchar(500),@案例类型 nvarchar(500),
   @结构 nvarchar(500),@建筑类型 nvarchar(500),@总价 nvarchar(500),@所在楼层 nvarchar(500),@总楼层 nvarchar(500),
   @户型 nvarchar(500),@朝向 nvarchar(500),@装修 nvarchar(500),@建筑年代 nvarchar(500),@信息 nvarchar(500),
   @电话 nvarchar(500),@url varchar(900),@币种 nvarchar(500),@地址 nvarchar(500),@创建时间 datetime,
   @来源 nvarchar(500),@建筑形式 nvarchar(500),@花园面积 nvarchar(500),@厅结构 nvarchar(500),@车位数量 nvarchar(500),
   @配套设施 nvarchar(500),@地下室面积 nvarchar(500),@城市id int,@网站id int,@案例类型id int,@币种id int,
   @朝向id int,@户型id int,@建筑类型id int,@结构id int,@用途id int,@装修id int,@是否已进行入库整理 int,
   @进行入库整理时间 datetime,@fxtid int,@companyid bigint,@companyareaid bigint,@projectid bigint,
   @areaid bigint,@subareaid bigint,
   @nowID bigint output
)
as
begin
  insert into 案例信息 with(rowlock) (楼盘名,案例时间,行政区,片区,楼栋,房号,用途,面积,单价,案例类型,结构,建筑类型,总价,所在楼层,总楼层,户型,朝向,装修,建筑年代,信息,电话,url,币种,地址,创建时间,来源,建筑形式,花园面积,厅结构,车位数量,配套设施,地下室面积,城市id,网站id,案例类型id,币种id,朝向id,户型id,建筑类型id,结构id,用途id,装修id,是否已进行入库整理,进行入库整理时间,fxtid,companyid,companyareaid,projectid,areaid,subareaid) 
  values(@楼盘名,@案例时间,@行政区,@片区,@楼栋,@房号,@用途,@面积,@单价,@案例类型,@结构,@建筑类型,@总价,@所在楼层,@总楼层,@户型,@朝向,@装修,@建筑年代,@信息,@电话,@url,@币种,@地址,@创建时间,@来源,@建筑形式,@花园面积,@厅结构,@车位数量,@配套设施,@地下室面积,@城市id,@网站id,@案例类型id,@币种id,@朝向id,@户型id,@建筑类型id,@结构id,@用途id,@装修id,@是否已进行入库整理,@进行入库整理时间,@fxtid,@companyid,@companyareaid,@projectid,@areaid,@subareaid)
  set @NowID=scope_identity()
end
go

Create proc 案例信息_Update
(
   @楼盘名 varchar(900),@案例时间 datetime,@行政区 varchar(900),@片区 varchar(900),@楼栋 nvarchar(500),
   @房号 nvarchar(500),@用途 nvarchar(500),@面积 nvarchar(500),@单价 nvarchar(500),@案例类型 nvarchar(500),
   @结构 nvarchar(500),@建筑类型 nvarchar(500),@总价 nvarchar(500),@所在楼层 nvarchar(500),@总楼层 nvarchar(500),
   @户型 nvarchar(500),@朝向 nvarchar(500),@装修 nvarchar(500),@建筑年代 nvarchar(500),@信息 nvarchar(500),
   @电话 nvarchar(500),@url varchar(900),@币种 nvarchar(500),@地址 nvarchar(500),@创建时间 datetime,
   @来源 nvarchar(500),@建筑形式 nvarchar(500),@花园面积 nvarchar(500),@厅结构 nvarchar(500),@车位数量 nvarchar(500),
   @配套设施 nvarchar(500),@地下室面积 nvarchar(500),@城市id int,@网站id int,@案例类型id int,@币种id int,
   @朝向id int,@户型id int,@建筑类型id int,@结构id int,@用途id int,@装修id int,@是否已进行入库整理 int,
   @进行入库整理时间 datetime,@fxtid int,@companyid bigint,@companyareaid bigint,@projectid bigint,
   @areaid bigint,@subareaid bigint,
   @nowID bigint
)
as
begin
   update 案例信息 with(rowlock) set 楼盘名 = @楼盘名,案例时间 = @案例时间,行政区 = @行政区,片区 = @片区,楼栋 = @楼栋,房号 = @房号,用途 = @用途,面积 = @面积,单价 = @单价,案例类型 = @案例类型,结构 = @结构,建筑类型 = @建筑类型,总价 = @总价,所在楼层 = @所在楼层,总楼层 = @总楼层,户型 = @户型,朝向 = @朝向,装修 = @装修,建筑年代 = @建筑年代,信息 = @信息,电话 = @电话,url = @url,币种 = @币种,地址 = @地址,创建时间 = @创建时间,来源 = @来源,建筑形式 = @建筑形式,花园面积 = @花园面积,厅结构 = @厅结构,车位数量 = @车位数量,配套设施 = @配套设施,地下室面积 = @地下室面积,城市id = @城市id,网站id = @网站id,案例类型id = @案例类型id,币种id = @币种id,朝向id = @朝向id,户型id = @户型id,建筑类型id = @建筑类型id,结构id = @结构id,用途id = @用途id,装修id = @装修id,是否已进行入库整理 = @是否已进行入库整理,进行入库整理时间 = @进行入库整理时间,fxtid = @fxtid,companyid = @companyid,companyareaid = @companyareaid,projectid = @projectid,areaid = @areaid,subareaid = @subareaid
   where id = @nowID
end
