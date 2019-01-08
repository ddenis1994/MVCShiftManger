using finalProject.Dal;
using finalProject.ModelBinder;
using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace finalProject.Controllers
{


    public class WorkerController : Controller

    {

        public class cois
        {
            public string selected { get; set; }
        }

        [HttpPost]
        public ActionResult getWorkerShifts (string selected)
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
                List<shifts.shift> l=
                    (from x in k.Shifts
                     where x.shiftsId.Equals(s) 
                     select x).ToList<shifts.shift>();

                string data = "<table class=\"table table-hover table - striped\">";
                string temp="<tr>";
                for (int i = 0; i < l.Count; i++)
                {
                    DateTime dateTemp = DateTime.Parse(l[i].date);
                    temp = temp + "<td>" + dateTemp.ToString("dd/MM/yyyy") + "</td>";
                }
                data += temp+"</tr>";
                temp = "<tr>";
                for (int i = 0; i < l.Count; i++)
                    temp = temp + "<td>" + l[i].shiftChose + "</td>";
                data += temp + "</tr></table>";
                return Content(data);

            }

            return Content("cannot find any shifts for this week");

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
            string data="";
            for (int i = 0; i < result.Count; i++)
            {
                string temp = "<option value=\"" + result[i].week + "\">"+ result[i].week+ "</option>";
                data += temp;
            }
            string lable = "<label> select week:  </label >";
            string mainString = lable+ "<select id=\"selectdWeek\"  > " + data +"</select>";

            return Content(mainString);
        }

        public ActionResult Index()
        {
            User obj2 = new User();
            return View(obj2);
        }
        [HttpPost]
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

            return RedirectToAction("Index"); 
        }
        public ActionResult getTopManu()
        {
            return View("loginManu");
        }
        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}