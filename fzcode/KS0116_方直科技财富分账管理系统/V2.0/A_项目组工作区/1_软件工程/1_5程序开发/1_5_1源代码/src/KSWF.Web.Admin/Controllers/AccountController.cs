using KSWF.Web.Admin.Models;
using KSWF.WFM.BLL;
using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace KSWF.Web.Admin.Controllers
{
    public class AccountController : BaseController
    {
        //我的账户
        //2017-06-01
        public ActionResult Index()
        {
            List<orderdetailed> employeeorderlist = new List<orderdetailed>();
            List<orderdetailed> agentorderlist = new List<orderdetailed>();
            List<orderdetailed> deptorderlist = new List<orderdetailed>();
            List<orderdetailed> orderlist = new List<orderdetailed>();

            OrderManage manage = new OrderManage();
            Recursive give = new Recursive();
            if (masterinfo.dataauthority == (int)Dataauthority.全部)
            {
                if (UserIdentity == 1)
                {
                    agentorderlist = manage.GetAgentDetailed(" and  pm.agentid='" + masterinfo.agentid + "'",1);
                }
                else
                {
                    agentorderlist = manage.GetAgentDetailed(" and  cm.pagentid='" + masterinfo.agentid + "'",0);
                    employeeorderlist = manage.GetEmplloyerTotal(" or  agentid='" + masterinfo.agentid + "'  ", masterinfo.mastername, masterinfo.dataauthority);
                }
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商)
            {
                List<string> InIds = give.GetDeptNodeId(masterinfo.deptid, masterinfo.agentid);
                InIds.Add(masterinfo.deptid.ToString());
                employeeorderlist = manage.GetEmplloyerTotal(GetWhereDeptSql(InIds), masterinfo.mastername, masterinfo.dataauthority);
                if (UserIdentity == 1)
                {
                    agentorderlist = manage.GetAgentDetailed(" and  pm.agentid='" + masterinfo.agentid + "'", 1,GetDeptIds(InIds));
                }
                else
                {
                    agentorderlist = manage.GetAgentDetailed(" and  cm.pagentid='" + masterinfo.agentid + "'", 1, GetDeptIds(InIds));
                }
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人下级部门下级代理商)
            {
                List<string> InIds = give.GetDeptNodeId(masterinfo.deptid, masterinfo.agentid);
                employeeorderlist = manage.GetEmplloyerTotal(GetWhereDeptSql(InIds), masterinfo.mastername, masterinfo.dataauthority);
                if (UserIdentity == 1)
                {
                    agentorderlist = manage.GetAgentDetailed(" and  pm.agentid='" + masterinfo.agentid + "'", 1, GetDeptIds(InIds));
                }
                else
                {
                    if (InIds != null && InIds.Count > 0)
                        agentorderlist = manage.GetAgentDetailed(" and  cm.pagentid='" + masterinfo.agentid + "'", 1, GetDeptIds(InIds));
                }
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人下级代理商)
            {
                employeeorderlist = manage.GetEmployeeDetailed(masterinfo.mastername);
                agentorderlist = manage.GetParehAgentDetailed(masterinfo.mastername);
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人)
            {
                employeeorderlist = manage.GetEmployeeDetailed(masterinfo.mastername);
            }

            if (employeeorderlist != null && employeeorderlist.Count > 0 && agentorderlist != null && agentorderlist.Count > 0)
            {
                int endtime = Convert.ToInt32(DateTime.Now.Date.ToString("yyyy-MM-dd").Replace("-", ""));
                int starttime = Convert.ToInt32(DateTime.Now.Date.AddMonths(-1).ToString("yyyy-MM-dd").Replace("-", ""));
                for (int i = starttime; i <= endtime; i++)
                {
                    orderdetailed order = new orderdetailed();
                    order.days = i.ToString();
                    order.bonus = 0;
                    order.payamount = 0;
                    foreach (orderdetailed agent in agentorderlist)
                    {
                        if (i.ToString() == agent.days)
                        {
                            order.ordernumber += agent.ordernumber;
                            order.payamount += agent.payamount;
                            order.bonus += agent.bonus == null ? 0 : agent.bonus;
                            break;
                        }
                    }
                    foreach (orderdetailed emplloy in employeeorderlist)
                    {
                        if (i.ToString() == emplloy.days)
                        {
                            order.ordernumber += emplloy.ordernumber;
                            order.payamount += emplloy.payamount;
                            order.bonus += emplloy.bonus == null ? 0 : emplloy.bonus;
                            break;
                        }
                    }
                    if (order.ordernumber > 0)
                        orderlist.Add(order);
                    string daynumber = i.ToString().Substring(6, 2);
                    if (daynumber == "31")
                        i = i + 69;
                }
            }
            else if (agentorderlist != null && agentorderlist.Count > 0)
            {
                orderlist = agentorderlist;
            }
            else if (employeeorderlist != null && employeeorderlist.Count > 0)
            {
                orderlist = employeeorderlist;
            }
            if (orderlist.Count == 0)
            {
                orderlist.Add(new orderdetailed() { days = "", payamount = 0, bonus = 0, ordernumber = 0 });
            }
            OrdreTotal entity = new OrdreTotal();
            string times = "";
            string number = "";
            string payamout = "";
            if (orderlist != null && orderlist.Count > 0)
            {
                foreach (orderdetailed row in orderlist)
                {
                    entity.o_number += row.ordernumber;
                    entity.o_bonus += row.bonus;
                    entity.o_payamount += row.payamount;
                    times += ConTime(row.days) + ",";
                    number += row.ordernumber + ",";
                    payamout += row.payamount.ToString() + ",";
                }
            }
            ViewBag.Total = entity;
            ViewBag.TotalDetailed = orderlist;
            ViewBag.times = times.TrimEnd(',');
            ViewBag.number = number.TrimEnd(',');
            ViewBag.payamout = payamout.TrimEnd(',');
            return View();
        }

        #region 日期类型优化
        public string ConTime(string days)
        {
            if (!string.IsNullOrEmpty(days))
            {
                if (days.Length == 8)
                {
                    string moneys = days.Substring(4, 4);
                    return moneys.Substring(0, 2) + "-" + moneys.Substring(2, 2);
                }
            }
            return days;
        }
        #endregion

        public string GetDeptIds(List<string> DeptList)
        {
            string deptids = "";
            if (DeptList != null && DeptList.Count > 0)
            {
                foreach (string row in DeptList)
                    deptids += row + ",";
            }
            return deptids;
        }
        public string GetWhereDeptSql(List<string> DeptIds)
        {
            string wheresql = "";
            string deptids = "";
            if (DeptIds != null && DeptIds.Count > 0)
            {
                foreach (string row in DeptIds)
                    deptids += row + ",";
            }

            if (!string.IsNullOrEmpty(deptids))
                wheresql += " or m_deptid in (" + deptids.TrimEnd(',') + ")";
            return wheresql;
        }
    }
}
