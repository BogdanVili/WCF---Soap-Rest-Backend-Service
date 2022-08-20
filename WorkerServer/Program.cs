using Common;
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
            database.ReadWorkers();
            //database.AddWorker(new Firm("Schneider", 1),
            //                  new Department("ElectroDistribution", 2),
            //                  new Employee("Imerko3", "Prezimenkovic3", DateTime.Now, 3333, false, "lepmejl@mejl"));
            Console.ReadKey();
        }
    }
}
