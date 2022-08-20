using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Database database = Database.GetInstance();
            System.Threading.Thread.Sleep(1000);
            //database.AddWorker(new Firm("Bosch", 1), 
            //                   new Department("VeshoMashino", 1), 
            //                   new Employee("Imerko", "Prezimenkovic", DateTime.Now, 1111, true, "lepmejl@mejl"));
            database.ReadWorkers();
            Console.ReadKey();
        }
    }
}
