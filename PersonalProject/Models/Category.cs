using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class Category
    {
        
        [Key]
        [Display(Name="Category Name")]
        [Required]
        public string Name { get; set; }
        public List<JobAdvertisment> jobAdvertisments { get; set; }
        public Category(string name) { this.Name = name; jobAdvertisments = new List<JobAdvertisment>(); }

        public Category()
        {
            jobAdvertisments = new List<JobAdvertisment>();
        }
    }
}