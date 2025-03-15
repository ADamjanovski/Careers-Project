using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ganss.Xss;
using Microsoft.AspNet.Identity;
using PersonalProject.Models;

namespace PersonalProject.Controllers
{
    public class CompaniesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Companies
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Include(u => u.jobAdvertisments).Where(comp => comp.Id==id).First();
            if (company == null)
            {
                return HttpNotFound();
            }
            company.jobAdvertisments=company.jobAdvertisments.Where( m=> m.ActiveTill>DateTime.Now).ToList();

            ViewBag.isAuthenticated = User.Identity.IsAuthenticated;
            if (ViewBag.isAuthenticated && User.IsInRole("COMPANY"))
            {
                string userId = User.Identity.GetUserId();
                var userCompany = db.Users.Include(u => u.CustomUser).First(u => u.Id == userId);
                if (userCompany.CustomUser.Id == id)
                {
                    ViewBag.IsSameCompany = true;
                }
                else
                {
                    ViewBag.IsSameCompany = false;
                }
            }

            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,LogoURL,Address,Name,LinkedIn")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize(Roles ="COMPANY")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "COMPANY")]
        [ValidateInput(false)]

        public ActionResult Edit([Bind(Include = "Id,Description,LogoURL,Address,Name,LinkedIn")] Company company)
        {
            if (ModelState.IsValid)
            {
                var sanitize = new HtmlSanitizer();
                company.Description = sanitize.Sanitize(company.Description);
                var file = Request.Files["Logo"];
                if(file!=null && file.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(company.LogoURL))
                    {
                        var oldLogoPath = Path.Combine(Server.MapPath("~/Content/uploads"),
                            Path.GetFileName(company.LogoURL));
                        if (System.IO.File.Exists(oldLogoPath))
                        {
                            System.IO.File.Delete(oldLogoPath);
                        }
                    }
                    string fileName = Path.GetFileName(file.FileName);
                    string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                    string path = Path.Combine(Server.MapPath("~/Content/uploads"), uniqueFileName);

                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    file.SaveAs(path);
                    company.LogoURL = uniqueFileName;

                }
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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
