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
            WebServiceHost webServiceHost = new WebServiceHost(typeof(WorkerService));
            webServiceHost.AddServiceEndpoint(typeof(IWorkerRequest), new WebHttpBinding(), new Uri("http://localhost:8000/"));
            webServiceHost.Open();
            Console.ReadKey();
            webServiceHost.Close();
        }
    }
}
