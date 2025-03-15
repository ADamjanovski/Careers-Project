using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProject.Models.Helpers
{
    public class UserApplicationsViewModel
    {
        public int personId { get; set; }
        public List<SignedUserApplication> Applications { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}