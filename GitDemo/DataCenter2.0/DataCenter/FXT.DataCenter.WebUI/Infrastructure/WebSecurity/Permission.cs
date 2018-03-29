using System.Linq;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;

namespace FXT.DataCenter.WebUI.Infrastructure.WebSecurity
{

    public class Permission
    {
        public static void Check(int typeCode, int operateSelfCode, int operateAllCode, out int operate)
        {
            var _menu = new Menu();
            operate = 0;
            var roles = Passport.Current.Roles;
            var menu = _menu.GetMenuByTypeCode(typeCode).FirstOrDefault();
            foreach (var item in roles)
            {
                var funcs = _menu.GetFunctionsByParams(menu.ID, item.ID);
                if (funcs.Any(m => m.FunctionCode == operateAllCode))
                {
                    operate = 2;
                    break;
                }
                if (funcs.Any(m => m.FunctionCode == operateSelfCode))
                {
                    operate = 1;
                    break;
                }
               

            }
        }

    }

    public enum PermissionLevel
    {
        //无权限
        None = 0,
        //当前操作（自己）
        Self = 1,
        //当前操作（全部）
        All = 2
    }

}
