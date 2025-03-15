using PersonalProject.Models.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class JobAdvertisment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public DateTime Created { get; set; }

        [Required]
        public DateTime ActiveTill { get; set; }
        
        public List<string> PhotoUrl { get; set; }

        [Required]
        public int SalaryFrom { get; set; }
        [Required]
        public int SalaryTo { get; set; }


        public JobEnums.JobTypes Type { get; set; }

        public JobEnums.JobCondition Condition { get; set; }
        [Required]
        public Company Company { get; set; }

        public List<JobApplication> Applications { get; set; }
        public Category Category { get; set; }
        [Required]
        public string City { get; set; }
        public JobAdvertisment(string Title, string Description, DateTime Created, DateTime ActiveTill, Category category,int salaryFrom,int salaryTo,JobEnums.JobTypes type, JobEnums.JobCondition jobConditions,string city, List<string> PhotoUrl=null)
        {
            this.Title = Title;
            this.Description = Description;
            this.Created = Created;
            this.ActiveTill = ActiveTill;
            this.PhotoUrl = PhotoUrl;
            this.Applications = new List<JobApplication>();
            Category = category;
            this.Type = type;
            this.Condition = jobConditions;
            this.SalaryFrom = salaryFrom;
            this.SalaryTo = salaryTo;
            this.City = city;
        }

        public JobAdvertisment()
        {
            Applications=new List<JobApplication>();
        }
    }
}