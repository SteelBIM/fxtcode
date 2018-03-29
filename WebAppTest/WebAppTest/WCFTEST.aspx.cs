using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebAppTest.ServiceReference1;

namespace WebAppTest
{
    public partial class WCFTEST : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EmployeesClient emp = new ServiceReference1.EmployeesClient();
            Response.Write(emp.GetString());
        }
    }
}