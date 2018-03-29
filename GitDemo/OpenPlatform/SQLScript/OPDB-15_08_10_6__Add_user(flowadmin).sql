/*
Date: 20150810
Description:增加流量控制连接用户
*/
GRANT ALL PRIVILEGES ON openplatform.apiinvokelog TO 'flowadmin'@'%' IDENTIFIED BY 'fxtcn.FlowControl@2015';
GRANT ALL PRIVILEGES ON openplatform.flowcontrolconfig TO 'flowadmin'@'%' IDENTIFIED BY 'fxtcn.FlowControl@2015';