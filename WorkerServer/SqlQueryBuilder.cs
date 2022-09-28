using Common.Model;
using Common.ModelCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServer
{
    internal class SqlQueryBuilder
    {
        #region Select
        public static string SelectAll(string obj)
        {
            return String.Format("SELECT * FROM {0};\n", obj);
        }

        public static string SelectFirmIdBuilder(string firmName)
        {
            return String.Format("SELECT Id " +
                                 "FROM Firm " +
                                 "WHERE Name = \'{0}\';\n", 
                                  firmName);
        }

        public static string SelectFirmByName(string firmName)
        {
            return String.Format("SELECT * " +
                                 "FROM Firm " +
                                 "WHERE Firm.Name = \'{0}\';\n",
                                  firmName);
        }

        public static string SelectDepartmentById(int departmentId)
        {
            return String.Format("SELECT * " +
                                 "FROM Department " +
                                 "WHERE Department.Id = {0};\n",
                                  departmentId);
        }

        public static string SelectEmployeeById(long employeeId)
        {
            return String.Format("SELECT * " +
                                 "FROM Employee " +
                                 "WHERE Employee.JMBG = {0};\n",
                                  employeeId);
        }

        public static string SelectWorkingByFirmId(int firmId)
        {
            return String.Format("SELECT * " +
                                 "FROM Working " +
                                 "WHERE Working.FirmId = {0};\n",
                                  firmId);
        }

        public static string SelectWorkingByDepartmentId(int departmentId)
        {
            return String.Format("SELECT * " +
                                 "FROM Working " +
                                 "WHERE Working.DepartmentId = {0};\n",
                                  departmentId);
        }

        public static string SelectWorkingByEmployeeId(long employeeId)
        {
            return String.Format("SELECT * " +
                                 "FROM Working " +
                                 "WHERE Working.EmployeeId = {0};\n",
                                  employeeId);
        }
        #endregion

        #region Exists
        public static string FirmExists(string firmName)
        {
            return String.Format("EXEC FirmExists \'{0}\'",
                                  firmName);
        }

        public static string DepartmentExists(int departmentId)
        {
            return String.Format("EXEC DepartmentExists {0}",
                                  departmentId);
        }

        public static string EmployeeExists(long employeeJMBG)
        {
            return String.Format("EXEC EmployeeExists {0}",
                                  employeeJMBG);
        }

        public static string DepartmentNameExistsInFirm(string firmName, string departmentName)
        {
            return String.Format("EXEC DepartmentNameExistsInFirm \'{0}\', \'{1}\';\n",
                                  firmName,
                                  departmentName);
        }

        public static string WorkingExists(int firmId, int departmentId, long employeeId)
        {
            return String.Format("EXEC WorkingExists {0}, {1}, {2}",
                                  firmId,
                                  departmentId,
                                  employeeId);
        }
        #endregion

        #region Insert
        public static string InsertFirmBuilder(Firm firm)
        {
            return String.Format("INSERT INTO Firm (Name) " +
                                 "VALUES (\'{0}\');\n", 
                                  firm.Name);
        }
        public static string InsertDepartmentBuilder(Department department)
        {
            return String.Format("INSERT INTO Department (Id, Name) " +
                                 "VALUES ({0}, \'{1}\');\n", 
                                  department.Id, 
                                  department.Name);
        }
        public static string InsertEmployeeBuilder(Employee employee)
        {
            return String.Format("INSERT INTO Employee (FirstName, LastName, DateOfBirth, JMBG, DeservesRaise, Email) " +
                                 "VALUES (\'{0}\', \'{1}\', \'{2}\', {3}, {4},\'{5}\');\n",
                                  employee.FirstName,
                                  employee.LastName,
                                  employee.DateOfBirth.ToString("yyyyMMdd"),
                                  employee.JMBG,
                                  Convert.ToInt32(employee.DeservesRaise),
                                  employee.Email);
        }
        public static string InsertWorkingBuilder(int firmId, int departmentId, long employeeId)
        {
            return String.Format("INSERT INTO Working (FirmId, DepartmentId, EmployeeId) " +
                                 "VALUES ({0}, {1}, {2});\n",
                                  firmId,
                                  departmentId,
                                  employeeId);
        }
        #endregion

        #region Update
        public static string UpdateFirmBuilder(Firm firm)
        {
            return String.Format("UPDATE Firm SET Name = \'{0}\'" +
                                 "WHERE Id = {1};\n",
                                  firm.Name,
                                  firm.Id);
        }

        public static string UpdateDepartmentBuilder(Department department)
        { 
            return String.Format("UPDATE Department SET Name = \'{0}\' " +
                                 "WHERE Id = {1};\n",
                                  department.Name,
                                  department.Id);
        }

        public static string UpdateEmployeeBuilder(Employee employee)
        {
            return String.Format("UPDATE Employee " +
                                 "SET FirstName = \'{0}\', LastName = \'{1}\', DateOfBirth = \'{2}\', DeservesRaise = {3}, Email = \'{4}\'" +
                                 "WHERE JMBG = {5};\n",
                                  employee.FirstName,
                                  employee.LastName,
                                  employee.DateOfBirth.ToString("yyyyMMdd"),
                                  Convert.ToInt32(employee.DeservesRaise),
                                  employee.Email,
                                  employee.JMBG);
        }
        #endregion

        #region Delete
        public static string DeleteFirmBuilder(int firmId)
        {
            return String.Format("DELETE FROM Firm " +
                                 "WHERE Id = {0};\n",
                                 firmId);
        }

        public static string DeleteDepartmentBuilder(int departmentId)
        {
            return String.Format("DELETE FROM Department " +
                                 "WHERE Id = {0};\n",
                                  departmentId);
        }

        public static string DeleteEmployeeBuilder(long employeeId)
        {
            return String.Format("DELETE FROM Employee " +
                                 "WHERE JMBG = {0};\n",
                                  employeeId);
        }

        public static string DeleteWorkingBuilder(Working working)
        {
            return String.Format("DELETE FROM Working " +
                                 "WHERE FirmId = {0} AND DepartmentId = {1} AND EmployeeId = {2};\n", 
                                  working.FirmId, 
                                  working.DepartmentId, 
                                  working.EmployeeId);

        }
        #endregion

        #region UpdateEmployeeData
        public static string UpdateEmployeeDataBuilder(EmployeeUpdateData employeeUpdateData)
        {
            return String.Format("UPDATE Employee " +
                                 "SET DeservesRaise = {0}, Email = \'{1}\' " +
                                 "WHERE JMBG = {2};\n", 
                                  Convert.ToInt32(employeeUpdateData.DeservesRaise), 
                                  employeeUpdateData.Email, 
                                  employeeUpdateData.JMBG);
        }
        #endregion


    }
}
