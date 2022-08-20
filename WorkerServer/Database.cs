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

        List<Firm> firms = new List<Firm>();

        #region Add
        public void AddWorker(Firm firm, Department department, Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string _query = "";

                if (firms.Contains(firm))
                {
                    Firm _firm = firms.Find(f => f.Id == firm.Id);
                
                    if(_firm.Departments.Contains(department))
                    {
                        Department _department = _firm.Departments.Find(d => d.Id == department.Id);

                        _department.Employees.Add(employee);
                    }
                    else
                    {
                        _firm.Departments.Find(d => d.Id == department.Id).Employees.Add(employee);
                        _firm.Departments.Add(department);
                        _query += SqlQueryBuilder.InsertDepartmentBuilder(department);
                    }
                }
                else
                {
                    firm.Departments.Add(department);
                    firm.Departments.Find(d => d.Id == department.Id).Employees.Add(employee);
                    firms.Add(firm);
                    _query += SqlQueryBuilder.InsertFirmBuilder(firm);
                    _query += SqlQueryBuilder.InsertDepartmentBuilder(department);
                }

                _query += SqlQueryBuilder.InsertEmployeeBuilder(employee);
                _query += SqlQueryBuilder.InsertWorkingBuilder(firm.Id, department.Id, employee.JMBG);

                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Records Inserted Successfully");
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



        #endregion

        public void UpdateWorker()
        {

        }

        public void ReadWorkers()
        {
            //First read Working, then Firm, then Department, then Employee, put all into lists
            //Take Working find Firm add Department add Worker
            //After adding department check for id so as to not add again
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string _query = "";
                _query += SqlQueryBuilder.SelectAll("Firm");

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
                            Console.WriteLine($"{reader.GetValue(0)},{reader.GetValue(1)}");
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
    }
}
