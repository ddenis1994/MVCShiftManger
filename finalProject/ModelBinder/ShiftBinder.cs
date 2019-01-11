using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace finalProject.ModelBinder
{
    public class ShiftBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objcontex = controllerContext.HttpContext;
            string date = objcontex.Request.Form["date"];
            int userId = 0;
            if (objcontex.Request.Form["userId"] != "" && objcontex.Request.Form["userId"] != null) 
                 userId = int.Parse(objcontex.Request.Form["userId"]);

            string su = objcontex.Request.Form["Sunday"];
            string mo = objcontex.Request.Form["Monday"];
            string tu = objcontex.Request.Form["Tuesday"];
            string we = objcontex.Request.Form["Wensday"];
            string th = objcontex.Request.Form["Thursday"];
            string fr = objcontex.Request.Form["Friday"];
            string sa = objcontex.Request.Form["Saturday"];
            if (date == "")
                date = DateTime.Today.ToString();
            DateTime parsedDate = DateTime.Parse(date);
            int start = (int)parsedDate.DayOfWeek;
            int target = (int)DayOfWeek.Sunday;
            if (target <= start)
                target += 7;
            DateTime nextS = parsedDate.AddDays(target - start);


            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            DateTime LastDay = nextS;
            int weeknum = myCal.GetWeekOfYear(LastDay, myCWR, myFirstDOW);


            List<shifts.shift> shiftlist = new List<shifts.shift>();
            shiftlist.Add(new shifts.shift() { date = (nextS.AddDays(0)).ToString(), shiftChose = su });
            shiftlist.Add(new shifts.shift() { date = (nextS.AddDays(1)).ToString(), shiftChose = mo });
            shiftlist.Add(new shifts.shift() { date = (nextS.AddDays(2)).ToString(), shiftChose = tu });
            shiftlist.Add(new shifts.shift() { date = (nextS.AddDays(3)).ToString(), shiftChose = we });
            shiftlist.Add(new shifts.shift() { date = (nextS.AddDays(4)).ToString(), shiftChose = th });
            shiftlist.Add(new shifts.shift() { date = (nextS.AddDays(5)).ToString(), shiftChose = fr });
            shiftlist.Add(new shifts.shift() { date = (nextS.AddDays(6)).ToString(), shiftChose = sa });
            shifts obj = new shifts()
            {
                startDate = nextS,
                year = nextS.Year,
                userId = userId,
                week= weeknum,
                shiftList=shiftlist
            };
            return obj;
        }
    }
}