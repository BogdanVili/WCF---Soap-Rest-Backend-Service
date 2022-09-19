using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Validation
    {
        public static bool ContainsFirmName(string name)
        {
            Firm firm = Collections.firms.Find(f => f.Name == name);

            if (firm != null)
                return false;

            return true;
        }

        public static bool FirmContainsDepartmentName(Firm firm, Department department)
        {
            Firm _firm = Collections.firms.Find(f => f.Name == firm.Name);

            if (_firm == null)
                return false;

            if (_firm.Departments.Count == 0)
                return false;

            Department _department = _firm.Departments.Find(d => d.Name == department.Name);

            if (_department != null)
                if (_department.Id != department.Id)
                    return true;

            return false;
        }

        public static bool IsWorkerEmpty(Firm firm, Department department, Employee employee)
        {
            if (firm.Empty() || department.Empty() || employee.Empty())
                return true;

            return false;
        }

        public static bool ExistsInWorkings(Firm firm, Department department, Employee employee)
        {
            if (Collections.firms.Any(f => f.Id == firm.Id) &&
               Collections.departments.Any(d => d.Id == department.Id) &&
               Collections.employees.Any(e => e.JMBG == employee.JMBG))
                return true;
            return false;
        }
    }
}
