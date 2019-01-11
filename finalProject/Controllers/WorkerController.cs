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
        private MainDal dal = new MainDal();
        [HttpPost]
        //method for creathing each worker shifts from history
        public ActionResult getWorkerShifts (string selected)
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
                List<shifts.shift> l=
                    (from x in dal.Shifts1
                     where x.shiftsId.Equals(s) 
                     select x).ToList<shifts.shift>();
                //creath the table for the user
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
        //actino to find worker weeks of wor
        [HttpPost]
        public ActionResult getdates()
        {

            //first need to get all dates

            int id = (int)Session["userId"];
            List<shifts> result =
                (from x in dal.WeekShifts
                 where x.userId.Equals(id)
                 select x).ToList<shifts>();
            string data="";
            for (int i = 0; i < result.Count; i++)
            {
                string temp = "<option value=\"" + result[i].week + "\">"+ result[i].week+ "</option>";
                data += temp;
            }
            string lable = "<span class=\"label label-default\">Select Week</span>";
            string mainString = lable+ "<select id=\"selectdWeek\" class=\"form - control\" > " + data +"</select>";

            return Content(mainString);
        }
        //adede chack if loged in
        public ActionResult Index()
        {
            if (Session["id"] != null)
            {
                User obj2 = new User();
                return View("_index", obj2);
            }
            return RedirectToAction("index", "home");
        }
        [HttpPost]
        //added control only if loged in
        public ActionResult submitShifts([ModelBinder(typeof(ShiftBinder))] shifts obj)
        {
            if (Session["id"] != null)
            {
                //chackif got this shift
                if (ModelState.IsValid)
                {

                    DateTime temp1 =DateTime.Parse ( obj.startDate.Value.ToString());
                    int temp2 = obj.userId;


                    List<shifts> result =
                    (from x in dal.WeekShifts
                     where x.startDate == temp1 && x.userId== temp2
                     select x).ToList<shifts>();


                    if (result.Count == 0)
                    {
                        dal.WeekShifts.Add(obj);
                        dal.SaveChanges();
                    }
                    else
                    {
                        ViewBag.eror = "alredy submited for the week";
                        return View("_index");
                    }
                }
                return View("_index");
            }
            return RedirectToAction("index", "home");
        }
        public ActionResult getTopManu()
        {
            return View("_loginManu");
        }
        //added chach if loged in
        public ActionResult logout()
        {
            if (Session["username"] != null)
            {
                Session.Clear();
            }
            return RedirectToAction("Index", "Home");

        }
    }
}