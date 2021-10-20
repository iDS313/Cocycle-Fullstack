using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Cocycle.Models;
using Microsoft.AspNet.Identity;

namespace Cocycle.Controllers
{
    public class RouteGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RouteGroups
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.Routes = db.Routes.ToList();
            //return View(db.RouteGroup.ToList());
            return View(db.RouteGroup.Where(x=>x.routes.CreatedBy==userid).ToList());
        }
        public ActionResult MyGroups()
        {
            var userid = User.Identity.GetUserId();
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.Routes = db.Routes.ToList();
            //return View(db.RouteGroup.ToList());
            return View(db.RouteGroup.Where(x => x.RequestBy == userid).ToList());
        }


        public ActionResult MyRequest()
        {
            var userid = User.Identity.GetUserId();
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.Routes = db.Routes.ToList();
            //return View(db.RouteGroup.ToList());
            return View(db.RouteGroup.Where(x => x.RequestBy == userid).ToList());
        }
        public ActionResult ViewMembers(int routeid)
        {
            var userid = User.Identity.GetUserId();
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.Routes = db.Routes.ToList();
            //return View(db.RouteGroup.ToList());
            return View(db.RouteGroup.Where(x => x.RouteId == routeid).ToList());
        }


        // GET: RouteGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteGroup routeGroup = db.RouteGroup.Find(id);
            if (routeGroup == null)
            {
                return HttpNotFound();
            }
            return View(routeGroup);
        }

        // GET: RouteGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RouteGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RequestBy,ApproveBy,RequestDate,ApproveDate,IsApproved")] RouteGroup routeGroup)
        {
            if (ModelState.IsValid)
            {
                db.RouteGroup.Add(routeGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(routeGroup);
        }

        [Authorize]
         [HttpGet]
        public ActionResult RequestToJoin(string requestby,DateTime requestdate,int RouteId)
        {
            RouteGroup routeGroup = new RouteGroup();
            if (ModelState.IsValid)
            {
                routeGroup.RequestBy = requestby;
                routeGroup.RequestDate = requestdate;
                routeGroup.RouteId = RouteId;
                var IsExist = db.RouteGroup.Where(x => x.RouteId == RouteId && x.RequestBy == requestby).FirstOrDefault();
                if (IsExist != null)
                {
                    return RedirectToAction("MyProfile","Account");
                }
                db.RouteGroup.Add(routeGroup);
                db.SaveChanges();
                
                var userid = User.Identity.GetUserId();
                var users = db.Users.Where(s => s.Id == userid).FirstOrDefault();
                //SendMail(users.Email, RouteId);
                return RedirectToAction("MyProfile", "Account", new { tabid = "request-tab" });
            }
            return View(routeGroup);
        }


        public void SendMail(string email,int RouteId)
        {

            var emailaccounts = db.EmailAccount.FirstOrDefault();
            var message = new MailMessage();
            message.From = new MailAddress(emailaccounts.Email);
            message.To.Add(new MailAddress(email));
            message.Subject = "Cocycle Request For Joining Route Group";
            message.Body = "Dear User,<br> your request for joining route No." + RouteId + " has been sent ";
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                smtp.EnableSsl = emailaccounts.EnableSSL;
                smtp.UseDefaultCredentials = emailaccounts.UseDefaultCredentials;
                var credential = new NetworkCredential
                {
                    UserName = emailaccounts.Email,  // replace with valid value
                    Password = emailaccounts.Password // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = emailaccounts.Host;
                smtp.Port = Convert.ToInt32(emailaccounts.Port);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            //return View(model);
        }

        [Authorize(Roles = "cyclist")]
        public ActionResult AcceptRequest(string ApproveBy, DateTime Approvedate, int RouteGroupId)
        {
            RouteGroup routeGroup = new RouteGroup();
            if (ModelState.IsValid)
            {
                routeGroup = (from r in db.RouteGroup
                              where r.Id == RouteGroupId
                              select r).SingleOrDefault();
                routeGroup.ApproveBy= ApproveBy;
                routeGroup.ApproveDate = Approvedate;
                routeGroup.IsApproved = true;
                db.Entry(routeGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyProfile", "Account",new { tabid = "my_route-tab" });
            }
            return View(routeGroup);
        }


        // GET: RouteGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteGroup routeGroup = db.RouteGroup.Find(id);
            if (routeGroup == null)
            {
                return HttpNotFound();
            }
            return View(routeGroup);
        }

        // POST: RouteGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RequestBy,ApproveBy,RequestDate,ApproveDate,IsApproved")] RouteGroup routeGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(routeGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(routeGroup);
        }

        // GET: RouteGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RouteGroup routeGroup = db.RouteGroup.Find(id);
            if (routeGroup == null)
            {
                return HttpNotFound();
            }
            return View(routeGroup);
        }

        // POST: RouteGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RouteGroup routeGroup = db.RouteGroup.Find(id);
            db.RouteGroup.Remove(routeGroup);
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
