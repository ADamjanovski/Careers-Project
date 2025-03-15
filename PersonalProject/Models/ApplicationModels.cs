using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class SignedApplicationViewModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]

        public HttpPostedFileBase CV {  get; set; }
        [Required]
        public int JobAdvertismentId { get; set; }
    }

    public class UnsignedApplicationViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Required]

        public HttpPostedFileBase CV { get; set; }
        [Required]
        public int JobAdvertismentId { get; set; }
        public string LinkedIn { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }

    public class JobAdvertismentViewModel
    {
            [Required]
            public string Title { get; set; }

            [Required]
            public string Description { get; set; }
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
            public int CompanyId { get; set; }

            public List<JobApplication> Applications { get; set; }
            public Category Category { get; set; }
            [Required]
            public string City { get; set; }
            
        }
    }
