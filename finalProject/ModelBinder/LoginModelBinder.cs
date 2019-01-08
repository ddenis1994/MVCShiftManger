using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace finalProject.ModelBinder
{
    public class LoginModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {


            HttpContextBase objcontex = controllerContext.HttpContext;
            string username = objcontex.Request.Form["inputIdMain"];
            string password = objcontex.Request.Form["inputPasswordMain"];

            User obj = new User()
            {
                username = username,
                password = password
            };
            return obj;
            
        }
    }
}