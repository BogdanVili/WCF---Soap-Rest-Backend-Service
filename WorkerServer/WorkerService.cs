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
        Database database = Database.GetInstance();

        public string AddWorkerRest(Firm firm, Department department, Employee employee)
        {
            firm.Departments = new List<Department>();
            department.Employees = new List<Employee>();
            employee.DateOfBirth = DateTime.ParseExact(employee.DateOfBirthString, "yyyy-MM-dd", null);
            firm.Id = Collections.GetCurrentFirmId(firm.Name);

            if (Validation.IsWorkerEmpty(firm, department, employee))
                return "Fields are empty! Please fill in all the fields.";
            if (Validation.FirmContainsDepartmentName(firm, department))
                return "Department with same Name but different id already exists!.";

            return database.AddWorker(firm, department, employee);
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

        public string UpdateWorkerRest(Firm firm, Department department, Employee employee)
        {
            employee.DateOfBirth = DateTime.ParseExact(employee.DateOfBirthString, "yyyy-MM-dd", null);

            database.UpdateWorker(firm, department, employee);

            return "";
        }

        public string UpdateWorkerSoap(string message)
        {
            throw new NotImplementedException();
        }
    }
}
