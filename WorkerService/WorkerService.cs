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
        private static Database database = Database.GetInstance();

        public WorkerService()
        {
            CSVManager csvManager = new CSVManager("employee.csv");
            Thread threadCSVManager = new Thread(() => CSVManager.StartThread());
            threadCSVManager.Name = "CSVManager";
            threadCSVManager.Start();
        }

        #region REST
        public string AddWorkerRest(FirmRequest firmRequest, DepartmentRequest departmentRequest, EmployeeRequest employeeRequest)
        {
            Firm _firm = database.converter.ConvertFirmRequestToFirm(firmRequest);
            Department _department = database.converter.ConvertDepartmentRequestToDepartment(departmentRequest);
            Employee _employee = database.converter.ConvertEmployeeRequestToEmployee(employeeRequest);

            if (database.validation.IsWorkerEmpty(_firm, _department, _employee))
                return "Some Fields are empty!";

            if (database.validation.CheckIfWorkingExists(_firm.Id, _department.Id, _employee.JMBG))
                return "Same Ids already exist!";

            return database.AddWorker(_firm, _department, _employee);
        }

        public string UpdateWorkerRest(FirmRequest firmRequest, DepartmentRequest departmentRequest, EmployeeRequest employeeRequest)
        {
            Firm _firm = database.converter.ConvertFirmRequestToFirm(firmRequest);
            Department _department = database.converter.ConvertDepartmentRequestToDepartment(departmentRequest);
            Employee _employee = database.converter.ConvertEmployeeRequestToEmployee(employeeRequest);

            if (!database.validation.CheckIfWorkingExists(_firm.Id, _department.Id, _employee.JMBG))
                return "Some Id is incorrect ";

            if (database.validation.IsWorkerEmpty(_firm, _department, _employee))
                return "Fields are empty! Please fill in all the fields.";

            return database.UpdateWorker(_firm, _department, _employee);
        }

        public string DeleteWorkerRest(DeleteParameters deleteParameters)
        {
            if (deleteParameters.FirmName != null)
            {
                if(!database.validation.CheckIfFirmExists(deleteParameters.FirmName))
                    return "Firm with that Name doesnt exist";

                return database.DeleteWorker(deleteParameters.FirmName,
                                             0,
                                             0);
            }

            if (deleteParameters.DepartmentId > 0)
            {
                if(!database.validation.CheckIfDepartmentExists(deleteParameters.DepartmentId))
                    return "Department with that Id doesnt exist";

                return database.DeleteWorker(null,
                                             deleteParameters.DepartmentId,
                                             0);
            }

            if (deleteParameters.EmployeeJMBG > 0)
            {
                if(!database.validation.CheckIfEmployeeExists(deleteParameters.EmployeeJMBG))
                    return "Employee with that JMBG doesnt exist";

                return database.DeleteWorker(null,
                                             0,
                                             deleteParameters.EmployeeJMBG);
            }

            return "Deleting Failed, Need to send Id.\n";
        }
        #endregion

        #region SOAP
        public string AddWorkerSoap(FirmRequest firmRequest, DepartmentRequest departmentRequest, EmployeeRequest employeeRequest)
        {
            Firm _firm = database.converter.ConvertFirmRequestToFirm(firmRequest);
            Department _department = database.converter.ConvertDepartmentRequestToDepartment(departmentRequest);
            Employee _employee = database.converter.ConvertEmployeeRequestToEmployee(employeeRequest);

            if (database.validation.IsWorkerEmpty(_firm, _department, _employee))
                return "Some Fields are empty!";

            if (database.validation.CheckIfWorkingExists(_firm.Id, _department.Id, _employee.JMBG))
                return "Same Ids already exist!";

            return database.AddWorker(_firm, _department, _employee);
        }

        public string UpdateWorkerSoap(FirmRequest firmRequest, DepartmentRequest departmentRequest, EmployeeRequest employeeRequest)
        {
            Firm _firm = database.converter.ConvertFirmRequestToFirm(firmRequest);
            Department _department = database.converter.ConvertDepartmentRequestToDepartment(departmentRequest);
            Employee _employee = database.converter.ConvertEmployeeRequestToEmployee(employeeRequest);

            if (!database.validation.CheckIfWorkingExists(_firm.Id, _department.Id, _employee.JMBG))
                return "Some Id is incorrect ";

            if (database.validation.IsWorkerEmpty(_firm, _department, _employee))
                return "Fields are empty! Please fill in all the fields.";

            return database.UpdateWorker(_firm, _department, _employee);
        }

        public string DeleteWorkerSoap(DeleteParameters deleteParameters)
        {
            if (deleteParameters.FirmName != null)
            {
                if (!database.validation.CheckIfFirmExists(deleteParameters.FirmName))
                    return "Firm with that Name doesnt exist";

                return database.DeleteWorker(deleteParameters.FirmName,
                                             0,
                                             0);
            }

            if (deleteParameters.DepartmentId > 0)
            {
                if (!database.validation.CheckIfDepartmentExists(deleteParameters.DepartmentId))
                    return "Department with that Id doesnt exist";

                return database.DeleteWorker(null,
                                             deleteParameters.DepartmentId,
                                             0);
            }

            if (deleteParameters.EmployeeJMBG > 0)
            {
                if (!database.validation.CheckIfEmployeeExists(deleteParameters.EmployeeJMBG))
                    return "Employee with that JMBG doesnt exist";

                return database.DeleteWorker(null,
                                             0,
                                             deleteParameters.EmployeeJMBG);
            }

            return "Deleting Failed, Need to send Id.\n";
        }
        #endregion
    }
}
