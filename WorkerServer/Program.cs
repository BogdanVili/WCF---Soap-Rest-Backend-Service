using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Database database = Database.GetInstance();        
            database.ReadModels();
            //database.UpdateWorker(new Firm("Schneider5", 1),
            //                  new Department("ElectroDistribution5", 2),
            //                  new Employee("Imerko5", "Prezimenkovic5", new DateTime(2022, 8, 20), 3333, true, "lepmejl3@mejl"));
            WebServiceHost webServiceHost = new WebServiceHost(typeof(WorkerService));
            webServiceHost.AddServiceEndpoint(typeof(IWorkerRequest), new WebHttpBinding(), new Uri("http://localhost:8000/"));
            webServiceHost.Open();
            Console.ReadKey();
            webServiceHost.Close();
        }
    }
}
