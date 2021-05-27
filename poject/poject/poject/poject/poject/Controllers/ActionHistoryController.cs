using poject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace poject.Controllers
{
    public class ActionHistoryController : Controller
    {
        // GET: ActionHistory
        public ActionResult Index()
        {
            using (var db = new ECSalesEntities2())
            {

                ViewBag.list = db.productHistories.Include("TblCategory").ToList();
            }
             return View();
        }
    }
}