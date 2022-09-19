using Common;
using Common.Model;
using Common.ModelRequest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using WorkerServer;

namespace WorkerService
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class WorkerService : IWorkerServiceRest, IWorkerServiceSoap
    {
        public static Database database = Database.GetInstance();

        static WorkerService()
        {
            database.ReadModels();

            CSVManager csvManager = new CSVManager("employee.csv");
            Thread threadCSVManager = new Thread(() => CSVManager.StartThread());
            threadCSVManager.Name = "CSVManager";
            threadCSVManager.Start();
        }

        #region REST
        public string AddWorkerRest(FirmRequest firm, DepartmentRequest department, EmployeeRequest employee)
        {
            Firm _firm = firm.ConvertModelRequestToModel();
            Department _department = department.ConvertModelRequestToModel();
            Employee _employee = employee.ConvertModelRequestToModel();

            if (Validation.IsWorkerEmpty(_firm, _department, _employee))
                return "Fields are empty! Please fill in all the fields.";
            if (Validation.FirmContainsDepartmentName(_firm, _department))
                return "Department with same Name but different id already exists!.";

            return database.AddWorker(_firm, _department, _employee);
        }

        public string UpdateWorkerRest(FirmRequest firm, DepartmentRequest department, EmployeeRequest employee)
        {
            Firm _firm = firm.ConvertModelRequestToModel();
            Department _department = department.ConvertModelRequestToModel();
            Employee _employee = employee.ConvertModelRequestToModel();

            if (!Validation.ExistsInWorkings(_firm, _department, _employee))
                return "Some Id is incorrect ";

            if (Validation.IsWorkerEmpty(_firm, _department, _employee))
                return "Fields are empty! Please fill in all the fields.";

            return database.UpdateWorker(_firm, _department, _employee);
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
        public string AddWorkerSoap(FirmRequest firm, DepartmentRequest department, EmployeeRequest employee)
        {
            Firm _firm = firm.ConvertModelRequestToModel();
            Department _department = department.ConvertModelRequestToModel();
            Employee _employee = employee.ConvertModelRequestToModel();

            if (Validation.IsWorkerEmpty(_firm, _department, _employee))
                return "Fields are empty! Please fill in all the fields.";
            if (Validation.FirmContainsDepartmentName(_firm, _department))
                return "Department with same Name but different id already exists!.";

            return database.AddWorker(_firm, _department, _employee);
        }

        public string UpdateWorkerSoap(FirmRequest firm, DepartmentRequest department, EmployeeRequest employee)
        {
            Firm _firm = firm.ConvertModelRequestToModel();
            Department _department = department.ConvertModelRequestToModel();
            Employee _employee = employee.ConvertModelRequestToModel();

            if (!Validation.ExistsInWorkings(_firm, _department, _employee))
                return "Some Id is incorrect ";

            if (Validation.IsWorkerEmpty(_firm, _department, _employee))
                return "Fields are empty! Please fill in all the fields.";

            return database.UpdateWorker(_firm, _department, _employee);
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
