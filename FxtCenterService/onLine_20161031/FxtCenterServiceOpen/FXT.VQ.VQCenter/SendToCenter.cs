using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/***********************************************************
 * 功能：运营中心 数据服务类
 *  
 * 创建：魏贝
 * 时间：2015/10
***********************************************************/

namespace FXT.VQ.VQCenter
{
    public static class SendToCenter
    {
        /// <summary>
        /// 发送数据 至 运营中心 
        /// <param name="pm">运营中心 接口数据的实体类</param>
        /// </summary>
        public static int Send(VQCenterService.PropertyMain pm) 
        {
            VQCenterService.PropertyHandClient phc = new VQCenterService.PropertyHandClient();
            return phc.Start(pm);
        }

    }
}
