using PersonalProject.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProject.Models
{
    public class AdvertismentAndSearchModel
    {
        public List<JobAdvertisment> JobAdvertisment { get; set; }
        public SearchBar SearchBar { get; set; }
        public PaginationInfo PaginationInfo { get; set; }

        public AdvertismentAndSearchModel() { }
    }
}