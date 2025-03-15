using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CV { get; set; }

        [Required]
        public JobAdvertisment jobAdvertisment { get; set; }

        public enum Status
        {
            ACCEPTED,
            PENDING,
            REJECTED
        }
        
        public Status JobStatus { get; set; }
        public JobApplication(string CV, JobAdvertisment jobAdvertisment)
        {
            this.CV = CV;
            this.jobAdvertisment = jobAdvertisment;
            JobStatus = Status.PENDING;
        }
        public JobApplication() {
            JobStatus = Status.PENDING;
        }
    }
}