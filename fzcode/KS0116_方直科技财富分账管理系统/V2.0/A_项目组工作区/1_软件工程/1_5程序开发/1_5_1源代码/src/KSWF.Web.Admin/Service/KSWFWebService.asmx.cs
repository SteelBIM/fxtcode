using KSWF.Core.Utility;
using KSWF.Framework.BLL;
using KSWF.WFM.BLL;
using KSWF.WFM.Constract.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace KSWF.Web.Admin.Service
{
    /// <summary>
    /// KSWFWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class KSWFWebService : System.Web.Services.WebService
    {
        public CertficateSoapHeader soapHeader = new CertficateSoapHeader();
        KSWF.WFM.BLL.ProductManage Pmanage = new WFM.BLL.ProductManage();
        OrderManage Omanage = new OrderManage();

        [SoapCertficate]
        [SoapHeader("soapHeader", Direction = SoapHeaderDirection.In)]
        [WebMethod]
        public ReturnInfo AddProduct(Models.Product productInfo)
        {
            
            try
            {
                #region 记录日志，调试操作
                int index = Convert.ToInt32(ConfigurationManager.AppSettings["Index"]);
                index++;
                TestLog4Net.LogHelper.WriteLog(typeof(KSWFWebService), productInfo.ProductNo + "|" + productInfo.Isshevel + "|" + index);
                ConfigurationManager.AppSettings["Index"] = index.ToString();
                #endregion

                ReturnInfo rinfo = new ReturnInfo();
                rinfo.Success = true;
                if (productInfo == null)
                {
                    rinfo.Success = false;
                    rinfo.ErrorMsg = "传入参数有误";
                }

                List<cfg_product> list = Pmanage.SelectSearch<cfg_product>(x => x.productno == productInfo.ProductNo);
                cfg_product pinfo = new cfg_product();
                pinfo.category = productInfo.Category;
                pinfo.categorykey = productInfo.CategoryKey;
                pinfo.channel = productInfo.Channel ?? 0;
                pinfo.grade = productInfo.Grade;
                pinfo.gradeid = productInfo.GradeID ?? 0;
                pinfo.productname = productInfo.ProductName;
                pinfo.productno = productInfo.ProductNo;
                pinfo.subject = productInfo.Subject;
                pinfo.subjectid = productInfo.SubjectID ?? 0;
                pinfo.version = productInfo.Version;
                pinfo.versionid = productInfo.VersionID ?? 0;
                pinfo.price = productInfo.Price;
                pinfo.isshevel = productInfo.Isshevel;

                if (list.Count == 0)
                {
                    KSWF.Core.Utility.KingResponse krinfo = Pmanage.CheckProductInfo(pinfo);
                    if (krinfo.Success)
                    {
                        int re = 0;
                        if (pinfo.grade == null)
                        {
                            re = Pmanage.Add<cfg_product>(pinfo, new string[] { "grade" });
                        }
                        else
                        {
                            re = Pmanage.Add<cfg_product>(pinfo);
                        }

                        if (re <= 0)
                        {
                            rinfo.Success = false;
                            rinfo.ErrorMsg = "插入信息失败";
                        }
                    }
                }
                else
                {
                    pinfo.id = list.First().id;
                    if (!Pmanage.Update<cfg_product>(pinfo))
                    {
                        rinfo.Success = false;
                        rinfo.ErrorMsg = "更新信息失败";
                        return rinfo;
                    }
                }
                return rinfo;
            }
            catch (Exception ex)
            {
                TestLog4Net.LogHelper.WriteLog(typeof(KSWFWebService), ex.Message);
                return new ReturnInfo
                {
                    Success = false,
                    ErrorMsg = "服务器内部错误！"
                };
            }
        }


        [SoapCertficate]
        [SoapHeader("soapHeader", Direction = SoapHeaderDirection.In)]
        [WebMethod]
        public ReturnInfo AddOrderInfo(Order oInfo)
        {
            int index = Convert.ToInt32(ConfigurationManager.AppSettings["Index"]);
            index++;
            ReturnInfo rinfo = new ReturnInfo();
            try
            {
                #region 记录日志，调试操作
                //TestLog4Net.LogHelper.WriteLog(typeof(KSWFWebService), oInfo.OrderID + "|" + index);
                ConfigurationManager.AppSettings["Index"] = index.ToString();
                #endregion

                orderinfo order = new orderinfo();

                if (oInfo == null)
                {
                    rinfo.Success = false;
                    rinfo.ErrorMsg = "请提供订单信息";
                    TestLog4Net.LogHelper.WriteLog(typeof(KSWFWebService), rinfo.ErrorMsg + "|" + index);
                    return rinfo;
                }

                order.o_guid = Guid.NewGuid();
                List<orderinfo> olist = Omanage.SelectSearch<orderinfo>(i => i.o_id == oInfo.OrderID);
                if (olist != null && olist.Count > 0)
                {
                    rinfo.Success = false;
                    rinfo.ErrorMsg = "订单已经存在";
                    TestLog4Net.LogHelper.WriteLog(typeof(KSWFWebService), rinfo.ErrorMsg + "|" + index + "|" + oInfo.OrderID);
                    return rinfo;
                }

                #region 订单信息去往支付系统校验
                if (oInfo.PayType != 2)
                {
                    if (!CheckOrderService(oInfo.OrderID))
                    {
                        rinfo.Success = false;
                        rinfo.ErrorMsg = "订单支付系统结果确认失败";
                        TestLog4Net.LogHelper.WriteLog(typeof(KSWFWebService), rinfo.ErrorMsg + "|" + index);
                        return rinfo;

                        //order.err_flg = 101;
                        //order.err_msg = "订单支付系统结果确认失败";
                    }
                }
                #endregion

                order.o_id = oInfo.OrderID;
                order.o_datetime = oInfo.OrderDate;
                order.u_ip = oInfo.UserClientIP;
                order.classid = oInfo.ClassID;
                order.classname = oInfo.ClassName;
                order.u_teacherid = oInfo.TeacherUserID;
                order.u_teachername = oInfo.TeacherUserName;
                order.u_userid = oInfo.BuyUserID;
                order.u_mobile = oInfo.BuyUserPhone;
                order.o_payamount = oInfo.PayAmount;
                order.o_feetype = oInfo.PayType;
                order.channel = oInfo.Channel;
                order.schoolid = oInfo.SchoolID;
                order.gradeid = oInfo.GradeID;
                order.gradename = oInfo.GradeName;

                #region 计算订单手续费
                if (!GetOrderFeeAmount(ref order))
                {
                    order.err_flg += 102;
                    order.err_msg += " | 没有查到支付费率";
                }
                #endregion

                //判断学校编号是否匹配
                FindArea(ref order);

                //通过区域ID去找区域负责人,如果学校不存在，区域也不存在，订单归属公司
                GetOrderMaster(ref order);

                #region 获取订单中的产品列表
                List<order_pinfo> oplist = new List<order_pinfo>();
                if (!string.IsNullOrEmpty(oInfo.ProductNO))
                {
                    string[] pnos = oInfo.ProductNO.Split(',');
                    if (pnos.Length < 0)
                    {
                        order.err_flg += 103;
                        order.err_msg += " | 未提供商品信息";
                    }
                    else
                    {
                        IList<cfg_product> plist = Pmanage.SelectSearch<cfg_product>(i => pnos.Contains(i.productno));
                        if (plist == null || plist.Count == 0)
                        {
                            order.err_flg += 104;
                            order.err_msg += string.Format(" | 根据商品编号:{0}，未找到商品信息", oInfo.ProductNO);//根据商品编号，未找到商品信息                    
                        }
                        else
                        {
                            foreach (cfg_product p in plist)
                            {
                                order_pinfo pinfo = new order_pinfo();
                                pinfo.o_pno = p.productno;
                                pinfo.p_name = p.productname;
                                pinfo.p_subjectid = p.subjectid;
                                pinfo.p_subject = p.subject;
                                pinfo.p_versionid = p.versionid;
                                pinfo.p_version = p.version;
                                pinfo.p_category = p.category;
                                pinfo.p_categorykey = p.categorykey;
                                pinfo.gradeid = p.gradeid;
                                pinfo.gradename = p.grade;
                                pinfo.guid = Guid.NewGuid();
                                pinfo.p_price = p.price;
                                pinfo.o_guid = order.o_guid.Value;
                                oplist.Add(pinfo);
                            }
                        }
                    }
                }
                #endregion

                List<order_psetbonus> ops = new List<order_psetbonus>();
                if (order.err_flg != 102)
                {
                    //通过区域负责人找到分账比例，计算提成金额
                    ops = GetOrderBonus(ref order, oplist);
                }

                #region 事务队列
                List<RepositoryAction> actions = new List<RepositoryAction>();
                RepositoryAction ac = new RepositoryAction();
                ac.Entity = order;
                ac.Actions = Acitons.Insert;
                actions.Add(ac);
                if (order.err_flg != 103 && order.err_flg != 104)
                {
                    foreach (order_pinfo item in oplist)
                    {
                        RepositoryAction tac = new RepositoryAction();
                        tac.Actions = Acitons.Insert;
                        tac.Entity = item;
                        actions.Add(tac);
                    }

                    foreach (order_psetbonus item1 in ops)
                    {
                        RepositoryAction tac1 = new RepositoryAction();
                        tac1.Actions = Acitons.Insert;
                        tac1.Entity = item1;
                        actions.Add(tac1);
                    }
                }

                if (Omanage.TransactionOperate(actions))
                {
                    rinfo.Success = true;
                }
                else
                {
                    rinfo.Success = false;
                    rinfo.ErrorMsg = "服务内部错误，插入数据操作失败";
                    TestLog4Net.LogHelper.WriteLog(typeof(KSWFWebService), rinfo.ErrorMsg + "|" + index);
                }
                #endregion
                return rinfo;
            }
            catch (Exception ex)
            {
                TestLog4Net.LogHelper.WriteLog(typeof(KSWFWebService), ex.Message + "| 操作数据库异常" + "|" + index);
                rinfo.Success = false;
                rinfo.ErrorMsg = "服务内部错误";
                return rinfo;
            }
        }

        /// <summary>
        /// 寻找区域负责人
        /// </summary>
        /// <param name="order"></param>
        /// <param name="message"></param>
        private void FindArea(ref orderinfo order)
        {

            //通过学校获取区域
            if (order.schoolid != null && order.schoolid.Value != 0)
            {
                //服务不同，数据不同（待最后确定）
                AreaService.ServiceSoapClient client1 = new AreaService.ServiceSoapClient();
                string schoolstr = client1.GetSchoolInfo(order.schoolid.Value);
                //KSWF.WFM.BLL.MateService.Service mateservice = new WFM.BLL.MateService.Service();
                //string schoolstr = mateservice.GetSchoolInfo(order.schoolid.Value);
                if (schoolstr.IndexOf("错误|") == -1)
                {
                    KSWF.Web.Admin.Models.ViewSchoolInfo schoolInfo = JsonConvert.DeserializeObject<KSWF.Web.Admin.Models.ViewSchoolInfo>(schoolstr);
                    //var strSchoolTypeNo = ConfigurationManager.AppSettings["SchoolTypeNo"];
                    //var schoolTypeNos = strSchoolTypeNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (schoolInfo != null)
                    {
                        //if (!schoolTypeNos.Contains(schoolInfo.SchoolTypeNo))
                        //{
                        //    order.schoolid = null;
                        //}
                        //else
                        //{
                        order.districtid = int.Parse(schoolInfo.DistrictID);//这里直接取得区域号，没有考虑城镇和街道
                        order.path = schoolInfo.Area.Trim();
                        order.schoolname = schoolInfo.SchoolName;
                        return;
                        //}
                    }
                }
                else
                {
                    //order.err_flg = 105;
                    //order.err_msg = schoolstr;
                }
            }

            HttpClient client = new HttpClient();

            //通过手机号获取区域
            if (!string.IsNullOrEmpty(order.u_mobile))
            {
                //或购买服务 www.juhe.com
                var returnData = client.GetStringAsync("http://cx.shouji.360.cn/phonearea.php?number=" + order.u_mobile).Result;//13715338369
                var data = JsonConvert.DeserializeObject<KSWF.Web.Admin.Models.ViewProvinceCity>(returnData);
                if (data != null)
                {
                    if (SelectAreaOrDept(order, data.data.city, data.data.province))
                    {
                        return;
                    }
                }
                return;
            }

            //通过IP地址获取区域
            if (!string.IsNullOrEmpty(order.u_ip))
            {
                //或购买服务 www.juhe.com
                var strData = client.GetStringAsync("http://int.dpool.sina.com.cn/iplookup/iplookup.php?ip=" + order.u_ip).Result;//14.215.177.37

                var strAreaName = strData.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (strAreaName.Count() > 5)
                {
                    if (SelectAreaOrDept(order, strAreaName[5], strAreaName[4]))
                    {
                        return;
                    }
                }
            }
            //如果以上都查不到数据，未知区域
            order.districtid = 0;
            order.path = "未知区域";
        }

        private bool SelectAreaOrDept(orderinfo order, string city, string province)
        {
            //通过市去查找区域及部门
            if (!string.IsNullOrEmpty(city))
            {
                if (SelectAreaLike(order, city))
                {
                    return true;
                }
            }

            //通过省去查找区域及部门
            if (!string.IsNullOrEmpty(province))
            {
                if (SelectAreaLike(order, province))
                {
                    return true;
                }
            }
            return false;
        }

        private bool SelectAreaLike(orderinfo order, string likeValue)
        {
            //找人
            var listMaster = Pmanage.SelectSearch<join_mastertarea>("path like '%" + likeValue + "%'");
            if (listMaster.Count > 0)
            {
                //几种情况：1.一个区域 2.两个区域，父子区域，选子区域 3.多个区域，归属不到人

                if (listMaster.Count == 1)
                {
                    order.districtid = Convert.ToInt32(listMaster.First().districtid);
                    order.path = listMaster.First().path;
                    return true;
                }
                else if (listMaster.Count == 2)
                {
                    order.districtid = Convert.ToInt32(listMaster.OrderByDescending(x => x.path.Length).First().districtid);
                    order.path = listMaster.OrderByDescending(x => x.path.Length).First().path;
                    return true;
                }
            }

            //找部门
            var listDept = Pmanage.SelectSearch<base_deptarea>("path like '%" + likeValue + "%'");
            if (listDept.Count > 0)
            {
                if (listDept.Count == 1)
                {
                    order.districtid = listDept.First().districtid;
                    order.path = listDept.First().path;
                    return true;
                }
                else
                {
                    var base_deptarea = listDept.OrderBy(x => x.path.Length).First();
                    order.districtid = base_deptarea.districtid;
                    order.path = base_deptarea.path;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取订单归属
        /// </summary>
        /// <param name="order"></param>
        /// <param name="message"></param>
        private void GetOrderMaster(ref orderinfo order)
        {
            Models.Recursive rec = new Models.Recursive();
            order.m_mastername = "";
            bool isAff = false;
            if (order.schoolid != null || order.districtid != null)
            {
                com_master masterinfo = rec.GetUserNameDeptId(order.channel, order.districtid, order.schoolid);
                if (masterinfo != null)
                {
                    if (!string.IsNullOrEmpty(masterinfo.mastername))//存在负责人
                    {
                        order.m_mastername = masterinfo.mastername;
                        order.m_mastertype = masterinfo.mastertype.ToString();
                        order.agentid = masterinfo.agentid;//渠道,是否与agentid相同判断直销还是代理
                        order.m_a_name = masterinfo.truename;

                        //通过用户找部门信息
                        base_dept deptinfo = Pmanage.GetDeptInfoByMasterID(masterinfo.deptid);
                        if (deptinfo != null)
                        {
                            order.m_deptid = deptinfo.deptid;
                            order.m_deptname = deptinfo.deptname;
                        }
                        isAff = true;
                    }
                    else if (masterinfo.mastername == "")//没有负责人，只有部门
                    {

                        order.m_deptid = masterinfo.deptid;
                        order.m_deptname = masterinfo.password;//特殊字段，传递部门名称
                        order.agentid = masterinfo.agentid;
                        isAff = true;
                    }
                    else
                    {   //部门或者负责人都不存在
                        //如果没有归于部门，归属于方直科技
                        order.m_deptid = 1;
                        order.m_deptname = "总公司";
                        order.agentid = "KSWF";
                        isAff = true;
                    }
                }
            }
            if (isAff == false)
            {
                order.m_deptid = 1;
                order.m_deptname = "总公司";
                order.agentid = "KSWF";
            }
        }

        /// <summary>
        /// 计算订单提成
        /// </summary>
        /// <param name="order"></param>
        /// <param name="message"></param>
        private List<order_psetbonus> GetOrderBonus(ref orderinfo order, List<order_pinfo> oplist)
        {
            if (string.IsNullOrEmpty(order.m_mastername)) //如果不存在归属负责人
            {
                if (order.m_deptid == 1)//归属方直
                {
                    order.o_totype = 3;
                }
                else//归属部门（计算团队提成）
                {
                    order.o_totype = 2;
                }
            }
            else
            {
                if (order.classid.HasValue)//自己（计算基础提成+班级奖励）
                {
                    //校验班级是否存在
                    order.o_totype = 0;
                }
                else//归属自己（计算基础提成）
                {
                    order.o_totype = 1;
                }
            }
            return GetBonus(order, oplist);
        }

        /// <summary>
        /// 计算提成
        /// </summary>
        /// <param name="type">提成类型</param>
        /// <returns></returns>
        public List<order_psetbonus> GetBonus(orderinfo order, List<order_pinfo> oplist)
        {
            List<order_psetbonus> op = new List<order_psetbonus>();
            //获取负责人的提成比例
            switch (order.o_totype)
            {
                case 0://自己（计算基础提成+班级奖励）
                    op = GetPersonBonus(order, true, oplist);
                    break;
                case 1://归属自己（计算基础提成）
                    op = GetPersonBonus(order, false, oplist);
                    break;
                case 2://归属部门（计算团队提成）
                    op = GetDeptBonus(order, oplist);
                    break;
                case 3://归属方直
                    break;
            }
            return op;
        }

        private List<order_psetbonus> GetPersonBonus(orderinfo order, bool hasClassDivided, List<order_pinfo> oplist)
        {
            /* 注意多产品的算法：
             *             总价=产品1单价+产品2单价
             *             总差价=总价-实际到账
             *             产品1比例= 产品1单价/总价
             *             产品1提成=（产品1单价-总差价*产品1比例）*产品1提成比例
             */
            order.o_bonus = 0;
            List<order_psetbonus> order_psetbonuss = new List<order_psetbonus>();
            if (oplist != null && oplist.Count > 0)
            {
                decimal sum_Price = 0;
                oplist.ForEach(x => sum_Price += x.p_price);
                var dif_Amount = sum_Price - order.o_actamount;

                List<com_master> masters = new List<com_master>();
                if (order.m_mastertype == "0")
                {
                    List<com_master> com_masters = Pmanage.SelectSearch<com_master>(x => (x.mastername == order.m_mastername));
                    if (com_masters.Count > 0)//&& com_masters.First().agentid == "KSWF" //(代理商员工是否增加明细)
                    {
                        masters.Add(com_masters.First());
                    }
                }
                //代理商添加自己，以及递归查出所有的父代理 
                GetParentAgencys(order.agentid, masters);

                foreach (var pinfo in oplist)
                {
                    for (int i = 0; i < masters.Count; i++)
                    {
                        order_psetbonus op = new order_psetbonus();
                        var bpolicys = Pmanage.SelectSearch<join_masterbpolicypr, cfg_bpolicy>((T1, T2) => T1.mastername == masters[i].mastername
                                  && T2.pid == order.channel, (T1, T2) => T1.bid == T2.bid);
                        List<string> bids = new List<string>();
                        bpolicys.ForEach(x => bids.Add(x.bid.ToString()));
                        if (masters[i].mastertype == 0)
                        {
                            op.mastertype = 0;
                        }
                        else
                        {
                            op.mastertype = 1;
                        }
                        if (bpolicys != null && sum_Price != 0)
                        {
                            var listbpolicyproduct = Pmanage.SelectIn<cfg_bpolicyproduct>("bid", bids);
                            List<cfg_bpolicyproduct> usebpolicyproducts = new List<cfg_bpolicyproduct>();
                            foreach (var item1 in listbpolicyproduct)
                            {
                                if (item1.categorykey == pinfo.p_categorykey || item1.categorykey == 0)
                                {
                                    if (item1.versionid == pinfo.p_versionid || item1.versionid == 0)
                                    {
                                        usebpolicyproducts.Add(item1);
                                    }
                                }
                            }
                            if (usebpolicyproducts.Count == 1)
                            {
                                decimal reality_Amount = pinfo.p_price - dif_Amount * (pinfo.p_price / sum_Price);
                                decimal p_bonus;
                                if (hasClassDivided)
                                {
                                    p_bonus = reality_Amount * (usebpolicyproducts[0].divided + usebpolicyproducts[0].class_divided);
                                    op.p_class_bonus = reality_Amount * usebpolicyproducts[0].class_divided;
                                    op.class_divided = usebpolicyproducts[0].class_divided;
                                }
                                else
                                {
                                    p_bonus = reality_Amount * usebpolicyproducts[0].divided;
                                }

                                if (i == 0)
                                {
                                    order.o_bonus += p_bonus;
                                }
                                op.p_bonus = reality_Amount * usebpolicyproducts[0].divided;
                                op.divided = usebpolicyproducts[0].divided;
                                op.agentid = masters[i].agentid;
                                op.guid = Guid.NewGuid();
                                op.op_guid = pinfo.guid;
                                op.p_price = pinfo.p_price;
                                order_psetbonuss.Add(op);
                            }
                            else
                            {
                                op.p_class_bonus = 0;
                                op.class_divided = 0;
                                op.p_bonus = 0;
                                op.divided = 0;
                                op.agentid = masters[i].agentid;
                                op.guid = Guid.NewGuid();
                                op.op_guid = pinfo.guid;
                                op.p_price = pinfo.p_price;
                                order_psetbonuss.Add(op);
                            }
                        }
                        else
                        {
                            op.p_class_bonus = 0;
                            op.class_divided = 0;
                            op.p_bonus = 0;
                            op.divided = 0;
                            op.agentid = masters[i].agentid;
                            op.guid = Guid.NewGuid();
                            op.op_guid = pinfo.guid;
                            op.p_price = pinfo.p_price;
                            order_psetbonuss.Add(op);
                        }
                    }
                }
            }
            else
            {
                //未添加产品
            }
            return order_psetbonuss;
        }

        private List<order_psetbonus> GetDeptBonus(orderinfo order, List<order_pinfo> oplist)
        {
            /* 注意多产品的算法：
             *             总价=产品1单价+产品2单价
             *             总差价=总价-实际到账
             *             产品1比例= 产品1单价/总价
             *             产品1提成=（产品1单价-总差价*产品1比例）*产品1提成比例
             */
            order.o_bonus = 0;
            List<order_psetbonus> order_psetbonuss = new List<order_psetbonus>();
            if (oplist != null && oplist.Count > 0)
            {
                decimal sum_Price = 0;
                oplist.ForEach(x => sum_Price += x.p_price);
                var dif_Amount = sum_Price - order.o_actamount;

                List<com_master> masters = new List<com_master>();
                List<com_master> com_masters = Pmanage.SelectSearch<com_master>("mastertype=1 and agentid='" + order.agentid + "'");
                if (com_masters != null && com_masters.Count > 0)
                {
                    var item = com_masters.First();
                    if (item.agentid != "KSWF")
                    {
                        GetParentAgencys(item.pagentid, masters);
                    }
                }

                foreach (var pinfo in oplist)
                {
                    order_psetbonus opFirst = new order_psetbonus();//第一个为部门订单
                    opFirst.p_class_bonus = 0;
                    opFirst.class_divided = 0;
                    opFirst.p_bonus = 0;
                    opFirst.divided = 0;
                    opFirst.agentid = order.agentid;
                    opFirst.guid = Guid.NewGuid();
                    opFirst.op_guid = pinfo.guid;
                    opFirst.p_price = pinfo.p_price;
                    order_psetbonuss.Add(opFirst);

                    for (int i = 0; i < masters.Count; i++)
                    {
                        order_psetbonus op = new order_psetbonus();
                        var bpolicys = Pmanage.SelectSearch<join_masterbpolicypr, cfg_bpolicy>((T1, T2) => T1.mastername == masters[i].mastername
                                  && T2.pid == order.channel, (T1, T2) => T1.bid == T2.bid);
                        List<string> bids = new List<string>();
                        bpolicys.ForEach(x => bids.Add(x.bid.ToString()));
                        if (bpolicys != null && sum_Price != 0)
                        {
                            var listbpolicyproduct = Pmanage.SelectIn<cfg_bpolicyproduct>("bid", bids);
                            List<cfg_bpolicyproduct> usebpolicyproducts = new List<cfg_bpolicyproduct>();
                            foreach (var item1 in listbpolicyproduct)
                            {
                                if (item1.categorykey == pinfo.p_categorykey || item1.categorykey == 0)
                                {
                                    if (item1.versionid == pinfo.p_versionid || item1.versionid == 0)
                                    {
                                        usebpolicyproducts.Add(item1);
                                    }
                                }
                            }
                            if (usebpolicyproducts.Count == 1)
                            {
                                decimal reality_Amount = pinfo.p_price - dif_Amount * (pinfo.p_price / sum_Price);
                                decimal p_bonus;
                                if (usebpolicyproducts[0].class_divided > 0)
                                {
                                    p_bonus = reality_Amount * (usebpolicyproducts[0].divided + usebpolicyproducts[0].class_divided);
                                    op.p_class_bonus = reality_Amount * usebpolicyproducts[0].class_divided;
                                    op.class_divided = usebpolicyproducts[0].class_divided;
                                }
                                else
                                {
                                    p_bonus = reality_Amount * usebpolicyproducts[0].divided;
                                }

                                if (i == 0)
                                {
                                    order.o_bonus += p_bonus;
                                    op.p_bonus = reality_Amount * usebpolicyproducts[0].divided;
                                    op.divided = usebpolicyproducts[0].divided;
                                    op.agentid = masters[i].agentid;
                                    op.guid = Guid.NewGuid();
                                    op.op_guid = pinfo.guid;
                                    op.p_price = pinfo.p_price;
                                    order_psetbonuss.Add(op);
                                }
                                else
                                {
                                    op.p_bonus = reality_Amount * usebpolicyproducts[0].divided;
                                    op.divided = usebpolicyproducts[0].divided;
                                    op.agentid = masters[i].agentid;
                                    op.guid = Guid.NewGuid();
                                    op.op_guid = pinfo.guid;
                                    op.p_price = pinfo.p_price;
                                    order_psetbonuss.Add(op);
                                }
                            }
                            else
                            {
                                op.p_class_bonus = 0;
                                op.class_divided = 0;
                                op.p_bonus = 0;
                                op.divided = 0;
                                op.agentid = masters[i].agentid;
                                op.guid = Guid.NewGuid();
                                op.op_guid = pinfo.guid;
                                op.p_price = pinfo.p_price;
                                order_psetbonuss.Add(op);
                            }
                        }
                        else
                        {
                            op.p_class_bonus = 0;
                            op.class_divided = 0;
                            op.p_bonus = 0;
                            op.divided = 0;
                            op.agentid = masters[i].agentid;
                            op.guid = Guid.NewGuid();
                            op.op_guid = pinfo.guid;
                            op.p_price = pinfo.p_price;
                            order_psetbonuss.Add(op);
                        }
                    }
                }
            }
            else
            {
                //未添加产品
            }
            return order_psetbonuss;
        }

        public void GetParentAgencys(string agentid, List<com_master> masters)
        {
            List<com_master> com_masters = Pmanage.SelectSearch<com_master>("mastertype=1 and agentid='" + agentid + "'");
            if (com_masters != null && com_masters.Count > 0)
            {
                var item = com_masters.First();
                if (item.agentid != "KSWF")
                {
                    masters.Add(item);
                    GetParentAgencys(item.pagentid, masters);
                }
            }
        }

        /// <summary>
        /// 计算订单手续费
        /// </summary>
        /// <param name="order"></param>
        /// <param name="message"></param>
        public bool GetOrderFeeAmount(ref orderinfo order)
        {
            cfg_feeratio fee = Pmanage.GetFeeRation(order.channel.Value, order.o_feetype.Value);
            if (fee == null)
            {
                return false;
            }
            else
            {
                order.o_feeamount = order.o_payamount * fee.divided;
                order.o_actamount = order.o_payamount - order.o_feeamount;
                return true;
            }
        }


        /// <summary>
        /// 检查订单是否完成
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        private bool CheckOrderService(string orderid)
        {
            KSWF.WFM.BLL.PayService.FZPayService payservice = new WFM.BLL.PayService.FZPayService();
            string state = payservice.GetOrderState(orderid, "");
            if (state == "0001")//返回的订单状态0001代表订单支付完成了
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    public class ReturnInfo
    {
        public bool Success { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class Order
    {
        public string OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? AreaID { get; set; }
        public string AreaPath { get; set; }
        public int? SchoolID { get; set; }
        public string SchoolName { get; set; }
        public int? GradeID { get; set; }
        public string GradeName { get; set; }
        public Guid? ClassID { get; set; }
        public string ClassName { get; set; }
        public string ProductNO { get; set; }
        public string ProductName { get; set; }
        public int? TeacherUserID { get; set; }
        public string TeacherUserName { get; set; }
        public string BuyUserID { get; set; }
        public string BuyUserPhone { get; set; }
        public decimal PayAmount { get; set; }
        public int? PayType { get; set; }
        public int? Channel { get; set; }
        public string UserClientIP { get; set; }

    }

}
