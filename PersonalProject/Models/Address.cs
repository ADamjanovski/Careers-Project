using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public Address(string Street, string City, string PostalCode)
        {
            this.Street = Street;
            this.City = City;
            this.PostalCode = PostalCode;
        }
        public Address() { }
        public override string ToString()
        {
            return $"{Street}, {City}, {PostalCode}";
        }
    }
}