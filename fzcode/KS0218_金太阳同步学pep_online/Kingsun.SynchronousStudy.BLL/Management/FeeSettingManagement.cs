using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Kingsun.AppLibrary.Model;
using Kingsun.DB;
using Kingsun.PSO.KSWFWebService;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using Kingsun.IBS.Model;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;

namespace Kingsun.SynchronousStudy.BLL
{
    public class FeeSettingManagement : BaseManagement
    {
        readonly string _kswfUserName = WebConfigurationManager.AppSettings["kswfUserName"];
        readonly string _kswfPassWord = WebConfigurationManager.AppSettings["kswfPassWord"];

        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        /// <summary>
        /// 获取收费套餐
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryFeeComboList(KingRequest request)
        {
            PageParameter param = JsonHelper.DecodeJson<PageParameter>(request.Data);
            #region 验证相关数据有效性
            if (param == null)
            {
                return KingResponse.GetErrorResponse("当前信息为空");
            }
            if (param.PageIndex < 1)
            {
                return KingResponse.GetErrorResponse("当前页面数不正确");
            }
            if (param.PageSize < 1)
            {
                return KingResponse.GetErrorResponse("当前页pageSize不正确");
            }
            #endregion
            param.OrderColumns = "ModifyDate";
            param.TbNames = "TB_FeeCombo";
            param.IsOrderByASC = 2;
            param.Columns = "*";
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = param.getParameterList();
            System.Data.DataSet ds = ExecuteProcedure("proc_pageView", list);
            if (ds == null || ds.Tables.Count < 1)
            {

                return KingResponse.GetErrorResponse("执行存储过程失败，" + _operatorError);
            }
            object obj = new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = FillData<TB_FeeCombo>(ds.Tables[0])
            };
            return KingResponse.GetResponse(request, obj);
        }

        public KingResponse QueryAppID(KingRequest request)
        {
            var appList = Search<TB_APPManagement>("");
            if (appList == null)
            {
                return KingResponse.GetErrorResponse("没有应用信息");
            }
            return KingResponse.GetResponse(request, appList);
        }


        public KingResponse QueryED(KingRequest request)
        {
            var appList = Search<TB_CurriculumManage>("");
            if (appList == null)
            {
                return KingResponse.GetErrorResponse("没有应用信息");
            }
            return KingResponse.GetResponse(request, appList);
        }

        public KingResponse AddCombo(KingRequest request)
        {

            TB_FeeCombo fee = JsonHelper.DecodeJson<TB_FeeCombo>(request.Data);
            fee.ID = Guid.NewGuid();
            fee.CreateDate = DateTime.Now;
            fee.ModifyDate = DateTime.Now;

            var uc = HttpContext.Current.Request.Cookies["FZKSTBX"];
            if (uc == null)
            {
                fee.CreateUser = "";
                fee.ModifyUser = "";
                //return KingResponse.GetErrorResponse("没有登陆信息", request);
            }
            else
            {
                string cookie = uc.Value;
                string[] arrCookie = cookie.Split('&');
                for (var i = 0; i < arrCookie.Length; i++)
                {
                    string[] arr = arrCookie[i].Split('=');
                    if (arr[0] == "UserName")
                    {
                        fee.CreateUser = arr[1];
                        fee.ModifyUser = arr[1];
                        break;
                    }
                }
            }

            if (Insert<TB_FeeCombo>(fee))
            {
                IList<TB_APPManagement> am = Search<TB_APPManagement>("ID='" + fee.AppID + "'");
                KSWFWebService kswf = new KSWFWebService();
                Product pd = new Product();
                if (am != null)
                {
                    foreach (var item in am)
                    {
                        pd.VersionID = item.VersionID;
                        pd.Version = item.VersionName;
                    }
                }
                pd.Channel = 1;
                pd.ProductNo = "TBX_" + fee.ID;
                pd.ProductName = fee.FeeName;
                pd.Subject = "英语";
                pd.SubjectID = 3;
                pd.Category = "E-BOOK";
                pd.CategoryKey = 101;
                pd.Isshevel = fee.State ?? 0;
                if (fee.FeePrice != null) pd.Price = fee.FeePrice.Value;
                CertficateSoapHeader header = new CertficateSoapHeader();
                header.UserName = _kswfUserName;
                header.PassWord = _kswfPassWord;
                kswf.CertficateSoapHeaderValue = header;
                kswf.AddProduct(pd);
                return KingResponse.GetResponse(request, "添加成功");
            }

            return KingResponse.GetErrorResponse("添加失败" + _operatorError, request);
        }



