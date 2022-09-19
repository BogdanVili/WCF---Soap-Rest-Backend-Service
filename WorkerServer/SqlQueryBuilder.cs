﻿using Common.Model;
using Common.ModelCSV;
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
            return String.Format("SELECT * FROM {0};\n", obj);
        }
        public static string SelectFirmIdBuilder(string firmName)
        {
            return String.Format("SELECT Id " +
                "                 FROM Firm " +
                "                 WHERE Name = \'{0}\';\n", 
                                  firmName);
        }
        #endregion

        #region Insert
        public static string InsertFirmBuilder(Firm firm)
        {
            return String.Format("INSERT INTO Firm (Name) " +
                "                 VALUES (\'{0}\');\n", 
                                  firm.Name);
        }
        public static string InsertDepartmentBuilder(Department department)
        {
            return String.Format("INSERT INTO Department (Id, Name) " +
                "                 VALUES {0}, \'{1}\';\n", 
                                  department.Id, 
                                  department.Name);
        }
        public static string InsertEmployeeBuilder(Employee employee)
        {
            return String.Format("INSERT INTO Employee (FirstName, LastName, DateOfBirth, JMBG, DeservesRaise, Email) " +
                "                 VALUES \'{0}\', \'{1}\', \'{2}\', {3}, {4},\'{5}\';\n",
                                  employee.FirstName,
                                  employee.LastName,
                                  employee.DateOfBirth.ToString("yyyyMMdd"),
                                  employee.JMBG,
                                  Convert.ToInt32(employee.DeservesRaise),
                                  employee.Email);
        }
        public static string InsertWorkingBuilder(int firmId, int departmentId, long employeeId)
        {
            return String.Format("INSERT INTO Working (FirmId, DepartmentId, EmployeeId)" +
                                 "VALUES {0}, {1}, {2};\n",
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
        public static string DeleteFirmBuilder(Firm firm)
        {
            return String.Format("DELETE FROM Firm " +
                                 "WHERE Id = {0};\n",
                                 firm.Id);
        }

        public static string DeleteDepartmentBuilder(Department department)
        {
            return String.Format("DELETE FROM Department " +
                                 "WHERE Id = {0};\n",
                                  department.Id);
        }

        public static string DeleteEmployeeBuilder(Employee employee)
        {
            return String.Format("DELETE FROM Employee " +
                                 "WHERE JMBG = {0};\n",
                                  employee.JMBG);
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
