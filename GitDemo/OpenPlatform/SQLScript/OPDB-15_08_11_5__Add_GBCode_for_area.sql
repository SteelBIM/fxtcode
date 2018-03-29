/*
Date: 20150811
Description: 调整sysarea表行政区名字重复的数据
*/

USE openplatform;

START TRANSACTION;

UPDATE sysarea SET gbcode = '440305' WHERE cityid =6 AND areaname = '南山区';
UPDATE sysarea SET gbcode = '230404' WHERE cityid =221 AND areaname = '南山区';
UPDATE sysarea SET gbcode = '230803' WHERE cityid =24 AND areaname = '向阳区';
UPDATE sysarea SET gbcode = '230402' WHERE cityid =221 AND areaname = '向阳区';
UPDATE sysarea SET gbcode = '120101' WHERE cityid =3 AND areaname = '和平区';
UPDATE sysarea SET gbcode = '210102' WHERE cityid =28 AND areaname = '和平区';
UPDATE sysarea SET gbcode = '450202' WHERE cityid =197 AND areaname = '城中区';
UPDATE sysarea SET gbcode = '630103' WHERE cityid =145 AND areaname = '城中区';
UPDATE sysarea SET gbcode = '620102' WHERE cityid =150 AND areaname = '城关区';
UPDATE sysarea SET gbcode = '540102' WHERE cityid =338 AND areaname = '城关区';
UPDATE sysarea SET gbcode = '140202' WHERE cityid =135 AND areaname = '城区';
UPDATE sysarea SET gbcode = '140502' WHERE cityid =305 AND areaname = '城区';
UPDATE sysarea SET gbcode = '441502' WHERE cityid =218 AND areaname = '城区';
UPDATE sysarea SET gbcode = '140402' WHERE cityid =304 AND areaname = '城区';
UPDATE sysarea SET gbcode = '140302' WHERE cityid =311 AND areaname = '城区';
UPDATE sysarea SET gbcode = '310113' WHERE cityid =2 AND areaname = '宝山区';
UPDATE sysarea SET gbcode = '230506' WHERE cityid =227 AND areaname = '宝山区';
UPDATE sysarea SET gbcode = '511102' WHERE cityid =289 AND areaname = '市中区';
UPDATE sysarea SET gbcode = '511102' WHERE cityid =293 AND areaname = '市中区';
UPDATE sysarea SET gbcode = '370402' WHERE cityid =303 AND areaname = '市中区';
UPDATE sysarea SET gbcode = '370103' WHERE cityid =120 AND areaname = '市中区';
UPDATE sysarea SET gbcode = '410402' WHERE cityid =54 AND areaname = '新华区';
UPDATE sysarea SET gbcode = '130902' WHERE cityid =44 AND areaname = '新华区';
UPDATE sysarea SET gbcode = '130105' WHERE cityid =37 AND areaname = '新华区';
UPDATE sysarea SET gbcode = '150102' WHERE cityid =143 AND areaname = '新城区';
UPDATE sysarea SET gbcode = '610102' WHERE cityid =147 AND areaname = '新城区';
UPDATE sysarea SET gbcode = '650104' WHERE cityid =136 AND areaname = '新市区';
UPDATE sysarea SET gbcode = '130602' WHERE cityid =39 AND areaname = '新市区';
UPDATE sysarea SET gbcode = '310107' WHERE cityid =2 AND areaname = '普陀区';
UPDATE sysarea SET gbcode = '330903' WHERE cityid =267 AND areaname = '普陀区';
UPDATE sysarea SET gbcode = '110105' WHERE cityid =1 AND areaname = '朝阳区';
UPDATE sysarea SET gbcode = '220104' WHERE cityid =26 AND areaname = '朝阳区';
UPDATE sysarea SET gbcode = '130702' WHERE cityid =40 AND areaname = '桥东区';
UPDATE sysarea SET gbcode = '130103' WHERE cityid =37 AND areaname = '桥东区';
UPDATE sysarea SET gbcode = '130502' WHERE cityid =245 AND areaname = '桥东区';
UPDATE sysarea SET gbcode = '130703' WHERE cityid =40 AND areaname = '桥西区';
UPDATE sysarea SET gbcode = '130104' WHERE cityid =37 AND areaname = '桥西区';
UPDATE sysarea SET gbcode = '130503' WHERE cityid =245 AND areaname = '桥西区';
UPDATE sysarea SET gbcode = '430802' WHERE cityid =266 AND areaname = '永定区';
UPDATE sysarea SET gbcode = '330205' WHERE cityid =75 AND areaname = '江北区';
UPDATE sysarea SET gbcode = '500105' WHERE cityid =4 AND areaname = '江北区';
UPDATE sysarea SET gbcode = '371312' WHERE cityid =131 AND areaname = '河东区';
UPDATE sysarea SET gbcode = '120102' WHERE cityid =3 AND areaname = '河东区';
UPDATE sysarea SET gbcode = '320706' WHERE cityid =94 AND areaname = '海州区';
UPDATE sysarea SET gbcode = '210902' WHERE cityid =240 AND areaname = '海州区';
UPDATE sysarea SET gbcode = '320802' WHERE cityid =92 AND areaname = '清河区';
UPDATE sysarea SET gbcode = '211204' WHERE cityid =35 AND areaname = '清河区';
UPDATE sysarea SET gbcode = '440111' WHERE cityid =7 AND areaname = '白云区';
UPDATE sysarea SET gbcode = '520113' WHERE cityid =106 AND areaname = '白云区';
UPDATE sysarea SET gbcode = '140203' WHERE cityid =135 AND areaname = '矿区';
UPDATE sysarea SET gbcode = '140303' WHERE cityid =311 AND areaname = '矿区';
UPDATE sysarea SET gbcode = '231005' WHERE cityid =224 AND areaname = '西安区';
UPDATE sysarea SET gbcode = '220403' WHERE cityid =232 AND areaname = '西安区';
UPDATE sysarea SET gbcode = '360103' WHERE cityid =151 AND areaname = '西湖区';
UPDATE sysarea SET gbcode = '330106' WHERE cityid =74 AND areaname = '西湖区';
UPDATE sysarea SET gbcode = '110112' WHERE cityid =1 AND areaname = '通州区';
UPDATE sysarea SET gbcode = '320612' WHERE cityid =91 AND areaname = '通州区';
UPDATE sysarea SET gbcode = '230811' WHERE cityid =24 AND areaname = '郊区';
UPDATE sysarea SET gbcode = '340711' WHERE cityid =276 AND areaname = '郊区';
UPDATE sysarea SET gbcode = '140411' WHERE cityid =304 AND areaname = '郊区';
UPDATE sysarea SET gbcode = '140311' WHERE cityid =311 AND areaname = '郊区';
UPDATE sysarea SET gbcode = '220303' WHERE cityid =233 AND areaname = '铁东区';
UPDATE sysarea SET gbcode = '210302' WHERE cityid =29 AND areaname = '铁东区';
UPDATE sysarea SET gbcode = '220302' WHERE cityid =233 AND areaname = '铁西区';
UPDATE sysarea SET gbcode = '210106' WHERE cityid =28 AND areaname = '铁西区';
UPDATE sysarea SET gbcode = '210303' WHERE cityid =29 AND areaname = '铁西区';
UPDATE sysarea SET gbcode = '130102' WHERE cityid =37 AND areaname = '长安区';
UPDATE sysarea SET gbcode = '610116' WHERE cityid =147 AND areaname = '长安区';
UPDATE sysarea SET gbcode = '150204' WHERE cityid =144 AND areaname = '青山区';
UPDATE sysarea SET gbcode = '420107' WHERE cityid =60 AND areaname = '青山区';
UPDATE sysarea SET gbcode = '320106' WHERE cityid =84 AND areaname = '鼓楼区';
UPDATE sysarea SET gbcode = '410204' WHERE cityid =53 AND areaname = '鼓楼区';
UPDATE sysarea SET gbcode = '320302' WHERE cityid =87 AND areaname = '鼓楼区';
UPDATE sysarea SET gbcode = '350102' WHERE cityid =109 AND areaname = '鼓楼区';

COMMIT;