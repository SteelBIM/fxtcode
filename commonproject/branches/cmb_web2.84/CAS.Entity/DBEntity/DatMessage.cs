using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Message")]
    public class DatMessage : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _companyid;
        /// <summary>
        /// �ͻ���˾ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _senduserid;
        /// <summary>
        /// ������
        /// </summary>
        public string senduserid
        {
            get { return _senduserid; }
            set { _senduserid = value; }
        }
        private string _touserid;
        /// <summary>
        /// ������
        /// </summary>
        public string touserid
        {
            get { return _touserid; }
            set { _touserid = value; }
        }
        private string _title;
        /// <summary>
        /// ����
        /// </summary>
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _message;
        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }
        private string _remark;
        /// <summary>
        /// URL��
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int _isread = 0;
        /// <summary>
        /// �Ƿ�鿴
        /// </summary>
        public int isread
        {
            get { return _isread; }
            set { _isread = value; }
        }
        private DateTime? _readdate;
        /// <summary>
        /// �鿴ʱ��
        /// </summary>
        public DateTime? readdate
        {
            get { return _readdate; }
            set { _readdate = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// �Ƿ���Ч
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }

}