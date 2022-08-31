using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ModelCSV
{
    public class EmployeeUpdateData
    {
        private long jmbg;
        public long JMBG
        {
            get { return jmbg; }
            set { jmbg = value; }
        }

        private bool deservesRaise;
        public bool DeservesRaise
        {
            get { return deservesRaise; }
            set { deservesRaise = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public EmployeeUpdateData(long jMBG, bool deservesRaise, string email)
        {
            JMBG = jMBG;
            DeservesRaise = deservesRaise;
            Email = email;
        }
    }
}
