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
    [Authorize]
    public class ArrangedsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
      
        // GET: Arrangeds
        public ActionResult Index()
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.loggedinuser = User.Identity.GetUserId();
            return View(db.Arranged.Where(x => x.IsScheduled==false).ToList());
        }

      
        public ActionResult IndexLearner()
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.loggedinuser = User.Identity.GetUserId();
            string userid = User.Identity.GetUserId();
            ViewBag.feedbacks = db.FeedBacks.Where(x=>x.UserId==userid).ToList(); 
            return View(db.Arranged.Where(x=>x.RequestBy== userid).Include(x => x.FeedBack).ToList());
        }
        public ActionResult IndexCyclist()
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.loggedinuser = User.Identity.GetUserId();
            string userid = User.Identity.GetUserId();
            ViewBag.feedbacks = db.FeedBacks.Where(x => x.UserId == userid).ToList();
            return View(db.Arranged.Where(x => x.ApprovedBy == userid).Include(x=>x.FeedBack).ToList());
        }

        public ActionResult OfferHelp(int Id)
        {

            if (ModelState.IsValid)
            {
                Arranged arranged = (from r in db.Arranged
                                     where r.Id == Id
                                     select r).SingleOrDefault();

                arranged.ApproveDate = DateTime.Now;
                arranged.ApprovedBy = User.Identity.GetUserId();
                arranged.IsApproved = true;
                db.SaveChanges();
                return RedirectToAction("MyProfile", "Account",new { tabid= "arranged-tab" });
                //return RedirectToAction("IndexCyclist", "Arrangeds");
            }
            return RedirectToAction("MyProfile", "Account", new { tabid = "arranged-tab" });
        }
        
        public ActionResult ScheduleDate(int Id,DateTime startdate,string ApproveRemark)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                Arranged arranged = (from r in db.Arranged
                                     where r.Id == Id
                                     select r).SingleOrDefault();

                //var schedules = offeringdates.Replace(@"\","");
                //arranged.OfferingDates = schedules;
                arranged.ApproveRemark = ApproveRemark;
                arranged.StartTime = startdate;
                arranged.IsApproved = true;
                arranged.ApprovedBy = userid;
                arranged.IsScheduled = true;
                var users = db.Users.Where(s => s.Id == arranged.RequestBy).FirstOrDefault();
                db.SaveChanges();
                //trigger mail
             //   SendMail(users.Email, Id,arranged.applicationUserApproved.Email);
                return Json(arranged, JsonRequestBehavior.AllowGet);
               
            }
            return RedirectToAction("MyProfile","Account", new { tabid = "arranged-tab" });
        }
        public void SendMail(string email, int rideid,string approvalemail)
        {

            var emailaccounts = db.EmailAccount.FirstOrDefault();
            var message = new MailMessage();
            message.From = new MailAddress(emailaccounts.Email);
            message.To.Add(new MailAddress(email));
            message.Subject = "Cocycle Help Offered for ride";
            message.Body = "Dear User,<br> your request for ride No." + rideid + " has been accepted by the user  "+ approvalemail;
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

        public ActionResult Confirm(int Id, DateTime confirmdate)
        {

            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                Arranged arranged = (from r in db.Arranged
                                     where r.Id == Id
                                     select r).SingleOrDefault();
                arranged.StartTime = confirmdate;
                //arranged.EndTime = EndDate;
                arranged.IsScheduled = true;
                db.SaveChanges();
                var users = db.Users.Where(s => s.Id == arranged.ApprovedBy).FirstOrDefault();
              //  SendconfirmMail(users.Email, Id);
                return Json(arranged, JsonRequestBehavior.AllowGet);
                // return RedirectToAction("IndexLearner", "Arrangeds");
                // return RedirectToAction("IndexLearner");
            }
            return RedirectToAction("MyProfile", "Account");
        }

        public void SendconfirmMail(string email, int rideid)
        {
            var emailaccounts = db.EmailAccount.FirstOrDefault();
            var message = new MailMessage();
            message.From = new MailAddress(emailaccounts.Email);
            message.To.Add(new MailAddress(email));
            message.Subject = "Cocycle ride date confirmed";
            message.Body = "Dear User,<br> the ride No." + rideid + " has been scheduled ";
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

        // GET: Arrangeds/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.Routes = db.Routes.ToList();
            ViewBag.loggedinuser = User.Identity.GetUserId();
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arranged arranged = db.Arranged.Find(id);
            ViewBag.CyclistFeedback = db.FeedBacks.Where(x => x.RideId == id && x.UserId == arranged.ApprovedBy).FirstOrDefault();
            ViewBag.LearnerFeedback = db.FeedBacks.Where(x => x.RideId == id && x.UserId == arranged.RequestBy).FirstOrDefault();
            if (arranged == null)
            {
                return HttpNotFound();
            }
            return View(arranged);
        }

        public void fill_dropdown()
        {
            List<StateList> liststate = new List<StateList>();
            var lststate = db.States.ToList();
            foreach (var s in lststate)
            {
                StateList stateList = new StateList
                {
                    StateId = Convert.ToInt32(s.Id),
                    StateName = s.StateName
                };
                liststate.Add(stateList);
            }
            ViewBag.States = liststate;
            List<Area> Areas = new List<Area>();
            Areas = (from c in db.Areas select c).ToList();
            ViewBag.Areas = Areas;

            List<PostCode> postCodes = new List<PostCode>();
            postCodes = (from c in db.postCodes select c).ToList();
            ViewBag.postCodes = postCodes;
        }

        [Authorize(Roles = "learner")]
        // GET: Arrangeds/Create
        public ActionResult Create()
        {

            fill_dropdown();

            return View();
        }
        public ActionResult ScheduleDates(int Id)
        {
            ViewBag.ArrangeId = Id;
            var offerdates = db.Arranged.Where(x => x.Id == Id).FirstOrDefault();
            ViewBag.offerdates = offerdates;
            return View("ScheduleDate",offerdates);
        }

        public ActionResult ViewSchedule(int Id)
        {
            //ApplicationDbContext dbContext = new ApplicationDbContext();
            ViewBag.ArrangeId = Id;
            var a = db.Arranged.Where(x => x.Id == Id).FirstOrDefault();
            return View(a);
        }
        public ActionResult EditScheduleDates(int Id)
        {
            //ApplicationDbContext dbContext = new ApplicationDbContext();
            ViewBag.ArrangeId = Id;
            var a = db.Arranged.Where(x => x.Id == Id).FirstOrDefault();
            return View(a);
        }
        
        // POST: Arrangeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles="learner")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RouteId,RequestBy,ApprovedBy,StartPoint,EndPoint,StartTime,EndTime,RequestRemark,ApproveRemark,RequestDate,ApproveDate,StateId,AreaId,IsApproved,PostCodeId,RequestingDates,OfferingDates")] Arranged arranged)
        {
            if (ModelState.IsValid)
            {
                arranged.RequestBy = User.Identity.GetUserId();
                arranged.RequestDate = DateTime.Now;
                arranged.IsApproved = false;
                arranged.IsScheduled = false;
                db.Arranged.Add(arranged);
                db.SaveChanges();
                return RedirectToAction("MyProfile", "Account", new { tabid = "requestrides-tab" });
                //return RedirectToAction("IndexLearner");
            }
            fill_dropdown();
            return View(arranged);
        }

        // GET: Arrangeds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arranged arranged = db.Arranged.Find(id);
            if (arranged == null)
            {
                return HttpNotFound();
            }
            fill_dropdown();
            return View(arranged);
        }

        // POST: Arrangeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RouteId,RequestBy,ApprovedBy,StartPoint,EndPoint,StartTime,EndTime,RequestRemark,ApproveRemark,RequestDate,ApproveDate,StateId,AreaId")] Arranged arranged)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arranged).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            fill_dropdown();
            return View(arranged);
        }

        // GET: Arrangeds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arranged arranged = db.Arranged.Find(id);
            if (arranged == null)
            {
                return HttpNotFound();
            }
            return View(arranged);
        }

        // POST: Arrangeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Arranged arranged = db.Arranged.Find(id);
            db.Arranged.Remove(arranged);
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
