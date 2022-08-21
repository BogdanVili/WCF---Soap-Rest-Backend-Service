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
        #region Select
        public static string SelectAll(string obj)
        {
            return $"SELECT * FROM {obj};\n";
        }
        #endregion

        #region Insert
        public static string InsertFirmBuilder(Firm firm)
        {
            return "INSERT INTO " + "Firm (Name) " +
                   "VALUES " + $"(\'{firm.Name}\')" + ";\n";
        }

        public static string InsertDepartmentBuilder(Department department)
        {
            return "INSERT INTO " + "Department (Id, Name) " +
                   "VALUES " + $"({department.Id}, " +
                              $"\'{department.Name}\')" + ";\n";
        }
        public static string InsertEmployeeBuilder(Employee employee)
        {
            return "INSERT INTO " + "Employee (FirstName, LastName, DateOfBirth, JMBG, DeservesRaise, Email)" +
                   "VALUES " + $"(\'{employee.FirstName}\', " +
                                $"\'{employee.LastName}\', " +
                                $"\'{employee.DateOfBirth.ToString("yyyyMMdd")}\', " +
                                  $"{employee.JMBG}, " +
                                  $"{Convert.ToInt32(employee.DeservesRaise)}, " +
                                $"\'{employee.Email}\')" + ";\n";
        }

        public static string InsertWorkingBuilder(int firmId, int departmentId, long employeeId)
        {
            return "INSERT INTO " + "Working (Id_Firm, Id_Department, Id_Employee) " + 
                   "VALUES " + $"({firmId}, " +
                               $"{departmentId}, " +
                               $"{employeeId})"+ ";\n";
        }
        #endregion

        #region Update
        public static string UpdateFirmBuilder(Firm firm)
        {
            return "UPDATE FIRM SET " + $"Name = \'{firm.Name}\' " + "WHERE " + $"Id = {firm.Id}";
        }

        public static string UpdateDepartmentBuilder(Department department)
        { 
            return "UPDATE Department SET " + $"Name = \'{department.Name}\' " + "WHERE " + $"Id = {department.Id}";
        }

        public static string UpdateEmployeeBuilder(Employee employee)
        {
            return "UPDATE Employee SET " +   $"FirstName = \'{employee.FirstName}\', " +
                                               $"LastName = \'{employee.LastName}\', " +
                                            $"DateOfBirth = \'{employee.DateOfBirth.ToString("yyyyMMdd")}\', " +
                                            $"DeservesRaise = {Convert.ToInt32(employee.DeservesRaise)}, " +
                                                  $"Email = \'{employee.Email}\'" +
                   "WHERE " + $"JMBG = {employee.JMBG}" + ";\n";
        }
        #endregion
    }
}
