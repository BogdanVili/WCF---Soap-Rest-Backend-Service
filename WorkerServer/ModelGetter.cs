using Common.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WorkerServer
{
    internal class ModelGetter
    {
        Database database;

        public ModelGetter(Database database)
        {
            this.database = database;
        }

        public Department GetDepartmentById(int departmentId)
        {
            string _query = SqlQueryBuilder.SelectDepartmentById(departmentId);

            Department returnDepartment = null;

            using (SqlConnection connection = new SqlConnection(database.connectionString))
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

        public Employee GetEmployeeById(long employeeId)
        {
            string _query = SqlQueryBuilder.SelectEmployeeById(employeeId);

            Employee returnEmployee = null;

            using (SqlConnection connection = new SqlConnection(database.connectionString))
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

        public Firm GetFirmByName(string firmName)
        {
            string _query = SqlQueryBuilder.SelectFirmByName(firmName);

            Firm returnFirm = null;

            using (SqlConnection connection = new SqlConnection(database.connectionString))
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

        public List<Working> GetWorkingsByDepartmentId(int departmentId)
        {
            string _query = SqlQueryBuilder.SelectWorkingByDepartmentId(departmentId);

            List<Working> returnWorkings = new List<Working>();

            using (SqlConnection connection = new SqlConnection(database.connectionString))
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

        public List<Working> GetWorkingsByEmployeeId(long employeeId)
        {
            string _query = SqlQueryBuilder.SelectWorkingByEmployeeId(employeeId);

            List<Working> returnWorkings = new List<Working>();

            using (SqlConnection connection = new SqlConnection(database.connectionString))
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

        public List<Working> GetWorkingsByFirmId(int firmId)
        {
            string _query = SqlQueryBuilder.SelectWorkingByFirmId(firmId);

            List<Working> returnWorkings = new List<Working>();

            using (SqlConnection connection = new SqlConnection(database.connectionString))
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
    }
}