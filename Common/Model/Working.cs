using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Working
    {
        private int firmId;

        public int FirmId
        {
            get { return firmId; }
            set { firmId = value; }
        }

        private int departmentId;

        public int DepartmentId
        {
            get { return departmentId; }
            set { departmentId = value; }
        }

        private long employeeId;

        public long EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; }
        }

        public Working(int departmentId, long employeeId, int firmId)
        {
            FirmId = firmId;
            DepartmentId = departmentId;
            EmployeeId = employeeId;
        }
    }
}
