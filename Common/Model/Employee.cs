using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Employee
    {
        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private DateTime dateOfBirth;

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }

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

        public Employee(string firstName, string lastName, DateTime dateOfBirth, long jMBG, bool deservesRaise, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            JMBG = jMBG;
            DeservesRaise = deservesRaise;
            Email = email;
        }
    }
}
