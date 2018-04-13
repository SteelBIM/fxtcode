using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSWF.Framework.BLL;
using KSWF.WFM.Constract.Models;
using KSWF.Core.Utility;
using System.Linq.Expressions;

namespace KSWF.WFM.BLL
{
    public class ProductManage : Manage
    {
       

        /// <summary>
        /// 获取来源下面的分类
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public List<cfg_keyvalue> GetCatogoryByChannel(int channel)
        {
            return SelectSearch<cfg_keyvalue>(i => i.UseType == "Category");//&& i.ParentKey == channel.ToString()
        }

        public KingResponse CheckProductInfo(cfg_product productInfo)
        {
            KingResponse rinfo = new KingResponse();
            rinfo.Success = true;
            if (productInfo == null)
            {
                rinfo.Success = false;
                rinfo.ErrorMsg = "商品信息为空";
                return rinfo;
            }
            List<cfg_keyvalue> ChannelList = KeyValueManage.GetChannleData();
            List<cfg_keyvalue> CatogoryList = KeyValueManage.GetCatogoryData();
            if (!CheckChannel(productInfo.channel, ChannelList))
            {
                rinfo.Success = false;
                rinfo.ErrorMsg = "商品来源信息错误";
                return rinfo;
            }
            if (!CheckCatogory(productInfo.categorykey.ToString(), productInfo.channel, CatogoryList))
            {
                rinfo.Success = false;
                rinfo.ErrorMsg = "商品分类信息错误";
                return rinfo;
            }
            //if (string.IsNullOrEmpty(productInfo.grade) || productInfo.gradeid == 0)
            //{
            //    rinfo.Success = false;
            //    rinfo.ErrorMsg = "商品年级信息错误";
            //    return rinfo;
            //}
            //if (string.IsNullOrEmpty(productInfo.subject) || productInfo.subjectid == 0)
            //{
            //    rinfo.Success = false;
            //    rinfo.ErrorMsg = "商品学科信息错误";
            //    return rinfo;
            //}
            if (string.IsNullOrEmpty(productInfo.version) || productInfo.versionid == 0)
            {
                rinfo.Success = false;
                rinfo.ErrorMsg = "商品版本信息错误";
                return rinfo;
            }
            if (string.IsNullOrEmpty(productInfo.productname))
            {
                rinfo.Success = false;
                rinfo.ErrorMsg = "商品名称信息错误";
                return rinfo;
            }
            if (string.IsNullOrEmpty(productInfo.productno))
            {
                rinfo.Success = false;
                rinfo.ErrorMsg = "商品编号信息错误";
                return rinfo;
            }
            List<cfg_product> list = SelectSearch<cfg_product>(i => i.productname == productInfo.productname || i.productno == productInfo.productno);
            if (list != null && list.Count > 0)
            {
                foreach (cfg_product p in list)
                {
                    if (productInfo.id != p.id)
                    {
                        if (p.productno == productInfo.productno)
                        {
                            rinfo.Success = false;
                            rinfo.ErrorMsg = "业务系统商品已经存在";
                            return rinfo;
                        }
                        //if (p.productname == productInfo.productname)
                        //{
                        //    rinfo.Success = false;
                        //    rinfo.ErrorMsg = "产品名称已经存在";
                        //    return rinfo;
                        //}
                       
                    }
                }
            }
            return rinfo;
        }

        /// <summary>
        /// 检测来源是否正确
        /// </summary>
        /// <param name="list"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        private bool CheckChannel(int channel, IList<cfg_keyvalue> list = null)
        {
            if (list == null)
            {
                list = KeyValueManage.GetChannleData();
            }
            foreach (cfg_keyvalue kv in list)
            {
                if (kv.Key == channel.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检测类别是否正确
        /// </summary>
        /// <param name="list"></param>
        /// <param name="catogory"></param>
        /// <returns></returns>
        private bool CheckCatogory(string catogory, List<cfg_keyvalue> list = null)
        {
            if (list == null)
            {
                list = KeyValueManage.GetCatogoryData();
            }
            foreach (cfg_keyvalue kv in list)
            {
                if (kv.Key == catogory)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检测来源下面的分类是否正确
        /// </summary>
        /// <param name="catogory"></param>
        /// <param name="channel"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool CheckCatogory(string catogory, int channel, List<cfg_keyvalue> list = null)
        {
            if (list == null)
            {
                list = GetCatogoryByChannel(channel);
            }
            foreach (cfg_keyvalue kv in list)
            {
                if (kv.Key == catogory)// && kv.ParentKey == channel.ToString()
                {
                    return true;
                }
            }
            return false;
        }

        public base_dept GetDeptInfoByMasterID(int deptid)
        {
            //List<com_master> masterinfo = base.SelectSearch<com_master>(i => i.mastername == masterName);
            //if (masterinfo != null && masterinfo.Count > 0)
            //{
                base_dept deptinf = base.Select<base_dept>(deptid);
                return deptinf;
            //}
            //return null;
        }

        /// <summary>
        /// 获取支付费率
        /// </summary>
        /// <param name="channle"></param>
        /// <param name="paytype"></param>
        /// <returns></returns>
        public cfg_feeratio GetFeeRation(int channle, int paytype)
        {
            List<cfg_feeratio> list = base.SelectSearch<cfg_feeratio>(i => i.channel == channle && i.feetype == paytype);
            if (list == null || list.Count == 0)
            {
                return null;
            }
            return list[0];
        }

    }
}
