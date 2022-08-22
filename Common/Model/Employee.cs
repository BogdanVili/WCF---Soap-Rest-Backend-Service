using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [DataContract]
    public class Employee
    {
        private string firstName;
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;
        [DataMember]
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

        private string dateOfBirthString;
        [DataMember(Name = "DateOfBirth")]
        public string DateOfBirthString
        {
            get { return dateOfBirthString; }
            set { dateOfBirthString = value; }
        }


        private long jmbg;
        [DataMember]
        public long JMBG
        {
            get { return jmbg; }
            set { jmbg = value; }
        }

        private bool deservesRaise;
        [DataMember]
        public bool DeservesRaise
        {
            get { return deservesRaise; }
            set { deservesRaise = value; }
        }

        private string email;
        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public Employee() 
        {
            
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
