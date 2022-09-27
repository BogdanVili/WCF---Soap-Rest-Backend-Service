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

            firm.Id = GetFirmId(firm.Name);

            AddWorking(firm.Id, department.Id, employee.JMBG);

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
                _firm = GetFirmByName(firm.Name);
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

        public int GetFirmId(string firmName)
        {
            string _query = SqlQueryBuilder.SelectFirmIdBuilder(firmName);

            int returnFirmId = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable schemaTable = reader.GetSchemaTable();

                    if (reader.Read())
                    {
                        returnFirmId = (int)reader["Id"];
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

            return returnFirmId;
        }

        private void AddWorking(int firmId, int departmentId, long employeeId)
        {
            string _query = SqlQueryBuilder.InsertWorkingBuilder(firmId, departmentId, employeeId);

            bool _successfulExecution = executeNonQuery(_query);

            return;
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
            
            return "Updated Successfully\n";
        }

        private string UpdateQueryConstructor(Firm firm, Department department, Employee employee)
        {
            string returnQuery = "";

            //returnQuery += SqlQueryBuilder.UpdateFirmBuilder(firm);
            
            returnQuery += SqlQueryBuilder.UpdateDepartmentBuilder(department);

            returnQuery += SqlQueryBuilder.UpdateEmployeeBuilder(employee);
            
            return returnQuery;
        }

        #endregion

        #region Delete

        public string DeleteWorker(string firmName, int departmentId, long employeeId)
        {
            string _query = DeleteQueryConstructor(firmName, departmentId, employeeId);

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

            return "Deleted Successfully\n";
        }

        private string DeleteQueryConstructor(string firmName, int departmentId, long employeeId)
        {
            string returnQuery = "";

            if(firmName != null)
            {
                Firm _firm = GetFirmByName(firmName);

                List<Working> _workings = GetWorkingsByFirmId(_firm.Id);

                foreach(Working _working in _workings)
                {
                    returnQuery += SqlQueryBuilder.DeleteWorkingBuilder(_working);
                }

                foreach(int _firmId in _workings.Select(w => w.FirmId).Distinct())
                {
                    returnQuery += SqlQueryBuilder.DeleteFirmBuilder(_firmId);
                }

                foreach(int _departmentId in _workings.Select(w => w.DepartmentId).Distinct())
                {
                    returnQuery += SqlQueryBuilder.DeleteDepartmentBuilder(_departmentId);
                }

                foreach(int _employeeId in _workings.Select(w => w.EmployeeId).Distinct())
                {
                    returnQuery += SqlQueryBuilder.DeleteEmployeeBuilder(_employeeId);
                }
            }

            if (departmentId > 0)
            {
                Department _department = GetDepartmentById(departmentId);

                List<Working> _workings = GetWorkingsByDepartmentId(_department.Id);

                foreach (Working _working in _workings)
                {
                    returnQuery += SqlQueryBuilder.DeleteWorkingBuilder(_working);
                }

                foreach (int _departmentId in _workings.Select(w => w.DepartmentId).Distinct())
                {
                    returnQuery += SqlQueryBuilder.DeleteDepartmentBuilder(_departmentId);
                }

                foreach (int _employeeId in _workings.Select(w => w.EmployeeId).Distinct())
                {
                    returnQuery += SqlQueryBuilder.DeleteEmployeeBuilder(_employeeId);
                }
            }

            if (employeeId > 0)
            {
                Employee _employee = GetEmployeeById(employeeId);
                List<Working> _workings = GetWorkingsByEmployeeId(_employee.JMBG);

                foreach (Working _working in _workings)
                {
                    returnQuery += SqlQueryBuilder.DeleteWorkingBuilder(_working);
                }

                foreach (int _employeeId in _workings.Select(w => w.EmployeeId).Distinct())
                {
                    returnQuery += SqlQueryBuilder.DeleteEmployeeBuilder(_employeeId);
                }
            }

            return returnQuery;
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
        }

        private string UpdateEmployeeDataQueryConstructor(EmployeeUpdateData employeeUpdateData)
        {
            string returnQuery = "";
            
            if (validation.CheckIfEmployeeExists(employeeUpdateData.JMBG))
            {
                returnQuery += SqlQueryBuilder.UpdateEmployeeDataBuilder(employeeUpdateData);
            }

            return returnQuery;
        }

        #endregion

        private Firm GetFirmByName(string firmName)
        {
            string _query = SqlQueryBuilder.SelectFirmByName(firmName);

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

        private List<Working> GetWorkingsByFirmId(int firmId)
        {
            string _query = SqlQueryBuilder.SelectWorkingByFirmId(firmId);

            List<Working> returnWorkings = new List<Working>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while(reader.HasRows)
                    {
                        if(reader.Read())
                        {
                            returnWorkings.Add(new Working((int)reader["DepartmentId"],
                                                           (long)reader["EmployeeId"],
                                                           (int)reader["FirmId"]));
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

            return returnWorkings;
        }

        private List<Working> GetWorkingsByDepartmentId(int departmentId)
        {
            string _query = SqlQueryBuilder.SelectWorkingByDepartmentId(departmentId);

            List<Working> returnWorkings = new List<Working>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            returnWorkings.Add(new Working((int)reader["DepartmentId"],
                                                           (long)reader["EmployeeId"],
                                                           (int)reader["FirmId"]));
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

            return returnWorkings;
        }

        private List<Working> GetWorkingsByEmployeeId(long employeeId)
        {
            string _query = SqlQueryBuilder.SelectWorkingByEmployeeId(employeeId);

            List<Working> returnWorkings = new List<Working>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            returnWorkings.Add(new Working((int)reader["DepartmentId"],
                                                           (long)reader["EmployeeId"],
                                                           (int)reader["FirmId"]));
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

            return returnWorkings;
        }
    }
}

