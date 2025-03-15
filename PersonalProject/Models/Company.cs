using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalProject.Models
{
    public class Company :CustomUser
    {
        [Required]
        [AllowHtml]
        public string Description { get; set; }

        [Required]
        public string LogoURL { get; set; }
        public List<string> PhotosURL { get; set; }
        public List<JobAdvertisment> jobAdvertisments { get; set; }
        public Company(string name,string description, string logoURL, Address address,  string linkedIn = null) : base(name, address, linkedIn)
        {
            Description = description;
            LogoURL = logoURL;
            jobAdvertisments = new List<JobAdvertisment>();
        }
        public Company() { }


    }
}