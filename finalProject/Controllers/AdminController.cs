using finalProject.Dal;
using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using finalProject.ModelBinder;
using System.Web.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace finalProject.Controllers
{
    public class AdminController : Controller
    {
        private MainDal dal = new MainDal();
        
        //finshid aget all working personal
        [ChildActionOnly]
        public ActionResult GetAllWorker()
        {
            if (Session["role"]!=null)
            {
                //find all working personal
                List<User> result = (from x in dal.users
                                     where x.EndWork.Equals(null)
                                     select x).ToList<User>();

                return PartialView("_workersPage", result);

            }
            return Content("");
        }
        public async Task<ActionResult> DeleteCandidate(string id)
        {
            await Task.Run(() => {

                if (Session["role"] != null)
                {
                    int idCandidate = int.Parse(id);
                    List<Candidate> found2 = (
                    from x in dal.Candidates
                    where x.candidateId.Equals(idCandidate)
                    select x).ToList<Candidate>();
                    dal.Candidates.Remove(found2[0]);
                    dal.SaveChanges();
                }
            });
            return Content("_index");
        }
        [HttpPost]
        public async Task<ActionResult> updateCandidate(string id,string newStatus)
        {
            await Task.Run(() =>
            {
                if (Session["role"] != null)
                {
                    int idCandidate = int.Parse(id);
                    List<Candidate> found2 = (
                    from x in dal.Candidates
                    where x.candidateId.Equals(idCandidate)
                    select x).ToList<Candidate>();

                    found2[0].status = newStatus;
                    dal.SaveChanges();
                }
            });
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> HireCandidate(string id,string password,string salary, string jobTitle)
        {
            await Task.Run(() =>
            {
                if (Session["role"] != null)
                {


                    int idCandidate = int.Parse(id);
                    List<Candidate> found2 = (
                    from x in dal.Candidates
                    where x.candidateId.Equals(idCandidate)
                    select x).ToList<Candidate>();
                    if (found2.Count > 0)
                    {
                        if (password == "")
                            password = found2[0].candidateId.ToString();
                        if (jobTitle == "")
                            jobTitle = "regulaer";
                        float tempSalary;
                        if (salary == "")
                            tempSalary = float.Parse("0");
                        else
                            tempSalary = float.Parse(salary);
                        User temp = new User()
                        {
                            userId = found2[0].candidateId,
                            birtday = found2[0].Birtday,
                            FirstName = found2[0].firstName,
                            password = password,
                            startWork = DateTime.Today,
                            LastName = found2[0].lastName,
                            gander = found2[0].gander,
                            jobTitle = jobTitle,
                            salary = tempSalary
                        };

                        dal.users.Add(temp);
                        dal.SaveChanges();
                        dal.Candidates.Remove(found2[0]);
                        dal.SaveChanges();
                    }
                }

            });
            return View();
        }

        [ChildActionOnly]
        public ActionResult getmyProfile()
        {
            if (Session["role"] != null)
            {
                int id = int.Parse(Session["id"].ToString());
                List<User> result =
               (from x in dal.users
                where x.Id.Equals(id)
                select x).ToList<User>();
                return PartialView("_profile", result[0]);
            }
            return Content("");

        }

        [ChildActionOnly]
        public ActionResult getAllCadidates()
        {
            if (Session["role"] != null)
            {
                List<Candidate> result =
               (from x in dal.Candidates
                select x).ToList<Candidate>();
                return PartialView("_CandidatePage", result);
            }
            return Content("");
        }
        [HttpPost]
        public ActionResult submitShifts([ModelBinder(typeof(ShiftBinder))] shifts obj)
        {

            if (Session["id"] != null)
            {
                //chackif got this shift
                if (ModelState.IsValid)
                {
                    List<shifts> result =
                    (from x in dal.WeekShifts
                     where x.startDate.Equals(obj.startDate) && x.userId.Equals(obj.userId)
                     select x).ToList<shifts>();


                    if (result.Count == 0)
                    {
                        dal.WeekShifts.Add(obj);
                        dal.SaveChanges();
                    }
                    else
                    {
                        User obj2 = new User();
                        obj2.logInErorMassege = "alredy submited for the week";
                        return View("Index", obj2);
                    }

                }
            }

            return View("Index");
        }
        public ActionResult getdates()
        {

            //first need to get all dates
            int id = (int)Session["userId"];
            List<shifts> result =
                (from x in dal.WeekShifts
                 where x.userId.Equals(id)
                 select x).ToList<shifts>();
            string data = "";
            for (int i = 0; i < result.Count; i++)
            {
                string temp = "<option value=\"" + result[i].week + "\">" + result[i].week + "</option>";
                data += temp;
            }
            string lable = "<label> select week:  </label >";
            string mainString = lable + "<select id=\"selectdWeek\"  > " + data + "</select>";

            return Content(mainString);
        }
        public ActionResult getWorkerShifts(string selected)
        {

            int id = (int)Session["userId"];
            int s = int.Parse(selected);
            List<shifts> result =
                (from x in dal.WeekShifts
                 where x.userId.Equals(id) && x.week.Equals(s)
                 select x).ToList<shifts>();
            

            if (result.Count > 0)
            {
                s = result[0].shiftsId;
                List<shifts.shift> l =
                    (from x in dal.Shifts1
                     where x.shiftsId.Equals(s)
                     select x).ToList<shifts.shift>();
                
                string data = "<table class=\"table table-hover table - striped\">";
                string temp = "<tr>";
                for (int i = 0; i < l.Count; i++)
                {
                    DateTime dateTemp = DateTime.Parse(l[i].date);
                    temp = temp + "<td>" + dateTemp.ToString("dd/MM/yyyy") + "</td>";
                }
                data += temp + "</tr>";
                temp = "<tr>";
                for (int i = 0; i < l.Count; i++)
                    temp = temp + "<td>" + l[i].shiftChose + "</td>";
                data += temp + "</tr></table>";
                return Content(data);

            }

            return Content("cannot find any shifts for this week");

        }
        public ActionResult getAllShiftsDates()
        {


            List<int> result =
                (from x in dal.WeekShifts
                 select x.week).ToList<int>();
            int maxWeek = result.Max();

            string data = "<label> week num:" + maxWeek + "  </label ><br>";

            return Content(data);
        }
        public ActionResult Index()
        {
            if (Session["role"] == null || (string)Session["role"]!="admin")
                return RedirectToAction("index", "Home"); 
            User obj = new User();
            return View(obj);
        }
        public ActionResult getOptions(string weeknum)
        {



            //finds next sunday for the query
            int start = (int)new DateTime().DayOfWeek;
            int target = (int)DayOfWeek.Sunday;
            if (target <= start)
                target += 7;
            DateTime nextS = new DateTime().AddDays(target - start);


            //first find the currect week id with the user id
            List<tempData> result =
                (from x in dal.WeekShifts
                 where x.week.Equals(3)
                 select new tempData{
                     weekId =(int)x.shiftsId,
                     userId =(int)x.userId }
                 ).ToList<tempData>();
            string data = "<form><table class=\"table table-hover table - striped\"><tr>" +
                "<td>name</td>" +
                "<td>Sunday</td>" +
                "<td>Monday</td>" +
                "<td>Tuesday</td>" +
                "<td>Wensday</td>" +
                "<td>Thursday</td>" +
                "<td>Friday</td>" +
                "<td>Saturday</td>" +
                "</tr> ";
            //loop all weeks
            foreach (tempData y in result)
            {

                string oneperosn = "<tr>";
                List<string> username =
                (from x in dal.users
                 where x.userId.Equals(y.userId)
                 select x.FirstName+" "+x.LastName
                 ).ToList<string>();
                if (username.Count > 0)
                {
                    //add thr user name
                    oneperosn += "<td>"+username[0]+"</td>";
                }
                else return Content("problem with the user name");
                //add each shift
                List<string> shifts =
                (from x in dal.Shifts1
                 where x.shiftsId.Equals(y.weekId)
                 select x.shiftChose
                 ).ToList<string>();
                foreach (string shiftsnum in shifts)
                {
                    oneperosn += "<td>" + shiftsnum + "</td>";
                }
                oneperosn += "</tr>";
                data += oneperosn;
            }
            data += "</table></form>";
            return Content(data);
        }
       
   
    }
    public class tempData{
        public string username { get; set; }
        public int weekId { get; set; }
        public int userId { get; set; }
    }
    public class oneWeek
    {
        public int WorkerId { get; set; }
        public string name { get; set; }
        public string Sunday { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wensday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public int Saturday { get; set; }



    }
}