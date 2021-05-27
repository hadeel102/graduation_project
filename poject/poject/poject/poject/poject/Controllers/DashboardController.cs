using poject.Model;
using poject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace poject.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {

            using (var db = new ECSalesEntities2())
            {
                var olstcategoryies = db.TblCategories.ToList();
                ViewBag.Category = olstcategoryies;
            }

            return View();
        }
        [HttpGet]
        public PartialViewResult getCategories(string Classification = "All")
        {
            var olstCategories = new List<vmCategories>();
            var olstCategor = new List<TblCategory>();

            using (
                var db = new ECSalesEntities2())
            {
                var Query = db.TblCategories.Where(x => x.ParentID == null);
                switch (Classification)
                {
                    case "Product":
                        Query = Query.Where(x => x.IsService == false);

                        break;
                    case "Service":
                        Query = Query.Where(x => x.IsService == true);
                        break;
                    default:
                        break;
                }
                olstCategor = Query.ToList();

                olstCategor.ForEach(
              x =>
              {

                  var P = new vmCategories()
                  {
                      ID = x.ID,
                      ParentID = x.ParentID,
                      Active = x.Active,
                      AName = x.AName,
                      Ename = x.Ename,
                      Image = x.Image,
                      IsService = x.IsService,
                      Category = db.TblCategories.Where(y => y.ParentID == x.ID).ToList()
                  };

                  olstCategories.Add(P);
              }
              );
            }
            return PartialView("vwCategories", olstCategories);
        }

        [HttpPost]
        public PartialViewResult getProductsByCriteria(List<FilterCategory> data)
        {
            List<int> olstData = data.Select(x => x.id).ToList();
            List<product> OLSTproduct = new List<product>();
            using (
                 var db = new ECSalesEntities2())
            {
                OLSTproduct = db.products.Where(x => olstData.Any(y => y == x.CategoryID) && x.status == 1).ToList();
            }
            return PartialView("vwProducts", OLSTproduct);
        }
        [HttpPost]
        public PartialViewResult getAllProducts()
        {

            List<product> OLSTproduct = new List<product>();
            using (
                 var db = new ECSalesEntities2())
            {
                OLSTproduct = db.products.Where(x => x.status == 1).ToList();
            }
            return PartialView("vwProducts", OLSTproduct);
        }
    }

    public class FilterCategory
    {
        public int id { get; set; }
        public string isService { get; set; }
    }
}
