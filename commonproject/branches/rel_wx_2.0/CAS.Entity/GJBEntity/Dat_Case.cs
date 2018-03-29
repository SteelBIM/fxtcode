using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Case : DATCase
    {
        #region 面积段均价
        /// <summary>
        /// 60以下
        /// </summary>
        [SQLReadOnly]
        public decimal oneStageUnitprice { get; set; }
        /// <summary>
        /// 60~90
        /// </summary>
        [SQLReadOnly]
        public decimal twoStageUnitprice { get; set; }
        /// <summary>
        /// 90~120
        /// </summary>
        [SQLReadOnly]
        public decimal threeStageUnitprice { get; set; }
        /// <summary>
        /// 120~144
        /// </summary>
        [SQLReadOnly]
        public decimal forStageUnitprice { get; set; }
        /// <summary>
        /// 144以上
        /// </summary>
        [SQLReadOnly]
        public decimal fiveStageUnitprice { get; set; }
        #endregion
        /// <summary>
        /// 案例类型
        /// </summary>
        [SQLReadOnly]
        public string codetypename { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        [SQLReadOnly]
        public string purposename { get; set; }

        /// <summary>
        ///景观
        /// </summary>
        [SQLReadOnly]
        public string sightcodename { get; set; }

        /// <summary>
        /// 货币单位
        /// </summary>
        [SQLReadOnly]
        public string moneyunitcodename { get; set; }

        /// <summary>
        /// 建筑结构
        /// </summary>
        [SQLReadOnly]
        public string structurecodename { get; set; }

        /// <summary>
        /// 建筑类型
        /// </summary>
        [SQLReadOnly]
        public string buildingtypecodename { get; set; }

        /// <summary>
        /// 户型
        /// </summary>
        [SQLReadOnly]
        public string housetypecodename { get; set; }

        /// <summary>
        /// 装修情况
        /// </summary>
        [SQLReadOnly]
        public string fitmentcodename { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [SQLReadOnly]
        public string cityname { get; set; }

        /// <summary>
        /// 案例所有者
        /// </summary>
        [SQLReadOnly]
        public string fxtcompanyname { get; set; }
    }
}
