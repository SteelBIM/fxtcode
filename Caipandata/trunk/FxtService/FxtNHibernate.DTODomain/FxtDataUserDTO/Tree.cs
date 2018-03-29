using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * 作者:李晓东
 * 摘要:新建 2014.01.09
 * **/
namespace FxtNHibernate.DTODomain.FxtDataUserDTO
{
    public class Tree
    {
        public Tree()
        {
            Children = new List<Object>();
        }
        public int Id { get; set; }
        public string Text { get; set; }
        public int ParentId { get; set; }
        public bool IsMenu { get; set; }
        public object Children { get; set; }
    }
}
