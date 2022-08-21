using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Firm
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private List<Department> departments;

        public List<Department> Departments
        {
            get { return departments; }
            set { departments = value; }
        }

        public Firm(string name, int id)
        {
            Name = name;
            Id = id;
            departments = new List<Department>();
        }

        public override bool Equals(object obj)
        {
            return obj is Firm firm &&
                   Name == firm.Name &&
                   Id == firm.Id;
        }
    }
}
