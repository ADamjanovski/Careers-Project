using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class UnsignedUserApplication : JobApplication
    {
        [Required]
        [Display(Name="Full Name")]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string LinkedIn { get; set; }
        public UnsignedUserApplication(string name, string phoneNumber, string email, string linkedIn, string CV, JobAdvertisment jobAdvertisment) : base(CV, jobAdvertisment)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            LinkedIn = linkedIn;
        }
        public UnsignedUserApplication()
        {

        }
    }
}