using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServer
{
    class Database
    {
        private static Database databaseInstance;
        string connectionString = "Data Source = DESKTOP-2TUI0SB\\SQLEXPRESS; Initial Catalog = Schneider_Zadatak_1; Integrated Security = True";

        private Database()
        {
            Connect();
        }

        public static Database GetInstance()
        {
            if(databaseInstance == null)
            {
                databaseInstance = new Database();
            }
            return databaseInstance;
        }

        private void Connect()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Console.WriteLine("State: {0}", connection.State);
                Console.WriteLine("ConnectionString: {0}", connection.ConnectionString);

                connection.Close();
            }
        }


        #region Read
        public void ReadModels()
        {
            ReadTable("Working");
            ReadTable("Firm");
            ReadTable("Department");
            ReadTable("Employee");

            ConnectModelsWithWorking();

            Console.WriteLine("Loading Models done.\n");
        }

        private void ReadTable(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string _query = "";
                _query += SqlQueryBuilder.SelectAll(tableName);

                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable schemaTable = reader.GetSchemaTable();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            switch (tableName)
                            {
                                case "Working":
                                    Collections.workings.Add(new Working((int)reader.GetValue(0),
                                                                         (long)reader.GetValue(1),
                                                                         (int)reader.GetValue(2)));
                                    break;
                                case "Firm":
                                    Collections.firms.Add(new Firm(reader.GetValue(0).ToString().Trim(),
                                                                   (int)reader.GetValue(1)));
                                    break;
                                case "Department":
                                    Collections.departments.Add(new Department(reader.GetValue(0).ToString().Trim(),
                                                                               (int)reader.GetValue(1)));
                                    break;
                                case "Employee":
                                    Collections.employees.Add(new Employee(reader.GetValue(0).ToString().Trim(),
                                                                           reader.GetValue(1).ToString().Trim(),
                                                                           DateTime.Parse(reader.GetValue(2).ToString()),
                                                                           (long)reader.GetValue(3),
                                                                           (bool)reader.GetValue(4),
                                                                           reader.GetValue(5).ToString().Trim()));
                                    break;

                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Error Generated. Details: " + e.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void ConnectModelsWithWorking()
        {
            Firm _currentFirm;
            Department _currentDepartment;
            Employee _currentEmployee;

            foreach (Working working in Collections.workings)
            {
                _currentFirm = Collections.firms.Find(f => f.Id == working.FirmId);
                _currentDepartment = Collections.departments.Find(d => d.Id == working.DepartmentId);
                _currentEmployee = Collections.employees.Find(e => e.JMBG == working.EmployeeId);
                if (!_currentFirm.Departments.Contains(_currentDepartment))
                {
                    _currentFirm.Departments.Add(_currentDepartment);
                }

                if(!_currentFirm.Departments.Find(d => d.Id == working.DepartmentId).Employees.Contains(_currentEmployee))
                {
                    _currentFirm.Departments.Find(d => d.Id == working.DepartmentId).Employees.Add(_currentEmployee);
                }
            }
        }
        #endregion

        #region Add
        public void AddWorker(Firm firm, Department department, Employee employee)
        {
            string _query = AddQueryConstructor(firm, department, employee);

            if (_query != "")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(_query, connection);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Inserted Successfully");
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Error Generated. Details: " + e.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                CollectionsLinker(firm, department, employee);
            }
        }

        private string AddQueryConstructor(Firm firm, Department department, Employee employee)
        {
            string returnQuery = "";

            Firm _firm = Collections.firms.Find(f => f.Id == firm.Id);
            Department _department = null;

            if (_firm != null)
            {
                _department = _firm.Departments.Find(d => d.Id == department.Id);
            }

            returnQuery += SqlQueryBuilder.InsertEmployeeBuilder(employee);

            if (_firm != null && _department == null)
            {
                returnQuery += SqlQueryBuilder.InsertDepartmentBuilder(department);
            }

            if (_firm == null)
            {
                returnQuery += SqlQueryBuilder.InsertDepartmentBuilder(department);
                returnQuery += SqlQueryBuilder.InsertFirmBuilder(firm);
            }

            returnQuery += SqlQueryBuilder.InsertWorkingBuilder(firm.Id, department.Id, employee.JMBG);

            return returnQuery;
        }

        private void CollectionsLinker(Firm firm, Department department, Employee employee)
        {

            Firm _firm = Collections.firms.Find(f => f.Id == firm.Id);
            Department _department = null;

            if (_firm != null)
            {
                _department = _firm.Departments.Find(d => d.Id == department.Id);
            }

            if (_firm != null && _department != null)
            {
                _department.Employees.Add(employee);
            }

            if (_firm != null && _department == null)
            {
                _firm.Departments.Add(department);
                _firm.Departments.Find(d => d.Id == department.Id).Employees.Add(employee);
            }

            if (_firm == null)
            {
                firm.Departments.Add(department);
                firm.Departments.Find(d => d.Id == department.Id).Employees.Add(employee);
                Collections.firms.Add(firm);
            }
        }
        #endregion

        #region Update
        public void UpdateWorker(Firm firm, Department department, Employee employee)
        {
            string _query = UpdateQueryConstructor(firm, department, employee);

            if (_query != "")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(_query, connection);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Updated Successfully");
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Error Generated. Details: " + e.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                CollectionsUpdater(firm, department, employee);
            }
        }

        private string UpdateQueryConstructor(Firm firm, Department department, Employee employee)
        {
            string returnQuery = "";
            if (!Collections.firms.Contains(firm))
            {
                returnQuery += SqlQueryBuilder.UpdateFirmBuilder(firm);
            }

            if(!Collections.departments.Contains(department))
            {
                returnQuery += SqlQueryBuilder.UpdateDepartmentBuilder(department);
            }

            if(!Collections.employees.Contains(employee))
            {
                returnQuery += SqlQueryBuilder.UpdateEmployeeBuilder(employee);
            }
            return returnQuery;
        }

        private void CollectionsUpdater(Firm firm, Department department, Employee employee)
        {
            if(!Collections.firms.Contains(firm))
            {
                Firm _firm = Collections.firms.Find(f => f.Id == firm.Id);
                int _firmIndex = Collections.firms.IndexOf(_firm);
                Collections.firms[_firmIndex] = firm;
            }

            if (!Collections.departments.Contains(department))
            {
                Department _department = Collections.departments.Find(d => d.Id == department.Id);
                int _departmentIndex = Collections.departments.IndexOf(_department);
                Collections.departments[_departmentIndex] = department;
            }

            if(!Collections.employees.Contains(employee))
            {
                Employee _employee = Collections.employees.Find(e => e.JMBG == employee.JMBG);
                int _employeeIndex = Collections.employees.IndexOf(_employee);
                Collections.employees[_employeeIndex] = employee;
            }

            ConnectModelsWithWorking();
        }
        #endregion

    }
}

