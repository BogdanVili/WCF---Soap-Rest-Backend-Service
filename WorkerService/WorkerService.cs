using Common;
using Common.Model;
using Common.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WorkerServer;

namespace WorkerService
{
    public class WorkerService : IWorkerServiceRest, IWorkerServiceSoap
    {
        public static Database database = Database.GetInstance();

        static WorkerService()
        {
            database.ReadModels();

            CSVManager csvManager = new CSVManager("employee.csv");
        }

        #region REST
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

        public string UpdateWorkerRest(Firm firm, Department department, Employee employee)
        {
            employee.DateOfBirth = DateTime.ParseExact(employee.DateOfBirthString, "yyyy-MM-dd", null);
            firm.Id = Collections.GetCurrentFirmId(firm.Name);

            if (!Validation.ExistsInWorkings(firm, department, employee))
                return "Some Id is incorrect ";

            if (Validation.IsWorkerEmpty(firm, department, employee))
                return "Fields are empty! Please fill in all the fields.";

            return database.UpdateWorker(firm, department, employee);
        }

        public string DeleteWorkerRest(DeleteParameters deleteParameters)
        {
            string _firmName = deleteParameters.FirmName;
            if (_firmName != "" && _firmName != null)
            {
                if (!Collections.firms.Any(f => f.Name == _firmName))
                    return "Firm with that Name doesnt exist";

                return database.DeleteWorker(Collections.firms.Find(f => f.Name == _firmName),
                                             null,
                                             null);
            }

            int _departmentId = deleteParameters.DepartmentId;
            if (_departmentId > 0)
            {

                if (!Collections.departments.Any(d => d.Id == _departmentId))
                    return "Department with that Name doesnt exist";

                return database.DeleteWorker(null,
                                             Collections.departments.Find(d => d.Id == _departmentId),
                                             null);
            }

            long _employeeJMBG = deleteParameters.EmployeeJMBG;
            if (_employeeJMBG > 0)
            {
                if (!Collections.employees.Any(e => e.JMBG == _employeeJMBG))
                    return "Employee with that Name doesnt exist";

                return database.DeleteWorker(null,
                                             null,
                                             Collections.employees.Find(e => e.JMBG == _employeeJMBG));
            }

            return "Deleting Failed, Need to send Id.\n";
        }
        #endregion

        #region SOAP
        public string AddWorkerSoap(Firm firm, Department department, Employee employee)
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

        public string UpdateWorkerSoap(Firm firm, Department department, Employee employee)
        {
            employee.DateOfBirth = DateTime.ParseExact(employee.DateOfBirthString, "yyyy-MM-dd", null);
            firm.Id = Collections.GetCurrentFirmId(firm.Name);

            if (!Validation.ExistsInWorkings(firm, department, employee))
                return "Some Id is incorrect ";

            if (Validation.IsWorkerEmpty(firm, department, employee))
                return "Fields are empty! Please fill in all the fields.";

            return database.UpdateWorker(firm, department, employee);
        }

        public string DeleteWorkerSoap(DeleteParameters deleteParameters)
        {
            string _firmName = deleteParameters.FirmName;
            if (_firmName != "" && _firmName != null)
            {
                if (!Collections.firms.Any(f => f.Name == _firmName))
                    return "Firm with that Name doesnt exist";

                return database.DeleteWorker(Collections.firms.Find(f => f.Name == _firmName),
                                             null,
                                             null);
            }

            int _departmentId = deleteParameters.DepartmentId;
            if (_departmentId > 0)
            {

                if (!Collections.departments.Any(d => d.Id == _departmentId))
                    return "Department with that Name doesnt exist";

                return database.DeleteWorker(null,
                                             Collections.departments.Find(d => d.Id == _departmentId),
                                             null);
            }

            long _employeeJMBG = deleteParameters.EmployeeJMBG;
            if (_employeeJMBG > 0)
            {
                if (!Collections.employees.Any(e => e.JMBG == _employeeJMBG))
                    return "Employee with that Name doesnt exist";

                return database.DeleteWorker(null,
                                             null,
                                             Collections.employees.Find(e => e.JMBG == _employeeJMBG));
            }

            return "Deleting Failed, Need to send Id.\n";
        }
        #endregion
    }
}
