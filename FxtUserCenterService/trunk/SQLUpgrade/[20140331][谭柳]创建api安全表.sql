use fxtusercenter
--��˾��ʶ��
alter table companyinfo add signname varchar(50) null default(newid())

/*
��Ʒ�Ľӿ����ñ�ͨ�ã�
��˾��sianname��ʵ����Ϊÿ����˾��ΨһKEYʹ��
*/
create table
dbo.Product_App
(
ProductTypeCode int not null,--�ƹ��۲�ƷCODE
AppId int not null,--�ƹ��۵�API�ӿ�CODE
AppPwd varchar(32) not null,--API�ӿ����룬���岻��
AppKey varchar(50) not null,--API�ӿ�key
ApiUrl varchar(50) not null --�ӿ�URL,
)

/*
��˾��Ʒ�ӿ����ñ���������
*/
create table
dbo.Product_App_Black
(
ProductTypeCode int not null,--�ƹ��۲�ƷCODE
AppId int not null,--�ƹ��۵�API�ӿ�CODE
CompanyId int not null--��������˾
)


/*
�ӿڹ������ñ�ͨ�ã�
*/
create table
dbo.App_Function
(
AppId int not null,--�ƹ��۵�API�ӿ�CODE
FunctionName varchar(50) not null,--��������
CallBackUrl varchar(200) null,--�ص���ַ
CallBackFormat varchar(500) null,--�ص��������ݸ�ʽ
Parame1 varchar(50) null,--�ص���Ҫ�Ĳ���
Parame2 varchar(50) null,--�ص���Ҫ�Ĳ���
Parame3 varchar(50) null,--�ص���Ҫ�Ĳ���
Parame4 varchar(50) null--�ص���Ҫ�Ĳ���
)

/*
�ӿڹ������ñ���������
*/
create table
dbo.App_Function_Black
(
AppId int not null,--�ƹ��۵�API�ӿ�CODE
FunctionName varchar(50) not null,--��������
CompanyId int not null,--��������˾
ProductTypeCode int not null,--�ƹ��۲�ƷCODE
SplaType varchar(30) null--ƽ̨(android��ios��pc)
)
