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

CREATE PROC SysData_װ��_Insert--
(
    @zxName varchar(800),
    @NowID int output
)
as
begin
    Insert into SysData_װ�� with(rowlock)(װ��) 
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
Create proc ������Ϣ_Insert
(
   @¥���� varchar(900),@����ʱ�� datetime,@������ varchar(900),@Ƭ�� varchar(900),@¥�� nvarchar(500),
   @���� nvarchar(500),@��; nvarchar(500),@��� nvarchar(500),@���� nvarchar(500),@�������� nvarchar(500),
   @�ṹ nvarchar(500),@�������� nvarchar(500),@�ܼ� nvarchar(500),@����¥�� nvarchar(500),@��¥�� nvarchar(500),
   @���� nvarchar(500),@���� nvarchar(500),@װ�� nvarchar(500),@������� nvarchar(500),@��Ϣ nvarchar(500),
   @�绰 nvarchar(500),@url varchar(900),@���� nvarchar(500),@��ַ nvarchar(500),@����ʱ�� datetime,
   @��Դ nvarchar(500),@������ʽ nvarchar(500),@��԰��� nvarchar(500),@���ṹ nvarchar(500),@��λ���� nvarchar(500),
   @������ʩ nvarchar(500),@��������� nvarchar(500),@����id int,@��վid int,@��������id int,@����id int,
   @����id int,@����id int,@��������id int,@�ṹid int,@��;id int,@װ��id int,@�Ƿ��ѽ���������� int,
   @�����������ʱ�� datetime,@fxtid int,@companyid bigint,@companyareaid bigint,@projectid bigint,
   @areaid bigint,@subareaid bigint,
   @nowID bigint output
)
as
begin
  insert into ������Ϣ with(rowlock) (¥����,����ʱ��,������,Ƭ��,¥��,����,��;,���,����,��������,�ṹ,��������,�ܼ�,����¥��,��¥��,����,����,װ��,�������,��Ϣ,�绰,url,����,��ַ,����ʱ��,��Դ,������ʽ,��԰���,���ṹ,��λ����,������ʩ,���������,����id,��վid,��������id,����id,����id,����id,��������id,�ṹid,��;id,װ��id,�Ƿ��ѽ����������,�����������ʱ��,fxtid,companyid,companyareaid,projectid,areaid,subareaid) 
  values(@¥����,@����ʱ��,@������,@Ƭ��,@¥��,@����,@��;,@���,@����,@��������,@�ṹ,@��������,@�ܼ�,@����¥��,@��¥��,@����,@����,@װ��,@�������,@��Ϣ,@�绰,@url,@����,@��ַ,@����ʱ��,@��Դ,@������ʽ,@��԰���,@���ṹ,@��λ����,@������ʩ,@���������,@����id,@��վid,@��������id,@����id,@����id,@����id,@��������id,@�ṹid,@��;id,@װ��id,@�Ƿ��ѽ����������,@�����������ʱ��,@fxtid,@companyid,@companyareaid,@projectid,@areaid,@subareaid)
  set @NowID=scope_identity()
end
go

Create proc ������Ϣ_Update
(
   @¥���� varchar(900),@����ʱ�� datetime,@������ varchar(900),@Ƭ�� varchar(900),@¥�� nvarchar(500),
   @���� nvarchar(500),@��; nvarchar(500),@��� nvarchar(500),@���� nvarchar(500),@�������� nvarchar(500),
   @�ṹ nvarchar(500),@�������� nvarchar(500),@�ܼ� nvarchar(500),@����¥�� nvarchar(500),@��¥�� nvarchar(500),
   @���� nvarchar(500),@���� nvarchar(500),@װ�� nvarchar(500),@������� nvarchar(500),@��Ϣ nvarchar(500),
   @�绰 nvarchar(500),@url varchar(900),@���� nvarchar(500),@��ַ nvarchar(500),@����ʱ�� datetime,
   @��Դ nvarchar(500),@������ʽ nvarchar(500),@��԰��� nvarchar(500),@���ṹ nvarchar(500),@��λ���� nvarchar(500),
   @������ʩ nvarchar(500),@��������� nvarchar(500),@����id int,@��վid int,@��������id int,@����id int,
   @����id int,@����id int,@��������id int,@�ṹid int,@��;id int,@װ��id int,@�Ƿ��ѽ���������� int,
   @�����������ʱ�� datetime,@fxtid int,@companyid bigint,@companyareaid bigint,@projectid bigint,
   @areaid bigint,@subareaid bigint,
   @nowID bigint
)
as
begin
   update ������Ϣ with(rowlock) set ¥���� = @¥����,����ʱ�� = @����ʱ��,������ = @������,Ƭ�� = @Ƭ��,¥�� = @¥��,���� = @����,��; = @��;,��� = @���,���� = @����,�������� = @��������,�ṹ = @�ṹ,�������� = @��������,�ܼ� = @�ܼ�,����¥�� = @����¥��,��¥�� = @��¥��,���� = @����,���� = @����,װ�� = @װ��,������� = @�������,��Ϣ = @��Ϣ,�绰 = @�绰,url = @url,���� = @����,��ַ = @��ַ,����ʱ�� = @����ʱ��,��Դ = @��Դ,������ʽ = @������ʽ,��԰��� = @��԰���,���ṹ = @���ṹ,��λ���� = @��λ����,������ʩ = @������ʩ,��������� = @���������,����id = @����id,��վid = @��վid,��������id = @��������id,����id = @����id,����id = @����id,����id = @����id,��������id = @��������id,�ṹid = @�ṹid,��;id = @��;id,װ��id = @װ��id,�Ƿ��ѽ���������� = @�Ƿ��ѽ����������,�����������ʱ�� = @�����������ʱ��,fxtid = @fxtid,companyid = @companyid,companyareaid = @companyareaid,projectid = @projectid,areaid = @areaid,subareaid = @subareaid
   where id = @nowID
end
