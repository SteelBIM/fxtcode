using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CBSS.ResourcesManager.IBLL
{
    public interface IResourcesService
    {
        string ImportVideoDetails(string fileName, string filePath, HttpPostedFileBase ofile, int bookId,string bookName);
    }
}
