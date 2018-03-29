using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.FxtApi.Fxt.Wcf;

namespace FxtSpider.FxtApi.FxtApiClientManager
{
    public class FxtSpiderClientExtend
    {
        private bool ExistParent
        {
            get;
            set;
        }
        public FxtspiderClient FxtApi
        {
            get;
            set;
        }

        public FxtSpiderClientExtend(FxtSpiderClientExtend fxtApi = null)
        {
            if (fxtApi == null)
            {
                this.FxtApi = new FxtspiderClient();
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
