using finalProject.Dal;
using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using finalProject.Models;
using finalProject.ModelBinder;

namespace finalProject.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult submitShifts([ModelBinder(typeof(ShiftBinder))] shifts obj)
        {


            //chackif got this shift
            if (ModelState.IsValid)
            {
                ShiftDal dal = new ShiftDal();

                List<shifts> result =
                (from x in dal.shifts
                 where x.startDate.Equals(obj.startDate) && x.userId.Equals(obj.userId)
                 select x).ToList<shifts>();


                if (result.Count == 0)
                {
                    dal.shifts.Add(obj);
                    dal.SaveChanges();
                }
                else
                {
                    User obj2 = new User();
                    obj2.logInErorMassege = "alredy submited for the week";
                    return View("Index", obj2);
                }

            }

            return View("Index");
        }
        public ActionResult getdates()
        {

            //first need to get all dates

            ShiftDal dal = new ShiftDal();
            int id = (int)Session["userId"];
            List<shifts> result =
                (from x in dal.shifts
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

            ShiftDal dal = new ShiftDal();
            int id = (int)Session["userId"];
            int s = int.Parse(selected);
            List<shifts> result =
                (from x in dal.shifts
                 where x.userId.Equals(id) && x.week.Equals(s)
                 select x).ToList<shifts>();


            if (result.Count > 0)
            {
                shiftsDalcs k = new shiftsDalcs();
                s = result[0].shiftsId;
                List<shifts.shift> l =
                    (from x in k.Shifts
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

            ShiftDal dal = new ShiftDal();

            List<int> result =
                (from x in dal.shifts
                 select x.week).ToList<int>();
            int maxWeek = result.Max();

            string data = "<label> week num:" + maxWeek + "  </label ><br>";

            return Content(data);
        }
        public ActionResult Index()
        {
            if (Session["id"] == null || (string)Session["role"]!="admin")
                return RedirectToAction("index", "Home"); 
            User obj = new User();
            return View(obj);
        }
        public ActionResult getOptions(string weeknum)
        {
            //get all the dalls for the query 
            ShiftDal MainShiftDal = new ShiftDal();
            shiftsDalcs secundaryShiftDal = new shiftsDalcs();
            LogInDal userDal = new LogInDal();


            //finds next sunday for the query
            int start = (int)new DateTime().DayOfWeek;
            int target = (int)DayOfWeek.Sunday;
            if (target <= start)
                target += 7;
            DateTime nextS = new DateTime().AddDays(target - start);


            //first find the currect week id with the user id
            List<tempData> result =
                (from x in MainShiftDal.shifts
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
                (from x in userDal.users
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
                (from x in secundaryShiftDal.Shifts
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
            string buttonTosubmit = "<button class=\"btn btn-lg btn - primary btn - block\" type=\"submit\">submit changes</button>";
            data += buttonTosubmit;
            return Content(data);
        }
       
        
    }
    public class tempData{
        public string username { get; set; }
        public int weekId { get; set; }
        public int userId { get; set; }
    }
}