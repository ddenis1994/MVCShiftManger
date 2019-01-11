using finalProject.Dal;
using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using finalProject.ModelBinder;

namespace finalProject.Controllers
{
    public class HomeController : Controller
    {
        private MainDal dal = new MainDal();
        [RequireHttps]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AutorizeUser([ModelBinder(typeof(LoginModelBinder))] User obj)
        {
            if ((obj.username!=null) && obj.password!=null)
            {
                List<User> users =
                    (from x in dal.users
                     where x.username.Equals(obj.username) && x.password.Equals(obj.password)
                     select x).ToList<User>();
                if (users.Count == 0)
                {
                    obj.logInErorMassege = "wrong user name or password";
                    return View("signin", obj);
                }
                else
                {
                    Session["userId"] = users[0].userId;
                    Session["username"] = obj.username;
                    Session["id"] = users[0].Id;
                    Session["FirstName"] = users[0].FirstName;
                    Session["LastName"] = users[0].LastName;
                    Session["jobTitle"] = users[0].jobTitle;
                    Session["startWork"] = users[0].startWork.ToString();
                    Session["birtday"] = users[0].birtday.ToString();
                    Session["salary"] = users[0].salary;


                    int ss = users[0].userId;
                    List<roles> r =
                    (from x in dal.roles
                     where x.userid.Equals(ss)
                     select x).ToList<roles>();

                    if (r.Count > 0)
                    {
                        Session["role"] = r[0].role;
                        if (r[0].role == "admin")
                            return RedirectToAction("Index", "Admin");
                    }
                    else
                        Session["role"] = "";

                    return RedirectToAction("Index", "Worker");
                }
            }
            else {
                obj = new User();
                obj.logInErorMassege = "cannot pass argument";
                return View("signin", obj);
                    }
        }

        public ActionResult workerZone()
        {
             if (Session["id"] != null) {
                if (Session["role"].ToString() == "admin")
                    return RedirectToAction("Index", "Admin");
                return RedirectToAction("Index", "Worker");
            }
            User obj = new User();
            return View("signin",obj);
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
    }
}