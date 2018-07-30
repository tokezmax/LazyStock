using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LazyStock.ScheduleServices
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        static void Main()
        {
            /*
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };

            ServiceBase.Run(ServicesToRun);
            */

            //ServiceBase.Run(new ServiceBase[] { new Service1() });
            Application.Run(new TestForm());
            //Application.Run(new CrawlerForm());
        }
    }
}
