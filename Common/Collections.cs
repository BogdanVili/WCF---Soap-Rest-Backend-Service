using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Collections
    {
        public static List<Firm> firms = new List<Firm>();
        public static List<Department> departments = new List<Department>();
        public static List<Employee> employees = new List<Employee>();
        public static List<Working> workings = new List<Working>();

        public static int GetCurrentFirmId(string firmName)
        {
            if(firms.Count == 0)
                return 1;

            Firm _firm = firms.Find(f => f.Name == firmName);
            if (_firm != null)
                return _firm.Id;

            return firms.Max(f => f.Id) + 1;
        }
    }
}
