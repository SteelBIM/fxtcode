using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Log4Net;

namespace Kingsun.SynchronousStudy.DAL
{
    public class VideoInfoDAL
    {
        static RedisHashHelper redis = new RedisHashHelper();
        private readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        private readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];


        public void InsertVideoRedis(RedisVideoInfo submitData)
        {
            //RedisVideoInfo submitData = JsonHelper.DecodeJson<RedisVideoInfo>(strJson);

            #region 插入用户配音数据
            bool bl1 = InsertVideoInfoRedis(submitData);
            #endregion

            if (submitData.UserType != "12")
            {
                if (!string.IsNullOrEmpty(submitData.SchoolID))
                {
                    #region 校级榜插入数据
                    bool bl2 = InsertSchoolRank(submitData);
                    #endregion
                }

                if (!string.IsNullOrEmpty(submitData.ClassID))
                {
                    #region 班级榜插入数据
                    bool bl3 = InsertClassRank(submitData);
                    #endregion
                }
            }
            #region 最新榜数据
            bool bl4 = InsertNewRank(submitData);
            #endregion
        }

        /// <summary>
        /// 插入用户配音数据
        /// </summary>
        /// <param name="UserVideoID"></param>
        /// <param name="submitData"></param>
        /// <param name="tname"></param>
        /// <param name="img"></param>
        /// <param name="lst"></param>
        private bool InsertVideoInfoRedis(RedisVideoInfo redisVideoInfo)
        {
            bool bl1 = false;

            List<string> lst = new List<string>();
            Redis_IntDubb_VideoInfo videoinfo = new Redis_IntDubb_VideoInfo()
            {
                VideoId = redisVideoInfo.VideoID.ToString(),
                UserId = redisVideoInfo.UserId,
                TrueName = redisVideoInfo.TrueName,
                UserImage = redisVideoInfo.UserImage,
                TotalScore = redisVideoInfo.TotalScore,
                NumberOfOraise = lst,
                CreateTime = DateTime.Now.ToString()
            };
            string intVideoInfo = redis.Get("Redis_IntDubb_VideoInfo_" + redisVideoInfo.BookId, redisVideoInfo.VideoID.ToString());
            if (intVideoInfo == null)
            {
                bl1 = redis.Set<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + redisVideoInfo.BookId, redisVideoInfo.VideoID.ToString(), videoinfo);
                LogHelper.Info("插入用户配音数据:VideoID:" + redisVideoInfo.VideoID + ";BookID:" + redisVideoInfo.BookId + "；VideoInfo:" + JsonHelper.EncodeJson(redisVideoInfo) + "是否成功：" + bl1);
            }
            return bl1;
        }

        /// <summary>
        /// 最新榜插入数据
        /// </summary>
        /// <param name="submitData"></param>
        /// <param name="UserVideoID"></param>
        /// <param name="rank"></param>
        private bool InsertNewRank(RedisVideoInfo redisVideoInfo)
        {
            List<Redis_IntDubb_NewRank> listNewRanke = new List<Redis_IntDubb_NewRank>();
            List<Redis_IntDubb_NewRank> idn = redis.Get<List<Redis_IntDubb_NewRank>>("Redis_IntDubb_NewRank_" + redisVideoInfo.BookId, redisVideoInfo.VideoNumber);
            if (idn != null)
            {
                foreach (var item in idn)
                {
                    Redis_IntDubb_NewRank Ranke = new Redis_IntDubb_NewRank
                    {
                        VideoId = item.VideoId.ToString(),
                        Sort = item.Sort
                    };
                    listNewRanke.Add(Ranke);
                }
            }
            Redis_IntDubb_NewRank newRanke = new Redis_IntDubb_NewRank
            {
                VideoId = redisVideoInfo.VideoID.ToString(),
                Sort = 0
            };

            listNewRanke.Add(newRanke);
            listNewRanke = listNewRanke.OrderByDescending(i => i.VideoId).Take(200).ToList();
            bool bl6 = redis.Set<List<Redis_IntDubb_NewRank>>("Redis_IntDubb_NewRank_" + redisVideoInfo.BookId, redisVideoInfo.VideoNumber, listNewRanke);
            LogHelper.Info("插入最新榜数据:BookID:" + redisVideoInfo.BookId + ";VideoNumber:" + redisVideoInfo.VideoNumber + ";ListNewRanke:" + JsonHelper.EncodeJson(redisVideoInfo) + "是否成功：" + bl6);
            return bl6;
        }

        /// <summary>
        /// 班级榜插入数据
        /// </summary>
        /// <param name="submitData"></param>
        /// <param name="rel"></param>
        /// <param name="rank"></param>
        private bool InsertClassRank(RedisVideoInfo redisVideoInfo)
        {
            int c = 0;
            Redis_IntDubb_Rank rank = new Redis_IntDubb_Rank
            {
                UserId = redisVideoInfo.UserId,
                VideoId = redisVideoInfo.VideoID.ToString(),
                TotalScore = Convert.ToDouble(redisVideoInfo.TotalScore)
            };
            List<Redis_IntDubb_Rank> cRank = new List<Redis_IntDubb_Rank>();
            List<Redis_IntDubb_Rank> ClassRin = redis.Get<List<Redis_IntDubb_Rank>>("Redis_IntDubb_ClassRank_" + redisVideoInfo.BookId, redisVideoInfo.ClassID.ToLower() + "_" + redisVideoInfo.VideoNumber);
            if (ClassRin != null)
            {
                foreach (var item in ClassRin)
                {
                    if (item.UserId == redisVideoInfo.UserId)
                    {
                        c = 1;
                        if (Convert.ToDouble(item.TotalScore) <= Convert.ToDouble(redisVideoInfo.TotalScore))
                        {
                            cRank.Add(rank);
                        }
                        else
                        {
                            cRank.Add(new Redis_IntDubb_Rank
                            {
                                UserId = item.UserId,
                                VideoId = item.VideoId,
                                TotalScore = item.TotalScore
                            });
                        }
                    }
                    else
                    {
                        cRank.Add(new Redis_IntDubb_Rank
                        {
                            UserId = item.UserId,
                            VideoId = item.VideoId,
                            TotalScore = item.TotalScore
                        });
                    }
                }
                if (c == 0)
                {
                    cRank.Add(rank);
                }
                cRank = cRank.OrderByDescending(i => i.TotalScore).Take(200).ToList();
                bool bl4 = redis.Set<List<Redis_IntDubb_Rank>>("Redis_IntDubb_ClassRank_" + redisVideoInfo.BookId, redisVideoInfo.ClassID.ToLower() + "_" + redisVideoInfo.VideoNumber, cRank);
                LogHelper.Info("插入班级榜数据：BookId:" + redisVideoInfo.BookId + ";ClassID:" + redisVideoInfo.ClassID.ToLower() + "_" + redisVideoInfo.VideoNumber + "；redisVideoInfo：" + JsonHelper.EncodeJson(redisVideoInfo) + "是否成功：" + bl4);
                return bl4;
            }
            else
            {
                cRank.Add(rank);
                cRank = cRank.OrderByDescending(i => i.TotalScore).Take(200).ToList();
                bool bl5 = redis.Set<List<Redis_IntDubb_Rank>>("Redis_IntDubb_ClassRank_" + redisVideoInfo.BookId, redisVideoInfo.ClassID.ToLower() + "_" + redisVideoInfo.VideoNumber, cRank);
                LogHelper.Info("插入班级榜数据：BookId:" + redisVideoInfo.BookId + ";ClassID:" + redisVideoInfo.ClassID.ToLower() + "_" + redisVideoInfo.VideoNumber + "；redisVideoInfo：" + JsonHelper.EncodeJson(redisVideoInfo) + "是否成功：" + bl5);
                return bl5;
            }
        }

        /// <summary>
        /// 校级榜插入数据
        /// </summary>
        /// <param name="submitData"></param>
        /// <param name="rel"></param>
        /// <param name="s"></param>
        /// <param name="intRank"></param>
        /// <param name="rank"></param>
        private bool InsertSchoolRank(RedisVideoInfo redisVideoInfo)
        {
            List<Redis_IntDubb_Rank> intRank = new List<Redis_IntDubb_Rank>();
            int s = 0;
            Redis_IntDubb_Rank rank = new Redis_IntDubb_Rank
            {
                UserId = redisVideoInfo.UserId,
                VideoId = redisVideoInfo.VideoID.ToString(),
                TotalScore = Convert.ToDouble(redisVideoInfo.TotalScore)
            };
            List<Redis_IntDubb_Rank> rin = redis.Get<List<Redis_IntDubb_Rank>>("Redis_IntDubb_SchoolRank_" + redisVideoInfo.BookId, redisVideoInfo.SchoolID + "_" + redisVideoInfo.VideoNumber);
            if (rin != null)
            {
                foreach (var item in rin)
                {
                    if (item.UserId == redisVideoInfo.UserId)
                    {
                        s = 1;
                        if (Convert.ToDouble(item.TotalScore) <= Convert.ToDouble(redisVideoInfo.TotalScore))
                        {
                            intRank.Add(rank);
                        }
                        else
                        {
                            intRank.Add(new Redis_IntDubb_Rank
                            {
                                UserId = item.UserId,
                                VideoId = item.VideoId,
                                TotalScore = item.TotalScore
                            });
                        }
                    }
                    else
                    {
                        intRank.Add(new Redis_IntDubb_Rank
                        {
                            UserId = item.UserId,
                            VideoId = item.VideoId,
                            TotalScore = item.TotalScore
                        });
                    }
                }
                if (s == 0)
                {
                    intRank.Add(rank);
                }
                intRank = intRank.OrderByDescending(i => i.TotalScore).Take(200).ToList();
                bool bl2 = redis.Set<List<Redis_IntDubb_Rank>>("Redis_IntDubb_SchoolRank_" + redisVideoInfo.BookId, redisVideoInfo.SchoolID + "_" + redisVideoInfo.VideoNumber, intRank);
                LogHelper.Info("插入校级榜数据:redisVideoInfo:" + JsonHelper.EncodeJson(redisVideoInfo) + ";是否成功：" + bl2);
                return bl2;
            }
            else
            {
                intRank.Add(rank);
                intRank.OrderByDescending(i => i.TotalScore).ToList().Take(200);
                bool bl3 = redis.Set<List<Redis_IntDubb_Rank>>("Redis_IntDubb_SchoolRank_" + redisVideoInfo.BookId, redisVideoInfo.SchoolID + "_" + redisVideoInfo.VideoNumber, intRank);
                LogHelper.Info("插入校级榜数据redisVideoInfo:" + JsonHelper.EncodeJson(redisVideoInfo) + ";是否成功：" + bl3);
                return bl3;
            }

        }

        //  string kingsun_ot = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_ot"].ConnectionString;
        //  string kingsun_bj = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_bj"].ConnectionString;
        //  string kingsun_sz = System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_sz"].ConnectionString;


        /// <summary>
        /// 北京版插入用户配音数据
        /// </summary>
        /// <returns></returns>
        public bool insertBJVideoInfo()
        {
            // initializeUserVideo("3", kingsun_bj);
            return true;
        }
        public void insertBJNewRank()
        {
            //initializeNewRank("3", kingsun_bj);
        }

        public void insertBJSchoolRank()
        {
            //initializeSchoolRank("3", kingsun_bj);
        }

        public void insertBJClassRank()
        {
            //initializeClassRank("3", kingsun_bj);
        }

        /// <summary>
        /// 深圳版插入用户配音数据
        /// </summary>
        /// <returns></returns>
        public bool insertSZVideoInfo()
        {
            //initializeUserVideo("21", kingsun_sz);
            return true;
        }

        public void insertSZNewRank()
        {
            //initializeNewRank("21", kingsun_sz);
        }

        public void insertSZSchoolRank()
        {
            //initializeSchoolRank("21", kingsun_sz);
        }

        public void insertSZClassRank()
        {
            // initializeClassRank("21", kingsun_sz);
        }

        /// <summary>
        /// 其他版插入用户配音数据
        /// </summary>
        /// <returns></returns>
        public bool insertOTVideoInfo()
        {
            //initializeUserVideo("24,39,25,5", kingsun_ot);
            return true;
        }

        public void insertOTNewRank()
        {
            //initializeNewRank("24,39,25,5", kingsun_ot);
        }

        public void insertOTSchoolRank()
        {
            //initializeSchoolRank("24,39,25,5", kingsun_ot);
        }

        public void insertOTClassRank()
        {
            //initializeClassRank("24,39,25,5", kingsun_ot);
        }

        /// <summary>
        /// 用户配音数据
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool initializeUserVideo(string VID, string connectionstring)
        {
            Stopwatch st = new Stopwatch();
            TimeSpan ts = new TimeSpan();
            int time = 0;
            try
            {
                string strBid = string.Format(@"SELECT  b.BookID,b.VideoNumber
                                                    FROM    dbo.TB_UserClassRelation a
                                                            RIGHT JOIN FZ_InterestDubbing.dbo.TB_UserVideoDetails b ON b.UserID = a.UserID
                                                    WHERE   b.State = 1
                                                            AND b.BookID <> 0
                                                            AND a.ClassLongID IS NOT NULL
                                                            AND b.VersionID in ({0})
                                                    GROUP BY  b.BookID,b.VideoNumber ", VID);
                DataSet dsBID = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, strBid);
                List<bookInfo> ListBID = JsonHelper.DataSetToIList<bookInfo>(dsBID, 0);

                string UserVideoSql = string.Format(@"SELECT    a.TotalScore ,
                                                                    a.NumberOfOraise ,
                                                                    a.UserID ,
                                                                    a.BookID ,
                                                                    VideoNumber ,
                                                                    a.ID ,
                                                                    a.CreateTime ,
                                                                    b.UserImage ,
                                                                    b.TrueName,
                                                                    b.IsEnableOss
                                                            FROM    FZ_InterestDubbing.dbo.TB_UserVideoDetails a
                                                                    LEFT JOIN ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo b ON b.UserID = a.UserID
                                                            WHERE a.VersionID in ({0})
                                                            ORDER BY CreateTime DESC", VID);
                DataSet UserVideods = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, UserVideoSql);
                List<VInfo> videoInfo = JsonHelper.DataSetToIList<VInfo>(UserVideods, 0);

                st.Stop();
                ts = st.Elapsed;
                time = (int)ts.Milliseconds;
                LogHelper.Info("用户配音第一个日志时间：" + time);


                st.Start();

                string sql = string.Format(@"SELECT  UserVideoID ,
                                                    value = ( STUFF(( SELECT    ',' + CAST(UserID AS VARCHAR(50))
                                                                        FROM      ( SELECT    UserVideoID ,
                                                                                            UserID
                                                                                    FROM      dbo.TB_PraiseStatistics
                                                                                ) a
                                                                        WHERE     a.UserVideoID = SyncClass.UserVideoID
                                                                    FOR
                                                                        XML PATH('')
                                                                    ), 1, 1, '') )
                                            FROM    ( SELECT    UserVideoID ,
                                                                UserID
                                                        FROM      dbo.TB_PraiseStatistics
                                                    ) SyncClass
                                            GROUP BY UserVideoID");
                DataTable dt = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql).Tables[0];
                Hashtable ht = new Hashtable();
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int uvid = Convert.ToInt32(dt.Rows[i]["UserVideoID"].ToString());
                        string[] uid = dt.Rows[i]["value"].ToString().Split(',');

                        ht.Add(uvid, uid.ToList());
                    }
                }


                //foreach (var item in ps1)
                //{
                //    List<string> listUID = new List<string>();
                //    foreach (var em in ps)
                //    {
                //        if (em.UserVideoID == item.UserVideoID)
                //        {
                //            listUID.Add(em.UserID.ToString());
                //        }
                //    }
                //    ht.Add(item.UserVideoID, listUID);
                //}
                st.Stop();
                ts = st.Elapsed;
                time = (int)ts.Milliseconds;
                LogHelper.Info("用户配音第二个日志时间：" + time);

                st.Start();
                using (var Redis = RedisManager.GetClient(0))
                {
                    foreach (var itemBID in ListBID)
                    {
                        List<KeyValuePair<string, string>> kv = new List<KeyValuePair<string, string>>();

                        #region 用户配音数据

                        foreach (var ui in videoInfo)
                        {
                            List<string> listUID = new List<string>();

                            if (ui.BookID == itemBID.BookID)
                            {
                                Redis_IntDubb_VideoInfo videoinfo = new Redis_IntDubb_VideoInfo()
                                {
                                    VideoId = ui.ID.ToString(),
                                    UserId = ui.UserID.ToString(),
                                    TrueName = !string.IsNullOrEmpty(ui.TrueName) ? ui.TrueName : "暂未填写",
                                    UserImage = ui.IsEnableOss != 0
                                            ? _getOssFilesUrl + (string.IsNullOrEmpty(ui.UserImage)
                                                ? "00000000-0000-0000-0000-000000000000"
                                                : ui.UserImage)
                                            : _getFilesUrl + "?FileID=" +
                                              (string.IsNullOrEmpty(ui.UserImage)
                                                  ? "00000000-0000-0000-0000-000000000000"
                                                  : ui.UserImage),
                                    TotalScore = ui.TotalScore.ToString(),
                                    NumberOfOraise = ht.Contains(ui.ID) ? ht[ui.ID] as List<string> : listUID,
                                    CreateTime = ui.CreateTime.ToString(),
                                };

                                //KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(ui.ID.ToString(),JsonHelper.EncodeJson(videoinfo));
                                //kv.Add(kvp);
                                Redis.SetEntryInHash("Redis_IntDubb_VideoInfo_" + itemBID.BookID, ui.ID.ToString(),
                                    JsonHelper.EncodeJson(videoinfo));
                                //redis.Set<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + itemBID.BookID, ui.ID.ToString(), videoinfo);
                            }
                        }
                        //Redis.SetRangeInHash("Redis_IntDubb_VideoInfo_" + itemBID.BookID, kv);
                        //redis.Set("Redis_IntDubb_VideoInfo_" + itemBID.BookID, kv);

                        #endregion
                    }
                }
                st.Stop();
                ts = st.Elapsed;
                time = (int)ts.Milliseconds;
                LogHelper.Info("用户配音第三个日志时间：" + time);
            }
            catch (Exception ex)
            {
                LogHelper.Error("用户配音数据同步错误：" + ex);
            }
            return true;
        }

        /// <summary>
        /// 最新榜
        /// </summary>
        /// <param name="_VID"></param>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool initializeNewRank(string _VID, string connectionstring)
        {
            try
            {
                string strBid = string.Format(@"SELECT  b.BookID,b.VideoNumber
                                            FROM    dbo.TB_UserClassRelation a
                                                    RIGHT JOIN FZ_InterestDubbing.dbo.TB_UserVideoDetails b ON b.UserID = a.UserID
                                            WHERE   b.State = 1
                                                    AND b.BookID <> 0
                                                    AND a.ClassLongID IS NOT NULL
                                                    AND b.VersionID in ({0})
                                            GROUP BY  b.BookID,b.VideoNumber ", _VID);
                DataSet dsBID = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, strBid);
                List<bookInfo> ListBID = JsonHelper.DataSetToIList<bookInfo>(dsBID, 0);

                string newSql = string.Format(@"SELECT  BookID ,
                                                        VideoNumber ,
                                                        MAX(ID) ID,
                                                        UserID
                                                FROM    FZ_InterestDubbing.dbo.TB_UserVideoDetails
                                                WHERE   VersionID IN ( {0} )
                                                GROUP BY BookID ,
                                                        VideoNumber,
                                                        UserID
                                                ORDER BY ID DESC", _VID);
                DataSet UserInfo = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, newSql);
                List<videoInfo> Uinfo = JsonHelper.DataSetToIList<videoInfo>(UserInfo, 0);

                foreach (var itemBID in ListBID)
                {
                    #region 最新榜

                    List<Redis_IntDubb_NewRank> newRank = new List<Redis_IntDubb_NewRank>();
                    int newsort = 1;
                    foreach (var ui in Uinfo)
                    {
                        if (ui.BookID == itemBID.BookID && ui.VideoNumber == itemBID.VideoNumber)
                        {
                            Redis_IntDubb_NewRank intNewRank = new Redis_IntDubb_NewRank
                            {
                                VideoId = ui.ID.ToString(),
                                CreateTime = ui.CreateTime.ToString(),
                                Sort = newsort++
                            };
                            newRank.Add(intNewRank);
                        }
                    }
                    List<Redis_IntDubb_NewRank> nr = newRank.OrderByDescending(i => i.CreateTime).Take(200).ToList();
                    if (nr.Count > 0)
                    {
                        redis.Set<List<Redis_IntDubb_NewRank>>("Redis_IntDubb_NewRank_" + itemBID.BookID, itemBID.VideoNumber.ToString(), nr);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("最新榜错误" + ex);
            }
            return true;
        }

        /// <summary>
        /// 校级榜
        /// </summary>
        /// <param name="_VID"></param>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool initializeSchoolRank(string _VID, string connectionstring)
        {
            try
            {
                string sql = string.Format(@"SELECT  ClassLongID ,
                                                    Test.BookID ,
                                                    Test.VideoNumber ,
                                                    Test.SchoolID ,
                                                    value = ( STUFF(( SELECT    ',' + CAST(id AS VARCHAR(50))
                                                                      FROM      ( SELECT    MAX(a.ID) id ,
                                                                                            a.UserID ,
                                                                                            b.ClassLongID ,
                                                                                            BookID ,
                                                                                            VideoNumber ,
                                                                                            b.SchoolID
                                                                                  FROM      FZ_InterestDubbing.dbo.TB_UserVideoDetails a
                                                                                            LEFT JOIN dbo.TB_UserClassRelation b ON b.UserID = a.UserID
                                                                                  WHERE     b.ClassLongID IS NOT NULL
                                                                                            AND a.BookID <> 0
                                                                                  GROUP BY  b.ClassLongID ,
                                                                                            BookID ,
                                                                                            VideoNumber ,
                                                                                            a.UserID ,
                                                                                            b.SchoolID
                                                                                ) a
                                                                      WHERE     a.ClassLongID = Test.ClassLongID
                                                                                AND a.BookID = Test.BookID
                                                                                AND a.VideoNumber = Test.VideoNumber
                                                                                AND a.SchoolID = Test.SchoolID
                                                                    FOR
                                                                      XML PATH('')
                                                                    ), 1, 1, '') )
                                            FROM    ( SELECT    MAX(a.ID) id ,
                                                                a.UserID ,
                                                                b.ClassLongID ,
                                                                BookID ,
                                                                VideoNumber ,
                                                                b.SchoolID
                                                      FROM      FZ_InterestDubbing.dbo.TB_UserVideoDetails a
                                                                LEFT JOIN dbo.TB_UserClassRelation b ON b.UserID = a.UserID
                                                      WHERE     b.ClassLongID IS NOT NULL
                                                                AND a.BookID <> 0
                                                                AND b.SchoolID IS NOT NULL
                                                                AND b.SchoolID <> 0
                                                                AND a.VersionID IN ( {0} )
                                                      GROUP BY  b.ClassLongID ,
                                                                b.SchoolID ,
                                                                BookID ,
                                                                VideoNumber ,
                                                                a.UserID
                                                    ) AS Test
                                            GROUP BY ClassLongID ,
                                                    Test.BookID ,
                                                    Test.VideoNumber ,
                                                    Test.SchoolID
                                            ", _VID);

                DataTable dt = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql).Tables[0];

                using (var Redis = RedisManager.GetClient(0))
                {
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            List<Redis_IntDubb_Rank> CIDRank = new List<Redis_IntDubb_Rank>();
                            string[] uid = dt.Rows[i]["value"].ToString().Split(',');
                            for (int j = 0; j < uid.Length; j++)
                            {
                                CIDRank.Add(new Redis_IntDubb_Rank
                                {
                                    UserId = uid[j]
                                });
                            }
                            List<Redis_IntDubb_Rank> sc = CIDRank.OrderByDescending(s => s.TotalScore).Take(200).ToList();
                            if (sc.Count > 0)
                            {
                                Redis.SetEntryInHash("Redis_IntDubb_SchoolRank_" + dt.Rows[i]["BookID"].ToString(), dt.Rows[i]["SchoolID"].ToString() + "_" + dt.Rows[i]["VideoNumber"].ToString(), JsonHelper.EncodeJson(sc));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("校级榜错误：" + ex);
            }
            return true;
        }

        /// <summary>
        ///班级榜
        /// </summary>
        /// <param name="_VID"></param>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public bool initializeClassRank(string _VID, string connectionstring)
        {
            try
            {

                string sql = string.Format(@"SELECT  ClassLongID ,
                                                    Test.BookID ,
                                                    Test.VideoNumber ,
                                                    value = ( STUFF(( SELECT    ',' + CAST(id AS VARCHAR(50))
                                                                      FROM      ( SELECT    MAX(a.ID) id ,
                                                                                            a.UserID ,
                                                                                            b.ClassLongID ,
                                                                                            BookID ,
                                                                                            VideoNumber
                                                                                  FROM      FZ_InterestDubbing.dbo.TB_UserVideoDetails a
                                                                                            LEFT JOIN dbo.TB_UserClassRelation b ON b.UserID = a.UserID
                                                                                  WHERE     b.ClassLongID IS NOT NULL
                                                                                            AND a.BookID <> 0
                                                                                  GROUP BY  b.ClassLongID ,
                                                                                            BookID ,
                                                                                            VideoNumber ,
                                                                                            a.UserID
                                                                                ) a
                                                                      WHERE     a.ClassLongID = Test.ClassLongID
                                                                                AND a.BookID = Test.BookID
                                                                                AND a.VideoNumber = Test.VideoNumber                                    
                                                                    FOR
                                                                      XML PATH('')
                                                                    ), 1, 1, '') )
                                            FROM    ( SELECT    MAX(a.ID) id ,
                                                                a.UserID ,
                                                                b.ClassLongID ,
                                                                BookID ,
                                                                VideoNumber
                                                      FROM      FZ_InterestDubbing.dbo.TB_UserVideoDetails a
                                                                LEFT JOIN dbo.TB_UserClassRelation b ON b.UserID = a.UserID
                                                      WHERE     b.ClassLongID IS NOT NULL
                                                                AND a.BookID <> 0
                                                                AND a.VersionID IN ({0})
                                                      GROUP BY  b.ClassLongID ,
                                                                BookID ,
                                                                VideoNumber ,
                                                                a.UserID
                                                    ) AS Test
                                            GROUP BY ClassLongID ,
                                                    Test.BookID ,
                                                    Test.VideoNumber", _VID);
                DataTable dt = SqlHelper.ExecuteDataset(connectionstring, CommandType.Text, sql).Tables[0];

                using (var Redis = RedisManager.GetClient(0))
                {
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            List<Redis_IntDubb_Rank> CIDRank = new List<Redis_IntDubb_Rank>();
                            string[] uid = dt.Rows[i]["value"].ToString().Split(',');
                            for (int j = 0; j < uid.Length; j++)
                            {
                                CIDRank.Add(new Redis_IntDubb_Rank
                                {
                                    UserId = uid[j]
                                });
                            }
                            List<Redis_IntDubb_Rank> CID =
                                CIDRank.OrderByDescending(s => s.TotalScore).Take(200).ToList();
                            if (CID.Count > 0)
                            {
                                Redis.SetEntryInHash(
                                    "Redis_IntDubb_ClassRank_" + dt.Rows[i]["BookID"].ToString(),
                                    dt.Rows[i]["ClassLongID"].ToString().ToLower() + "_" +
                                    dt.Rows[i]["VideoNumber"].ToString(), JsonHelper.EncodeJson(CID));
                            }
                        }
                    }
                }

                //foreach (var itemBID in ListBID)
                //{
                //    #region 班级榜

                //    int classsort = 1;
                //    foreach (var cd in classlid)
                //    {
                //        List<Redis_IntDubb_Rank> CIDRank = new List<Redis_IntDubb_Rank>();
                //        foreach (var cm in ClassRank)
                //        {
                //            //if (cm.UserType != "12")
                //            //{
                //            if (cd.ClassLongID.ToString() == cm.ClassId && cm.BookID == itemBID.BookID.ToString() && cm.VideoNumber == itemBID.VideoNumber.ToString())
                //            {
                //                Redis_IntDubb_Rank cRank = new Redis_IntDubb_Rank
                //                {
                //                    UserId = cm.UserId,
                //                    TotalScore = cm.TotalScore,
                //                    VideoId = cm.VideoId,
                //                    Sort = classsort++
                //                };
                //                CIDRank.Add(cRank);
                //            }
                //            //}

                //        }
                //        List<Redis_IntDubb_Rank> CID = CIDRank.OrderByDescending(i => i.TotalScore).Take(200).ToList();
                //        if (CID.Count > 0)
                //        {
                //            redis.Set<List<Redis_IntDubb_Rank>>("Redis_IntDubb_ClassRank_" + itemBID.BookID, cd.ClassLongID.ToString().ToLower() + "_" + itemBID.VideoNumber.ToString(), CID);
                //        }
                //    }

                //    #endregion
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error("班级榜错误:" + ex);
            }
            return true;
        }
    }

    public class Redis_UserInfo
    {
        public string UserId { get; set; }
        public string VideoId { get; set; }
        public double TotalScore { get; set; }
        public string SchoolId { get; set; }
        public string ClassId { get; set; }
        public string BookID { get; set; }
        public string VideoNumber { get; set; }
        public string UserType { get; set; }
    }

    public class videoInfo
    {
        public Guid ClassLongID { get; set; }
        public int UserID { get; set; }
        public string TrueName { get; set; }
        public int SchoolID { get; set; }
        public int ID { get; set; }
        public int BookID { get; set; }
        public int VideoNumber { get; set; }
        public string VideoFileID { get; set; }
        public double TotalScore { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class bookInfo
    {
        public int BookID { get; set; }
        public int VideoNumber { get; set; }
    }

    public class SchoolInfo
    {
        public int SchoolID { get; set; }
    }

    public class ClassInfo
    {
        public Guid ClassLongID { get; set; }
    }

    public class VInfo
    {
        public double TotalScore { get; set; }
        public int UserID { get; set; }
        public int BookID { get; set; }
        public int VideoNumber { get; set; }
        public int ID { get; set; }
        public DateTime CreateTime { get; set; }
        public string UserImage { get; set; }
        public string TrueName { get; set; }
        public int IsEnableOss { get; set; }
        public int NumberOfOraise { get; set; }
    }

    public class PraiseStatistics
    {
        public string UserVideoID { get; set; }
        public int UserID { get; set; }
    }
}
