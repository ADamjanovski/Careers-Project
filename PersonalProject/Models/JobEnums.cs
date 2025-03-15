using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class JobEnums
    {
        public enum JobTypes
        {
            FullTime,
            PartTime,
            Internship,
            Apprenticeship,
            Seasonal
        }

        public enum JobCondition
        {
            Remote,
            Hybrid,
            In_Person
        }
    }
}