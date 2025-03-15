using PersonalProject.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class SearchBar
    {
        public string Title { get; set; }
        public string City { get; set; }
        public int SalaryFrom { get; set; }
        public int SalaryTo { get; set; }
        public int? ChosenSalary { get; set; }
        public JobEnums.JobTypes? Type { get; set; }

        public JobEnums.JobCondition? Condition { get; set; }
        public List<Company> Company { get; set; }
        public List<int> CompanyIds { get; set; }
        public List<Category> Category { get; set; }
        public List<string> CategoryIds { get; set; }


        public SearchBar() { 
        
        }
    }
}