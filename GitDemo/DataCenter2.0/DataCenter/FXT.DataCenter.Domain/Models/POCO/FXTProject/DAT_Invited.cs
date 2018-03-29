using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_Invited
    {
        private int _invitedid;
		public int invitedid
		{
			get{ return _invitedid;}
			set{ _invitedid=value;}
		}
		private string _invitedname;
		/// <summary>
		/// 被邀请人
		/// </summary>
		public string invitedname
		{
			get{ return _invitedname;}
			set{ _invitedname=value;}
		}
		private string _operator;
		/// <summary>
		/// 邀请人
		/// </summary>
		public string operators
		{
			get{ return _operator;}
			set{ _operator=value;}
		}
		private DateTime _inviteddate = DateTime.Now;
		public DateTime inviteddate
		{
			get{ return _inviteddate;}
			set{ _inviteddate=value;}
		}
		private bool _isactivation = false;
		public bool isactivation
		{
			get{ return _isactivation;}
			set{ _isactivation=value;}
		}
		private string _remark;
		public string remark
		{
			get{ return _remark;}
			set{ _remark=value;}
		}

    }
}
