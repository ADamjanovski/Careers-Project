using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class SignedUserApplication : JobApplication
    {
        [Required]
        [ForeignKey("ForeignKeyPerson")]
        public virtual Person Person { get; set; }

        [Required]
        public int ForeignKeyPerson { get; set; }
        public SignedUserApplication(string CV, JobAdvertisment jobAdvertisment, Person person) : base(CV, jobAdvertisment)
        {


            this.Person = person;
        }
        public SignedUserApplication()
        {

        }
    }
}