        public KingResponse ModifyFeeCombo(KingRequest request)
        {
            TB_FeeCombo fee = JsonHelper.DecodeJson<TB_FeeCombo>(request.Data);
            var fc = Select<TB_FeeCombo>(fee.ID);
            if (fc == null)
            {
                return KingResponse.GetErrorResponse("找不到此套餐信息");
            }
            var uc = HttpContext.Current.Request.Cookies["FZKSTBX"];
            if (uc == null)
            {
                fee.CreateUser = "";
                fee.ModifyUser = "";
                //return KingResponse.GetErrorResponse("没有登陆信息", request);
            }
            else
            {
                string cookie = uc.Value;
                string[] arrCookie = cookie.Split('&');
                for (var i = 0; i < arrCookie.Length; i++)
                {
                    string[] arr = arrCookie[i].Split('=');
                    if (arr[0] == "UserName")
                    {
                        fee.ModifyUser = arr[1];
                        break;
                    }
                }
            }

            fc.AppID = fee.AppID;
            fc.AppleID = fee.AppleID;
            fc.FeeName = fee.FeeName;
            fc.FeePrice = fee.FeePrice;
            fc.Discount = fee.Discount;
            fc.Month = fee.Month;
            fc.Type = fee.Type;
            fc.ComboType = fee.ComboType;
            fc.ModifyUser = fee.ModifyUser;
            fc.ModifyDate = DateTime.Now;
            fc.ImageUrl = fee.ImageUrl;
            if (Update<TB_FeeCombo>(fc))
            {
                IList<TB_APPManagement> am = Search<TB_APPManagement>("ID='" + fee.AppID + "'");
                KSWFWebService kswf = new KSWFWebService();
                Product pd = new Product();
                if (am != null)
                {
                    foreach (var item in am)
                    {
                        pd.VersionID = item.VersionID;
                        pd.Version = item.VersionName;
                    }
                }
                pd.Channel = 1;
                pd.ProductNo = "TBX_" + fee.ID;
                pd.ProductName = fee.FeeName;
                pd.Subject = "英语";
                pd.SubjectID = 3;
                pd.Category = "E-BOOK";
                pd.CategoryKey = 101;
                if (fc.FeePrice != null) pd.Price = fc.FeePrice.Value;
                pd.Isshevel = fc.State ?? 0;
                CertficateSoapHeader header = new CertficateSoapHeader();
                header.UserName = _kswfUserName;
                header.PassWord = _kswfPassWord;
                kswf.CertficateSoapHeaderValue = header;
                ReturnInfo ri = kswf.AddProduct(pd);
                if (ri.Success)
                {

                }
                return KingResponse.GetResponse(request, "更新成功");
            }
            return KingResponse.GetErrorResponse("更新失败" + _operatorError, request);
        }


        public KingResponse QueryFee(KingRequest request)
        {
            TB_FeeCombo p = JsonHelper.DecodeJson<TB_FeeCombo>(request.Data);
            var fee = Select<TB_FeeCombo>(p.ID);
            return KingResponse.GetResponse(request, fee);

        }

        public KingResponse JFeeCombo(KingRequest request)
        {
            TB_FeeCombo p = JsonHelper.DecodeJson<TB_FeeCombo>(request.Data);
            var fee = Select<TB_FeeCombo>(p.ID);
            fee.State = fee.State == 0 ? 1 : 0;
            if (Update<TB_FeeCombo>(fee))
            {
                IList<TB_APPManagement> am = Search<TB_APPManagement>("ID='" + fee.AppID + "'");
                KSWFWebService kswf = new KSWFWebService();
                Product pd = new Product();
                if (am != null)
                {
                    foreach (var item in am)
                    {
                        pd.VersionID = item.VersionID;
                        pd.Version = item.VersionName;
                    }
                }
                pd.Channel = 1;
                pd.ProductNo = "TBX_" + fee.ID;
                pd.ProductName = fee.FeeName;
                pd.Subject = "英语";
                pd.SubjectID = 3;
                pd.Category = "E-BOOK";
                pd.CategoryKey = 101;
                pd.Isshevel = fee.State ?? 0;
                if (fee.FeePrice != null) pd.Price = fee.FeePrice.Value;
                CertficateSoapHeader header = new CertficateSoapHeader();
                header.UserName = _kswfUserName;
                header.PassWord = _kswfPassWord;
                kswf.CertficateSoapHeaderValue = header;
                ReturnInfo ri = kswf.AddProduct(pd);
                return KingResponse.GetResponse(request, fee.State);
            }
            return KingResponse.GetErrorResponse("更新失败" + _operatorError, request);


        }

