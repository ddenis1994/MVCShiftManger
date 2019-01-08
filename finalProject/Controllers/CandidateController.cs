using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using finalProject.Dal;
using finalProject.ModelBinder;

namespace finalProject.Controllers
{
    public class CandidateController : Controller
    {
        public ActionResult getCandidatePage()
        {
            return View("CandidatePage");
        }
        public ActionResult Register()
        {
            Candidate can = new Candidate();
            can.Id = null;
            return View("RegisterForm", can);
        }
        public ActionResult submit([ModelBinder(typeof(CandidateBinder))]Candidate obj)
        {

            if (ModelState.IsValid)
            {
                CandidateDal dal = new CandidateDal();
                if (dal.Candidates.Find(obj.Id) == null)
                {
                    obj.status = "new";
                    dal.Candidates.Add(obj);
                    dal.SaveChanges();
                }
                else
                {
                    return View("RegisterForm", obj);
                }

            }

            return RedirectToAction("index", "Home"); ;
        }
        public ActionResult ChackStatus()
        {
            ViewBag.denis = "deni";
            return View("ChackStatus");
        }
        public ActionResult getStatus()
        {
            if (ModelState.IsValid)
            {
                CandidateDal dal = new CandidateDal();
                if (dal.Candidates.Find(Request.Form["txtIdEnter"]) != null)
                {
                    ViewBag.status = "your status is :" + dal.Candidates.Find(Request.Form["txtIdEnter"]).status;
                    //todo find more sutibul way to pass the data corractrly
                }
                else
                    ViewBag.status = "not exsists in data base";
            }

            return View("ChackStatus");
        }
    }
}