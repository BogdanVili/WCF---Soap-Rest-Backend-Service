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
    public class EmployeeRequest
    {
        private string firstName;
        [DataMember(Name = "FirstName")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;
        [DataMember(Name = "LastName")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string dateOfBirthString;
        [DataMember(Name = "DateOfBirth")]
        public string DateOfBirthString
        {
            get { return dateOfBirthString; }
            set { dateOfBirthString = value; }
        }

        private long jmbg;
        [DataMember(Name = "JMBG")]
        public long JMBG
        {
            get { return jmbg; }
            set { jmbg = value; }
        }

        private bool deservesRaise;
        [DataMember(Name = "DeservesRaise")]
        public bool DeservesRaise
        {
            get { return deservesRaise; }
            set { deservesRaise = value; }
        }

        private string email;
        [DataMember(Name = "Email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public EmployeeRequest()
        {

        }

        public EmployeeRequest(string firstName, string lastName, string dateOfBirthString, long jMBG, bool deservesRaise, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirthString = dateOfBirthString;
            JMBG = jMBG;
            DeservesRaise = deservesRaise;
            Email = email;
        }

        public Employee ConvertModelRequestToModel()
        {
            return new Employee(this.FirstName,
                                this.LastName,
                                DateTime.ParseExact(this.DateOfBirthString, "yyyy-MM-dd", null),
                                this.JMBG,
                                this.DeservesRaise,
                                this.Email);
        }
    }
}
