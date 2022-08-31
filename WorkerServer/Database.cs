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
    public class Database
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
        public string AddWorker(Firm firm, Department department, Employee employee)
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
                        Console.WriteLine($"Inserted Firm - {firm.Id}; Department - {department.Id}; Employee - {employee.JMBG}\n");
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Error Generated. Details: " + e.ToString());
                        connection.Close();
                        return "Adding Failed\n";
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                CollectionsAddUpdater(firm, department, employee);
            }

            return "Added Successfully\n";
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

        private void CollectionsAddUpdater(Firm firm, Department department, Employee employee)
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
        #endregion

        #region Update
        public string UpdateWorker(Firm firm, Department department, Employee employee)
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
                        Console.WriteLine("Updated Successfully\n");
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Error Generated. Details: " + e.ToString());
                        connection.Close();
                        return "Updating failed\n";
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                CollectionsUpdateUpdater(firm, department, employee);
            }

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
        }
        #endregion

        #region Delete
        public string DeleteWorker(Firm firm, Department department, Employee employee)
        {
            string _query = DeleteQueryConstructor(firm, department, employee);

            if (_query != "")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(_query, connection);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Deleted Successfully");
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Error Generated. Details: " + e.ToString());
                        connection.Close();
                        return "Deleting Failed";
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                CollectionsDeleteUpdater(firm, department, employee);
            }

            return "Deleted Successfully";
        }

        private string DeleteQueryConstructor(Firm firm, Department department, Employee employee)
        {
            string returnQuery = "";

            if(firm != null)
            {
                //delete all working with firm
                //foreach department and foreach employee delete all employees, than delete department, then delete firm
                foreach(Working working in Collections.workings)
                {
                    if(working.FirmId == firm.Id)
                    {
                        returnQuery += SqlQueryBuilder.DeleteWorkingBuilder(working);
                    }
                }

                Firm _firm = Collections.firms.Find(f => f.Id == firm.Id);
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
                //delete all working with department
                //foreach employees delete all employees, than delete department
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
                //delete working with that employee
                //delete employee
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
    }
}

