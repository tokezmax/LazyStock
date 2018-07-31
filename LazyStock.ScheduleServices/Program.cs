using System.Windows.Forms;

namespace LazyStock.ScheduleServices
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        private static void Main()
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