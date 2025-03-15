using PersonalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalProject.Models.Helpers
{
    public static class SearchHelper
    {
        public static IQueryable<JobAdvertisment> ApplyFilters(IQueryable<JobAdvertisment> query, SearchBar searchBar)
        {
            if (!string.IsNullOrEmpty(searchBar.Title))
                query = query.Where(j => j.Title.Contains(searchBar.Title));

            if (!string.IsNullOrEmpty(searchBar.City))
                query = query.Where(j => j.City.Contains(searchBar.City));

            if (searchBar.ChosenSalary!=null && searchBar.ChosenSalary > 0)
                query= query.Where(j => j.SalaryFrom>=searchBar.ChosenSalary || j.SalaryTo<=searchBar.ChosenSalary);
                

            if (searchBar.Type != null)
                query = query.Where(j => j.Type == searchBar.Type);

            if (searchBar.Condition != null)
                query = query.Where(j => j.Condition == searchBar.Condition);

            if (searchBar.CompanyIds?.Any() == true)
                query = query.Where(j => searchBar.CompanyIds.Contains(j.Company.Id));

            if (searchBar.CategoryIds?.Any() == true)
                query = query.Where(j => searchBar.CategoryIds.Contains(j.Category.Name));
            query = query.Where(j => j.ActiveTill > DateTime.Now);
            return query;
        }
    }
}