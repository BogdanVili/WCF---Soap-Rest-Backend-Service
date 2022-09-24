using Common;
using Common.Model;
using Common.ModelCSV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServer
{
    public class Database
    {
        private static Database databaseInstance;
        public string connectionString = "Data Source = DESKTOP-2TUI0SB\\SQLEXPRESS; Initial Catalog = Schneider_Zadatak_1; Integrated Security = True";
        public Validation validation;

        private Database()
        {
            Connect();
            ReadModels();
            validation = new Validation(this);
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

        private bool executeNonQuery(string _query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Error Generated. Details: " + e.ToString());
                    connection.Close();
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }

            return true;
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
            string _query = "";
            _query += SqlQueryBuilder.SelectAll(tableName);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
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
                                    Collections.workings.Add(new Working((int)reader["DepartmentId"],
                                                                         (long)reader["EmployeeId"],
                                                                         (int)reader["FirmId"]));
                                    break;
                                case "Firm":
                                    Collections.firms.Add(new Firm(reader["Name"].ToString().Trim(),
                                                                   (int)reader["Id"]));
                                    break;
                                case "Department":
                                    Collections.departments.Add(new Department(reader["Name"].ToString().Trim(),
                                                                               (int)reader["Id"]));
                                    break;
                                case "Employee":
                                    Collections.employees.Add(new Employee(reader["FirstName"].ToString().Trim(),
                                                                           reader["LastName"].ToString().Trim(),
                                                                           DateTime.Parse(reader["DateOfBirth"].ToString()),
                                                                           (long)reader["JMBG"],
                                                                           (bool)reader["DeservesRaise"],
                                                                           reader["Email"].ToString().Trim()));
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
                    Console.WriteLine("Reading " + tableName + " Successful");
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

        public string AddWorker(Firm firm, Department department, Employee employee)
        {
            string _query = AddQueryConstructor(firm, department, employee);

            if (_query == "")
            {
                return "Adding Failed\n";
            }

            bool _successfulExecution = executeNonQuery(_query);

            if (_successfulExecution)
            {
                Console.WriteLine($"Inserted Firm - {firm.Id}; Department - {department.Id}; Employee - {employee.JMBG}\n");
            }
            else
            {
                return "Adding Failed\n";
            }

            UpdateFirmId(firm);

            AddWorking(firm.Id, department.Id, employee.JMBG);

            CollectionsAddUpdater(firm, department, employee);

            return "Added Successfully\n";
        }

        private string AddQueryConstructor(Firm firm, Department department, Employee employee)
        {
            string returnQuery = "";

            returnQuery += SqlQueryBuilder.InsertEmployeeBuilder(employee);

            Firm _firm = null;
            Department _department = null;

            if (validation.CheckIfFirmExists(firm.Name))
            {
                _firm = GetFirmById(firm.Id);
            }    

            if (validation.CheckIfDepartmentExistsInFirm(firm.Name, department.Name))
            {
                _department = GetDepartmentById(department.Id);
            } 

            if (_firm != null && _department == null)
            {
                returnQuery += SqlQueryBuilder.InsertDepartmentBuilder(department);
            }

            if (_firm == null)
            {
                returnQuery += SqlQueryBuilder.InsertDepartmentBuilder(department);
                returnQuery += SqlQueryBuilder.InsertFirmBuilder(firm);
            }

            return returnQuery;
        }

        private void CollectionsAddUpdater(Firm firm, Department department, Employee employee)
        {

            Firm _firm = Collections.firms.Find(f => f.Name == firm.Name);
            Department _department = null;

            if (_firm != null)
            {
                _department = _firm.Departments.Find(d => d.Id == department.Id);
            }

            if (_firm != null && _department != null)
            {
                _department.Employees.Add(employee);
                Collections.employees.Add(employee);

                Collections.workings.Add(new Working(department.Id, employee.JMBG, firm.Id));
            }

            if (_firm != null && _department == null)
            {
                _firm.Departments.Add(department);
                Collections.departments.Add(department);

                _firm.Departments.Find(d => d.Id == department.Id).Employees.Add(employee);
                Collections.employees.Add(employee);

                Collections.workings.Add(new Working(department.Id, employee.JMBG, firm.Id));
            }

            if (_firm == null)
            {
                firm.Departments.Add(department);
                Collections.departments.Add(department);

                firm.Departments.Find(d => d.Id == department.Id).Employees.Add(employee);
                Collections.employees.Add(employee);

                Collections.firms.Add(firm);

                Collections.workings.Add(new Working(department.Id, employee.JMBG, firm.Id));
            }
        }

        public void UpdateFirmId(Firm firm)
        {
            string _query = SqlQueryBuilder.SelectFirmIdBuilder(firm.Name);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
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
                            firm.Id = (int)reader["Id"];
                        }
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

        private void AddWorking(int firmId, int departmentId, long employeeId)
        {
            string _query = SqlQueryBuilder.InsertWorkingBuilder(firmId, departmentId, employeeId);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Error Generated. Details: " + e.ToString());
                    connection.Close();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region Update

        public string UpdateWorker(Firm firm, Department department, Employee employee)
        {
            string _query = UpdateQueryConstructor(firm, department, employee);

            if (_query == "")
            {
                return "Updating failed\n";
            }

            bool _successfulExecution = executeNonQuery(_query);

            if (_successfulExecution)
            {
                Console.WriteLine("Updated Successfully\n");

            }
            else
            {
                return "Updating failed\n";
            }

            CollectionsUpdateUpdater(firm, department, employee);
            
            return "Updated Successfully\n";
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

        private void CollectionsUpdateUpdater(Firm firm, Department department, Employee employee)
        {
            //if(!Collections.firms.Contains(firm))
            //{
            //    Firm _firm = Collections.firms.Find(f => f.Name == firm.Name);
            //    int _firmIndex = Collections.firms.IndexOf(_firm);
            //    firm.Departments = _firm.Departments;
            //    Collections.firms[_firmIndex] = firm;
            //}

            if (!Collections.departments.Contains(department))
            {
                Department _department = Collections.departments.Find(d => d.Id == department.Id);
                int _departmentIndex = Collections.departments.IndexOf(_department);
                department.Employees = _department.Employees;
                Collections.departments[_departmentIndex] = department;
            }

            if(!Collections.employees.Contains(employee))
            {
                Employee _employee = Collections.employees.Find(e => e.JMBG == employee.JMBG);
                int _employeeIndex = Collections.employees.IndexOf(_employee);
                Collections.employees[_employeeIndex] = employee;
            }
        }

        #endregion

        #region Delete

        public string DeleteWorker(Firm firm, Department department, Employee employee)
        {
            string _query = DeleteQueryConstructor(firm, department, employee);

            if (_query == "")
            {
                return "Deleting Failed\n";
            }

            bool _successfulExecution = executeNonQuery(_query);

            if (_successfulExecution)
            {
                Console.WriteLine("Deleted Successfully\n");
            }
            else
            {
                return "Deleting Failed\n";
            }

            CollectionsDeleteUpdater(firm, department, employee);

            return "Deleted Successfully\n";
        }

        private string DeleteQueryConstructor(Firm firm, Department department, Employee employee)
        {
            string returnQuery = "";

            if(firm != null)
            {
                foreach(Working working in Collections.workings)
                {
                    if(working.FirmId == firm.Id)
                    {
                        returnQuery += SqlQueryBuilder.DeleteWorkingBuilder(working);
                    }
                }

                Firm _firm = Collections.firms.Find(f => f.Name == firm.Name);
                foreach(Department _department in _firm.Departments)
                {
                    foreach(Employee _employee in _department.Employees)
                    {
                        returnQuery += SqlQueryBuilder.DeleteEmployeeBuilder(_employee);
                    }
                    returnQuery += SqlQueryBuilder.DeleteDepartmentBuilder(_department);
                }
                returnQuery += SqlQueryBuilder.DeleteFirmBuilder(firm);
            }

            if (department != null)
            {
                foreach (Working _working in Collections.workings)
                {
                    if (_working.DepartmentId == department.Id)
                    {
                        returnQuery += SqlQueryBuilder.DeleteWorkingBuilder(_working);
                    }
                }

                Department _department = Collections.departments.Find(d => d.Id == department.Id);
                foreach(Employee _employee in _department.Employees)
                {
                    returnQuery += SqlQueryBuilder.DeleteEmployeeBuilder(_employee);
                }
                returnQuery += SqlQueryBuilder.DeleteDepartmentBuilder(department);
            }

            if (employee != null)
            {
                foreach (Working working in Collections.workings)
                {
                    if (working.EmployeeId == employee.JMBG)
                    {
                        returnQuery += SqlQueryBuilder.DeleteWorkingBuilder(working);
                    }
                }

                returnQuery += SqlQueryBuilder.DeleteEmployeeBuilder(employee);
            }

            return returnQuery;
        }

        private void CollectionsDeleteUpdater(Firm firm, Department department, Employee employee)
        {
            if (firm != null)
            {
                foreach(Working _working in Collections.workings)
                {
                    if(_working.FirmId == firm.Id)
                    {
                        Collections.employees.RemoveAll(e => e.JMBG == _working.EmployeeId);

                        Department _department = Collections.departments.Find(d => d.Id == _working.DepartmentId);
                        if(_department != null)
                        {
                            Collections.departments.Remove(_department);
                        }

                        Firm _firm = Collections.firms.Find(f => f.Id == _working.FirmId);
                        if(_firm != null)
                        {
                            Collections.firms.Remove(_firm);
                        }
                    }
                }

                Collections.workings.RemoveAll(w => w.FirmId == firm.Id);
            }

            if (department != null)
            {
                foreach (Working _working in Collections.workings)
                {
                    if(_working.DepartmentId == department.Id)
                    {
                        Collections.employees.RemoveAll(e => e.JMBG == _working.EmployeeId);

                        Department _department = Collections.departments.Find(d => d.Id == _working.DepartmentId);
                        if (_department != null)
                        {
                            Collections.departments.Remove(_department);
                        }
                    }
                }

                Collections.workings.RemoveAll(w => w.DepartmentId == department.Id);
            }

            if (employee != null)
            {
                Collections.employees.Remove(employee);
                Collections.workings.RemoveAll(w => w.EmployeeId == employee.JMBG);
            }
        }

        #endregion

        #region UpdateEmployeeData

        public void UpdateEmployeeData(EmployeeUpdateData employeeUpdateData)
        {
            string _query = UpdateEmployeeDataQueryConstructor(employeeUpdateData);

            if (_query == "")
            {
                return;
            }

            executeNonQuery(_query);

            CollectionsUpdateEmployeeDataUpdater(employeeUpdateData);
        }

        private string UpdateEmployeeDataQueryConstructor(EmployeeUpdateData employeeUpdateData)
        {
            string returnQuery = "";

            if (Collections.employees.Any(e => e.JMBG == employeeUpdateData.JMBG))
            {
                returnQuery += SqlQueryBuilder.UpdateEmployeeDataBuilder(employeeUpdateData);
            }

            return returnQuery;
        }

        private void CollectionsUpdateEmployeeDataUpdater(EmployeeUpdateData employeeUpdateData)
        {
            if (Collections.employees.Any(e => e.JMBG == employeeUpdateData.JMBG))
            {
                Employee _employee = Collections.employees.Find(e => e.JMBG == employeeUpdateData.JMBG);
                int _employeeIndex = Collections.employees.IndexOf(_employee);
                Collections.employees[_employeeIndex].DeservesRaise = employeeUpdateData.DeservesRaise;
                Collections.employees[_employeeIndex].Email = employeeUpdateData.Email;
            }
        }

        #endregion

        private Firm GetFirmById(int firmId)
        {
            string _query = SqlQueryBuilder.SelectFirmById(firmId);

            Firm returnFirm = null; 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        returnFirm = new Firm(reader["Name"].ToString().Trim(),
                                              (int)reader["Id"]);
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

            return returnFirm;
        }

        private Department GetDepartmentById(int departmentId)
        {
            string _query = SqlQueryBuilder.SelectDepartmentById(departmentId);

            Department returnDepartment = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        returnDepartment = new Department(reader["Name"].ToString().Trim(),
                                                          (int)reader["Id"]);
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

            return returnDepartment;
        }

        private Employee GetEmployeeById(long employeeId)
        {
            string _query = SqlQueryBuilder.SelectEmployeeById(employeeId);

            Employee returnEmployee = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        returnEmployee = new Employee(reader["FirstName"].ToString().Trim(),
                                                      reader["LastName"].ToString().Trim(),
                                                      DateTime.Parse(reader["DateOfBirth"].ToString()),
                                                      (long)reader["JMBG"],
                                                      (bool)reader["DeservesRaise"],
                                                      reader["Email"].ToString().Trim());
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

            return returnEmployee;
        }
    }
}

