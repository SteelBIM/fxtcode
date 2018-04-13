using System;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Http;
using System.Xml.Serialization;
using Kingsun.AppLibrary.Model;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.App.Filter;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Common.Model;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    /// <summary>
    /// 活动相关接口
    /// </summary>
    public class ActivityController : ApiController
    {

        BaseManagement bm = new BaseManagement();
        ///// <summary>
        ///// 获取活动
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[ShowApi]
        //public ApiResponse GetActivity(string AppID, string SyetemID)
        //{
        //    return GetResult(new
        //    {

        //    });
        //}


        /// <summary>
        /// 获取优惠卷
        /// </summary>
        /// <param name="UserID">用户ID(测试先随便填)</param>
        /// <param name="CourseID">书本ID</param>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse GetCoupon(string UserID)
        {

            var couList = bm.Search<TB_Coupon>("UserID='" + UserID + "'");
            if (couList != null && couList.Count > 0)
            {

                return GetResult(new Temp
                {
                    CouponKey = couList[0].ID.ToString(),
                    CouponStatus = couList[0].Status.Value,
                    CouponImage = couList[0].CouponImage
                });
            }
            else
            {
                Guid id = Guid.NewGuid();
                var img = "http://" + Request.RequestUri.Authority + "/Images/Coupon.png";
                if (bm.Insert<TB_Coupon>(new TB_Coupon
                {
                    ID = id,
                    UserID = UserID,
                    Status = 1,
                    CouponImage = img
                }))
                {
                    return GetResult(new Temp
                    {
                        CouponKey = id.ToString(),
                        CouponStatus = 1,
                        CouponImage = img
                    });
                }
                else
                {
                    return GetErrorResult("获取失败_" + bm._operatorError);

                }
            }




        }




        /// <summary>
        /// 查询用户优惠卷状态(0:未拥有优惠卷  1:有优惠卷未使用 2:已经使用优惠卷)
        /// </summary>
        /// <param name="UserID">用户ID(测试先随便填)</param>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse GetCouponStatus(string UserID)
        {
            var couList = bm.Search<TB_Coupon>("UserID='" + UserID + "'");
            int status = 0;
            string key = "";
            string img = "";
            if (couList != null && couList.Count > 0)
            {
                status = couList[0].Status.Value;
                key = couList[0].ID.ToString();
                img = couList[0].CouponImage;
            }
            return GetResult(new Temp
            {
                CouponKey = key,
                CouponStatus = status,
                CouponImage = img
            });
        }


        /// <summary>
        /// 使用优惠劵
        /// </summary>
        /// <param name="UserID">用户ID(测试先随便填)</param>
        /// <param name="CouponKey">优惠卷码(测试先随便填)</param>
        /// <param name="CourseID">书本ID(测试先随便填)</param>
        /// <returns>返回用户的权限信息</returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse UseCoupon(string UserID, string CouponKey, string CourseID)
        {

            var couList = bm.Search<TB_Coupon>("UserID='" + UserID + "' and ID='" + CouponKey + "' and Status=1");

            if (couList == null || couList.Count == 0)
            {
                return GetErrorResult("未找到此用户的优惠卷");
            }

            var vip = bm.Search<TB_UserMember>("UserID='" + UserID + "' and CourseID='" + CourseID + "'", " EndDate desc");

            bool isopen = false;
            if (vip == null || vip.Count == 0)
            {
                if (bm.Insert<TB_UserMember>(new TB_UserMember
                {
                    ID = Guid.NewGuid(),
                    CourseID = CourseID,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(12),
                    UserID = UserID,
                    Months = 12,
                    TbOrderID = Guid.NewGuid()
                }))
                {
                    isopen = true;
                }
                else
                {
                    isopen = false;
                }

            }
            else
            {
                if (vip[0].EndDate.Value < DateTime.Now)
                {
                    if (bm.Insert<TB_UserMember>(new TB_UserMember
                    {
                        ID = Guid.NewGuid(),
                        CourseID = CourseID,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(12),
                        UserID = UserID,
                        Months = 12,
                        TbOrderID = Guid.NewGuid()
                    }))
                    {
                        isopen = true;
                    }
                    else
                    {
                        isopen = false;
                    }
                }
                else
                {
                    if (bm.Insert<TB_UserMember>(new TB_UserMember
                    {
                        ID = Guid.NewGuid(),
                        CourseID = CourseID,
                        StartDate = vip[0].EndDate.Value.AddDays(1),
                        EndDate = vip[0].EndDate.Value.AddDays(1).AddMonths(12),
                        UserID = UserID,
                        Months = 12,
                        TbOrderID = Guid.NewGuid()
                    }))
                    {
                        isopen = true;
                    }
                    else
                    {
                        isopen = false;
                    }
                }
            }



            AccountController acc = new AccountController();
            if (isopen)
            {
                couList[0].Status = 2;
                couList[0].CourseID = CourseID;
                if (bm.Update<TB_Coupon>(couList[0]))
                {
                    return GetResult(new
                    {
                        UserCombo = acc.QueryCombo(UserID)
                    }, "使用成功");
                }
                else
                {
                    return GetErrorResult(bm._operatorError);
                }
            }
            else
            {
                return GetErrorResult(bm._operatorError);
            }


        }



        private ApiResponse GetErrorResult(string message)
        {
            return new ApiResponse
            {
                Success = false,
                data = null,
                Message = message
            };
        }

        private ApiResponse GetResult(object Data, string message = "")
        {

            return new ApiResponse
            {
                Success = true,
                data = Data,
                Message = message
            };
        }
    }

    ///
    [KnownType(typeof(Temp))]

    public class Temp
    {
        /// <summary>
        /// 
        /// </summary>
        public string CouponKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CouponImage { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public int CouponStatus { get; set; }

    }
}