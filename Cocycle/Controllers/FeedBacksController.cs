using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cocycle.Models;
using Microsoft.AspNet.Identity;

namespace Cocycle.Controllers
{
    public class FeedBacksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FeedBacks
        public ActionResult Index()
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            return View(db.FeedBacks.ToList());

        }

        // GET: FeedBacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = db.FeedBacks.Find(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            return View(feedBack);
        }

        // GET: FeedBacks/Create
        public ActionResult Create(int id)
        {
           //int rideid = id;
            var feedback = new FeedBack { RideId = id };
            return View(feedback);
        }
        public ActionResult AddRideFeedback(int id)
        {
            //int rideid = id;
            var feedback = new FeedBack { RideId = id };
            return View(feedback);
        }
        public ActionResult AddRouteFeedback(int id)
        {
            //int rideid = id;
            var feedback = new FeedBack { RouteId = id };
            return View(feedback);
        }


        // POST: FeedBacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRideFeedback([Bind(Include = "RideId,UserId,description,Created,RideHappened")] FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                feedBack.Created = DateTime.Now;
                feedBack.UserId = userid;
                db.FeedBacks.Add(feedBack);
                Arranged a = (from x in db.Arranged where x.Id == feedBack.RideId select x).First();
                a.RideCompleted = true;
                db.SaveChanges();
                return RedirectToAction("MyProfile", "Account");
            }
            return View(feedBack);
        }


        // POST: FeedBacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRouteFeedback([Bind(Include = "RideId,RouteId,UserId,description,Created,RideHappened")] FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                feedBack.Created = DateTime.Now;
                feedBack.UserId = userid;
                db.FeedBacks.Add(feedBack);
                db.SaveChanges();
                return RedirectToAction("MyProfile", "Account");
            }
            return View(feedBack);
        }



        // POST: FeedBacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RideId,UserId,description,Created,RideHappened")] FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                feedBack.Created = DateTime.Now;
                feedBack.UserId = userid;
                db.FeedBacks.Add(feedBack);
                Arranged a = (from x in db.Arranged where x.Id ==feedBack.RideId select x).First();
                a.RideCompleted = feedBack.RideHappened;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            return View(feedBack);
        }

        // GET: FeedBacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = db.FeedBacks.Find(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            return View(feedBack);
        }

        // POST: FeedBacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RideId,UserId,description,Created")] FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedBack).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feedBack);
        }

        // GET: FeedBacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = db.FeedBacks.Find(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            return View(feedBack);
        }

        // POST: FeedBacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FeedBack feedBack = db.FeedBacks.Find(id);
            db.FeedBacks.Remove(feedBack);
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
