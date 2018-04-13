using System;
using System.Collections.Generic;
using CBSS.Framework.Contract;

namespace CBSS.Tbx.Contract
{
    public class ArticleRequest : Request
    {

        public string Title { get; set; }
        public int ChannelId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ChannelRequest : Request
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }

    public class TagRequest : Request
    {
        public Orderby Orderby { get; set; }
    }
    public class ApiFunctionRequest : Request
    {
        public string ApiFunctionName { get; set; }
        public string SystemCode { get; set; }
    }
    public class ModelRequest : Request
    {
        public string ModelName { get; set; }
    }

    public class ModuleRequest : Request
    {
        public string ModuleName { get; set; }
    }
    public class ModuleManageRequest : Request
    {
        public int MarketBookID { get; set; }
        public string ModuleName { get; set; }
    }
    public class AppRequest : Request
    {
        public string AppName { get; set; }
    }
    public class AppVersionRequest : Request
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 应用类别(1安卓，2苹果)
        /// </summary>
        public int AppType { get; set; }
    }
    public class MarketBookRequest : Request
    {
        /// <summary>
        /// 市场书籍名
        /// </summary>
        public string MarketBookName { get; set; }

        public int? MarketClassifyId { get; set; }

        public string MarketClassifyName { get; set; }

        public string MarketClassifyIdList { get; set; }
        public string AppID { get; set; }
    }
    public class GoodModuleItemRequest : Request
    {
        public int? MarketClassifyId { get; set; }
        public string MarketClassifyIdList { get; set; }
    }
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserInfoRequest : Request
    {
        public string UserName { get; set; }

        public string TrueName { get; set; }
        public string TelePhone { get; set; }
    }
    /// <summary>
    /// 图片库管理
    /// </summary>
    public class ModelImgLibraryRequest : Request
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModelID { get; set; }

    }
    /// <summary>
    /// 商品
    /// </summary>
    public class GoodRequest : Request
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodName { get; set; }
        /// <summary>
        /// 商品出售方式1单册2套餐
        /// </summary>
        public int GoodWay { get; set; }

    }
    /// <summary>
    /// 商品价格
    /// </summary>
    public class GoodPriceRequest : Request
    {
        public int GoodID { get; set; }
    }

    public class AppModuleItemRequest : Request
    {
        public string AppID { get; set; }
        public string ModuleName { get; set; }
    }

    public enum Orderby
    {
        ID = 0,
        Hits = 1
    }
}
