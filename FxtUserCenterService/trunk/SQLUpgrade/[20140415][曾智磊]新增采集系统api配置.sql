---���appCode
use fxtproject
select * from SYS_Code where id=1003 and code=1003100
if not exists(select * from sys_code where 
id=1003 and code=1003034 and codename='��ֽ��סլ��ҵ��Ϣ�ɼ�ϵͳ' and codetype='ϵͳ����')
begin
    insert sys_code(id,code,codename,codetype,remark,subcode)
    select 1003,1003034,'��ֽ��סլ��ҵ��Ϣ�ɼ�ϵͳ','ϵͳ����','DB.FxtTemp',4
end
go
if not exists(select * from sys_code where 
id=1003 and code=1003106 and codename='��ֽ��סլ��ҵ��Ϣ�ɼ�ϵͳapi' and codetype='api����')
begin
    insert sys_code(id,code,codename,codetype,remark,subcode)
    select 1003,1003106,'��ֽ��סլ��ҵ��Ϣ�ɼ�ϵͳapi','api����','DB.FxtTemp',4
end
--������ͨ��Ʒ
go
use FxtUserCenter

if not exists (select * from CompanyProduct where companyid=25 and productTypeCode=1003034)
begin
   INSERT FxtUserCenter.dbo.CompanyProduct(CompanyId,ProductTypeCode,CurrentVersion,StartDate,
            OverDate,WebUrl,APIUrl,OutAPIUrl ,MsgServer,CreateDate,Valid,AppAbbreviation)
   select 25,1003034,'1.0',getdate(),'2019-04-10','http://acquisition.fxtcn.com',
   'http://api.fxtcn.com/acquisi/mobileapi/runflats','','',getdate(),1,'wzhzzwyxxcjxt'
end
--��Ʒ��ͨAPI
go
use FxtUserCenter
--�ɼ�ϵͳ ��ͨ-�ɼ�ϵͳapi
if not exists(select * from Product_App where productTypeCode=1003034 and appid=1003106)
begin
    insert Product_App(productTypeCode,appid,apppwd,appkey,apiurl)
    select 1003034,1003106,REPLACE(cast(CHECKSUM(NEWID()) as varchar),'-',''),REPLACE(cast(CHECKSUM(NEWID()) as varchar),'-',''),'http://api.fxtcn.com/acquisi/mobileapi/runflats'
end

--�ɼ�ϵͳ ��ͨ ��������api
if not exists(select * from Product_App where productTypeCode=1003034 and appid=1003104)
begin
    insert Product_App(productTypeCode,appid,apppwd,appkey,apiurl)
    select 1003034,1003104,REPLACE(cast(CHECKSUM(NEWID()) as varchar),'-',''),REPLACE(cast(CHECKSUM(NEWID()) as varchar),'-',''),'http://api.fxtcn.com/datacenter/dc/active'
end
--�ɼ�ϵͳ ��ͨ �û�����api
if not exists(select * from Product_App where productTypeCode=1003034 and appid=1003105)
begin
    insert Product_App(productTypeCode,appid,apppwd,appkey,apiurl)
    select 1003034,1003105,REPLACE(cast(CHECKSUM(NEWID()) as varchar),'-',''),REPLACE(cast(CHECKSUM(NEWID()) as varchar),'-',''),'http://api.fxtcn.com/usercenter/uc/active'
end