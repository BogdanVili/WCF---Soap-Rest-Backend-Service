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
    public class FirmRequest
    {
        private string name;
        [DataMember(Name = "FirmName")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int id;
        [DataMember(Name = "FirmId")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public FirmRequest()
        {

        }

        public FirmRequest(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}
