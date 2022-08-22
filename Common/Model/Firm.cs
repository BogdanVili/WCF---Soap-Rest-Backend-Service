using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [DataContract]
    public class Firm
    {
        private string name;
        [DataMember(Name = "FirmName")]
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

        public Firm() 
        {
            Departments = new List<Department>();
        }
        public Firm(string name, int id)
        {
            Name = name;
            Id = id;
            departments = new List<Department>();
        }
    }
}
