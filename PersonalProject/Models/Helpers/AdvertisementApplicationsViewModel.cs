using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProject.Models.Helpers
{
    public class AdvertisementApplicationsViewModel
    {
        public int AdvertismentId { get; set; }
        public List<JobApplication> Applications { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}