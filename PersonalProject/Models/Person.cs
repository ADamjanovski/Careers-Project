using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class Person:CustomUser
    {
        public string LastName { get; set; }
        public List<SignedUserApplication> Applications { get; set; }
        public Person(string FirstName, string LastName, Address address,  string LinkedIn = null) : base(FirstName, address, LinkedIn)
        {
            this.LastName = LastName;
            this.Applications = new List<SignedUserApplication>();
        }
        public Person() { }
    }
}