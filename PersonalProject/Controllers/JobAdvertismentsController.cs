using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using PersonalProject.Models.Helpers;
using PersonalProject.Models;
using System.Drawing.Printing;
using System.Web.UI;
using Ganss.Xss;

namespace PersonalProject.Controllers
{
    public class JobAdvertismentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: JobAdvertisments
        public ActionResult Index(int page = 1, int pageSize = 1)
        {
            ViewBag.IsAuthenticated = User.Identity.IsAuthenticated;

            var jobAdvertisments = db.JobAdvertisments
                .Where(job => job.ActiveTill > DateTime.Now)
                .Include(j => j.Company)
                .ToList();

            var model = BuildAdvertismentModel(jobAdvertisments, page, pageSize);
            return View(model);

        }
        [HttpPost]
        public ActionResult Index([Bind(Include = "Title,City,CategoryIds,ChosenSalary,Type,Condition,CompanyIds")] SearchBar searchBar)
        {


            var filteredJobs = SearchHelper.ApplyFilters(db.JobAdvertisments.AsQueryable(), searchBar);

            var model = BuildAdvertismentModel(filteredJobs.ToList());
            model.SearchBar = searchBar; // Maintain the state of the search bar
            ViewBag.IsAuthenticated = User.Identity.IsAuthenticated;
            model.SearchBar.Category = db.Category.ToList();
            model.SearchBar.Company = db.Companies.ToList();
            if (model.SearchBar.CompanyIds == null) { model.SearchBar.CompanyIds = new List<int>(); }
            if (model.SearchBar.CategoryIds == null) model.SearchBar.CategoryIds = new List<string>();
            if (model.SearchBar.SalaryTo == 0) model.SearchBar.SalaryTo = model.JobAdvertisment.Any() ? model.JobAdvertisment.Max(m => m.SalaryTo) : 10;
            if (model.SearchBar.SalaryFrom == 0) model.SearchBar.SalaryFrom = model.JobAdvertisment.Any() ? model.JobAdvertisment.Min(m => m.SalaryFrom) : 0;

            return View(model);
        }

        private AdvertismentAndSearchModel BuildAdvertismentModel(List<JobAdvertisment> jobAdvertisment, int page = 1, int pageSize = 1)
        {
            if (jobAdvertisment == null)
            {
                jobAdvertisment = db.JobAdvertisments
                    .Where(job => job.ActiveTill > DateTime.Now)
                    .Include(j => j.Company)
                    .ToList();
            }
            var totalJobs = jobAdvertisment.Count;
            var paginatedJobs = jobAdvertisment
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return new AdvertismentAndSearchModel
            {
                JobAdvertisment = paginatedJobs,
                SearchBar = new SearchBar
                {
                    Category = db.Category.ToList(),
                    Company = db.Companies.ToList(),
                    SalaryFrom = jobAdvertisment.Any() ? jobAdvertisment.Min(m => m.SalaryFrom) : 0,
                    SalaryTo = jobAdvertisment.Any() ? jobAdvertisment.Max(m => m.SalaryTo) : 10,
                    CompanyIds = new List<int>(),
                    CategoryIds = new List<string>(),

                },

                        PaginationInfo = new PaginationInfo
                        {
                            CurrentPage = page,
                            TotalPages = (int)Math.Ceiling(totalJobs / (double)pageSize),
                            TotalItems = totalJobs
                        }
            };
        }

