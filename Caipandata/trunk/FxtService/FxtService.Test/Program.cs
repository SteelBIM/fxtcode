using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtService.Proxy.FxtSpiderProxy;
using System.ServiceModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using FxtService.Proxy.FxtCollateral;
using FxtService.Proxy.FxtUser;
using System.Data.OleDb;
using System.Data;
using FxtService.Proxy;

namespace FxtService.Test
{

    class Program
    {
        public static List<string> GetStrByRegexByIndex(string str, string regex, string index)
        {
            List<string> resultList = new List<string>();
            if (string.IsNullOrEmpty(regex))
            {
                return resultList;
            }
            Regex r = new Regex(regex, RegexOptions.IgnoreCase); //定义一个Regex对象实例            
            MatchCollection mc = r.Matches(str);
            if (mc.Count > 0)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    if (string.IsNullOrEmpty(index))
                    {
                        resultList.Add(mc[i].Value);
                        continue;
                    }
                    string result = "";
                    if (Regex.IsMatch(str, regex, RegexOptions.IgnoreCase))
                    {
                        result = Regex.Replace(mc[i].Value, regex, index, RegexOptions.IgnoreCase);
                    }
                    resultList.Add(result);


                }
            }
            return resultList;
        }
        static dynamic aa()
        {
            return new
            {
                data = "",
                message = ""
            };
        }
        static void Main(string[] args)
        {
            //Regex regxs = new Regex("[\u4e00-\u9fa5]");
            //string[,] array = { {"危险", "1.2+∞", "1.0-1.2" }, 
            //                  { "风险","0.9-10", "0.8-0.9" },
            //                  { "正常","0.7-0.8", "0.6-0.7" },
            //                  { "安全","0.5-0.6", "0-0.5" } };
            //foreach (var item in array)
            //{
            //    if (!regxs.IsMatch(item))
            //    {
            //        string[] aa = item.Split(new char[] { '-', '+' });
            //        Console.WriteLine(aa[0] + ',' + aa[1]);
            //    }
            //}
            //Console.WriteLine(Regex.Replace("208号", "[\u4e00-\u9fa5]", ""));
            //Console.WriteLine(11206.89655172414+1*0.5);
            //Console.WriteLine(System.Text.RegularExpressions.Regex.IsMatch("1s", @"^[0-9]+$"));
            //using (FxtUsersClient client = new FxtUsersClient())
            //{
            //    Console.WriteLine(client.GetMenuListAll());
            //}
            //Console.WriteLine("正在执行...");
            //Console.WriteLine(new Regex("^[0-9]+$").Match("18").Success);

            //string str = string.Format("Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=No;IMEX=1;'",
            //    @"\\192.168.0.10\Ftp\Collateral_Upload\2014012409395198.xlsx");
            //OleDbConnection con = new OleDbConnection(str);
            //con.Open();
            //DataTable schemaTable = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //string tableName = schemaTable.Rows[0][2].ToString();
            //int pageSize = 10, pageIndex = 3,
            //        top1 = pageSize * pageIndex;
            //    top1 = top1.Equals(pageSize) ? 1 : top1;
            //    string strSql =
            //        string.Format("select top {1} * from [{0}] where F1 not in (select top {2} F1 from [{0}])",
            //        tableName, pageSize, top1 > 10 ? top1 - 9 : top1);
            //OleDbCommand objCmd = new OleDbCommand(strSql, con);
            //OleDbDataReader reader = objCmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    Console.WriteLine(reader.GetString(0));
            //}
            //con.Close();
            //using (FxtspiderClient fxt = new FxtspiderClient())
            //{
            //Console.WriteLine(fxt.SpiderExport(GetDate(), "", ""));

            //string result = fxt.GetDatProject(6, "", "", "", 10, 1);
            //var aa = JArray.Parse(result);
            //Console.WriteLine(aa[0]["List"]);
            //}
            /* string strTest = "广东省深圳市南山区科苑北路799号科兴科学小区二期B3栋502号";
             //strTest = "1号楼10-11层";
             //strTest = "1001及跃层";
             //strTest = "南海区sss";
             strTest = "峨眉山市胜利镇名山路东段2号";
             //([0-9]{1,2}层)|([0-9]{1,2}.[0-9]{1,2}层)
             //
             Regex regex = new Regex("([0-9]{1,}跃[0-9]{1,}层)|([0-9]{1,}跃[0-9]{1,})|([0-9]{1,}跃)|([0-9]{1,})");
             regex = new Regex("(.){1,}省");
             string val = regex.Match(strTest).Value;
             Console.WriteLine(val);
             if (!string.IsNullOrEmpty(val))
                 strTest = strTest.Replace(val, "");
             Console.WriteLine(strTest);
             regex = new Regex("(?:(?!市).)*市");
             val = regex.Match(strTest).Value;
             Console.WriteLine(val);

             strTest = strTest.Replace(val, "");
             Console.WriteLine(strTest);
             //(?:(?!((区)|(县)|(市))).)*((区)|(县)|(市))
             regex = new Regex("(?:(?!((区)|(县))).)*((区)|(县))");
             val = regex.Match(strTest).Value;
             Console.WriteLine(val);

             strTest = strTest.Replace(val, "");
             Console.WriteLine(strTest);
             regex = new Regex("^[^号]*号");
            
             val = regex.Match("621号华天滨中嘉园").Value;
             List<string> list = GetStrByRegexByIndex(val, "号.*", "$1");
             Console.WriteLine(val);*/
            //Assembly assembly = typeof(Program).Assembly;

            //long begin = System.DateTime.Now.Ticks;
            //try
            //{
            //    using (Stream is_Renamed = File.OpenRead("dict\\wordbase.dic"))
            //    {
            //        using (StreamReader br = new StreamReader(is_Renamed, Encoding.UTF8, false, 512))
            //        {
            //            string theWord;
            //            while ((theWord = br.ReadLine()) != null)
            //            {
            //                if (strTest.Contains(theWord))
            //                    Console.WriteLine(theWord);
            //            }
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    //Console.WriteLine("字典文件读取错误！" + file_Connector);
            //    //throw e;
            //}
            //int end = (int)((System.DateTime.Now.Ticks - begin) * 1.0 / 100000);

            //System.Console.Out.WriteLine("\u8017\u65F6 : " + (end) + "ms");
            //Console.WriteLine(strTest);
            //string strRegexProvince = "(?:(?!省).)*";//匹配省之前的所有信息
            //string strRegexExistsProvince = "(?:(?!省).)*省";//匹配省之前的所有信息且包含省
            //string strRegexNotProvince = "(?:(?!省).)*$";//匹配省后前的所有信息

            //string strRegexCity = "(?:(?!市).)*市";//匹配市之前的所有信息
            //string strRegexExistsCity = "(?:(?!市).)+市";//匹配市之前的所有信息且包含市
            //string strRegexNotCity = "(?:(?!市).)*$";//匹配市后前的所有信息



            //string strRegexArea = "(?:(?!((区)|(县)|(市))).)*((区)|(县)|(市))";//匹配市之前的所有信息
            //string strRegexExistsArea = "(?:(?!市).)*市";//匹配市之前的所有信息且包含市
            //string strRegexNotArea = "(?:(?!((区)|(县)|(市))).)*((区)|(县)|(市))$";//匹配市后前的所有信息

            //string strRegexRoad = "";

            //Regex regexNProvince = new Regex(strRegexNotProvince);//非省
            //Regex regexNCity = new Regex(strRegexNotCity);//非市
            //Regex regexNArea = new Regex(strRegexArea);//包含区县市
            //string strNotProvince = regexNProvince.Match(strTest).Value;
            //Console.WriteLine("省:" + new Regex(strRegexExistsProvince).Match(strTest).Value);//省
            //Console.WriteLine("省后:" + strNotProvince);//省以后的信息

            //Console.WriteLine("市:" + new Regex(strRegexCity).Match(strNotProvince).Value);//市

            //List<string> list = GetStrByRegexByIndex(strNotProvince, "市((?:(?!市).)+市(?:(?!市).)*)", "$1");
            //string strNotCity = list.Count > 0 ? list[0] : regexNCity.Match(strNotProvince).Value;
            //Console.WriteLine("市后:" + strNotCity);//市以后的信息


            //Console.WriteLine("区县市:" + regexNArea.Match(strNotCity).Value);//包含区县市
            ////
            //List<string> list2 = GetStrByRegexByIndex(strNotCity, "(?:(?!区).)+区(.*)", "$1");
            //string strNotArea = list2.Count > 0 ? list2[0] : "";
            //Console.WriteLine("区县市后:" + strNotArea);//区县市后

            //string strRoad = new Regex("((?:(?!路).)+路(?:(?!号).)+号)|((?:(?!街).)+街(?:(?!号).)+号)|((?:(?!以北).)+以北)").Match(strNotArea).Value;
            //Console.WriteLine("路号:" + strRoad);//包含路号等
            //List<string> list3 = GetStrByRegexByIndex(strNotArea, "(?:(?!号).)+号(.*)", "$1");
            //string strNotRoad = list3.Count > 0 ? list3[0] : "";
            //Console.WriteLine("路号后:" + strNotRoad);//路号等后

            //string strProject = new Regex("((?:(?!小区).)+小区)|((?:(?!苑).)+苑)|((?:(?!院).)+院)").Match(strNotRoad).Value;
            //Console.WriteLine("楼盘:" + strProject);//包楼盘等
            //List<string> list4 = GetStrByRegexByIndex(strNotRoad, "(?:(?!苑).)+苑(.*)", "$1");
            //string strNotProject = list4.Count > 0 ? list4[0] : "";
            //Console.WriteLine("楼盘后:" + strNotProject);//楼盘等后

            //string strPI = new Regex("((?:(?!期).)+期)").Match(strNotProject).Value;
            //Console.WriteLine("期:" + strPI);//包含期等
            //List<string> list5 = GetStrByRegexByIndex(strNotProject, "(?:(?!期).)+期(.*)", "$1");
            //string strNotPI = list5.Count > 0 ? list5[0] : "";
            //Console.WriteLine("期后:" + strNotPI);//期等后

            //string strBuilding = new Regex("((?:(?!栋).)+栋)").Match(strNotPI).Value;
            //Console.WriteLine("楼栋:" + strBuilding);//包含楼栋等
            //List<string> list6 = GetStrByRegexByIndex(strNotPI, "(?:(?!栋).)+栋(.*)", "$1");
            //string strNotBuilding = list6.Count > 0 ? list6[0] : "";
            //Console.WriteLine("楼栋后:" + strNotBuilding);//楼栋等后

            //string strHouse = new Regex("((?:(?!号).)+号)").Match(strNotBuilding).Value;
            //Console.WriteLine("房号:" + strHouse);//包含房号等
            //List<string> list7 = GetStrByRegexByIndex(strNotBuilding, "(?:(?!号).)+号(.*)", "$1");
            //string strNotHouse = list7.Count > 0 ? list7[0] : "";
            //Console.WriteLine("房号后:" + strNotHouse);//房号等后

            //DateTime lastMonth = DateTime.Now.AddMonths(-1);
            //string LastMonth_firstDay = lastMonth.AddDays(1 - lastMonth.Day).ToString("yyyy-MM-dd");
            //string LastMonth_lastDay = lastMonth.AddDays(1 - lastMonth.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            //lastMonth.AddDays(1 - lastMonth.Day).AddMonths(1).AddDays(-1).AddMonths(-3).ToString("yyyy-MM-dd");
            //using (FxtService.Proxy.FxtAPI.FxtAPIClient fxtapi = new Proxy.FxtAPI.FxtAPIClient())
            //{
            //    DateTime dtStart = DateTime.Now;
            //    Console.WriteLine(dtStart.ToString("HH:mm:ss"));
            //    string strJson = fxtapi.Cross(20688, 4, 1002027, "2011-06-01");
            //    Console.WriteLine(strJson);
            //    DateTime dtEnd = DateTime.Now;
            //    Console.WriteLine(dtEnd.ToString("HH:mm:ss"));
            //    Console.WriteLine((dtEnd - dtStart).TotalSeconds);
            //JArray province = JArray.Parse(fxtapi.GetProvince());
            //int pi = 0, count = 0;
            //string path = string.Format("{0}\\test.dic", AppDomain.CurrentDomain.BaseDirectory);
            //if (File.Exists(path))
            //    File.Delete(path);
            //FileStream file = File.Create(path);
            //file.Flush();
            //file.Close();
            //StreamWriter sw = new StreamWriter(path);
            //while (province.Count > pi)
            //{
            //    Console.WriteLine(string.Format("省份{0}{1}",
            //        province[pi]["ProvinceName"].ToString().Replace("直辖", ""),
            //        province[pi]["Alias"]));
            //    sw.WriteLine(province[pi]["ProvinceName"].ToString().Replace("直辖", ""));
            //    sw.WriteLine(province[pi]["Alias"]);
            //    JArray city = JArray.Parse(fxtapi.GetCity(int.Parse(province[pi]["ProvinceId"].ToString())));
            //    foreach (var citem in city)
            //    {
            //        //Console.WriteLine(string.Format("城市{0}{1}", citem["CityName"], citem["CityId"]));
            //        if (province[pi]["ProvinceName"].ToString().IndexOf("直辖") < 0 )
            //        {
            //            sw.WriteLine(citem["CityName"]);
            //            sw.WriteLine(citem["Alias"]);
            //        }
            //        JArray area = JArray.Parse(fxtapi.GetArea(int.Parse(citem["CityId"].ToString())));
            //        //Console.WriteLine(string.Format("行政区{0}{1}", area[0]["AreaName"], area[0]["AreaId"]));
            //        foreach (var aitem in area)
            //        {
            //            sw.WriteLine(aitem["AreaName"]);
            //            sw.WriteLine(aitem["AreaName"].ToString()
            //                .Replace("县", "")
            //                .Replace("市", "")
            //                .Replace("区", ""));
            //            count++;
            //        }
            //        count++;
            //    }
            //    count++;
            //    pi++;
            //}
            //Console.WriteLine("共:" + count);
            //sw.Flush();
            //sw.Close();
            //}
            //    var vJobject = JArray.Parse(strJson);
            //    int i = 0;
            //    List<Temps> listTemp = new List<Temps>();
            //    while (vJobject.Count > i)
            //    {
            //        JObject jobj = (JObject)vJobject[i];
            //        Temps _temps = new Temps();
            //        _temps.jg = jobj["JieGou"].Value<string>();
            //        foreach (var item in jobj)
            //        {
            //            if (item.Key.Equals("MianJi"))
            //            {
            //                switch (int.Parse(item.Value.Value<string>()))
            //                {
            //                    case 30:
            //                        _temps.th = !string.IsNullOrEmpty(jobj["Zhi"].Value<string>()) ?
            //                            jobj["Zhi"].Value<string>() : "0";
            //                        break;
            //                    case 3060:
            //                        _temps.thsix = !string.IsNullOrEmpty(jobj["Zhi"].Value<string>()) ?
            //                            jobj["Zhi"].Value<string>() : "0";
            //                        break;
            //                    case 6090:
            //                        _temps.sixnine = !string.IsNullOrEmpty(jobj["Zhi"].Value<string>()) ?
            //                            jobj["Zhi"].Value<string>() : "0";
            //                        break;
            //                    case 90120:
            //                        _temps.ninehtwo = !string.IsNullOrEmpty(jobj["Zhi"].Value<string>()) ?
            //                            jobj["Zhi"].Value<string>() : "0";
            //                        break;
            //                    case 120:
            //                        _temps.htwo = !string.IsNullOrEmpty(jobj["Zhi"].Value<string>()) ?
            //                            jobj["Zhi"].Value<string>() : "0";
            //                        break;

            //                }
            //            }
            //        }
            //        if (listTemp.Where(tem => tem.jg.Equals(_temps.jg)).Any())
            //        {
            //            listTemp.ForEach((ltemp) =>
            //            {
            //                if (ltemp.jg.Equals(_temps.jg))
            //                {
            //                    if (decimal.Parse(!string.IsNullOrEmpty(_temps.th) ? _temps.th : "0") > 0)
            //                        ltemp.th = _temps.th;
            //                    if (decimal.Parse(!string.IsNullOrEmpty(_temps.thsix) ? _temps.thsix : "0") > 0)
            //                        ltemp.thsix = _temps.thsix;
            //                    if (decimal.Parse(!string.IsNullOrEmpty(_temps.sixnine) ? _temps.sixnine : "0") > 0)
            //                        ltemp.sixnine = _temps.sixnine;
            //                    if (decimal.Parse(!string.IsNullOrEmpty(_temps.ninehtwo) ? _temps.ninehtwo : "0") > 0)
            //                        ltemp.ninehtwo = _temps.ninehtwo;
            //                    if (decimal.Parse(!string.IsNullOrEmpty(_temps.htwo) ? _temps.htwo : "0") > 0)
            //                        ltemp.htwo = _temps.htwo;
            //                }
            //            });
            //        }
            //        else
            //        {
            //            listTemp.Add(_temps);
            //        }
            //        i++;
            //    }
            //    Console.WriteLine(JsonConvert.SerializeObject(listTemp));

            //}

            Console.ReadLine();
        }

        static string GetDate()
        {
            return "[{\"ID\":\"1722019\",\"楼盘名\":\"万科四季花城\",\"案例时间\":\"2013-11-29 10:26:58\",\"行政区\":\"于洪\",\"片区\":\"怒江北\",\"楼栋\":\"\",\"房号\":\"\",\"用途\":\"普通住宅\",\"面积\":\"126\",\"单价\":\"7302\",\"案例类型\":\"买卖报盘\",\"结构\":\"平面\",\"建筑类型\":\"多层\",\"总价\":\"92\",\"所在楼层\":\"5\",\"总楼层\":\"6\",\"户型\":\"三室两厅\",\"朝向\":\"南北\",\"装修\":\"普通\",\"建筑年代\":\"2001\",\"信息\":\"万科四季花城 南北三室 精装好房 急售\",\"电话\":\"13804996293\",\"URL\":\"http://esf.sy.soufun.com/chushou/3_19905786.htm\",\"币种\":\"人民币\",\"地址\":\"于洪区西江街99号\",\"创建时间\":\"11 29 2013 10:26AM\",\"来源\":\"搜房网\",\"建筑形式\":\"\",\"花园面积\":\"\",\"厅结构\":\"\",\"车位数量\":\"\",\"配套设施\":\"煤气/天然气,暖气,车位/车库,储藏室/地下室,花园/小院\",\"地下室面积\":\"\",\"城市\":\"沈阳\",\"网站\":\"搜房网\"}," +
"{\"ID\":\"1727627\",\"楼盘名\":\"万科四季花城\",\"案例时间\":\"2013-11-29 10:59:52\",\"行政区\":\"于洪\",\"片区\":\"怒江北\",\"楼栋\":\"\",\"房号\":\"\",\"用途\":\"普通住宅\",\"面积\":\"80\",\"单价\":\"6875\",\"案例类型\":\"买卖楼盘\",\"结构\":\"平面\",\"建筑类型\":\"多层\",\"总价\":\"55\",\"所在楼层\":\"1\",\"总楼层\":\"6\",\"户型\":\"两房一厅\",\"朝向\":\"西北\",\"装修\":\"精装修\",\"建筑年代\":\"2001\",\"信息\":\"万科四季花城 精装修 好房急卖\",\"电话\":\"18698883176\",\"URL\":\"http://esf.sy.soufun.com/chushou/3_19892395.htm\",\"币种\":\"人民币\",\"地址\":\"于洪区西江街99号\",\"创建时间\":\"11 29 2013 10:59AM\",\"来源\":\"搜房网\",\"建筑形式\":\"\",\"花园面积\":\"\",\"厅结构\":\"\",\"车位数量\":\"\",\"配套设施\":\"\",\"地下室面积\":\"\",\"城市\":\"沈阳\",\"网站\":\"搜房网\"}," +
"{\"ID\":\"1716578\",\"楼盘名\":\"万科四季花城\",\"案例时间\":\"2013-11-29 09:59:54\",\"行政区\":\"于洪\",\"片区\":\"怒江北\",\"楼栋\":\"\",\"房号\":\"\",\"用途\":\"普通住宅\",\"面积\":\"97\",\"单价\":\"7394\",\"案例类型\":\"买卖楼盘\",\"结构\":\"平面\",\"建筑类型\":\"多层\",\"总价\":\"71.8\",\"所在楼层\":\"2\",\"总楼层\":\"6\",\"户型\":\"两房两厅\",\"朝向\":\"南北\",\"装修\":\"精装修\",\"建筑年代\":\"2006\",\"信息\":\"万科四季花城 97.11平71.8万急售 南北通透精装房 拎包即可入住\",\"电话\":\"13709819187\",\"URL\":\"http://esf.sy.soufun.com/chushou/3_19912032.htm\",\"币种\":\"人民币\",\"地址\":\"于洪区西江街99号\",\"创建时间\":\"11 29 2013  9:59AM\",\"来源\":\"搜房网\",\"建筑形式\":\"\",\"花园面积\":\"\",\"厅结构\":\"\",\"车位数量\":\"\",\"配套设施\":\"煤气/天然气,暖气,车位/车库,储藏室/地下室,花园/小院\",\"地下室面积\":\"\",\"城市\":\"沈阳\",\"网站\":\"搜房网\"}," +
"{\"ID\":\"1716144\",\"楼盘名\":\"万科四季花城\",\"案例时间\":\"2013-11-29 09:58:13\",\"行政区\":\"于洪\",\"片区\":\"怒江北\",\"楼栋\":\"\",\"房号\":\"\",\"用途\":\"普通住宅\",\"面积\":\"141\",\"单价\":\"11126\",\"案例类型\":\"买卖楼盘\",\"结构\":\"平面\",\"建筑类型\":\"多层\",\"总价\":\"157\",\"所在楼层\":\"2\",\"总楼层\":\"4\",\"户型\":\"三房两厅\",\"朝向\":\"南北\",\"装修\":\"简装修\",\"建筑年代\":\"2001\",\"信息\":\"万科四季花城花园洋房 紧邻欢乐谷 精装修 拎包即住 无税 急售\",\"电话\":\"15640173733\",\"URL\":\"http://esf.sy.soufun.com/chushou/3_19912727.htm\",\"币种\":\"人民币\",\"地址\":\"于洪区西江街99号\",\"创建时间\":\"11 29 2013  9:58AM\",\"来源\":\"搜房网\",\"建筑形式\":\"\",\"花园面积\":\"\",\"厅结构\":\"\",\"车位数量\":\"\",\"配套设施\":\"煤气/天然气,暖气\",\"地下室面积\":\"\",\"城市\":\"沈阳\",\"网站\":\"搜房网\"}]";
        }
    }
    class Temps
    {
        public string jg { set; get; }
        public string th { set; get; }
        public string thsix { set; get; }
        public string sixnine { set; get; }
        public string ninehtwo { set; get; }
        public string htwo { set; get; }
    }
}
