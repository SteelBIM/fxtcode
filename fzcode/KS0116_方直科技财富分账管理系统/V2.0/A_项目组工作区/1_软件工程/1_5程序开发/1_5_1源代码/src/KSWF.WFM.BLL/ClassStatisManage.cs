using KSWF.Framework.BLL;
using KSWF.WFM.Constract.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KSWF.WFM.BLL
{
    public class ClassStatisManage : StatisManage
    {
        public List<Constract.Statistic.tb_class> GetAllClassList()
        {
            List<Constract.Statistic.tb_class> list = SelectAll<Constract.Statistic.tb_class>();

            return list;
        }

        public void InitTbClassData()
        {
            SNSService.FZUUMS_Relation2 rService = new SNSService.FZUUMS_Relation2();
            SNSService.ReturnInfo rinfo = rService.GetAllClassInfo();
            List<tb_class> clist = JsonConvert.DeserializeObject<List<tb_class>>(rinfo.Data.ToString());
            var ls = clist.GroupBy(i => i.SchoolID).Select(g => g.Key);
            List<int> listid = new List<int>();
            foreach (var l in ls)
            {
                if (l.HasValue)
                    listid.Add(l.Value);
            }
            KSWF.WFM.BLL.MateService.Service service = new KSWF.WFM.BLL.MateService.Service();
            string result = service.GetSchoolsInfo(listid.ToArray());
            List<tb_schoolinfo> schoollist = JsonConvert.DeserializeObject<List<tb_schoolinfo>>(result);
            string[] schooltypeno = System.Configuration.ConfigurationManager.AppSettings["SchoolTypeNo"].Split(',');
            schoollist = schoollist.Where(i => schooltypeno.Contains(i.SchoolTypeNo)).ToList();
            InitTbSchoolData(schoollist);

            var classlist = from c in clist
                            join s in schoollist on c.SchoolID equals s.ID
                            where schooltypeno.Contains(s.SchoolTypeNo)
                            select new tb_class
                            {
                                ID = c.ID,
                                ClassName = c.ClassName,
                                CreateDate = c.CreateDate,
                                ClassNum = c.ClassNum,
                                Flag = c.Flag,
                                GradeID = c.GradeID,
                                GradeName = c.GradeID.ToGradeName(),
                                SchoolID = c.SchoolID,
                                SchoolName = s.SchoolName
                            };
            string sql = "TRUNCATE TABLE tb_class";
            ExcueteSql(sql);
            InsertRange<tb_class>(classlist.ToList<tb_class>());
            InsertLog("Tb_Class", 0, "");
        }

        public void InitTbSchoolData(List<tb_schoolinfo> schoollist)
        {

            string sql = "TRUNCATE TABLE tb_schoolinfo";
            ExcueteSql(sql);
            InsertRange<tb_schoolinfo>(schoollist);
            InsertLog("Tb_SchoolInfo", schoollist.Count, "");
        }

        public void InitTbClassTeaData()
        {
            string sql = "TRUNCATE TABLE tb_classtea";
            ExcueteSql(sql);
            SNSService.FZUUMS_Relation2 rService = new SNSService.FZUUMS_Relation2();
            SNSService.ReturnInfo rinfo = rService.GetAllClassTeaInfo();
            List<tb_classtea> clist = JsonConvert.DeserializeObject<List<tb_classtea>>(rinfo.Data.ToString());
            InsertRange<tb_classtea>(clist);
            InsertLog("Tb_ClassTea", clist.Count, "");
        }

        public void InitTbCLassStuData()
        {
            string sql = "TRUNCATE TABLE tb_classstu";
            ExcueteSql(sql);
            SNSService.FZUUMS_Relation2 rService = new SNSService.FZUUMS_Relation2();
            SNSService.ReturnInfo rinfo = rService.GetAllClassStuInfo();
            List<tb_classstu> clist = JsonConvert.DeserializeObject<List<tb_classstu>>(rinfo.Data.ToString());
            InsertRange<tb_classstu>(clist);
            InsertLog("Tb_ClassStu", clist.Count, "");
           
            //班级统计数据 每个班级人数
            sql = @"TRUNCATE table tb_classstucount;insert into tb_classstucount SELECT ClassID AS classid,Count(1) AS stucount from `tb_classstu` group by ClassID; ";
            ExcueteSql(sql);
            InsertLog("tb_classstucount", 100, "");

        }

        public void InitTbTmpAreaSchool()
        {
            List<KSWF.WFM.Constract.Statistic.tb_tmpareaschool> query = new List<KSWF.WFM.Constract.Statistic.tb_tmpareaschool>();
            KSWF.Framework.BLL.Manage manager = new Manage();
            List<KSWF.WFM.Constract.VW.vw_masterarea> malist = new List<Constract.VW.vw_masterarea>();
            //查询学校不为空的用户区域对应的数据
            string sqlstr = @"select * from fz_wfs.vw_statistic_masterarea where (schoolid,masterid) in
                            (select A.schoolid,max(A.masterid) as masterid from (select * from  fz_wfs.vw_statistic_masterarea where schoolid<>0) as A  group by A.schoolid ) ;";
            malist.AddRange(manager.SqlQuery<KSWF.WFM.Constract.VW.vw_masterarea>(sqlstr));
            //查询学校为空的用户区域对应的数据
            sqlstr = @"select * from fz_wfs.vw_statistic_masterarea where (districtid,masterid) in
                            (select A.districtid,max(A.masterid) as masterid from (select * from  fz_wfs.vw_statistic_masterarea where schoolid=0) as A  group by A.districtid ) ;";
            malist.AddRange(manager.SqlQuery<KSWF.WFM.Constract.VW.vw_masterarea>(sqlstr));

            List<KSWF.WFM.Constract.VW.vw_masterarea> schoolmalist = malist.Where(i => i.schoolid != 0).ToList();
            List<int> listid = schoolmalist.Select(i => i.schoolid).ToList<int>();
            if (listid == null)
            {
                listid = new List<int>();
            }
            List<KSWF.WFM.Constract.Statistic.tb_class> list = GetAllClassList();
            var ls = list.GroupBy(i => i.SchoolID).Select(g => g.Key);
            object obj = new object();
            foreach (var l in ls)
            {
                listid.Add((int)l);
            }
            // KSWF.WFM.BLL.MateService.Service service = new KSWF.WFM.BLL.MateService.Service();
            // string result = service.GetSchoolsInfo(listid.ToArray());
            //  List<Tb_SchoolInfo> schoollist = JsonConvert.DeserializeObject<List<Tb_SchoolInfo>>(result);
            List<tb_schoolinfo> schoollist = SelectAll<tb_schoolinfo>();
            if (schoollist == null || schoollist.Count == 0)
            {
                throw new Exception("没有找到学校信息");
            }
            foreach (KSWF.WFM.Constract.VW.vw_masterarea ma in schoolmalist)
            {
                query.Add(new tb_tmpareaschool
                {
                    MasterName = ma.mastername,
                    DeptID = ma.deptid,
                    DeptName = ma.deptname,
                    SchoolID = ma.schoolid,
                    SchoolName = ma.schoolname,
                    DistrictID = ma.districtid,
                    Path = ma.path.Trim(),
                    MasterType = ma.mastertype,
                    TrueName = ma.truename,
                    AgentID = ma.agentid,
                    AgentName = ma.agentname
                });
                //var sinfo = schoollist.Select(i => i.ID == ma.schoolid);
                //if (ma.schoolid == 593682) {
                //    string str = "";
                //}
                for (int i = 0; i < schoollist.Count; i++)
                {
                    if (schoollist[i].ID == ma.schoolid)
                    {
                        schoollist.Remove(schoollist[i]);
                    }
                }

                //if (sinfo != null)
                //schoollist.Remove(sinfo);
            }
            List<KSWF.WFM.Constract.VW.vw_masterarea> NoSmalist = malist.Where(i => i.schoolid == 0).ToList().OrderByDescending(i => i.districtid.ToMinDistrictID()).ToList();

            for (int i = 0; i < NoSmalist.Count; i++)
            {
                List<tb_schoolinfo> tmpSlist = new List<tb_schoolinfo>();
                for (int j = 0; j < schoollist.Count; j++)
                {
                    //if (NoSmalist[i].districtid.StartsWith("4401"))
                    //{
                    //    string hold = "";
                    //}
                    //if (schoollist[j].ID == 38632)
                    //{
                    //    string hold = "";
                    //}
                    if (schoollist[j].DistrictID.StartsWith(NoSmalist[i].districtid.ToMinDistrictID()))
                    {
                        tmpSlist.Add(schoollist[j]);
                        query.Add(new tb_tmpareaschool
                        {
                            MasterName = NoSmalist[i].mastername,
                            DeptID = NoSmalist[i].deptid,
                            DeptName = NoSmalist[i].deptname,
                            SchoolID = schoollist[j].ID,
                            SchoolName = schoollist[j].SchoolName,
                            DistrictID = schoollist[j].DistrictID,
                            Path = schoollist[j].Area==null ?"":schoollist[j].Area.Trim(),
                            MasterType = NoSmalist[i].mastertype,
                            TrueName = NoSmalist[i].truename,
                            AgentID = NoSmalist[i].agentid,
                            AgentName = NoSmalist[i].agentname
                        });
                    }
                }
                foreach (tb_schoolinfo s in tmpSlist)
                {
                    schoollist.Remove(s);
                }
            }

            string sql = "TRUNCATE TABLE tb_tmpareaschool";
            ExcueteSql(sql);
            InsertRange<tb_tmpareaschool>(query.ToList());
            InsertLog("Tb_TmpAreaSchool", query.Count, "");
        }

        public string VerificationNull(string verificationclass)
        {
            if (!string.IsNullOrEmpty(verificationclass))
                return verificationclass.Trim();
            return "";
        }

        public bool InsertLog(string tablename, int count, string mastername)
        {
            tb_initlog model = new tb_initlog();
            model.InitTable = tablename;
            model.InitMaster = mastername;
            model.InitDate = DateTime.Now;
            model.InitDataCount = count;
            return Add<tb_initlog>(model) > 0;
        }
    }

    public static class ObjectExtend
    {


        //重写GetHashCode方法（重写Equals方法必须重写GetHashCode方法，否则发生警告

        public static string ToMinDistrictID(this object obj)
        {
            string temp = obj.ToString();
            try
            {
                string code = "";
                string[] strs = new string[4];
                strs[0] = temp.ToString().Substring(0, 2);
                strs[1] = temp.ToString().Substring(2, 2);
                strs[2] = temp.ToString().Substring(4, 2);
                strs[3] = temp.ToString().Substring(6);
                for (int i = 3; i >= 0; i--)
                {
                    if (int.Parse(strs[i]) != 0)
                    {
                        code = strs[i] + code;
                    }
                }
                return code;
            }
            catch
            {

            }
            return temp;
        }

        public static string ToGradeName(this object obj)
        {
            string temp = obj.ToString();
            try
            {
                string code = "";
                switch (temp)
                {
                    case "1":
                        code = "学前";
                        break;
                    case "2":
                        code = "一年级";
                        break;
                    case "3":
                        code = "二年级";
                        break;
                    case "4":
                        code = "三年级";
                        break;
                    case "5":
                        code = "四年级";
                        break;
                    case "6":
                        code = "五年级";
                        break;
                    case "7":
                        code = "六年级";
                        break;
                    case "8":
                        code = "七年级";
                        break;
                    case "9":
                        code = "八年级";
                        break;
                    case "10":
                        code = "九年级";
                        break;
                    case "11":
                        code = "高一";
                        break;
                    case "12":
                        code = "高二";
                        break;
                    case "13":
                        code = "高三";
                        break;
                    default:
                        code = "X年级";
                        break;
                }
                return code;
            }
            catch
            {

            }
            return temp;
        }
    }
}
