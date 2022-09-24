using Common.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServer
{
    public class Validation
    {
        Database database;

        public Validation(Database database)
        {
            this.database = database;
        }

        public bool CheckIfFirmExists(string firmName)
        {
            bool returnValue = true;

            string _query = SqlQueryBuilder.FirmExists(firmName);

            using (SqlConnection connection = new SqlConnection(database.connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        returnValue = Boolean.Parse(reader["ReturnValue"].ToString());
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

            return returnValue;
        }

        public bool CheckIfDepartmentExistsInFirm(string firmName, string departmentName)
        {
            bool returnValue = true;

            string _query = SqlQueryBuilder.DepartmentNameExistsInFirm(firmName, departmentName);

            using (SqlConnection connection = new SqlConnection(database.connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        returnValue = Boolean.Parse(reader["ReturnValue"].ToString());
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

            return returnValue;
        }

        public bool CheckIfWorkingExists(int firmId, int departmentId, long employeeId)
        {
            bool returnValue = true;

            string _query = SqlQueryBuilder.WorkingExists(firmId, departmentId, employeeId);

            using (SqlConnection connection = new SqlConnection(database.connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        returnValue = Boolean.Parse(reader["ReturnValue"].ToString());
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

            return returnValue;
        }

        public bool IsWorkerEmpty(Firm firm, Department department, Employee employee)
        {
            if (firm.Empty() || department.Empty() || employee.Empty())
                return true;

            return false;
        }
    }
}
