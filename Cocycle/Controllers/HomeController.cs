using Cocycle.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cocycle.Controllers
{
    public class HomeController : Controller
    {
       
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //public ActionResult Gallery()
        //{
        //    ViewBag.Message = "Your Gallery page.";

        //    return View();
        //}
        public ActionResult MyProfile()
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

            var userid = User.Identity.GetUserId();
            ViewBag.Message = "Your Profile page.";
            ApplicationUser user = (from p in db.Users
                                          where p.Id == userid
                                          select p).SingleOrDefault();
            return View(user);
            
        }
    }
}