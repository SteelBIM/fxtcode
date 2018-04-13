using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CBSS.IBS.Contract
{
    public class MOD_PushData
    {
        /// <summary>
        /// 数据类型,1=账号信息（暂不用）,2=班级信息,3=学校信息,4=用户信息,5=区域信息,6=课程信息,7=用户班级关系,8=班级学校关系,9=学校区域关系
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 变更类型,0=查询,1=更新,2=增加,3=删除
        /// </summary>
        public int ChangeType { get; set; }

        /// <summary>
        /// 数据ID
        /// </summary>
        public string Data { get; set; }
    }

}
