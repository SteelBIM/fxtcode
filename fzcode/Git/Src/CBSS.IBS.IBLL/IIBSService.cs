using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.IBLL
{
    public interface IIBSService: IIBSUserRelation,IIBSBookResource,IIBSResource, IIBSData_AreaSchRelationBLL, IIBSData_UserInfoBLL, IIBSData_SchClassRelationBLL, IIBSData_ClassUserRelationBLL
    {
    }
}