        public KingResponse KVip(KingRequest request)
        {
            TB_UserMember u = JsonHelper.DecodeJson<TB_UserMember>(request.Data);

            var userList = Search<Tb_UserInfo>("TelePhone='" + u.UserID + "'  AND IsUser=1");
            if (userList == null || userList.Count == 0)
            {
                userList = new List<Tb_UserInfo>();
                var user = userBLL.GetUserInfoByUserOtherID(u.UserID.ToString(),1);

                if (user == null)
                {
                    var rinfo=userBLL.AppRegister2(System.Configuration.ConfigurationManager.AppSettings["AppID"], u.UserID.ToString(), "123456", (int)UserTypeEnum.Student);

                    if (!rinfo.Success)
                    {
                        return KingResponse.GetErrorResponse("开通失败", request);
                    }

                    userList.Add(new Tb_UserInfo
                    {
                        UserID = string.IsNullOrEmpty(rinfo.ToString()) ? 0 : Convert.ToInt32(rinfo.ToString())
                    });
                    Insert<Tb_UserInfo>(new Tb_UserInfo
                    {
                        UserID = string.IsNullOrEmpty(rinfo.ToString()) ? 0 : Convert.ToInt32(rinfo.ToString()),
                        TelePhone = u.UserID.ToString(),
                        UserName = "TBX" + u.UserID
                    });
                }
                else
                {
                    userList.Add(new Tb_UserInfo
                    {
                        UserID =Convert.ToInt32(user.UserID)
                    });

                }


            }



            var vip = Search<TB_UserMember>("UserID='" + userList[0].UserID + "' and CourseID='" + u.CourseID + "'", " EndDate desc");


            if (vip == null || vip.Count == 0)
            {


                if (Insert<TB_UserMember>(new TB_UserMember
                {
                    ID = Guid.NewGuid(),
                    CourseID = u.CourseID,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(u.Months.Value),
                    UserID = userList[0].UserID.ToString(),
                    Months = u.Months,
                    TbOrderID = Guid.NewGuid()
                }))
                {
                    return KingResponse.GetResponse(request, "开通成功");
                }
                else
                {
                    return KingResponse.GetErrorResponse(_operatorError);
                }

            }

            if (vip[0].EndDate.Value < DateTime.Now)
            {
                if (Insert<TB_UserMember>(new TB_UserMember
                {
                    ID = Guid.NewGuid(),
                    CourseID = u.CourseID,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(u.Months.Value),
                    UserID = userList[0].UserID.ToString(),
                    Months = u.Months,
                    TbOrderID = Guid.NewGuid()
                }))
                {

                    return KingResponse.GetResponse(request, "开通成功");
                }
                else
                {
                    return KingResponse.GetErrorResponse(_operatorError);
                }
            }
            else
            {
                if (Insert<TB_UserMember>(new TB_UserMember
                {
                    ID = Guid.NewGuid(),
                    CourseID = u.CourseID,
                    StartDate = vip[0].EndDate.Value.AddDays(1),
                    EndDate = vip[0].EndDate.Value.AddDays(1).AddMonths(u.Months.Value),
                    UserID = userList[0].UserID.ToString(),
                    Months = u.Months,
                    TbOrderID = Guid.NewGuid()
                }))
                {
                    return KingResponse.GetResponse(request, "开通成功");
                }
                else
                {
                    return KingResponse.GetErrorResponse(_operatorError);
                }
            }
        }



        /// <summary>
        /// 分页查询用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public KingResponse QueryUserInfo(KingRequest request)
        {

            PageParameter parameter = JsonHelper.DecodeJson<PageParameter>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponse("找不到当前的参数", request);
            }
            if (parameter.PageIndex < 1)
            {
                return KingResponse.GetErrorResponse("当前页面数不正确", request);
            }
            if (parameter.PageSize < 1)
            {
                return KingResponse.GetErrorResponse("当前页pageSize不正确", request);
            }
            parameter.OrderColumns = "CreateTime";
            parameter.TbNames = "ITSV_Base.[FZ_SynchronousStudy].dbo.[Tb_UserInfo]";
            parameter.IsOrderByASC = 2;
            parameter.Columns = "*";

            try
            {
                string[] str = parameter.Where.Split(',');
                if (str.Length > 2)
                {
                    parameter.Where = str[0] + str[1] + str[2];
                }
                else if (str.Length > 1)
                {
                    parameter.Where = str[0] + str[1];
                    // " UserName like '%" + str[1] + "%' or TelePhone like '%" + str[1] + "%'";
                }
                else
                {
                    parameter.Where = str[0];
                }
            }
            catch
            {
                return KingResponse.GetErrorResponse("查询参数不正确！", request);
            }

            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = parameter.getParameterList();
            System.Data.DataSet ds = ExecuteProcedure("proc_pageView", list);

            return KingResponse.GetResponse(request, new
            {
                total = ds.Tables[1].Rows[0][0],
                rows = ds.Tables[0].ToList()
            });
        }
    }
}
