using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.ModelRequest
{
    [DataContract]
    public class DeleteParameters
    {
        private string firmName;

        [DataMember]
        public string FirmName
        {
            get { return firmName; }
            set { firmName = value; }
        }

        private int departmentId;

        [DataMember]
        public int DepartmentId
        {
            get { return departmentId; }
            set { departmentId = value; }
        }

        private long employeeJMBG;

        [DataMember]
        public long EmployeeJMBG
        {
            get { return employeeJMBG; }
            set { employeeJMBG = value; }
        }

        public DeleteParameters()
        {
        }
    }
}
