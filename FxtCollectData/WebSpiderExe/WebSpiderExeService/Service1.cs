using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSpiderExeService
{
    public partial class Service1 : ServiceBase
    {
        private Thread Bg公评网析价格Thread { set; get; }

        public Service1()
        {
            InitializeComponent();
            try
            {
                this.Bg公评网析价格Thread = new Thread(Th公评网析价格);
                this.Bg公评网析价格Thread.IsBackground = true;
            }
            catch (Exception ex)
            {
                
            }
        }

        protected override void OnStart(string[] args)
        {
            this.Bg公评网析价格Thread.Start();
        }

        protected override void OnStop()
        {
        }

        private void Th公评网析价格()
        {
            while (true)
            {
                try
                {
                    公评网析价格 s = new 公评网析价格();
                    s.GetPrice();
                    Thread.Sleep(100 * 1000);//一分钟
                }
                catch (Exception ex)
                {                    
                    Thread.Sleep(100 * 1000);//一分钟
                }
            }
        }
    }
}
