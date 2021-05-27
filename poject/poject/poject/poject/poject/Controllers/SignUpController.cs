using poject.Model;
using poject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace poject.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Users oUser)
        {
            using (var db = new ECSalesEntities2())
            {
                var DataInfo = db.TblUsers.Where(x => x.UserName == oUser.Username).Count();
                if (DataInfo <= 0)
                {
                    var oData = new TblUser
                    {
                        UserName = oUser.Username,
                        Address = oUser.Address,
                        CreationDate = DateTime.Now,
                        Email = oUser.Email,
                        MobileNumber = oUser.MobileNumber,
                        Name = oUser.Name,
                        Password = oUser.Password
                    };
                    db.TblUsers.Add(oData);
                    db.SaveChanges();
                    Session["UserInfo"] = oData;

                    return Redirect("/dashboard");
                }
                else
                {
                    ViewBag.ErrorMessage = "The Username od Password is incorrect";
                    return View("Index");
                }
            }
        }
    }
}