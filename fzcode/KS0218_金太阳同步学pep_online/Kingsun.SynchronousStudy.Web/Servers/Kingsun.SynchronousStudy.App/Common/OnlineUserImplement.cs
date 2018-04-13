using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;

namespace Kingsun.SynchronousStudy.App.Common
{
    public class OnlineUserImplement
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public double OverTime = 1;
        private string cacheKey = "cache_onlineuser";
        public string _ErrorMsg = "";

        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();

        /// <summary>
        /// 将新的在线用户添加到缓存中
        /// </summary>
        /// <param name="user"></param>
        public void AddOnlineUser(OnlineUser user)
        {
            Hashtable OnlineUserList = (Hashtable)CacheHelper.GetCache(cacheKey);
            AddOnlineUser(user, OnlineUserList);
        }

        /// <summary>
        /// 将新的在线用户添加到缓存中
        /// </summary>
        /// <param name="user"></param>
        /// <param name="list"></param>
        public void AddOnlineUser(OnlineUser user, Hashtable list)
        {
            if (list == null)
            {
                //缓存不存在，新建缓存
                NewOnlineCache(user);
                return;
            }
            list.Add(user.UserID, user);
            CacheHelper.ReplaceValue(cacheKey, list);
        }

        /// <summary>
        /// 获取在线用户，判断,更新时间
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public OnlineUser GetOnlineUser(string userid)
        {
            Hashtable OnlineUserList = (Hashtable)CacheHelper.GetCache(cacheKey);
            OnlineUser user = new OnlineUser();
            if (OnlineUserList == null)//缓存为空
            {
                user = GetOnlineuserByService(userid);
                if (user != null)
                {
                    NewOnlineCache(user);
                    return user;
                }
                else
                {
                    _ErrorMsg = "用户已经下线";
                    return null;
                }
            }
            else
            {
                user = (OnlineUser)OnlineUserList[userid];
                if (user == null)
                {
                    user = GetOnlineuserByService(userid);
                    if (user != null)
                    {
                        AddOnlineUser(user);
                        return user;
                    }
                    else
                    {
                        _ErrorMsg = "用户已经下线";
                        return null;
                    }
                }
                else
                {
                    if (DateTime.Now < user.EndDate && (user.EndDate - DateTime.Now).Minutes > 2)
                    {
                        ////更新用户过期时间
                        //user.StartDate = DateTime.Now;
                        //user.EndDate = DateTime.Now.AddMinutes(OverTime);
                        //OnlineUserList.Remove(usernum);
                        //OnlineUserList.Add(usernum, user);
                        //CacheHelper.ReplaceValue(cacheKey, OnlineUserList);
                        return user;
                    }
                    else
                    {
                        OnlineUserList.Remove(userid);
                        user = GetOnlineuserByService(userid);
                        if (user != null)
                        {
                            AddOnlineUser(user);
                            return user;
                        }
                        else
                        {
                            _ErrorMsg = "用户已经下线";
                            return null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 移除所有的在线用户缓存
        /// </summary>
        public void ClearALLOnlineUser()
        {
            CacheHelper.Remove(cacheKey);
        }

        public void RemoveOnlineUser(string userid)
        {
            Hashtable OnlineUserList = (Hashtable)CacheHelper.GetCache(cacheKey);
            if (OnlineUserList != null)
            {
                OnlineUserList.Remove(userid);
            }
        }

        public IList<OnlineUser> GetOnlineUserList()
        {
            Hashtable OnlineUserList = (Hashtable)CacheHelper.GetCache(cacheKey);
            if (OnlineUserList != null)
            {
                List<OnlineUser> list = new List<OnlineUser>();
                //object obj = OnlineUserList.Values;
                foreach (DictionaryEntry obj in OnlineUserList)
                {
                    list.Add((OnlineUser)obj.Value);
                }
                return list;
            }
            return null;
        }


        /// <summary>
        /// 缓存不存在，新建缓存
        /// </summary>
        /// <param name="user"></param>
        public void NewOnlineCache(OnlineUser user)
        {
            Hashtable OnlineUserList = new Hashtable();
            OnlineUserList.Add(user.UserID, user);
            CacheHelper.Insert(cacheKey, OnlineUserList);
        }

        /// <summary>
        /// 获取UUMS服务器在线登录信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public OnlineUser GetOnlineuserByService(string userid)
        {
            OnlineUser user = new OnlineUser();
            var rinfo = userBLL.AppCheckUserState(ProjectConstant.AppID, userid);
            if (rinfo.Success)
            {
                string[] strs = rinfo.Data.ToString().Split('|');
                if (strs == null || strs.Length == 0)
                {
                    return null;
                }
                user.UserID = strs[0];
                user.UserNum = strs[1];
                user.MachineCode = strs[2];
                user.MachineModel = strs[3];
                user.StartDate = DateTime.Now;
                user.EndDate = DateTime.Now.AddMinutes(OverTime);
                return user;
            }
            else
            {
                _ErrorMsg = "";
                return null;
            }
        }

    }
}