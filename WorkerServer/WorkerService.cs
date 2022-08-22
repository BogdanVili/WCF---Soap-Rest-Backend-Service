using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServer
{
    public class WorkerService : IWorkerRequest
    {
        public string AddWorkerRest(Firm firm, Department department, Employee employee)
        {
            Database database = Database.GetInstance();

            employee.DateOfBirth = DateTime.ParseExact(employee.DateOfBirthString, "yyyy-MM-dd", null);
            firm.Id = Collections.GetCurrentFirmId();

            database.AddWorker(firm, department, employee);

            return "";
        }

        public string AddWorkerSoap(string message)
        {
            throw new NotImplementedException();
        }

        public string DeleteWorkerRest(string message)
        {
            throw new NotImplementedException();
        }

        public string DeleteWorkerSoap(string message)
        {
            throw new NotImplementedException();
        }

        public string UpdateWorkerRest(string message)
        {
            throw new NotImplementedException();
        }

        public string UpdateWorkerSoap(string message)
        {
            throw new NotImplementedException();
        }
    }
}
