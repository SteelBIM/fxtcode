using KSWF.Framework.BLL;
using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSWF.Framework.DAL;
using MySql.Data.MySqlClient;
using KSWF.WFM.Constract.VW;

namespace KSWF.WFM.BLL
{
    public class OrderManage : OrderBaseManage
    {

        public bool AddOrder(orderinfo oinfo)
        {
            return Add<orderinfo>(oinfo) > 0;
        }

        public bool AddOrderProductInfo(List<order_pinfo> pinf)
        {
            return InsertRange<order_pinfo>(pinf).Count > 0;
        }

        /// <summary>
        /// 获取订单总金额
        /// </summary>
        /// <returns></returns>
        public string GetTotalAmount(string whereSql)
        {
            string sql = "select sum(o_payamount) as paycount from orderinfo where " + whereSql;
            string d = base.SelectString(sql, null);
            if (string.IsNullOrEmpty(d))
            {
                d = "0";
            }
            decimal de = decimal.Parse(d);
            return de.ToString("0.00");
        }

        /// <summary>
        /// 获取订单销售额和总金额
        /// </summary>
        /// <returns></returns>
        public ViewAmount GetSumAmount(string whereSql, string etime)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(whereSql))
            {
                sql = "select  ifnull(sum(o_payamount),0) as paycount, ifnull(sum(o_bonus),0) as actcount,count(*) as total from orderinfo where " + whereSql;
                sql += " && o_datetime>'" + etime + "'";
            }
            return base.SelectString<ViewAmount>(sql, null);
        }

        public ViewAmount GetAgentSumAmount(string whereSql, string etime)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(whereSql))
            {
                sql = "select  ifnull(sum(o_payamount),0) as paycount, ifnull(sum(o_bonus),0) as actcount,count(*) as total from vw_agentorderdetails where " + whereSql;
                sql += " && o_datetime>'" + etime + "'";
            }
            return base.SelectString<ViewAmount>(sql, null);
        }

        /// <summary>
        /// 应收结算管理，提成结算单，计算总提成金额
        /// </summary>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        public string GetTotalBonus(string whereSql, int pannelType)
        {
            string sql = "";
            if (pannelType == 0 || pannelType == 1)
            {
                sql = "select sum(total_bonus + adjust_amount) as totalBonus from order_setbonus ";
            }
            else
            {
                sql = "select sum(total_bonus + adjust_amount) as totalBonus from order_setbonus_dept ";
            }

            if (!string.IsNullOrEmpty(whereSql))
            {
                sql = sql + "where " + whereSql;
            }

            string d = base.SelectString(sql, null);
            if (string.IsNullOrEmpty(d))
            {
                d = "0";
            }
            decimal de = decimal.Parse(d);
            return de.ToString("0.00");
        }
        /// <summary>
        /// 获取订单总数
        /// </summary>
        /// <returns></returns>
        public int GetOrderCount(string whereSql)
        {
            return base.GetTotalCount<orderinfo>(whereSql);
        }

        /// <summary>
        /// 获取代理商未提成总订单
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public List<OrdreTotal> GetAgentNotCommissionedOrdreTotal(string agentid, string agentname, string deptids)
        {
            string where = "";
            if (!string.IsNullOrEmpty(agentname))
                where += " and cm.agentname='" + agentname + "' ";
            if (!string.IsNullOrEmpty(deptids))
                where += " and cmdept.deptid in (" + deptids + ")";
            string sql = string.Format(@"select sum(o_number) as o_number, sum(o_payamount)as o_payamount,sum(o_bonus) as o_bonus from (select  count(*)as o_number,sum(o_payamount)as o_payamount,sum(bouns) as o_bonus  from
                                        (select  cm.masterid,sum(p_bonus +p_class_bonus) as bouns,oi.o_payamount
                                        from fz_wfs.com_master cm 
                                        join fz_wfs_order.order_psetbonus opb on cm.agentid=opb.agentid
                                        join fz_wfs_order.order_pinfo op on op.guid=opb.op_guid
                                        join fz_wfs_order.orderinfo oi on oi.o_guid=op.o_guid
                                        join fz_wfs.com_master cmdept on cm.parentid=cmdept.masterid
                                        where cm.mastertype=1  and opb.mastertype=1
                                        and  timestampadd(day, 1,cm.agent_enddate)>o_datetime and o_datetime>cm.agent_startdate and  UNIX_TIMESTAMP(o_datetime)>IFNULL((select enddate from  fz_wfs_order.order_setbonus os where  os.mastername_t=cm.mastername  order by  createtime desc  limit 0,1 ),0)
                                        and opb.p_bonus>0 and cm.pagentid=@agentid {0} group by cm.pagentid, cm.masterid,oi.o_guid)tab  group by  masterid)t ", where);
            var result = SqlQuery<OrdreTotal>(sql, new List<MySqlParameter> { new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "agentid", Value = agentid } });
            return result;
        }

        /// <summary>
        /// 获取代理商提成总金额
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public List<OrdreTotal> GetAgentOrdreTotal(string agentid, string enddate, string stime = "")
        {
            string where = "";
            if (!string.IsNullOrEmpty(enddate))
                where += " and oi.o_datetime<'" + enddate + "'";
            if (!string.IsNullOrEmpty(stime))
                where += " and oi.o_datetime>'" + stime + "'";

            string sql = string.Format(@" select  sum( o_payamount) as o_payamount,sum( o_bonus)as o_bonus,count(o_number)as o_number
                                        from(select  ifnull( oi.o_payamount,0) as o_payamount,ifnull(sum( opb.p_bonus+opb.p_class_bonus),0)as o_bonus,count(oi.o_id)as o_number
                                         from fz_wfs_order.orderinfo  oi 
                                         join fz_wfs_order.order_pinfo op on oi.o_guid=op.o_guid
                                        join fz_wfs_order.order_psetbonus opb on op.guid=opb.op_guid
                                       join fz_wfs.com_master cm on opb.agentid=cm.agentid and cm.mastertype=1 and o_datetime<timestampadd(day, 1,agent_enddate)  and o_datetime>cm.agent_startdate
                                        where opb.p_bonus>0 and opb.mastertype=1   and UNIX_TIMESTAMP(o_datetime)>IFNULL((select enddate from  fz_wfs_order.order_setbonus os where os.mastername_t=cm.mastername  and os.os_type=1  order by  os.createtime desc  limit 0,1 ),0)
                                        and opb.agentid=@agentid" + where + " group by oi.o_guid)tab");
            var result = SqlQuery<OrdreTotal>(sql, new List<MySqlParameter> { new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "agentid", Value = agentid } });
            return result;
        }
        /// <summary>
        /// 获取代理商提成总金额(代理商展示)
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public List<OrdreTotal> GetAgentOrdreTotal(string agentid, int starttime, DateTime agent_endtime)
        {
            agent_endtime = agent_endtime.AddDays(1);
            string sql = string.Format(@"select  sum( o_payamount) as o_payamount,sum( o_bonus)as o_bonus,count(o_number)as o_number
                                        from(select oi.o_payamount as o_payamount,sum( opb.p_bonus+opb.p_class_bonus)as o_bonus,count(oi.o_id)as o_number
                                        from fz_wfs_order.orderinfo  oi 
                                        join fz_wfs_order.order_pinfo op on oi.o_guid=op.o_guid
                                        join fz_wfs_order.order_psetbonus opb on op.guid=opb.op_guid
                                        join fz_wfs.com_master cm on opb.agentid=cm.agentid and cm.mastertype=1 and opb.mastertype=1 and o_datetime< timestampadd(day, 1,cm.agent_enddate)  and o_datetime>cm.agent_startdate
                                        where opb.p_bonus>0 and UNIX_TIMESTAMP(o_datetime)> @starttime and o_datetime<@agent_endtime and opb.agentid=@agentid  group by oi.o_guid)tab");
            var result = SqlQuery<OrdreTotal>(sql, new List<MySqlParameter> { new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "agentid", Value = agentid }
            ,new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "starttime", Value = starttime }
            ,new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "agent_endtime", Value = agent_endtime }
            });
            return result;
        }

        /// <summary>
        /// 获取员工提成总详细
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public List<orderkont> Getemployee(string mastername, int starttime, int endtime)
        {
            string str = string.Format(@"select (select `Value`  from  fz_wfs.cfg_keyvalue ck where  ck.`Key`=tab.channel and UseType='Channel' order by ID desc  limit 0,1)as productname,tab.*, IfNULL(classnumber,0)as classnumber,ifnull(classpayamount,0)as classpayamount,ifnull(classactamount,0)as classactamount  from 
                                    (select  channel,p_category,p_version,divided,class_divided,count(*) as ordernumber,sum(o_payamount) as o_payamount,sum(o_actamount) as o_actamount,
                                    IFNULL(sum(p_bonus),0) as basis_bonus, IFNULL(sum(p_class_bonus),0) as p_class_bonus   
                                     from fz_wfs_order.orderinfo oi left join fz_wfs_order.order_pinfo op on oi.o_guid=op.o_guid
                                      JOIN fz_wfs_order.order_psetbonus opb on opb.op_guid=op.guid                                   
                                      where (o_totype=0 or o_totype=1) and m_mastername=@mastername and  @endtime>UNIX_TIMESTAMP(o_datetime) and UNIX_TIMESTAMP(o_datetime)>@starttime and oi.o_bonus>0 and opb.mastertype=0
                                     group by channel,p_category,p_version,divided,class_divided)tab left join 
                                    (select channel,p_category,p_version,divided,class_divided,count(*)  as classnumber, sum(oi.o_payamount) as classpayamount,sum(oi.o_actamount) as classactamount 
                                     from fz_wfs_order.orderinfo oi 
                                     join fz_wfs_order.order_pinfo op on oi.o_guid=op.o_guid 
                                     JOIN fz_wfs_order.order_psetbonus opb on opb.op_guid=op.guid       
                                    where (o_totype=0 or o_totype=1) and oi.classid is not null 
                                   and m_mastername=@mastername and @endtime>UNIX_TIMESTAMP(o_datetime) and UNIX_TIMESTAMP(o_datetime)>@starttime and oi.o_bonus>0 and opb.mastertype=0
                                    group by channel,p_category,p_version,divided,class_divided)
                                    ta on tab.channel=ta.channel and tab.p_category=ta.p_category and tab.p_version=ta.p_version and tab.divided=ta.divided and tab.class_divided=ta.class_divided");
            var result = SqlQuery<orderkont>(str, new List<MySqlParameter> {
                new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "mastername", Value = mastername }, 
                new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "starttime", Value = starttime }, 
                new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "endtime", Value = endtime }
            });
            return result;
        }
        /// <summary>
        /// 获取部门提成总详细
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public List<orderkont> Getdept(int deptid, int starttime, int endtime)
        {
            string str = string.Format(@"select  (select `Value`  from  fz_wfs.cfg_keyvalue ck where  ck.`Key`= oi.channel and UseType='Channel' order by ID desc  limit 0,1)as productname,
                                     channel,p_category,p_version,count(*) as ordernumber,sum(o_payamount) as o_payamount,sum(o_actamount) as o_actamount 
                                     from fz_wfs_order.orderinfo oi left join fz_wfs_order.order_pinfo op on oi.o_guid=op.o_guid
                                     where  o_totype=2  and m_deptid=@m_deptid and  @endtime>UNIX_TIMESTAMP(o_datetime) and UNIX_TIMESTAMP(o_datetime)>@starttime
                                     group by channel,p_category,p_version");
            var result = SqlQuery<orderkont>(str, new List<MySqlParameter> {
                new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "m_deptid", Value = deptid }, 
                new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "starttime", Value = starttime }, 
                new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "endtime", Value = endtime }
            });
            return result;
        }

        /// <summary>
        /// 获取代理商提成详细
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public List<orderkont> GetAgent(string agentid, int starttime, int endtime)
        {
            //            string sql = string.Format(@"select (select `Value`  from  fz_wfs.cfg_keyvalue ck where  ck.`Key`= tab.channel and UseType='Channel' order by ID desc  limit 0,1)as productname,
            //                                          tab.p_category,tab.p_version,tab.divided,tab.class_divided,count(*) as ordernumber,sum(tab.o_payamount) as o_payamount,sum(tab.o_actamount) as o_actamount,
            //                                          sum( tab.p_bonus)  as basis_bonus, sum( classnumber)  as classnumber , sum( classpayamount)  as classpayamount , sum( classactamount)  as classactamount, sum( p_class_bonus)  as p_class_bonus from
            //                                        ((select  oi.channel,  p_category,p_version,divided,class_divided,count(*) as ordernumber,sum(oi.o_payamount) as o_payamount,sum(o_actamount) as o_actamount,
            //                                         IFNULL(sum( p_bonus),0) as p_bonus   
            //                                         from fz_wfs_order.orderinfo oi  
            //                                         join fz_wfs_order.order_pinfo op on oi.o_guid=op.o_guid 
            //                                         join fz_wfs_order.order_psetbonus opb on op.guid=opb.op_guid
            //                                         where  o_datetime<@agent_endtime and   @endtime>UNIX_TIMESTAMP(o_datetime) and UNIX_TIMESTAMP(o_datetime)>@starttime and opb.p_bonus>0 and opb.agentid =@agentid
            //                                         group by channel,p_category,p_version,divided,class_divided,oi.o_guid)tab left join 
            //                                        (select  oi.channel,  p_category,p_version,divided,class_divided,count(*)  as classnumber, sum(oi.o_payamount) as classpayamount,sum(oi.o_actamount) as classactamount,IFNULL(sum( p_class_bonus),0) as p_class_bonus   
            //                                         from fz_wfs_order.orderinfo oi  
            //                                         join fz_wfs_order.order_pinfo op on oi.o_guid=op.o_guid 
            //                                         join fz_wfs_order.order_psetbonus opb on op.guid=opb.op_guid
            //                                         where  o_datetime<@agent_endtime and   @endtime>UNIX_TIMESTAMP(o_datetime) and UNIX_TIMESTAMP(o_datetime)>@starttime and opb.p_bonus>0 and opb.agentid =@agentid  and oi.classid is not null      
            //                                         group by channel,p_category,p_version,divided,class_divided,oi.o_guid)ta
            //                                          on tab.channel=ta.channel and tab.p_category=ta.p_category and tab.p_version=ta.p_version and tab.divided=ta.divided and tab.class_divided=ta.class_divided)
            //                                          group by tab.channel,tab.p_category,tab.p_version,tab.divided,tab.class_divided");
            string sqlstr = string.Format(@"select  (select `Value`  from  fz_wfs.cfg_keyvalue ck where  ck.`Key`=tab.channel and UseType='Channel' order by ID desc  limit 0,1)as productname,tab.p_category,tab.p_version,tab.divided,tab.class_divided,tab.ordernumber
                                        ,sum(tab.o_payamount) as o_payamount,sum(tab.o_actamount) as o_actamount,  sum( tab.p_bonus) as basis_bonus,sum(classnumber) as classnumber, sum(classpayamount) as classpayamount , sum( classactamount)  as classactamount, sum( p_class_bonus)  as p_class_bonus
                                        from(
                                        (select channel,p_category,p_version,divided,class_divided,count(*) as ordernumber,sum(o_payamount) as o_payamount, sum(o_actamount)as o_actamount,sum(p_bonus) as p_bonus from 

                                        (select  oi.channel,  p_category,p_version,divided,class_divided,sum(oi.o_payamount) as o_payamount,sum(o_actamount) as o_actamount,
                                                                                 IFNULL(sum( p_bonus),0) as p_bonus   
                                                                                 from fz_wfs_order.orderinfo oi  
                                                                                 join fz_wfs_order.order_pinfo op on oi.o_guid=op.o_guid 
                                                                                 join fz_wfs_order.order_psetbonus opb on op.guid=opb.op_guid
                                                                                 join fz_wfs.com_master cm on opb.agentid=cm.agentid and cm.mastertype=1 and o_datetime< timestampadd(day, 1,cm.agent_enddate)  and o_datetime>cm.agent_startdate                                                                              
                                                                                 where  @endtime>UNIX_TIMESTAMP(o_datetime) and UNIX_TIMESTAMP(o_datetime)>@starttime and opb.p_bonus>0 and opb.agentid =@agentid and opb.mastertype=1
                                                                                 group by channel,p_category,p_version,divided,class_divided,oi.o_guid)t1
                                         group by channel,p_category,p_version,divided,class_divided
                                         )tab 
                                        left join 
                                        (select channel,p_category,p_version,divided,class_divided,count(*)  as classnumber,sum(classpayamount) as classpayamount,sum(classactamount) as classactamount,sum( p_class_bonus) as p_class_bonus
                                         from (
                                           select  oi.channel,  p_category,p_version,divided,class_divided, sum(oi.o_payamount) as classpayamount,sum(oi.o_actamount) as classactamount,IFNULL(sum( p_class_bonus),0) as p_class_bonus   
                                                                                 from fz_wfs_order.orderinfo oi  
                                                                                 join fz_wfs_order.order_pinfo op on oi.o_guid=op.o_guid 
                                                                                 join fz_wfs_order.order_psetbonus opb on op.guid=opb.op_guid
                                                                                 join fz_wfs.com_master cm on opb.agentid=cm.agentid and cm.mastertype=1 and o_datetime< timestampadd(day, 1,cm.agent_enddate)  and o_datetime>cm.agent_startdate                                                                                
                                                                                 where  @endtime>UNIX_TIMESTAMP(o_datetime) and UNIX_TIMESTAMP(o_datetime)>@starttime and opb.p_bonus>0 and opb.agentid =@agentid  and oi.classid is not null    and opb.mastertype=1 
                                                                                 group by channel,p_category,p_version,divided,class_divided,oi.o_guid)t2 
                                          group by channel,p_category,p_version,divided,class_divided
                                        )ta
                                        on tab.channel=ta.channel and tab.p_category=ta.p_category and tab.p_version=ta.p_version and tab.divided=ta.divided and tab.class_divided=ta.class_divided)
                                        group by tab.channel,tab.p_category,tab.p_version,tab.divided,tab.class_divided");
            var result = SqlQuery<orderkont>(sqlstr, new List<MySqlParameter> {
                 new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "agentid", Value = agentid }, 
                new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "starttime", Value = starttime }, 
                new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "endtime", Value = endtime } 
            });
            return result;
        }


        /// <summary>
        /// 根据权限获员工取提成总金额
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public List<orderdetailed> GetEmplloyerTotal(string wheresql, string m_mastername, int dataauthority)
        {
            DateTime endtime = DateTime.Now.Date;
            DateTime starttime = endtime.AddMonths(-1);
            string authority = "";
            if (dataauthority > 0)
                authority = "(((o_totype=0 or o_totype=1) and o_bonus>0) or o_totype=2)  and ";
            string str = string.Format(@"select  DATE_FORMAT(o_datetime,'%Y%m%d') days,count(*) as ordernumber,ifnull( sum(o_payamount),0) as payamount,ifnull( sum(o_bonus),0) as bonus  from  fz_wfs_order.orderinfo 
                                        where  ( {0} '{1}'>o_datetime and o_datetime>='{2}') and ( m_mastername=@mastername   {3}) group by days; ", authority, endtime, starttime, wheresql);
            var result = SqlQuery<orderdetailed>(str, new List<MySqlParameter> {
                new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "mastername", Value = m_mastername }  
            });
            return result;
        }
        /// <summary>
        /// 获取部门员工代理商ID
        /// </summary>
        /// <param name="deptids"></param>
        /// <returns></returns>
        public List<com_master> GetDeptEmplloyelAgentId(string deptids)
        {
            string str = "select cme.agentid from  fz_wfs.com_master cm join  fz_wfs.com_master cme on cm.mastername=cme.parentname where cm.deptid in(" + deptids + ")";
            var result = SqlQuery<com_master>(str, new List<MySqlParameter> { new MySqlParameter { } });
            return result;
        }

        /// <summary>
        /// 获取员工本人提成提成详细
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public List<orderdetailed> GetEmployeeDetailed(string mastername)
        {
            DateTime endtime = DateTime.Now.Date;
            DateTime starttime = endtime.AddMonths(-1);
            string str = string.Format(@"select DATE_FORMAT(o_datetime,'%Y%m%d') days,count(*)as ordernumber, ifnull(sum(o_bonus),0)as bonus,ifnull(sum(o_payamount),0)as payamount  from orderinfo 
                                       where (o_totype=0 or o_totype=1) and o_bonus>0 and  '{0}'>o_datetime and o_datetime>='{1}'  and m_mastername=@mastername group by days;", endtime, starttime);
            var result = SqlQuery<orderdetailed>(str, new List<MySqlParameter> {
                new MySqlParameter { MySqlDbType = MySqlDbType.String, ParameterName = "mastername", Value = mastername }
            });
            return result;
        }


        /// <summary>
        /// 获取代理商提成提成详细
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public List<orderdetailed> GetAgentDetailed(string agentid, int isbonus, string deptids = "")
        {
            string bonuswhere = "";
            if (isbonus > 0)
                bonuswhere = "pm.p_bonus>0 and ";
            if (!string.IsNullOrEmpty(deptids))
            {
                deptids = " and cmchnnl.deptid in (" + deptids.TrimEnd(',') + ") ";
            }
            if (!string.IsNullOrEmpty(agentid))
            {
                DateTime endtime = DateTime.Now.Date;
                DateTime starttime = endtime.AddMonths(-1);
                string str = string.Format(@"select DATE_FORMAT(o_datetime,'%Y%m%d') days ,ifnull(sum(bouns),0)as bonus ,ifnull(sum(o_payamount),0)as payamount,count(o_datetime) as ordernumber    from 
                                         (select oi.o_datetime,sum(pm.p_bonus+pm.p_class_bonus)as bouns,oi.o_payamount from 
                                         fz_wfs_order.orderinfo oi
                                         join fz_wfs_order.order_pinfo op  on oi.o_guid=op.o_guid
                                         join fz_wfs_order.order_psetbonus pm on op.guid=pm.op_guid
                                         join fz_wfs.com_master cm on pm.agentid=cm.agentid and cm.mastertype=1 and pm.mastertype=1 -- and timestampadd(day, 1,cm.agent_enddate)   and o_datetime>cm.agent_startdate
                                         join fz_wfs.com_master cmchnnl on cm.parentid=cmchnnl.masterid                                        
                                         where {0} '{1}'>o_datetime and o_datetime>='{2}'   {3} {4} group by  oi.o_datetime,oi.o_payamount )tab group by days;", bonuswhere, endtime, starttime, agentid.TrimEnd(','), deptids);
                var result = SqlQuery<orderdetailed>(str, new List<MySqlParameter> { });
                return result;
            }
            return new List<orderdetailed>();
        }



        /// <summary>
        /// 获取本人的下级代理商
        /// </summary>
        /// <param name="mastername"></param>
        /// <returns></returns>
        public List<orderdetailed> GetParehAgentDetailed(string mastername)
        {

            DateTime endtime = DateTime.Now.Date;
            DateTime starttime = endtime.AddMonths(-1);
            string str = string.Format(@"select DATE_FORMAT(o_datetime,'%Y%m%d') days ,ifnull(sum(bouns),0)as bonus ,ifnull(sum(o_payamount),0)as payamount,count(o_datetime) as ordernumber    from 
                                         (select oi.o_datetime,sum(pm.p_bonus+pm.p_class_bonus)as bouns,oi.o_payamount from 
                                         fz_wfs_order.orderinfo oi
                                         join fz_wfs_order.order_pinfo op  on oi.o_guid=op.o_guid
                                         join fz_wfs_order.order_psetbonus pm on op.guid=pm.op_guid
                                         join fz_wfs.com_master cm on pm.agentid=cm.agentid and cm.mastertype=1 and pm.mastertype=1 -- and o_datetime< timestampadd(day, 1,cm.agent_enddate)  and o_datetime>cm.agent_startdate                                     
                                         where pm.p_bonus>0 and '{0}'>o_datetime and o_datetime>='{1}' and cm.parentname ='{2}'    group by  oi.o_datetime,oi.o_payamount )tab group by days;", endtime, starttime, mastername);
            var result = SqlQuery<orderdetailed>(str, new List<MySqlParameter> { });
            return result;
        }
      
    }
}
