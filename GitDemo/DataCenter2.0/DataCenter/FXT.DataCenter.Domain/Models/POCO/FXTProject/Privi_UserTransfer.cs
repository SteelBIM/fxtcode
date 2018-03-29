using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_UserTransfer
    {
        private int _usertransferid;
		/// <summary>
		/// 员工调动表
		/// </summary>
		//[SQLField("usertransferid", EnumDBFieldUsage.PrimaryKey, true)]
		public int usertransferid
		{
			get{ return _usertransferid;}
			set{ _usertransferid=value;}
		}
		private string _userid;
		public string userid
		{
			get{ return _userid;}
			set{ _userid=value;}
		}
		private int _oldcompanyid;
		/// <summary>
		/// 旧公司
		/// </summary>
		public int oldcompanyid
		{
			get{ return _oldcompanyid;}
			set{ _oldcompanyid=value;}
		}
		private string _oldcompanyname;
		public string oldcompanyname
		{
			get{ return _oldcompanyname;}
			set{ _oldcompanyname=value;}
		}
		private int _olddepartmentid;
		/// <summary>
		/// 旧部门
		/// </summary>
		public int olddepartmentid
		{
			get{ return _olddepartmentid;}
			set{ _olddepartmentid=value;}
		}
		private string _olddepartmentname;
		public string olddepartmentname
		{
			get{ return _olddepartmentname;}
			set{ _olddepartmentname=value;}
		}
		private int _oldpostid;
		/// <summary>
		/// 旧职位
		/// </summary>
		public int oldpostid
		{
			get{ return _oldpostid;}
			set{ _oldpostid=value;}
		}
		private string _oldpostname;
		public string oldpostname
		{
			get{ return _oldpostname;}
			set{ _oldpostname=value;}
		}
		private int _newcompanyid;
		/// <summary>
		/// 新公司
		/// </summary>
		public int newcompanyid
		{
			get{ return _newcompanyid;}
			set{ _newcompanyid=value;}
		}
		private string _newcompanyname;
		public string newcompanyname
		{
			get{ return _newcompanyname;}
			set{ _newcompanyname=value;}
		}
		private int _newdepartmentid;
		/// <summary>
		/// 新部门
		/// </summary>
		public int newdepartmentid
		{
			get{ return _newdepartmentid;}
			set{ _newdepartmentid=value;}
		}
		private string _newdepartmentname;
		public string newdepartmentname
		{
			get{ return _newdepartmentname;}
			set{ _newdepartmentname=value;}
		}
		private int _newpostid;
		/// <summary>
		/// 新职位
		/// </summary>
		public int newpostid
		{
			get{ return _newpostid;}
			set{ _newpostid=value;}
		}
		private string _newpostname;
		public string newpostname
		{
			get{ return _newpostname;}
			set{ _newpostname=value;}
		}
		private string _operator;
		/// <summary>
		/// 操作人
		/// </summary>
		public string operators
		{
			get{ return _operator;}
			set{ _operator=value;}
		}
		private DateTime _operatedate = DateTime.Now;
		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime operatedate
		{
			get{ return _operatedate;}
			set{ _operatedate=value;}
		}
		private int _transfertypecode = 9006001;
		/// <summary>
		/// 调动类型
		/// </summary>
		public int transfertypecode
		{
			get{ return _transfertypecode;}
			set{ _transfertypecode=value;}
		}

    }
}
