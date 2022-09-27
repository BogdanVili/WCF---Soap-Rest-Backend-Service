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
        public ModelRequestToModelConverter converter;
        internal ModelGetter modelGetter;
        internal QueryConstructor queryConstructor;

        private Database()
        {
            Connect();
            validation = new Validation(this);
            converter = new ModelRequestToModelConverter(this);
            modelGetter = new ModelGetter(this);
            queryConstructor = new QueryConstructor(validation, modelGetter);
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

        public string AddWorker(Firm firm, Department department, Employee employee)
        {
            string _query = queryConstructor.Add(firm, department, employee);

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

            firm.Id = modelGetter.GetFirmId(firm.Name);

            AddWorking(firm.Id, department.Id, employee.JMBG);

            return "Added Successfully\n";
        }

        private void AddWorking(int firmId, int departmentId, long employeeId)
        {
            string _query = SqlQueryBuilder.InsertWorkingBuilder(firmId, departmentId, employeeId);

            executeNonQuery(_query);
        }

        public string UpdateWorker(Firm firm, Department department, Employee employee)
        {
            string _query = queryConstructor.Update(firm, department, employee);

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

        public string DeleteWorker(string firmName, int departmentId, long employeeId)
        {
            string _query = queryConstructor.Delete(firmName, departmentId, employeeId);

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

        public void UpdateEmployeeData(EmployeeUpdateData employeeUpdateData)
        {
            string _query = queryConstructor.UpdateEmployeeData(employeeUpdateData);

            if (_query == "")
            {
                return;
            }

            executeNonQuery(_query);
        }
    }
}

