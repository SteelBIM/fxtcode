using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.FxtApi.Fxt.Api;

namespace FxtSpider.FxtApi.FxtApiClientManager
{
    public class FxtAPIClientExtend
    {
        private bool ExistParent
        {
            get;
            set;
        }
        public FxtAPIClient FxtApi
        {
            get;
            set;
        }
        public FxtAPIClientExtend(FxtAPIClientExtend fxtApi = null)
        {
            if (fxtApi == null)
            {
                this.FxtApi = new FxtAPIClient();
                this.ExistParent = false;
            }
            else
            {
                this.FxtApi = fxtApi.FxtApi;
                this.ExistParent = true;
            }
        }
        /// <summary>
        /// 使 System.ServiceModel.ClientBase&lt;TChannel&gt; 对象立即从其当前状态转换到关闭状态。
        /// </summary>
        public void Abort()
        {
            if (!this.ExistParent)
            {
                this.FxtApi.Abort();
            }
        }
    }
}
