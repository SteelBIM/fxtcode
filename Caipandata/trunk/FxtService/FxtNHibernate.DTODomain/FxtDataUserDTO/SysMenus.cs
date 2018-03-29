using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtNHibernate.FxtDataUserDomain.Entities;

/**
 * 作者:李晓东
 * 摘要:新建2014.01.07 
 * **/
namespace FxtNHibernate.DTODomain.FxtDataUserDTO
{
    /// <summary>
    /// SysProductMenu DTO
    /// </summary>
    public class SysMenus:SysMenu
    {
        public SysMenus(SysMenu sysmenu)
        {
            Id = sysmenu.Id;
            MenuName = sysmenu.MenuName;
            ParentId = sysmenu.ParentId;
            Url = sysmenu.Url;
            CreateDate = sysmenu.CreateDate;            
        }
        public int ProductId { get; set; }
    }
}
