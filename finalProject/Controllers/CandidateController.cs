using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using finalProject.Dal;
using finalProject.ModelBinder;
using Newtonsoft.Json;

namespace finalProject.Controllers
{
    public class CandidateController : Controller
    {
        private MainDal dal = new MainDal();

        [HttpGet]
        //action for retiving all the available jobs
        public ActionResult getCandidateJobes()
        {
            var job = (from x in dal.jobs
                       select x).ToList<Job>();
            return Content(JsonConvert.SerializeObject(job));
        }
        //main mction for return candidate index
        public ActionResult getCandidatePage()
        {
            return View("_index");
        }
        //func for retriving the register form
        public ActionResult Register()
        {
            Candidate can = new Candidate();
            return View("_RegisterForm", can);
        }
        [HttpPost]
        //func for adding candidate
        public ActionResult submit([ModelBinder(typeof(CandidateBinder))]Candidate obj)
        {
            if (ModelState.IsValid)
            {
                //first chack id dandidate is in the system
                List<Candidate> result= (from x in dal.Candidates
                                         where x.candidateId.Equals(obj.candidateId)
                                         select x).ToList<Candidate>();
                //first chack to find if there are dnadidates with the same name
                if (result.Count == 0)
                {
                    obj.status = "new";
                    dal.Candidates.Add(obj);
                    dal.SaveChanges();
                }
                else
                {
                    ViewBag.registerForm = "alredy has this id plz wait";
                    return View("_RegisterForm");
                }

            }
            //if all is good
            return RedirectToAction("index", "Home"); ;
        }
        //func for chaching candidate status
        public ActionResult ChackStatus()
        {
            return View("_ChackStatus");
        }
        [HttpPost]
        //finshid action for geting the view with candidate status
        public ActionResult getStatus()
        {
            //firest get the candidate string
            string statusString = Request.Form["txtIdEnter"];
            //only parse to int if the candidate sand id
            if (statusString != "")
            {
                if (ModelState.IsValid)
                {
                    List<Candidate> result= 
                    (from x in dal.Candidates
                    where x.candidateId.Equals(statusString)
                    select x).ToList<Candidate>();

                    if (result.Count > 0)
                        ViewBag.status = "your status is : " +result[0].status;
                }
            }
            else
                ViewBag.status = "not exsists in data base";
            return View("_ChackStatus");
        }
    }
}