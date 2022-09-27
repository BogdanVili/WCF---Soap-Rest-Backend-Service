using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Database database = Database.GetInstance();        

            CSVManager csvManager = new CSVManager("employee.csv");
            Thread threadCSVManager = new Thread(() => CSVManager.StartThread());
            threadCSVManager.Name = "CSVManager";
            threadCSVManager.Start();

            Console.ReadKey();
        }
    }
}
