using Common.Model;
using Common.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServer
{
    public class ModelRequestToModelConverter
    {
        Database database;
        public ModelRequestToModelConverter(Database database)
        {
            this.database = database;
        }
        public Firm ConvertFirmRequestToFirm(FirmRequest firmRequest)
        {
            return new Firm(firmRequest.Name,
                            database.GetFirmId(firmRequest.Name));
        }

        public Department ConvertDepartmentRequestToDepartment(DepartmentRequest departmentRequest)
        {
            return new Department(departmentRequest.Name,
                                  departmentRequest.Id);
        }

        public Employee ConvertEmployeeRequestToEmployee(EmployeeRequest employeeRequest)
        {
            return new Employee(employeeRequest.FirstName,
                                employeeRequest.LastName,
                                DateTime.ParseExact(employeeRequest.DateOfBirthString, "yyyy-MM-dd", null),
                                employeeRequest.JMBG,
                                employeeRequest.DeservesRaise,
                                employeeRequest.Email);
        }
    }
}
