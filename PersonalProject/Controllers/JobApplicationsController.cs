using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PersonalProject.Models;
using PersonalProject.Models.Helpers;

namespace PersonalProject.Controllers
{
    public class JobApplicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: JobApplications
        public ActionResult Index()
        {
            return View(db.JobApplications.ToList());
        }

        // GET: JobApplications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobApplication jobApplication = db.JobApplications.Find(id);
            if (jobApplication == null)
            {
                return HttpNotFound();
            }
            return View(jobApplication);
        }

        // GET: JobApplications/Create
        [Authorize(Roles ="COMPANY")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "COMPANY")]

        public ActionResult Create([Bind(Include = "Id,CV")] JobApplication jobApplication)
        {
            if (ModelState.IsValid)
            {
                db.JobApplications.Add(jobApplication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobApplication);
        }

        // GET: JobApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobApplication jobApplication = db.JobApplications.Find(id);
            if (jobApplication == null)
            {
                return HttpNotFound();
            }
            return View(jobApplication);
        }
        [Authorize(Roles = "USER")]

        public ActionResult PartialJobApplicationSigned(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Retrieve the JobAdvertisment
            var jobAdvertisment = db.JobAdvertisments.Find(id);
            if (jobAdvertisment == null)
            {
                // Handle case where job advertisement is not found
                return HttpNotFound("Job Advertisement not found.");
            }
            var userId = User.Identity.GetUserId();
            var person = db.Users.Where(m => m.Id == userId).Include(m => m.CustomUser).First().CustomUser;
            SignedApplicationViewModel model = new SignedApplicationViewModel
            {
                JobAdvertismentId = id.Value,
                UserId = person.Id,

            };

            return PartialView("_PartialJobApplicationSigned", model);
        }
        private void SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = "your_email@example.com"; // Replace with your email
            var smtpClient = new SmtpClient("smtp.example.com") // Replace with your SMTP server
            {
                Port = 587,
                Credentials = new NetworkCredential("your_email@example.com", "your_password"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }
        [Authorize(Roles ="COMPANY")]
        public ActionResult DownloadCV(string fileName)
        {
            try
            {
                // Map the file path
                var filePath = Server.MapPath("~/App_Data/uploads/CV/" + fileName);

                // Check if the file exists
                if (!System.IO.File.Exists(filePath))
                {
                    return HttpNotFound("The requested file was not found.");
                }

                // Get the file bytes and MIME type
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var contentType = MimeMapping.GetMimeMapping(filePath);

                // Return the file
                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                return new HttpStatusCodeResult(500, "An error occurred while processing the file download.");
            }
        }

        [HttpPost]
        [Authorize(Roles ="COMPANY")]
        public ActionResult ChangeStatus(int? id, string status)
        {
            if (id == null || status=="")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var application = db.JobApplications.Find(id.Value);
                if (application == null)
                {
                    return Json(new { success = false, message = "Application not found." });
                }

                if (application is SignedUserApplication signedApplication)
                {
                    var user = db.Users.Where(u => u.CustomUser.Id == signedApplication.Person.Id).FirstOrDefault();

                    // Handle SignedUserApplication
                    if (status == "ACCEPTED")
                    {
                        signedApplication.JobStatus = JobApplication.Status.ACCEPTED;
               //         SendEmail(user.Email, "Your application has been accepted", "Congratulations! Your application has been accepted.");
                    }
                    else if (status == "REJECTED")
                    {
                        signedApplication.JobStatus = JobApplication.Status.REJECTED;
             //           SendEmail(user.Email, "Your application has been declined", "We regret to inform you that your application has been declined.");
                    }
                    else
                    {
                        return Json(new { success = false, message = "Invalid status." });
                    }
                }
                else if (application is UnsignedUserApplication unsignedApplication)
                {
                    // Handle UnsignedUserApplication
                    if (status == "ACCEPTED")
                    {
                        unsignedApplication.JobStatus = JobApplication.Status.ACCEPTED;
             //           SendEmail(unsignedApplication.Email, "Your application has been accepted", "Congratulations! Your application has been accepted.");
                    }
                    else if (status == "REJECTED")
                    {
                        unsignedApplication.JobStatus = JobApplication.Status.REJECTED;
        //                SendEmail(unsignedApplication.Email, "Your application has been declined", "We regret to inform you that your application has been declined.");
                    }
                    else
                    {
                        return Json(new { success = false, message = "Invalid status." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Unknown application type." });
                }

                db.SaveChanges();
                return Json(new { success = true, message = $"Application status changed to {status}." });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return Json(new { success = false, message = "An error occurred while updating the application status." });
            }
        }


        [HttpPost]
        [Authorize(Roles = "USER")]
        public ActionResult PartialJobApplicationSigned(SignedApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["CV"];

                if (model.UserId == -1)
                {
                    // Handle case where person is not found in the database
                    return HttpNotFound("Person not found.");
                }
                var uploadsFolder = Server.MapPath("/App_Data/uploads/CV");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.CV.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                file.SaveAs(filePath);
                SignedUserApplication signedUserApplication = new SignedUserApplication
                {
                    CV = uniqueFileName,
                    Person = (Person)db.CustomUsers.Where(m => m.Id == model.UserId).First(),
                    JobStatus = JobApplication.Status.PENDING,
                    jobAdvertisment=db.JobAdvertisments.Where(m => m.Id == model.JobAdvertismentId).First(),
                };
                db.JobApplications.Add(signedUserApplication);
                db.SaveChanges();
                return Json(new { success = true });

            }
            var partialViewHtml = RenderRazorViewToString("_PartialJobApplicationSigned", model);

            return Json(new { success = false, html = partialViewHtml });
        }

        [Authorize(Roles = "USER")]
        public ActionResult UserApplications(int? id, int page = 1, int pageSize = 1,string filter="")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var totalApplications = 0; // Declare totalApplications once
            List<SignedUserApplication> applications = null; // Declare applications once

            if (filter == "")
            {
                totalApplications = db.JobApplications
                    .OfType<SignedUserApplication>()
                    .Count(a => a.ForeignKeyPerson == id.Value);

                applications = db.JobApplications
                    .OfType<SignedUserApplication>()
                    .Include(a => a.jobAdvertisment)
                    .Include(a => a.jobAdvertisment.Company)
                    .Where(a => a.ForeignKeyPerson == id.Value)
                    .OrderBy(a => a.Id) // Or any other sorting criteria
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                totalApplications = db.JobApplications
                    .OfType<SignedUserApplication>() 
                    .Count(a => a.ForeignKeyPerson == id.Value && a.JobStatus.ToString() == filter);

                applications = db.JobApplications
                    .OfType<SignedUserApplication>()
                    .Include(a => a.jobAdvertisment)
                    .Include(a => a.jobAdvertisment.Company)
                    .Where(a => a.ForeignKeyPerson == id.Value && a.JobStatus.ToString()==filter)
                    .OrderBy(a => a.Id) // Or any other sorting criteria
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            // Get the total number of applications for the given advertisement


            // Create a ViewModel to hold the data
            var viewModel = new UserApplicationsViewModel
            {
                personId = id.Value,
                Applications = applications,
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalApplications / (double)pageSize),
                    TotalItems = totalApplications
                }
            };

            return PartialView("UserApplications", viewModel);
        }

        [AllowAnonymous]
        public ActionResult PartialJobAppUnSigned(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (User.Identity.IsAuthenticated) // Check if the user is logged in
            {
                // If the user is logged in, redirect them to a different page (e.g., home or job listings)
                return RedirectToAction("PartialJobApplicationSigned", "JobAdvertisments"); // Or any other redirect location
            }
            var jobAdvertisment = db.JobAdvertisments.Find(id);
            if (jobAdvertisment == null)
            {
                // Handle case where job advertisement is not found
                return HttpNotFound("Job Advertisement not found.");
            }
            UnsignedApplicationViewModel model = new UnsignedApplicationViewModel
            {
                JobAdvertismentId = id.Value,

            };

            return PartialView("_PartialJobAppUnSigned", model);
        }
        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;

            using (var stringWriter = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                return stringWriter.GetStringBuilder().ToString();
            }
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult PartialJobAppUnSigned(UnsignedApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return the partial view with validation errors
                var partialViewHtml = RenderRazorViewToString("_PartialJobAppUnSigned", model);

                return Json(new { success = false, html = partialViewHtml });
            }
            if(User.Identity.IsAuthenticated)
            {
                var partialViewHtml = RenderRazorViewToString("_PartialJobAppUnSigned", model);

                return Json(new { success = false, html = partialViewHtml });
            }
            try
            {
                // Process the uploaded CV file
                if (model.CV != null && model.CV.ContentLength > 0)
                {
                    var uploadsFolder = Server.MapPath("~/App_Data/uploads/CV");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(model.CV.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.CV.SaveAs(filePath);

                    // Create a new UnsignedUserApplication
                    var unsignedUserApplication = new UnsignedUserApplication
                    {
                        CV = uniqueFileName,
                        Email = model.Email,
                        LinkedIn = model.LinkedIn,
                        PhoneNumber = model.PhoneNumber,
                        Name = model.Name,
                        JobStatus = JobApplication.Status.PENDING,
                        jobAdvertisment = db.JobAdvertisments.Find(model.JobAdvertismentId)
                    };

                    db.JobApplications.Add(unsignedUserApplication);
                    db.SaveChanges();

                    // Return success response as JSON
                    return Json(new { success = true });
                }
                else
                {
                    ModelState.AddModelError("CV", "Please upload a valid CV file.");
                    return PartialView("PartialJobAppUnSigned", model);
                }
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                ModelState.AddModelError("", "An error occurred while processing your application. Please try again.");
                return PartialView("PartialJobAppUnSigned", model);
            }
        }

        // POST: JobApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CV")] JobApplication jobApplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobApplication);
        }

        // GET: JobApplications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobApplication jobApplication = db.JobApplications.Find(id);
            if (jobApplication == null)
            {
                return HttpNotFound();
            }
            return View(jobApplication);
        }

        // POST: JobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobApplication jobApplication = db.JobApplications.Find(id);
            db.JobApplications.Remove(jobApplication);
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
