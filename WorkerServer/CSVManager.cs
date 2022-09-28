using Common.ModelCSV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Windows;

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

        public void StartThread()
        {
            Timer timer = new Timer(ReadData, null, 0, 2000);
        }

        private void ReadData(object state)
        {
            //string path = Path.GetFullPath("../../../WorkerServer/Data/" + FileName);
            string path = "C:\\Users\\bokic\\source\\repos\\SchneiderZadatak1\\WorkerServer\\Data\\" + FileName;
            FileInfo fileInfo = new FileInfo(path);
            if(IsFileInUse(fileInfo))
            {
                return;
            }

            StreamReader streamReader = new StreamReader(path);
            string _line = "";

            while ((_line = streamReader.ReadLine()) != null)
            {
                if (_line == "")
                    continue;

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

            streamReader.Close();
        }

        private static bool IsFileInUse(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }
    }
}
