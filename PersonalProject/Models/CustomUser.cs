using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class CustomUser
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LinkedIn { get; set; }
        public Address Address { get; set; }


        public CustomUser(string name, Address address,string linkedIn = null)
        {
            Name = name;
            LinkedIn = linkedIn;
            Address = address;
        }
        public CustomUser() { }

        
}
}