using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Framework.DAL;
using CBSS.ResourcesManager.IBLL;

namespace CBSS.ResourcesManager.BLL
{
    public partial class ResourcesService: IResourcesService
    {
        Repository repository = new Repository("Resources");
    }
}
