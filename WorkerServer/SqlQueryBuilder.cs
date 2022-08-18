using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServer
{
    public class SqlQueryBuilder
    {
        public static string InsertFirmBuilder(Firm firm)
        {
            return "INSERT INTO " + "Firm (Name) " +
                   "VALUES " + $"(\'{firm.Name}\')" + ";\n";
        }

        public static string InsertDepartmentBuilder(Department department)
        {
            return "INSERT INTO " + "Department (Id, Name) " +
                   "VALUES " + $"({department.Id}, \'{department.Name}\')" + ";\n";
        }
        public static string InsertEmployeeBuilder(Employee employee)
        {
            return "INSERT INTO " + "Employee (FirstName, LastName, DateOfBirth, JMBG, DeservesRaise, Email)" +
                   "VALUES " + $"(\'{employee.FirstName}\', \'{employee.LastName}\', \'{employee.DateOfBirth.ToString("yyyyMMdd")}\', {employee.JMBG}, {Convert.ToInt32(employee.DeservesRaise)}, \'{employee.Email}\')" + ";\n";
        }

        public static string InsertWorkingBuilder(string firmId, string departmentId, string employeeId)
        {
            return "INSERT INTO " + "Working (Id_Firm, Id_Department, Id_Employee) " + 
                   "VALUES " + $"({firmId},{departmentId},{employeeId})" + ";\n";
        }
    }
}
