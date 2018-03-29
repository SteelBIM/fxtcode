namespace FXT.DataCenter.Domain.Models.QueryObjects
{
    public class BaseParams
    {
        public BaseParams()
        {
            this.pageIndex = 1;
            this.pageSize = 30;
        }
        /// <summary>
        /// 页索引
        /// </summary>
        public int pageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int pageSize { get; set; }
    }
}
