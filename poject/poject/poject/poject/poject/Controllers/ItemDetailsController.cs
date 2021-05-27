using poject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace poject.Controllers
{
    public class ItemDetailsController : Controller
    {
        // GET: ItemDetails
        public ActionResult Index()
        {
            var Id = int.Parse(  Request.QueryString["ID"] + "");
            using (var db = new ECSalesEntities2())
            {
                ViewBag.item = db.products.Include("TblUser").Where(x => x.ID == Id).FirstOrDefault();
            }
                return View();
        }
    }
}