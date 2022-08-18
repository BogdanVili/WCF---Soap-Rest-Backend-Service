using Common.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServer
{
    class Database
    {
        private static Database databaseInstance;

        private SqlConnection connection = new SqlConnection();

        private Database()
        {
            this.Connect();
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
            string connectionString = "Data Source = DESKTOP-2TUI0SB\\SQLEXPRESS; Initial Catalog = Schneider_Zadatak_1; Integrated Security = True";
            using (connection)
            {
                connection.ConnectionString = connectionString;

                connection.Open();

                Console.WriteLine("State: {0}", connection.State);
                Console.WriteLine("ConnectionString: {0}", connection.ConnectionString);
            }
        }

        List<Firm> firms = new List<Firm>();
        public void AddWorker(Firm firm, Department department, Employee employee)
        {
            using (connection)
            {
                if (firms.Contains(firm))
                {
                    Firm _firm = firms.Find(f => f.Id == firm.Id);
                
                    if(_firm.Departments.Contains(department))
                    {
                        Department _department = _firm.Departments.Find(d => d.Id == department.Id);

                        _department.Employees.Add(employee);
                        //add employee, plus working
                    }
                    else
                    {
                        _firm.Departments.Find(d => d.Id == department.Id).Employees.Add(employee);
                        _firm.Departments.Add(department);
                        //add department and employee, plus working
                    }
                }
                else
                {
                    firm.Departments.Add(department);
                    firm.Departments.Find(d => d.Id == department.Id).Employees.Add(employee);
                    firms.Add(firm);
                    //add firm department and employee, plus working
                } 
            }
        }

        public void UpdateWorker()
        {

        }

        public void ReadWorkers()
        {

        }
    }
}
