using Common.ModelCSV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace WorkerServer
{
    public class CSVManager
    {
        private static Database database = Database.GetInstance();
        private static List<EmployeeUpdateData> employeeUpdateDatas = new List<EmployeeUpdateData>();
        public static string FileName;

        public CSVManager(string fileName)
        {
            FileName = fileName;
        }

        public void ReadData()
        {
            string path = HostingEnvironment.MapPath(FileName);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string _line = "";

            while ((_line = sr.ReadLine()) != null)
            {
                string[] _fields = _line.Split(',');

                long _jmbg = long.Parse(_fields[0]);
                bool _deservesRaise = bool.Parse(_fields[1]);
                string _email = _fields[2];

                EmployeeUpdateData _employeeUpdateData = new EmployeeUpdateData(_jmbg, _deservesRaise, _email);

                if (employeeUpdateDatas.Contains(_employeeUpdateData))
                    continue;

                if (employeeUpdateDatas.Any(ed => ed.JMBG == _jmbg))
                {
                    employeeUpdateDatas.Find(e => e.JMBG == _jmbg).DeservesRaise = _deservesRaise;
                    employeeUpdateDatas.Find(e => e.JMBG == _jmbg).Email = _email;
                    database.UpdateEmployeeData(_employeeUpdateData);
                    continue;
                }

                employeeUpdateDatas.Add(new EmployeeUpdateData(_jmbg, _deservesRaise, _email));
                database.UpdateEmployeeData(_employeeUpdateData);
            }
        }

        public void UpdateEmployeeDatabase(EmployeeUpdateData employeeUpdateData)
        {

        }

        public void UpdateEmployeeCollection(EmployeeUpdateData employeeUpdateData)
        {

        }
    }
}
