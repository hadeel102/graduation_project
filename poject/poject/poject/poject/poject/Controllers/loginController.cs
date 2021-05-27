using poject.Model;
using poject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace poject.Controllers
{
    public class loginController : Controller
    {
        // GET: login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Users oUser)
        {
            using (var db = new ECSalesEntities2())
            {
                var DataInfo = db.TblUsers.Where(x => x.UserName == oUser.Username && x.Password == oUser.Password).FirstOrDefault();
                if (DataInfo != null)
                {
                    Session["UserInfo"] = DataInfo;
                    Session["IsAdmin"] = (DataInfo.ID == 0);
                    return Redirect("/Dashboard/Index");
                }
                else
                {
                    var AdminUserName = System.Configuration.ConfigurationManager.AppSettings["AdminUserName"] + "";
                    var AdminPassword = System.Configuration.ConfigurationManager.AppSettings["AdminPassword"] + "";

                    if (oUser.Username == AdminUserName && oUser.Password == AdminPassword)
                    {
                        Session["UserInfo"] = new TblUser()
                        {
                            ID = 0,
                            Name = AdminUserName,
                            UserName = AdminUserName
                        };
                        Session["IsAdmin"] = true;
                        return Redirect("/Dashboard/Index");
                    }
                    ViewBag.ErrorMessage = "The Username od Password is incorrect";
                    return View("Index");
                }
            }

        }

        public ActionResult Logout() {

            Session["UserInfo"] = null;
            Session["IsAdmin"] = null;
            
        return Redirect("/Dashboard"); ;
        }
    }
}