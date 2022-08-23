using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [DataContract]
    public class Department
    {
        private string name;
        [DataMember(Name = "DepartmentName")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int id;
        [DataMember(Name = "DepartmentId")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private List<Employee> employees;

        public List<Employee> Employees
        {
            get { return employees; }
            set { employees = value; }
        }

        public Department()
        {
            Employees = new List<Employee>();
        }
        public Department(string name, int id)
        {
            Name = name;
            Id = id;
            employees = new List<Employee>();
        }

        public bool Empty()
        {
            if (Name == "" || Name == null)
                return true;
            if (Id <= 0)
                return true;
            return false;
        }
    }
}
