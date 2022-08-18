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

            Console.ReadKey();
        }
    }
}
