using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace finalProject.ModelBinder
{
    public class CandidateBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContex = controllerContext.HttpContext;
            int idstring = int.Parse(objContex.Request.Form["txtIdEnter"]);
            string firstNameString = objContex.Request.Form["txtFirstName"];
            string lastNameStrin = objContex.Request.Form["txtLastName"];
            string ganderstring = objContex.Request.Form["txtGander"];
            DateTime birthDayString = new DateTime();
            if (objContex.Request.Form["txtBirthDay"] != "")
            {
                birthDayString = DateTime.Parse(objContex.Request.Form["txtBirthDay"]);
            }
            string emailString = objContex.Request.Form["txtEmail"];
            string jobString = objContex.Request.Form["txtJobTitle"];

            Candidate obj = new Candidate()
            {
                candidateId = idstring,
                firstName = firstNameString,
                lastName = lastNameStrin,
                gander = char.Parse(ganderstring),
                Birtday = DateTime.Parse(birthDayString.Day+"/"+birthDayString.Month+"/"+birthDayString.Year),
                email = emailString,
                jobTitle = jobString

            };
            return obj;


        }
    }
}