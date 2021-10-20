using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cocycle.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text;
using System.Runtime.Serialization.Json;

namespace Cocycle.Controllers
{
    public class RoutesController : Controller
    {
      
         ApplicationDbContext db = new ApplicationDbContext();

      

        // GET: Routes
        public ActionResult Index()
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.loggedinuser = User.Identity.GetUserId();
            //    ViewBag.RouteSchedules = db.RouteSchedules.Include(r => r.RouteId);
            var routes = db.Routes.Where(x => x.IsApproved == true).Include(x => x.RouteSchedule).ToList();
          
            return View(routes);
        }


        public ActionResult MyRoutes()
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.loggedinuser = User.Identity.GetUserId();

            var userid = User.Identity.GetUserId();
            return View(db.Routes.Where(x => x.IsApproved == true  && x.CreatedBy== userid).Include(x => x.RouteSchedule).ToList());
        }

        public ActionResult ViewRequest()
        {
           ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.loggedinuser = User.Identity.GetUserId();
            var lists = db.Routes.Where(x => x.IsApproved == false).Include(x => x.RouteSchedule).ToList();
            return View(lists);
        }

        [HttpGet]
        public ActionResult GetByArea(int areaid)
        {
            List<Routes> routes = new List<Routes>();
            routes = db.Routes.Where(x=>x.AreaId==areaid && x.IsApproved==true).Include(x => x.RouteSchedule).ToList();
            return Json(routes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchBy(int? stateid, int? areaid)
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.loggedinuser = User.Identity.GetUserId();

            //var routelist ;
            List<Routes> routelist = new List<Routes>();
            
            if (areaid > 0)
                {
                    routelist = db.Routes.Where(x => x.StateId == stateid && x.AreaId == areaid && x.IsApproved == true).Include(x => x.RouteSchedule).ToList();
                }
                else if (stateid > 0)
                {
                    routelist = db.Routes.Where(x => x.StateId != 0 && x.StateId == stateid && x.IsApproved == true).Include(x => x.RouteSchedule).ToList();
                }
                else
                {
                    routelist = db.Routes.Where(x => x.IsApproved == true).Include(x => x.RouteSchedule).ToList();
                }
            return View("Index",routelist);
        }
        [HttpPost]
        public ActionResult RequestSearchBy(int? stateid, int? areaid)
        {
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.loggedinuser = User.Identity.GetUserId();

            List<Routes> routelist = new List<Routes>();

                if (areaid > 0)
                {
                    routelist = db.Routes.Where(x => x.StateId == stateid && x.AreaId == areaid && x.IsApproved == false).Include(x => x.RouteSchedule).ToList();
                }
                else if (stateid > 0)
                {
                    routelist = db.Routes.Where(x => x.StateId != 0 && x.StateId == stateid && x.IsApproved == false).Include(x => x.RouteSchedule).ToList();
                }
                else
                {
                    routelist = db.Routes.Where(x => x.IsApproved == false).Include(x => x.RouteSchedule).ToList();
                }

            return View("ViewRequest", routelist);
        }

        [HttpGet]
        public ActionResult AcceptRequest(string requestby, DateTime requestdate, int RouteId)
        {
           
            if (ModelState.IsValid)
            {
                Routes route = (from r in db.Routes
                                 where r.Id== RouteId
                                select r).SingleOrDefault();

                route.CreatedBy = requestby;
                route.Created = requestdate;
                route.IsApproved = true;
                route.hidInput = "route";
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Index");
        }

        //[HttpPost]
        //public ActionResult Index(FileUpload upload, HttpPostedFileBase file)
        //{
        //    if (file.ContentLength > 0)
        //    {
        //        var fileName = Path.GetFileName(file.FileName);
        //        var guid = Guid.NewGuid().ToString();
        //        var path = Path.Combine(Server.MapPath("~/uploads"), guid + fileName);
        //        file.SaveAs(path);
        //        string fl = path.Substring(path.LastIndexOf("\\"));
        //        string[] split = fl.Split('\\');
        //        string newpath = split[1];
        //        string imagepath = "~/uploads/" + newpath;
        //        upload.length = imagepath;
        //        db.FileUploads.Add(upload);
        //        db.SaveChanges();
        //    }
        //    TempData["Success"] = "Upload successful";
        //    return RedirectToAction("Index");
        //}



        // GET: Routes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = new Routes();
            //db.Routes.Find(id);
            ViewBag.States = db.States.ToList();
            ViewBag.Areas = db.Areas.ToList();
            ViewBag.Users = db.Users.ToList();
            routes = db.Routes.Include(x => x.RouteSchedule).Include(x=>x.FeedBack).Where(x => x.Id == id).FirstOrDefault();
            if (routes == null)
            {
                return HttpNotFound();
            }
            return View(routes);
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
        // GET: Routes/Create
        public ActionResult Create()
        {
            fill_dropdown();



            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RouteType,PosterImage,PostCode,StartingPoint,EndPoint,StartTime,EndTime,AreaId,StateId,Message,CreatedBy,Created,RequestedBy,RequestedDate,IsApproved,AddReturn,hidInput,PostCodeId,Description")] Routes routes)
        {
            HttpPostedFileBase file = Request.Files["PosterImage"];
            if (file.FileName!="")
            {
                var fileName = Path.GetFileName(file.FileName);
                var guid = Guid.NewGuid().ToString();
                var path = Path.Combine(Server.MapPath("~/Content/Images"), guid + fileName);
                file.SaveAs(path);
                routes.PosterImage = "../Content/Images/" + guid + fileName;
            }
            var currentUser = User.Identity.GetUserId();
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault();
            if (roles.Value == "cyclist")
            {
                routes.CreatedBy = currentUser;
                routes.Created = DateTime.Now;
                routes.IsActive = true;
                routes.IsApproved = true;
                if (ModelState.IsValid)
                {
                    db.Routes.Add(routes);
                    db.SaveChanges();
                    var schedule = routes.hidInput;
                    var schedules = schedule.Replace(@"\", string.Empty);
                    var data = JsonConvert.DeserializeObject<List<RouteSchedule>>(schedule);
                    foreach (var obj in data)
                    { 
                        obj.RouteId= routes.Id;
                    }
                        foreach (var obj in data)
                    {
                        db.RouteSchedules.Add(obj);
                    }
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    return RedirectToAction("MyProfile", "Account",new { tabid= "my_route-tab" });
                }
            }
            if (roles.Value == "learner")
            {
                routes.RequestedBy = currentUser;
                routes.RequestedDate = DateTime.Now;
                routes.IsApproved = false;
                routes.IsActive = true;
                if (ModelState.IsValid)
                {
                    db.Routes.Add(routes);
                    db.SaveChanges();
                    var schedule = routes.hidInput;
                    var schedules = schedule.Replace(@"\", string.Empty);
                    var data = JsonConvert.DeserializeObject<List<RouteSchedule>>(schedule);
                    foreach (var obj in data)
                    {
                        obj.RouteId = routes.Id;
                    }
                    foreach (var obj in data)
                    {
                        db.RouteSchedules.Add(obj);
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                   // return RedirectToAction("MyProfile", "Account", new { tabid = "my_route-tab" });
                }
            }
            if (roles.Value == "admin")
            {
                routes.CreatedBy = currentUser;
                routes.Created = DateTime.Now;
                routes.IsApproved = true;
                routes.IsActive = true;
                if (ModelState.IsValid)
                {
                    db.Routes.Add(routes);
                    db.SaveChanges();
                    var schedule = routes.hidInput;
                    var schedules = schedule.Replace(@"\", string.Empty);
                    var data = JsonConvert.DeserializeObject<List<RouteSchedule>>(schedule);
                    foreach (var obj in data)
                    {
                        obj.RouteId = routes.Id;
                    }
                    foreach (var obj in data)
                    {
                        db.RouteSchedules.Add(obj);
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            fill_dropdown();
            return View(routes);
        }

        // GET: Routes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = db.Routes.Find(id);
            if (routes == null)
            {
                return HttpNotFound();
            }
            fill_dropdown();
            return View(routes);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RouteType,PosterImage,PostCode,StartingPoint,EndPoint,StartTime,EndTime,AreaId,StateId,Message,CreatedBy,Created,RequestedBy,RequestedDate,IsApproved")] Routes routes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(routes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            fill_dropdown();
            return View(routes);
        }

        // GET: Routes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = db.Routes.Find(id);
            if (routes == null)
            {
                return HttpNotFound();
            }
            return View(routes);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Routes routes = db.Routes.Find(id);
            db.Routes.Remove(routes);
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
