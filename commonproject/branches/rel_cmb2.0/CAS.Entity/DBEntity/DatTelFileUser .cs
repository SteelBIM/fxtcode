using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_TelFile_User")]
    public class DatTelFileUser : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _telfileid;
        /// <summary>
        /// 传阅文件Id
        /// </summary>
        public int telfileid
        {
            get { return _telfileid; }
            set { _telfileid = value; }
        }
        private int _touserid;
        /// <summary>
        /// 接收人Id
        /// </summary>
        public int touserid
        {
            get { return _touserid; }
            set { _touserid = value; }
        }
        private string _tousername;
        /// <summary>
        /// 接收人姓名
        /// </summary>
        public string tousername
        {
            get { return _tousername; }
            set { _tousername = value; }
        }
        private string _chuanyueyijian;
        /// <summary>
        /// 传阅意见
        /// </summary>
        public string chuanyueyijian
        {
            get { return _chuanyueyijian; }
            set { _chuanyueyijian = value; }
        }
        private DateTime? _qianshoushijian;
        /// <summary>
        /// 签收时间
        /// </summary>
        public DateTime? qianshoushijian
        {
            get { return _qianshoushijian; }
            set { _qianshoushijian = value; }
        }

        private bool _status = false;
        /// <summary>
        /// 状态
        /// </summary>
        public bool status
        {
            get { return _status; }
            set { _status = value; }
        }
    }

}