        public ActionResult SearchBar()
        {
            SearchBar model = new SearchBar
            {
                SalaryTo = db.JobAdvertisments.Max(m => m.SalaryTo),
                SalaryFrom = db.JobAdvertisments.Min(m => m.SalaryFrom),
                Category = db.Category.ToList(),
                Company = db.Companies.ToList(),
                CompanyIds = new List<int>(),
                CategoryIds = new List<string>()
            };
            return PartialView("_PartialSearchBar", model);
        }
        // GET: JobAdvertisments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobAdvertisment jobAdvertisment = db.JobAdvertisments.Include(u => u.Company).Where(u => u.Id==id).First();
            if (jobAdvertisment == null)
            {
                return HttpNotFound();
            }
            ViewBag.isAuthenticated = User.Identity.IsAuthenticated;
            if (ViewBag.isAuthenticated && User.IsInRole("USER"))
            {
                ViewBag.Role = "USER";
                string userId = User.Identity.GetUserId();
                var person = db.Users.Include(u => u.CustomUser).First(u => u.Id == userId);
                int personId = person.CustomUser.Id;

                if (db.JobApplications.OfType<SignedUserApplication>().Where(m => m.jobAdvertisment.Id == jobAdvertisment.Id && m.ForeignKeyPerson == personId).FirstOrDefault() != null)
                {
                    ViewBag.Applied = true;
                }
            }else if(ViewBag.isAuthenticated && User.IsInRole("COMPANY"))
            {
                string userId = User.Identity.GetUserId();
                var company = db.Users.Include(u => u.CustomUser).First(u => u.Id == userId);
                if (company.CustomUser.Id == jobAdvertisment.Company.Id)
                {
                    ViewBag.IsSameCompany = true;
                }
                else
                {
                    ViewBag.IsSameCompany=false;
                }
            }
            else
            {
                ViewBag.Role = "";
            }
            return View(jobAdvertisment);
        }
        [Authorize(Roles="COMPANY")]
        public ActionResult ApplicantDetails(int id, int page = 1, int pageSize = 1)
        {
            // Get the total number of applications for the given advertisement
            var totalApplications = db.JobApplications.Count(a => a.jobAdvertisment.Id == id);

            // Retrieve paginated applications for the advertisement
            var applications = db.JobApplications
                .Where(a => a.jobAdvertisment.Id == id)
                .OrderBy(a => a.Id) // Or any other sorting criteria
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create a ViewModel to hold the data
            var viewModel = new AdvertisementApplicationsViewModel
            {
                AdvertismentId = id,
                Applications = applications,
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalApplications / (double)pageSize),
                    TotalItems = totalApplications
                }
            };

            return PartialView("_PartialJobApplicationsDetails", viewModel);
        }



        public ActionResult PartialDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobAdvertisment jobAdvertisment = db.JobAdvertisments.Where(m => m.Id==id).Include(u => u.Company).First();
            if (jobAdvertisment == null)
            {
                return HttpNotFound();
            }
            ViewBag.isAuthenticated = User.Identity.IsAuthenticated;
            if (ViewBag.isAuthenticated && User.IsInRole("USER"))
            {
                ViewBag.Role = "USER";
                string userId = User.Identity.GetUserId();
                var person = db.Users.Include(u => u.CustomUser).First(u => u.Id==userId);
                int personId=person.CustomUser.Id;

                if (db.JobApplications.OfType<SignedUserApplication>().Where(m => m.jobAdvertisment.Id == jobAdvertisment.Id && m.ForeignKeyPerson == personId).FirstOrDefault() != null)
                {
                    ViewBag.Applied = true;
                }
            }
            else
            {
                ViewBag.Role = "";
            }
            return PartialView("_PartialDetails",jobAdvertisment);
        }
        // GET: JobAdvertisments/Create

        [Authorize(Roles="COMPANY")]
        public ActionResult Create()
        {
            var model = new JobAdvertismentViewModel();
            model.Category=new Category();

            var sanitizer = new HtmlSanitizer();
            model.Description = sanitizer.Sanitize(model.Description);

            var userId = User.Identity.GetUserId();

            // Use your DbContext to fetch the ApplicationUser

            var user = db.Users
                .Include(u => u.CustomUser).SingleOrDefault(u => u.Id == userId);
            model.CompanyId = user.CustomUser.Id;
                // Now you have the ApplicationUser object
            model.ActiveTill = DateTime.Now.AddDays(1);
            model.Category.Name = "";
            return View(model);
        }

        // POST: JobAdvertisments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "COMPANY")]
        [ValidateInput(false)]

        public ActionResult Create([Bind(Include = "Id,Title,Description,ActiveTill,City,Category,SalaryFrom,SalaryTo,Type,Condition,CompanyId")] JobAdvertismentViewModel jobAdvertismentViewModel)
        {
            var sanitize =new HtmlSanitizer();
            jobAdvertismentViewModel.Description=sanitize.Sanitize(jobAdvertismentViewModel.Description);
            if (ModelState.IsValid)
            {
                JobAdvertisment jobAdvertisment = new JobAdvertisment
                {
                    Company = db.Companies.SingleOrDefault(c => c.Id == jobAdvertismentViewModel.CompanyId),
                    Created = DateTime.Now,
                    Title=jobAdvertismentViewModel.Title,
                    Description =jobAdvertismentViewModel.Description,
                    ActiveTill = jobAdvertismentViewModel.ActiveTill,
                    City=jobAdvertismentViewModel.City,
                   SalaryFrom=jobAdvertismentViewModel.SalaryFrom,
                   SalaryTo=jobAdvertismentViewModel.SalaryTo,
                   Type=jobAdvertismentViewModel.Type,
                   Condition=jobAdvertismentViewModel.Condition,
                };
                var existingCategory = db.Category.SingleOrDefault(c => c.Name == jobAdvertismentViewModel.Category.Name);

                if (existingCategory != null)
                {
                    // Use the existing category
                    jobAdvertisment.Category = existingCategory;
                }
                else
                {
                    // Add the new category to the database
                    db.Category.Add(jobAdvertismentViewModel.Category);
                    jobAdvertisment.Category = jobAdvertismentViewModel.Category;
                }
                db.JobAdvertisments.Add(jobAdvertisment);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobAdvertismentViewModel);
        }

        // GET: JobAdvertisments/Edit/5
        [Authorize(Roles="COMPANY")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobAdvertisment jobAdvertisment = db.JobAdvertisments.Include(job => job.Category).Include(job => job.Company).Where(u => u.Id==id).First();
            if (jobAdvertisment == null)
            {
                return HttpNotFound();
            }
            return View(jobAdvertisment);
        }

        // POST: JobAdvertisments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,ActiveTill,Category,City,SalaryFrom,SalaryTo,Created,Type,Condition,Description,Company")]  JobAdvertisment jobAdvertisment)
        {
            var sanitize = new HtmlSanitizer();
            jobAdvertisment.Description = sanitize.Sanitize(jobAdvertisment.Description);
            if (ModelState.IsValid)
            {

                var existingJob = db.JobAdvertisments
                                    .Include(j => j.Category)
                                    .Include(j => j.Company)
                                    .FirstOrDefault(j => j.Id == jobAdvertisment.Id);

                if (existingJob == null)
                {
                    return HttpNotFound();
                }

                // Update scalar properties
                existingJob.Title = jobAdvertisment.Title;
                existingJob.ActiveTill = jobAdvertisment.ActiveTill;
                existingJob.City = jobAdvertisment.City;
                existingJob.SalaryFrom = jobAdvertisment.SalaryFrom;
                existingJob.SalaryTo = jobAdvertisment.SalaryTo;
                existingJob.Type = jobAdvertisment.Type;
                existingJob.Condition = jobAdvertisment.Condition;
                existingJob.Description = jobAdvertisment.Description;

                // Update the Category if it has changed
                if (jobAdvertisment.Category != null && !string.IsNullOrEmpty(jobAdvertisment.Category.Name))
                {
                    var newCategory = db.Category.FirstOrDefault(c => c.Name == jobAdvertisment.Category.Name);

                    if (newCategory != null)
                    {
                        existingJob.Category = newCategory;
                    }
                    else
                    {
                        // Optionally handle cases where the category does not exist
                        ModelState.AddModelError("Category", "The specified category does not exist.");
                        return View(jobAdvertisment);
                    }
                }

                db.Entry(existingJob).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(jobAdvertisment);
        }

        // GET: JobAdvertisments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobAdvertisment jobAdvertisment = db.JobAdvertisments.Find(id);
            if (jobAdvertisment == null)
            {
                return HttpNotFound();
            }
            return View(jobAdvertisment);
        }

        // POST: JobAdvertisments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="COMPANY")]
        public ActionResult DeleteConfirmed(int id)
        {
            JobAdvertisment jobAdvertisment = db.JobAdvertisments.Find(id);
            db.JobAdvertisments.Remove(jobAdvertisment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
