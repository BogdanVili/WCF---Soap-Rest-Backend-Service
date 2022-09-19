using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.ModelRequest
{
    [DataContract]
    public class DepartmentRequest
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

        public DepartmentRequest()
        {

        }
        public DepartmentRequest(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public Department ConvertModelRequestToModel()
        {
            return new Department(this.Name, 
                                  this.Id);
        }
    }
}
